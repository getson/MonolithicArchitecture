using AutoMapper;
using BinaryOrigin.SeedWork.Core;

namespace BinaryOrigin.SeedWork.WebApi.Mapping
{
    /// <inheritdoc />
    /// <summary>
    /// AutoMapper type adapter implementation
    /// </summary>
    public class AutoMapperTypeAdapter : ITypeAdapter
    {
        private readonly IMapper _mapper;

        public AutoMapperTypeAdapter(IMapper mapper)
        {
            _mapper = mapper;
        }

        public TTarget Adapt<TSource, TTarget>(TSource source)
            where TSource : class
            where TTarget : class
        {
            return _mapper.Map<TSource, TTarget>(source);
        }

        public TTarget Adapt<TTarget>(object source)
            where TTarget : class
        {
            return _mapper.Map<TTarget>(source);
        }
    }
}