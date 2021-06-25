using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using Contracts;
using Geolocation;
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
            // TODO: We need a validation func here to make sure everything that is comming in is legit.
            // fail fast if nonsense is passed in.

            var dataSet = _dataSetCache.GetCurrentDataSet();
            if (dataSet == null)
            {
                _logger.LogError("No food truck data is available");
                return null;
            }

            if (string.IsNullOrEmpty(request.Pagination.NextToken))
            {
                request.Pagination.NextToken = "0";
            }

            
            var skip = int.Parse(request.Pagination.NextToken);
            

            IEnumerable<DistanceFromOrginRecord> query;
            if (!string.IsNullOrEmpty(request.Filter.Status))
            {
                query = from c in dataSet
                        where c.Status == request.Filter.Status
                        let dfo = new DistanceFromOrginRecord
                        {
                            Permit = c,
                            Distance = CalculateDistance(request.Filter.Origin, new Coordinate
                            {
                                Longitude = c.Longitude,
                                Latitude = c.Latitude
                            })
                        }
                        select dfo;
            }
            else
            {
                query = from c in dataSet
                        let dfo = new DistanceFromOrginRecord
                        {
                            Permit = c,
                            Distance = CalculateDistance(request.Filter.Origin, new Coordinate
                            {
                                Longitude = c.Longitude,
                                Latitude = c.Latitude
                            })
                        }
                        select dfo;
            }


            var pageSet = query.Skip(skip).Take((int)request.Pagination.Limit).OrderBy(x => x.Distance);
            var total = query.Count();
            var results = pageSet.ToList();
            var done = false;
            if ((total - skip) <= request.Pagination.Limit)
            {
                done = true;
            }

            return new ListFoodTruckPermitsResponse()
            {
                Origin = request.Filter.Origin,
                MobileFoodFacilityPermits = results,
                PaginationResponse = new PaginationResponse()
                {
                    TotalAvailable = true,
                    Total = (uint)total,
                    Limit = (uint)results.Count,
                    NextToken = $"{skip + request.Pagination.Limit}",
                    Done = done
                }
            };

        }

        private double CalculateDistance(Coordinate origin, Coordinate destination)
        {
            double distance = GeoCalculator.GetDistance(origin, destination, 1);
            return distance;
        }
    }
}
