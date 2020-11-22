using ERPBLL.SalesAndDistribution.Interface;
using ERPBO.SalesAndDistribution.DTOModels;
using ERPDAL.SalesAndDistributionDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.SalesAndDistribution
{
    public class DealerRequisitionDetailBusiness : IDealerRequisitionDetailBusiness
    {
        private readonly DealerRequisitionDetailRepository _dealerRequisitionDetailRepository;
        private readonly ISalesAndDistributionUnitOfWork _salesAndDistributionDb;
        public DealerRequisitionDetailBusiness(ISalesAndDistributionUnitOfWork salesAndDistributionDb)
        {
            this._salesAndDistributionDb = salesAndDistributionDb;
            this._dealerRequisitionDetailRepository = new DealerRequisitionDetailRepository(this._salesAndDistributionDb);
        }
        public IEnumerable<DealerRequisitionDetailDTO> GetDealerRequisitionDetails(long reqInfoId, long orgId)
        {
            return _salesAndDistributionDb.Db.Database.SqlQuery<DealerRequisitionDetailDTO>(string.Format(@"Exec spDealerRequisitionDetail {0},{1}", orgId, reqInfoId)).ToList();
        }
    }
}
