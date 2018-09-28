using MyApp.Domain.Example.CustomerAgg;
using MyApp.Domain.Example.OrderAgg;
using MyApp.Domain.Example.ProductAgg;
using MyApp.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MyApp.Services.Tests.Adapters
{
    public class OrderAdapterTests : TestsInitialize
    {
        [Fact]
        public void OrderToOrderDtoAdapter()
        {
            //Arrange

            var customer = new Customer
            {
                FirstName = "Unai",
                LastName = "Zorrilla"
            };
            customer.GenerateNewIdentity();
            Product product = new Software("the product title", "the product description", "license code");
            product.GenerateNewIdentity();

            var order = new Order
            {
                OrderDate = DateTime.Now,
                ShippingInformation = new ShippingInfo("shippingName", "shippingAddress", "shippingCity", "shippingZipCode")
            };
            order.GenerateNewIdentity();
            order.SetTheCustomerForThisOrder(customer);

            var orderLine = order.AddNewOrderLine(product.Id, 10, 10, 0.5M);
            orderLine.SetProduct(product);

            //Act
            var orderDto = TypeAdapter.Adapt<Order, OrderDto>(order);

            //Assert
            Assert.Equal(orderDto.Id, order.Id);
            Assert.Equal(orderDto.OrderDate, order.OrderDate);
            Assert.Equal(orderDto.DeliveryDate, order.DeliveryDate);

            Assert.Equal(orderDto.ShippingAddress, order.ShippingInformation.ShippingAddress);
            Assert.Equal(orderDto.ShippingCity, order.ShippingInformation.ShippingCity);
            Assert.Equal(orderDto.ShippingName, order.ShippingInformation.ShippingName);
            Assert.Equal(orderDto.ShippingZipCode, order.ShippingInformation.ShippingZipCode);

            Assert.Equal(orderDto.CustomerFullName, order.Customer.FullName);
            Assert.Equal(orderDto.CustomerId, order.Customer.Id);

            Assert.Equal(orderDto.OrderNumber, $"{order.OrderDate.Year}/{order.OrderDate.Month}-{order.SequenceNumberOrder}");


            Assert.NotNull(orderDto.OrderLines);
            Assert.True(orderDto.OrderLines.Any());

            Assert.Equal(orderDto.OrderLines[0].Id, orderLine.Id);
            Assert.Equal(orderDto.OrderLines[0].Amount, orderLine.Amount);
            Assert.Equal(orderDto.OrderLines[0].Discount, orderLine.Discount * 100);
            Assert.Equal(orderDto.OrderLines[0].UnitPrice, orderLine.UnitPrice);
            Assert.Equal(orderDto.OrderLines[0].TotalLine, orderLine.TotalLine);
            Assert.Equal(orderDto.OrderLines[0].ProductId, product.Id);
            Assert.Equal(orderDto.OrderLines[0].ProductTitle, product.Title);

        }

        [Fact]
        public void EnumerableOrderToOrderListDtoAdapter()
        {
            //Arrange

            var customer = new Customer
            {
                FirstName = "Unai",
                LastName = "Zorrilla"
            };
            customer.GenerateNewIdentity();

            Product product = new Software("the product title", "the product description", "license code");
            product.GenerateNewIdentity();


            var order = new Order
            {
                OrderDate = DateTime.Now,
                ShippingInformation = new ShippingInfo("shippingName", "shippingAddress", "shippingCity", "shippingZipCode")
            };
            order.GenerateNewIdentity();
            order.SetTheCustomerForThisOrder(customer);

            var line = order.AddNewOrderLine(product.Id, 1, 200, 0);


            var orders = new List<Order> { order };

            //Act

            var orderListDto = TypeAdapter.Adapt<IEnumerable<Order>, List<OrderListDto>>(orders);

            //Assert
            Assert.Equal(orderListDto[0].Id, order.Id);
            Assert.Equal(orderListDto[0].OrderDate, order.OrderDate);
            Assert.Equal(orderListDto[0].DeliveryDate, order.DeliveryDate);
            Assert.Equal(orderListDto[0].TotalOrder, order.GetOrderTotal());

            Assert.Equal(orderListDto[0].ShippingAddress, order.ShippingInformation.ShippingAddress);
            Assert.Equal(orderListDto[0].ShippingCity, order.ShippingInformation.ShippingCity);
            Assert.Equal(orderListDto[0].ShippingName, order.ShippingInformation.ShippingName);
            Assert.Equal(orderListDto[0].ShippingZipCode, order.ShippingInformation.ShippingZipCode);

            Assert.Equal(orderListDto[0].CustomerFullName, order.Customer.FullName);
            Assert.Equal(orderListDto[0].CustomerId, order.Customer.Id);
        }
    }
}
