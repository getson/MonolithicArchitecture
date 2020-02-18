using App.WebApi.Localization;
using Autofac;
using BinaryOrigin.SeedWork.Core;
using BinaryOrigin.SeedWork.Messages;
using BinaryOrigin.SeedWork.WebApi.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.WebApi.Infrastructure
{
    public class DependencyRegistrations : IDependencyRegistration
    {
        public int Order => 2;

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder, IConfiguration config)
        {
            builder.RegisterType<HttpContextAccessor>()
                   .As<IHttpContextAccessor>()
                   .SingleInstance();
            builder.RegisterType<ScopeAuthorizationPolicyProvider>()
                   .As<IAuthorizationPolicyProvider>()
                   .SingleInstance();
            builder.RegisterInstance(new HasScopeHandler(config["Auth:Authority"]))
                   .As<IAuthorizationHandler>()
                   .SingleInstance();
            builder.RegisterType<LocalizerService>()
                   .As<ILocalizerService>()
                   .InstancePerLifetimeScope();


        }
    }
}
