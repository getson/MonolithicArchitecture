using System;
using System.IO;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MyApp.Core.Abstractions.Infrastructure;
using MyApp.Core.Abstractions.Mapping;
using MyApp.Core.Abstractions.Validator;
using MyApp.Core.Common;
using MyApp.Core.Configuration;
using MyApp.Core.Infrastructure;
using MyApp.Core.SharedKernel.Validator;
using MyApp.Infrastructure.FileSystem;
using MyApp.Services.Logging;

namespace MyApp.Services.Tests
{
    public class TestsInitialize
    {
        public void ConfigureMapping()
        {
            //create AutoMapper configuration
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ExampleProfile());
            });
            AutoMapperConfiguration.Init(config);

        }
        public TestsInitialize()
        {
            var entityValidatorFactory = new Mock<IEntityValidatorFactory>();
            entityValidatorFactory.Setup(x => x.Create())
                                  .Returns(() => new DataAnnotationsEntityValidator());
            EntityValidatorFactory = entityValidatorFactory.Object;

            var loggerMock = new Mock<ILogger>();
            Logger = loggerMock.Object;

            var engine = new Mock<IEngine>();
            engine.Setup(x => x.Resolve<IEntityValidatorFactory>())
                .Returns(() => new DataAnnotationsEntityValidatorFactory());
            engine.Setup(x => x.Resolve<ITypeAdapterFactory>())
                .Returns(() => new AutoMapperTypeAdapterFactory());

            EngineContext.SetEngine(engine.Object);
            TypeAdapter = TypeAdapterFactory.Instance.CreateAdapter();
            ConfigureMapping();
        }
        protected ILogger Logger { get; }
        protected ITypeAdapter TypeAdapter { get; }

        protected IEntityValidatorFactory EntityValidatorFactory { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {

            //create, initialize and configure the engine
            var (rootPath, contentPath) = GetPaths(services);

            var myAppConfig = new MyAppConfig();
            services.AddSingleton(myAppConfig);

            //DefaultFileProvider.Instance = new MyAppFileProvider(rootPath, contentPath);
            var fileProvider = new Mock<IMyAppFileProvider>();
            //create, initialize and configure the engine
            var engine = EngineContext.Create();

            engine.Initialize(services, fileProvider.Object, myAppConfig);

            var serviceProvider = engine.ConfigureServices(services, myAppConfig);

            return serviceProvider;
        }
        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //EngineContext.Current.ConfigureRequestPipeline(app);

        }


        private static (string rootPath, string contentPath) GetPaths(IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var hostingEnvironment = provider.GetRequiredService<IHostingEnvironment>();
            var webRootPath = File.Exists(hostingEnvironment.WebRootPath) ? Path.GetDirectoryName(hostingEnvironment.WebRootPath) : hostingEnvironment.WebRootPath;

            return (webRootPath, hostingEnvironment.ContentRootPath);
        }



    }
}
