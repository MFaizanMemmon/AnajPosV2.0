using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnajPos.BAL
{
    class clsBlCustomer
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerNameUrdu { get; set; }
        public int ZoneId { get; set; }
        public int AddressId { get; set; }
        public string ProperAddress { get; set; }
        public byte[] Picture { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Telephone { get; set; }
        public string Remark { get; set; }
        public DateTime ClosingDate { get; set; }
        public int OpeningAmount { get; set; }
        public bool IsActive { get; set; }
        
    }
}
