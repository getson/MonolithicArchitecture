using Autofac;
using BinaryOrigin.SeedWork.Core.Domain;
using BinaryOrigin.SeedWork.Persistence.Ef;
using System;
using System.Linq;

namespace BinaryOrigin.SeedWork.Core
{
    public static class EngineExtensions
    {

        public static void AddDefaultPagination(this IEngine engine, Action<PaginationOptions> option)
        {
            var paginationOptions = new PaginationOptions();
            option.Invoke(paginationOptions);

            engine.Register(builder =>
            {
                builder.Register(c => new PaginationService(c.Resolve<ITypeAdapter>(), paginationOptions))
                       .As<IPaginationService>()
                       .SingleInstance();
            });
        }
        public static void AddRepositories(this IEngine engine)
        {
            engine.Register(builder =>
            {
                var assembliesWithRepositories = engine.FindClassesOfType(typeof(IRepository<>))
                                                        .Where(x => !x.AssemblyQualifiedName.Contains("SeedWork"))
                                                        .Select(x => x.Assembly)
                                                        .DistinctBy(x => x.FullName)
                                                        .ToList();

                foreach (var asm in assembliesWithRepositories)
                {
                    builder.RegisterAssemblyTypes(asm)
                            .AsClosedTypesOf(typeof(IRepository<>))
                            .InstancePerLifetimeScope();
                }
            });
        }
    }
}
