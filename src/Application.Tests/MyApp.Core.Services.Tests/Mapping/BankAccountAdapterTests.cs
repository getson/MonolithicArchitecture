using System;
using System.Collections.Generic;
using MyApp.Core.Abstractions.Mapping;
using MyApp.Domain.Example.BankAccountAgg;
using MyApp.Domain.Example.CountryAgg;
using MyApp.Domain.Example.CustomerAgg;
using MyApp.Services.DTOs;
using Xunit;

namespace MyApp.Services.Tests.Mapping
{
    public class BankAccountAdapterTests : TestsInitialize
    {

        [Fact]
        public void AdaptBankActivityToBankActivityDto()
        {
            //Arrange
            var activity = new BankAccountActivity
            {
                Date = DateTime.Now,
                Amount = 1000,
                ActivityDescription = "transfer...",
                Id = (Guid.NewGuid())
            };

            //activity.GenerateNewIdentity();


            //Act
            var adapter = TypeAdapterFactory.Instance.CreateAdapter();

            var activityDto = adapter.Adapt<BankAccountActivity, BankActivityDto>(activity);

            //Assert
            Assert.Equal(activity.Date, activityDto.Date);
            Assert.Equal(activity.Amount, activityDto.Amount);
            Assert.Equal(activity.ActivityDescription, activityDto.ActivityDescription);
        }
        [Fact]
        public void AdaptEnumerableBankActivityToListBankActivityDto()
        {
            //Arrange
            var activity = new BankAccountActivity
            {

                //activity.GenerateNewIdentity();
                Date = DateTime.Now,
                Amount = 1000,
                ActivityDescription = "transfer..."
            };

            IEnumerable<BankAccountActivity> activities = new List<BankAccountActivity>
            {
                activity
            };

            //Act
            var activitiesDto = TypeAdapter.Adapt<IEnumerable<BankAccountActivity>, List<BankActivityDto>>(activities);

            //Assert
            Assert.NotNull(activitiesDto);
            Assert.True(activitiesDto.Count == 1);

            Assert.Equal(activity.Date, activitiesDto[0].Date);
            Assert.Equal(activity.Amount, activitiesDto[0].Amount);
            Assert.Equal(activity.ActivityDescription, activitiesDto[0].ActivityDescription);
        }
        [Fact]
        public void AdaptBankAccountToBankAccountDto()
        {
            //Arrange
            var country = new Country("Spain", "es-ES")
            {
                Id = Guid.NewGuid()
            };
            country.GenerateNewIdentity();

            var customer = CustomerFactory.CreateCustomer("john", "el rojo", "+3441", "company", country, new Address("", "", "", ""));
            customer.Id = Guid.NewGuid();
            //customer.GenerateNewIdentity();

            var account = new BankAccount
            {
                Id = (Guid.NewGuid()),
                BankAccountNumber = new BankAccountNumber("4444", "5555", "3333333333", "02")
            };
            account.Id = Guid.NewGuid();
            account.SetCustomerOwnerOfThisBankAccount(customer);
            account.DepositMoney(1000, "reason");
            account.Lock();

            //Act
            var bankAccountDto = TypeAdapter.Adapt<BankAccount, BankAccountDto>(account);


            //Assert
            Assert.Equal(account.Id, bankAccountDto.Id);
            Assert.Equal(account.Iban, bankAccountDto.BankAccountNumber);
            Assert.Equal(account.Balance, bankAccountDto.Balance);
            Assert.Equal(account.Customer.FirstName, bankAccountDto.CustomerFirstName);
            Assert.Equal(account.Customer.LastName, bankAccountDto.CustomerLastName);
            Assert.Equal(account.Locked, bankAccountDto.Locked);
        }
        [Fact]
        public void AdaptEnumerableBankAccountToListBankAccountListDto()
        {
            //Arrange

            var country = new Country("spain", "es-ES");
            country.GenerateNewIdentity();

            var customer = CustomerFactory.CreateCustomer("john", "el rojo", "+341232", "company", country, new Address("", "", "", ""));
            var account = new BankAccount
            {
                BankAccountNumber = new BankAccountNumber("4444", "5555", "3333333333", "02")
            };
            account.SetCustomerOwnerOfThisBankAccount(customer);
            account.DepositMoney(1000, "reason");
            var accounts = new List<BankAccount> { account };

            //Act
            var bankAccountsDto = TypeAdapter.Adapt<IEnumerable<BankAccount>, List<BankAccountDto>>(accounts);


            //Assert
            Assert.NotNull(bankAccountsDto);
            Assert.True(bankAccountsDto.Count == 1);

            Assert.Equal(account.Id, bankAccountsDto[0].Id);
            Assert.Equal(account.Iban, bankAccountsDto[0].BankAccountNumber);
            Assert.Equal(account.Balance, bankAccountsDto[0].Balance);
            Assert.Equal(account.Customer.FirstName, bankAccountsDto[0].CustomerFirstName);
            Assert.Equal(account.Customer.LastName, bankAccountsDto[0].CustomerLastName);
        }
    }
}
