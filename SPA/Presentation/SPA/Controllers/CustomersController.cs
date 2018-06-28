using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MyApp.Mapping;
using MyApp.Mapping.DTOs;
using MyApp.Services.Sales;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SPA.Controllers
{
    /// <summary>
    /// Customers API
    /// </summary>
    [Route("api/[controller]")]
    public class CustomersController : Controller, IDisposable
    {
        private readonly ICustomerAppService _customerAppService;

        /// <inheritdoc />
        public CustomersController(ICustomerAppService customerAppService)
        {
            _customerAppService = customerAppService;
        }
        // GET: api/customers?filter=filter
        [HttpGet]
        public IEnumerable<CustomerListDto> Get(string filter)
        {
            return _customerAppService.FindCustomers(filter);
        }
        // GET: api/customers/getpaged/?pageIndex=1&pageCount=1
        [HttpGet("GetPaged")]
        public IEnumerable<CustomerListDto> Get(int pageIndex, int pageCount)
        {
            return _customerAppService.FindCustomers(pageIndex, pageCount);
        }
        // GET: api/customers/1
        [HttpGet("{id}")]
        public CustomerDto Get(int id)
        {
            return _customerAppService.FindCustomer(id);
        }
        // POST: api/customers/
        [HttpPost]
        public CustomerDto Post([FromBody]CustomerDto customer)
        {
            return _customerAppService.AddNewCustomer(customer);
        }
        // PUT: api/customers/
        [HttpPut("{id}")]
        public void Put([FromBody]CustomerDto customer)
        {
            _customerAppService.UpdateCustomer(customer);
        }
        /// <summary>
        /// DELETE: api/customers/
        /// </summary>

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _customerAppService.RemoveCustomer(id);
        }

        //// GET: api/customers/countries
        [HttpGet("GetCountries")]
        public IEnumerable<CountryDto> Countries(string filter)
        {
            return _customerAppService.FindCountries(filter);
        }
        //// GET: api/customers/getpagedcountries?pageIndex=1&pageCount=1
        [HttpGet("GetPagedCountries")]
        public IEnumerable<CountryDto> Countries(int pageIndex, int pageCount)
        {
            return _customerAppService.FindCountries(pageIndex, pageCount);
        }

        #region IDisposable Members
        public void Dispose()
        {
            //dispose all resources
            _customerAppService.Dispose();
        }
        #endregion
    }
}
