using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.FrontDesk.ViewModels
{
   public class SparePartsAvailableAndReqQtyViewModel
    {
        public long MobilePartId { get; set; }
        public string MobilePartName { get; set; }
        public int AvailableQty { get; set; }
        public int RequistionQty { get; set; }
    }
}
