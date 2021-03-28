using ERPBLL.Common;
using ERPBLL.Configuration.Interface;
using ERPBLL.ControlPanel.Interface;
using ERPBLL.FrontDesk.Interface;
using ERPBLL.ReportSS.Interface;
using ERPBO.Common;
using ERPBO.FrontDesk.DomainModels;
using ERPBO.FrontDesk.DTOModels;
using ERPBO.Production.DTOModel;
using ERPDAL.FrontDeskDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.FrontDesk
{
    public class JobOrderBusiness : IJobOrderBusiness
    {
        private readonly JobOrderRepository _jobOrderRepository;
        private readonly JobOrderAccessoriesRepository _jobOrderAccessoriesRepository;
        private readonly JobOrderProblemRepository _jobOrderProblemRepository;
        private readonly IFrontDeskUnitOfWork _frontDeskUnitOfWork;
        private readonly IJobOrderAccessoriesBusiness _jobOrderAccessoriesBusiness;
        private readonly IJobOrderProblemBusiness _jobOrderProblemBusiness;
        private readonly IJobOrderReportBusiness _jobOrderReportBusiness;
        private readonly IBranchBusiness _branchBusiness;
        //private readonly IJobOrderTSBusiness _jobOrderTSBusiness;

        public JobOrderBusiness(IFrontDeskUnitOfWork frontDeskUnitOfWork, IJobOrderAccessoriesBusiness jobOrderAccessoriesBusiness, IJobOrderProblemBusiness jobOrderProblemBusiness, IJobOrderReportBusiness jobOrderReportBusiness, IBranchBusiness branchBusiness)
        {
            this._frontDeskUnitOfWork = frontDeskUnitOfWork;
            this._jobOrderRepository = new JobOrderRepository(this._frontDeskUnitOfWork);
            this._jobOrderAccessoriesBusiness = jobOrderAccessoriesBusiness;
            this._jobOrderAccessoriesRepository = new JobOrderAccessoriesRepository(this._frontDeskUnitOfWork);
            this._jobOrderProblemBusiness = jobOrderProblemBusiness;
            this._jobOrderProblemRepository = new JobOrderProblemRepository(this._frontDeskUnitOfWork);
            this._jobOrderReportBusiness = jobOrderReportBusiness;
            this._branchBusiness = branchBusiness;

        }

        public JobOrder GetJobOrderById(long jobOrderId, long orgId)
        {
            return _jobOrderRepository.GetOneByOrg(j => j.JodOrderId == jobOrderId && j.OrganizationId == orgId);
        }

        public IEnumerable<JobOrderDTO> GetJobOrders(string mobileNo, long? modelId, string status, long? jobOrderId, string jobCode, string iMEI, string iMEI2, long orgId, long branchId, string fromDate, string toDate, string customerType, string jobType, string repairStatus, string customer)
        {
            return _frontDeskUnitOfWork.Db.Database.SqlQuery<JobOrderDTO>(QueryForJobOrder(mobileNo, modelId, status, jobOrderId, jobCode, iMEI, iMEI2, orgId, branchId, fromDate, toDate,customerType,jobType,repairStatus,customer)).ToList();
        }

        private string QueryForJobOrder(string mobileNo, long? modelId, string status, long? jobOrderId, string jobCode, string iMEI, string iMEI2, long orgId, long branchId, string fromDate, string toDate, string customerType, string jobType, string repairStatus,string customer)
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
                    param += string.Format(@"and de.ModelId ={0}", modelId);
                }
                if (!string.IsNullOrEmpty(status))
                {
                    param += string.Format(@"and jo.StateStatus ='{0}'", status);
                }
                if (!string.IsNullOrEmpty(customerType))
                {
                    param += string.Format(@"and jo.CustomerType ='{0}'", customerType);
                }
                if (!string.IsNullOrEmpty(jobType))
                {
                    param += string.Format(@"and jo.JobOrderType ='{0}'", jobType);
                }
                if (!string.IsNullOrEmpty(jobCode))
                {
                    param += string.Format(@"and jo.JobOrderCode Like '%{0}%'", jobCode);
                }
                if (!string.IsNullOrEmpty(repairStatus))
                {
                    param += string.Format(@"and jo.TsRepairStatus Like '%{0}%'", repairStatus);
                }
                if (!string.IsNullOrEmpty(iMEI))
                {
                    param += string.Format(@"and jo.IMEI Like '%{0}%'", iMEI);
                }
                if (!string.IsNullOrEmpty(iMEI2))
                {
                    param += string.Format(@"and jo.IMEI2 Like '%{0}%'", iMEI2);
                }
                if (!string.IsNullOrEmpty(customer))
                {
                    param += string.Format(@"and jo.CustomerName Like '%{0}%'", customer);
                }
            }
            if (orgId > 0)
            {
                param += string.Format(@"and jo.OrganizationId={0}", orgId);
            }
            if (branchId > 0)
            {
                param += string.Format(@"and ((jo.BranchId={0} and (jo.IsTransfer is null or jo.IsTransfer='True'))
OR 
(jo.TransferBranchId={0} and jo.IsTransfer= 'true')
OR
(jo.IsReturn='True' and jo.BranchId={0} and jo.IsTransfer='False'))", branchId);
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

            query = string.Format(@"Select JodOrderId,TsRepairStatus,JobOrderCode,CustomerName,MobileNo,[Address],ModelName,IsWarrantyAvailable,IsWarrantyPaperEnclosed,IsHandset,StateStatus,JobOrderType,EntryDate,EntryUser,
CloseDate,TSRemarks,
SUBSTRING(FaultName,1,LEN(FaultName)-1) 'FaultName',SUBSTRING(ServiceName,1,LEN(ServiceName)-1) 'ServiceName',
SUBSTRING(AccessoriesNames,1,LEN(AccessoriesNames)-1) 'AccessoriesNames',SUBSTRING(PartsName,1,LEN(PartsName)-1) 'PartsName',
SUBSTRING(Problems,1,LEN(Problems)-1) 'Problems',TSId,TSName,RepairDate,
IMEI,[Type],ModelColor,WarrantyDate,Remarks,ReferenceNumber,IMEI2,CloseUser,InvoiceCode,InvoiceInfoId,CustomerType,CourierNumber,CourierName,ApproxBill,IsTransfer,JobLocationB,IsReturn,CustomerApproval,CallCenterRemarks
From (Select jo.JodOrderId,jo.CustomerName,jo.MobileNo,jo.[Address],de.ModelName,jo.IsWarrantyAvailable,jo.InvoiceInfoId,jo.IsWarrantyPaperEnclosed,jo.IsHandset,jo.JobOrderType,jo.StateStatus,jo.EntryDate,ap.UserName'EntryUser',jo.CloseDate,jo.InvoiceCode,jo.CustomerType,jo.CourierNumber,jo.CourierName,jo.ApproxBill,jo.IsTransfer,jo.IsReturn,bb.BranchName'JobLocationB',jo.CustomerApproval,jo.CallCenterRemarks,

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
Where jt.JodOrderId = jo.JodOrderId and (j.TsRepairStatus='REPAIR AND RETURN' or(j.TsRepairStatus='MODULE SWAP' and j.StateStatus='HandSetChange')) Order by jt.JTSId desc) 'RepairDate'

from tblJobOrders jo
Inner Join [Configuration].dbo.tblModelSS de on jo.DescriptionId = de.ModelId
Inner Join [ControlPanel].dbo.tblApplicationUsers ap on jo.EUserId = ap.UserId
Left Join [ControlPanel].dbo.tblBranch bb on jo.JobLocation=bb.BranchId

Where 1 = 1{0}) tbl Order By JobOrderCode desc
", Utility.ParamChecker(param));
            return query;
        }

        public bool SaveJobOrder(JobOrderDTO jobOrderDto, List<JobOrderAccessoriesDTO> jobOrderAccessoriesDto, List<JobOrderProblemDTO> jobOrderProblemsDto, long userId, long orgId, long branchId)
        {
            if (jobOrderDto.JodOrderId == 0)
            {
                JobOrder jobOrder = new JobOrder
                {
                    CustomerId = jobOrderDto.CustomerId,
                    CustomerName = jobOrderDto.CustomerName,
                    MobileNo = jobOrderDto.MobileNo,
                    Address = jobOrderDto.Address,
                    IMEI = jobOrderDto.IMEI,
                    IMEI2 = jobOrderDto.IMEI2,
                    Type = jobOrderDto.Type,
                    CustomerType = jobOrderDto.CustomerType,
                    ModelColor = jobOrderDto.ModelColor,
                    JobOrderType= jobOrderDto.JobOrderType,
                    WarrantyDate = jobOrderDto.WarrantyDate,
                    Remarks = jobOrderDto.Remarks,
                    ReferenceNumber = jobOrderDto.ReferenceNumber,
                    DescriptionId = jobOrderDto.DescriptionId,
                    IsWarrantyAvailable = jobOrderDto.IsWarrantyAvailable,
                    IsWarrantyPaperEnclosed = jobOrderDto.IsWarrantyPaperEnclosed,
                    EntryDate = jobOrderDto.EntryDate,
                    EUserId = userId,
                    OrganizationId = orgId,
                    StateStatus = JobOrderStatus.JobInitiated,
                    JobOrderCode = ("JOB-" + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss")),
                    BranchId = branchId
                };
                if (jobOrder.JobOrderType == "Warrenty")
                {
                    jobOrder.WarrantyDate = jobOrderDto.WarrantyDate.Value.Date;
                    jobOrder.IsWarrantyPaperEnclosed = jobOrderDto.IsWarrantyPaperEnclosed;
                    //jobOrder.WarrantyEndDate = jobOrderDto.WarrantyEndDate.Value.Date;
                }
                List<JobOrderAccessories> listJobOrderAccessories = new List<JobOrderAccessories>();
                foreach (var item in jobOrderAccessoriesDto)
                {
                    JobOrderAccessories jobOrderAccessories = new JobOrderAccessories
                    {
                        AccessoriesId = item.AccessoriesId,
                        EntryDate = DateTime.Now,
                        EUserId = userId,
                        OrganizationId = orgId
                    };
                    listJobOrderAccessories.Add(jobOrderAccessories);
                }

                if (jobOrderAccessoriesDto.Count > 0)
                {
                    jobOrder.JobOrderAccessories = listJobOrderAccessories;
                }

                List<JobOrderProblem> listjobOrderProblems = new List<JobOrderProblem>();
                foreach (var item in jobOrderProblemsDto)
                {
                    JobOrderProblem jobOrderProblem = new JobOrderProblem
                    {
                        ProblemId = item.ProblemId,
                        EntryDate = DateTime.Now,
                        EUserId = userId,
                        OrganizationId = orgId
                    };
                    listjobOrderProblems.Add(jobOrderProblem);
                }
                jobOrder.JobOrderProblems = listjobOrderProblems;

                _jobOrderRepository.Insert(jobOrder);
            }
            else
            {
                // when edit first retreive the data from database then change the entity object with new data
                var jobOrderDb = GetJobOrderById(jobOrderDto.JodOrderId, orgId);

                jobOrderDb.JodOrderId = jobOrderDto.JodOrderId;
                jobOrderDb.CustomerId = jobOrderDto.CustomerId;
                jobOrderDb.CustomerName = jobOrderDto.CustomerName;
                jobOrderDb.MobileNo = jobOrderDto.MobileNo;
                jobOrderDb.Address = jobOrderDto.Address;
                jobOrderDb.IMEI = jobOrderDto.IMEI;
                jobOrderDb.IMEI2 = jobOrderDto.IMEI2;
                jobOrderDb.Type = jobOrderDto.Type;
                jobOrderDb.CustomerType = jobOrderDto.CustomerType;
                jobOrderDb.ModelColor = jobOrderDto.ModelColor;
                jobOrderDb.JobOrderType = jobOrderDto.JobOrderType;
                jobOrderDb.WarrantyDate = jobOrderDto.WarrantyDate;
                jobOrderDb.Remarks = jobOrderDto.Remarks;
                jobOrderDb.ReferenceNumber = jobOrderDto.ReferenceNumber;
                jobOrderDb.DescriptionId = jobOrderDto.DescriptionId;
                jobOrderDb.IsWarrantyAvailable = jobOrderDto.IsWarrantyAvailable;
                jobOrderDb.IsWarrantyPaperEnclosed = jobOrderDto.IsWarrantyPaperEnclosed;
                jobOrderDb.UpUserId = userId;
                jobOrderDb.UpdateDate = DateTime.Now;

                if (jobOrderDb.JobOrderType == "Warrenty")
                {
                    jobOrderDb.WarrantyDate = jobOrderDto.WarrantyDate.Value.Date;
                    jobOrderDb.IsWarrantyPaperEnclosed = jobOrderDto.IsWarrantyPaperEnclosed;
                    //jobOrderDb.WarrantyEndDate = jobOrderDto.WarrantyEndDate.Value.Date;
                }

                var jobOrderAccessoriesInDb = _jobOrderAccessoriesBusiness.GetJobOrderAccessoriesByJobOrder(jobOrderDb.JodOrderId, orgId).ToList();
                _jobOrderAccessoriesRepository.DeleteAll(jobOrderAccessoriesInDb);

                List<JobOrderAccessories> listJobOrderAccessories = new List<JobOrderAccessories>();
                foreach (var item in jobOrderAccessoriesDto)
                {
                    JobOrderAccessories jobOrderAccessories = new JobOrderAccessories
                    {
                        AccessoriesId = item.AccessoriesId,
                        UpdateDate = DateTime.Now,
                        UpUserId = userId,
                        OrganizationId = orgId
                    };
                    listJobOrderAccessories.Add(jobOrderAccessories);
                }
                if (listJobOrderAccessories.Count > 0)
                {
                    jobOrderDb.JobOrderAccessories = listJobOrderAccessories;
                }

                // Now retreive the accessories data From Job Order Accessories Business;
                #region Accessories_obsoulte


                //if (jobOrderAccessoriesInDb.Count() > 0)  // the joborder has got one ore more accessories ib Db
                //{
                //    //new accessories
                //    var allAccessories = jobOrderAccessoriesDto.Select(s => s.AccessoriesId).ToList();
                //    if (allAccessories.Count > 0)
                //    { // jobOrder has got one or more new accessories

                //        // find the unmatchaing accessories with the new data..
                //        var unMatchingAccessories = jobOrderAccessoriesInDb.Where(acc => !allAccessories.Contains(acc.AccessoriesId)).ToList();

                //        if (unMatchingAccessories.Count > 0)
                //        {
                //            // _jobOrderAccessoriesRepository.Delete(ass=> ass.);
                //            _jobOrderAccessoriesRepository.DeleteAll(unMatchingAccessories);
                //        }

                //        var matchingAccessories = jobOrderAccessoriesInDb.Where(acc => allAccessories.Contains(acc.AccessoriesId)).Select(s => s.AccessoriesId).ToList();
                //        if (matchingAccessories.Count > 0)
                //        {
                //            var newAccessories = jobOrderAccessoriesDto.Where(acc => !matchingAccessories.Contains(acc.AccessoriesId)).ToList();
                //        }
                //        else
                //        {

                //        }
                //    }
                //}
                //else
                //{
                //    // If the joborder does not have any accessories in Db
                //    // New Accessories
                //    List<JobOrderAccessories> listJobOrderAccessories = new List<JobOrderAccessories>();
                //    foreach (var item in jobOrderAccessoriesDto)
                //    {
                //        JobOrderAccessories jobOrderAccessories = new JobOrderAccessories
                //        {
                //            AccessoriesId = item.AccessoriesId,
                //            UpdateDate = DateTime.Now,
                //            UpUserId = userId,
                //            OrganizationId = orgId
                //        };
                //        listJobOrderAccessories.Add(jobOrderAccessories);
                //    }
                //    if (listJobOrderAccessories.Count > 0)
                //    {
                //        jobOrderDb.JobOrderAccessories = listJobOrderAccessories;
                //    }
                //} 

                #endregion

                var jobOrderPrblmInDb = _jobOrderProblemBusiness.GetJobOrderProblemByJobOrderId(jobOrderDb.JodOrderId, orgId).ToList();
                _jobOrderProblemRepository.DeleteAll(jobOrderPrblmInDb);

                List<JobOrderProblem> listjobOrderProblems = new List<JobOrderProblem>();
                foreach (var item in jobOrderProblemsDto)
                {
                    JobOrderProblem jobOrderProblem = new JobOrderProblem
                    {
                        ProblemId = item.ProblemId,
                        UpdateDate = DateTime.Now,
                        UpUserId = userId,
                        OrganizationId = orgId
                    };

                    listjobOrderProblems.Add(jobOrderProblem);
                }
                if (listjobOrderProblems.Count > 0) {
                    jobOrderDb.JobOrderProblems = listjobOrderProblems;
                }

                _jobOrderRepository.Update(jobOrderDb);
            }

            return _jobOrderRepository.Save();
        }

        public ExecutionStateWithText SaveJobOrderWithReport(JobOrderDTO jobOrderDto, List<JobOrderAccessoriesDTO> jobOrderAccessoriesDto, List<JobOrderProblemDTO> jobOrderProblemsDto, long userId, long orgId, long branchId)
        {
            var branchShortName = _branchBusiness.GetBranchOneByOrgId(branchId, orgId).ShortName;

            ExecutionStateWithText execution = new ExecutionStateWithText();
            JobOrder jobOrder = null;
            if (jobOrderDto.JodOrderId == 0)
            {
                jobOrder = new JobOrder
                {
                    CustomerId = jobOrderDto.CustomerId,
                    CustomerName = jobOrderDto.CustomerName,
                    MobileNo = jobOrderDto.MobileNo,
                    Address = jobOrderDto.Address,
                    IMEI = jobOrderDto.IMEI,
                    IMEI2 = jobOrderDto.IMEI2,
                    Type = jobOrderDto.Type,
                    CustomerType = jobOrderDto.CustomerType,
                    ModelColor = jobOrderDto.ModelColor,
                    JobOrderType = jobOrderDto.JobOrderType,
                    WarrantyDate = jobOrderDto.WarrantyDate,
                    Remarks = jobOrderDto.Remarks,
                    JobLocation = branchId,
                    CourierName =jobOrderDto.CourierName,
                    CourierNumber= jobOrderDto.CourierNumber,
                    ApproxBill= jobOrderDto.ApproxBill,
                    ReferenceNumber = jobOrderDto.ReferenceNumber,
                    DescriptionId = jobOrderDto.DescriptionId,
                    IsWarrantyAvailable = jobOrderDto.IsWarrantyAvailable,
                    IsWarrantyPaperEnclosed = jobOrderDto.IsWarrantyPaperEnclosed,
                    IsHandset= jobOrderDto.IsHandset,
                    EntryDate = jobOrderDto.EntryDate,
                    EUserId = userId,
                    OrganizationId = orgId,
                    StateStatus = JobOrderStatus.JobInitiated,
                    
                    BranchId = branchId
                };
                if (jobOrder.JobOrderType == "Warrenty")
                {
                    jobOrder.WarrantyDate = jobOrderDto.WarrantyDate.Value.Date;
                    jobOrder.IsWarrantyPaperEnclosed = jobOrderDto.IsWarrantyPaperEnclosed;
                    jobOrder.IsHandset = jobOrderDto.IsHandset;
                    //jobOrder.WarrantyEndDate = jobOrderDto.WarrantyEndDate.Value.Date;
                }
                List<JobOrderAccessories> listJobOrderAccessories = new List<JobOrderAccessories>();
                foreach (var item in jobOrderAccessoriesDto)
                {
                    JobOrderAccessories jobOrderAccessories = new JobOrderAccessories
                    {
                        AccessoriesId = item.AccessoriesId,
                        EntryDate = DateTime.Now,
                        EUserId = userId,
                        OrganizationId = orgId
                    };
                    listJobOrderAccessories.Add(jobOrderAccessories);
                }

                if (jobOrderAccessoriesDto.Count > 0)
                {
                    jobOrder.JobOrderAccessories = listJobOrderAccessories;
                }

                List<JobOrderProblem> listjobOrderProblems = new List<JobOrderProblem>();
                foreach (var item in jobOrderProblemsDto)
                {
                    JobOrderProblem jobOrderProblem = new JobOrderProblem
                    {
                        ProblemId = item.ProblemId,
                        EntryDate = DateTime.Now,
                        EUserId = userId,
                        OrganizationId = orgId
                    };
                    listjobOrderProblems.Add(jobOrderProblem);
                }
                jobOrder.JobOrderProblems = listjobOrderProblems;

                string jobOrderCode = branchShortName+"-" +GetJobOrderSerial(orgId,branchId).PadLeft(7,'0'); // 0000001
                jobOrder.JobOrderCode = jobOrderCode;
                _jobOrderRepository.Insert(jobOrder);
            }
            else
            {
                // when edit first retreive the data from database then change the entity object with new data
                var jobOrderDb = GetJobOrderById(jobOrderDto.JodOrderId, orgId);

                jobOrderDb.JodOrderId = jobOrderDto.JodOrderId;
                jobOrderDb.CustomerId = jobOrderDto.CustomerId;
                jobOrderDb.CustomerName = jobOrderDto.CustomerName;
                jobOrderDb.MobileNo = jobOrderDto.MobileNo;
                jobOrderDb.Address = jobOrderDto.Address;
                jobOrderDb.IMEI = jobOrderDto.IMEI;
                jobOrderDb.IMEI2 = jobOrderDto.IMEI2;
                jobOrderDb.Type = jobOrderDto.Type;
                jobOrderDb.JobLocation = branchId;
                jobOrderDb.CustomerType = jobOrderDto.CustomerType;
                jobOrderDb.ModelColor = jobOrderDto.ModelColor;
                jobOrderDb.JobOrderType = jobOrderDto.JobOrderType;
                jobOrderDb.WarrantyDate = jobOrderDto.WarrantyDate;
                jobOrderDb.Remarks = jobOrderDto.Remarks;
                jobOrderDb.CourierName = jobOrderDto.CourierName;
                jobOrderDb.CourierNumber = jobOrderDto.CourierNumber;
                jobOrderDb.ApproxBill = jobOrderDto.ApproxBill;
                jobOrderDb.ReferenceNumber = jobOrderDto.ReferenceNumber;
                jobOrderDb.DescriptionId = jobOrderDto.DescriptionId;
                jobOrderDb.IsWarrantyAvailable = jobOrderDto.IsWarrantyAvailable;
                jobOrderDb.IsWarrantyPaperEnclosed = jobOrderDto.IsWarrantyPaperEnclosed;
                jobOrderDb.IsHandset = jobOrderDto.IsHandset;
                jobOrderDb.UpUserId = userId;
                jobOrderDb.UpdateDate = DateTime.Now;

                if (jobOrderDb.JobOrderType == "Warrenty")
                {
                    jobOrderDb.WarrantyDate = jobOrderDto.WarrantyDate.Value.Date;
                    jobOrderDb.IsWarrantyPaperEnclosed = jobOrderDto.IsWarrantyPaperEnclosed;
                    jobOrderDb.IsHandset = jobOrderDto.IsHandset;
                    //jobOrderDb.WarrantyEndDate = jobOrderDto.WarrantyEndDate.Value.Date;
                }

                var jobOrderAccessoriesInDb = _jobOrderAccessoriesBusiness.GetJobOrderAccessoriesByJobOrder(jobOrderDb.JodOrderId, orgId).ToList();
                _jobOrderAccessoriesRepository.DeleteAll(jobOrderAccessoriesInDb);

                List<JobOrderAccessories> listJobOrderAccessories = new List<JobOrderAccessories>();
                foreach (var item in jobOrderAccessoriesDto)
                {
                    JobOrderAccessories jobOrderAccessories = new JobOrderAccessories
                    {
                        AccessoriesId = item.AccessoriesId,
                        UpdateDate = DateTime.Now,
                        UpUserId = userId,
                        OrganizationId = orgId
                    };
                    listJobOrderAccessories.Add(jobOrderAccessories);
                }
                if (listJobOrderAccessories.Count > 0)
                {
                    jobOrderDb.JobOrderAccessories = listJobOrderAccessories;
                }

                // Now retreive the accessories data From Job Order Accessories Business;
                #region Accessories_obsoulte


                //if (jobOrderAccessoriesInDb.Count() > 0)  // the joborder has got one ore more accessories ib Db
                //{
                //    //new accessories
                //    var allAccessories = jobOrderAccessoriesDto.Select(s => s.AccessoriesId).ToList();
                //    if (allAccessories.Count > 0)
                //    { // jobOrder has got one or more new accessories

                //        // find the unmatchaing accessories with the new data..
                //        var unMatchingAccessories = jobOrderAccessoriesInDb.Where(acc => !allAccessories.Contains(acc.AccessoriesId)).ToList();

                //        if (unMatchingAccessories.Count > 0)
                //        {
                //            // _jobOrderAccessoriesRepository.Delete(ass=> ass.);
                //            _jobOrderAccessoriesRepository.DeleteAll(unMatchingAccessories);
                //        }

                //        var matchingAccessories = jobOrderAccessoriesInDb.Where(acc => allAccessories.Contains(acc.AccessoriesId)).Select(s => s.AccessoriesId).ToList();
                //        if (matchingAccessories.Count > 0)
                //        {
                //            var newAccessories = jobOrderAccessoriesDto.Where(acc => !matchingAccessories.Contains(acc.AccessoriesId)).ToList();
                //        }
                //        else
                //        {

                //        }
                //    }
                //}
                //else
                //{
                //    // If the joborder does not have any accessories in Db
                //    // New Accessories
                //    List<JobOrderAccessories> listJobOrderAccessories = new List<JobOrderAccessories>();
                //    foreach (var item in jobOrderAccessoriesDto)
                //    {
                //        JobOrderAccessories jobOrderAccessories = new JobOrderAccessories
                //        {
                //            AccessoriesId = item.AccessoriesId,
                //            UpdateDate = DateTime.Now,
                //            UpUserId = userId,
                //            OrganizationId = orgId
                //        };
                //        listJobOrderAccessories.Add(jobOrderAccessories);
                //    }
                //    if (listJobOrderAccessories.Count > 0)
                //    {
                //        jobOrderDb.JobOrderAccessories = listJobOrderAccessories;
                //    }
                //} 

                #endregion

                var jobOrderPrblmInDb = _jobOrderProblemBusiness.GetJobOrderProblemByJobOrderId(jobOrderDb.JodOrderId, orgId).ToList();
                _jobOrderProblemRepository.DeleteAll(jobOrderPrblmInDb);

                List<JobOrderProblem> listjobOrderProblems = new List<JobOrderProblem>();
                foreach (var item in jobOrderProblemsDto)
                {
                    JobOrderProblem jobOrderProblem = new JobOrderProblem
                    {
                        ProblemId = item.ProblemId,
                        UpdateDate = DateTime.Now,
                        UpUserId = userId,
                        OrganizationId = orgId
                    };

                    listjobOrderProblems.Add(jobOrderProblem);
                }
                if (listjobOrderProblems.Count > 0)
                {
                    jobOrderDb.JobOrderProblems = listjobOrderProblems;
                }

                _jobOrderRepository.Update(jobOrderDb);
            }

            execution.isSuccess =  _jobOrderRepository.Save();
            execution.text = jobOrderDto.JodOrderId == 0 ? jobOrder.JodOrderId.ToString() : jobOrderDto.JodOrderId.ToString();
            return execution;
        }

        public string GetJobOrderSerial(long orgId,long branchId)
        {
//             this._frontDeskUnitOfWork.Db.Database.SqlQuery<string>(string.Format(@"Select Cast( ISNULL(MAX(Cast(SUBSTRING(JobOrderCode,5,LEN(JobOrderCode)) as bigint)),0)+1 as Nvarchar(20)) 'Value'  from [FrontDesk].dbo.tblJobOrders 
//Where  JobOrderCode not like 'JOB%' and 
//OrganizationId= {0} and BranchId={1}
//",orgId,branchId)).FirstOrDefault();
            var query = string.Format(@"Exec [dbo].[spJobOrderSerial] {0},{1}", orgId,branchId);
            return _frontDeskUnitOfWork.Db.Database.SqlQuery<string>(query).FirstOrDefault();
        }

        public bool UpdateJobOrderStatus(long jobOrderId, string status, string type, long userId, long orgId, long branchId)
        {
            var jobOrder = GetJobOrderById(jobOrderId, orgId);
            if (jobOrder != null && jobOrder.StateStatus == JobOrderStatus.RepairDone)
            {
                jobOrder.StateStatus = status.Trim();
                jobOrder.JobOrderType = type.Trim();
                jobOrder.UpUserId = userId;
                jobOrder.UpdateDate = DateTime.Now;
                _jobOrderRepository.Update(jobOrder);
            }
            return _jobOrderRepository.Save();
        }

        public bool AssignTSForJobOrder(long jobOrderId, long tsId, long userId, long orgId, long branchId)
        {
            var jobOrder = GetJobOrderById(jobOrderId, orgId);
            if (jobOrder != null && jobOrder.StateStatus == JobOrderStatus.JobInitiated)
            {
                jobOrder.TSId = tsId;
                jobOrder.StateStatus = JobOrderStatus.AssignToTS;
                jobOrder.UpUserId = userId;
                jobOrder.BranchId = branchId;
                jobOrder.UpdateDate = DateTime.Now;
                _jobOrderRepository.Update(jobOrder);
            }
            return _jobOrderRepository.Save();
        }

        public IEnumerable<DashboardRequisitionSummeryDTO> DashboardJobOrderSummery(long orgId, long branchId)
        {
            return _frontDeskUnitOfWork.Db.Database.SqlQuery<DashboardRequisitionSummeryDTO>(string.Format(@"select StateStatus, count(*) as TotalCount from tblJobOrders Where OrganizationId={0} and BranchId={1} group by StateStatus", orgId, branchId)).ToList();
        }

        public IEnumerable<JobOrder> GetAllJobOrdersByOrgId(long orgId)
        {
            return _jobOrderRepository.GetAll(access => access.OrganizationId == orgId).ToList();
        }
        private string QueryForJobOrderTS(string roleName, string mobileNo, long? modelId, long? jobOrderId, string jobCode, long userId, long orgId,long branchId)
        {
            string query = string.Empty;
            string param = string.Empty;
            if (jobOrderId != null && jobOrderId > 0) // Single Job Order Searching
            {
                param += string.Format(@" and jo.JodOrderId ={0}", jobOrderId);
            }
            else
            {
                // Multiple Job Order Searching
                if (!string.IsNullOrEmpty(mobileNo))
                {
                    param += string.Format(@" and jo.MobileNo Like '%{0}%'", mobileNo);
                }
                if (modelId != null && modelId > 0)
                {
                    param += string.Format(@" and de.DescriptionId ={0}", modelId);
                }
                if (!string.IsNullOrEmpty(jobCode))
                {
                    param += string.Format(@" and jo.JobOrderCode Like '%{0}%'", jobCode);
                }
            }
            if (orgId > 0)
            {
                param += string.Format(@" and jo.OrganizationId={0}", orgId);
            }
            if (branchId > 0)
            {
                param += string.Format(@" and ((jo.BranchId= {0} 
and (IsTransfer is null or IsTransfer = 'False')) OR (IsTransfer = 'True' and TransferBranchId={0}))", branchId);
            }
            if (roleName== "Technical Services")
            {
                param += string.Format(@" and jo.TSId ={0}", userId);
            }
            query = string.Format(@"Select JodOrderId,JobOrderCode,CustomerName,MobileNo,[Address],ModelName,IsWarrantyAvailable,IsWarrantyPaperEnclosed,StateStatus,JobOrderType,EntryDate,EntryUser,
SUBSTRING(AccessoriesNames,1,LEN(AccessoriesNames)-1) 'AccessoriesNames',
SUBSTRING(Problems,1,LEN(Problems)-1) 'Problems',TSId,TSName,BranchId
From (Select jo.JodOrderId,jo.CustomerName,jo.MobileNo,jo.[Address],de.ModelName,jo.IsWarrantyAvailable,jo.IsWarrantyPaperEnclosed,jo.JobOrderType,jo.StateStatus,jo.EntryDate,UserName,jo.BranchId,

Cast((Select AccessoriesName+',' From [Configuration].dbo.tblAccessories ass
Inner Join tblJobOrderAccessories joa on ass.AccessoriesId = joa.AccessoriesId
Where joa.JobOrderId = jo.JodOrderId
Order BY AccessoriesName For XML PATH('')) as nvarchar(MAX))  'AccessoriesNames',

Cast((Select ProblemName+',' From [Configuration].dbo.tblClientProblems prob
Inner Join tblJobOrderProblems jop on prob.ProblemId = jop.ProblemId
Where jop.JobOrderId = jo.JodOrderId
Order BY ProblemName For XML PATH(''))as nvarchar(MAX)) 'Problems',jo.JobOrderCode,jo.TSId,ap.UserName 'TSName',
(Select Top 1 app.UserName  from tblJobOrders j
Inner Join [ControlPanel].dbo.tblApplicationUsers app on j.EUserId = app.UserId
Where j.JodOrderId = jo.JodOrderId
Order By j.EUserId desc) 'EntryUser'

from tblJobOrders jo
Inner Join [Configuration].dbo.tblModelSS de on jo.DescriptionId = de.ModelId
Inner Join [ControlPanel].dbo.tblApplicationUsers ap on jo.TSId = ap.UserId 
Where 1 = 1{0} and (jo.StateStatus='TS-Assigned' or jo.StateStatus='QC-Assigned' or jo.StateStatus='Repair-Done' or jo.StateStatus='Delivery-Done')) tbl Order By JobOrderCode desc", Utility.ParamChecker(param));
            return query;
        }

        public IEnumerable<JobOrderDTO> GetJobOrdersTS(string roleName, string mobileNo, long? modelId, long? jobOrderId, string jobCode, long userId, long orgId, long branchId)
        {
            return _frontDeskUnitOfWork.Db.Database.SqlQuery<JobOrderDTO>(QueryForJobOrderTS(roleName,mobileNo, modelId, jobOrderId, jobCode, userId, orgId, branchId)).ToList();
        }

        public IEnumerable<JobOrderDTO> GetJobOrdersPush(long? jobOrderId, long orgId, long branchId)
        {
            return _frontDeskUnitOfWork.Db.Database.SqlQuery<JobOrderDTO>(QueryForJobOrderPush(jobOrderId, orgId,branchId)).ToList();
        }

        private string QueryForJobOrderPush(long? jobOrderId, long orgId, long branchId)
        {
            string query = string.Empty;
            string param = string.Empty;

            if (jobOrderId != null && jobOrderId > 0)
            {
                param += string.Format(@"and jo.JodOrderId ={0}", jobOrderId);
            }
            if (orgId > 0)
            {
                param += string.Format(@"and jo.OrganizationId={0}", orgId);
            }
            if (branchId > 0)
            {
                param += string.Format(@"and ((jo.BranchId= {0} 
and (IsTransfer is null or IsTransfer = 'False')) OR (IsTransfer = 'True' and TransferBranchId={0}))", branchId);
            }

            query = string.Format(@"Select JodOrderId,JobOrderCode,CustomerName,MobileNo,[Address],ModelName,IsWarrantyAvailable,IsWarrantyPaperEnclosed,StateStatus,JobOrderType,EntryDate,EntryUser,
SUBSTRING(AccessoriesNames,1,LEN(AccessoriesNames)-1) 'AccessoriesNames',
SUBSTRING(Problems,1,LEN(Problems)-1) 'Problems',TSId,TSName,BranchName
From (Select jo.JodOrderId,jo.CustomerName,jo.MobileNo,jo.[Address],de.ModelName,jo.IsWarrantyAvailable,jo.IsWarrantyPaperEnclosed,jo.JobOrderType,jo.StateStatus,jo.EntryDate,ap.UserName 'EntryUser',b.BranchName,

Cast((Select AccessoriesName+',' From [Configuration].dbo.tblAccessories ass
Inner Join tblJobOrderAccessories joa on ass.AccessoriesId = joa.AccessoriesId
Where joa.JobOrderId = jo.JodOrderId
Order BY AccessoriesName For XML PATH('')) as nvarchar(MAX))  'AccessoriesNames',

Cast((Select ProblemName+',' From [Configuration].dbo.tblClientProblems prob
Inner Join tblJobOrderProblems jop on prob.ProblemId = jop.ProblemId
Where jop.JobOrderId = jo.JodOrderId
Order BY ProblemName For XML PATH(''))as nvarchar(MAX)) 'Problems',jo.JobOrderCode,jo.TSId,ts.Name 'TSName'

from tblJobOrders jo
Inner Join [Configuration].dbo.tblModelSS de on jo.DescriptionId = de.ModelId
Left Join [Configuration].dbo.tblTechnicalServiceEngs ts on jo.TSId =ts.EngId
Inner Join [ControlPanel].dbo.tblApplicationUsers ap on jo.EUserId = ap.UserId
left join [ControlPanel].dbo.tblBranch b on jo.BranchId=b.BranchId
Where 1 = 1{0} and jo.StateStatus='Job-Initiated'
) tbl", Utility.ParamChecker(param));
            return query;
        }

        public bool SaveJobOrderPushing(long ts, long[] jobOrders, long userId, long orgId, long branchId)
        {
            //bool IsSuccess = false;
            foreach (var job in jobOrders)
            {
                //var jobTransderInDb = 
                var jobOrderInDb = GetJobOrdersByIdWithBranch(job, branchId, orgId);
                jobOrderInDb = jobOrderInDb == null ? GetJobOrdersByIdWithTransferBranch(job, branchId, orgId) : jobOrderInDb;

                if (jobOrderInDb != null && jobOrderInDb.StateStatus == JobOrderStatus.JobInitiated)
                {
                    jobOrderInDb.StateStatus = JobOrderStatus.AssignToTS;
                    jobOrderInDb.TSId = ts;
                    jobOrderInDb.UpUserId = userId;
                    jobOrderInDb.UpdateDate = DateTime.Now;
                    jobOrderInDb.JobOrderTS = new List<JobOrderTS>() {
                        new JobOrderTS()
                        {
                            TSId =ts,
                            BranchId = branchId,
                            OrganizationId = orgId,
                            EUserId = userId,
                            AssignDate = DateTime.Now,
                            IsActive = true,
                            JobOrderCode = jobOrderInDb.JobOrderCode,
                            JodOrderId = job,
                            StateStatus="Sign-In",
                            EntryDate =DateTime.Now,
                            Remarks="Pushing"
                        }
                    };
                    _jobOrderRepository.Update(jobOrderInDb);
                }
            }
            return _jobOrderRepository.Save();
        }

        public IEnumerable<JobOrder> GetJobOrdersByBranch(long branchId, long orgId)
        {
            return _jobOrderRepository.GetAll(j => j.BranchId == branchId && j.OrganizationId == orgId);
        }

        public JobOrder GetJobOrdersByIdWithBranch(long jobOrderId, long branchId, long orgId)
        {
            return _jobOrderRepository.GetOneByOrg(j => j.JodOrderId == jobOrderId && j.BranchId == branchId && j.OrganizationId == orgId);
        }

        public bool SaveJobOrderPulling(long jobOrderId, long userId, long orgId, long branchId)
        {
            var jobId = GetJobOrderById(jobOrderId, orgId);
            var jobOrderInDb = GetJobOrdersByIdWithBranch(jobOrderId, jobId.BranchId.Value, orgId);
            if (jobOrderInDb != null && jobOrderInDb.StateStatus == JobOrderStatus.JobInitiated)
            {
                jobOrderInDb.StateStatus = JobOrderStatus.AssignToTS;
                jobOrderInDb.TSId = userId;
                jobOrderInDb.UpUserId = userId;
                jobOrderInDb.UpdateDate = DateTime.Now;
                jobOrderInDb.JobOrderTS = new List<JobOrderTS>() {
                        new JobOrderTS()
                        {
                            TSId =userId,
                            BranchId = branchId,
                            OrganizationId = orgId,
                            EUserId = userId,
                            AssignDate = DateTime.Now,
                            IsActive = true,
                            JobOrderCode = jobOrderInDb.JobOrderCode,
                            JodOrderId = jobOrderId,
                            StateStatus="Sign-In",
                            EntryDate =DateTime.Now,
                            Remarks="Pulling"
                        }
                    };

                _jobOrderRepository.Update(jobOrderInDb);
            }
            return _jobOrderRepository.Save();
        }

        public JobOrder GetReferencesNumberByIMEI(string imei, long orgId, long branchId)
        {
            imei = imei.Trim();
            return _jobOrderRepository.GetAll(job => (job.IMEI == imei.ToString() || job.IMEI2 == imei.ToString()) && job.OrganizationId == orgId && job.BranchId == branchId).OrderByDescending(o => o.JodOrderId).FirstOrDefault();
        }
        public JobOrder GetReferencesNumberByIMEI2(string imei2, long orgId, long branchId)
        {
            imei2 = imei2.Trim();
            return _jobOrderRepository.GetAll(job => job.IMEI2 == imei2.ToString() && job.OrganizationId == orgId && job.BranchId == branchId).OrderByDescending(o => o.JodOrderId).FirstOrDefault();
        }

        public IEnumerable<DashboardDailyReceiveJobOrderDTO> DashboardDailyJobOrder(long orgId, long branchId)
        {
            return this._frontDeskUnitOfWork.Db.Database.SqlQuery<DashboardDailyReceiveJobOrderDTO>(
                string.Format(@"select COUNT(StateStatus) as Total from tblJobOrders 
                Where Cast(GETDATE() as date) = Cast(EntryDate as date) and  OrganizationId={0} and BranchId={1}", orgId, branchId)).ToList();
        }

        public IEnumerable<DashboardDailyBillingAndWarrantyJobDTO> DashboardDailyBillingAndWarrantyJob(long orgId, long branchId)
        {
            return this._frontDeskUnitOfWork.Db.Database.SqlQuery<DashboardDailyBillingAndWarrantyJobDTO>(
                string.Format(@"Select 
(select ISNULL(COUNT(JobOrderType),0)'Billing' from tblJobOrders 
Where Cast(GETDATE() as date) = Cast(EntryDate as date) and  OrganizationId={0} and BranchId={1} and JobOrderType='Billing')'Billing',

(select ISNULL(COUNT(JobOrderType),0)'Warrenty' from tblJobOrders 
Where Cast(GETDATE() as date) = Cast(EntryDate as date) and  OrganizationId={0} and BranchId={1} and JobOrderType='Warrenty')'Warrenty'", orgId, branchId)).ToList();
        }

        public bool GetJobOrderById(long jobOrderId, long orgId, long branchId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SparePartsAvailableAndReqQtyDTO> SparePartsAvailableAndReqQty(long orgId, long branchId, long jobOrderId)
        {

            string query = string.Format(@"Select *,ISNULL((
Select SUM(jobDetail.Quantity) 'RequisitionQty' From [FrontDesk].dbo.tblRequsitionDetailForJobOrders jobDetail 
Inner Join [FrontDesk].dbo.tblRequsitionInfoForJobOrders jobinfo on jobDetail.JobOrderId = jobinfo.JobOrderId and jobinfo.StateStatus='Pending' and jobinfo.OrganizationId = {0} and jobinfo.BranchId={1} and jobInfo.RequsitionInfoForJobOrderId = jobDetail.RequsitionInfoForJobOrderId
Where jobinfo.JobOrderId={2} and tbl.MobilePartId = jobDetail.PartsId
Group By jobDetail.PartsId

),0) 'RequistionQty' From (Select parts.MobilePartId,parts.MobilePartName,SUM(ISNULL(stock.StockInQty,0)-ISNULL(stock.StockOutQty,0)) 'AvailableQty' From [Configuration].dbo.tblMobilePartStockInfo stock
Inner Join [Configuration].dbo.tblMobileParts parts on stock.MobilePartId = parts.MobilePartId and stock.OrganizationId = parts.OrganizationId
Where stock.OrganizationId = {0} and stock.BranchId={1}
Group By parts.MobilePartId,parts.MobilePartName) tbl", orgId, branchId, jobOrderId);

            return this._frontDeskUnitOfWork.Db.Database.SqlQuery<SparePartsAvailableAndReqQtyDTO>(
                query).ToList();
        }

        public bool UpdateJobOrderTsRemarks(long jobOrderId, string remarks, long userId, long orgId, long branchId)
        {
            var jobOrder = GetJobOrderById(jobOrderId, orgId);
           // var jobOrderStatus = jobOrder.TSRemarks;
           // _jobOrderRepository.Delete(jobOrderStatus);//Delete

            if (jobOrder != null)
            {
                jobOrder.JodOrderId = jobOrderId;
                jobOrder.TSRemarks = remarks.Trim();
                jobOrder.UpUserId = userId;
                jobOrder.UpdateDate = DateTime.Now;
                _jobOrderRepository.Update(jobOrder);
            }
            return _jobOrderRepository.Save();
        }

        public IEnumerable<DashboardApprovedRequsitionDTO> DashboardPendingRequsition(long orgId, long branchId)
        {
            return this._frontDeskUnitOfWork.Db.Database.SqlQuery<DashboardApprovedRequsitionDTO>(
                string.Format(@"select JodOrderId,rq.RequsitionInfoForJobOrderId,jo.JobOrderCode,RequsitionCode,rq.StateStatus,rq.EntryDate from tblJobOrders jo
                Inner join tblRequsitionInfoForJobOrders rq on jo.JodOrderId=rq.JobOrderId
                Where Cast(GETDATE() as date) = Cast(rq.EntryDate as date) and  rq.StateStatus='Pending' and  jo.OrganizationId={0} and jo.BranchId={1}", orgId, branchId)).ToList();
        }

        public IEnumerable<DashboardApprovedRequsitionDTO> DashboardCurrentRequsition(long orgId, long branchId)
        {
            return this._frontDeskUnitOfWork.Db.Database.SqlQuery<DashboardApprovedRequsitionDTO>(
                string.Format(@"select JodOrderId,rq.RequsitionInfoForJobOrderId,jo.JobOrderCode,RequsitionCode,rq.StateStatus,rq.EntryDate from tblJobOrders jo
                Inner join tblRequsitionInfoForJobOrders rq on jo.JodOrderId=rq.JobOrderId
                Where Cast(GETDATE() as date) = Cast(rq.EntryDate as date) and  rq.StateStatus='Current' and  jo.OrganizationId={0} and jo.BranchId={1}", orgId, branchId)).ToList();
        }

        public bool UpdateJobSingOutStatus(long jobOrderId, long userId, long orgId, long branchId)
        {
            var jobOrder = GetJobOrderById(jobOrderId, orgId);
            //var jobStatus = jobOrder.TsRepairStatus == "REPAIR AND RETURN" ? JobOrderStatus.RepairDone : JobOrderStatus.JobInitiated;
            if (jobOrder != null)
            {
                jobOrder.JodOrderId = jobOrderId;

                if (jobOrder.TsRepairStatus == "QC")
                {
                    jobOrder.StateStatus = JobOrderStatus.QCAssigned;
                }
                else if(jobOrder.TsRepairStatus == "MODULE SWAP")
                {
                    jobOrder.StateStatus = JobOrderStatus.HandSetChange;
                }
                else
                {
                    jobOrder.StateStatus = JobOrderStatus.JobInitiated;
                }
                jobOrder.UpUserId = userId;
                jobOrder.UpdateDate = DateTime.Now;
                _jobOrderRepository.Update(jobOrder);
            }
            return _jobOrderRepository.Save();
        }

        public bool UpdateJobOrderDeliveryStatus(long jobOrderId, long userId, long orgId, long branchId)
        {
            var jobOrder = GetJobOrderById(jobOrderId, orgId);
            if (jobOrder != null)
            {
                jobOrder.StateStatus = JobOrderStatus.DeliveryDone;
                jobOrder.CUserId = userId;
                jobOrder.CloseDate = DateTime.Now;
                _jobOrderRepository.Update(jobOrder);
            }
            return _jobOrderRepository.Save();
        }

        public JobOrderDTO GetJobOrderReceipt(long jobOrderId, long userId, long orgId, long branchId)
        {
            JobOrderDTO jobOrder = new JobOrderDTO();
            if (UpdateJobOrderDeliveryStatus(jobOrderId, userId, orgId, branchId))
            {
                jobOrder= _jobOrderReportBusiness.GetReceiptForJobOrder(jobOrderId, orgId, branchId);
            }
            return jobOrder;
        }

        public JobOrder GetReferencesNumberByMobileNumber(string mobileNumber, long orgId, long branchId)
        {
            mobileNumber = mobileNumber.Trim();
            return _jobOrderRepository.GetAll(job => job.MobileNo == mobileNumber.ToString() && job.OrganizationId == orgId && job.BranchId == branchId).OrderByDescending(o => o.JodOrderId).FirstOrDefault();
        }

        

        public bool IsIMEIExistWithRunningJobOrder(long jobOrderId, string iMEI1, long orgId, long branchId)
        {
            bool IsExist = false;
            //job.JodOrderId== jobOrderId && 
            var jobOrder = _jobOrderRepository.GetAll(job => job.IMEI == iMEI1 && job.OrganizationId == orgId && job.BranchId == branchId).LastOrDefault();
            if (jobOrder != null) {
                if(jobOrder.StateStatus != JobOrderStatus.DeliveryDone)
                {
                    IsExist = true;
                }
            }
            return IsExist;
        }

        public JobOrder GetAllJobOrderById(long branchId, long orgId)
        {
            return _jobOrderRepository.GetOneByOrg(job => job.BranchId==branchId && job.OrganizationId==orgId);
        }

        public IEnumerable<JobOrderDTO> GetJobCreateReceipt(long jobOrderId, long orgId, long branchId)
        {
            return this._frontDeskUnitOfWork.Db.Database.SqlQuery<JobOrderDTO>(
                string.Format(@"Select JodOrderId,TsRepairStatus,JobOrderCode,CustomerName,MobileNo,[Address],ModelName,IsWarrantyAvailable,IsWarrantyPaperEnclosed,StateStatus,JobOrderType,EntryDate,EntryUser,
CloseDate,TSRemarks,
SUBSTRING(FaultName,1,LEN(FaultName)-1) 'FaultName',SUBSTRING(ServiceName,1,LEN(ServiceName)-1) 'ServiceName',
SUBSTRING(AccessoriesNames,1,LEN(AccessoriesNames)-1) 'AccessoriesNames',SUBSTRING(PartsName,1,LEN(PartsName)-1) 'PartsName',
SUBSTRING(Problems,1,LEN(Problems)-1) 'Problems',TSId,TSName,RepairDate,
IMEI,[Type],ModelColor,WarrantyDate,Remarks,ReferenceNumber,IMEI2,CloseUser,InvoiceCode,InvoiceInfoId,CustomerType,ApproxBill
From (Select jo.JodOrderId,jo.CustomerName,jo.MobileNo,jo.[Address],de.ModelName,jo.IsWarrantyAvailable,jo.InvoiceInfoId,jo.IsWarrantyPaperEnclosed,jo.JobOrderType,jo.StateStatus,jo.EntryDate,ap.UserName 'EntryUser',jo.CloseDate,jo.InvoiceCode,jo.CustomerType,jo.ApproxBill,

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
Where tstock.JobOrderId = jo.JodOrderId
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
Inner Join [Configuration].dbo.tblModelSS de on jo.DescriptionId = de.ModelId
Inner Join [ControlPanel].dbo.tblApplicationUsers ap on jo.EUserId = ap.UserId

Where jo.JodOrderId={0} and  jo.OrganizationId={1} and jo.BranchId={2}) tbl Order By EntryDate desc", jobOrderId, orgId, branchId)).ToList();
        }

        public bool IsIMEI2ExistWithRunningJobOrder(long jobOrderId, string iMEI2, long orgId, long branchId)
        {
            bool IsExist = false;
            //job.JodOrderId== jobOrderId && 
            var jobOrder = _jobOrderRepository.GetAll(job => job.IMEI2 == iMEI2 && job.OrganizationId == orgId && job.BranchId == branchId).LastOrDefault();
            if (jobOrder != null)
            {
                if (jobOrder.StateStatus != JobOrderStatus.DeliveryDone)
                {
                    IsExist = true;
                }
            }
            return IsExist;
        }

        public IEnumerable<JobOrderDTO> JobOrderTransfer(long orgId, long branchId)
        {
            return _frontDeskUnitOfWork.Db.Database.SqlQuery<JobOrderDTO>(QueryForJobOrderTransfer(orgId, branchId)).ToList();
        }
        private string QueryForJobOrderTransfer(long orgId, long branchId)
        {
            string query = string.Empty;
            string param = string.Empty;
           
            if (orgId > 0)
            {
                param += string.Format(@"and jo.OrganizationId={0}", orgId);
            }
            if (branchId > 0)
            {
                param += string.Format(@"and jo.BranchId={0}", branchId);
            }

            query = string.Format(@"Select JodOrderId,JobOrderCode,CustomerName,MobileNo,[Address],ModelName,IsWarrantyAvailable,IsWarrantyPaperEnclosed,StateStatus,JobOrderType,EntryDate,EntryUser,CourierName,CourierNumber,
SUBSTRING(AccessoriesNames,1,LEN(AccessoriesNames)-1) 'AccessoriesNames',
SUBSTRING(Problems,1,LEN(Problems)-1) 'Problems',TSId,TSName
From (Select jo.JodOrderId,jo.CustomerName,jo.MobileNo,jo.[Address],de.DescriptionName 'ModelName',jo.IsWarrantyAvailable,jo.IsWarrantyPaperEnclosed,jo.JobOrderType,jo.StateStatus,jo.EntryDate,ap.UserName 'EntryUser',jo.CourierName,jo.CourierNumber,

Cast((Select AccessoriesName+',' From [Configuration].dbo.tblAccessories ass
Inner Join tblJobOrderAccessories joa on ass.AccessoriesId = joa.AccessoriesId
Where joa.JobOrderId = jo.JodOrderId
Order BY AccessoriesName For XML PATH('')) as nvarchar(MAX))  'AccessoriesNames',

Cast((Select ProblemName+',' From [Configuration].dbo.tblClientProblems prob
Inner Join tblJobOrderProblems jop on prob.ProblemId = jop.ProblemId
Where jop.JobOrderId = jo.JodOrderId
Order BY ProblemName For XML PATH(''))as nvarchar(MAX)) 'Problems',jo.JobOrderCode,jo.TSId,ts.Name 'TSName'

from tblJobOrders jo
Inner Join [Inventory].dbo.tblDescriptions de on jo.DescriptionId = de.DescriptionId
Left Join [Configuration].dbo.tblTechnicalServiceEngs ts on jo.TSId =ts.EngId
Inner Join [ControlPanel].dbo.tblApplicationUsers ap on jo.EUserId = ap.UserId Where 1 = 1 and jo.IsTransfer is null and jo.StateStatus='Job-Initiated' {0}) tbl Order By EntryDate desc", Utility.ParamChecker(param));
            return query;
        }

        public bool SaveJobOrderTransfer(long transferId,long[] jobOrders, long userId, long orgId, long branchId, string cName, string cNumber)
        {
            foreach(var job in jobOrders)
            {
                var jobOrderInDb = GetJobOrdersByIdWithBranch(job, branchId, orgId);
                if (jobOrderInDb != null )
                {
                    jobOrderInDb.IsTransfer = true;
                    jobOrderInDb.JobLocation = transferId;
                    jobOrderInDb.CourierName = cName;
                    jobOrderInDb.CourierNumber = cNumber;
                    jobOrderInDb.UpUserId = userId;
                    jobOrderInDb.UpdateDate = DateTime.Now;
                    _jobOrderRepository.Update(jobOrderInDb);
                }
            }
            return _jobOrderRepository.Save();
        }

        public JobOrder GetJobOrdersByIdWithTransferBranch(long jobOrderId, long transferId, long orgId)
        {
            return _jobOrderRepository.GetOneByOrg(j => j.JodOrderId == jobOrderId && j.TransferBranchId == transferId && j.OrganizationId == orgId);
        }

        public IEnumerable<JobOrderDTO> TransferReceiveJobOrder(long orgId, long branchId, long? branchName)
        {
            return _frontDeskUnitOfWork.Db.Database.SqlQuery<JobOrderDTO>(QueryForTransferReceiveJobOrder(orgId, branchId,branchName)).ToList();
        }
        private string QueryForTransferReceiveJobOrder(long orgId, long branchId, long? branchName)
        {
            string query = string.Empty;
            string param = string.Empty;
            
            if (orgId > 0)
            {
                param += string.Format(@"and jo.OrganizationId={0}", orgId);
            }
            if (branchId > 0)
            {
                param += string.Format(@"and jo.TransferBranchId={0}", branchId);
            }
            if (branchName != null && branchName > 0)
            {
                param += string.Format(@"and jo.BranchId={0}", branchName);
            }

            query = string.Format(@"Select JodOrderId,JobOrderCode,CustomerName,MobileNo,CourierName,CourierNumber,[Address],ModelName,IsWarrantyAvailable,IsWarrantyPaperEnclosed,StateStatus,JobOrderType,EntryDate,EntryUser,
SUBSTRING(AccessoriesNames,1,LEN(AccessoriesNames)-1) 'AccessoriesNames',
SUBSTRING(Problems,1,LEN(Problems)-1) 'Problems',TSId,TSName,BranchId
From (Select jo.JodOrderId,jo.CustomerName,jo.MobileNo,jo.[Address],de.DescriptionName 'ModelName',jo.IsWarrantyAvailable,jo.IsWarrantyPaperEnclosed,jo.JobOrderType,jo.StateStatus,jo.EntryDate,ap.UserName 'EntryUser',jo.CourierName,jo.CourierNumber,

Cast((Select AccessoriesName+',' From [Configuration].dbo.tblAccessories ass
Inner Join tblJobOrderAccessories joa on ass.AccessoriesId = joa.AccessoriesId
Where joa.JobOrderId = jo.JodOrderId
Order BY AccessoriesName For XML PATH('')) as nvarchar(MAX))  'AccessoriesNames',

Cast((Select ProblemName+',' From [Configuration].dbo.tblClientProblems prob
Inner Join tblJobOrderProblems jop on prob.ProblemId = jop.ProblemId
Where jop.JobOrderId = jo.JodOrderId
Order BY ProblemName For XML PATH(''))as nvarchar(MAX)) 'Problems',jo.JobOrderCode,jo.TSId,ts.Name 'TSName',jo.BranchId

from tblJobOrders jo
Inner Join [Inventory].dbo.tblDescriptions de on jo.DescriptionId = de.DescriptionId
Left Join [Configuration].dbo.tblTechnicalServiceEngs ts on jo.TSId =ts.EngId
Inner Join [ControlPanel].dbo.tblApplicationUsers ap on jo.EUserId = ap.UserId Where 1 = 1 and jo.IsTransfer='True' and (jo.StateStatus='Repair-Done' OR jo.StateStatus='Job-Initiated'){0}) tbl Order By EntryDate desc", Utility.ParamChecker(param));
            return query;
        }

        public bool SaveTransferReceiveJob(long transferId, long[] jobOrders, long userId, long orgId, long branchId)
        {
            foreach (var job in jobOrders)
            {
                var jobOrderInDb = GetJobOrdersByIdWithBranch(job, branchId, orgId);
                jobOrderInDb = jobOrderInDb == null ? GetJobOrdersByIdWithTransferBranch(job, branchId, orgId) : jobOrderInDb;
                if (jobOrderInDb != null)
                {
                    jobOrderInDb.TransferBranchId = branchId;
                    jobOrderInDb.IsTransfer = false;
                    jobOrderInDb.UpUserId = userId;
                    jobOrderInDb.UpdateDate = DateTime.Now;
                    _jobOrderRepository.Update(jobOrderInDb);
                }
            }
            return _jobOrderRepository.Save();
        }

        public bool UpdateTransferJob(long jobOrderId, long userId, long orgId, long branchId)
        {
            var jobOrderInDb = GetJobOrderById(jobOrderId, orgId);
            if (jobOrderInDb != null)
            {
                jobOrderInDb.TransferBranchId = branchId;
                jobOrderInDb.UpUserId = userId;
                jobOrderInDb.UpdateDate = DateTime.Now;
                _jobOrderRepository.Update(jobOrderInDb);
            }
            return _jobOrderRepository.Save();
        }

        public JobOrder GetAllJobOrderByOrgId(long orgId)
        {
            return _jobOrderRepository.GetOneByOrg(o => o.OrganizationId == orgId);
        }

        public bool SaveJobOrderReturnAndUpdateJobOrder(long transferId, long[] jobOrders, long userId, long orgId, long branchId,string cName,string cNumber)
        {
            foreach (var job in jobOrders)
            {
                var jobOrderInDb = GetJobOrdersByIdWithBranch(job, transferId, orgId);
                if (jobOrderInDb != null)
                {
                    jobOrderInDb.IsTransfer = false;
                    jobOrderInDb.JobLocation = transferId;
                    jobOrderInDb.CourierName = cName;
                    jobOrderInDb.CourierNumber = cNumber;
                    jobOrderInDb.UpUserId = userId;
                    jobOrderInDb.UpdateDate = DateTime.Now;
                    _jobOrderRepository.Update(jobOrderInDb);
                }
            }
            return _jobOrderRepository.Save();
        }

        public bool UpdateReturnJob(long jobOrderId, long userId,long orgId)
        {
            var jobOrderInDb = GetJobOrderById(jobOrderId,orgId);
            if (jobOrderInDb != null)
            {
                jobOrderInDb.IsReturn = true;
                jobOrderInDb.UpUserId = userId;
                jobOrderInDb.UpdateDate = DateTime.Now;
                _jobOrderRepository.Update(jobOrderInDb);
            }
            return _jobOrderRepository.Save();
        }

        public JobOrderDTO GetJobOrderDetails(long jobOrderId, long orgId)
        {
            var data= this._frontDeskUnitOfWork.Db.Database.SqlQuery<JobOrderDTO>(
                string.Format(@"Select JodOrderId,TsRepairStatus,JobOrderCode,CustomerName,MobileNo,[Address],ModelName,IsWarrantyAvailable,IsWarrantyPaperEnclosed,StateStatus,JobOrderType,EntryDate,EntryUser,
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

Where 1 = 1 and jo.JodOrderId ={0} and jo.OrganizationId={1}
) tbl Order By EntryDate desc

", jobOrderId, orgId)).FirstOrDefault();
            return data;
        }

        public IEnumerable<DashboardApprovedRequsitionDTO> DashboardAnotherBranchRequsition(long orgId, long branchId)
        {
            var data= this._frontDeskUnitOfWork.Db.Database.SqlQuery<DashboardApprovedRequsitionDTO>(
                string.Format(@"Select RequsitionInfoForJobOrderId,RequsitionCode,j.JobOrderCode,q.StateStatus,
JobOrderId,q.EntryDate From tblRequsitionInfoForJobOrders q
left join tblJobOrders j on q.JobOrderId=j.JodOrderId
inner join[ControlPanel].dbo.tblApplicationUsers app on q.OrganizationId = app.OrganizationId and q.EUserId= app.UserId 
where Cast(GETDATE() as date) = Cast(q.EntryDate as date) and j.IsTransfer='True' and j.TransferBranchId={0} and q.OrganizationId={1}", branchId, orgId)).ToList();
            return data;
        }

        public IEnumerable<DailySummaryReportDTO> DailySummaryReport(long orgId, long branchId, string fromDate, string toDate)
        {
            fromDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
            var query = string.Format(@"Exec [dbo].[spDailySummeryForBranch] '{0}','{1}','{2}','{3}'", fromDate, toDate, branchId, orgId);
            return _frontDeskUnitOfWork.Db.Database.SqlQuery<DailySummaryReportDTO>(query).ToList();
        }
        private string QueryForDailySummaryReport(long orgId, long branchId, string fromDate, string toDate)
        {
            string query = string.Empty;
            string param = string.Empty;

            var q= string.Format(@"Exec YourProceName '{0}','{1}','{2}','{3}'", fromDate, toDate,branchId,orgId);

//            if (orgId > 0)
//            {
//                param += string.Format(@"and jo.OrganizationId={0}", orgId);
//            }
//            if (branchId > 0)
//            {
//                param += string.Format(@"and jo.BranchId={0}", branchId);
//            }
//            if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "" && !string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
//            {
//                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
//                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
//                param += string.Format(@" and Cast(jo.EntryDate as date) between '{0}' and '{1}'", fDate, tDate);
//            }
//            else if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "")
//            {
//                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
//                param += string.Format(@" and Cast(jo.EntryDate as date)='{0}'", fDate);
//            }
//            else if (!string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
//            {
//                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
//                param += string.Format(@" and Cast(jo.EntryDate as date)='{0}'", tDate);
//            }
//            query = string.Format(@"Select  Cast(jo.EntryDate as Date) 'EntryDate',
//Count(jo.JobOrderCode)'DailyJobOrder',
//(Select count(JobOrderType) From tblJobOrders
//Where JobOrderType='Billing' and BranchId=jo.BranchId and Cast(EntryDate as Date) = Cast(jo.EntryDate as Date))'BillingJob',

//(Select count(JobOrderType) From tblJobOrders
//Where JobOrderType='Warrenty' and BranchId=jo.BranchId and Cast(EntryDate as Date) = Cast(jo.EntryDate as Date))'WarrentyJob',

//(select Sum(NetAmount) From tblInvoiceInfo
//Where Cast(EntryDate as Date) = Cast(jo.EntryDate as Date) and BranchId=jo.BranchId)'CASH',

//(Select count(StateStatus) From tblJobOrders
//Where StateStatus='TS-Assigned' and BranchId=jo.BranchId and Cast(EntryDate as Date) = Cast(jo.EntryDate as Date))'TSBacklog',

//(Select count(StateStatus) From tblJobOrders
//Where StateStatus='TS-Assigned' and BranchId=jo.BranchId And Cast(EntryDate as date) <= Cast(jo.EntryDate as date))'TSOverAllBacklog',

//(Select count(StateStatus) From tblJobOrders
//Where StateStatus='Job-Initiated' and BranchId=jo.BranchId and Cast(EntryDate as Date) = Cast(jo.EntryDate as Date))'TSSignInPending',

//(Select count(TsRepairStatus) From tblJobOrders
//Where TsRepairStatus='REPAIR AND RETURN' and BranchId=jo.BranchId and Cast(EntryDate as Date) = Cast(jo.EntryDate as Date))'RepairAndReturn',

//(Select count(TsRepairStatus) From tblJobOrders
//Where TsRepairStatus='RETURN WITHOUT REPAIR' and BranchId=jo.BranchId and Cast(EntryDate as Date) = Cast(jo.EntryDate as Date))'ReturnWithoutRepair',

//(Select count(TsRepairStatus) From tblJobOrders
//Where TsRepairStatus='WORK IN PROGRESS' and BranchId=jo.BranchId and Cast(EntryDate as Date) = Cast(jo.EntryDate as Date))'WorkInProgress',

//(Select count(TsRepairStatus) From tblJobOrders
//Where TsRepairStatus='PENDING FOR SPARE PARTS' and BranchId=jo.BranchId and Cast(EntryDate as Date) = Cast(jo.EntryDate as Date))'PendingForSpareParts',

//(Select count(TsRepairStatus) From tblJobOrders
//Where TsRepairStatus='PENDING FOR APPROVAL' and BranchId=jo.BranchId and Cast(EntryDate as Date) = Cast(jo.EntryDate as Date))'PendingForApproval',

//(Select count(StateStatus) From tblJobOrders
//Where StateStatus='Delivery-Done' and JobOrderType='Warrenty' and BranchId=jo.BranchId and Cast(EntryDate as Date) = Cast(jo.EntryDate as Date))'WarrentyDelivery',

//(Select count(StateStatus) From tblJobOrders
//Where StateStatus='Delivery-Done' and JobOrderType='Billing' and BranchId=jo.BranchId and Cast(EntryDate as Date) = Cast(jo.EntryDate as Date))'BillingDelivery',

//(Select count(StateStatus) From tblJobOrders
//Where StateStatus='Delivery-Done' and BranchId=jo.BranchId and Cast(EntryDate as Date) = Cast(jo.EntryDate as Date))'TotalDelivery',

//(Select count(StateStatus) From tblJobOrders
//Where StateStatus='Repair-Done' and BranchId=jo.BranchId and Cast(EntryDate as Date) = Cast(jo.EntryDate as Date))'DeliveryPending',

//(Select count(StateStatus) From tblJobOrders
//Where (StateStatus='Repair-Done' or StateStatus='Job-Initiated' or StateStatus='TS-Assigned') and JobOrderType='Warrenty' and BranchId=jo.BranchId  And Cast(EntryDate as date) <= Cast(jo.EntryDate as date))'OverAllWarrentyUnDelivered',

//(Select count(StateStatus) From tblJobOrders
//Where (StateStatus='Repair-Done' or StateStatus='Job-Initiated' or StateStatus='TS-Assigned') and JobOrderType='Billing' and BranchId=jo.BranchId  And Cast(EntryDate as date) <= Cast(jo.EntryDate as date)
//)'OverAllBillingUnDelivered',

//(Select count(StateStatus) From tblJobOrders
//Where (StateStatus='Repair-Done' or StateStatus='Job-Initiated' or StateStatus='TS-Assigned') and BranchId=jo.BranchId 
//And Cast(EntryDate as date) <= Cast(jo.EntryDate as date))'TotalUnDelivered'

//From tblJobOrders jo
//where 1=1{0}
//Group By  jo.BranchId ,Cast(jo.EntryDate as Date)
//Order By EntryDate desc", Utility.ParamChecker(param));
            return query;
        }

        public IEnumerable<DailySummaryReportDTO> AllBranchDailySummaryReport(long orgId, string fromDate, string toDate)
        {
            fromDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
            var query = string.Format(@"Exec [dbo].[spAllBranchDailySummery] '{0}','{1}','{2}'", fromDate, toDate,orgId);
            return _frontDeskUnitOfWork.Db.Database.SqlQuery<DailySummaryReportDTO>(query).ToList();
        }
        private string QueryForAllBranchDailySummaryReport(long orgId, string fromDate, string toDate)
        {
            string query = string.Empty;
            string param = string.Empty;

            if (orgId > 0)
            {
                param += string.Format(@"and jo.OrganizationId={0}", orgId);
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
            query = string.Format(@"Select  Cast(jo.EntryDate as Date) 'EntryDate',

(Select '('+(b.ShortName+'-' +  Cast((select Count(JobOrderCode)) as Nvarchar(100)))+') '  from tblJobOrders j
inner join [ControlPanel].dbo.tblBranch b on b.BranchId=j.BranchId
where Cast(j.EntryDate as Date) = Cast(jo.EntryDate as Date)
Group by b.ShortName
Order BY (b.ShortName+'-' + Cast((select Count(JobOrderCode)) as Nvarchar(100))
) For XML PATH(''))'BranchWiseDailyJob',


(select Count(JobOrderCode)'TOTAL' from tblJobOrders
Where Cast(EntryDate as Date) = Cast(jo.EntryDate as Date)) 'TOTAL',

(Select count(JobOrderType) From tblJobOrders
Where JobOrderType='Billing' and Cast(EntryDate as Date) = Cast(jo.EntryDate as Date))'BillingJob',

(Select count(JobOrderType) From tblJobOrders
Where JobOrderType='Warrenty'  and Cast(EntryDate as Date) = Cast(jo.EntryDate as Date))'WarrentyJob',

(select Sum(NetAmount) From tblInvoiceInfo
Where Cast(EntryDate as Date) = Cast(jo.EntryDate as Date))'CASH',

(Select count(StateStatus) From tblJobOrders
Where StateStatus='TS-Assigned'  and Cast(EntryDate as Date) = Cast(jo.EntryDate as Date))'TSBacklog',

(Select count(StateStatus) From tblJobOrders
Where StateStatus='TS-Assigned'  And Cast(EntryDate as date) <= Cast(jo.EntryDate as date))'TSOverAllBacklog',

(Select count(StateStatus) From tblJobOrders
Where StateStatus='Job-Initiated' and Cast(EntryDate as Date) = Cast(jo.EntryDate as Date))'TSSignInPending',

(Select count(TsRepairStatus) From tblJobOrders
Where TsRepairStatus='REPAIR AND RETURN' and Cast(EntryDate as Date) = Cast(jo.EntryDate as Date))'RepairAndReturn',

(Select count(TsRepairStatus) From tblJobOrders
Where TsRepairStatus='RETURN WITHOUT REPAIR' and Cast(EntryDate as Date) = Cast(jo.EntryDate as Date))'ReturnWithoutRepair',

(Select count(TsRepairStatus) From tblJobOrders
Where TsRepairStatus='WORK IN PROGRESS' and Cast(EntryDate as Date) = Cast(jo.EntryDate as Date))'WorkInProgress',

(Select count(TsRepairStatus) From tblJobOrders
Where TsRepairStatus='PENDING FOR SPARE PARTS' and Cast(EntryDate as Date) = Cast(jo.EntryDate as Date))'PendingForSpareParts',

(Select count(TsRepairStatus) From tblJobOrders
Where TsRepairStatus='PENDING FOR APPROVAL' and Cast(EntryDate as Date) = Cast(jo.EntryDate as Date))'PendingForApproval',

(Select count(StateStatus) From tblJobOrders
Where StateStatus='Delivery-Done' and JobOrderType='Warrenty' and Cast(EntryDate as Date) = Cast(jo.EntryDate as Date))'WarrentyDelivery',

(Select count(StateStatus) From tblJobOrders
Where StateStatus='Delivery-Done' and JobOrderType='Billing' and Cast(EntryDate as Date) = Cast(jo.EntryDate as Date))'BillingDelivery',

(Select count(StateStatus) From tblJobOrders
Where StateStatus='Delivery-Done' and Cast(EntryDate as Date) = Cast(jo.EntryDate as Date))'TotalDelivery',

(Select count(StateStatus) From tblJobOrders
Where StateStatus='Repair-Done' and Cast(EntryDate as Date) = Cast(jo.EntryDate as Date))'DeliveryPending',

(Select count(StateStatus) From tblJobOrders
Where (StateStatus='Repair-Done' or StateStatus='Job-Initiated' or StateStatus='TS-Assigned') and JobOrderType='Warrenty' And Cast(EntryDate as date) <= Cast(jo.EntryDate as date))'OverAllWarrentyUnDelivered',

(Select count(StateStatus) From tblJobOrders
Where (StateStatus='Repair-Done' or StateStatus='Job-Initiated' or StateStatus='TS-Assigned') and JobOrderType='Billing'  And Cast(EntryDate as date) <= Cast(jo.EntryDate as date))'OverAllBillingUnDelivered',

(Select count(StateStatus) From tblJobOrders
Where (StateStatus='Repair-Done' or StateStatus='Job-Initiated' or StateStatus='TS-Assigned') And Cast(EntryDate as date) <= Cast(jo.EntryDate as date))'TotalUnDelivered'

From tblJobOrders jo
where 1=1{0}
Group By  Cast(jo.EntryDate as Date)
Order By EntryDate desc", Utility.ParamChecker(param));
            return query;
        }

        public JobOrder GetReferencesNumberByJobOrder(string jobOrder, long orgId, long branchId)
        {
            jobOrder = jobOrder.Trim();
            return _jobOrderRepository.GetAll(job => job.JobOrderCode == jobOrder.ToString() && job.OrganizationId == orgId && job.BranchId == branchId).FirstOrDefault();
        }

        public IEnumerable<DashboardDailyReceiveJobOrderDTO> DashboardDailyTransferJob(long orgId, long branchId)
        {
            return this._frontDeskUnitOfWork.Db.Database.SqlQuery<DashboardDailyReceiveJobOrderDTO>(
                string.Format(@"Select(Select count(TransferCode)'Total' From tblJobOrderTransferDetail
Where  Cast(EntryDate as date)=Cast(GETDATE()  as date) and OrganizationId={0} and BranchId={1})'TransferJob',

(Select count(TransferCode)'Total' From tblJobOrderReturnDetails
Where  Cast(EntryDate as date)=Cast(GETDATE()  as date) and OrganizationId={0} and BranchId={1})'ReturnJob'", orgId, branchId)).ToList();
        }

        public IEnumerable<DashboardDailyReceiveJobOrderDTO> DashboardDailyReceiveJob(long orgId, long branchId)
        {
            return this._frontDeskUnitOfWork.Db.Database.SqlQuery<DashboardDailyReceiveJobOrderDTO>(
                string.Format(@"select(Select count(TransferCode)'Total' From tblJobOrderTransferDetail
Where  Cast(UpDateDate as date)=Cast(GETDATE()  as date) and OrganizationId={0} and ToBranch={1}
and TransferStatus='Received')'ReceiveJob',

(Select count(TransferCode)'Total' From tblJobOrderReturnDetails
Where  Cast(UpDateDate as date)=Cast(GETDATE()  as date) and OrganizationId={0} and ToBranch={1}
and TransferStatus='Received')'ReceiveReturnJob'", orgId, branchId)).ToList();
        }

        public IEnumerable<DashboardSellsDTO> DashboardDailySells(long orgId, long branchId)
        {
            return this._frontDeskUnitOfWork.Db.Database.SqlQuery<DashboardSellsDTO>(
                string.Format(@"select ISNULL(Sum(NetAmount),0)'Total' from tblInvoiceInfo
Where OrganizationId={0} and BranchId={1} and Cast(GETDATE() as date) = Cast(EntryDate as date)", orgId, branchId)).ToList();
        }

        public IEnumerable<DashboardSellsDTO> DashboardTotalSells(long orgId, long branchId)
        {
            return this._frontDeskUnitOfWork.Db.Database.SqlQuery<DashboardSellsDTO>(
                string.Format(@"select ISNULL(Sum(NetAmount),0)'Total' from tblInvoiceInfo
Where OrganizationId={0} and BranchId={1}", orgId, branchId)).ToList();
        }

        public IEnumerable<DailySellsChart> DailySellsChart(string fromDate, string toDate, long orgId, long branchId)
        {
            IEnumerable<DailySellsChart> data = new List<DailySellsChart>();
            if(orgId>0 && branchId > 0)
            {
                return this._frontDeskUnitOfWork.Db.Database.SqlQuery<DailySellsChart>(
                string.Format(@"Exec [dbo].[spDailySellsChart] '{0}','{1}',{2},{3}",fromDate,toDate, branchId, orgId)).ToList();
            }
            return data;
        }

        public IEnumerable<DashboardDailyReceiveJobOrderDTO> DashboardNotAssignJob(long orgId, long branchId)
        {
            return this._frontDeskUnitOfWork.Db.Database.SqlQuery<DashboardDailyReceiveJobOrderDTO>(
                string.Format(@"select ISNULL(Count(StateStatus),0)'Total' 
from tblJobOrders where OrganizationId={0} and ((BranchId={1} and JobLocation={1}) or (TransferBranchId={1} and IsTransfer='True')) 
and StateStatus='Job-Initiated' ", orgId, branchId)).ToList(); 
        }

        public bool SaveCallCenterApproval(long jobId, string approval, string remarks, long userId, long orgId)
        {
            var jobOrderInDb = GetJobOrderById(jobId, orgId);
            if(jobOrderInDb != null)
            {
                jobOrderInDb.CustomerApproval = approval;
                jobOrderInDb.CallCenterRemarks = remarks;
                jobOrderInDb.UpdateDate = DateTime.Now;
                jobOrderInDb.UpUserId = userId;
                _jobOrderRepository.Update(jobOrderInDb);
            }
            return _jobOrderRepository.Save();
        }

        public IEnumerable<JobOrderDTO> DashboardCallCenterApproval(long orgId, long branchId, long userId)
        {
            return this._frontDeskUnitOfWork.Db.Database.SqlQuery<JobOrderDTO>(
                string.Format(@"Select JodOrderId,JobOrderCode,CustomerApproval from tblJobOrders
Where OrganizationId={0} and BranchId={1} and TsRepairStatus='PENDING FOR APPROVAL' and (CustomerApproval='Approved' or CustomerApproval='DisApproved') and TSId={2}", orgId, branchId,userId)).ToList();
        }

        public IEnumerable<JobOrderDTO> GetJobOrderForQc(string mobileNo, long? modelId, string status, long? jobOrderId, string jobCode, string iMEI, string iMEI2, long orgId, long branchId, string fromDate, string toDate, string customerType, string jobType, string repairStatus)
        {
            return _frontDeskUnitOfWork.Db.Database.SqlQuery<JobOrderDTO>(QueryForJobOrderQC(mobileNo, modelId, status, jobOrderId, jobCode, iMEI, iMEI2, orgId, branchId, fromDate, toDate, customerType, jobType, repairStatus)).ToList();
        }
        private string QueryForJobOrderQC(string mobileNo, long? modelId, string status, long? jobOrderId, string jobCode, string iMEI, string iMEI2, long orgId, long branchId, string fromDate, string toDate, string customerType, string jobType, string repairStatus)
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
                    param += string.Format(@"and de.ModelId ={0}", modelId);
                }
                if (!string.IsNullOrEmpty(status))
                {
                    param += string.Format(@"and jo.StateStatus ='{0}'", status);
                }
                if (!string.IsNullOrEmpty(customerType))
                {
                    param += string.Format(@"and jo.CustomerType ='{0}'", customerType);
                }
                if (!string.IsNullOrEmpty(jobType))
                {
                    param += string.Format(@"and jo.JobOrderType ='{0}'", jobType);
                }
                if (!string.IsNullOrEmpty(jobCode))
                {
                    param += string.Format(@"and jo.JobOrderCode Like '%{0}%'", jobCode);
                }
                if (!string.IsNullOrEmpty(repairStatus))
                {
                    param += string.Format(@"and jo.TsRepairStatus Like '%{0}%'", repairStatus);
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
//            if (branchId > 0)
//            {
//                param += string.Format(@"and ((jo.BranchId={0} and (jo.IsTransfer is null or jo.IsTransfer='True'))
//OR 
//(jo.TransferBranchId={0} and jo.IsTransfer= 'true')
//OR
//(jo.IsReturn='True' and jo.BranchId={0} and jo.IsTransfer='False'))", branchId);
//            }
            if (branchId > 0)
            {
                param += string.Format(@"and ((jo.TsRepairStatus='QC' and jo.IsTransfer is null and jo.BranchId={0}) or (jo.TsRepairStatus='QC' and jo.IsTransfer='True' and jo.TransferBranchId={0}))", branchId);
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

            query = string.Format(@"Select JodOrderId,TsRepairStatus,JobOrderCode,CustomerName,MobileNo,[Address],ModelName,IsWarrantyAvailable,IsWarrantyPaperEnclosed,IsHandset,StateStatus,JobOrderType,EntryDate,EntryUser,
CloseDate,TSRemarks,
SUBSTRING(FaultName,1,LEN(FaultName)-1) 'FaultName',SUBSTRING(ServiceName,1,LEN(ServiceName)-1) 'ServiceName',
SUBSTRING(AccessoriesNames,1,LEN(AccessoriesNames)-1) 'AccessoriesNames',SUBSTRING(PartsName,1,LEN(PartsName)-1) 'PartsName',
SUBSTRING(Problems,1,LEN(Problems)-1) 'Problems',TSId,TSName,RepairDate,QCTransferStatus,
IMEI,[Type],ModelColor,WarrantyDate,Remarks,ReferenceNumber,IMEI2,CloseUser,InvoiceCode,InvoiceInfoId,CustomerType,CourierNumber,CourierName,ApproxBill,IsTransfer,JobLocationB,IsReturn,CustomerApproval,CallCenterRemarks,QCStatus,QCRemarks
From (Select jo.JodOrderId,jo.CustomerName,jo.MobileNo,jo.[Address],de.ModelName,jo.IsWarrantyAvailable,jo.InvoiceInfoId,jo.IsWarrantyPaperEnclosed,jo.IsHandset,jo.JobOrderType,jo.StateStatus,jo.EntryDate,ap.UserName'EntryUser',jo.CloseDate,jo.InvoiceCode,jo.CustomerType,jo.CourierNumber,jo.CourierName,jo.ApproxBill,jo.IsTransfer,jo.IsReturn,bb.BranchName'JobLocationB',jo.CustomerApproval,jo.CallCenterRemarks,jo.QCStatus,jo.QCRemarks,jo.QCTransferStatus,

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
Where jt.JodOrderId = jo.JodOrderId and (j.TsRepairStatus='REPAIR AND RETURN' or(j.TsRepairStatus='MODULE SWAP' and j.StateStatus='HandSetChange')) Order by jt.JTSId desc) 'RepairDate'

from tblJobOrders jo
Inner Join [Configuration].dbo.tblModelSS de on jo.DescriptionId = de.ModelId
Inner Join [ControlPanel].dbo.tblApplicationUsers ap on jo.EUserId = ap.UserId
Left Join [ControlPanel].dbo.tblBranch bb on jo.JobLocation=bb.BranchId

Where 1 = 1{0} and jo.StateStatus='QC-Assigned') tbl Order By JobOrderCode desc
", Utility.ParamChecker(param));
            return query;
        }

        public bool SaveQCApproval(long jobId, string approval, string remarks, long userId, long orgId,long branchId)
        {
            bool isSuccess = false;
            var jobOrderInDb = GetJobOrderById(jobId, orgId);
            if (approval == "QC-Pass")
            {
                if (jobOrderInDb != null)
                {
                    jobOrderInDb.QCStatus = approval;
                    jobOrderInDb.QCRemarks = remarks;
                    jobOrderInDb.StateStatus = JobOrderStatus.RepairDone;
                    jobOrderInDb.TsRepairStatus = "REPAIR AND RETURN";
                    jobOrderInDb.UpdateDate = DateTime.Now;
                    jobOrderInDb.UpUserId = userId;
                    _jobOrderRepository.Update(jobOrderInDb);
                    isSuccess = _jobOrderRepository.Save();
                }
            }
            else
            {
                if(jobOrderInDb != null)
                {
                    jobOrderInDb.QCStatus = approval;
                    jobOrderInDb.QCRemarks = remarks;
                    jobOrderInDb.StateStatus = JobOrderStatus.AssignToTS;
                    jobOrderInDb.UpdateDate = DateTime.Now;
                    jobOrderInDb.UpUserId = userId;
                    _jobOrderRepository.Update(jobOrderInDb);
                    isSuccess = _jobOrderRepository.Save() == true;
                   // {
                   //     isSuccess = _jobOrderTSBusiness.UpdateJobOrderTsForQcFail(jobId, userId, orgId, branchId);
                   // }
                }
            }
            
            return isSuccess;
        }

        public IEnumerable<JobOrderDTO> DashboardQCStatus(long orgId, long branchId, long userId)
        {
            return this._frontDeskUnitOfWork.Db.Database.SqlQuery<JobOrderDTO>(
                string.Format(@"Select JodOrderId,JobOrderCode,QCStatus from tblJobOrders
Where OrganizationId={0} and BranchId={1} and TsRepairStatus='QC' 
and QCStatus='QC-Fail' and TSId={2}", orgId, branchId, userId)).ToList();
        }

        public IEnumerable<JobOrderDTO> GetJobOrderForDelivery(string mobileNo, long? modelId, string status, long? jobOrderId, string jobCode, string iMEI, string iMEI2, long orgId, long branchId, string fromDate, string toDate, string customerType, string jobType, string repairStatus)
        {
            return _frontDeskUnitOfWork.Db.Database.SqlQuery<JobOrderDTO>(QueryForJobOrderDelivery(mobileNo, modelId, status, jobOrderId, jobCode, iMEI, iMEI2, orgId, branchId, fromDate, toDate, customerType, jobType, repairStatus)).ToList();
        }
        private string QueryForJobOrderDelivery(string mobileNo, long? modelId, string status, long? jobOrderId, string jobCode, string iMEI, string iMEI2, long orgId, long branchId, string fromDate, string toDate, string customerType, string jobType, string repairStatus)
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
                    param += string.Format(@"and de.ModelId ={0}", modelId);
                }
                if (!string.IsNullOrEmpty(status))
                {
                    param += string.Format(@"and jo.StateStatus ='{0}'", status);
                }
                if (!string.IsNullOrEmpty(customerType))
                {
                    param += string.Format(@"and jo.CustomerType ='{0}'", customerType);
                }
                if (!string.IsNullOrEmpty(jobType))
                {
                    param += string.Format(@"and jo.JobOrderType ='{0}'", jobType);
                }
                if (!string.IsNullOrEmpty(jobCode))
                {
                    param += string.Format(@"and jo.JobOrderCode Like '%{0}%'", jobCode);
                }
                if (!string.IsNullOrEmpty(repairStatus))
                {
                    param += string.Format(@"and jo.TsRepairStatus Like '%{0}%'", repairStatus);
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
                param += string.Format(@"and ((jo.BranchId={0} and (jo.IsTransfer is null or jo.IsTransfer='True'))
OR 
(jo.TransferBranchId={0} and jo.IsTransfer= 'true')
OR
(jo.IsReturn='True' and jo.BranchId={0} and jo.IsTransfer='False'))", branchId);
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

            query = string.Format(@"Select JodOrderId,TsRepairStatus,JobOrderCode,CustomerName,MobileNo,[Address],ModelName,IsWarrantyAvailable,IsWarrantyPaperEnclosed,IsHandset,StateStatus,JobOrderType,EntryDate,EntryUser,
CloseDate,TSRemarks,
SUBSTRING(FaultName,1,LEN(FaultName)-1) 'FaultName',SUBSTRING(ServiceName,1,LEN(ServiceName)-1) 'ServiceName',
SUBSTRING(AccessoriesNames,1,LEN(AccessoriesNames)-1) 'AccessoriesNames',SUBSTRING(PartsName,1,LEN(PartsName)-1) 'PartsName',
SUBSTRING(Problems,1,LEN(Problems)-1) 'Problems',TSId,TSName,RepairDate,
IMEI,[Type],ModelColor,WarrantyDate,Remarks,ReferenceNumber,IMEI2,CloseUser,InvoiceCode,InvoiceInfoId,CustomerType,CourierNumber,CourierName,ApproxBill,IsTransfer,JobLocationB,IsReturn,CustomerApproval,CallCenterRemarks,QCStatus,QCRemarks
From (Select jo.JodOrderId,jo.CustomerName,jo.MobileNo,jo.[Address],de.ModelName,jo.IsWarrantyAvailable,jo.InvoiceInfoId,jo.IsWarrantyPaperEnclosed,jo.IsHandset,jo.JobOrderType,jo.StateStatus,jo.EntryDate,ap.UserName'EntryUser',jo.CloseDate,jo.InvoiceCode,jo.CustomerType,jo.CourierNumber,jo.CourierName,jo.ApproxBill,jo.IsTransfer,jo.IsReturn,bb.BranchName'JobLocationB',jo.CustomerApproval,jo.CallCenterRemarks,jo.QCStatus,jo.QCRemarks,

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
Where jt.JodOrderId = jo.JodOrderId and (j.TsRepairStatus='REPAIR AND RETURN' or(j.TsRepairStatus='MODULE SWAP' and j.StateStatus='HandSetChange')) Order by jt.JTSId desc) 'RepairDate'

from tblJobOrders jo
Inner Join [Configuration].dbo.tblModelSS de on jo.DescriptionId = de.ModelId
Inner Join [ControlPanel].dbo.tblApplicationUsers ap on jo.EUserId = ap.UserId
Left Join [ControlPanel].dbo.tblBranch bb on jo.JobLocation=bb.BranchId

Where 1 = 1{0} and jo.CustomerType='Dealer' and ((jo.TsRepairStatus='REPAIR AND RETURN' and jo.StateStatus='Repair-Done' and jo.JobOrderType='Warrenty') or (jo.TsRepairStatus='REPAIR AND RETURN' and jo.StateStatus='Repair-Done' and jo.JobOrderType='Billing'and jo.InvoiceCode !=null) or (jo.TsRepairStatus='RETURN WITHOUT REPAIR' and jo.StateStatus='Job-Initiated') or jo.StateStatus='HandSetChange')) tbl Order By JobOrderCode desc
", Utility.ParamChecker(param));
            return query;
        }

        public ExecutionStateWithText SaveJobOrderMDelivey(long[] jobOrders, long userId, long orgId, long branchId)
        {
            //bool IsSuccess = false;
            ExecutionStateWithText executionState = new ExecutionStateWithText();
            List<JobOrder> jobOrderDelivery = new List<JobOrder>();
            string deliveryCode = ("DC-" + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss"));
            foreach (var job in jobOrders)
            {
                var jobOrderInDb = GetJobOrdersByIdWithBranch(job, branchId, orgId);
                jobOrderInDb.MultipleDeliveryCode = deliveryCode;
                jobOrderInDb.StateStatus = JobOrderStatus.DeliveryDone;
                jobOrderInDb.CloseDate = DateTime.Now;
                jobOrderInDb.UpUserId = userId;
                jobOrderInDb.UpdateDate = DateTime.Now;
                _jobOrderRepository.Update(jobOrderInDb);
            }
            _jobOrderRepository.UpdateAll(jobOrderDelivery);
            if (_jobOrderRepository.Save() == true)
            {
                executionState.isSuccess = true;
                executionState.text = deliveryCode;
            }
            return executionState;
        }
        public IEnumerable<JobOrderDTO> GetMultipleJobDeliveryChalan(string deliveryCode,long branchId, long orgId)
        {
            var data = this._frontDeskUnitOfWork.Db.Database.SqlQuery<JobOrderDTO>(
                string.Format(@"Select JodOrderId,TsRepairStatus,JobOrderCode,CustomerName,MobileNo,[Address],ModelName,IsWarrantyAvailable,IsWarrantyPaperEnclosed,StateStatus,JobOrderType,EntryDate,EntryUser,
CloseDate,TSRemarks,
SUBSTRING(FaultName,1,LEN(FaultName)-1) 'FaultName',SUBSTRING(ServiceName,1,LEN(ServiceName)-1) 'ServiceName',
SUBSTRING(AccessoriesNames,1,LEN(AccessoriesNames)-1) 'AccessoriesNames',SUBSTRING(PartsName,1,LEN(PartsName)-1) 'PartsName',
SUBSTRING(Problems,1,LEN(Problems)-1) 'Problems',TSId,TSName,RepairDate,
IMEI,[Type],ModelColor,WarrantyDate,Remarks,ReferenceNumber,IMEI2,CloseUser,InvoiceCode,InvoiceInfoId,CustomerType,CourierNumber,CourierName,ApproxBill,IsTransfer,TransferCode,DestinationBranch,MultipleDeliveryCode
From (Select jo.JodOrderId,jo.CustomerName,jo.MobileNo,jo.[Address],de.DescriptionName 'ModelName',jo.IsWarrantyAvailable,jo.InvoiceInfoId,jo.IsWarrantyPaperEnclosed,jo.JobOrderType,jo.StateStatus,jo.EntryDate,ap.UserName'EntryUser',jo.CloseDate,jo.InvoiceCode,jo.CustomerType,jo.CourierNumber,jo.CourierName,jo.ApproxBill,jo.IsTransfer,jo.MultipleDeliveryCode,

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

(Select Top 1 BranchName 'DestinationBranch' from tblJobOrderTransferDetail tdt
Inner Join tblJobOrders jodr on tdt.JobOrderId = jodr.JodOrderId
inner join [ControlPanel].dbo.tblBranch bb on tdt.ToBranch=bb.BranchId
Where jodr.JodOrderId = jo.JodOrderId 
Order By JobOrderId desc) 'DestinationBranch',

(Select Top 1 TransferCode 'TransferCode' from tblJobOrderTransferDetail tjt
Inner Join tblJobOrders joo on tjt.JobOrderId = joo.JodOrderId
Where tjt.JobOrderId = jo.JodOrderId)'TransferCode' ,

(Select top 1 SignOutDate from tblJobOrderTS jt
Inner Join tblJobOrders j on jt.JodOrderId = j.JodOrderId
Where jt.JodOrderId = jo.JodOrderId and j.TsRepairStatus='REPAIR AND RETURN' Order by jt.JTSId desc) 'RepairDate'

from tblJobOrders jo
Inner Join [Inventory].dbo.tblDescriptions de on jo.DescriptionId = de.DescriptionId
Inner Join [ControlPanel].dbo.tblApplicationUsers ap on jo.EUserId = ap.UserId

where MultipleDeliveryCode='{0}' and jo.BranchId={1} and jo.OrganizationId={2}) tbl Order By EntryDate asc", deliveryCode,branchId, orgId)).ToList();
            return data;
        }

        public bool UpdateQCTransferStatus(long jobOrderId, long orgId, long branchId, long userId)
        {
            var jobOrderInDb = GetJobOrdersByIdWithBranch(jobOrderId, branchId, orgId);
           
            if (jobOrderInDb != null)
            {
                jobOrderInDb.QCTransferStatus = "Received";
                jobOrderInDb.UpUserId = userId;
                jobOrderInDb.UpdateDate = DateTime.Now;
                _jobOrderRepository.Update(jobOrderInDb);
            }
            return _jobOrderRepository.Save();
        }
    }
}
