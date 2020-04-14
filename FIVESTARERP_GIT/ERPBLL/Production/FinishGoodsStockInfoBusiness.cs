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
    public class FinishGoodsStockInfoBusiness : IFinishGoodsStockInfoBusiness
    {
        private readonly IProductionUnitOfWork _productionDb; // database
        private readonly FinishGoodsStockInfoRepository _finishGoodsStockInfoRepository; // table

        public FinishGoodsStockInfoBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._finishGoodsStockInfoRepository = new FinishGoodsStockInfoRepository(this._productionDb);
        }
        public IEnumerable<FinishGoodsStockInfo> GetAllFinishGoodsStockInfoByOrgId(long orgId)
        {
            return _finishGoodsStockInfoRepository.GetAll(ware => ware.OrganizationId == orgId).ToList();
        }
        public FinishGoodsStockInfo GetAllFinishGoodsStockInfoByItemLineId(long orgId,long itemId,long lineId)
        {
            return _finishGoodsStockInfoRepository.GetAll(ware => ware.OrganizationId == orgId && ware.ItemId == itemId  && ware.LineId == lineId).FirstOrDefault();
        }
        public FinishGoodsStockInfo GetAllFinishGoodsStockInfoByLineAndModelId(long orgId, long itemId, long lineId, long modelId)
        {
            return _finishGoodsStockInfoRepository.GetAll(ware => ware.OrganizationId == orgId && ware.ItemId == itemId && ware.LineId == lineId && ware.DescriptionId == modelId).FirstOrDefault();
        }
    }
}
