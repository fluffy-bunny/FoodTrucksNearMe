using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using FakeItEasy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FoodTruckNearMe_TestServer
{
    public class CustomWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddScoped<ISomething>(sp =>
                {
                    var fakeSomething = A.Fake<ISomething>();
                    A.CallTo(() => fakeSomething.GetName()).Returns("custom fake name");
                    return fakeSomething;
                });
                var sp = services.BuildServiceProvider();
            });
        }
    }
}
