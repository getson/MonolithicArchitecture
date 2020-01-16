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
}