using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace BinaryOrigin.SeedWork.WebApi.ModelBinders
{
    public class RouteDataComplexTypeModelBinder : IModelBinder
    {
        private readonly IModelBinder _complexTypeModelBinder;

        public RouteDataComplexTypeModelBinder(IModelBinder complexTypeModelBinder)
        {
            _complexTypeModelBinder = complexTypeModelBinder;
        }

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext.ActionContext.RouteData.Values.TryGetValue(bindingContext.ModelName, out var value))
                bindingContext.Result = ModelBindingResult.Success(value);
            else if (_complexTypeModelBinder != null)
                await _complexTypeModelBinder.BindModelAsync(bindingContext);
        }
    }

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
            if (_httpContextAccessor.HttpContext.Request.Method == HttpMethods.Get && !_httpContextAccessor.HttpContext.Request.QueryString.HasValue)
            {
                if (context.Metadata.IsComplexType && !context.Metadata.IsCollectionType)
                    return new RouteDataComplexTypeModelBinder(_complexTypeModelBinderProvider.GetBinder(context));
            }
            return null;
        }
    }
}