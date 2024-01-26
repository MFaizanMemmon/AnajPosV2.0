using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnajPos.BAL_Cash_BANK
{
    class clsBlinventoryTransfer
    {
        public int InventoryTransferID { get; set; }

        public DateTime TransferDate { get; set; }
      
        public int ProductId { get; set; }
        public int FromLocationID { get; set; }
        public int ToLocationID { get; set; }
        public decimal transferQty { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedDate { get; set; }



    }
}
