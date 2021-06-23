using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FoodTruckNearMe.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FoodTruckNearMe.Services.HostedServices
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly BackgroundPermitOptions _options;
        private readonly ILogger<TimedHostedService> _logger;
        private Timer _timer;

        public TimedHostedService(
            IOptions<BackgroundPermitOptions> options,
            ILogger<TimedHostedService> logger)
        {
            _options = options.Value;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(10));

            return Task.CompletedTask;
        }

        private async Task DoPermitDownloadAsync()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    _logger.LogInformation($"Attempt Downloaded: url: {_options.DownloadUrl}");

                    using (var result = await client.GetAsync(_options.DownloadUrl))
                    {
                        if (result.IsSuccessStatusCode)
                        {
                            var bytes = await result.Content.ReadAsByteArrayAsync();
                            _logger.LogInformation(
                                $"Downloaded: {bytes.Length} bytes from url: {_options.DownloadUrl}");
                            _timer?.Change(TimeSpan.FromSeconds(_options.ScheduleSeconds), TimeSpan.Zero);
                            MobileFoodFacilityPermitLoader.LoadMobileFoodFacilityPermitsByBytes(bytes);
                        }
                        else
                        {
                            _logger.LogError(
                                $"Attempt Downloaded: StatusCode: {result.StatusCode}, url: {_options.DownloadUrl} failed");
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in downloaded permits");
            }


        }

        private  void DoWork(object state)
        {
            var count = Interlocked.Increment(ref executionCount);

            _logger.LogInformation(
                "Timed Hosted Service is working. Count: {Count}", count);
            Task.Run(async () => await DoPermitDownloadAsync());
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
