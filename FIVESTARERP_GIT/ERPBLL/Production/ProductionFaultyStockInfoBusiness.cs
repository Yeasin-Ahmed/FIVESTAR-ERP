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
    public class ProductionFaultyStockInfoBusiness : IProductionFaultyStockInfoBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly ProductionFaultyStockInfoRepository _productionFaultyStockInfoRepository;
        public ProductionFaultyStockInfoBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._productionFaultyStockInfoRepository = new ProductionFaultyStockInfoRepository(this._productionDb);
        }

        public IEnumerable<ProductionFaultyStockInfo> GetProductionFaultyByFloor(long floor, long orgId)
        {
            return this._productionFaultyStockInfoRepository.GetAll(s => s.ProductionFloorId == floor && s.OrganizationId == orgId);
        }

        public IEnumerable<ProductionFaultyStockInfo> GetProductionFaultyStockInfoByFloorAndItem(long floor, long itemId, long orgId)
        {
            return this._productionFaultyStockInfoRepository.GetAll(s => s.ProductionFloorId == floor && s.ItemId == itemId && s.OrganizationId == orgId);
        }

        public ProductionFaultyStockInfo GetProductionFaultyStockInfoByFloorAndModelAndItem(long floor, long modelId, long itemId, long orgId)
        {
            return this._productionFaultyStockInfoRepository.GetOneByOrg(s => s.ProductionFloorId == floor && s.ItemId == itemId && s.OrganizationId == orgId && s.DescriptionId == modelId);
        }

        public IEnumerable<ProductionFaultyStockInfo> GetProductionFaultyStockInfos(long orgId)
        {
            return this._productionFaultyStockInfoRepository.GetAll(s => s.OrganizationId == orgId);
        }
    }
}
