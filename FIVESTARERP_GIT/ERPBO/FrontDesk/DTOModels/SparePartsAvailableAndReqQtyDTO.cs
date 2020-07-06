using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.FrontDesk.DTOModels
{
   public class SparePartsAvailableAndReqQtyDTO
    {
        public long MobilePartId { get; set; }
        public string MobilePartName { get; set; }
        public int? AvailableQty { get; set; }
        public int? RequistionQty { get; set; }
    }
}
