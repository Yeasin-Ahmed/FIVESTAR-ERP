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
        JobOrderTS GetJobOrderActiveTsByJobOrderId(long joborderId, long orgId, long branchId);
        bool UpdateJobOrderTsStatus(long joborderId, long userId, long orgId, long branchId);
        IEnumerable<JobOrderTSDTO> JobSignInAndOut(long? tsId,string jobCode,long orgId, long branchId, string fromDate, string toDate);
        bool UpdateJobOrderTsForQcFail(long joborderId, long userId, long orgId, long branchId);
        JobOrderTS GetJobOrderTsByJobOrderId(long joborderId, long orgId, long branchId);
    }
}
