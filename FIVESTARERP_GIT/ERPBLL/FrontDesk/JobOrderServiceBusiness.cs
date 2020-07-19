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
   public class JobOrderServiceBusiness: IJobOrderServiceBusiness
    {
        private readonly IFrontDeskUnitOfWork _frontDeskUnitOfWork;
        private readonly JobOrderServiceRepository jobOrderServiceRepository;

        public JobOrderServiceBusiness(IFrontDeskUnitOfWork frontDeskUnitOfWork)
        {
            this._frontDeskUnitOfWork = frontDeskUnitOfWork;
            this.jobOrderServiceRepository = new JobOrderServiceRepository(this._frontDeskUnitOfWork);
        }

        public JobOrderService GetJobOrderServiceByJobId(long joborderId, long orgId)
        {
            return jobOrderServiceRepository.GetOneByOrg(a => a.JobOrderId == joborderId && a.OrganizationId == orgId);
        }

        public IEnumerable<JobOrderService> GetJobOrderServiceByJobOrderId(long joborderId, long orgId)
        {
            return jobOrderServiceRepository.GetAll(a => a.JobOrderId == joborderId && a.OrganizationId == orgId).ToList();
        }

        public bool IsDuplicateServicesName(long jobOrderId, long serviceId, long orgId)
        {
            return jobOrderServiceRepository.GetOneByOrg(ser => ser.JobOrderId == jobOrderId && ser.ServiceId == serviceId && ser.OrganizationId == orgId) != null ? true : false;
        }

        public bool SaveJobOrderServicve(List<JobOrderServiceDTO> jobOrderServices, long userId, long orgId)
        {
            List<JobOrderService> listjobOrderService = new List<JobOrderService>();
            foreach (var item in jobOrderServices)
            {
                JobOrderService jobOrderService = new JobOrderService
                {
                    ServiceId = item.ServiceId,
                    EntryDate = DateTime.Now,
                    EUserId = userId,
                    OrganizationId = orgId,
                    JobOrderId = item.JobOrderId
                };
                listjobOrderService.Add(jobOrderService);
            }
            jobOrderServiceRepository.InsertAll(listjobOrderService);
            return jobOrderServiceRepository.Save();
        }
    }
}
