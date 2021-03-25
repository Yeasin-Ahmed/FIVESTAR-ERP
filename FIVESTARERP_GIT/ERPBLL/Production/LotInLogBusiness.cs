using ERPBLL.Common;
using ERPBLL.Production.Interface;
using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using ERPDAL.ProductionDAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production
{
   public class LotInLogBusiness: ILotInLogBusiness
    {
        private readonly IProductionUnitOfWork _productionDb; // database
        private readonly LotInLogRepository lotInLogRepository; // table
        public LotInLogBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            lotInLogRepository = new LotInLogRepository(this._productionDb);
        }

        public IEnumerable<LotInLogDTO> GetAllDataByDateWise(string fromDate, string toDate, string qrCode)
        {
            IEnumerable<LotInLogDTO> lotInLogs = new List<LotInLogDTO>();
            lotInLogs = this._productionDb.Db.Database.SqlQuery<LotInLogDTO>(QueryForLotInDateWiseData(fromDate, toDate, qrCode)).ToList();
            return lotInLogs;
        }
        private string QueryForLotInDateWiseData(string fromDate, string toDate, string qrCode)
        {
            string query = string.Empty;
            string param = string.Empty;
            string q = qrCode.Trim();
            if (!string.IsNullOrEmpty(q) && q.Trim() != "")
            {
                param += string.Format(@" and li.CodeNo='{0}'", q);
            }

            if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "" && !string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(li.EntryDate as date) between '{0}' and '{1}'", fDate, tDate);
            }
            else if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(li.EntryDate as date)='{0}'", fDate);
            }
            else if (!string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(li.EntryDate as date)='{0}'", tDate);
            }

            query = string.Format(@" Select li.EntryDate, au.UserName 'EUserName',ISNULL(li.ReferenceNumber,'N/A') as 'ReferenceNumber', li.StateStatus,li.CodeNo,li.LotInLogId,li.Remarks
From tblLotInLog li
Left Join [ControlPanel].dbo.tblApplicationUsers au on li.EUserId = au.UserId
Where 1=1 {0}", Utility.ParamChecker(param));
            return query;
        }

        public IEnumerable<LotInLog> GetAllLotInByToDay(long userId, long orgId, DateTime date)
        {
            return lotInLogRepository.GetAll(s => s.EUserId == userId && s.OrganizationId == orgId && DbFunctions.TruncateTime(s.EntryDate) == DbFunctions.TruncateTime(date)).ToList();
        }
    }
}
