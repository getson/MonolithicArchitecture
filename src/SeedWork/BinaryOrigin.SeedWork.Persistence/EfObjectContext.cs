using BinaryOrigin.SeedWork.Core;
using BinaryOrigin.SeedWork.Core.Domain;
using BinaryOrigin.SeedWork.Core.Exceptions;
using EFSecondLevelCache.Core;
using EFSecondLevelCache.Core.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace BinaryOrigin.SeedWork.Persistence.Ef
{
    /// <summary>
    /// Represents base object context
    /// </summary>
    public class EfObjectContext : DbContext, IDbContext
    {
        public EfObjectContext(DbContextOptions<EfObjectContext> options)
            : base(options)
        {
        }

        public EfObjectContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        }

        /// <summary>
        /// Further configuration the model
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //dynamically load all entity and query type configurations

            var configurations = EngineContext.Current.FindClassesOfType<IMappingConfiguration>()
                                           .Where(t => !t.IsGenericType);

            foreach (var typeConfiguration in configurations)
            {
                var configuration = (IMappingConfiguration)Activator.CreateInstance(typeConfiguration);
                configuration.ApplyConfiguration(modelBuilder);
            }
        }

        /// <summary>
        /// Modify the input SQL query by adding passed parameters
        /// </summary>
        /// <param name="sql">The raw SQL query</param>
        /// <param name="parameters">The values to be assigned to parameters</param>
        /// <returns>Modified raw SQL query</returns>
        protected virtual string CreateSqlWithParameters(string sql, params object[] parameters)
        {
            //add parameters to sql
            for (var i = 0; i <= (parameters?.Length ?? 0) - 1; i++)
            {
                if (!(parameters[i] is DbParameter parameter))
                {
                    continue;
                }

                sql = $"{sql}{(i > 0 ? "," : string.Empty)} @{parameter.ParameterName}";

                //whether parameter is output
                if (parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Output)
                {
                    sql = $"{sql} output";
                }
            }

            return sql;
        }

        /// <summary>
        /// Creates a DbSet that can be used to query and save instances of entity
        /// </summary>
        /// <typeparam name="TEntity">data object type</typeparam>
        /// <returns>A set for the given entity type</returns>
        public new virtual DbSet<TEntity> Set<TEntity>()
            where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }

        public new async virtual Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var efCacheServiceProvider = EngineContext.Current.Resolve<IEFCacheServiceProvider>();
                if (efCacheServiceProvider == null)
                {
                    return await base.SaveChangesAsync(cancellationToken);
                }
                return await SaveChangesAndCleanCache(efCacheServiceProvider, cancellationToken);
            }
            catch (DbUpdateException ex)
            {
                // it will throw always
                await GetFullErrorTextAndRollbackEntityChanges(ex);
                throw;
            }
        }

        private async Task<int> SaveChangesAndCleanCache(IEFCacheServiceProvider efCacheServiceProvider, CancellationToken cancellationToken)
        {
            var changedEntityNames = this.GetChangedEntityNames()
                                .Where(x => !x.Contains("SeedWork"))
                                .ToArray();

            ChangeTracker.AutoDetectChangesEnabled = false; // for performance reasons, to avoid calling DetectChanges() again.

            var result = await base.SaveChangesAsync(cancellationToken);

            ChangeTracker.AutoDetectChangesEnabled = true;
            efCacheServiceProvider.InvalidateCacheDependencies(changedEntityNames);
            return result;
        }

        /// <summary>
        /// Generate a script to create all tables for the current model
        /// </summary>
        /// <returns>A SQL script</returns>
        public virtual string GenerateCreateScript()
        {
            return Database.GenerateCreateScript();
        }

        /// <inheritdoc />
        /// <summary>
        /// Create the database
        /// </summary>
        public bool CreateDatabase()
        {
            return Database.EnsureCreated();
        }

        public bool IsDatabaseUpdated()
        {
            return !Database.GetPendingMigrations().Any();
        }

        /// <inheritdoc />
        /// <summary>
        /// Update database based on migrations
        /// </summary>
        public void UpdateDatabase()
        {
            Database.Migrate();
        }

        /// <summary>
        /// Executes the given SQL against the database
        /// </summary>
        /// <param name="sql">The SQL to execute</param>
        /// <param name="doNotEnsureTransaction">true - the transaction creation is not ensured; false - the transaction creation is ensured.</param>
        /// <param name="timeout">The timeout to use for command. Note that the command timeout is distinct from the connection timeout, which is commonly set on the database connection string</param>
        /// <param name="parameters">Parameters to use with the SQL</param>
        /// <returns>The number of rows affected</returns>
        public async virtual Task<int> ExecuteSqlCommandAsync(string sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters)
        {
            //set specific command timeout
            var previousTimeout = Database.GetCommandTimeout();
            Database.SetCommandTimeout(timeout);

            var result = 0;
            if (!doNotEnsureTransaction && Transaction.Current == null)
            {
                //use with transaction
                using (var transaction = Database.BeginTransaction())
                {
                    result = await Database.ExecuteSqlRawAsync(sql, parameters);
                    transaction.Commit();
                }
            }
            else
            {
                result = await Database.ExecuteSqlRawAsync(sql, parameters);
            }

            //return previous timeout back
            Database.SetCommandTimeout(previousTimeout);

            return result;
        }

        public virtual int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters)
        {
            //set specific command timeout
            var previousTimeout = Database.GetCommandTimeout();
            Database.SetCommandTimeout(timeout);

            var result = 0;
            if (!doNotEnsureTransaction && Transaction.Current == null)
            {
                //use with transaction
                using (var transaction = Database.BeginTransaction())
                {
                    result = Database.ExecuteSqlRaw(sql, parameters);
                    transaction.Commit();
                }
            }
            else
            {
                result = Database.ExecuteSqlRaw(sql, parameters);
            }

            //return previous timeout back
            Database.SetCommandTimeout(previousTimeout);

            return result;
        }

        /// <summary>
        /// Detach an entity from the context
        /// </summary>
        /// <typeparam name="TEntity">Data object type</typeparam>
        /// <param name="entity">Entity</param>
        public virtual void Detach<TEntity>(TEntity entity)
            where TEntity : BaseEntity
        {
            if (entity == null)
            {
                throw new GeneralException(nameof(entity));
            }

            var entityEntry = Entry(entity);
            if (entityEntry == null)
            {
                return;
            }

            //set the entity is not being tracked by the context
            entityEntry.State = EntityState.Detached;
        }

        /// <summary>
        /// Rollback of entity changes and return full error message
        /// </summary>
        /// <param name="exception">AppException</param>
        /// <returns>Error message</returns>
        public async Task GetFullErrorTextAndRollbackEntityChanges(DbUpdateException exception)
        {
            //rollback entity changes
            if (this is DbContext dbContext)
            {
                try
                {
                    var entries = dbContext.ChangeTracker.Entries()
                        .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified).ToList();

                    entries.ForEach(entry => entry.State = EntityState.Unchanged);
                }
                catch (Exception ex)
                {
                    exception = new DbUpdateException(exception.ToString(), ex);
                }
            }
            // save previous state
            await base.SaveChangesAsync();
            var exceptionParser = EngineContext.Current.Resolve<IDbExceptionParserProvider>();
            if (exceptionParser != null)
            {
                throw new GeneralException(exceptionParser.Parse(exception));
            };
            throw exception;
        }
    }
}