using ERPBLL.FrontDesk.Interface;
using ERPBO.FrontDesk.DomainModels;
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
    }
}
