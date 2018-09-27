using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MyApp.Services.DTOs;
using MyApp.Services.Example;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyApp.Spa.Controllers
{
     /// <summary>
     /// Orders API
     /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class OrdersController : Controller
    {
        private readonly ISalesAppService _salesAppService;
        public OrdersController(ISalesAppService salesAppService)
        {
            //_salesAppService = salesAppService;
        }

        // GET: api/orders/?pageIndex=1&pageCount=1
        [HttpGet]
        public IEnumerable<OrderListDto> Get(int pageIndex, int pageCount)
        {
            return _salesAppService.FindOrders(pageIndex, pageCount);
        }

        // GET: api/orders/?from=20170101&to=20170131
        [HttpGet("GetBetweenDates")]
        public IEnumerable<OrderListDto> Get(DateTime from, DateTime to)
        {
            return _salesAppService.FindOrders(from, to);
        }

        // GET api/orders/customers/1
        [HttpGet("[action]/{id}")]
        public IEnumerable<OrderListDto> Customers(int customerId)
        {
            return _salesAppService.FindOrders(customerId);
        }

        // POST api/orders
        [HttpPost]
        public OrderDto Post(OrderDto order)
        {
            return _salesAppService.AddNewOrder(order);
        }

        // POST api/orders/software
        [HttpPost("Software")]
        public SoftwareDto Software(SoftwareDto software)
        {
            return _salesAppService.AddNewSoftware(software);
        }

        // POST api/orders/book
        [HttpPost("Book")]
        public BookDto Book(BookDto book)
        {
            return _salesAppService.AddNewBook(book);
        }

        // GET: api/orders/getpagedproducts/?pageIndex=1&pageCount=1
        [HttpGet("GetPagedProducts")]
        public IEnumerable<ProductDto> Products(int pageIndex, int pageCount)
        {
            return _salesAppService.FindProducts(pageIndex, pageCount);
        }

        // GET: api/orders/getproducts?filter=filter
        [HttpGet("GetProducts")]
        public IEnumerable<ProductDto> Products(string filter)
        {
            return _salesAppService.FindProducts(filter);
        }
    }
}
