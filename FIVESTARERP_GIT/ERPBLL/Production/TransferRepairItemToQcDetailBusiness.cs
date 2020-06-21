using ERPBLL.Production.Interface;
using ERPBO.Production.DomainModels;
using ERPDAL.ProductionDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production
{
    public class TransferRepairItemToQcDetailBusiness : ITransferRepairItemToQcDetailBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly TransferRepairItemToQcDetailRepository _transferRepairItemToQcRepository;

        public TransferRepairItemToQcDetailBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._transferRepairItemToQcRepository = new TransferRepairItemToQcDetailRepository(this._productionDb);
        }

        public IEnumerable<TransferRepairItemToQcDetail> GetTransferRepairItemToQcDetailByInfo(long infoId, long orgId)
        {
            return _transferRepairItemToQcRepository.GetAll(t => t.TRQInfoId == infoId && t.OrganizationId == orgId).ToList();
        }

        public IEnumerable<TransferRepairItemToQcDetail> GetTransferRepairItemToQcDetails(long orgId)
        {
            return _transferRepairItemToQcRepository.GetAll(t => t.OrganizationId == orgId).ToList();
        }
    }
}
