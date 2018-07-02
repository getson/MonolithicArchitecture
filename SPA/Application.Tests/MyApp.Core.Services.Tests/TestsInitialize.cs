using System;
using MyApp.Infrastructure.Common.Adapter;
using MyApp.Infrastructure.Common.Validator;
using MyApp.Infrastructure.Mapping.DTOs;
using Xunit;

namespace MyApp.Core.Services.Tests
{
    public class TestsInitialize
    {
        public TestsInitialize()
        {
            InitializeFactories();
        }
        private void InitializeFactories()
        {
            EntityValidatorFactory.SetCurrent(new DataAnnotationsEntityValidatorFactory());
            // this is only to force  current domain to load de mapping assembly and all profiles
            var dto = new CountryDto();

            var adapterfactory = new AutomapperTypeAdapterFactory();
            TypeAdapterFactory.SetCurrent(adapterfactory);


        }
    }
}
