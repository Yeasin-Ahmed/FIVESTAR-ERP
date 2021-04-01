using ERPBLL.Common;
using ERPBLL.FrontDesk.Interface;
using ERPBO.Common;
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
   public class JobOrderReturnDetailBusiness: IJobOrderReturnDetailBusiness
    {
        private readonly IFrontDeskUnitOfWork _frontDeskUnitOfWork;
        private readonly JobOrderReturnDetailRepository _jobOrderReturnDetailRepository;
        private readonly IJobOrderBusiness _jobOrderBusiness;

        public JobOrderReturnDetailBusiness(IFrontDeskUnitOfWork frontDeskUnitOfWork, IJobOrderBusiness jobOrderBusiness)
        {
            this._frontDeskUnitOfWork = frontDeskUnitOfWork;
            this._jobOrderReturnDetailRepository = new JobOrderReturnDetailRepository(this._frontDeskUnitOfWork);
            this._jobOrderBusiness = jobOrderBusiness;
        }

        public IEnumerable<JobOrderReturnDetailDTO> GetReturnJobOrder(long orgId, long branchId, long? branchName, string jobCode, string transferCode, string fromDate, string toDate, string tstatus)
        {
            return _frontDeskUnitOfWork.Db.Database.SqlQuery<JobOrderReturnDetailDTO>(QueryForGetReturnJob(orgId, branchId, branchName, jobCode, transferCode,fromDate,toDate,tstatus)).ToList();
        }
        private string QueryForGetReturnJob(long orgId, long branchId, long? branchName, string jobCode, string transferCode, string fromDate, string toDate,string tstatus)
        {
            string query = string.Empty;
            string param = string.Empty;
            if (orgId > 0)
            {
                param += string.Format(@"and d.OrganizationId={0}", orgId);
            }
            if (branchId > 0)
            {
                param += string.Format(@"and d.ToBranch={0} ", branchId);
            }
            if (branchName != null && branchName > 0)
            {
                param += string.Format(@"and d.BranchId ={0}", branchName);
            }
            if (!string.IsNullOrEmpty(jobCode))
            {
                param += string.Format(@"and d.JobOrderCode Like '%{0}%'", jobCode);
            }
            if (!string.IsNullOrEmpty(tstatus))
            {
                param += string.Format(@"and d.TransferStatus ='{0}'", tstatus);
            }
            if (!string.IsNullOrEmpty(transferCode))
            {
                param += string.Format(@"and d.TransferCode Like '%{0}%'", transferCode);
            }
            if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "" && !string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(d.EntryDate as date) between '{0}' and '{1}'", fDate, tDate);
            }
            else if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(d.EntryDate as date)='{0}'", fDate);
            }
            else if (!string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(d.EntryDate as date)='{0}'", tDate);
            }

            query = string.Format(@"Select JobOrderReturnDetailId,BranchId,TransferCode,JobOrderId,JobOrderCode,JobStatus,
TransferStatus,FromBranchName,EntryDate,ModelColor,ModelName,SUBSTRING(AccessoriesNames,1,LEN(AccessoriesNames)-1) 'AccessoriesNames',UserName'ReceivedBy' From (Select d.JobOrderReturnDetailId,d.BranchId,d.TransferCode,d.JobOrderId,d.JobOrderCode,d.JobStatus,
d.TransferStatus,b.BranchName'FromBranchName',d.EntryDate,j.ModelColor,de.ModelName,
(Cast((Select AccessoriesName+',' From [Configuration].dbo.tblAccessories ass
Inner Join tblJobOrderAccessories joa on ass.AccessoriesId = joa.AccessoriesId
Where joa.JobOrderId = j.JodOrderId
Order BY AccessoriesName For XML PATH('')) as nvarchar(MAX)))  'AccessoriesNames',apu.UserName
from [FrontDesk].dbo.tblJobOrderReturnDetails d
left join tblJobOrders j on d.JobOrderId=j.JodOrderId
left join [Configuration].dbo.tblModelSS de on j.DescriptionId=de.ModelId
left join [ControlPanel].dbo.tblBranch b on d.BranchId=b.BranchId
left join [ControlPanel].dbo.tblApplicationUsers apu on d.UpUserId=apu.UserId
where TransferStatus='Pending' and 1=1 {0}) tbl order by EntryDate desc", Utility.ParamChecker(param));
            return query;
        }

        public bool SaveJobOrderReturn(long transferId, long[] jobOrders, long userId, long orgId, long branchId)
        {
            bool IsSuccess = false;
            List<JobOrderReturnDetail> jobOrderReturn = new List<JobOrderReturnDetail>();
            string transferCode = ("R-" + DateTime.Now.ToString("yy") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss"));
            foreach (var job in jobOrders)
            {
                var jobOrderInDb = _jobOrderBusiness.GetJobOrdersByIdWithBranch(job, transferId, orgId);
                JobOrderReturnDetail detail = new JobOrderReturnDetail();
                detail.TransferCode = transferCode;
                detail.JobOrderId = jobOrderInDb.JodOrderId;
                detail.JobOrderCode = jobOrderInDb.JobOrderCode;
                detail.JobStatus = jobOrderInDb.StateStatus;
                detail.TransferStatus = "Pending";
                detail.BranchId = branchId;
                detail.FromBranch = branchId;
                detail.ToBranch = transferId;
                detail.OrganizationId = orgId;
                detail.EUserId = userId;
                detail.EntryDate = DateTime.Now;
                _jobOrderReturnDetailRepository.Insert(detail);
            }
            _jobOrderReturnDetailRepository.InsertAll(jobOrderReturn);
            if (_jobOrderReturnDetailRepository.Save() == true)
            {
                IsSuccess = _jobOrderBusiness.SaveJobOrderReturnAndUpdateJobOrder(transferId, jobOrders, userId, orgId, branchId,string.Empty,string.Empty);
            }
            return IsSuccess;
        }

        public bool UpdateReturnJobOrder(long returnId,long jobOrderId, long userId, long orgId, long branchId)
        {
            bool IsSuccess = false;
            var receive = GetOneByOrgId(returnId, orgId, branchId);
            if (receive != null)
            {
                receive.TransferStatus = JobOrderTransferStatus.Received;
                receive.UpUserId = userId;
                receive.UpdateDate = DateTime.Now;
                _jobOrderReturnDetailRepository.Update(receive);
            }
            if(_jobOrderReturnDetailRepository.Save() == true)
            {
                IsSuccess = _jobOrderBusiness.UpdateReturnJob(jobOrderId, userId, orgId);
            }
            return IsSuccess;
        }

        public JobOrderReturnDetail GetOneByOrgId(long returnId, long orgId, long branchId)
        {
            return _jobOrderReturnDetailRepository.GetOneByOrg(r => r.JobOrderReturnDetailId == returnId && r.OrganizationId == orgId && r.ToBranch == branchId);
        }

        public IEnumerable<JobOrderDTO> GetReturnDeliveryChalan(string transferCode, long orgId)
        {
            var data= this._frontDeskUnitOfWork.Db.Database.SqlQuery<JobOrderDTO>(
                string.Format(@"Select JodOrderId,TsRepairStatus,JobOrderCode,CustomerName,MobileNo,[Address],ModelName,IsWarrantyAvailable,IsWarrantyPaperEnclosed,StateStatus,JobOrderType,EntryDate,EntryUser,
CloseDate,TSRemarks,
SUBSTRING(FaultName,1,LEN(FaultName)-1) 'FaultName',SUBSTRING(ServiceName,1,LEN(ServiceName)-1) 'ServiceName',
SUBSTRING(AccessoriesNames,1,LEN(AccessoriesNames)-1) 'AccessoriesNames',SUBSTRING(PartsName,1,LEN(PartsName)-1) 'PartsName',
SUBSTRING(Problems,1,LEN(Problems)-1) 'Problems',TSId,TSName,RepairDate,
IMEI,[Type],ModelColor,WarrantyDate,Remarks,ReferenceNumber,IMEI2,CloseUser,InvoiceCode,InvoiceInfoId,CustomerType,CourierNumber,CourierName,ApproxBill,IsTransfer,TransferCode,DestinationBranch
From (Select jo.JodOrderId,jo.CustomerName,jo.MobileNo,jo.[Address],de.ModelName,jo.IsWarrantyAvailable,jo.InvoiceInfoId,jo.IsWarrantyPaperEnclosed,jo.JobOrderType,jo.StateStatus,jo.EntryDate,ap.UserName'EntryUser',jo.CloseDate,jo.InvoiceCode,jo.CustomerType,jo.CourierNumber,jo.CourierName,jo.ApproxBill,jo.IsTransfer,

Cast((Select FaultName+',' From [Configuration].dbo.tblFault fa
Inner Join tblJobOrderFault jof on fa.FaultId = jof.FaultId
Where jof.JobOrderId = jo.JodOrderId
Order BY FaultName For XML PATH('')) as nvarchar(MAX))  'FaultName',

Cast((Select ServiceName+',' From [Configuration].dbo.tblServices ser
Inner Join tblJobOrderServices jos on ser.ServiceId = jos.ServiceId
Where jos.JobOrderId = jo.JodOrderId
Order BY ServiceName For XML PATH('')) as nvarchar(MAX))  'ServiceName',

Cast((Select AccessoriesName+',' From [Configuration].dbo.tblAccessories ass
Inner Join tblJobOrderAccessories joa on ass.AccessoriesId = joa.AccessoriesId
Where joa.JobOrderId = jo.JodOrderId
Order BY AccessoriesName For XML PATH('')) as nvarchar(MAX))  'AccessoriesNames',

Cast((Select  (parts.MobilePartName+' (Qty-' + Cast(tstock.UsedQty as nvarchar(20)))+')'+',' from [FrontDesk].dbo.tblTechnicalServicesStock tstock
inner join [Configuration].dbo.tblMobileParts parts
on tstock.PartsId=parts.MobilePartId
Where tstock.UsedQty>0 and tstock.JobOrderId = jo.JodOrderId
Order BY (parts.MobilePartName+'#' + Cast(tstock.UsedQty as nvarchar(20))) For XML PATH('')) as nvarchar(MAX)) 'PartsName',

Cast((Select ProblemName+',' From [Configuration].dbo.tblClientProblems prob
Inner Join tblJobOrderProblems jop on prob.ProblemId = jop.ProblemId
Where jop.JobOrderId = jo.JodOrderId 
Order BY ProblemName For XML PATH(''))as nvarchar(MAX)) 'Problems',jo.JobOrderCode,jo.TSId,jo.TSRemarks,
--ts.Name ,
jo.IMEI,jo.[Type],jo.ModelColor,jo.WarrantyDate,jo.Remarks,jo.ReferenceNumber,jo.IMEI2,jo.TsRepairStatus,
(Select UserName  from tblJobOrders job
Inner Join [ControlPanel].dbo.tblApplicationUsers app on job.CUserId = app.UserId where job.JodOrderId=jo.JodOrderId) 'CloseUser',

(Select Top 1 UserName 'TSName' from tblJobOrderTS jts
Inner Join [ControlPanel].dbo.tblApplicationUsers app on jts.TSId = app.UserId
Where jts.JodOrderId = jo.JodOrderId 
Order By JTSId desc) 'TSName',

(Select Top 1 TransferCode 'TransferCode' from tblJobOrderReturnDetails tjt
Inner Join tblJobOrders joo on tjt.JobOrderId = joo.JodOrderId
Where tjt.JobOrderId = jo.JodOrderId)'TransferCode' ,

(Select Top 1 BranchName 'DestinationBranch' from tblJobOrderReturnDetails tdt
Inner Join tblJobOrders jodr on tdt.JobOrderId = jodr.JodOrderId
inner join [ControlPanel].dbo.tblBranch bb on tdt.ToBranch=bb.BranchId
Where jodr.JodOrderId = jo.JodOrderId 
Order By JobOrderId desc) 'DestinationBranch',

(Select top 1 SignOutDate from tblJobOrderTS jt
Inner Join tblJobOrders j on jt.JodOrderId = j.JodOrderId
Where jt.JodOrderId = jo.JodOrderId and j.TsRepairStatus='REPAIR AND RETURN' Order by jt.JTSId desc) 'RepairDate'

from tblJobOrders jo
Inner Join [Configuration].dbo.tblModelSS de on jo.DescriptionId = de.ModelId
Inner Join [ControlPanel].dbo.tblApplicationUsers ap on jo.EUserId = ap.UserId

where JodOrderId IN(
Select JobOrderId From tblJobOrderReturnDetails Where TransferCode='{0}' and OrganizationId={1})
) tbl Order By EntryDate asc", transferCode, orgId)).ToList();
            return data;
        }

        public ExecutionStateWithText SaveJobOrderReturnWithReport(long transferId, long[] jobOrders, long userId, long orgId, long branchId, string cName, string cNumber)
        {
            bool IsSuccess = false;
            ExecutionStateWithText executionState = new ExecutionStateWithText();
            List<JobOrderReturnDetail> jobOrderReturn = new List<JobOrderReturnDetail>();
            string transferCode = ("DO-" + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss"));
            foreach (var job in jobOrders)
            {
                var jobOrderInDb = _jobOrderBusiness.GetJobOrdersByIdWithBranch(job, transferId, orgId);
                JobOrderReturnDetail detail = new JobOrderReturnDetail();
                detail.TransferCode = transferCode;
                detail.JobOrderId = jobOrderInDb.JodOrderId;
                detail.JobOrderCode = jobOrderInDb.JobOrderCode;
                detail.JobStatus = jobOrderInDb.StateStatus;
                detail.TransferStatus = JobOrderTransferStatus.Pending;
                detail.BranchId = branchId;
                detail.FromBranch = branchId;
                detail.ToBranch = transferId;
                detail.OrganizationId = orgId;
                detail.EUserId = userId;
                detail.EntryDate = DateTime.Now;
                _jobOrderReturnDetailRepository.Insert(detail);
            }
            _jobOrderReturnDetailRepository.InsertAll(jobOrderReturn);
            if (_jobOrderReturnDetailRepository.Save() == true)
            {
                IsSuccess = _jobOrderBusiness.SaveJobOrderReturnAndUpdateJobOrder(transferId, jobOrders, userId, orgId, branchId,cName,cNumber);
                executionState.isSuccess = IsSuccess;
                executionState.text = transferCode;
            }
            return executionState;
        }

        public IEnumerable<JobOrderReturnDetailDTO> RepairOtherBranchJob(long branchId, long? branchName, long orgId, string fromDate, string toDate)
        {
            return _frontDeskUnitOfWork.Db.Database.SqlQuery<JobOrderReturnDetailDTO>(QueryForOtherBranchRepairJob(branchId, branchName, orgId, fromDate, toDate)).ToList();
        }
        private string QueryForOtherBranchRepairJob(long branchId, long? branchName, long orgId, string fromDate, string toDate)
        {
            string query = string.Empty;
            string param = string.Empty;
            if (orgId > 0)
            {
                param += string.Format(@"and jrd.OrganizationId={0}", orgId);
            }
            if (branchId > 0)
            {
                param += string.Format(@"and jrd.BranchId={0} ", branchId);
            }
            if (branchName != null && branchName > 0)
            {
                param += string.Format(@"and jrd.ToBranch ={0}", branchName);
            }
            if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "" && !string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(jrd.EntryDate as date) between '{0}' and '{1}'", fDate, tDate);
            }
            else if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(jrd.EntryDate as date)='{0}'", fDate);
            }
            else if (!string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(jrd.EntryDate as date)='{0}'", tDate);
            }

            query = string.Format(@"select JobOrderCode,JobStatus,BranchName 'FromBranchName',jrd.EntryDate,jrd.ToBranch From tblJobOrderReturnDetails jrd
left join [ControlPanel].dbo.tblBranch b on b.BranchId=jrd.ToBranch
where JobStatus='Repair-Done' and 1=1{0}

", Utility.ParamChecker(param));
            return query;
        }

        public IEnumerable<JobOrderReturnDetailDTO> RepairedJobOfOtherBranch(long branchId, long? branchName, long orgId, string fromDate, string toDate)
        {
            return _frontDeskUnitOfWork.Db.Database.SqlQuery<JobOrderReturnDetailDTO>(QueryForRepairedJobOfOtherBranch(branchId, branchName, orgId, fromDate, toDate)).ToList();
        }
        private string QueryForRepairedJobOfOtherBranch(long branchId, long? branchName, long orgId, string fromDate, string toDate)
        {
            string query = string.Empty;
            string param = string.Empty;
            if (orgId > 0)
            {
                param += string.Format(@"and jrd.OrganizationId={0}", orgId);
            }
            if (branchId > 0)
            {
                param += string.Format(@"and jrd.ToBranch={0} ", branchId);
            }
            if (branchName != null && branchName > 0)
            {
                param += string.Format(@"and jrd.FromBranch ={0}", branchName);
            }
            if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "" && !string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(jrd.EntryDate as date) between '{0}' and '{1}'", fDate, tDate);
            }
            else if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(jrd.EntryDate as date)='{0}'", fDate);
            }
            else if (!string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(jrd.EntryDate as date)='{0}'", tDate);
            }

            query = string.Format(@"select JobOrderCode,JobStatus,BranchName 'FromBranchName',jrd.EntryDate,jrd.FromBranch From tblJobOrderReturnDetails jrd
left join [ControlPanel].dbo.tblBranch b on b.BranchId=jrd.FromBranch
where JobStatus='Repair-Done' and 1=1 {0}", Utility.ParamChecker(param));
            return query;
        }
    }
}
