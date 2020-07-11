using ERPBLL.Production.Interface;
using ERPBO.Common;
using ERPBO.Production.DomainModels;
using ERPDAL.ProductionDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production
{
    public class ProductionAssembleStockInfoBusiness : IProductionAssembleStockInfoBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly ProductionAssembleStockInfoRepository _productionAssembleStockInfoRepository;
        public ProductionAssembleStockInfoBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._productionAssembleStockInfoRepository = new ProductionAssembleStockInfoRepository(this._productionDb);
        }

        public List<Dropdown> GetAllItemsInStock(long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<Dropdown>(string.Format(@"Select (i.ItemName+ ' ['+it.ItemName+'-'+w.WarehouseName+']') 'text',
(Cast(i.ItemId as nvarchar(50))+'#'+Cast(it.ItemId as nvarchar(50))+'#'+Cast(w.Id as nvarchar(50))) 'value'
From tblProductionAssembleStockInfo stock
Inner Join [Inventory].dbo.tblItems i on stock.ItemId =i.ItemId
Inner Join [Inventory].dbo.tblItemTypes it on stock.ItemTypeId = it.ItemId
Inner Join [Inventory].dbo.tblWarehouses w on stock.WarehouseId = w.Id
Where stock.OrganizationId={0}", orgId)).ToList();
        }

        public IEnumerable<ProductionAssembleStockInfo> GetProductionAssembleStockInfos(long orgId)
        {
            return this._productionAssembleStockInfoRepository.GetAll(s => s.OrganizationId == orgId);
        }
        public ProductionAssembleStockInfo productionAssembleStockInfoByFloorAndModelAndItem(long floorId, long modelId, long itemId, long orgId)
        {
            return this._productionAssembleStockInfoRepository.GetOneByOrg(s => s.ProductionFloorId == floorId && s.DescriptionId == modelId && s.ItemId == itemId && s.OrganizationId == orgId);
        }
    }
}
