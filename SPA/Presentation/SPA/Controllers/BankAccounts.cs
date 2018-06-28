using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MyApp.Mapping;
using MyApp.Mapping.DTOs;
using MyApp.Services.Sales;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SPA.Controllers
{
    [Route("api/[controller]")]
    public class BankAccounts : Controller, IDisposable
    {
        readonly IBankAppService _bankAppService;

        public BankAccounts(IBankAppService bankAppService)
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
        public IEnumerable<BankActivityDto> Get(int bankAccountId)
        {
            return _bankAppService.FindBankAccountActivities(bankAccountId);
        }

        // POST api/bankaccounts
        [HttpPost]
        public BankAccountDto Post([FromBody]BankAccountDto newBankAccount)
        {
            return _bankAppService.AddBankAccount(newBankAccount);
        }

        // PUT api/bankaccounts/lock/5
        [HttpPut("{id}")]
        public bool Lock(int bankAccountId)
        {
            return _bankAppService.LockBankAccount(bankAccountId);
        }

        // PUT api/bankaccounts/performtransfer
        [HttpPut]
        public void PerformTransfer([FromBody]BankAccountDto from, [FromBody]BankAccountDto to, [FromBody]decimal amount)
        {
            _bankAppService.PerformBankTransfer(from, to, amount);
        }

        #region IDisposable Members
        public void Dispose()
        {
            //dispose all resources
            _bankAppService.Dispose();
        }
        #endregion
    }
}
