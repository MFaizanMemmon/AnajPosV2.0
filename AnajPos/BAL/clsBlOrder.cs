using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnajPos.BAL
{
    class clsBlOrder
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public int ZoneId { get; set; }
        public int AddressId { get; set; }
        public int CustomerID { get; set; }
        public string PageNo { get; set; }
        public string Bilty { get; set; }
        public string Remark { get; set; }
        public string Transport { get; set; }
        public string EntryType { get; set; }
    }
}
