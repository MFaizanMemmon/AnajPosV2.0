using AnajPos.DalVendor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnajPos.BALVendor
{
    class clsBlPayment : clsBlVendor
    {
        public int PaymentID { get; set; }
        public DateTime PaymentDate { get; set; }
        public int VendorID { get; set; }
        public int HeadID { get; set; }
        public string PayRemark { get; set; }
        public int Amount { get; set; }
        public int PreviousAmount { get; set; }
        public DateTime EntryDate { get; set; }
    }
}
