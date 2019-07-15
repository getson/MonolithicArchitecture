﻿using MyApp.Domain.Example.CustomerAgg;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MyApp.SharedKernel.Domain;

namespace MyApp.Domain.Example.BankAccountAgg
{
    /// <summary>
    /// The bank account representation (Domain Entity)
    /// </summary>
    public class BankAccount : AggregateRoot
    {

        #region Properties
        /// <summary>
        /// Get or set the bank account number
        /// </summary>

        public virtual BankAccountNumber BankAccountNumber { get; set; }

        /// <summary>
        /// Get the IBAN  (International Bank Account Number)
        /// <remarks>
        /// http://en.wikipedia.org/wiki/International_Bank_Account_Number
        /// Spanish format
        /// </remarks>
        /// </summary>
        public string Iban
        {
            get
            {
                if (BankAccountNumber != null)
                {
                    return $"ES{BankAccountNumber.CheckDigits} {BankAccountNumber.NationalBankCode} {BankAccountNumber.OfficeNumber} {BankAccountNumber.CheckDigits}{BankAccountNumber.AccountNumber}";
                }

                return "No Bank Account Provided";
            }
        }

        /// <summary>
        /// Get the current balance of this account
        /// </summary>
        public decimal Balance { get; private set; }

        /// <summary>
        /// Get the state of this account
        /// </summary>
        public bool Locked { get; private set; }

        /// <summary>
        /// Get or set the customer id associated with this bank account
        /// </summary>
        public Guid CustomerId { get; private set; }

        /// <summary>
        /// The related customer
        /// </summary>
        public virtual Customer Customer { get; private set; }

        private HashSet<BankAccountActivity> _bankAccountActivity;

        /// <summary>
        /// Get the bank account activity into this account
        /// </summary>
        public virtual ICollection<BankAccountActivity> BankAccountActivity
        {
            get
            {
                if (_bankAccountActivity == null)
                    _bankAccountActivity = new HashSet<BankAccountActivity>();

                return _bankAccountActivity;
            }
            set
            {
                _bankAccountActivity = new HashSet<BankAccountActivity>(value);
            }
        }


        #endregion

        #region Public Methods


        /// <summary>
        /// Lock current bank account
        /// </summary>
        public void Lock()
        {
            if (!Locked)
                Locked = true;
        }

        /// <summary>
        /// Un lock current bank account
        /// </summary>
        public void UnLock()
        {
            if (Locked)
                Locked = false;
        }

        /// <summary>
        /// Deposit momey into this bank account
        /// </summary>
        /// <param name="amount">the amount of money to deposit </param>
        public void DepositMoney(decimal amount, string reason)
        {
            if (amount < 0) throw new ArgumentException("exception_BankAccountInvalidWithdrawAmount");

            //DepositMoney is a term of our Ubiquitous Language. Means adding money to this account
            if (!Locked)
            {
                //set balance
                Balance += amount;

                //anotate activity
                var activity = new BankAccountActivity
                {
                    Date = DateTime.UtcNow,
                    Amount = amount,
                    ActivityDescription = reason,
                    BankAccountId = Id
                };
                //activity.GenerateNewIdentity();

                BankAccountActivity.Add(activity);
            }
            else
            {
                throw new InvalidOperationException("exception_BankAccountCannotDeposit");
            }
        }

        /// <summary>
        /// WithdrawMoney operation over this bankaccount
        /// </summary>
        /// <param name="amount">The amount of money for this withdraw operation</param>
        public void WithdrawMoney(decimal amount, string reason)
        {
            if (amount < 0) throw new ArgumentException("exception_BankAccountInvalidWithdrawAmount");

            //WithdrawMoney is a term of our Ubiquitous Language. Means deducting money to this account
            if (CanBeWithdrawed(amount))
            {
                Balance -= amount;

                //anotate activity
                var activity = new BankAccountActivity
                {
                    Date = DateTime.UtcNow,
                    Amount = -amount,
                    ActivityDescription = reason,
                    BankAccountId = Id
                };
                //activity.GenerateNewIdentity();

                BankAccountActivity.Add(activity);
            }
            else
                throw new InvalidOperationException("exception_BankAccountCannotWithdraw");
        }

        /// <summary>
        /// Check if in this bankaccount is posible withdraw <paramref name="amount"/>
        /// </summary>
        /// <param name="amount">The amount of money </param>
        /// <returns>True if is posible perform the operation, else false</returns>
        public bool CanBeWithdrawed(decimal amount)
        {
            return !Locked && (Balance >= amount);
        }

        /// <summary>
        /// Set customer for this bank account
        /// </summary>
        /// <param name="customer">The customer owner of this bank account</param>
        public void SetCustomerOwnerOfThisBankAccount(Customer customer)
        {
            if (customer == null || customer.IsTransient())
            {
                throw new ArgumentException("exception_CannotAssociateTransientOrNullCustomer");
            }

            //fix id and set reference
            CustomerId = customer.Id;
            Customer = customer;
        }

        /// <summary>
        /// Change current customer reference using a customer id
        /// </summary>
        /// <param name="customerId">The new customer identifier</param>
        public void ChangeCustomerOwnerReference(Guid customerId)
        {
            if (customerId != Guid.Empty)
            {
                //fix a new id 
                Customer = null;
                CustomerId = customerId;
            }
        }

        #endregion

        #region IValidatableObject Members

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();

            if (BankAccountNumber == null)
                validationResults.Add(new ValidationResult("validation_BankAccountNumberCannotBeNull", new[] { "BankAccountNumber" }));
            else
            {
                if (string.IsNullOrWhiteSpace(BankAccountNumber.AccountNumber))
                    validationResults.Add(new ValidationResult("validation_BankAccountBankAccountNumberCannotBeNull", new[] { "AccountNumber" }));

                if (string.IsNullOrWhiteSpace(BankAccountNumber.CheckDigits))
                    validationResults.Add(new ValidationResult("validation_BankAccountBankCheckDigitsCannotBeNull", new[] { "CheckDigits" }));

                if (string.IsNullOrWhiteSpace(BankAccountNumber.NationalBankCode))
                    validationResults.Add(new ValidationResult("validation_BankAccountBankNationalBankCodeCannotBeNull", new[] { "NationalBankCode" }));

                if (string.IsNullOrWhiteSpace(BankAccountNumber.OfficeNumber))
                    validationResults.Add(new ValidationResult("validation_BankAccountBankOfficeNumberCannotBeNull", new[] { "OfficeNumber" }));
            }

            if (CustomerId == Guid.Empty)
                validationResults.Add(new ValidationResult("validation_BankAccountCustomerIdIsEmpty", new[] { "CustomerId" }));

            return validationResults;
        }

        #endregion

        public void ChangeCurrentIdentity(Guid targetId)
        {
            Id = targetId;
        }
    }
}