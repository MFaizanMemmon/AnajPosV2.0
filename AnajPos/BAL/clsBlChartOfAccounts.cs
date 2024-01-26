using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnajPos.BAL
{
    class clsBlChartOfAccounts
    {
		
		public int? AccountId { get; set; }
		public string AccountName { get; set; }
		public int? ParentId { get; set; }
		public string Description { get; set; }
		public decimal? Dr { get; set; }
		public decimal? Cr { get; set; }
		public bool? IsActive { get; set; }
		public bool? IsEditable { get; set; }
		public bool? IsChildable { get; set; }

	}
}
