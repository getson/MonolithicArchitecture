using Autofac;
using BinaryOrigin.SeedWork.Core.Domain;
using BinaryOrigin.SeedWork.Core.Extensions;
using BinaryOrigin.SeedWork.Persistence.Ef;
using System.Linq;

namespace BinaryOrigin.SeedWork.Core
{
    public static class EngineExtensions
    {

        public static void AddDefaultPagination(this IEngine engine)
        {
            engine.Register(builder =>
            {
                builder.RegisterType<PaginationService>()
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
