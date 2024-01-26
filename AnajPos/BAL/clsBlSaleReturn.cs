using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnajPos.BAL
{
    class clsBlSaleReturn
    {
        public int ReturnId { get; set; }
        public int ZoneId { get; set; }
        public int AddressId { get; set; }
        public int CustomerId { get; set; }
        public DateTime ReturnDate { get; set; }
        public string Remark { get; set; }
        public int Amount { get; set; }

    }

    class clsBlReturnProcduct
    {
        public int ReturnId { get; set; }
        public DateTime ReturnDate { get; set; }
        public int CustomerVendorAccountId { get; set; }
        public int ProductId { get; set; }
        public int Qty { get; set; }
        public string ProductName { get; set; }
        public float Kg { get; set; }
        public float Weight { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }
    }
}
