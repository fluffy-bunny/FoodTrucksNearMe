using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FoodTruckNearMe.Configuration;
using FoodTruckNearMe.Services.DataSetCache;
using FoodTruckNearMe.Services.HostedServices;
using FoodTruckNearMe.Services.Something;
using FoodTruckService;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;

namespace FoodTruckNearMe
{
    public class Startup
    {
        private Exception _deferedExecption;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public Startup(IConfiguration configuration, IWebHostEnvironment env )
        {
            Configuration = configuration;
            _webHostEnvironment = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions<BackgroundPermitOptions>().Configure(options =>
            {
                // TODO: These options need to come in from configuration appsettings.{environment}.json
                options.ScheduleSeconds = 60;  // 1 minute
//                options.DownloadUrl = "https://data.sfgov.org/api/views/rqzj-sfat/rows.csv";
                options.DownloadUrl = "http://localhost:5000/TestFileDownload/download/rows.csv";

            });
            // this service will pull new data on a periodic period.
            services.AddHostedService<TimedHostedService>();
            try
            {
                var fileLocation = Path.Combine(_webHostEnvironment.ContentRootPath, "Mobile_Food_Facility_Permit.csv");
                services.AddOptions<FileDownloadOptions>().Configure(options =>
                {
                    options.FileLocation = fileLocation;
                });

                var mobilePermits = MobileFoodFacilityPermitLoader.LoadMobileFoodFacilityPermitsByFile(fileLocation);
                services.AddSingleton(mobilePermits);  // primarily for the integration tests.  For production we will be fetching the real data once a day and caching it
            }
            catch (Exception e)
            {
                // TODO: Introduce a logger buffer to capture logs from ConfigureService because we get our real logger later in the pipeline (Configure(...))
                _deferedExecption = e;
            }
            services.AddSomething();
            services.AddFoodTruckService();
            services.AddDataSetCache();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FoodTruckNearMe", Version = "v1" });
            });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IServiceProvider serviceProvider,
            IApplicationBuilder app, 
            IWebHostEnvironment env, 
            ILogger<Startup> logger)
        {
            if (_deferedExecption != null)
            {
                logger.LogCritical(_deferedExecption, "Exception caught in ConfigureServices");
            }

            var server = serviceProvider.GetRequiredService<IServer>();
            var addressFeature = server.Features.Get<IServerAddressesFeature>();
            foreach (var address in addressFeature.Addresses)
            {
                logger.LogInformation("Listing on address: " + address);
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FoodTruckNearMe v1"));
            }

            
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
