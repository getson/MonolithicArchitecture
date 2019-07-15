﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MyApp.Application;
using MyApp.Services.DTOs;
using MyApp.Services.Example;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyApp.WebApi.Controllers
{
    /// <summary>
    /// Customers API
    /// </summary>
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CustomersController : MyAppBaseController
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
        public CustomerDto Get(Guid id)
        {
            return _customerAppService.FindCustomer(id);
        }
        // POST: api/customers/
        [HttpPost]
        public CustomerDto Post(CustomerDto customer)
        {
            return _customerAppService.AddNewCustomer(customer);
        }
        // PUT: api/customers/
        [HttpPut("{id}")]
        public void Put(CustomerDto customer)
        {
            _customerAppService.UpdateCustomer(customer);
        }
        /// <summary>
        /// DELETE: api/customers/
        /// </summary>

        [HttpDelete("{id}")]
        public void Delete(Guid id)
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
    }
}