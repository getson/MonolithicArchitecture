using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Linq;

namespace App.WebApi.Extensions
{
    public static class SwaggerExtensions
    {
        public static void UseAppSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = string.Empty;
                options.SwaggerEndpoint($"/swagger/v1/swagger.json", "App");
            });
        }

        /// <summary>
        /// Configure Swagger
        /// </summary>
        /// <param name="services"></param>
        public static void AddAppSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My App", Version = "v1" });

                // add a custom operation filter which sets default values
                c.OperationFilter<SwaggerOperationFilter>();

                // Set the comments path for the Swagger JSON and UI.
                var basePath = AppContext.BaseDirectory;
                var xmlPath = Path.Combine(basePath, "App.WebApi.xml");
                c.IncludeXmlComments(xmlPath);
            });
            services.ConfigureSwaggerGen(cfg =>
            {
                cfg.DescribeAllParametersInCamelCase();
                cfg.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Authorization header using the Bearer scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                cfg.AddSecurityRequirement(new OpenApiSecurityRequirement());
            });
        }
    }

    public class SwaggerOperationFilter : IOperationFilter
    {
        /// <summary>
        /// Applies the filter to the specified operation using the given context.
        /// </summary>
        /// <param name="operation">The operation to apply the filter to.</param>
        /// <param name="context">The current operation filter context.</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                return;
            }

            // REF: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/412
            // REF: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/pull/413
            foreach (var parameter in operation.Parameters)
            {
                var description = context.ApiDescription.ParameterDescriptions.First(p => string.Compare(p.Name, parameter.Name, true) == 0);
                var routeInfo = description.RouteInfo;

                if (parameter.Description == null)
                {
                    parameter.Description = description.ModelMetadata?.Description;
                }

                if (routeInfo == null)
                {
                    continue;
                }

                //if (parameter..Default == null)
                //{
                //    parameter.Default = routeInfo.DefaultValue;
                //}

                parameter.Required |= !routeInfo.IsOptional;
            }
        }
    }
}