using ERPBLL.Common;
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
                string.Format(@"Select (select COUNT(*) From tblJobOrderTS Where Cast(AssignDate as date)=Cast(GETDATE()  as date) and OrganizationId={0} and BranchId={1}) 'TotalSignInToday' ,
(select COUNT(*) From tblJobOrderTS Where Cast(SignOutDate as date)=Cast(GETDATE()  as date) and OrganizationId={0} and BranchId={1}) 'TotalSignOutToday'", orgId, branchId)).ToList();
        }

        public JobOrderTS GetJobOrderActiveTsByJobOrderId(long joborderId, long orgId, long branchId)
        {
            return _jobOrderTSRepository.GetOneByOrg(ts => ts.JodOrderId == joborderId && ts.OrganizationId == orgId && ts.BranchId == branchId && ts.IsActive == true);
        }

        public IEnumerable<JobOrderTSDTO> JobSignInAndOut(long? tsId, string jobCode, long orgId, long branchId, string fromDate, string toDate)
        {
            return _frontDeskUnitOfWork.Db.Database.SqlQuery<JobOrderTSDTO>(QueryForJobSignInAndOut(tsId,jobCode,orgId,branchId,fromDate,toDate)).ToList();
        }
        private string QueryForJobSignInAndOut(long? tsId, string jobCode, long orgId, long branchId, string fromDate, string toDate)
        {
            string query = string.Empty;
            string param = string.Empty;
            if (orgId > 0)
            {
                param += string.Format(@"and ts.OrganizationId={0}", orgId);
            }
            if (branchId > 0)
            {
                param += string.Format(@"and ts.BranchId={0} ", branchId);
            }
            if (tsId != null && tsId > 0)
            {
                param += string.Format(@"and ts.TSId ={0}", tsId);
            }
            if (!string.IsNullOrEmpty(jobCode))
            {
                param += string.Format(@"and ts.JobOrderCode Like '%{0}%'", jobCode);
            }
            if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "" && !string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(ts.EntryDate as date) between '{0}' and '{1}'", fDate, tDate);
            }
            else if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(ts.EntryDate as date)='{0}'", fDate);
            }
            else if (!string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(ts.EntryDate as date)='{0}'", tDate);
            }

            query = string.Format(@"select JobOrderCode,TSId,StateStatus,Remarks,apu.UserName'TSName',ts.EntryDate,ts.AssignDate,ts.SignOutDate 
                from tblJobOrderTS ts
                left join [ControlPanel].dbo.tblApplicationUsers apu on ts.TSId=apu.UserId
                where 1=1{0} order by ts.EntryDate desc", Utility.ParamChecker(param));
            return query;
        }

        public bool UpdateJobOrderTsStatus(long joborderId, long userId, long orgId, long branchId)
        {
            var jobOrderTsStatus = GetJobOrderActiveTsByJobOrderId(joborderId, orgId,branchId);
            if (jobOrderTsStatus != null)
            {
                jobOrderTsStatus.StateStatus = "Sign-Out";
                jobOrderTsStatus.IsActive = false;
                jobOrderTsStatus.UpUserId = userId;
                jobOrderTsStatus.SignOutDate = DateTime.Now;
                _jobOrderTSRepository.Update(jobOrderTsStatus);
            }
            return _jobOrderTSRepository.Save();
        }
    }
}
