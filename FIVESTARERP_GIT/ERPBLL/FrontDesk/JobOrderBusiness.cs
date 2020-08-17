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

        public IEnumerable<JobOrderDTO> GetJobOrders(string mobileNo, long? modelId, string status, long? jobOrderId, string jobCode, string iMEI, string iMEI2, long orgId, long branchId, string fromDate, string toDate)
        {
            return _frontDeskUnitOfWork.Db.Database.SqlQuery<JobOrderDTO>(QueryForJobOrder(mobileNo, modelId, status, jobOrderId, jobCode, iMEI, iMEI2, orgId, branchId, fromDate, toDate)).ToList();
        }

        private string QueryForJobOrder(string mobileNo, long? modelId, string status, long? jobOrderId, string jobCode, string iMEI, string iMEI2, long orgId, long branchId, string fromDate, string toDate)
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
                param += string.Format(@"and jo.BranchId={0}", branchId);
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
IMEI,[Type],ModelColor,WarrantyDate,Remarks,ReferenceNumber,IMEI2,CloseUser,InvoiceCode,InvoiceInfoId,CustomerType,CourierNumber,CourierName,ApproxBill
From (Select jo.JodOrderId,jo.CustomerName,jo.MobileNo,jo.[Address],de.DescriptionName 'ModelName',jo.IsWarrantyAvailable,jo.InvoiceInfoId,jo.IsWarrantyPaperEnclosed,jo.JobOrderType,jo.StateStatus,jo.EntryDate,ap.UserName 'EntryUser',jo.CloseDate,jo.InvoiceCode,jo.CustomerType,jo.CourierNumber,jo.CourierName,jo.ApproxBill,

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

Where 1 = 1{0}) tbl Order By EntryDate desc

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
                    CourierName=jobOrderDto.CourierName,
                    CourierNumber= jobOrderDto.CourierNumber,
                    ApproxBill= jobOrderDto.ApproxBill,
                    ReferenceNumber = jobOrderDto.ReferenceNumber,
                    DescriptionId = jobOrderDto.DescriptionId,
                    IsWarrantyAvailable = jobOrderDto.IsWarrantyAvailable,
                    IsWarrantyPaperEnclosed = jobOrderDto.IsWarrantyPaperEnclosed,
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
            return this._frontDeskUnitOfWork.Db.Database.SqlQuery<string>(string.Format(@"Select Cast( ISNULL(MAX(Cast(SUBSTRING(JobOrderCode,5,LEN(JobOrderCode)) as bigint)),0)+1 as Nvarchar(20)) 'Value'  from [FrontDesk].dbo.tblJobOrders 
Where  JobOrderCode not like 'JOB%' and 
OrganizationId= {0} and BranchId={1}
",orgId,branchId)).FirstOrDefault();
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
                param += string.Format(@" and jo.BranchId={0}", branchId);
            }
            if (roleName== "Technical Services")
            {
                param += string.Format(@" and jo.TSId ={0}", userId);
            }
            query = string.Format(@"Select JodOrderId,JobOrderCode,CustomerName,MobileNo,[Address],ModelName,IsWarrantyAvailable,IsWarrantyPaperEnclosed,StateStatus,JobOrderType,EntryDate,EntryUser,
SUBSTRING(AccessoriesNames,1,LEN(AccessoriesNames)-1) 'AccessoriesNames',
SUBSTRING(Problems,1,LEN(Problems)-1) 'Problems',TSId,TSName,BranchId
From (Select jo.JodOrderId,jo.CustomerName,jo.MobileNo,jo.[Address],de.DescriptionName 'ModelName',jo.IsWarrantyAvailable,jo.IsWarrantyPaperEnclosed,jo.JobOrderType,jo.StateStatus,jo.EntryDate,UserName,jo.BranchId,

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
Inner Join [Inventory].dbo.tblDescriptions de on jo.DescriptionId = de.DescriptionId
Inner Join [ControlPanel].dbo.tblApplicationUsers ap on jo.TSId = ap.UserId 
Where 1 = 1  and (jo.StateStatus='TS-Assigned' or jo.StateStatus='Repair-Done' or jo.StateStatus='Delivery-Done'){0}) tbl Order By EntryDate desc", Utility.ParamChecker(param));
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
                param += string.Format(@"and jo.BranchId={0}", branchId);
            }

            query = string.Format(@"Select JodOrderId,JobOrderCode,CustomerName,MobileNo,[Address],ModelName,IsWarrantyAvailable,IsWarrantyPaperEnclosed,StateStatus,JobOrderType,EntryDate,EntryUser,
SUBSTRING(AccessoriesNames,1,LEN(AccessoriesNames)-1) 'AccessoriesNames',
SUBSTRING(Problems,1,LEN(Problems)-1) 'Problems',TSId,TSName
From (Select jo.JodOrderId,jo.CustomerName,jo.MobileNo,jo.[Address],de.DescriptionName 'ModelName',jo.IsWarrantyAvailable,jo.IsWarrantyPaperEnclosed,jo.JobOrderType,jo.StateStatus,jo.EntryDate,ap.UserName 'EntryUser',

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
Inner Join [ControlPanel].dbo.tblApplicationUsers ap on jo.EUserId = ap.UserId Where 1 = 1 and jo.StateStatus='Job-Initiated' {0}) tbl Order By EntryDate desc", Utility.ParamChecker(param));
            return query;
        }

        public bool SaveJobOrderPushing(long ts, long[] jobOrders, long userId, long orgId, long branchId)
        {
            //bool IsSuccess = false;
            foreach (var job in jobOrders)
            {
                var jobOrderInDb = GetJobOrdersByIdWithBranch(job, branchId, orgId);
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
            var jobOrderInDb = GetJobOrdersByIdWithBranch(jobOrderId, branchId, orgId);
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
            return _jobOrderRepository.GetAll(job => job.IMEI == imei.ToString() && job.OrganizationId == orgId && job.BranchId == branchId).OrderByDescending(o => o.JodOrderId).FirstOrDefault();
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
                string.Format(@"select JobOrderType,COUNT(JobOrderType) as Total from tblJobOrders 
                Where Cast(GETDATE() as date) = Cast(EntryDate as date) and  OrganizationId={0} and BranchId={1}
				group by JobOrderType", orgId, branchId)).ToList();
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
                Where  rq.StateStatus='Pending' and  jo.OrganizationId={0} and jo.BranchId={1}", orgId, branchId)).ToList();
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
            var jobStatus = jobOrder.TsRepairStatus == "REPAIR AND RETURN" ? JobOrderStatus.RepairDone : JobOrderStatus.JobInitiated;
            if (jobOrder != null)
            {
                jobOrder.JodOrderId = jobOrderId;
                jobOrder.StateStatus = jobStatus;
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

        public JobOrder GetReferencesNumberByIMEI2(string imei2, long orgId, long branchId)
        {
            imei2 = imei2.Trim();
            return _jobOrderRepository.GetAll(job => job.IMEI2 == imei2.ToString() && job.OrganizationId == orgId && job.BranchId == branchId).OrderByDescending(o => o.JodOrderId).FirstOrDefault();
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
IMEI,[Type],ModelColor,WarrantyDate,Remarks,ReferenceNumber,IMEI2,CloseUser,InvoiceCode,InvoiceInfoId,CustomerType
From (Select jo.JodOrderId,jo.CustomerName,jo.MobileNo,jo.[Address],de.DescriptionName 'ModelName',jo.IsWarrantyAvailable,jo.InvoiceInfoId,jo.IsWarrantyPaperEnclosed,jo.JobOrderType,jo.StateStatus,jo.EntryDate,ap.UserName 'EntryUser',jo.CloseDate,jo.InvoiceCode,jo.CustomerType,

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
Inner Join [Inventory].dbo.tblDescriptions de on jo.DescriptionId = de.DescriptionId
Inner Join [ControlPanel].dbo.tblApplicationUsers ap on jo.EUserId = ap.UserId

Where jo.JodOrderId={0} and  jo.OrganizationId={1} and jo.BranchId={2}) tbl Order By EntryDate desc",jobOrderId, orgId, branchId)).ToList();
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

            query = string.Format(@"Select JodOrderId,JobOrderCode,CustomerName,MobileNo,[Address],ModelName,IsWarrantyAvailable,IsWarrantyPaperEnclosed,StateStatus,JobOrderType,EntryDate,EntryUser,
SUBSTRING(AccessoriesNames,1,LEN(AccessoriesNames)-1) 'AccessoriesNames',
SUBSTRING(Problems,1,LEN(Problems)-1) 'Problems',TSId,TSName
From (Select jo.JodOrderId,jo.CustomerName,jo.MobileNo,jo.[Address],de.DescriptionName 'ModelName',jo.IsWarrantyAvailable,jo.IsWarrantyPaperEnclosed,jo.JobOrderType,jo.StateStatus,jo.EntryDate,ap.UserName 'EntryUser',

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
    }
}
