using System;
using System.Collections.Generic;
using System.Linq;
using MyApp.Core.Common;
using MyApp.Core.Exceptions;
using MyApp.Core.Extensions;
using MyApp.Domain.Example.CountryAgg;
using MyApp.Domain.Example.CustomerAgg;
using MyApp.Services.DTOs;
using MyApp.SharedKernel.Specification;
using MyApp.SharedKernel.Validator;

namespace MyApp.Services.Example
{
    /// <summary>
    /// The customer management service implementation.
    /// </summary>
    public class CustomerAppService : ICustomerAppService
    {
        #region Members

        private readonly ICountryRepository _countryRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IEntityValidatorFactory _entityValidatorFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of Customer Management Service
        /// </summary>
        /// <param name="countryRepository">Associated country repository</param>
        /// <param name="customerRepository">Associated CustomerRepository, intented to be resolved with DI</param>
        /// <param name="entityValidatorFactory"></param>
        public CustomerAppService(ICountryRepository countryRepository,
                                  ICustomerRepository customerRepository,
                                  IEntityValidatorFactory entityValidatorFactory)
        {
            _countryRepository = countryRepository;
            _customerRepository = customerRepository;
            _entityValidatorFactory = entityValidatorFactory;
        }

        #endregion

        #region ICustomerAppService Members

        public CustomerDto AddNewCustomer(CustomerDto customerDto)
        {
            //check preconditions
            if (customerDto == null || customerDto.CountryId == Guid.Empty)
                throw new ArgumentException("warning_CannotAddCustomerWithEmptyInformation");

            var country = _countryRepository.GetById(customerDto.CountryId);

            if (country != null)
            {
                //Create the entity and the required associated data
                var address = new Address(customerDto.AddressCity,
                                          customerDto.AddressZipCode,
                                          customerDto.AddressAddressLine1,
                                          customerDto.AddressAddressLine2
                                          );

                var customer = CustomerFactory.CreateCustomer(customerDto.FirstName,
                                                              customerDto.LastName,
                                                              customerDto.Telephone,
                                                              customerDto.Company,
                                                              country,
                                                              address);

                //save entity
                SaveCustomer(customer);

                //return the data with id and assigned default values
                return customer.ProjectedAs<CustomerDto>();
            }
            return null;
        }

        public void UpdateCustomer(CustomerDto customerDto)
        {
            if (customerDto == null || customerDto.Id == Guid.Empty)
                throw new ArgumentException("warning_CannotUpdateCustomerWithEmptyInformation");

            //get persisted item
            var persisted = _customerRepository.GetById(customerDto.Id);

            if (persisted != null) //if customer exist
            {
                //materialize from customer dto
                var current = MaterializeCustomerFromDto(customerDto);

                //Merge changes
                _customerRepository.Merge(persisted, current);
            }
        }

        public void RemoveCustomer(Guid customerId)
        {
            var customer = _customerRepository.GetById(customerId);

            if (customer != null) //if customer exist
            {
                //disable customer ( "logical delete" ) 
                customer.Disable();
            }
        }

        public List<CustomerListDto> FindCustomers(int pageIndex, int pageCount)
        {
            if (pageIndex < 0 || pageCount <= 0)
                throw new ArgumentException("warning_InvalidArgumentsForFindCustomers");

            //get customers
            var customers = new PagedList<Customer>(_customerRepository.TableNoTracking.Where(x => x.IsEnabled), pageIndex, pageCount);

            if (customers.Any())
            {
                return customers.ProjectedAsCollection<CustomerListDto>();
            }
            return null;
        }

        public List<CustomerListDto> FindCustomers(string text)
        {
            //get the specification

            var enabledCustomers = CustomerSpecifications.EnabledCustomers();
            var filter = CustomerSpecifications.CustomerFullText(text);

            ISpecification<Customer> spec = enabledCustomers & filter;

            //Query this criteria
            var customers = _customerRepository.AllMatching(spec).ToList();

            if (customers.Any())
            {
                //return adapted data
                return customers.ProjectedAsCollection<CustomerListDto>();
            }
            return null;
        }

        public CustomerDto FindCustomer(Guid customerId)
        {
            //recover existing customer and map
            var customer = _customerRepository.GetById(customerId);

            if (customer != null) //adapt
            {
                return customer.ProjectedAs<CustomerDto>();
            }
            return null;
        }

        public List<CountryDto> FindCountries(int pageIndex, int pageCount)
        {
            if (pageIndex < 0 || pageCount <= 0)
                throw new ArgumentException("warning_InvalidArgumentsForFindCountries");

            //recover countries
            var countries = new PagedList<Country>(_countryRepository.TableNoTracking, pageIndex, pageCount);
            if (countries.Any())
            {
                return countries.ProjectedAsCollection<CountryDto>();
            }
            return null;
        }

        public List<CountryDto> FindCountries(string text)
        {
            //get the specification
            //ISpecification<Country> specification = CountrySpecifications.CountryFullText(text);

            //Query this criteria
            var countries = _countryRepository.TableNoTracking.Where(c => c.CountryName == text);

            return countries.Any() ? countries.ProjectedAsCollection<CountryDto>() : null;
        }

        #endregion

        #region Private Methods

        private void SaveCustomer(Customer customer)
        {
            //recover validator
            var validator = _entityValidatorFactory.Create();

            if (validator.IsValid(customer)) //if customer is valid
            {
                //add the customer into the repository
                _customerRepository.Create(customer);
            }
            else
            {
                //customer is not valid, throw validation errors
                throw new ApplicationValidationErrorsException(validator.GetInvalidMessages(customer));
            }
        }

        private Customer MaterializeCustomerFromDto(CustomerDto customerDto)
        {
            //create the current instance with changes from CustomerDto
            var address = new Address(customerDto.AddressCity,
                                      customerDto.AddressZipCode,
                                      customerDto.AddressAddressLine1,
                                      customerDto.AddressAddressLine2
                                      );

            var country = new Country("Spain", "es-ES");
            // country.ChangeCurrentIdentity(CustomerDto.CountryId);

            var current = CustomerFactory.CreateCustomer(customerDto.FirstName,
                                                         customerDto.LastName,
                                                         customerDto.Telephone,
                                                         customerDto.Company,
                                                         country,
                                                         address);

            current.SetTheCountryReference(customerDto.CountryId);

            //set credit
            current.ChangeTheCurrentCredit(customerDto.CreditLimit);

            //set picture
            var picture = new Picture { RawPhoto = customerDto.PictureRawPhoto };
            //picture.ChangeCurrentIdentity(current.Id);

            current.ChangePicture(picture);

            //set identity
            //current.ChangeCurrentIdentity(CustomerDto.Id);


            return current;
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
    }
}
