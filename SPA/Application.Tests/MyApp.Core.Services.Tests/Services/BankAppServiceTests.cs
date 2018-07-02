using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Moq;
using MyApp.Core.Domain.Example.BankAccountAgg;
using MyApp.Core.Domain.Example.CustomerAgg;
using MyApp.Core.Domain.Services.Banking;
using MyApp.Infrastructure.Mapping.DTOs;
using MyApp.Services.Example;
using MyApp.Services.Logging;
using Xunit;

namespace MyApp.Core.Services.Tests.Services
{
    public class BankAppServiceTests : TestsInitialize
    {
        [Fact]
        public void LockBankAccountReturnFalseIfBankAccountNotExist()
        {
            //Arrange
            var customerRepository = new Mock<ICustomerRepository>();
            var transferService = new BankTransferService();
            var activityLogService = new Mock<IUserActivityService>();
            var bankAccountRepository = new Mock<IBankAccountRepository>();
            var logger = new Mock<MyApp.Services.Logging.ILogger>();

            bankAccountRepository
                .Setup(x => x.GetById(It.IsAny<int>()))
                .Returns((int id) => null);

            var mockLogger = new Mock<ILogger<BankAppService>>();

            IBankAppService bankingService = new BankAppService(bankAccountRepository.Object, customerRepository.Object, transferService, activityLogService.Object, logger.Object);

            //Act
            var result = bankingService.LockBankAccount(0);

            //Assert
            Assert.False(result);
        }
        [Fact]
        public void AddBankAccountThrowArgumentNullExceptionWhenBankAccountDtoIsNull()
        {
            //Arrange
            var customerRepository = new Mock<ICustomerRepository>();
            var activityLogService = new Mock<IUserActivityService>();
            var bankAccountRepository = new Mock<IBankAccountRepository>();
            var logger = new Mock<MyApp.Services.Logging.ILogger>();

            IBankTransferService transferService = new BankTransferService();
            var bankingService = new BankAppService(bankAccountRepository.Object, customerRepository.Object, transferService, activityLogService.Object, logger.Object);

            var ex = Assert.Throws<ArgumentException>(() =>
                {
                    //Act
                    var result = bankingService.AddBankAccount(null);

                    //Assert

                    Assert.Null(result);
                }
            );

            Assert.IsType<ArgumentException>(ex);

        }
        [Fact]
        public void AddBankAccountReturnNullWhenCustomerIdIsEmpty()
        {
            //Arrange
            var customerRepository = new Mock<ICustomerRepository>();
            var activityLogService = new Mock<IUserActivityService>();
            var bankAccountRepository = new Mock<IBankAccountRepository>();
            var logger = new Mock<MyApp.Services.Logging.ILogger>();

            IBankTransferService transferService = new BankTransferService();

            var dto = new BankAccountDto()
            {
                CustomerId = 0
            };

            var bankingService = new BankAppService(bankAccountRepository.Object, customerRepository.Object, transferService, activityLogService.Object, logger.Object);

            Exception ex = Assert.Throws<ArgumentException>(() =>
                {
                    //Act
                    var result = bankingService.AddBankAccount(dto);
                }
            );

            Assert.IsType<ArgumentException>(ex);

        }

        [Fact]
        public void AddBankAccountReturnDtoWhenSaveSucceed()
        {
            //Arrange
            IBankTransferService transferService = new BankTransferService();

            var customerRepository = new Mock<ICustomerRepository>();
            var activityLogService = new Mock<IUserActivityService>();
            var bankAccountRepository = new Mock<IBankAccountRepository>();
            var logger = new Mock<MyApp.Services.Logging.ILogger>();

            customerRepository
                .Setup(x => x.GetById(It.IsAny<int>()))
                .Returns((Guid guid) =>
                    {
                        var customer = new Customer()
                        {
                            FirstName = "Jhon",
                            LastName = "El rojo"
                        };
                        return customer;
                    }
                );

            bankAccountRepository.Setup(x => x.Insert(It.IsAny<BankAccount>()));


            var dto = new BankAccountDto()
            {
                CustomerId = new Random().Next(),
                BankAccountNumber = "BA"
            };

            Mock<ILogger<BankAppService>> mockLogger = new Mock<ILogger<BankAppService>>();

            IBankAppService bankingService = new BankAppService(bankAccountRepository.Object, customerRepository.Object, transferService, activityLogService.Object, logger.Object);

            //Act
            var result = bankingService.AddBankAccount(dto);

            //Assert
            Assert.NotNull(result);

        }
        
        [Fact]
        public void PerformBankTransfer()
        {
            //Arrange

            //--> source bank account data

            var sourceId = new Random().Next();
            var bankAccountNumberSource = new BankAccountNumber("4444", "5555", "3333333333", "02");
            var sourceCustomer = new Customer();
        

            var source = BankAccountFactory.CreateBankAccount(sourceCustomer, bankAccountNumberSource);
    
            source.DepositMoney(1000, "initial");

            var sourceBankAccountDto = new BankAccountDto()
            {
                Id = sourceId,
                BankAccountNumber = source.Iban
            };

            //--> target bank account data
            var targetCustomer = new Customer();
   
            var targetId =new Random().Next();
            var bankAccountNumberTarget = new BankAccountNumber("1111", "2222", "3333333333", "01");
            var target = BankAccountFactory.CreateBankAccount(targetCustomer, bankAccountNumberTarget);
            target.ChangeCurrentIdentity(targetId);


            var targetBankAccountDto = new BankAccountDto()
            {
                Id = targetId,
                BankAccountNumber = target.Iban
            };

            var accounts = new List<BankAccount>() { source, target };

            var bankAccountRepository = new Mock<IBankAccountRepository>();

            bankAccountRepository
                .Setup(x => x.GetById(It.IsAny<int>()))
                .Returns((int id) =>
                {
                    return accounts.SingleOrDefault(ba => ba.Id == id);
                });


            var customerRepository = new Mock<ICustomerRepository>();
            var activityLogService = new Mock<IUserActivityService>();
            var logger = new Mock<MyApp.Services.Logging.ILogger>();

            IBankTransferService transferService = new BankTransferService();
            IBankAppService bankingService = new BankAppService(bankAccountRepository.Object, customerRepository.Object, transferService, activityLogService.Object,logger.Object);

            //Act
            bankingService.PerformBankTransfer(sourceBankAccountDto, targetBankAccountDto, 100M);

            //Assert
            Assert.Equal(900, source.Balance);
            Assert.Equal(100, target.Balance);
        }
    }
}
