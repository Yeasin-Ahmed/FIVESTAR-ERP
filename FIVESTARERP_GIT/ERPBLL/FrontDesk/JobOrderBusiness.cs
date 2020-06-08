using ERPBLL.Common;
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

        private string QueryForJobOrder(string mobileNo, long? modelId, string status, long? jobOrderId,string jobCode, long orgId)
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
SUBSTRING(Problems,1,LEN(Problems)-1) 'Problems'
From (Select jo.JodOrderId,jo.CustomerName,jo.MobileNo,jo.[Address],de.DescriptionName 'ModelName',jo.IsWarrantyAvailable,jo.IsWarrantyPaperEnclosed,jo.JobOrderType,jo.StateStatus,jo.EntryDate,ap.UserName 'EntryUser',

Cast((Select AccessoriesName+',' From [Configuration].dbo.tblAccessories ass
Inner Join tblJobOrderAccessories joa on ass.AccessoriesId = joa.AccessoriesId
Where joa.JobOrderId = jo.JodOrderId
Order BY AccessoriesName For XML PATH('')) as nvarchar(MAX))  'AccessoriesNames',

Cast((Select ProblemName+',' From [Configuration].dbo.tblClientProblems prob
Inner Join tblJobOrderProblems jop on prob.ProblemId = jop.ProblemId
Where jop.JobOrderId = jo.JodOrderId
Order BY ProblemName For XML PATH(''))as nvarchar(MAX)) 'Problems',jo.JobOrderCode

from tblJobOrders jo
Inner Join [Inventory].dbo.tblDescriptions de on jo.DescriptionId = de.DescriptionId
Inner Join [ControlPanel].dbo.tblApplicationUsers ap on jo.EUserId = ap.UserId Where 1 = 1 {0}) tbl", Utility.ParamChecker(param));
            return query;
        }

        public bool SaveJobOrder(JobOrderDTO jobOrderDto, List<JobOrderAccessoriesDTO> jobOrderAccessoriesDto, List<JobOrderProblemDTO> jobOrderProblemsDto, long userId, long orgId)
        {
            JobOrder jobOrder = new JobOrder
            {
                CustomerId = jobOrderDto.CustomerId,
                CustomerName = jobOrderDto.CustomerName,
                MobileNo = jobOrderDto.MobileNo,
                Address = jobOrderDto.Address,
                DescriptionId = jobOrderDto.DescriptionId,
                IsWarrantyAvailable = jobOrderDto.IsWarrantyAvailable,
                IsWarrantyPaperEnclosed = jobOrderDto.IsWarrantyPaperEnclosed,
                EntryDate = DateTime.Now,
                EUserId = userId,
                OrganizationId = orgId,
                StateStatus = JobOrderStatus.PendingJobOrder,
                JobOrderCode = ("JOB-" + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss"))
            };
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

            if(jobOrderAccessoriesDto.Count > 0)
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

        public bool UpdateJobOrderStatus(long jobOrderId, string status, string type,long userId ,long orgId)
        {
            var jobOrder = GetJobOrderById(jobOrderId, orgId);
            if(jobOrder != null && jobOrder.StateStatus == JobOrderStatus.PendingJobOrder)
            {
                jobOrder.StateStatus = status.Trim();
                jobOrder.JobOrderType = type.Trim();
                jobOrder.UpUserId = userId;
                jobOrder.UpdateDate = DateTime.Now;
                _jobOrderRepository.Update(jobOrder);
            }
            return _jobOrderRepository.Save();
        }
    }
}
