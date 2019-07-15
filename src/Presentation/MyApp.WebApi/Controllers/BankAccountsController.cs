using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MyApp.Application;
using MyApp.Services.DTOs;
using MyApp.Services.Example;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyApp.WebApi.Controllers
{
    /// <summary>
    /// BankAccount API
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BankAccountsController : MyAppBaseController
    {
        private readonly IBankAppService _bankAppService;

        /// <inheritdoc />
        public BankAccountsController(IBankAppService bankAppService)
        {
            _bankAppService = bankAppService;
        }

        // GET: api/bankaccounts
        [HttpGet]
        public IEnumerable<BankAccountDto> Get()
        {
            return _bankAppService.FindBankAccounts();
        }

        // GET api/bankaccounts/getactivities/5
        [HttpGet("GetActivities/{id}")]
        public IEnumerable<BankActivityDto> Get(Guid bankAccountId)
        {
            return _bankAppService.FindBankAccountActivities(bankAccountId);
        }

        // POST api/bankaccounts
        [HttpPost]
        public BankAccountDto Post(BankAccountDto newBankAccount)
        {
            return _bankAppService.AddBankAccount(newBankAccount);
        }

        // PUT api/bankaccounts/lock/5
        [HttpPut("{id}")]
        public bool Lock(Guid bankAccountId)
        {
            return _bankAppService.LockBankAccount(bankAccountId);
        }

        // PUT api/bankaccounts/performtransfer
        [HttpPut]
        public void PerformTransfer([FromBody]BankAccountDto from, [FromBody]BankAccountDto to, decimal amount)
        {
            _bankAppService.PerformBankTransfer(from, to, amount);
        }
    }
}
