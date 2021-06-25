using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public class Pagination
    {
        public uint Limit { get; set; }
        public string NextToken { get; set; }
    }
    public class PaginationResponse
    {
        public bool TotalAvailable { get; set; }
        public uint Total { get; set; }
        public uint Limit { get; set; }
        public string NextToken { get; set; }
        public bool Done { get; set; }
    }

}
