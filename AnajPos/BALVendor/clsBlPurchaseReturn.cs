using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnajPos.BALVendor
{
    class clsBlPurchaseReturn
    {
        public int ReturnId { get; set; }
        public int VendorId { get; set; }
        public DateTime ReturnDate { get; set; }
        public string Remark { get; set; }
        public int Amount { get; set; }
        public bool IsLedger { get; set; }
        public int ZoneId { get; set; }
        public int AreaId { get; set; }

    }
}
