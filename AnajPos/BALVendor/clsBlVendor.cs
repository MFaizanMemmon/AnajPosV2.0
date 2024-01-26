using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnajPos.DalVendor
{
    class clsBlVendor
    {
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public int AddressName { get; set; }
        public string ProperAddress { get; set; }
        public byte[] Image { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Telephone { get; set; }
        public string Remark { get; set; }
        public DateTime ClosingDate { get; set; }
        public int OpeningAmount { get; set; }
        public bool IsActive { get; set; }

    }
}
