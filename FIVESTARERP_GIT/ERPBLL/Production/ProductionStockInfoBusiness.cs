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
    public class ProductionStockInfoBusiness : IProductionStockInfoBusiness
    {
        private readonly IProductionUnitOfWork _productionDb; // database
        private readonly ProductionStockInfoRepository productionStockInfoRepository; // table

        public ProductionStockInfoBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            productionStockInfoRepository = new ProductionStockInfoRepository(this._productionDb);
        }
        public IEnumerable<ProductionStockInfo> GetAllProductionStockInfoByOrgId(long orgId)
        {
            return productionStockInfoRepository.GetAll(ware => ware.OrganizationId == orgId).ToList();
        }
        public ProductionStockInfo GetAllProductionStockInfoByItemLineId(long orgId,long itemId,long lineId)
        {
            return productionStockInfoRepository.GetAll(ware => ware.OrganizationId == orgId && ware.ItemId == itemId  && ware.LineId == lineId).FirstOrDefault();
        }

        public ProductionStockInfo GetAllProductionStockInfoByLineAndModelId(long orgId, long itemId, long lineId, long modelId)
        {
            return productionStockInfoRepository.GetAll(ware => ware.OrganizationId == orgId && ware.ItemId == itemId && ware.LineId == lineId && ware.DescriptionId == modelId).FirstOrDefault();
        }

        public ProductionStockInfo GetAllProductionStockInfoByLineAndModelAndItemId(long orgId, long itemId, long lineId, long modelId, string stockFor)
        {
            return productionStockInfoRepository.GetAll(ware => ware.OrganizationId == orgId && ware.ItemId == itemId && ware.LineId == lineId && ware.DescriptionId == modelId && ware.StockFor ==stockFor).FirstOrDefault();
        }
    }
}
