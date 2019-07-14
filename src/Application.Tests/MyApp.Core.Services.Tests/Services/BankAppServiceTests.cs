using Moq;
using MyApp.Domain.Example.BankAccountAgg;
using MyApp.Domain.Example.CustomerAgg;
using MyApp.Services.DTOs;
using MyApp.Services.Example;
using MyApp.Services.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using MyApp.SharedKernel.Validator;
using Xunit;

namespace MyApp.Services.Tests.Services
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

            var entityValidatorFactory = new Mock<IEntityValidatorFactory>();

            bankAccountRepository
                .Setup(x => x.GetById(It.IsAny<int>()))
                .Returns((int id) => null);
            IBankAppService bankingService = new BankAppService(bankAccountRepository.Object,
                                                                customerRepository.Object,
                                                                transferService,
                                                                activityLogService.Object,
                                                                Logger,
                                                                entityValidatorFactory.Object);

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
            var logger = new Mock<ILogger>();
            var entityValidatorFactory = new Mock<IEntityValidatorFactory>();

            IBankTransferService transferService = new BankTransferService();
            var bankingService = new BankAppService(bankAccountRepository.Object,
                                                    customerRepository.Object,
                                                    transferService,
                                                    activityLogService.Object,
                                                    Logger,
                                                    entityValidatorFactory.Object
                                                    );

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
            var logger = new Mock<ILogger>();
            var entityValidatorFactory = new Mock<IEntityValidatorFactory>();
            IBankTransferService transferService = new BankTransferService();

            var dto = new BankAccountDto
            {
                CustomerId = 0
            };

            var bankingService = new BankAppService(bankAccountRepository.Object, customerRepository.Object, transferService, activityLogService.Object, Logger, entityValidatorFactory.Object);

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

            customerRepository
                .Setup(x => x.GetById(It.IsAny<int>()))
                .Returns((int any) =>
                    {
                        var customer = new Customer
                        {
                            Id = any,
                            FirstName = "John",
                            LastName = "El rojo"
                        };
                        return customer;
                    }
                );

            bankAccountRepository.Setup(x => x.Create(It.IsAny<BankAccount>()));


            var dto = new BankAccountDto
            {
                CustomerId = new Random().Next(),
                BankAccountNumber = "BA"
            };
            IBankAppService bankingService = new BankAppService(bankAccountRepository.Object,
                                                                customerRepository.Object,
                                                                transferService,
                                                                activityLogService.Object,
                                                                Logger,
                                                                EntityValidatorFactory);

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

            var bankAccountNumberSource = new BankAccountNumber("4444", "5555", "3333333333", "02");
            var sourceCustomer = new Customer();
            sourceCustomer.GenerateNewIdentity();

            var source = BankAccountFactory.CreateBankAccount(sourceCustomer, bankAccountNumberSource);
   

            source.DepositMoney(1000, "initial");

            var sourceBankAccountDto = new BankAccountDto
            {
                Id = source.Id,
                BankAccountNumber = source.Iban
            };

            //--> target bank account data
            var targetCustomer = new Customer();
            targetCustomer.GenerateNewIdentity();

            var bankAccountNumberTarget = new BankAccountNumber("1111", "2222", "3333333333", "01");
            var target = BankAccountFactory.CreateBankAccount(targetCustomer, bankAccountNumberTarget);
            //target.ChangeCurrentIdentity(targetId);


            var targetBankAccountDto = new BankAccountDto
            {
                Id = target.Id,
                BankAccountNumber = target.Iban
            };

            var accounts = new List<BankAccount> { source, target };

            var bankAccountRepository = new Mock<IBankAccountRepository>();
            var entityValidatorFactory = new Mock<IEntityValidatorFactory>();

            bankAccountRepository
                .Setup(x => x.GetById(It.IsAny<int>()))
                .Returns((int id) =>
                {
                    return accounts.SingleOrDefault(ba => ba.Id == id);
                });


            var customerRepository = new Mock<ICustomerRepository>();
            var activityLogService = new Mock<IUserActivityService>();
            var logger = new Mock<ILogger>();

            IBankTransferService transferService = new BankTransferService();
            IBankAppService bankingService = new BankAppService(bankAccountRepository.Object, customerRepository.Object, transferService, activityLogService.Object, Logger, entityValidatorFactory.Object);

            //Act
            bankingService.PerformBankTransfer(sourceBankAccountDto, targetBankAccountDto, 100M);

            //Assert
            Assert.Equal(900, source.Balance);
            Assert.Equal(100, target.Balance);
        }
    }
}
