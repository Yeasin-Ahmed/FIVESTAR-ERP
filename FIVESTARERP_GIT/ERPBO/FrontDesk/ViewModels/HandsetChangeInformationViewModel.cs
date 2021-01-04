using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.FrontDesk.ViewModels
{
   public class HandsetChangeInformationViewModel
    {
        public string JobOrderCode { get; set; }
        public string OldModel { get; set; }
        public string OldIMEI1 { get; set; }
        public string OldIMEI2 { get; set; }
        public string OldColor { get; set; }
        public string NewModel { get; set; }
        public string IMEI1 { get; set; }
        public string IMEI2 { get; set; }
        public string Color { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public Nullable<DateTime> FromDate { get; set; }
        public Nullable<DateTime> ToDate { get; set; }
    }
}
