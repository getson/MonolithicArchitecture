using BinaryOrigin.SeedWork.Core.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BinaryOrigin.SeedWork.Persistence.Ef
{
    /// <summary>
    /// Represents DB context
    /// </summary>
    public interface IDbContext
    {
        /// <summary>
        /// Creates a DbSet that can be used to query and save instances of entity
        /// </summary>
        /// <returns>A set for the given entity type</returns>
        DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity;

        /// <summary>
        /// Saves all changes made in this context to the database
        /// </summary>
        /// <returns>The number of state entries written to the database</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Generate a script to create all tables for the current model
        /// </summary>
        /// <returns>A SQL script</returns>
        string GenerateCreateScript();

        /// <summary>
        /// Create the database
        /// </summary>
        bool CreateDatabase();

        /// <summary>
        /// Update database based on migrations
        /// </summary>
        void UpdateDatabase();

        bool IsDatabaseUpdated();

        /// <summary>
        /// Creates a LINQ query for the query type based on a raw SQL query
        /// </summary>
        /// <typeparam name="TQuery">Query type</typeparam>
        /// <param name="sql">The raw SQL query</param>
        /// <returns>An IQueryable representing the raw SQL query</returns>
        IQueryable<TQuery> QueryFromSql<TQuery>(string sql) where TQuery : class, new();

        /// <summary>
        /// Creates a LINQ query for the entity based on a raw SQL query
        /// </summary>
        /// <param name="sql">The raw SQL query</param>
        /// <param name="parameters">The values to be assigned to parameters</param>
        /// <returns>An IQueryable representing the raw SQL query</returns>
        IQueryable<TEntity> EntityFromSql<TEntity>(string sql, params object[] parameters) where TEntity : BaseEntity;

        /// <summary>
        /// Executes the given SQL against the database
        /// </summary>
        /// <param name="sql">The SQL to execute</param>
        /// <param name="doNotEnsureTransaction">true - the transaction creation is not ensured; false - the transaction creation is ensured.</param>
        /// <param name="timeout">The timeout to use for command. Note that the command timeout is distinct from the connection timeout, which is commonly set on the database connection string</param>
        /// <param name="parameters">Parameters to use with the SQL</param>
        /// <returns>The number of rows affected</returns>
        int ExecuteSqlCommand(RawSqlString sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters);

        /// <summary>
        /// Detach an entity from the context
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="entity">Entity</param>
        void Detach<TEntity>(TEntity entity) where TEntity : BaseEntity;
    }
}