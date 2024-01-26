using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnajPos.BAL
{
    class clsBlTransaction
    {
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public int TransactionId { get; set; }
        public int CustomerId { get; set; }
        public string Remark { get; set; }
        public int Dr { get; set; }
        public int Cr { get; set; }
        public DateTime PostingDate { get; set; }
        public string TranTypeUrdu { get; set; }
    }
}
