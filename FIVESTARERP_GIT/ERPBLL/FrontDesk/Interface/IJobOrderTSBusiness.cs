using ERPBO.FrontDesk.DomainModels;
using ERPBO.FrontDesk.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.FrontDesk.Interface
{
   public interface IJobOrderTSBusiness
    {
        IEnumerable<DashboardDailySingInAndOutDTO> DashboardDailySingInAndOuts(long orgId, long branchId);
        JobOrderTS GetAllTJobOrderTs(long joborderId, long orgId, long branchId);
        bool UpdateJobOrderTsStatus(long joborderId, long userId, long orgId, long branchId);
    }
}
