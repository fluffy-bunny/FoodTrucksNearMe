using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
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
        public async Task<ListFoodTruckPermitsResponse> ListFoodTruckPermitsAsync(ListFoodTruckPermitsRequest request)
        {
            var dataSet = _dataSetCache.GetCurrentDataSet();
            if (dataSet == null)
            {
                _logger.LogError("No food truck data is available");
                return null;
            }

            if (string.IsNullOrEmpty(request.Pagination.NextToken))
            {
                request.Pagination.NextToken = $"0:{request.Pagination.Limit}";
            }

            var tokenParts = request.Pagination.NextToken.Split(':');
            var skip = int.Parse(tokenParts[0]);
            var take = int.Parse(tokenParts[1]);

            var query = from c in dataSet
                        select c;
            var pageSet = query.Skip(skip).Take(take);
            var total = query.Count();
            var results = pageSet.ToList();
            return new ListFoodTruckPermitsResponse()
            {
                MobileFoodFacilityPermits = results,
                PaginationResponse = new PaginationResponse()
                {
                    TotalAvailable = true,
                    Total = (ulong)total,
                    Limit = (uint)results.Count,
                    NextToken = $"{skip + take}:{take}"

                }
            };

        }
    }
}
