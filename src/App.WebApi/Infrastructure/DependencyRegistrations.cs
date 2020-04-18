using App.Core;
using App.WebApi.Infrastructure.Authorization;
using App.WebApi.Localization;
using Autofac;
using BinaryOrigin.SeedWork.Core;
using BinaryOrigin.SeedWork.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace App.WebApi.Infrastructure
{
    public class DependencyRegistrations : IDependencyRegistration
    {
        public int Order => 2;

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<HttpContextAccessor>()
                   .As<IHttpContextAccessor>()
                   .SingleInstance();
            builder.RegisterType<ScopeAuthorizationPolicyProvider>()
                   .As<IAuthorizationPolicyProvider>()
                   .SingleInstance();
            builder.RegisterType<HasScopeHandler>()
                   .As<IAuthorizationHandler>()
                   .SingleInstance();
            builder.RegisterType<LocalizerService>()
                   .As<ILocalizerService>()
                   .InstancePerLifetimeScope();
        }
    }
}