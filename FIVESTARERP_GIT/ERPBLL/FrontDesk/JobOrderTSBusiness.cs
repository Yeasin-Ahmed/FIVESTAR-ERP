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
  public class JobOrderTSBusiness: IJobOrderTSBusiness
    {
        private readonly IFrontDeskUnitOfWork _frontDeskUnitOfWork;
        private readonly JobOrderTSRepository _jobOrderTSRepository;
        private readonly IJobOrderRepairBusiness _jobOrderRepairBusiness;
        private readonly IRepairBusiness _repairBusiness;

        public JobOrderTSBusiness(IFrontDeskUnitOfWork frontDeskUnitOfWork, IJobOrderRepairBusiness jobOrderRepairBusiness, IRepairBusiness repairBusiness)
        {
            this._frontDeskUnitOfWork = frontDeskUnitOfWork;
            this._jobOrderTSRepository = new JobOrderTSRepository(this._frontDeskUnitOfWork);
            this._jobOrderRepairBusiness = jobOrderRepairBusiness;
            this._repairBusiness = repairBusiness;
        }

        public IEnumerable<DashboardDailySingInAndOutDTO> DashboardDailySingInAndOuts(long orgId, long branchId)
        {
            return this._frontDeskUnitOfWork.Db.Database.SqlQuery<DashboardDailySingInAndOutDTO>(
                string.Format(@"Select (select COUNT(*) From tblJobOrderTS Where Cast(AssignDate as date)=Cast(GETDATE()  as date)) 'TotalSignInToday' ,(select COUNT(*) From tblJobOrderTS Where Cast(SignOutDate as date)=Cast(GETDATE()  as date) and OrganizationId={0} and BranchId={1}) 'TotalSignOutToday'", orgId, branchId)).ToList();
        }

        public JobOrderTS GetAllTJobOrderTs(long joborderId, long orgId, long branchId)
        {
            return _jobOrderTSRepository.GetOneByOrg(ts => ts.JodOrderId == joborderId && ts.OrganizationId == orgId && ts.BranchId == branchId);
        }

        public bool UpdateJobOrderTsStatus(long joborderId, long userId, long orgId, long branchId)
        {
            var jobOrderTsStatus = GetAllTJobOrderTs(joborderId, orgId,branchId);
            if (jobOrderTsStatus != null)
            {
                jobOrderTsStatus.JodOrderId = joborderId;
                jobOrderTsStatus.StateStatus = "Sing-Out";
                jobOrderTsStatus.IsActive = false;
                jobOrderTsStatus.UpUserId = userId;
                jobOrderTsStatus.SignOutDate = DateTime.Now;
                _jobOrderTSRepository.Update(jobOrderTsStatus);
            }
            return _jobOrderTSRepository.Save();
        }
    }
}
