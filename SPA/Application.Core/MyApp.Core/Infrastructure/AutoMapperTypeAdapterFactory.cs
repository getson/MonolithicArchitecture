using MyApp.Core.Abstractions.Mapping;

namespace MyApp.Core.Infrastructure
{
    public class AutoMapperTypeAdapterFactory : ITypeAdapterFactory
    {
        public ITypeAdapter Create()
        {
            return new AutoMapperTypeAdapter();
        }

    }
}
