using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.FrontDesk.DTOModels
{
   public class DashboardDailySingInAndOutDTO
    {
        public int TotalSignInToday { get; set; }
        public int TotalSignOutToday { get; set; }
    }
}
