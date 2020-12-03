using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Configuration.ViewModels
{
    public class MissingStockViewModel
    {
        public long MissingStockId { get; set; }
        public long DescriptionId { get; set; }
        public long ColorId { get; set; }
        public long PartsId { get; set; }
        public string StockType { get; set; }
        public int Quantity { get; set; }
        public long OrganizationId { get; set; }
        public long BranchId { get; set; }
        public string Remarks { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }

        //Custom
        public string ModelName { get; set; }
        public string ColorName { get; set; }
        public string PartsName { get; set; }
    }
}
