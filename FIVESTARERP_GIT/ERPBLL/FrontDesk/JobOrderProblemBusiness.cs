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
   public class JobOrderProblemBusiness: IJobOrderProblemBusiness
    {
        private readonly IFrontDeskUnitOfWork _frontDeskUnitOfWork;
        private readonly JobOrderProblemRepository _jobOrderProblemRepository;

        public JobOrderProblemBusiness(IFrontDeskUnitOfWork frontDeskUnitOfWork)
        {
            this._frontDeskUnitOfWork = frontDeskUnitOfWork;
            this._jobOrderProblemRepository = new JobOrderProblemRepository(this._frontDeskUnitOfWork);
        }

        public JobOrderProblem GetJobOrderProblemById(long id, long orgId)
        {
            return _jobOrderProblemRepository.GetOneByOrg(a => a.JobOrderProblemId == id && a.OrganizationId == orgId);
        }

        public JobOrderProblem GetJobOrderProblemByJobId(long id, long orgId)
        {
            return _jobOrderProblemRepository.GetOneByOrg(a => a.JobOrderId == id && a.OrganizationId == orgId);
        }

        public IEnumerable<JobOrderProblem> GetJobOrderProblemByJobOrderId(long jobOrderId, long orgId)
        {
            return _jobOrderProblemRepository.GetAll(a => a.JobOrderId == jobOrderId && a.OrganizationId == orgId).ToList();
        }

        public bool IsDuplicateSymptomName(long id, long prblmId, long orgId)
        {
            return _jobOrderProblemRepository.GetOneByOrg(sym =>sym.JobOrderId == id && sym.ProblemId == prblmId && sym.OrganizationId == orgId) != null ? true : false;
        }

        public bool SaveJobOrderProblem(List<JobOrderProblemDTO> jobOrderProblems, long userId, long orgId)
        {
            List<JobOrderProblem> listjobOrderProblem = new List<JobOrderProblem>();
            foreach (var item in jobOrderProblems)
            {
                JobOrderProblem jobOrderProblem = new JobOrderProblem
                {
                    ProblemId = item.ProblemId,
                    EntryDate = DateTime.Now,
                    EUserId = userId,
                    OrganizationId = orgId,
                    JobOrderId = item.JobOrderId
                };
                listjobOrderProblem.Add(jobOrderProblem);
            }
            _jobOrderProblemRepository.InsertAll(listjobOrderProblem);
            return _jobOrderProblemRepository.Save();
        }
    }
}
