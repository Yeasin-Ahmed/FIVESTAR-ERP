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
   public class JobOrderFaultBusiness: IJobOrderFaultBusiness
    {
        private readonly IFrontDeskUnitOfWork _frontDeskUnitOfWork;
        private readonly JobOrderFaultRepository jobOrderFaultRepository;

        public JobOrderFaultBusiness(IFrontDeskUnitOfWork frontDeskUnitOfWork)
        {
            this._frontDeskUnitOfWork = frontDeskUnitOfWork;
            this.jobOrderFaultRepository = new JobOrderFaultRepository(this._frontDeskUnitOfWork);
        }

        public JobOrderFault GetJobOrderFaultByJobId(long joborderId, long orgId)
        {
            return jobOrderFaultRepository.GetOneByOrg(a => a.JobOrderId == joborderId && a.OrganizationId == orgId);
        }

        public IEnumerable<JobOrderFault> GetJobOrderFaultByJobOrderId(long joborderId, long orgId)
        {
            return jobOrderFaultRepository.GetAll(a => a.JobOrderId == joborderId && a.OrganizationId == orgId).ToList();
        }

        public bool IsDuplicateFaultName(long jobOrderId, long faultId, long orgId)
        {
            return jobOrderFaultRepository.GetOneByOrg(fault => fault.JobOrderId == jobOrderId && fault.FaultId == faultId && fault.OrganizationId == orgId) != null ? true : false;
        }

        public bool SaveJobOrderFault(List<JobOrderFaultDTO> jobOrderFaults, long userId, long orgId)
        {
            List<JobOrderFault> listjobOrderFault = new List<JobOrderFault>();
            foreach (var item in jobOrderFaults)
            {
                JobOrderFault jobOrderFault = new JobOrderFault
                {
                    FaultId = item.FaultId,
                    EntryDate = DateTime.Now,
                    EUserId = userId,
                    OrganizationId = orgId,
                    JobOrderId=item.JobOrderId
                };
                listjobOrderFault.Add(jobOrderFault);
            }
            jobOrderFaultRepository.InsertAll(listjobOrderFault);
            return jobOrderFaultRepository.Save();
        }
    }
}
