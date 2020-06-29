using ERPBLL.FrontDesk.Interface;
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

        public JobOrderTSBusiness(IFrontDeskUnitOfWork frontDeskUnitOfWork)
        {
            this._frontDeskUnitOfWork = frontDeskUnitOfWork;
            this._jobOrderTSRepository = new JobOrderTSRepository(this._frontDeskUnitOfWork);
        }

        public IEnumerable<DashboardDailySingInAndOutDTO> DashboardDailySingInAndOuts(long orgId, long branchId)
        {
            return this._frontDeskUnitOfWork.Db.Database.SqlQuery<DashboardDailySingInAndOutDTO>(
                string.Format(@"select StateStatus,COUNT(StateStatus) as Total from tblJobOrderTS
                Where Cast(GETDATE() as date) = Cast(EntryDate as date) and  OrganizationId={0} and BranchId={1}
				group by StateStatus", orgId, branchId)).ToList();
        }
    }
}
