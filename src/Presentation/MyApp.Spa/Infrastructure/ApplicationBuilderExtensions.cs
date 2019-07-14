using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using MyApp.Core.Infrastructure;

namespace MyApp.WebApi.Infrastructure
{
    /// <summary>
    /// Configure Application services
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// configure Swagger
        /// </summary>
        /// <param name="app"></param>
        public static void UseMySwagger(this IApplicationBuilder app)
        {

            var apiVersionDescriptionProvider = EngineContext.Current.Resolve<IApiVersionDescriptionProvider>();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(options =>
            {
                // build a swagger endpoint for each discovered API version
                foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
            });
        }
    }
}
