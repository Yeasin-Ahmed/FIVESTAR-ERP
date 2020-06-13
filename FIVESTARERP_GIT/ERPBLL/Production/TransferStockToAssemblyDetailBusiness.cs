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
    public class TransferStockToAssemblyDetailBusiness : ITransferStockToAssemblyDetailBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly TransferStockToAssemblyDetailRepository _transferStockToAssemblyDetailRepository;

        public TransferStockToAssemblyDetailBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._transferStockToAssemblyDetailRepository = new TransferStockToAssemblyDetailRepository(this._productionDb);
        }

        public IEnumerable<TransferStockToAssemblyDetail> GetTransferStockToAssemblyDetailByInfo(long infoId, long orgId)
        {
            return _transferStockToAssemblyDetailRepository.GetAll(t => t.TSAInfoId == infoId && t.OrganizationId == orgId).ToList();
        }

        public IEnumerable<TransferStockToAssemblyDetail> GetTransferStockToAssemblyDetails(long orgId)
        {
            return _transferStockToAssemblyDetailRepository.GetAll(t => t.OrganizationId == orgId).ToList();
        }
    }
}
