using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnajPos.BAL_Cash_BANK
{
    public class clsBlBank
    {
        public int? BankId { get; set; }
        public string BankTitle { get; set; }
        public string BankName { get; set; }
        public string BankNameUrdu { get; set; }
        public string AccountNo { get; set; }
        public string Address { get; set; }
        public DateTime OpeningDate { get; set; }
        public decimal OpeningBalance { get; set; }
        public bool IsActive { get; set; }
        public int? AccountId { get; set; }
    }
}
