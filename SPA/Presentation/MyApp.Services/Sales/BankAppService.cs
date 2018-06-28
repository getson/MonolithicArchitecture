using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Transactions;
using Microsoft.EntityFrameworkCore.Internal;
using MyApp.Core.Domain.Example.BankAccountAgg;
using MyApp.Core.Domain.Example.CustomerAgg;
using MyApp.Core.Domain.Services.Banking;
using MyApp.Core.Domain.Services.Logging;
using MyApp.Core.Interfaces.Data;
using MyApp.Infrastructure.Common;
using MyApp.Infrastructure.Common.Validator;
using MyApp.Mapping;
using ILogger = MyApp.Core.Domain.Services.Logging.ILogger;
using LogLevel = MyApp.Core.Domain.Logging.LogLevel;

namespace MyApp.Services.Sales
{
    /// <summary>
    /// The bank management service implementation
    /// </summary>
    public class BankAppService : IBankAppService
    {
        #region Members

        readonly IRepository<BankAccount> _bankAccountRepository;
        readonly IRepository<Customer> _customerRepository;
        readonly IBankTransferService _transferService;
        private readonly ILogger _logger;
        readonly IUserActivityService _activityLogService;

        #endregion

        #region Constructor

        /// <summary>
        /// Create a new instance 
        /// </summary>
        public BankAppService(IRepository<BankAccount> bankAccountRepository,
                              IRepository<Customer> customerRepository, // the customer repository dependency
                              IBankTransferService transferService,
                              IUserActivityService activityLogService,
                              ILogger logger)
        {
            //check preconditions
            if (bankAccountRepository == null)
                throw new ArgumentNullException("bankAccountRepository");

            if (customerRepository == null)
                throw new ArgumentNullException("customerRepository");

            if (transferService == null)
                throw new ArgumentNullException("trasferService");

            _bankAccountRepository = bankAccountRepository;
            _customerRepository = customerRepository;
            _transferService = transferService;
            _logger = logger;
            _activityLogService = activityLogService;
        }

        #endregion

        #region IBankAppService Members

        public BankAccountDto AddBankAccount(BankAccountDto bankAccountDto)
        {
            if (bankAccountDto == null || bankAccountDto.CustomerId == 0)
                throw new ArgumentException("warning_CannotAddNullBankAccountOrInvalidCustomer");

            //check if exists the customer for this bank account
            var associatedCustomer = _customerRepository.GetById(bankAccountDto.CustomerId);

            if (associatedCustomer != null) // if the customer exist
            {
                //Create a new bank account  number
                var accountNumber = CalculateNewBankAccountNumber();

                //Create account from factory 
                var account = BankAccountFactory.CreateBankAccount(associatedCustomer, accountNumber);

                //save bank account
                SaveBankAccount(account);

                return account.ProjectedAs<BankAccountDto>();
            }
            else //the customer for this bank account not exist, cannot create a new bank account
                throw new InvalidOperationException("warning_CannotCreateBankAccountForNonExistingCustomer");


        }

        public bool LockBankAccount(int bankAccountId)
        {
            //recover bank account, lock and commit changes
            var bankAccount = _bankAccountRepository.GetById(bankAccountId);

            if (bankAccount != null)
            {
                bankAccount.Lock();
                _bankAccountRepository.Update(bankAccount);
                return true;
            }
            else // if not exist the bank account return false
            {
                _logger.InsertLog(LogLevel.Warning, $"warning_CannotLockNonExistingBankAccount {bankAccountId}");

                return false;
            }
        }

        public List<BankAccountDto> FindBankAccounts()
        {
            var bankAccounts = _bankAccountRepository.TableNoTracking;

            if (bankAccounts != null
                &&
                bankAccounts.Any())
            {
                return bankAccounts.ProjectedAsCollection<BankAccountDto>();
            }
            else // no results
                return null;
        }

        public void PerformBankTransfer(BankAccountDto fromAccount, BankAccountDto toAccount, decimal amount)
        {
            //Application-Logic Process: 
            // 1º Get Accounts objects from Repositories
            // 2º Start Transaction
            // 3º Call PerformTransfer method in Domain Service
            // 4º If no exceptions, commit the unit of work and complete transaction

            if (BankAccountHasIdentity(fromAccount) && BankAccountHasIdentity(toAccount))
            {
                var source = _bankAccountRepository.GetById(fromAccount.Id);
                var target = _bankAccountRepository.GetById(toAccount.Id);

                if (source != null & target != null) // if all accounts exist
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        //perform transfer
                        _transferService.PerformTransfer(amount, source, target);

                        //comit unit of work
                        //_bankAccountRepository.UnitOfWork.Commit();

                        //complete transaction
                        scope.Complete();
                    }
                }
                else
                    _logger.Error("error_CannotPerformTransferInvalidAccounts");
            }
            else
                _logger.Error("error_CannotPerformTransferInvalidAccounts");

        }

        /// <summary>
        /// <see cref="IBankAppService"/>
        /// </summary>
        /// <param name="bankAccountId"><see /></param>
        /// <returns><see /></returns>
        public List<BankActivityDto> FindBankAccountActivities(int bankAccountId)
        {
            var account = _bankAccountRepository.GetById(bankAccountId);

            if (account != null)
            {
                return account.BankAccountActivity.ProjectedAsCollection<BankActivityDto>();
            }
            else // the bank account not exist
            {
                _logger.Warning("warning_CannotGetActivitiesForInvalidOrNotExistingBankAccount");
                return null;
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion

        #region Private Methods

        void SaveBankAccount(BankAccount bankAccount)
        {
            //validate bank account
            var validator = EntityValidatorFactory.CreateValidator();

            if (validator.IsValid<BankAccount>(bankAccount)) // save entity
            {
                _bankAccountRepository.Insert(bankAccount);
            }
            else //throw validation errors
                throw new ApplicationValidationErrorsException(validator.GetInvalidMessages(bankAccount));
        }

        BankAccountNumber CalculateNewBankAccountNumber()
        {
            var bankAccountNumber = new BankAccountNumber
            {
                OfficeNumber = "2354",
                NationalBankCode = "2134",
                CheckDigits = "02",
                AccountNumber = new Random().Next(1, Int32.MaxValue).ToString()
            };

            //simulate bank account number creation....


            return bankAccountNumber;

        }

        bool BankAccountHasIdentity(BankAccountDto bankAccountDto)
        {
            //return true is bank account dto has identity
            return (bankAccountDto != null && bankAccountDto.Id != 0);
        }

        #endregion
    }
}
