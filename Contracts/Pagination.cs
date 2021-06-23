using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public class Pagination
    {
        public UInt16 Limit { get; set; }
        public string NextToken { get; set; }
    }
    public class PaginationResponse
    {
        public bool TotalAvailable { get; set; }
        public UInt64 Total { get; set; }
        public UInt32 Limit { get; set; }
        public string NextToken { get; set; }
    }

}
