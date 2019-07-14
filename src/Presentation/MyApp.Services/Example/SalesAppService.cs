using System;
using System.Collections.Generic;
using System.Linq;
using MyApp.Core.Abstractions.Validator;
using MyApp.Core.Common;
using MyApp.Core.Exceptions;
using MyApp.Core.Extensions;
using MyApp.Domain.Example.CustomerAgg;
using MyApp.Domain.Example.OrderAgg;
using MyApp.Domain.Example.ProductAgg;
using MyApp.Domain.Logging;
using MyApp.Services.DTOs;
using MyApp.Services.Logging;

namespace MyApp.Services.Example
{
    public class SalesAppService : ISalesAppService
    {
        #region Members

        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger _logger;
        private readonly IEntityValidatorFactory _validatorFactory;

        #endregion

        #region Constructor

        /// <summary>
        /// Create a new instance of sales management service
        /// </summary>
        /// <param name="productRepository">The associated product repository</param>
        /// <param name="orderRepository">The associated order repository</param>
        /// <param name="customerRepository">The associated customer repository</param>
        /// <param name="logger"></param>
        /// <param name="validatorFactory"></param>
        public SalesAppService(IProductRepository productRepository,//associated product repository
                               IOrderRepository orderRepository,//associated order repository
                               ICustomerRepository customerRepository,//the associated customer repository
                               ILogger logger,
                               IEntityValidatorFactory validatorFactory)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _customerRepository = customerRepository;

            _logger = logger;
            _validatorFactory = validatorFactory;
        }
        #endregion

        #region ISalesAppService Members

        public List<OrderListDto> FindOrders(int pageIndex, int pageCount)
        {
            if (pageIndex < 0 || pageCount <= 0)
                throw new ArgumentException("warning_InvalidArgumentForFindOrders");

            //recover orders in paged fashion
            var ordersQuery = _orderRepository.TableNoTracking.OrderBy(o => o.OrderDate);
            var orders = new PagedList<Order>(ordersQuery, pageIndex, pageCount);

            return orders.Any() ? orders.ProjectedAsCollection<OrderListDto>() : null;
        }
        public List<OrderListDto> FindOrders(DateTime? dateFrom, DateTime? dateTo)
        {
            //create the specification ( how to filter orders from dates..)
            var spec = OrdersSpecifications.OrderFromDateRange(dateFrom, dateTo);

            //recover orders
            var orders = _orderRepository.AllMatching(spec).ToList();

            return orders.Any() ? orders.ProjectedAsCollection<OrderListDto>() : null;
        }

        public List<OrderListDto> FindOrders(int customerId)
        {
            var orders = _orderRepository.GetFiltered(o => o.CustomerId == customerId);

            var filterdOrders = orders as IList<Order> ?? orders.ToList();
            return filterdOrders.Any() ? filterdOrders.ProjectedAsCollection<OrderListDto>() : null;

        }

        public List<ProductDto> FindProducts(int pageIndex, int pageCount)
        {
            if (pageIndex < 0 || pageCount <= 0)
                throw new ArgumentException("warning_InvalidArgumentForFindProducts");

            //recover products
            var products = new PagedList<Product>(_productRepository.TableNoTracking, pageIndex, pageCount);
            return products.Any() ? products.ProjectedAsCollection<ProductDto>() : null;
        }

        public List<ProductDto> FindProducts(string text)
        {
            //create the specification ( howto find products for any string ) 
            var spec = ProductSpecifications.ProductFullText(text);

            //recover products
            var products = _productRepository.AllMatching(spec);

            //adapt results
            return products.ProjectedAsCollection<ProductDto>();
        }

        public OrderDto AddNewOrder(OrderDto orderDto)
        {
            //if orderdto data is not valid
            if (orderDto == null || orderDto.CustomerId == 0)
                throw new ArgumentException("warning_CannotAddOrderWithNullInformation");

            var customer = _customerRepository.GetById(orderDto.CustomerId);

            if (customer != null)
            {
                //Create a new order entity
                var newOrder = CreateNewOrder(orderDto, customer);

                if (newOrder.IsCreditValidForOrder()) //if total order is less than credit 
                {
                    //save order
                    SaveOrder(newOrder);

                    return newOrder.ProjectedAs<OrderDto>();
                }
                _logger.InsertLog(LogLevel.Warning, "info_OrderTotalIsGreaterCustomerCredit");
                return null;
            }
            _logger.InsertLog(LogLevel.Warning, "warning_CannotCreateOrderForNonExistingCustomer");
            return null;
        }
        public SoftwareDto AddNewSoftware(SoftwareDto softwareDto)
        {
            if (softwareDto == null)
                throw new ArgumentException("warning_CannotAddSoftwareWithNullInformation");

            //Create the softare entity
            var newSoftware = new Software(softwareDto.Title, softwareDto.Description, softwareDto.LicenseCode);

            //set unit price and stock
            newSoftware.ChangeUnitPrice(softwareDto.UnitPrice);
            newSoftware.IncrementStock(softwareDto.AmountInStock);

            //Assign the poid
            //newSoftware.GenerateNewIdentity();

            //save software
            SaveProduct(newSoftware);

            //return software dto
            return newSoftware.ProjectedAs<SoftwareDto>();
        }
        public BookDto AddNewBook(BookDto bookDto)
        {
            if (bookDto == null)
                throw new ArgumentNullException("warning_CannotAddSoftwareWithNullInformation");

            //Create the book entity
            var newBook = new Book(bookDto.Title, bookDto.Description, bookDto.Publisher, bookDto.Isbn);

            //set stock and unit price
            newBook.IncrementStock(bookDto.AmountInStock);
            newBook.ChangeUnitPrice(bookDto.UnitPrice);

            //Assign the poid
            //newBook.GenerateNewIdentity();

            //save software
            SaveProduct(newBook);

            //return software dto
            return newBook.ProjectedAs<BookDto>();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            //dispose all resources
            //_orderRepository.Dispose();
            //_productRepository.Dispose();
            //_customerRepository.Dispose();
        }

        #endregion

        #region Private Methods

        private void SaveOrder(Order order)
        {
            var entityValidator = _validatorFactory.Create();

            if (entityValidator.IsValid(order))//if entity is valid save. 
            {
                //add order and commit changes
                _orderRepository.Create(order);
            }
            else // if not valid throw validation errors
                throw new ApplicationValidationErrorsException(entityValidator.GetInvalidMessages(order));
        }

        private Order CreateNewOrder(OrderDto dto, Customer associatedCustomer)
        {
            //Create a new order entity from factory
            var newOrder = OrderFactory.CreateOrder(associatedCustomer,
                                                     dto.ShippingName,
                                                     dto.ShippingCity,
                                                     dto.ShippingAddress,
                                                     dto.ShippingZipCode);

            //if have lines..add
            if (dto.OrderLines != null)
            {
                foreach (var line in dto.OrderLines) //add order lines
                    newOrder.AddNewOrderLine(line.ProductId, line.Amount, line.UnitPrice, line.Discount / 100);
            }

            return newOrder;
        }

        private void SaveProduct(Product product)
        {
            var entityValidator = _validatorFactory.Create();

            if (entityValidator.IsValid(product)) // if is valid
            {
                _productRepository.Create(product);
                //_productRepository.UnitOfWork.Commit();
            }
            else //if not valid, throw validation errors
                throw new ApplicationValidationErrorsException(entityValidator.GetInvalidMessages(product));
        }

        #endregion
    }
}
