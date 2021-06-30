using Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodTruckNearMe.Controllers
{
   
    [ApiController]
    [Route("[controller]")]
    public class FoodTruckServiceController : ControllerBase
    {
        private ISomething _something;
        private IFoodTruckService _foodTruckService;
        private ILogger<FoodTruckServiceController> _logger;

        public FoodTruckServiceController(
              ISomething something,
              IFoodTruckService foodTruckService,
              ILogger<FoodTruckServiceController> logger)
        {
            _something = something;
            _foodTruckService = foodTruckService;
            _logger = logger;
        }
        [HttpPost]
        public async Task<ListFoodTruckPermitsResponse> GetPage(ListFoodTruckPermitsRequest request)
        {
            return await _foodTruckService.ListFoodTruckPermitsAsync(request); 
        }
    }
}
 