using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnajPos.BAL
{
    class clsBlProduct
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductNameUrdu { get; set; }
        public string Company { get; set; }
        public string Cetegory { get; set; }
        public int Pakaging { get; set; }
        public int PurchaseRate { get; set; }
        public int SaleRate { get; set; }
        //public DateTime OeningDate { get; set; }
        public decimal OpeningStock { get; set; }
        public string Remark { get; set; }
        public int WarehouseId { get; set; }
        public int PurchaseUnitId { get; set; }
        public int SaleUnitId { get; set; }
        public int MeasurementProduct { get; set; }
      

    }
}
