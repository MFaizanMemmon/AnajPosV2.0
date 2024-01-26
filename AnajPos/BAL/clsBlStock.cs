using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnajPos.BAL
{
    class clsBlStock
    {
        public int StockId { get; set; }
        public DateTime ProductTransDate { get; set; }
        public int ProductTransId { get; set; }
        public int ProductId { get; set; }

        public int UnitID { get; set; }
        public string TransactionType { get; set; }
        public int WarehouseId { get; set; }
        public decimal StockIn { get; set; }
        public decimal StockOut { get; set; }
        public decimal Amount { get; set; }
        public bool IsDeleted { get; set; }
        public string Remarks { get; set; }

        //_____ SAQIB _____ 1/13/2024
        //______ SP Get ________
        #region SP_Return
        public string ProductName { get; set; }
        public string WarehouseName { get; set; }
        #endregion

    }
}
