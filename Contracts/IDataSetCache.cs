using System.Collections.Generic;

namespace Contracts
{
    public interface IDataSetCache
    {
        List<MobileFoodFacilityPermit> GetCurrentDataSet();
    }
}