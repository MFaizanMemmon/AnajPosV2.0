using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnajPos.BAL_Cash_BANK
{
    class clsBlExpense
    {
        public int ExpenseId { get; set; }
        public DateTime ExpenseDate { get; set; }
        public int ExpenseTypeID { get; set; }
        public int PaymentModeID { get; set; }
        public decimal? Amount { get; set; }
        public string Notes { get; set; }
    }
}
