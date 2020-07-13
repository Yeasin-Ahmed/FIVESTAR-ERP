using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.FrontDesk.ViewModels
{
   public class DashbordTsPartsReturnViewModel
    {
        public long JobOrderId { get; set; }
        public long ReturnInfoId { get; set; }
        public string JobOrderCode { get; set; }
        public string RequsitionCode { get; set; }
        public string StateStatus { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
    }
}
