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
    public class PackagingLineStockInfoBusiness : IPackagingLineStockInfoBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly PackagingLineStockInfoRepository _packagingLineStockInfoRepository;

        public PackagingLineStockInfoBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._packagingLineStockInfoRepository = new PackagingLineStockInfoRepository(this._productionDb);
        }
        public PackagingLineStockInfo GetPackagingLineStockInfoByPackagingAndItemAndModelId(long packagingId, long itemId, long modelId, long orgId)
        {
            return _packagingLineStockInfoRepository.GetOneByOrg(s=> s.PackagingLineId == packagingId && s.ItemId == itemId && s.DescriptionId == modelId && s.OrganizationId == orgId);
        }

        public IEnumerable<PackagingLineStockInfo> GetPackagingLineStockInfoByPackagingAndItemId(long packagingId, long itemId, long orgId)
        {
            return _packagingLineStockInfoRepository.GetAll(s => s.PackagingLineId == packagingId && s.ItemId == itemId && s.OrganizationId == orgId);
        }

        public IEnumerable<PackagingLineStockInfo> GetPackagingLineStockInfos(long orgId)
        {
            return _packagingLineStockInfoRepository.GetAll(s => s.OrganizationId == orgId);
        }
    }
}
