using System.Collections.Generic;
using System.Linq;
using MyApp.Core.Domain.Example.CountryAgg;
using MyApp.Core.Interfaces.Mapping;
using MyApp.Infrastructure.Common.Adapter;
using MyApp.Infrastructure.Mapping.DTOs;
using Xunit;

namespace MyApp.Core.Services.Tests.Adapters
{
    public class CountryAdapterTests : TestsInitialize
    {
        [Fact]
        public void CountryToCountryDtoAdapter()
        {
            //Arrange
            var country = new Country("Spain", "es-ES");
            country.GenerateNewIdentity();

            //Act
            ITypeAdapter adapter = TypeAdapterFactory.CreateAdapter();
            var dto = adapter.Adapt<Country, CountryDto>(country);

            //Assert
            Assert.Equal(country.Id, dto.Id);
            Assert.Equal(country.CountryName, dto.CountryName);
            Assert.Equal(country.CountryIsoCode, dto.CountryIsoCode);
        }
        [Fact]
        public void CountryEnumerableToCountryDtoList()
        {
            //Arrange

            var country = new Country("Spain", "es-ES");
            country.GenerateNewIdentity();

            IEnumerable<Country> countries = new List<Country>() { country };

            //Act
            ITypeAdapter adapter = TypeAdapterFactory.CreateAdapter();
            var dtos = adapter.Adapt<IEnumerable<Country>, List<CountryDto>>(countries);

            //Assert
            Assert.NotNull(dtos);
            Assert.True(dtos.Any());
            Assert.True(dtos.Count == 1);

            var dto = dtos[0];

            Assert.Equal(country.Id, dto.Id);
            Assert.Equal(country.CountryName, dto.CountryName);
            Assert.Equal(country.CountryIsoCode, dto.CountryIsoCode);
        }
    }
}
