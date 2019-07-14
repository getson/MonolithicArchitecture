using System.Collections.Generic;
using MyApp.Core.Abstractions.Mapping;
using MyApp.Core.SharedKernel.Domain;

namespace MyApp.Core.Extensions
{

    public static class ProjectionsExtensionMethods
    {
        /// <summary>
        /// Project a type using a DTO
        /// </summary>
        /// <typeparam name="TProjection">The dto projection</typeparam>
        /// <param name="entity">The source entity to project</param>
        /// <returns>The projected type</returns>
        public static TProjection ProjectedAs<TProjection>(this IProjectableEntity entity)
            where TProjection : class, new()
        {
            var adapter = TypeAdapterFactory.Instance.CreateAdapter();
            return adapter.Adapt<TProjection>(entity);
        }

        /// <summary>
        /// projected a enumerable collection of items
        /// </summary>
        /// <typeparam name="TProjection">The dtop projection type</typeparam>
        /// <param name="items">the collection of entity items</param>
        /// <returns>Projected collection</returns>
        public static List<TProjection> ProjectedAsCollection<TProjection>(this IEnumerable<IProjectableEntity> items)
            where TProjection : class, new()
        {
            var adapter = TypeAdapterFactory.Instance.CreateAdapter();
            return adapter.Adapt<List<TProjection>>(items);
        }
    }
}
