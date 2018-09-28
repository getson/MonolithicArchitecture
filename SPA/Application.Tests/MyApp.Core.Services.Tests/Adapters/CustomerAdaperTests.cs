using System.Collections.Generic;
using System.Linq;
using MyApp.Domain.Example.CountryAgg;
using MyApp.Domain.Example.CustomerAgg;
using MyApp.Services.DTOs;
using Xunit;

namespace MyApp.Services.Tests.Adapters
{
    public class CustomerAdaperTests : TestsInitialize
    {
        [Fact]
        public void CustomerToCustomerDtoAdapt()
        {
            //Arrange

            var country = new Country("Spain", "es-ES");
            country.GenerateNewIdentity();

            var address = new Address("Monforte", "27400", "AddressLine1", "AddressLine2");

            var customer = CustomerFactory.CreateCustomer("john", "El rojo", "617404929", "Spirtis", country, address);
            var picture = new Picture { RawPhoto = new byte[0] { } };

            customer.ChangeTheCurrentCredit(1000M);
            customer.ChangePicture(picture);
            customer.SetTheCountryForThisCustomer(country);

            //Act
            var dto = TypeAdapter.Adapt<Customer, CustomerDto>(customer);

            //Assert

            Assert.Equal(customer.Id, dto.Id);
            Assert.Equal(customer.FirstName, dto.FirstName);
            Assert.Equal(customer.LastName, dto.LastName);
            Assert.Equal(customer.Company, dto.Company);
            Assert.Equal(customer.Telephone, dto.Telephone);
            Assert.Equal(customer.CreditLimit, dto.CreditLimit);

            Assert.Equal(customer.Country.CountryName, dto.CountryCountryName);
            Assert.Equal(country.Id, dto.CountryId);


            Assert.Equal(customer.Address.City, dto.AddressCity);
            Assert.Equal(customer.Address.ZipCode, dto.AddressZipCode);
            Assert.Equal(customer.Address.AddressLine1, dto.AddressAddressLine1);
            Assert.Equal(customer.Address.AddressLine2, dto.AddressAddressLine2);
        }

        [Fact]
        public void CustomerEnumerableToCustomerListDtoListAdapt()
        {
            //Arrange



            var country = new Country("Spain", "es-ES");
            country.GenerateNewIdentity();

            var address = new Address("Monforte", "27400", "AddressLine1", "AddressLine2");

            var customer = CustomerFactory.CreateCustomer("john", "El rojo", "617404929", "Spirtis", country, address);
            var picture = new Picture { RawPhoto = new byte[0] { } };

            customer.ChangeTheCurrentCredit(1000M);
            customer.ChangePicture(picture);
            customer.SetTheCountryForThisCustomer(country);

            IEnumerable<Customer> customers = new List<Customer> { customer };

            //Act
            var dtos = TypeAdapter.Adapt<IEnumerable<Customer>, List<CustomerListDto>>(customers);

            //Assert

            Assert.NotNull(dtos);
            Assert.True(dtos.Any());
            Assert.True(dtos.Count == 1);

            var dto = dtos[0];

            Assert.Equal(customer.Id, dto.Id);
            Assert.Equal(customer.FirstName, dto.FirstName);
            Assert.Equal(customer.LastName, dto.LastName);
            Assert.Equal(customer.Company, dto.Company);
            Assert.Equal(customer.Telephone, dto.Telephone);
            Assert.Equal(customer.CreditLimit, dto.CreditLimit);
            Assert.Equal(customer.Address.City, dto.AddressCity);
            Assert.Equal(customer.Address.ZipCode, dto.AddressZipCode);
            Assert.Equal(customer.Address.AddressLine1, dto.AddressAddressLine1);
            Assert.Equal(customer.Address.AddressLine2, dto.AddressAddressLine2);


        }
    }
}
