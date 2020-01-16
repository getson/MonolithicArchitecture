using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BinaryOrigin.SeedWork.WebApi.ModelBinders
{
    public class RouteDataComplexTypeModelBinderProvider : IModelBinderProvider
    {
        private readonly IModelBinderProvider _complexTypeModelBinderProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RouteDataComplexTypeModelBinderProvider(IModelBinderProvider complexTypeModelBinderProvider, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _complexTypeModelBinderProvider = complexTypeModelBinderProvider;
        }

        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            var httpMethod = _httpContextAccessor.HttpContext.Request.Method;
            if ((httpMethod == HttpMethods.Get || httpMethod == HttpMethods.Delete) && !_httpContextAccessor.HttpContext.Request.QueryString.HasValue)
            {
                if (context.Metadata.IsComplexType && !context.Metadata.IsCollectionType)
                    return new RouteDataComplexTypeModelBinder(_complexTypeModelBinderProvider.GetBinder(context));
            }
            return null;
        }
    }
}