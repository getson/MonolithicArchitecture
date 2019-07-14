using MyApp.Core.Abstractions.Mapping;

namespace MyApp.Core.Infrastructure
{
    /// <inheritdoc />
    /// <summary>
    /// AutoMapper type adapter implementation
    /// </summary>
    public class AutoMapperTypeAdapter : ITypeAdapter
    {
        #region ITypeAdapter Members

        public TTarget Adapt<TSource, TTarget>(TSource source) where TSource : class where TTarget : class, new()
        {
            return AutoMapperConfiguration.Mapper.Map<TSource, TTarget>(source);
        }

        public TTarget Adapt<TTarget>(object source) where TTarget : class, new()
        {
            return AutoMapperConfiguration.Mapper.Map<TTarget>(source);
        }

        #endregion
    }
}
