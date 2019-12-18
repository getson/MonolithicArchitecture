using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BinaryOrigin.SeedWork.WebApi.ModelBinders
{
    public class QueryModelBinder : IModelBinder
    {
        public QueryModelBinder()
        {
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            if (bindingContext.HttpContext.Request.QueryString.HasValue)
            {
                var queryParameters = bindingContext.HttpContext
                                                    .Request
                                                    .Query
                                                    .ToDictionary(pair => pair.Key, pair => pair.Value.ToString());

                var jsonString = JsonConvert.SerializeObject(queryParameters);
                var parameterType = ((ControllerContext)bindingContext.ActionContext)
                                    .ActionDescriptor.Parameters.FirstOrDefault()?
                                    .ParameterType;

                var obj = JsonConvert.DeserializeObject(jsonString, parameterType);
                bindingContext.Model = obj;
            }

            bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);

            return Task.CompletedTask;
        }
    }
}