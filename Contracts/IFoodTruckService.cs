using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Geolocation;

namespace Contracts
{
    public class ListFoodTruckPermitsResponse
    {
        public PaginationResponse PaginationResponse { get; set; }
        public List<MobileFoodFacilityPermit> MobileFoodFacilityPermits { get; set; }
    }

    /// <summary>
    /// ListFoodTruckPermitsFilter allows a call to list food trucks to return a select subset
    /// </summary>
    public class ListFoodTruckPermitsFilter
    {
        public Coordinate Origin { get; set; }
        public string Status { get; set; }
    }

    public interface IFoodTruckService
    {
        Task<IEnumerable<ListFoodTruckPermitsResponse>> ListFoodTruckPermitsAsync(Pagination pagination);
    }
}