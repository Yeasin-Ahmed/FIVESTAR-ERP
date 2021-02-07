using ERPBLL.Configuration.Interface;
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
        private readonly IJobOrderBusiness _jobOrderBusiness;
        private readonly JobOrderRepository _jobOrderRepository;
        private readonly IRepairBusiness _repairBusiness;

        public JobOrderRepairBusiness(IFrontDeskUnitOfWork frontDeskUnitOfWork, IJobOrderBusiness jobOrderBusiness, IRepairBusiness repairBusiness)
        {
            this._frontDeskUnitOfWork = frontDeskUnitOfWork;
            this.jobOrderRepairRepository = new JobOrderRepairRepository(this._frontDeskUnitOfWork);
            this._jobOrderRepository = new JobOrderRepository(this._frontDeskUnitOfWork);
            this._jobOrderBusiness = jobOrderBusiness;
            this._repairBusiness = repairBusiness;
        }

        public JobOrderRepair GetAllJobOrderRepair(long repairId, long orgId)
        {
            return jobOrderRepairRepository.GetOneByOrg(a => a.JobOrderRepairId == repairId && a.OrganizationId == orgId);
        }

        public JobOrderRepair GetJobOrderRepairByJobId(long joborderId, long orgId)
        {
            return jobOrderRepairRepository.GetOneByOrg(a => a.JobOrderId == joborderId && a.OrganizationId == orgId);
        }

        public IEnumerable<JobOrderRepair> GetJobOrderRepairByJobOrderId(long joborderId, long orgId)
        {
            return jobOrderRepairRepository.GetAll(a => a.JobOrderId == joborderId && a.OrganizationId == orgId).ToList();
        }

        public bool SaveJobOrderRepair(List<JobOrderRepairDTO> jobOrderRepairs, long jobOrderId, long userId, long orgId)
        {
            var repairCodeNew = jobOrderRepairs.FirstOrDefault();
            var jobOrderR = GetJobOrderRepairByJobOrderId(repairCodeNew.JobOrderId, orgId).ToList();
            jobOrderRepairRepository.DeleteAll(jobOrderR);//Delete


            // JobOrder.RepairStatus ="Status" // Repair table text

            JobOrderRepair jobOrderRepair = new JobOrderRepair
            {
                RepairId = repairCodeNew.RepairId,
                EntryDate = DateTime.Now,
                EUserId = userId,
                OrganizationId = orgId,
                JobOrderId = repairCodeNew.JobOrderId
            }; // New 

            jobOrderRepairRepository.Insert(jobOrderRepair);

            var jobOrder = _jobOrderBusiness.GetJobOrderById(jobOrderId, orgId);
            var jobOrderRepairStatus = _repairBusiness.GetRepairOneByOrgId(repairCodeNew.RepairId, orgId).RepairName;
            if (jobOrder != null)
            {
                jobOrder.TsRepairStatus = jobOrderRepairStatus; // Repair Code text //
                if (jobOrderRepairStatus == "QC")
                {
                    jobOrder.QCTransferStatus = "Pending";
                }
                else
                {
                    jobOrder.QCTransferStatus = null;
                }
                jobOrder.UpUserId = userId;
                jobOrder.UpdateDate = DateTime.Now;
                _jobOrderRepository.Update(jobOrder);
            }
            
           return jobOrderRepairRepository.Save();
        }

    }
}
