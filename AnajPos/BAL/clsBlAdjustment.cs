using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnajPos.BAL
{
    class clsBlAdjustment
    {
        public int Id { get; set; }
        public DateTime TranTime { get; set; }
        public int ZoneId { get; set; }
        public int AddressId { get; set; }
        public int CustomerId { get; set; }
        public int Dr { get; set; }
        public int Cr { get; set; }
        public string Remark { get; set; }
    }
}
