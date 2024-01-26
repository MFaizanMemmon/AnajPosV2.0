using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnajPos.BALVendor
{
    class clsBlVendorAdjustment
    {

        public int AdjustmentId { get; set; }
        public DateTime AdjustmentDate { get; set; }
        public int VendorId { get; set; }
        public int Dr { get; set; }
        public int Cr { get; set; }
        public string Notes { get; set; }
          
    }
}
