using MyApp.Core.Interfaces.Mapping;

namespace MyApp.Infrastructure.Common.Adapter
{
    public class AutomapperTypeAdapterFactory : ITypeAdapterFactory
    {
        public ITypeAdapter Create()
        {
            return new AutomapperTypeAdapter();
        }

    }
}
