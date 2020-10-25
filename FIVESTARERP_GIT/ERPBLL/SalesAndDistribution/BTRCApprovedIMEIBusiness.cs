using ERPBLL.SalesAndDistribution.Interface;
using ERPBO.Common;
using ERPBO.SalesAndDistribution.DomainModels;
using ERPBO.SalesAndDistribution.DTOModels;
using ERPDAL.SalesAndDistributionDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.SalesAndDistribution
{
    public class BTRCApprovedIMEIBusiness : IBTRCApprovedIMEIBusiness
    {
        // db
        private readonly ISalesAndDistributionUnitOfWork _salesAndDistributionDb;
        // repo
        private readonly BTRCApprovedIMEIRepository _bTRCApprovedIMEIRepository;
        public BTRCApprovedIMEIBusiness(ISalesAndDistributionUnitOfWork salesAndDistributionDb)
        {
            this._salesAndDistributionDb = salesAndDistributionDb;
            this._bTRCApprovedIMEIRepository = new BTRCApprovedIMEIRepository(this._salesAndDistributionDb);
        }
        
        public IEnumerable<BTRCApprovedIMEIDTO> GetBTRCApprovedIMEIs(long orgId, string stateStatus, long modelId = 0, string fromDate = "", string toDate = "")
        {
            return this._salesAndDistributionDb.Db.Database.SqlQuery<BTRCApprovedIMEIDTO>(string.Format(@"Exec spBTRCApprovedIMEIList {0},'{1}',{2},'{3}','{4}'",orgId,stateStatus,modelId,fromDate,toDate)).ToList();
        }

        public ExecutionStateWithText UploadIMEI(List<ApprovedIMEI> iMEIs, long userId, long orgId)
        {
            ExecutionStateWithText executionState = new ExecutionStateWithText();
            List<BTRCApprovedIMEI> list = new List<BTRCApprovedIMEI>();
            foreach (var imei in iMEIs)
            {
                BTRCApprovedIMEI item = new BTRCApprovedIMEI()
                {
                    DescriptionId = 0,
                    IMEI = imei.IMEI,
                    EUserId = userId,
                    EntryDate = DateTime.Now,
                    OrganizationId = orgId,
                    Remarks = "",
                    StateStatus = "Initiated"
                };
                list.Add(item);
            }
            _bTRCApprovedIMEIRepository.InsertAll(list);
            executionState.isSuccess = _bTRCApprovedIMEIRepository.Save();
            if (executionState.isSuccess)
            {
                executionState.text = "Data has been save successfully";
            }
            return executionState;
        }
    }
}
