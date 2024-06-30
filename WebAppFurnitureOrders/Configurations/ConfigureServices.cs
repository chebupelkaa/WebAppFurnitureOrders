namespace WebAppFurnitureOrders.Configurations
{
    public static class ConfigureServices
    {
        public static IServiceCollection ConfigureMVC(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Program).Assembly);

            return services;
        }
    }
}
