using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyApp.Core.Infrastructure;
using Swashbuckle.AspNetCore.Swagger;

namespace MyApp.WebApi.Infrastructure
{
    public static class ServiceColletionExtensions
    {
        /// <summary>
        /// Create, bind and register as service the specified configuration parameters 
        /// </summary>
        /// <typeparam name="TConfig">Configuration parameters</typeparam>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Set of key/value application configuration properties</param>
        /// <returns>Instance of configuration parameters</returns>
        public static TConfig AddConfiguration<TConfig>(this IServiceCollection services, IConfiguration configuration) where TConfig : class, new()
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            //create instance of config
            var config = new TConfig();

            //bind it to the appropriate section of configuration
            configuration.Bind(config);

            //and register it as a service
            services.AddSingleton(config);

            return config;
        }
        /// <summary>
        /// Add Versioning service
        /// </summary>
        /// <param name="services"></param>
        public static void AddMyApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.ApiVersionReader = new HeaderApiVersionReader("api-version");
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });
        }
        /// <summary>
        /// Configure Swagger
        /// </summary>
        /// <param name="services"></param>
        public static void AddMySwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {

                // resolve the IApiVersionDescriptionProvider service
                // note: that we have to build a temporary service provider here because one has not been created yet
                var apiVersionDescriptionProvider = EngineContext.Current.Resolve<IApiVersionDescriptionProvider>();

                // add a swagger document for each discovered API version
                // note: you might choose to skip or document deprecated API versions differently
                foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                {
                    c.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
                }

                // add a custom operation filter which sets default values
                c.OperationFilter<SwaggerDefaultValues>();

                // Set the comments path for the Swagger JSON and UI.
                var basePath = AppContext.BaseDirectory;
                var xmlPath = Path.Combine(basePath, "SPA.xml");
                c.IncludeXmlComments(xmlPath);
            });
        }

        #region Helper Methods

        private static Info CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new Info
            {
                Title = $"Demo API {description.ApiVersion}",
                Version = $"Api Version v{description.ApiVersion.ToString()}",
                Description = "This is a demo of my Enterprise App",
                Contact = new Contact
                {
                    Name = "Getson Cela",
                    Email = "cela.getson@gmail.com"
                },
                TermsOfService = "MyApp",
                License = new License
                {
                    Name = "Getson Cela"
                }
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }


        #endregion

    }
}
