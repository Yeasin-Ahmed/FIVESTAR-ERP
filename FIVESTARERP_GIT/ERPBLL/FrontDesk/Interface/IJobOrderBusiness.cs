using ERPBO.FrontDesk.DomainModels;
using ERPBO.FrontDesk.DTOModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.FrontDesk.Interface
{
    public interface IJobOrderBusiness
    {
        IEnumerable<JobOrderDTO> GetJobOrders(string mobileNo, long? modelId, string status, long? jobOrderId, string jobCode,string iMEI, string iMEI2, long orgId,long branchId);
        JobOrder GetJobOrderById(long jobOrderId, long orgId);
        bool SaveJobOrder(JobOrderDTO jobOrder, List<JobOrderAccessoriesDTO> jobOrderAccessories, List<JobOrderProblemDTO> jobOrderProblems, long userId, long orgId,long branchId);
        bool UpdateJobOrderStatus(long jobOrderId, string status, string type, long userId, long orgId);
        bool AssignTSForJobOrder(long jobOrderId, long tsId, long userId, long orgId);
        IEnumerable<DashboardRequisitionSummeryDTO> DashboardJobOrderSummery(long orgId,long branchId);
        IEnumerable<JobOrder> GetAllJobOrdersByOrgId(long orgId);
        IEnumerable<JobOrderDTO> GetJobOrdersTS(string roleName,string mobileNo, long? modelId, long? jobOrderId, string jobCode,long userId, long orgId);
        IEnumerable<JobOrderDTO> GetJobOrdersPush(long? jobOrderId, long orgId);

        IEnumerable<JobOrder> GetJobOrdersByBranch(long branchId, long orgId);
        JobOrder GetJobOrdersByIdWithBranch(long jobOrderId,long branchId, long orgId);
        bool SaveJobOrderPushing(long ts, long[] jobOrders,long userId, long orgId, long branchId);

        bool SaveJobOrderPulling(long jobOrderId, long userId, long orgId, long branchId);
        JobOrder GetReferencesNumberByIMEI( string imei, long orgId, long branchId);
        IEnumerable<DashboardDailyReceiveJobOrderDTO> DashboardDailyJobOrder(long orgId,long branchId);
        IEnumerable<DashboardDailyBillingAndWarrantyJobDTO> DashboardDailyBillingAndWarrantyJob(long orgId, long branchId);

        bool GetJobOrderById( long jobOrderId, long orgId, long branchId);

        IEnumerable<SparePartsAvailableAndReqQtyDTO> SparePartsAvailableAndReqQty(long orgId, long branchId,long jobOrderId);
        bool UpdateJobOrderTsRemarks(long jobOrderId, string remarks, long userId, long orgId,long branchId);

        IEnumerable<DashboardApprovedRequsitionDTO> DashboardApprovedRequsition(long orgId, long branchId);
        IEnumerable<DashboardApprovedRequsitionDTO> DashboardPendingRequsition(long orgId, long branchId);
        
    }
}
