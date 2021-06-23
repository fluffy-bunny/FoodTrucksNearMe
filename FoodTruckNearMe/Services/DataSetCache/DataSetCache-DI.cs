using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace FoodTruckNearMe.Services.DataSetCache
{
    public static class DataSetCacheDIExtensions
    {
        public static IServiceCollection AddDataSetCache(this IServiceCollection services)
        {
            services.AddSingleton<IDataSetCache, DataSetCache>();
            return services;
        }
    }
}
