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
    public class PackagingItemStockInfoBusiness : IPackagingItemStockInfoBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly PackagingItemStockInfoRepository _packagingItemStockInfoRepository;
        public PackagingItemStockInfoBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._packagingItemStockInfoRepository = new PackagingItemStockInfoRepository(this._productionDb);
        }
        public PackagingItemStockInfo GetPackagingItemStockInfoByPackagingId(long packagingLineId, long modelId, long itemId, long orgId)
        {
            return _packagingItemStockInfoRepository.GetOneByOrg(d => d.PackagingLineId == packagingLineId && d.DescriptionId == modelId && d.ItemId == itemId && d.OrganizationId == orgId);
        }

        public IEnumerable<PackagingItemStockInfo> GetPackagingItemStockInfoByModelAndItem(long modelId, long itemId, long orgId)
        {
            return _packagingItemStockInfoRepository.GetAll(d => d.DescriptionId == modelId && d.ItemId == itemId && d.OrganizationId == orgId);
        }

        public IEnumerable<PackagingItemStockInfo> GetPackagingItemStocks(long orgId)
        {
            var data= _packagingItemStockInfoRepository.GetAll(d => d.OrganizationId == orgId);
            return data;
        }
    }
}
