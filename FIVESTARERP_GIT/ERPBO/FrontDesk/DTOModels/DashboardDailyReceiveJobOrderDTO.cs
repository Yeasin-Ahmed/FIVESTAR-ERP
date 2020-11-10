using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.FrontDesk.DTOModels
{
   public class DashboardDailyReceiveJobOrderDTO
    {
        public int Total { get; set; }
        public int TransferJob { get; set; }
        public int ReturnJob { get; set; }
        public int ReceiveJob { get; set; }
        public int ReceiveReturnJob { get; set; }
    }
}
