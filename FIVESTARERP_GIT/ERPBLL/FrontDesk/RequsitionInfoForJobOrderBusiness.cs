using ERPBLL.Common;
using ERPBLL.Configuration.Interface;
using ERPBLL.FrontDesk.Interface;
using ERPBO.FrontDesk.DomainModels;
using ERPBO.FrontDesk.DTOModels;
using ERPDAL.FrontDeskDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.FrontDesk
{
    public class RequsitionInfoForJobOrderBusiness : IRequsitionInfoForJobOrderBusiness
    {
        private readonly IFrontDeskUnitOfWork _frontDeskUnitOfWork;//db
        private readonly RequsitionInfoForJobOrderRepository requsitionInfoForJobOrderRepository;
        private readonly IJobOrderBusiness _jobOrderBusiness;
        private readonly IServicesWarehouseBusiness _servicesWarehouseBusiness;

        public RequsitionInfoForJobOrderBusiness(IFrontDeskUnitOfWork frontDeskUnitOfWork, IJobOrderBusiness jobOrderBusiness, IServicesWarehouseBusiness servicesWarehouseBusiness)
        {
            this._jobOrderBusiness = jobOrderBusiness;
            this._frontDeskUnitOfWork = frontDeskUnitOfWork;
            this.requsitionInfoForJobOrderRepository = new RequsitionInfoForJobOrderRepository(this._frontDeskUnitOfWork);
            this._servicesWarehouseBusiness = servicesWarehouseBusiness;
        }

        public IEnumerable<DashboardRequestSparePartsDTO> DashboardRequestSpareParts(long orgId, long branchId)
        {
            return this._frontDeskUnitOfWork.Db.Database.SqlQuery<DashboardRequestSparePartsDTO>(
                string.Format(@"Select 
(select ISNULL(COUNT(StateStatus),0)'Approved' from tblRequsitionInfoForJobOrders
Where Cast(GETDATE() as date) = Cast(EntryDate as date) and  OrganizationId={0} and BranchId={1} and StateStatus='Approved')'Approved',

(select ISNULL(COUNT(StateStatus),0)'Current' from tblRequsitionInfoForJobOrders
Where Cast(GETDATE() as date) = Cast(EntryDate as date) and  OrganizationId={0} and BranchId={1} and StateStatus='Current')'Current',

(select ISNULL(COUNT(StateStatus),0)'Pending' from tblRequsitionInfoForJobOrders
Where Cast(GETDATE() as date) = Cast(EntryDate as date) and  OrganizationId={0} and BranchId={1} and StateStatus='Pending')'Pending',

(select ISNULL(COUNT(StateStatus),0)'Void' from tblRequsitionInfoForJobOrders
Where Cast(GETDATE() as date) = Cast(EntryDate as date) and  OrganizationId={0} and BranchId={1} and StateStatus='Void')'Void'
", orgId, branchId)).ToList();
        }

        public IEnumerable<RequsitionInfoForJobOrder> GetAllRequsitionInfoForJob(long orgId, long branchId)
        {
            return requsitionInfoForJobOrderRepository.GetAll(info => info.OrganizationId == orgId && info.BranchId == branchId).ToList();
        }

        public IEnumerable<RequsitionInfoForJobOrder> GetAllRequsitionInfoForJobOrder(long joborderId, long orgId, long branchId)
        {
            return requsitionInfoForJobOrderRepository.GetAll(info => info.JobOrderId == joborderId && info.OrganizationId == orgId && info.BranchId == branchId).ToList();
        }

        public RequsitionInfoForJobOrder GetAllRequsitionInfoForJobOrderId(long reqInfoId, long orgId)
        {
            return requsitionInfoForJobOrderRepository.GetOneByOrg(info => info.RequsitionInfoForJobOrderId == reqInfoId && info.OrganizationId == orgId);
        }

        public RequsitionInfoForJobOrder GetAllRequsitionInfoOneByOrgId(long ReqId, long orgId, long branchId)
        {
            return requsitionInfoForJobOrderRepository.GetOneByOrg(info => info.RequsitionInfoForJobOrderId == ReqId && info.OrganizationId == orgId && info.BranchId == branchId);
        }

        public IEnumerable<RequsitionInfoForJobOrderDTO> GetRequsitionInfoData(string reqCode, long? warehouseId, long? tsId, string status, string fromDate, string toDate, long orgId, long branchId,string jobCode)
        {
            return _frontDeskUnitOfWork.Db.Database.SqlQuery<RequsitionInfoForJobOrderDTO>(QueryForRequsitionInfoData( reqCode,   warehouseId,   tsId,  status,  fromDate,  toDate,  orgId,  branchId,jobCode)).ToList();
        }
        private string QueryForRequsitionInfoData(string reqCode, long? warehouseId, long? tsId, string status, string fromDate, string toDate, long orgId, long branchId,string jobCode)
        {
            string query = string.Empty;
            string param = string.Empty;
            if (orgId > 0)
            {
                param += string.Format(@"and q.OrganizationId={0}", orgId);
            }
            if (branchId > 0)
            {
                param += string.Format(@"and q.BranchId={0} ", branchId);
            }
            if (!string.IsNullOrEmpty(reqCode))
            {
                param += string.Format(@"and q.RequsitionCode Like '%{0}%'", reqCode);
            }
            if (warehouseId != null && warehouseId > 0)
            {
                param += string.Format(@"and q.SWarehouseId ={0}", warehouseId);
            }
            if (tsId != null && tsId > 0)
            {
                param += string.Format(@"and q.EUserId ={0}", tsId);
            }
            if (!string.IsNullOrEmpty(status))
            {
                param += string.Format(@"and q.StateStatus ='{0}'", status);
            }
            if (!string.IsNullOrEmpty(jobCode))
            {
                param += string.Format(@"and j.JobOrderCode Like '%{0}%'", jobCode);
            }
            if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "" && !string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(q.EntryDate as date) between '{0}' and '{1}'", fDate, tDate);
            }
            else if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(q.EntryDate as date)='{0}'", fDate);
            }
            else if (!string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(q.EntryDate as date)='{0}'", tDate);
            }
            query = string.Format(@"Select RequsitionInfoForJobOrderId,RequsitionCode,SWarehouseId,q.StateStatus,
JobOrderId,q.JobOrderCode,q.Remarks,q.BranchId,q.OrganizationId,q.EUserId,j.IsTransfer,j.JobOrderCode 'JobCode',
app.UserName'Requestby',
q.EntryDate,UserBranchId From tblRequsitionInfoForJobOrders q
inner join tblJobOrders j on q.JobOrderId=j.JodOrderId 
inner join[ControlPanel].dbo.tblApplicationUsers app on q.OrganizationId = app.OrganizationId and q.EUserId= app.UserId 
where j.IsTransfer IS NULL and 1=1 {0}
order by EntryDate desc
", Utility.ParamChecker(param));
            return query;
        }

        public bool SaveRequisitionInfoForJobOrder(RequsitionInfoForJobOrderDTO requsitionInfoDTO, List<RequsitionDetailForJobOrderDTO> details, long userId, long orgId, long branchId)
        {
            bool IsSuccess = false;
            var jobOrder = _jobOrderBusiness.GetJobOrderById(requsitionInfoDTO.JobOrderId.Value, orgId);
            var warehouse = _servicesWarehouseBusiness.GetWarehouseOneByOrgId(orgId, jobOrder.BranchId.Value);
            RequsitionInfoForJobOrder requsitionInfo = new RequsitionInfoForJobOrder();
            if (requsitionInfoDTO.RequsitionInfoForJobOrderId == 0)
            {
                requsitionInfo.RequsitionCode = ("SR-" + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss"));
                requsitionInfo.SWarehouseId = warehouse.SWarehouseId;
                requsitionInfo.JobOrderId = requsitionInfoDTO.JobOrderId;
                requsitionInfo.StateStatus = RequisitionStatus.Current;
                requsitionInfo.Remarks = requsitionInfoDTO.Remarks;
                requsitionInfo.EntryDate = DateTime.Now;
                requsitionInfo.EUserId = userId;
                requsitionInfo.OrganizationId = orgId;
                requsitionInfo.BranchId = jobOrder.BranchId;
                requsitionInfo.UserBranchId = branchId;
                List<RequsitionDetailForJobOrder> requsitionDetails = new List<RequsitionDetailForJobOrder>();

                foreach (var item in details)
                {
                    RequsitionDetailForJobOrder requsitionDetail = new RequsitionDetailForJobOrder();
                    requsitionDetail.RequsitionDetailForJobOrderId = item.RequsitionDetailForJobOrderId;
                    requsitionDetail.PartsId = item.PartsId;
                    requsitionDetail.Quantity = item.Quantity;
                    requsitionDetail.JobOrderId = requsitionInfo.JobOrderId;
                    requsitionDetail.Remarks = item.Remarks;
                    requsitionDetail.SWarehouseId = warehouse.SWarehouseId;
                    requsitionDetail.EUserId = userId;
                    requsitionDetail.EntryDate = DateTime.Now;
                    requsitionDetail.OrganizationId = orgId;
                    requsitionDetail.BranchId = jobOrder.BranchId;
                    requsitionDetail.UserBranchId = branchId;
                    requsitionDetails.Add(requsitionDetail);
                }
                requsitionInfo.RequsitionDetailForJobOrders = requsitionDetails;
                requsitionInfoForJobOrderRepository.Insert(requsitionInfo);
                IsSuccess = requsitionInfoForJobOrderRepository.Save();
            }
            return IsSuccess;
        }

        public bool SaveRequisitionStatus(long reqId, string status, long userId, long orgId, long branchId)
        {
            var r = GetAllRequsitionInfoForJobOrderId(reqId, orgId);
            var jobOrder = _jobOrderBusiness.GetJobOrderById(r.JobOrderId.Value, orgId);
            var reqInfo = requsitionInfoForJobOrderRepository.GetOneByOrg(req => req.RequsitionInfoForJobOrderId == reqId && req.OrganizationId == orgId && req.BranchId == jobOrder.BranchId);

             //reqInfo.JobOrderId
            if (reqInfo != null)
            {
                reqInfo.StateStatus = status;
                reqInfo.UpUserId = userId;
                requsitionInfoForJobOrderRepository.Update(reqInfo);
            }
            return requsitionInfoForJobOrderRepository.Save();
        }

        public bool UpdatePendingCurrentRequisitionStatus(long jobOrderId, string tsRepairStatus, long userId, long orgId, long branchId)
        {
            bool IsSuccess = false;
            var reqInfos = requsitionInfoForJobOrderRepository.GetAll(req => req.JobOrderId == jobOrderId && req.OrganizationId == orgId && req.BranchId == branchId);
            var Status = ((tsRepairStatus == "RETURN WITHOUT REPAIR") || (tsRepairStatus == "MODULE SWAP")) ? RequisitionStatus.Void : RequisitionStatus.Approved;
            if(reqInfos.Count() > 0)
            {
                foreach (var item in reqInfos)
                {
                    item.StateStatus = Status;
                    item.UpUserId = userId;
                    item.UpdateDate = DateTime.Now;
                    requsitionInfoForJobOrderRepository.Update(item);
                }
                IsSuccess = requsitionInfoForJobOrderRepository.Save();
            }
            else
            {
                IsSuccess = true;
            }
            return IsSuccess;
        }

        public IEnumerable<RequsitionInfoForJobOrderDTO> GetRequsitionInfoOtherBranchData(string reqCode, long? warehouseId, long? tsId, string status, string fromDate, string toDate, long orgId, long branchId)
        {
            return _frontDeskUnitOfWork.Db.Database.SqlQuery<RequsitionInfoForJobOrderDTO>(QueryForRequsitionInfoOtherBranchData(reqCode, warehouseId, tsId, status, fromDate, toDate, orgId, branchId)).ToList();
        }
        private string QueryForRequsitionInfoOtherBranchData(string reqCode, long? warehouseId, long? tsId, string status, string fromDate, string toDate, long orgId, long branchId)
        {
            string query = string.Empty;
            string param = string.Empty;
            if (orgId > 0)
            {
                param += string.Format(@"and q.OrganizationId={0}", orgId);
            }
            if (branchId > 0)
            {
                param += string.Format(@"and j.TransferBranchId={0} ", branchId);
            }
            if (!string.IsNullOrEmpty(reqCode))
            {
                param += string.Format(@"and q.RequsitionCode Like '%{0}%'", reqCode);
            }
            if (warehouseId != null && warehouseId > 0)
            {
                param += string.Format(@"and q.SWarehouseId ={0}", warehouseId);
            }
            if (tsId != null && tsId > 0)
            {
                param += string.Format(@"and q.EUserId ={0}", tsId);
            }
            if (!string.IsNullOrEmpty(status))
            {
                param += string.Format(@"and q.StateStatus ='{0}'", status);
            }
            if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "" && !string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(q.EntryDate as date) between '{0}' and '{1}'", fDate, tDate);
            }
            else if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(q.EntryDate as date)='{0}'", fDate);
            }
            else if (!string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(q.EntryDate as date)='{0}'", tDate);
            }
            query = string.Format(@"Select RequsitionInfoForJobOrderId,RequsitionCode,SWarehouseId,q.StateStatus,b.BranchName,
JobOrderId,q.JobOrderCode,q.Remarks,q.BranchId,q.OrganizationId,q.EUserId,j.IsTransfer,j.TransferBranchId,app.UserName'Requestby',
q.EntryDate,UserBranchId From tblRequsitionInfoForJobOrders q
left join tblJobOrders j on q.JobOrderId=j.JodOrderId
inner join[ControlPanel].dbo.tblApplicationUsers app on q.OrganizationId = app.OrganizationId and q.EUserId= app.UserId
left join [ControlPanel].dbo.tblBranch b on q.BranchId=b.BranchId
where j.IsTransfer='True' and 1=1{0}
order by q.EntryDate desc
", Utility.ParamChecker(param));
            return query;
        }

        public IEnumerable<RequsitionInfoForJobOrderDTO> GetRequsitionInfoTSData(long jobOrderId)
        {
            return this._frontDeskUnitOfWork.Db.Database.SqlQuery<RequsitionInfoForJobOrderDTO>(
                string.Format(@"Select RequsitionInfoForJobOrderId,RequsitionCode,SWarehouseId,q.StateStatus,
JobOrderId,q.JobOrderCode,q.Remarks,q.BranchId,q.OrganizationId,q.EUserId,j.IsTransfer,j.TransferBranchId,
q.EntryDate,UserBranchId From tblRequsitionInfoForJobOrders q
left join tblJobOrders j on q.JobOrderId=j.JodOrderId
where q.JobOrderId={0}", jobOrderId)).ToList();
        }
    }
}
