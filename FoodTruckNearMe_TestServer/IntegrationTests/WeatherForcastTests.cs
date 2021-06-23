using FoodTruckNearMe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace FoodTruckNearMe_TestServer.IntegrationTests
{
    public class WeatherForecastTests:  
        IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public WeatherForecastTests(
            CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }
        [Fact]
        public async Task Get_QuoteService_ProvidesQuoteInPage()
        {
            // Arrange
            var client = _factory.WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddScoped<ISomething>(sp =>
                        {
                            var fakeSomething = A.Fake<ISomething>();
                            A.CallTo(() => fakeSomething.GetName()).Returns("fakename");
                            return fakeSomething;
                        });
                    });
                })
                .CreateClient();

            //Act

            var result =  await client.GetFromJsonAsync<List<WeatherForecast>>("/WeatherForecast");
            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count);
        }
    }
}
