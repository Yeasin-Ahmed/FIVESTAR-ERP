using ERPBO.Common;
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
        IEnumerable<JobOrderDTO> GetJobOrders(string mobileNo, long? modelId, string status, long? jobOrderId, string jobCode,string iMEI, string iMEI2, long orgId,long branchId, string fromDate, string toDate,string customerType,string jobType,string repairStatus, string customer,string courierNumber,string recId);
        JobOrder GetJobOrderById(long jobOrderId, long orgId);
        bool SaveJobOrder(JobOrderDTO jobOrder, List<JobOrderAccessoriesDTO> jobOrderAccessories, List<JobOrderProblemDTO> jobOrderProblems, long userId, long orgId,long branchId);
        bool UpdateJobOrderStatus(long jobOrderId, string status, string type, long userId, long orgId,long branchId);
        bool AssignTSForJobOrder(long jobOrderId, long tsId, long userId, long orgId,long branchId);
        IEnumerable<DashboardRequisitionSummeryDTO> DashboardJobOrderSummery(long orgId,long branchId);
        IEnumerable<JobOrder> GetAllJobOrdersByOrgId(long orgId);
        IEnumerable<JobOrderDTO> GetJobOrdersTS(string roleName,string mobileNo, long? modelId, long? jobOrderId, string jobCode,string status,string recId,long userId, long orgId, long branchId);
        IEnumerable<JobOrderDTO> GetJobOrdersPush(long? jobOrderId,string recId, long orgId,long branchId);

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

        IEnumerable<DashboardApprovedRequsitionDTO> DashboardPendingRequsition(long orgId, long branchId);
        IEnumerable<DashboardApprovedRequsitionDTO> DashboardCurrentRequsition(long orgId, long branchId);
        bool UpdateJobSingOutStatus(long jobOrderId, long userId, long orgId, long branchId);
        bool UpdateJobOrderDeliveryStatus(long jobOrderId, long userId, long orgId, long branchId);

        JobOrder GetReferencesNumberByMobileNumber(string mobileNumber, long orgId, long branchId);
        JobOrder GetReferencesNumberByIMEI2(string imei2, long orgId, long branchId);
        JobOrderDTO GetJobOrderReceipt(long jobOrderId, long userId, long orgId, long branchId);
        bool IsIMEIExistWithRunningJobOrder(long jobOrderId,string iMEI1, long orgId, long branchId);
        bool IsIMEI2ExistWithRunningJobOrder(long jobOrderId, string iMEI2, long orgId, long branchId);
        JobOrder GetAllJobOrderById(long branchId, long orgId);
        IEnumerable<JobOrderDTO> GetJobCreateReceipt(long jobOrderId, long orgId, long branchId);
        ExecutionStateWithText SaveJobOrderWithReport(JobOrderDTO jobOrder, List<JobOrderAccessoriesDTO> jobOrderAccessories, List<JobOrderProblemDTO> jobOrderProblems, long userId, long orgId, long branchId);
        IEnumerable<JobOrderDTO> JobOrderTransfer(long orgId, long branchId);
        bool SaveJobOrderTransfer(long transferId,long[] jobOrders, long userId, long orgId, long branchId, string cName, string cNumber);
        JobOrder GetJobOrdersByIdWithTransferBranch(long jobOrderId, long transferId, long orgId);
        IEnumerable<JobOrderDTO> TransferReceiveJobOrder(long orgId, long branchId,long? branchName);
        bool SaveTransferReceiveJob(long transferId, long[] jobOrders, long userId, long orgId, long branchId);
        bool UpdateTransferJob(long jobOrderId, long userId, long orgId, long branchId);
        JobOrder GetAllJobOrderByOrgId(long orgId);
        bool SaveJobOrderReturnAndUpdateJobOrder(long transferId, long[] jobOrders, long userId, long orgId, long branchId, string cName, string cNumber);
        bool UpdateReturnJob(long jobOrderId, long userId,long orgId);
        JobOrderDTO GetJobOrderDetails(long jobOrderId, long orgId);
        IEnumerable<DashboardApprovedRequsitionDTO> DashboardAnotherBranchRequsition(long orgId, long branchId);
        IEnumerable<DailySummaryReportDTO> DailySummaryReport(long orgId, long branchId, string fromDate, string toDate);
        IEnumerable<DailySummaryReportDTO> AllBranchDailySummaryReport(long orgId, string fromDate, string toDate);
        JobOrder GetReferencesNumberByJobOrder(string jobOrder, long orgId, long branchId);
        //
        IEnumerable<DashboardDailyReceiveJobOrderDTO> DashboardDailyTransferJob(long orgId, long branchId);
        IEnumerable<DashboardDailyReceiveJobOrderDTO> DashboardDailyReceiveJob(long orgId, long branchId);
        IEnumerable<DashboardSellsDTO> DashboardDailySells(long orgId, long branchId);
        IEnumerable<DashboardSellsDTO> DashboardTotalSells(long orgId, long branchId);
        IEnumerable<DailySellsChart> DailySellsChart(string fromDate, string toDate, long orgId, long branchId);

        IEnumerable<DashboardDailyReceiveJobOrderDTO> DashboardNotAssignJob(long orgId, long branchId);

        bool SaveCallCenterApproval(long jobId,string approval, string remarks,long userId, long orgId);
        IEnumerable<JobOrderDTO> DashboardCallCenterApproval(long orgId, long branchId,long userId);
        IEnumerable<JobOrderDTO> GetJobOrderForQc(string mobileNo, long? modelId, string status, long? jobOrderId, string jobCode, string iMEI, string iMEI2, long orgId, long branchId, string fromDate, string toDate, string customerType, string jobType, string repairStatus,string recId);
        bool SaveQCApproval(long jobId, string approval, string remarks, long userId, long orgId,long branchId);
        IEnumerable<JobOrderDTO> DashboardQCStatus(long orgId, long branchId, long userId);

        IEnumerable<JobOrderDTO> GetJobOrderForDelivery(string mobileNo, long? modelId, string status, long? jobOrderId, string jobCode, string iMEI, string iMEI2, long orgId, long branchId, string fromDate, string toDate, string customerType, string jobType, string repairStatus,string recId);
        ExecutionStateWithText SaveJobOrderMDelivey(long[] jobOrders, long userId, long orgId, long branchId);
        IEnumerable<JobOrderDTO> GetMultipleJobDeliveryChalan(string deliveryCode, long branchId, long orgId);
        bool UpdateQCTransferStatus(long jobOrderId, long orgId, long branchId, long userId);

        ExecutionStateWithText SaveMultipleJobOrderWithReport(List<JobOrderDTO> jobOrders, long userId, long orgId, long branchId);

        IEnumerable<JobOrderDTO> GetMultipleJobReceipt(string multipleJobCode,long orgId, long branchId);
        IEnumerable<JobOrderDTO> GetRefeNumberCount(string imei, long branchId, long orgId);
        IEnumerable<JobOrderDTO> GetQCPassFailData(string jobCode, long? modelId, string status, long orgId, long branchId, string fromDate, string toDate);
    }
}
