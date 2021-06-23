using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace FoodTruckNearMe.Services.Something
{
    public static class SomethingDIExtensions
    {
        public static IServiceCollection AddSomething(this IServiceCollection services)
        {
            services.AddScoped<ISomething, Something>();
            return services;
        }
    }
}
