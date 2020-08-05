using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.FrontDesk.ViewModels
{
   public class InvoiceUsedPartsViewModel
    {
        public long JobOrderId { get; set; }
        public long PartsId { get; set; }
        public string MobilePartName { get; set; }
        public int UsedQty { get; set; }
        public double Price { get; set; }
        public double Total { get; set; }
    }
}
