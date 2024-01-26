using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnajPos.BAL
{
    class clsBlHeadOfAcc
    {
        public int HeadId { get; set; }
        public string HeadAccName { get; set; }
        public string HeadAccNameUrdu { get; set; }
        public string Remark { get; set; }
        public DateTime OpeningDate { get; set; }
        public int OpeningAmount { get; set; }
    }
}
