using ERPBO.Common;
using ERPBO.FrontDesk.DomainModels;
using ERPBO.FrontDesk.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.FrontDesk.Interface
{
   public interface IJobOrderReturnDetailBusiness
    {
        bool SaveJobOrderReturn(long transferId, long[] jobOrders, long userId, long orgId, long branchId);
        IEnumerable<JobOrderReturnDetailDTO> GetReturnJobOrder(long orgId, long branchId, long? branchName, string jobCode, string transferCode, string fromDate, string toDate, string tstatus);
        bool UpdateReturnJobOrder(long returnId,long jobOrderId, long userId, long orgId, long branchId);
        JobOrderReturnDetail GetOneByOrgId(long returnId, long orgId, long branchId);
        IEnumerable<JobOrderDTO> GetReturnDeliveryChalan(string transferCode,long orgId);
        ExecutionStateWithText SaveJobOrderReturnWithReport(long transferId, long[] jobOrders, long userId, long orgId, long branchId);
        IEnumerable<JobOrderReturnDetailDTO> RepairOtherBranchJob(long branchId, long? branchName, long orgId, string fromDate, string toDate);
        IEnumerable<JobOrderReturnDetailDTO> RepairedJobOfOtherBranch(long branchId, long? branchName, long orgId, string fromDate, string toDate);
    }
}
