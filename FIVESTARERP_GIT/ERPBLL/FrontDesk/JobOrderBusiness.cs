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
        private readonly IFrontDeskUnitOfWork _frontDeskUnitOfWork;

        public JobOrderBusiness(IFrontDeskUnitOfWork frontDeskUnitOfWork)
        {
            this._frontDeskUnitOfWork = frontDeskUnitOfWork;
            this._jobOrderRepository = new JobOrderRepository(this._frontDeskUnitOfWork);
        }

        public JobOrder GetJobOrderById(long jobOrderId, long orgId)
        {
            return _jobOrderRepository.GetOneByOrg(j => j.JodOrderId == jobOrderId && j.OrganizationId == orgId);
        }

        public IEnumerable<JobOrderDTO> GetJobOrders(string mobileNo, long? modelId, string status, long? jobOrderId, string jobCode, long orgId)
        {
            return _frontDeskUnitOfWork.Db.Database.SqlQuery<JobOrderDTO>(QueryForJobOrder(mobileNo, modelId, status, jobOrderId, jobCode, orgId)).ToList();
        }

        private string QueryForJobOrder(string mobileNo, long? modelId, string status, long? jobOrderId, string jobCode, long orgId)
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
            }
            if (orgId > 0)
            {
                param += string.Format(@"and jo.OrganizationId={0}", orgId);
            }

            query = string.Format(@"Select JodOrderId,JobOrderCode,CustomerName,MobileNo,[Address],ModelName,IsWarrantyAvailable,IsWarrantyPaperEnclosed,StateStatus,JobOrderType,EntryDate,EntryUser,
SUBSTRING(AccessoriesNames,1,LEN(AccessoriesNames)-1) 'AccessoriesNames',
SUBSTRING(Problems,1,LEN(Problems)-1) 'Problems',TSId,TSName,IMEI,[Type],ModelColor,WarrantyDate,Remarks,ReferenceNumber
From (Select jo.JodOrderId,jo.CustomerName,jo.MobileNo,jo.[Address],de.DescriptionName 'ModelName',jo.IsWarrantyAvailable,jo.IsWarrantyPaperEnclosed,jo.JobOrderType,jo.StateStatus,jo.EntryDate,ap.UserName 'EntryUser',

Cast((Select AccessoriesName+',' From [Configuration].dbo.tblAccessories ass
Inner Join tblJobOrderAccessories joa on ass.AccessoriesId = joa.AccessoriesId
Where joa.JobOrderId = jo.JodOrderId
Order BY AccessoriesName For XML PATH('')) as nvarchar(MAX))  'AccessoriesNames',

Cast((Select ProblemName+',' From [Configuration].dbo.tblClientProblems prob
Inner Join tblJobOrderProblems jop on prob.ProblemId = jop.ProblemId
Where jop.JobOrderId = jo.JodOrderId
Order BY ProblemName For XML PATH(''))as nvarchar(MAX)) 'Problems',jo.JobOrderCode,jo.TSId,ts.Name 'TSName',jo.IMEI,jo.[Type],jo.ModelColor,jo.WarrantyDate,jo.Remarks,jo.ReferenceNumber

from tblJobOrders jo
Inner Join [Inventory].dbo.tblDescriptions de on jo.DescriptionId = de.DescriptionId
Left Join [Configuration].dbo.tblTechnicalServiceEngs ts on jo.TSId =ts.EngId
Inner Join [ControlPanel].dbo.tblApplicationUsers ap on jo.EUserId = ap.UserId Where 1 = 1 {0}) tbl Order By EntryDate desc", Utility.ParamChecker(param));
            return query;
        }

        public bool SaveJobOrder(JobOrderDTO jobOrderDto, List<JobOrderAccessoriesDTO> jobOrderAccessoriesDto, List<JobOrderProblemDTO> jobOrderProblemsDto, long userId, long orgId, long branchId)
        {
            JobOrder jobOrder = new JobOrder
            {
                CustomerId = jobOrderDto.CustomerId,
                CustomerName = jobOrderDto.CustomerName,
                MobileNo = jobOrderDto.MobileNo,
                Address = jobOrderDto.Address,
                IMEI = jobOrderDto.IMEI,
                Type= PhoneTypes.Smartphone,
                ModelColor= ModelColors.Red,
                WarrantyDate= jobOrderDto.WarrantyDate,
                Remarks= jobOrderDto.Remarks,
                ReferenceNumber= jobOrderDto.ReferenceNumber,
                DescriptionId = jobOrderDto.DescriptionId,
                IsWarrantyAvailable = jobOrderDto.IsWarrantyAvailable,
                IsWarrantyPaperEnclosed = jobOrderDto.IsWarrantyPaperEnclosed,
                EntryDate = DateTime.Now,
                EUserId = userId,
                OrganizationId = orgId,
                StateStatus = JobOrderStatus.PendingJobOrder,
                JobOrderCode = ("JOB-" + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss")),
                BranchId = branchId
            };
            if (jobOrder.IsWarrantyAvailable)
            {
                jobOrder.WarrantyDate = jobOrderDto.WarrantyDate.Value.Date;
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
        private string QueryForJobOrderTS(string mobileNo, long? modelId, long? jobOrderId, string jobCode, long orgId)
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
                if (!string.IsNullOrEmpty(jobCode))
                {
                    param += string.Format(@"and jo.JobOrderCode Like '%{0}%'", jobCode);
                }
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
Inner Join [Configuration].dbo.tblTechnicalServiceEngs ts on jo.TSId =ts.EngId
Inner Join [ControlPanel].dbo.tblApplicationUsers ap on jo.EUserId = ap.UserId Where 1 = 1 and jo.StateStatus='TS-Assigned' {0}) tbl Order By EntryDate desc", Utility.ParamChecker(param));
            return query;
        }

        public IEnumerable<JobOrderDTO> GetJobOrdersTS(string mobileNo, long? modelId, long? jobOrderId, string jobCode, long orgId)
        {
            return _frontDeskUnitOfWork.Db.Database.SqlQuery<JobOrderDTO>(QueryForJobOrderTS(mobileNo, modelId, jobOrderId, jobCode, orgId)).ToList();
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

        public JobOrder GetReferencesNumberByIMEI( string imei, long orgId, long branchId)
        {
            imei = imei.Trim();
            return _jobOrderRepository.GetAll(job => job.IMEI == imei.ToString() && job.OrganizationId == orgId && job.BranchId == branchId).OrderByDescending(o=>o.JodOrderId).FirstOrDefault();
        }
    }
}
