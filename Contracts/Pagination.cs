using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public class Pagination
    {
        public UInt16 Limit;
        public string NextToken;
    }
    public class PaginationResponse
    {
        public bool TotalAvailable;
        public UInt64 Total;
        public UInt32 Limit;
        public string NextToken;
    }
}
