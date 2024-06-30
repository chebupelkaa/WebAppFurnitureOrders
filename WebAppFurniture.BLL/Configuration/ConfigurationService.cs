using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using WebAppFurniture.DAL.Configuration;
using WebAppFurniture.BLL.Interfaces;
using WebAppFurniture.BLL.Services;

namespace WebAppFurniture.BLL.Configuration
{
    public static class ConfigurationService
    {
        public static void ConfigureBLL(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDataAccessServices(configuration);

            services.AddScoped<IClientService, ClientService>()
                .AddScoped<IOrderService, OrderService>()
                .AddScoped<IParameterService, ParameterService>()
                .AddScoped<IParameterProductService, ParameterProductService>()
                .AddScoped<IProductService, ProductService>()
                .AddScoped<IProductGroupService, ProductGroupService>()
                .AddScoped<IProviderService, ProviderService>()
                .AddScoped<IReviewService, ReviewService>()
                .AddScoped<IWarehouseService, WarehouseService>()
                .AddScoped<IWarehouseProductService, WarehouseProductService>()
                .AddScoped<INotificationService, NotificationService>();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }
    }
}
