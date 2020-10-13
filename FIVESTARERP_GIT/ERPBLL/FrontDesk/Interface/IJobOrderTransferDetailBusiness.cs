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
    public interface IJobOrderTransferDetailBusiness
    {
        JobOrderTransferDetail GetAllTransferJob(long transferId, long orgId, long branchId);
        bool SaveJobOrderTransfer(long transferId, long[] jobOrders, long userId, long orgId, long branchId);
        IEnumerable<JobOrderTransferDetailDTO> GetReceiveJob(long orgId, long branchId, long? branchName, string jobCode, string transferCode, string fromDate, string toDate, string tstatus);
        bool UpdateReceiveJobOrder(long transferId, long jobOrderId, long userId, long orgId, long branchId);
        IEnumerable<JobOrderDTO> GetTransferDeliveryChalan(string transferCode, long orgId);
        ExecutionStateWithText SaveJobOrderTransferWithReport(long transferId, long[] jobOrders, long userId, long orgId, long branchId,string cName,string cNumber);
    }
}
