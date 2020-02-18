using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;

namespace BinaryOrigin.SeedWork.WebApi.ModelBinders
{
    public class QueryModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            //TODO GETSON remove static string
            var @interface = context.Metadata
                                    .ModelType
                                    .GetInterfaces()
                                    .FirstOrDefault(i => i.Name.StartsWith("IQuery"));
            if (@interface != null)
            {
                return new QueryModelBinder();
            }
            return null;
        }
    }
}