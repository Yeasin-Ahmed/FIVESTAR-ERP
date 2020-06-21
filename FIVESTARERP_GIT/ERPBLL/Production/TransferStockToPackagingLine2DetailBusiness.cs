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
    public class TransferStockToPackagingLine2DetailBusiness :
        ITransferStockToPackagingLine2DetailBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly TransferStockToPackagingLine2DetailRepository _transferStockToPackagingLine2DetailRepository;

        public TransferStockToPackagingLine2DetailBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._transferStockToPackagingLine2DetailRepository = new TransferStockToPackagingLine2DetailRepository(this._productionDb);
        }
        public IEnumerable<TransferStockToPackagingLine2Detail> GetTransferFromPLDetailByInfo(long infoId, long orgId)
        {
            return _transferStockToPackagingLine2DetailRepository.GetAll(s => s.OrganizationId == orgId && s.TP2InfoId== infoId);
        }

        public IEnumerable<TransferStockToPackagingLine2Detail> GetTransferFromPLDetails(long orgId)
        {
             return _transferStockToPackagingLine2DetailRepository.GetAll(s => s.OrganizationId == orgId);
        }
    }
}
