using ERPBLL.Common;
using ERPBLL.FrontDesk.Interface;
using ERPBLL.ReportSS.Interface;
using ERPBO.FrontDesk.DomainModels;
using ERPBO.FrontDesk.DTOModels;
using ERPBO.FrontDesk.ReportModels;
using ERPDAL.FrontDeskDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.ReportSS
{
   public class JobOrderReportBusiness: IJobOrderReportBusiness
    {
        private readonly IFrontDeskUnitOfWork _frontDeskUnitOfWork;
        public JobOrderReportBusiness(IFrontDeskUnitOfWork frontDeskUnitOfWork)
        {
            this._frontDeskUnitOfWork = frontDeskUnitOfWork;
        }

        public ServicesReportHead GetBranchInformation(long orgId, long branchId)
        {
            return this._frontDeskUnitOfWork.Db.Database.SqlQuery<ServicesReportHead>(
                string.Format(@"select org.OrganizationName,bh.BranchName,bh.Address,org.OrgLogoPath,bh.PhoneNo,bh.MobileNo,bh.Fax,bh.Email from 
            [ControlPanel].dbo.tblBranch bh
            inner join [ControlPanel].dbo.tblOrganizations org
        on bh.OrganizationId=org.OrganizationId
        where bh.OrganizationId={0} and bh.BranchId={1}", orgId, branchId)).FirstOrDefault();
        }
        private string QueryForJobOrderReport(string mobileNo, long? modelId, string status, long? jobOrderId, string jobCode, string iMEI, string iMEI2, long orgId, long branchId, string fromDate, string toDate)
        {
            string query = string.Empty;
            string param = string.Empty;

            if (jobOrderId != null && jobOrderId > 0) // Single Job Order Searching
            {
                param += string.Format(@"and jo.JodOrderId ={0}", jobOrderId);
            }
            else
            {
                // Multiple Job Order Searching
                if (!string.IsNullOrEmpty(mobileNo))
                {
                    param += string.Format(@"and jo.MobileNo Like '%{0}%'", mobileNo);
                }
                if (modelId != null && modelId > 0)
                {
                    param += string.Format(@"and de.DescriptionId ={0}", modelId);
                }
                if (!string.IsNullOrEmpty(status))
                {
                    param += string.Format(@"and jo.StateStatus ='{0}'", status);
                }
                if (!string.IsNullOrEmpty(jobCode))
                {
                    param += string.Format(@"and jo.JobOrderCode Like '%{0}%'", jobCode);
                }
                if (!string.IsNullOrEmpty(iMEI))
                {
                    param += string.Format(@"and jo.IMEI Like '%{0}%'", iMEI);
                }
                if (!string.IsNullOrEmpty(iMEI2))
                {
                    param += string.Format(@"and jo.IMEI2 Like '%{0}%'", iMEI2);
                }
            }
            if (orgId > 0)
            {
                param += string.Format(@"and jo.OrganizationId={0}", orgId);
            }
            if (branchId > 0)
            {
                param += string.Format(@"and (jo.BranchId={0} and jo.IsTransfer is null)
OR 
(jo.TransferBranchId={0} and jo.IsTransfer= 'true')
OR
(jo.IsReturn='True' and jo.BranchId={0} and jo.IsTransfer='False')", branchId);
            }
            if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "" && !string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(jo.EntryDate as date) between '{0}' and '{1}'", fDate, tDate);
            }
            else if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(jo.EntryDate as date)='{0}'", fDate);
            }
            else if (!string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(jo.EntryDate as date)='{0}'", tDate);
            }

            query = string.Format(@"Select JodOrderId,TsRepairStatus,JobOrderCode,CustomerName,MobileNo,[Address],ModelName,IsWarrantyAvailable,IsWarrantyPaperEnclosed,StateStatus,JobOrderType,EntryDate,EntryUser,
CloseDate,TSRemarks,
SUBSTRING(FaultName,1,LEN(FaultName)-1) 'FaultName',SUBSTRING(ServiceName,1,LEN(ServiceName)-1) 'ServiceName',
SUBSTRING(AccessoriesNames,1,LEN(AccessoriesNames)-1) 'AccessoriesNames',SUBSTRING(PartsName,1,LEN(PartsName)-1) 'PartsName',
SUBSTRING(Problems,1,LEN(Problems)-1) 'Problems',TSId,TSName,RepairDate,
IMEI,[Type],ModelColor,WarrantyDate,Remarks,ReferenceNumber,IMEI2,CloseUser,InvoiceCode,InvoiceInfoId,CustomerType,CourierNumber,CourierName,ApproxBill,IsTransfer
From (Select jo.JodOrderId,jo.CustomerName,jo.MobileNo,jo.[Address],de.DescriptionName 'ModelName',jo.IsWarrantyAvailable,jo.InvoiceInfoId,jo.IsWarrantyPaperEnclosed,jo.JobOrderType,jo.StateStatus,jo.EntryDate,ap.UserName'EntryUser',jo.CloseDate,jo.InvoiceCode,jo.CustomerType,jo.CourierNumber,jo.CourierName,jo.ApproxBill,jo.IsTransfer,

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

(Select top 1 SignOutDate from tblJobOrderTS jt
Inner Join tblJobOrders j on jt.JodOrderId = j.JodOrderId
Where jt.JodOrderId = jo.JodOrderId and j.TsRepairStatus='REPAIR AND RETURN' Order by jt.JTSId desc) 'RepairDate'

from tblJobOrders jo
Inner Join [Inventory].dbo.tblDescriptions de on jo.DescriptionId = de.DescriptionId
Inner Join [ControlPanel].dbo.tblApplicationUsers ap on jo.EUserId = ap.UserId

Where 1 = 1{0}
) tbl Order By EntryDate desc
", Utility.ParamChecker(param));
            return query;
        }

        public IEnumerable<JobOrderDTO> GetJobOrdersReport(string mobileNo, long? modelId, string status, long? jobOrderId, string jobCode, string iMEI, string iMEI2, long orgId, long branchId, string fromDate, string toDate)
        {
            //return _jobOrderBusiness.GetJobOrders(mobileNo, modelId, status, jobOrderId, jobCode, iMEI, iMEI2, orgId, branchId);
            return _frontDeskUnitOfWork.Db.Database.SqlQuery<JobOrderDTO>(QueryForJobOrderReport(mobileNo, modelId, status, jobOrderId, jobCode, iMEI, iMEI2, orgId, branchId,  fromDate,  toDate)).ToList();
        }

        public JobOrderDTO GetReceiptForJobOrder(long jobOrderId, long orgId, long branchId)
        {
            var data= this._frontDeskUnitOfWork.Db.Database.SqlQuery<JobOrderDTO>(
                string.Format(@"Select JodOrderId,JobOrderCode,CustomerName,MobileNo,ModelName,ModelColor,StateStatus,JobOrderType,EntryDate,EntryUser,
CloseDate,TsRepairStatus,
SUBSTRING(AccessoriesNames,1,LEN(AccessoriesNames)-1) 'AccessoriesNames',CloseUser
From (Select jo.JodOrderId,jo.CustomerName,jo.MobileNo,de.DescriptionName 'ModelName',jo.JobOrderType,jo.StateStatus,jo.EntryDate,ap.UserName 'EntryUser',jo.CloseDate,

Cast((Select AccessoriesName+',' From [Configuration].dbo.tblAccessories ass
Inner Join tblJobOrderAccessories joa on ass.AccessoriesId = joa.AccessoriesId
Where joa.JobOrderId = jo.JodOrderId
Order BY AccessoriesName For XML PATH('')) as nvarchar(MAX))  'AccessoriesNames',
jo.JobOrderCode,jo.TSId,jo.ModelColor,jo.TsRepairStatus,
(Select UserName  from tblJobOrders job
Inner Join [ControlPanel].dbo.tblApplicationUsers app on job.CUserId = app.UserId where job.JodOrderId=jo.JodOrderId) 'CloseUser'
from tblJobOrders jo
Inner Join [Inventory].dbo.tblDescriptions de on jo.DescriptionId = de.DescriptionId
Inner Join [ControlPanel].dbo.tblApplicationUsers ap on jo.EUserId = ap.UserId

Where 1 = 1 and jo.JodOrderId={0} and jo.OrganizationId={1} and jo.BranchId={2}) tbl Order By EntryDate desc", jobOrderId, orgId, branchId)).FirstOrDefault();
            return data;
        }
    }
}
