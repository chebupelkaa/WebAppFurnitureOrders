using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using WebAppFurniture.DAL.Data;
using WebAppFurniture.DAL.Interfaces;
using WebAppFurniture.DAL.Repositories;

namespace WebAppFurniture.DAL.Configuration
{
    public static class ConfigurationService
    {
        public static IServiceCollection AddDataAccessServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    builder => builder.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName)).EnableSensitiveDataLogging());

            services.AddScoped<IOrderRepository, OrderRepository>()
                .AddScoped<IClientRepository, ClientRepository>()
                .AddScoped<IParameterRepository, ParameterRepository>()
                .AddScoped<IParameterProductRepository, ParameterProductRepository>()
                .AddScoped<IProductRepository, ProductRepository>()
                .AddScoped<IProviderRepository, ProviderRepository>()
                .AddScoped<IProductGroupRepository, ProductGroupRepository>()
                .AddScoped<IReviewRepository, ReviewRepository>()
                .AddScoped<IWarehouseRepository, WarehouseRepository>()
                .AddScoped<IWarehouseProductRepository, WarehouseProductRepository>()
                .AddScoped<INotificationRepository, NotificationRepository>();

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<ApplicationContext>()
            .AddDefaultTokenProviders();

            return services;
        }
    }
}
