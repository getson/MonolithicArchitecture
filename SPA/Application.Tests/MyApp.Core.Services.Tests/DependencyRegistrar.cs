using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using MyApp.Core.Abstractions.Infrastructure;
using MyApp.Core.Configuration;

namespace MyApp.Services.Tests
{
   public class DependencyRegistrar:IDependencyRegistrar
    {
        public void Register(ContainerBuilder builder, ITypeFinder typeFinder, MyAppConfig config)
        {
            
        }

        public int Order => 0;
    }
}
