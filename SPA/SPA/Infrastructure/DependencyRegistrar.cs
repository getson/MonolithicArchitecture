using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using MyApp.Core;
using MyApp.Core.Configuration;
using MyApp.Core.Domain.Common;
using MyApp.Core.Domain.Services.Configuration;
using MyApp.Core.Domain.Services.Events;
using MyApp.Core.Domain.Services.Installation;
using MyApp.Core.Domain.Services.Logging;
using MyApp.Core.Domain.Services.Plugins;
using MyApp.Core.Infrastructure;
using MyApp.Core.Infrastructure.Common;
using MyApp.Core.Interfaces.Caching;
using MyApp.Core.Interfaces.Data;
using MyApp.Core.Plugins;
using MyApp.Infrastructure.Cache;
using MyApp.Infrastructure.Data;
using  MyApp.Core.Plugins;
using MyApp.Services.Helpers;

namespace SPA.Infrastructure
{
    /// <summary>
    /// Dependency registrar
    /// </summary>
    public class DependencyRegistrar : IDependencyRegistrar
    {
        /// <summary>
        /// Register services and interfaces
        /// </summary>
        /// <param name="builder">Container builder</param>
        /// <param name="typeFinder">Type finder</param>
        /// <param name="config">Config</param>
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, MyAppConfig config)
        {
            //file provider
            builder.RegisterType<MyAppFileProvider>().As<IMyAppFileProvider>().InstancePerLifetimeScope();

            //web helper
            builder.RegisterType<WebHelper>().As<IWebHelper>().InstancePerLifetimeScope();

            //user agent helper
            builder.RegisterType<UserAgentHelper>().As<IUserAgentHelper>().InstancePerLifetimeScope();

            //data layer
            builder.RegisterType<EfDataProviderManager>().As<IDataProviderManager>().InstancePerDependency();
            builder.Register(context => context.Resolve<IDataProviderManager>().DataProvider).As<IDataProvider>().InstancePerDependency();
            builder.Register(context => new MyAppObjectContext(context.Resolve<DbContextOptions<MyAppObjectContext>>()))
                .As<IDbContext>().InstancePerLifetimeScope();

            //repositories
            builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();

            //plugins
            builder.RegisterType<PluginFinder>().As<IPluginFinder>().InstancePerLifetimeScope();
            builder.RegisterType<OfficialFeedManager>().As<IOfficialFeedManager>().InstancePerLifetimeScope();

            //cache manager
            builder.RegisterType<PerRequestCacheManager>().As<ICacheManager>().InstancePerLifetimeScope();

            //static cache manager
            if (config.RedisCachingEnabled)
            {
                builder.RegisterType<RedisConnectionWrapper>()
                    .As<ILocker>()
                    .As<IRedisConnectionWrapper>()
                    .SingleInstance();
                builder.RegisterType<RedisCacheManager>().As<IStaticCacheManager>().InstancePerLifetimeScope();
            }
            else
            {
                builder.RegisterType<MemoryCacheManager>()
                    .As<ILocker>()
                    .As<IStaticCacheManager>()
                    .SingleInstance();
            }

            //work context
            builder.RegisterType<WebWorkContext>().As<IWorkContext>().InstancePerLifetimeScope();

            //store context
            //builder.RegisterType<WebStoreContext>().As<IStoreContext>().InstancePerLifetimeScope();
            /*
            //services
            builder.RegisterType<BackInStockSubscriptionService>().As<IBackInStockSubscriptionService>().InstancePerLifetimeScope();
            builder.RegisterType<CategoryService>().As<ICategoryService>().InstancePerLifetimeScope();
            builder.RegisterType<CompareProductsService>().As<ICompareProductsService>().InstancePerLifetimeScope();
            builder.RegisterType<RecentlyViewedProductsService>().As<IRecentlyViewedProductsService>().InstancePerLifetimeScope();
            builder.RegisterType<ManufacturerService>().As<IManufacturerService>().InstancePerLifetimeScope();
            builder.RegisterType<PriceFormatter>().As<IPriceFormatter>().InstancePerLifetimeScope();
            builder.RegisterType<ProductAttributeFormatter>().As<IProductAttributeFormatter>().InstancePerLifetimeScope();
            builder.RegisterType<ProductAttributeParser>().As<IProductAttributeParser>().InstancePerLifetimeScope();
            builder.RegisterType<ProductAttributeService>().As<IProductAttributeService>().InstancePerLifetimeScope();
            builder.RegisterType<ProductService>().As<IProductService>().InstancePerLifetimeScope();
            builder.RegisterType<CopyProductService>().As<ICopyProductService>().InstancePerLifetimeScope();
            builder.RegisterType<SpecificationAttributeService>().As<ISpecificationAttributeService>().InstancePerLifetimeScope();
            builder.RegisterType<ProductTemplateService>().As<IProductTemplateService>().InstancePerLifetimeScope();
            builder.RegisterType<CategoryTemplateService>().As<ICategoryTemplateService>().InstancePerLifetimeScope();
            builder.RegisterType<ManufacturerTemplateService>().As<IManufacturerTemplateService>().InstancePerLifetimeScope();
            builder.RegisterType<TopicTemplateService>().As<ITopicTemplateService>().InstancePerLifetimeScope();
            builder.RegisterType<ProductTagService>().As<IProductTagService>().InstancePerLifetimeScope();
            builder.RegisterType<AddressAttributeFormatter>().As<IAddressAttributeFormatter>().InstancePerLifetimeScope();
            builder.RegisterType<AddressAttributeParser>().As<IAddressAttributeParser>().InstancePerLifetimeScope();
            builder.RegisterType<AddressAttributeService>().As<IAddressAttributeService>().InstancePerLifetimeScope();
            builder.RegisterType<AddressService>().As<IAddressService>().InstancePerLifetimeScope();
            builder.RegisterType<AffiliateService>().As<IAffiliateService>().InstancePerLifetimeScope();
            builder.RegisterType<VendorService>().As<IVendorService>().InstancePerLifetimeScope();
            builder.RegisterType<VendorAttributeFormatter>().As<IVendorAttributeFormatter>().InstancePerLifetimeScope();
            builder.RegisterType<VendorAttributeParser>().As<IVendorAttributeParser>().InstancePerLifetimeScope();
            builder.RegisterType<VendorAttributeService>().As<IVendorAttributeService>().InstancePerLifetimeScope();
            builder.RegisterType<SearchTermService>().As<ISearchTermService>().InstancePerLifetimeScope();
            builder.RegisterType<GenericAttributeService>().As<IGenericAttributeService>().InstancePerLifetimeScope();
            builder.RegisterType<FulltextService>().As<IFulltextService>().InstancePerLifetimeScope();
            builder.RegisterType<MaintenanceService>().As<IMaintenanceService>().InstancePerLifetimeScope();
            builder.RegisterType<CustomerAttributeFormatter>().As<ICustomerAttributeFormatter>().InstancePerLifetimeScope();
            builder.RegisterType<CustomerAttributeParser>().As<ICustomerAttributeParser>().InstancePerLifetimeScope();
            builder.RegisterType<CustomerAttributeService>().As<ICustomerAttributeService>().InstancePerLifetimeScope();
            builder.RegisterType<CustomerService>().As<ICustomerService>().InstancePerLifetimeScope();
            builder.RegisterType<CustomerRegistrationService>().As<ICustomerRegistrationService>().InstancePerLifetimeScope();
            builder.RegisterType<CustomerReportService>().As<ICustomerReportService>().InstancePerLifetimeScope();
            builder.RegisterType<PermissionService>().As<IPermissionService>().InstancePerLifetimeScope();
            builder.RegisterType<AclService>().As<IAclService>().InstancePerLifetimeScope();
            builder.RegisterType<PriceCalculationService>().As<IPriceCalculationService>().InstancePerLifetimeScope();
            builder.RegisterType<GeoLookupService>().As<IGeoLookupService>().InstancePerLifetimeScope();
            builder.RegisterType<CountryService>().As<ICountryService>().InstancePerLifetimeScope();
            builder.RegisterType<CurrencyService>().As<ICurrencyService>().InstancePerLifetimeScope();
            builder.RegisterType<MeasureService>().As<IMeasureService>().InstancePerLifetimeScope();
            builder.RegisterType<StateProvinceService>().As<IStateProvinceService>().InstancePerLifetimeScope();
            builder.RegisterType<StoreService>().As<IStoreService>().InstancePerLifetimeScope();
            builder.RegisterType<StoreMappingService>().As<IStoreMappingService>().InstancePerLifetimeScope();
            builder.RegisterType<DiscountService>().As<IDiscountService>().InstancePerLifetimeScope();
            builder.RegisterType<LocalizationService>().As<ILocalizationService>().InstancePerLifetimeScope();
            builder.RegisterType<LocalizedEntityService>().As<ILocalizedEntityService>().InstancePerLifetimeScope();
            builder.RegisterType<LanguageService>().As<ILanguageService>().InstancePerLifetimeScope();
            builder.RegisterType<DownloadService>().As<IDownloadService>().InstancePerLifetimeScope();
            builder.RegisterType<MessageTemplateService>().As<IMessageTemplateService>().InstancePerLifetimeScope();
            builder.RegisterType<QueuedEmailService>().As<IQueuedEmailService>().InstancePerLifetimeScope();
            builder.RegisterType<NewsLetterSubscriptionService>().As<INewsLetterSubscriptionService>().InstancePerLifetimeScope();
            builder.RegisterType<CampaignService>().As<ICampaignService>().InstancePerLifetimeScope();
            builder.RegisterType<EmailAccountService>().As<IEmailAccountService>().InstancePerLifetimeScope();
            builder.RegisterType<WorkflowMessageService>().As<IWorkflowMessageService>().InstancePerLifetimeScope();
            builder.RegisterType<MessageTokenProvider>().As<IMessageTokenProvider>().InstancePerLifetimeScope();
            builder.RegisterType<Tokenizer>().As<ITokenizer>().InstancePerLifetimeScope();
            builder.RegisterType<EmailSender>().As<IEmailSender>().InstancePerLifetimeScope();
            builder.RegisterType<CheckoutAttributeFormatter>().As<ICheckoutAttributeFormatter>().InstancePerLifetimeScope();
            builder.RegisterType<CheckoutAttributeParser>().As<ICheckoutAttributeParser>().InstancePerLifetimeScope();
            builder.RegisterType<CheckoutAttributeService>().As<ICheckoutAttributeService>().InstancePerLifetimeScope();
            builder.RegisterType<GiftCardService>().As<IGiftCardService>().InstancePerLifetimeScope();
            builder.RegisterType<OrderService>().As<IOrderService>().InstancePerLifetimeScope();
            builder.RegisterType<OrderReportService>().As<IOrderReportService>().InstancePerLifetimeScope();
            builder.RegisterType<OrderProcessingService>().As<IOrderProcessingService>().InstancePerLifetimeScope();
            builder.RegisterType<OrderTotalCalculationService>().As<IOrderTotalCalculationService>().InstancePerLifetimeScope();
            builder.RegisterType<ReturnRequestService>().As<IReturnRequestService>().InstancePerLifetimeScope();
            builder.RegisterType<RewardPointService>().As<IRewardPointService>().InstancePerLifetimeScope();
            builder.RegisterType<ShoppingCartService>().As<IShoppingCartService>().InstancePerLifetimeScope();
            builder.RegisterType<CustomNumberFormatter>().As<ICustomNumberFormatter>().InstancePerLifetimeScope();
            builder.RegisterType<PaymentService>().As<IPaymentService>().InstancePerLifetimeScope();
            builder.RegisterType<EncryptionService>().As<IEncryptionService>().InstancePerLifetimeScope();
            builder.RegisterType<CookieAuthenticationService>().As<IAuthenticationService>().InstancePerLifetimeScope();
            builder.RegisterType<UrlRecordService>().As<IUrlRecordService>().InstancePerLifetimeScope();
            builder.RegisterType<ShipmentService>().As<IShipmentService>().InstancePerLifetimeScope();
            builder.RegisterType<ShippingService>().As<IShippingService>().InstancePerLifetimeScope();
            builder.RegisterType<DateRangeService>().As<IDateRangeService>().InstancePerLifetimeScope();
            builder.RegisterType<TaxCategoryService>().As<ITaxCategoryService>().InstancePerLifetimeScope();
            builder.RegisterType<TaxService>().As<ITaxService>().InstancePerLifetimeScope();
            builder.RegisterType<DefaultLogger>().As<ILogger>().InstancePerLifetimeScope();
            builder.RegisterType<CustomerActivityService>().As<ICustomerActivityService>().InstancePerLifetimeScope();
            builder.RegisterType<ForumService>().As<IForumService>().InstancePerLifetimeScope();
            builder.RegisterType<GdprService>().As<IGdprService>().InstancePerLifetimeScope();
            builder.RegisterType<PollService>().As<IPollService>().InstancePerLifetimeScope();
            builder.RegisterType<BlogService>().As<IBlogService>().InstancePerLifetimeScope();
            builder.RegisterType<WidgetService>().As<IWidgetService>().InstancePerLifetimeScope();
            builder.RegisterType<TopicService>().As<ITopicService>().InstancePerLifetimeScope();
            builder.RegisterType<NewsService>().As<INewsService>().InstancePerLifetimeScope();
            builder.RegisterType<DateTimeHelper>().As<IDateTimeHelper>().InstancePerLifetimeScope();
            builder.RegisterType<SitemapGenerator>().As<ISitemapGenerator>().InstancePerLifetimeScope();
            builder.RegisterType<PageHeadBuilder>().As<IPageHeadBuilder>().InstancePerLifetimeScope();
            builder.RegisterType<ScheduleTaskService>().As<IScheduleTaskService>().InstancePerLifetimeScope();
            builder.RegisterType<ExportManager>().As<IExportManager>().InstancePerLifetimeScope();
            builder.RegisterType<ImportManager>().As<IImportManager>().InstancePerLifetimeScope();
            builder.RegisterType<PdfService>().As<IPdfService>().InstancePerLifetimeScope();
            builder.RegisterType<UploadService>().As<IUploadService>().InstancePerLifetimeScope();
            builder.RegisterType<ThemeProvider>().As<IThemeProvider>().InstancePerLifetimeScope();
            builder.RegisterType<ThemeContext>().As<IThemeContext>().InstancePerLifetimeScope();
            builder.RegisterType<ExternalAuthenticationService>().As<IExternalAuthenticationService>().InstancePerLifetimeScope();
            */
            //builder.RegisterType<RoutePublisher>().As<IRoutePublisher>().SingleInstance();
            builder.RegisterType<EventPublisher>().As<IEventPublisher>().SingleInstance();
            builder.RegisterType<SubscriptionService>().As<ISubscriptionService>().SingleInstance();
            builder.RegisterType<SettingService>().As<ISettingService>().InstancePerLifetimeScope();


            builder.RegisterType<ActionContextAccessor>().As<IActionContextAccessor>().InstancePerLifetimeScope();

            //register all settings
            builder.RegisterSource(new SettingsSource());

            //picture service
            //if (!string.IsNullOrEmpty(config.AzureBlobStorageConnectionString))
            //    builder.RegisterType<AzurePictureService>().As<IPictureService>().InstancePerLifetimeScope();
            //else
            //    builder.RegisterType<PictureService>().As<IPictureService>().InstancePerLifetimeScope();

            //installation service
            if (!DataSettingsManager.DatabaseIsInstalled)
            {
                if (config.UseFastInstallationService)
                    builder.RegisterType<SqlFileInstallationService>().As<IInstallationService>().InstancePerLifetimeScope();
                else
                    builder.RegisterType<CodeFirstInstallationService>().As<IInstallationService>().InstancePerLifetimeScope();
            }

            //event consumers
            var consumers = typeFinder.FindClassesOfType(typeof(IConsumer<>)).ToList();
            foreach (var consumer in consumers)
            {
                builder.RegisterType(consumer)
                    .As(consumer.FindInterfaces((type, criteria) =>
                    {
                        var isMatch = type.IsGenericType && ((Type)criteria).IsAssignableFrom(type.GetGenericTypeDefinition());
                        return isMatch;
                    }, typeof(IConsumer<>)))
                    .InstancePerLifetimeScope();
            }
        }

        /// <summary>
        /// Gets order of this dependency registrar implementation
        /// </summary>
        public int Order
        {
            get { return 0; }
        }
    }


    /// <summary>
    /// Setting source
    /// </summary>
    public class SettingsSource : IRegistrationSource
    {
        static readonly MethodInfo BuildMethod = typeof(SettingsSource).GetMethod(
            "BuildRegistration",
            BindingFlags.Static | BindingFlags.NonPublic);

        /// <summary>
        /// Registrations for
        /// </summary>
        /// <param name="service">Service</param>
        /// <param name="registrations">Registrations</param>
        /// <returns>Registrations</returns>
        public IEnumerable<IComponentRegistration> RegistrationsFor(
            Service service,
            Func<Service, IEnumerable<IComponentRegistration>> registrations)
        {
            var ts = service as TypedService;
            if (ts != null && typeof(ISettings).IsAssignableFrom(ts.ServiceType))
            {
                var buildMethod = BuildMethod.MakeGenericMethod(ts.ServiceType);
                yield return (IComponentRegistration)buildMethod.Invoke(null, null);
            }
        }

        //static IComponentRegistration BuildRegistration<TSettings>() where TSettings : ISettings, new()
        //{
        //    //return RegistrationBuilder
        //    //    .ForDelegate((c, p) =>
        //    //    {
        //    //        //var currentStoreId = c.Resolve<IStoreContext>().CurrentStore.Id;
        //    //        ////uncomment the code below if you want load settings per store only when you have two stores installed.
        //    //        ////var currentStoreId = c.Resolve<IStoreService>().GetAllStores().Count > 1
        //    //        ////    c.Resolve<IStoreContext>().CurrentStore.Id : 0;

        //    //        ////although it's better to connect to your database and execute the following SQL:
        //    //        ////DELETE FROM [Setting] WHERE [StoreId] > 0
        //    //        //return c.Resolve<ISettingService>().LoadSetting<TSettings>(currentStoreId);
        //    //    })
        //    //    .InstancePerLifetimeScope()
        //    //    .CreateRegistration();
        //}

        /// <summary>
        /// Is adapter for individual components
        /// </summary>
        public bool IsAdapterForIndividualComponents { get { return false; } }
    }

}
