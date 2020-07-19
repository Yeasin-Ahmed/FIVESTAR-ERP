using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.FrontDesk.DTOModels
{
   public class TSSockDTO
    {
        public long RequsitionInfoForJobOrderId { get; set; }
        public string RequsitionCode { get; set; }
        public long PartsId { get; set; }
        public string PartsName { get; set; }
        public int Quantity { get; set; }
        public int UsedQty { get; set; }
    }
}
