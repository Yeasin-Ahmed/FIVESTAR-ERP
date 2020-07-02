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
   public class JobOrderAccessoriesBusiness: IJobOrderAccessoriesBusiness
    {
        private readonly IFrontDeskUnitOfWork _frontDeskUnitOfWork;
        private readonly JobOrderAccessoriesRepository _jobOrderAccessoriesRepository;

        public JobOrderAccessoriesBusiness(IFrontDeskUnitOfWork frontDeskUnitOfWork)
        {
            this._frontDeskUnitOfWork = frontDeskUnitOfWork;
            this._jobOrderAccessoriesRepository = new JobOrderAccessoriesRepository(this._frontDeskUnitOfWork);
        }

        public JobOrderAccessories GetJobOrderAccessoriesById(long id, long orgId)
        {
            return _jobOrderAccessoriesRepository.GetOneByOrg(a => a.JobOrderAccessoriesId == id && a.OrganizationId == orgId);
        }

        public IEnumerable<JobOrderAccessories> GetJobOrderAccessoriesByJobOrder(long jobOrderId, long orgId)
        {
            return _jobOrderAccessoriesRepository.GetAll(a => a.JobOrderId == jobOrderId && a.OrganizationId == orgId);
        }
    }
}
