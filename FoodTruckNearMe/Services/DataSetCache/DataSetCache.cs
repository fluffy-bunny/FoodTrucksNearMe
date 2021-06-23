using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;

namespace FoodTruckNearMe.Services.DataSetCache
{
    public class DataSetCache: IDataSetCache
    {
        public List<MobileFoodFacilityPermit> GetCurrentDataSet()
        {
            return MobileFoodFacilityPermitLoader.GetCurrentDataSet();
        }
    }
}
