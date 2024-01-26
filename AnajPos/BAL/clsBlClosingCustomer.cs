using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnajPos.BAL
{
    class clsBlClosingCustomer
    {
        public DateTime ClosingDate { get; set; }
        public int CustomerId { get; set; }
        public string Remark { get; set; }
        public int Amount { get; set; }
    }
}
