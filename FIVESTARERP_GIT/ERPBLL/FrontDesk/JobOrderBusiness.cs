using ERPBLL.Common;
using ERPBLL.FrontDesk.Interface;
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

        public JobOrderBusiness(IFrontDeskUnitOfWork frontDeskUnitOfWork, IJobOrderAccessoriesBusiness jobOrderAccessoriesBusiness, IJobOrderProblemBusiness jobOrderProblemBusiness)
        {
            this._frontDeskUnitOfWork = frontDeskUnitOfWork;
            this._jobOrderRepository = new JobOrderRepository(this._frontDeskUnitOfWork);
            this._jobOrderAccessoriesBusiness = jobOrderAccessoriesBusiness;
            this._jobOrderAccessoriesRepository = new JobOrderAccessoriesRepository(this._frontDeskUnitOfWork);
            this._jobOrderProblemBusiness = jobOrderProblemBusiness;
            this._jobOrderProblemRepository = new JobOrderProblemRepository(this._frontDeskUnitOfWork);
        }

        public JobOrder GetJobOrderById(long jobOrderId, long orgId)
        {
            return _jobOrderRepository.GetOneByOrg(j => j.JodOrderId == jobOrderId && j.OrganizationId == orgId);
        }

        public IEnumerable<JobOrderDTO> GetJobOrders(string mobileNo, long? modelId, string status, long? jobOrderId, string jobCode, string iMEI, string iMEI2, long orgId, long branchId)
        {
            return _frontDeskUnitOfWork.Db.Database.SqlQuery<JobOrderDTO>(QueryForJobOrder(mobileNo, modelId, status, jobOrderId, jobCode, iMEI, iMEI2, orgId, branchId)).ToList();
        }

        private string QueryForJobOrder(string mobileNo, long? modelId, string status, long? jobOrderId, string jobCode, string iMEI, string iMEI2, long orgId, long branchId)
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

            query = string.Format(@"Select JodOrderId,JobOrderCode,CustomerName,MobileNo,[Address],ModelName,IsWarrantyAvailable,IsWarrantyPaperEnclosed,StateStatus,JobOrderType,EntryDate,EntryUser,
SUBSTRING(AccessoriesNames,1,LEN(AccessoriesNames)-1) 'AccessoriesNames',
SUBSTRING(Problems,1,LEN(Problems)-1) 'Problems',TSId,TSName,IMEI,[Type],ModelColor,WarrantyDate,Remarks,ReferenceNumber,IMEI2
From (Select jo.JodOrderId,jo.CustomerName,jo.MobileNo,jo.[Address],de.DescriptionName 'ModelName',jo.IsWarrantyAvailable,jo.IsWarrantyPaperEnclosed,jo.JobOrderType,jo.StateStatus,jo.EntryDate,ap.UserName 'EntryUser',

Cast((Select AccessoriesName+',' From [Configuration].dbo.tblAccessories ass
Inner Join tblJobOrderAccessories joa on ass.AccessoriesId = joa.AccessoriesId
Where joa.JobOrderId = jo.JodOrderId
Order BY AccessoriesName For XML PATH('')) as nvarchar(MAX))  'AccessoriesNames',

Cast((Select ProblemName+',' From [Configuration].dbo.tblClientProblems prob
Inner Join tblJobOrderProblems jop on prob.ProblemId = jop.ProblemId
Where jop.JobOrderId = jo.JodOrderId
Order BY ProblemName For XML PATH(''))as nvarchar(MAX)) 'Problems',jo.JobOrderCode,jo.TSId,ts.Name 'TSName',jo.IMEI,jo.[Type],jo.ModelColor,jo.WarrantyDate,jo.Remarks,jo.ReferenceNumber,jo.IMEI2

from tblJobOrders jo
Inner Join [Inventory].dbo.tblDescriptions de on jo.DescriptionId = de.DescriptionId
Left Join [Configuration].dbo.tblTechnicalServiceEngs ts on jo.TSId =ts.EngId
Inner Join [ControlPanel].dbo.tblApplicationUsers ap on jo.EUserId = ap.UserId Where 1 = 1 {0}) tbl Order By EntryDate desc", Utility.ParamChecker(param));
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
                    StateStatus = JobOrderStatus.CustomerApproved,
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

        public bool UpdateJobOrderStatus(long jobOrderId, string status, string type, long userId, long orgId)
        {
            var jobOrder = GetJobOrderById(jobOrderId, orgId);
            if (jobOrder != null && jobOrder.StateStatus == JobOrderStatus.PendingJobOrder)
            {
                jobOrder.StateStatus = status.Trim();
                jobOrder.JobOrderType = type.Trim();
                jobOrder.UpUserId = userId;
                jobOrder.UpdateDate = DateTime.Now;
                _jobOrderRepository.Update(jobOrder);
            }
            return _jobOrderRepository.Save();
        }

        public bool AssignTSForJobOrder(long jobOrderId, long tsId, long userId, long orgId)
        {
            var jobOrder = GetJobOrderById(jobOrderId, orgId);
            if (jobOrder != null && jobOrder.StateStatus == JobOrderStatus.CustomerApproved)
            {
                jobOrder.TSId = tsId;
                jobOrder.StateStatus = JobOrderStatus.AssignToTS;
                jobOrder.UpUserId = userId;
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
        private string QueryForJobOrderTS(string roleName, string mobileNo, long? modelId, long? jobOrderId, string jobCode, long userId, long orgId)
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
            if (roleName== "Technical Services")
            {
                param += string.Format(@" and jo.TSId ={0}", userId);
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
Order BY ProblemName For XML PATH(''))as nvarchar(MAX)) 'Problems',jo.JobOrderCode,jo.TSId,ap.UserName 'TSName'

from tblJobOrders jo
Inner Join [Inventory].dbo.tblDescriptions de on jo.DescriptionId = de.DescriptionId
Inner Join [ControlPanel].dbo.tblApplicationUsers ap on jo.TSId = ap.UserId 
Where 1 = 1  and jo.StateStatus='TS-Assigned' {0}) tbl Order By EntryDate desc", Utility.ParamChecker(param));
            return query;
        }

        public IEnumerable<JobOrderDTO> GetJobOrdersTS(string roleName, string mobileNo, long? modelId, long? jobOrderId, string jobCode, long userId, long orgId)
        {
            return _frontDeskUnitOfWork.Db.Database.SqlQuery<JobOrderDTO>(QueryForJobOrderTS(roleName,mobileNo, modelId, jobOrderId, jobCode, userId, orgId)).ToList();
        }

        public IEnumerable<JobOrderDTO> GetJobOrdersPush(long? jobOrderId, long orgId)
        {
            return _frontDeskUnitOfWork.Db.Database.SqlQuery<JobOrderDTO>(QueryForJobOrderPush(jobOrderId, orgId)).ToList();
        }

        private string QueryForJobOrderPush(long? jobOrderId, long orgId)
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
Inner Join [ControlPanel].dbo.tblApplicationUsers ap on jo.EUserId = ap.UserId Where 1 = 1 and jo.StateStatus='Customer-Approved' {0}) tbl Order By EntryDate desc", Utility.ParamChecker(param));
            return query;
        }

        public bool SaveJobOrderPushing(long ts, long[] jobOrders, long userId, long orgId, long branchId)
        {
            //bool IsSuccess = false;
            foreach (var job in jobOrders)
            {
                var jobOrderInDb = GetJobOrdersByIdWithBranch(job, branchId, orgId);
                if (jobOrderInDb != null && jobOrderInDb.StateStatus == JobOrderStatus.CustomerApproved)
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
            if (jobOrderInDb != null && jobOrderInDb.StateStatus == JobOrderStatus.CustomerApproved)
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
            if (jobOrder != null)
            {
                jobOrder.JodOrderId = jobOrderId;
                jobOrder.TSRemarks = remarks.Trim();
                jobOrder.UpUserId = userId;
                jobOrder.OrganizationId = orgId;
                jobOrder.BranchId = branchId;
                jobOrder.UpdateDate = DateTime.Now;
                _jobOrderRepository.Update(jobOrder);
            }
            return _jobOrderRepository.Save();
        }

        public IEnumerable<DashboardApprovedRequsitionDTO> DashboardApprovedRequsition(long orgId, long branchId)
        {
            return this._frontDeskUnitOfWork.Db.Database.SqlQuery<DashboardApprovedRequsitionDTO>(
                string.Format(@"select JodOrderId,jo.JobOrderCode,RequsitionCode,rq.StateStatus,rq.EntryDate from tblJobOrders jo
                Inner join tblRequsitionInfoForJobOrders rq on jo.JodOrderId=rq.JobOrderId
                Where  rq.StateStatus='Approved' and  jo.OrganizationId={0} and jo.BranchId={1}", orgId, branchId)).ToList();
        }

        public IEnumerable<DashboardApprovedRequsitionDTO> DashboardPendingRequsition(long orgId, long branchId)
        {
            return this._frontDeskUnitOfWork.Db.Database.SqlQuery<DashboardApprovedRequsitionDTO>(
                string.Format(@"select JodOrderId,jo.JobOrderCode,RequsitionCode,rq.StateStatus,rq.EntryDate from tblJobOrders jo
                Inner join tblRequsitionInfoForJobOrders rq on jo.JodOrderId=rq.JobOrderId
                Where  rq.StateStatus='Pending' and  jo.OrganizationId={0} and jo.BranchId={1}", orgId, branchId)).ToList();
        }
    }
}
