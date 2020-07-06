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
   public class JobOrderRepairBusiness: IJobOrderRepairBusiness
    {
        private readonly IFrontDeskUnitOfWork _frontDeskUnitOfWork;
        private readonly JobOrderRepairRepository jobOrderRepairRepository;

        public JobOrderRepairBusiness(IFrontDeskUnitOfWork frontDeskUnitOfWork)
        {
            this._frontDeskUnitOfWork = frontDeskUnitOfWork;
            this.jobOrderRepairRepository = new JobOrderRepairRepository(this._frontDeskUnitOfWork);
        }

        public JobOrderRepair GetJobOrderRepairByJobId(long joborderId, long orgId)
        {
            return jobOrderRepairRepository.GetOneByOrg(a => a.JobOrderId == joborderId && a.OrganizationId == orgId);
        }

        public IEnumerable<JobOrderRepair> GetJobOrderRepairByJobOrderId(long joborderId, long orgId)
        {
            return jobOrderRepairRepository.GetAll(a => a.JobOrderId == joborderId && a.OrganizationId == orgId).ToList();
        }

        public bool SaveJobOrderRepair(List<JobOrderRepairDTO> jobOrderRepairs, long userId, long orgId)
        {
            List<JobOrderRepair> listjobOrderRepair = new List<JobOrderRepair>();
            foreach (var item in jobOrderRepairs)
            {
                JobOrderRepair jobOrderRepair = new JobOrderRepair
                {
                    RepairId = item.RepairId,
                    EntryDate = DateTime.Now,
                    EUserId = userId,
                    OrganizationId = orgId,
                    JobOrderId = item.JobOrderId
                };
                listjobOrderRepair.Add(jobOrderRepair);
            }
            jobOrderRepairRepository.InsertAll(listjobOrderRepair);
            return jobOrderRepairRepository.Save();
        }
    }
}
