using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Geolocation;

namespace Contracts
{
    public class DistanceFromOrginRecord
    {
        public double Distance { get; set; }
        public MobileFoodFacilityPermit Permit { get; set; }
    }
    public class ListFoodTruckPermitsResponse
    {
        public Coordinate Origin { get; set; }
        public PaginationResponse PaginationResponse { get; set; }
        public List<DistanceFromOrginRecord> MobileFoodFacilityPermits { get; set; }
    }

    /// <summary>
    /// ListFoodTruckPermitsFilter allows a call to list food trucks to return a select subset
    /// </summary>
    public class ListFoodTruckPermitsFilter
    {
        public Coordinate Origin { get; set; }
        public string Status { get; set; }
    }
    public class ListFoodTruckPermitsRequest
    {
        public ListFoodTruckPermitsFilter Filter { get; set; }
        public Pagination Pagination { get; set; }
    }
    public interface IFoodTruckService
    {
        Task<ListFoodTruckPermitsResponse> ListFoodTruckPermitsAsync(ListFoodTruckPermitsRequest request);
    }
}