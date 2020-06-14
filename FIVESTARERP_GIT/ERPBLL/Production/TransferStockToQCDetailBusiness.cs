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
    public class TransferStockToQCDetailBusiness : ITransferStockToQCDetailBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly TransferStockToQCDetailRepository _transferStockToQCDetailRepository;

        public TransferStockToQCDetailBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._transferStockToQCDetailRepository = new TransferStockToQCDetailRepository(this._productionDb);
        }

        public IEnumerable<TransferStockToQCDetail> GetTransferStockToQCDetailByInfo(long infoId, long orgId)
        {
            return _transferStockToQCDetailRepository.GetAll(t => t.TSQInfoId == infoId && t.OrganizationId == orgId).ToList();
        }

        public IEnumerable<TransferStockToQCDetail> GetTransferStockToQCDetails(long orgId)
        {
            return _transferStockToQCDetailRepository.GetAll(t => t.OrganizationId == orgId).ToList();
        }
    }
}
