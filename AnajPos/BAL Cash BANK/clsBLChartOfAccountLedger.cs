using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnajPos.BAL_Cash_BANK
{
    class clsBLChartOfAccountLedger
    {
        public int LedgerId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public int TransactionId { get; set; }
        public int ChartOfAccountId { get; set; }
        public string Remark { get; set; }
        public decimal Dr { get; set; }
        public decimal Cr { get; set; }

    }
}
