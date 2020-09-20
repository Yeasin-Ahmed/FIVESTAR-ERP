using ERPBLL.Common;
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
   public class TsStockReturnDetailsBusiness: ITsStockReturnDetailsBusiness
    {
        private readonly IFrontDeskUnitOfWork _frontDeskUnitOfWork;
        private readonly TsStockReturnDetailRepository _tsStockReturnDetailRepository;

        public TsStockReturnDetailsBusiness(IFrontDeskUnitOfWork frontDeskUnitOfWork)
        {
            this._frontDeskUnitOfWork = frontDeskUnitOfWork;
            this._tsStockReturnDetailRepository = new TsStockReturnDetailRepository(this._frontDeskUnitOfWork);
        }

        public IEnumerable<TsStockReturnDetail> GetAllTsStockReturn(long orgId, long BranchId)
        {
            return _tsStockReturnDetailRepository.GetAll(detail => detail.OrganizationId == orgId && detail.BranchId == BranchId);
        }

        public IEnumerable<TsStockReturnDetailDTO> GetReturnParts(long orgId, long branchId, long? tsId, long? partsId, string jobCode, string fromDate, string toDate)
        {
            return _frontDeskUnitOfWork.Db.Database.SqlQuery<TsStockReturnDetailDTO>(QueryForReturnParts(orgId, branchId, tsId, partsId, jobCode, fromDate, toDate));
        }
        private string QueryForReturnParts(long orgId, long branchId, long? tsId, long? partsId, string jobCode, string fromDate, string toDate)
        {
            string query = string.Empty;
            string param = string.Empty;
            if (tsId != null && tsId > 0)
            {
                param += string.Format(@"and tsr.EUserId={0}", tsId);
            }
            if (partsId != null && partsId > 0)
            {
                param += string.Format(@"and tsr.PartsId={0}", partsId);
            }
            if (orgId > 0)
            {
                param += string.Format(@"and tsr.OrganizationId={0}", orgId);
            }
            if (branchId > 0)
            {
                param += string.Format(@"and tsr.BranchId={0}", branchId);
            }
            if (!string.IsNullOrEmpty(jobCode))
            {
                param += string.Format(@"and job.JobOrderCode Like '%{0}%'", jobCode);
            }
            if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "" && !string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(tsr.EntryDate as date) between '{0}' and '{1}'", fDate, tDate);
            }
            else if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(tsr.EntryDate as date)='{0}'", fDate);
            }
            else if (!string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(tsr.EntryDate as date)='{0}'", tDate);
            }
            query = string.Format(@"Select tsr.RequsitionCode,job.JobOrderCode,p.MobilePartName 'PartsName',p.MobilePartCode 'PartsCode',tsr.PartsId,Quantity,apu.UserName 'EntryUser',tsr.EUserId,tsr.EntryDate 
from tblTsStockReturnDetails tsr
left join [Configuration].dbo.tblMobileParts p on tsr.PartsId=p.MobilePartId
left join tblJobOrders job on tsr.JobOrderId=job.JodOrderId
left join [ControlPanel].dbo.tblApplicationUsers apu on tsr.EUserId=apu.UserId
Where 1=1{0}  order by tsr.EntryDate desc
", Utility.ParamChecker(param));
            return query;
        }
    }
}
