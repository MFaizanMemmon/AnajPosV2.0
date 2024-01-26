using AnajPos.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnajPos.BAL
{
    class clsBlReceipt
    {
        public int RecId { get; set; }
        public DateTime RecDate { get; set; }
        public int Zone { get; set; }
        public int Area { get; set; }
        public int Customer { get; set; }
        public int Head { get; set; }
        public string AmountInEnglish { get; set; }
        public string Remark { get; set; }
        public int Amount { get; set; }
        public int PreviousAmount { get; set; }
        public string LastBill { get; set; }
        public string LastReceipt { get; set; }
        public int CurrentBalance { get; set; }
        //public DateTime EntryDate { get; set; }
    }
}
