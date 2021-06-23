using Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace FoodTruckService
{
    public static class DIExtensions
    {
        public static IServiceCollection AddFoodTruckService(this IServiceCollection services)
        {
            services.AddSingleton<IFoodTruckService, FoodTruckService>();
            return services;
        }
    }
}
