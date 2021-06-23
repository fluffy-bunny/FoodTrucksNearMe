using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using FoodTruckNearMe.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FoodTruckNearMe.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestFileDownload : ControllerBase
    {
        private readonly FileDownloadOptions _options;
        private readonly List<MobileFoodFacilityPermit> _testData;
        private readonly ILogger<TestFileDownload> _logger;

        public TestFileDownload(
            IOptions<FileDownloadOptions> options,
            List<MobileFoodFacilityPermit> testData,
            ILogger<TestFileDownload> logger)
        {
            _options = options.Value;
            _testData = testData;
            _logger = logger;
        }
        [HttpGet("download/{fileId}")]  
        public async Task<ActionResult> DownloadFile(string fileId)
        {
            var bytes = await System.IO.File.ReadAllBytesAsync(_options.FileLocation);
            return File(bytes, "text/plain", Path.GetFileName($"{fileId}"));
        }
    }
}
