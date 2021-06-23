using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using Microsoft.Extensions.Logging;

namespace FoodTruckService
{
    public class FoodTruckService : IFoodTruckService
    {
        private IDataSetCache _dataSetCache;
        private ILogger<FoodTruckService> _logger;

        public FoodTruckService(
            IDataSetCache dataSetCache,
            ILogger<FoodTruckService> logger)
        {
            _dataSetCache = dataSetCache;
            _logger = logger;
        }
        public async Task<IEnumerable<ListFoodTruckPermitsResponse>> ListFoodTruckPermitsAsync(Pagination pagination)
        {
            var dataSet = _dataSetCache.GetCurrentDataSet();
            if (dataSet == null)
            {
                _logger.LogError("No food truck data is available");
                return null;
            }


        }
    }
}
