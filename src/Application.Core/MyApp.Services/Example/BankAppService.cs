using System;
using System.Collections.Generic;
using System.Transactions;
using Microsoft.EntityFrameworkCore.Internal;
using MyApp.Core.Exceptions;
using MyApp.Core.Extensions;
using MyApp.Domain.Example.BankAccountAgg;
using MyApp.Domain.Example.CustomerAgg;
using MyApp.Services.DTOs;
using MyApp.SharedKernel.Validator;

namespace MyApp.Services.Example
{
    /// <summary>
    /// The bank management service implementation
    /// </summary>
    public class BankAppService : IBankAppService
    {
        #region Members

        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IBankTransferService _transferService;
        private readonly IEntityValidatorFactory _entityValidatorFactory;


        #endregion

        #region Constructor

        /// <summary>
        /// Create a new instance 
        /// </summary>
        public BankAppService(IBankAccountRepository bankAccountRepository,
                              ICustomerRepository customerRepository,
                              IBankTransferService transferService,
                              IEntityValidatorFactory entityValidatorFactory)
        {

            _bankAccountRepository = bankAccountRepository;
            _customerRepository = customerRepository;
            _transferService = transferService;
            _entityValidatorFactory = entityValidatorFactory;
        }

        #endregion

        #region IBankAppService Members

        public BankAccountDto AddBankAccount(BankAccountDto bankAccountDto)
        {
            if (bankAccountDto == null || bankAccountDto.CustomerId == Guid.Empty)
            {
                throw new ArgumentException("warning_CannotAddNullBankAccountOrInvalidCustomer");
            }

            //check if exists the customer for this bank account
            var associatedCustomer = _customerRepository.GetById(bankAccountDto.CustomerId);

            if (associatedCustomer == null)
            {
                throw new InvalidOperationException("warning_CannotCreateBankAccountForNonExistingCustomer");
            }
            //Create a new bank account  number
            var accountNumber = CalculateNewBankAccountNumber();

            //Create account from factory 
            var account = BankAccountFactory.CreateBankAccount(associatedCustomer, accountNumber);

            //save bank account
            SaveBankAccount(account);

            return account.ProjectedAs<BankAccountDto>();
        }

        public bool LockBankAccount(Guid bankAccountId)
        {
            //recover bank account, lock and commit changes
            var bankAccount = _bankAccountRepository.GetById(bankAccountId);

            if (bankAccount != null)
            {
                bankAccount.Lock();
                _bankAccountRepository.Update(bankAccount);
                return true;
            }
            return false;
        }

        public List<BankAccountDto> FindBankAccounts()
        {
            var bankAccounts = _bankAccountRepository.TableNoTracking;

            if (bankAccounts != null
                && bankAccounts.Any())
            {
                return bankAccounts.ProjectedAsCollection<BankAccountDto>();
            }
            return null;
        }

        public void PerformBankTransfer(BankAccountDto fromAccount, BankAccountDto toAccount, decimal amount)
        {
            //Application-Logic Process: 
            // 1º Get Accounts objects from Repositories
            // 2º Start Transaction
            // 3º Call PerformTransfer method in Domain Service
            // 4º If no exceptions, commit the unit of work and complete transaction

         
                var source = _bankAccountRepository.GetById(fromAccount.Id);
                var target = _bankAccountRepository.GetById(toAccount.Id);

                if (source != null & target != null) // if all accounts exist
                {
                    using (var scope = new TransactionScope())
                    {
                        //perform transfer
                        _transferService.PerformTransfer(amount, source, target);

                        //comit unit of work
                        //_bankAccountRepository.UnitOfWork.Commit();

                        //complete transaction
                        scope.Complete();
                    }
                }                 
        }

        /// <summary>
        /// <see cref="IBankAppService"/>
        /// </summary>
        /// <param name="bankAccountId"><see /></param>
        /// <returns><see /></returns>
        public List<BankActivityDto> FindBankAccountActivities(Guid bankAccountId)
        {
            var account = _bankAccountRepository.GetById(bankAccountId);

            return account?.BankAccountActivity.ProjectedAsCollection<BankActivityDto>();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion

        #region Private Methods

        private void SaveBankAccount(BankAccount bankAccount)
        {
            //validate bank account
            var validator = _entityValidatorFactory.Create();

            if (validator.IsValid(bankAccount)) // save entity
            {
                _bankAccountRepository.Create(bankAccount);
            }
            else //throw validation errors
            {
                throw new ApplicationValidationErrorsException(validator.GetInvalidMessages(bankAccount));
            }
        }

        private BankAccountNumber CalculateNewBankAccountNumber()
        {
            var bankAccountNumber = new BankAccountNumber
            {
                OfficeNumber = "2354",
                NationalBankCode = "2134",
                CheckDigits = "02",
                AccountNumber = new Random().Next(1, int.MaxValue).ToString()
            };

            //simulate bank account number creation....


            return bankAccountNumber;

        }

        private bool BankAccountHasIdentity(BankAccountDto bankAccountDto)
        {
            //return true is bank account dto has identity
            return (bankAccountDto != null && bankAccountDto.Id != Guid.Empty);
        }

        #endregion
    }
}
