using AutoMapper;
using MyApp.Core.Domain.Example.BankAccountAgg;
using MyApp.Core.Domain.Example.CountryAgg;
using MyApp.Core.Domain.Example.CustomerAgg;
using MyApp.Core.Domain.Example.OrderAgg;
using MyApp.Core.Domain.Example.ProductAgg;
using MyApp.Core.Interfaces.Mapping;
using MyApp.Mapping.DTOs;

namespace MyApp.Dto.Profiles
{
    public class ExampleProfile : Profile, IMapperProfile
    {
        public ExampleProfile()
        {
            //bankAccount => BankAccountDTO
            CreateMap<BankAccount, BankAccountDto>()
                .ForMember(dto => dto.BankAccountNumber, mc => mc.MapFrom(e => e.Iban))
                .PreserveReferences();

            //bankAccountActivity=>bankaccountactivityDTO
            CreateMap<BankAccountActivity, BankActivityDto>()
                .PreserveReferences();
            //book => book dto
            CreateMap<Book, BookDto>();

            //country => countrydto
            CreateMap<Country, CountryDto>();

            //customer => customerlistdto
            CreateMap<Customer, CustomerListDto>();

            //customer => customerdto
            var customerMappingExpression = CreateMap<Customer, CustomerDto>();

            //order => orderlistdto
            var orderListMappingExpression = CreateMap<Order, OrderListDto>();
            orderListMappingExpression.ForMember(dto => dto.TotalOrder, mc => mc.MapFrom(e => e.GetOrderTotal()));
            orderListMappingExpression.ForMember(dto => dto.ShippingAddress, mc => mc.MapFrom(e => e.ShippingInformation.ShippingAddress));
            orderListMappingExpression.ForMember(dto => dto.ShippingCity, mc => mc.MapFrom(e => e.ShippingInformation.ShippingCity));
            orderListMappingExpression.ForMember(dto => dto.ShippingName, mc => mc.MapFrom(e => e.ShippingInformation.ShippingName));
            orderListMappingExpression.ForMember(dto => dto.ShippingZipCode, mc => mc.MapFrom(e => e.ShippingInformation.ShippingZipCode));

            //order => orderdto
            var orderMappingExpression = CreateMap<Order, OrderDto>();

            orderMappingExpression.ForMember(dto => dto.ShippingAddress, map => map.MapFrom(o => o.ShippingInformation.ShippingAddress));
            orderMappingExpression.ForMember(dto => dto.ShippingCity, map => map.MapFrom(o => o.ShippingInformation.ShippingCity));
            orderMappingExpression.ForMember(dto => dto.ShippingName, map => map.MapFrom(o => o.ShippingInformation.ShippingName));
            orderMappingExpression.ForMember(dto => dto.ShippingZipCode, map => map.MapFrom(o => o.ShippingInformation.ShippingZipCode));

            //orderline => orderlinedto
            var lineMapperExpression = CreateMap<OrderLine, OrderLineDto>();
            lineMapperExpression.ForMember(dto => dto.Discount, mc => mc.MapFrom(o => o.Discount * 100));

            //product => productdto
            CreateMap<Product, ProductDto>();

            //software => softwaredto
            CreateMap<Software, SoftwareDto>();

        }

        public int Order => 11;
    }
}
