using ERPBLL.Common;
using ERPBLL.Production.Interface;
using ERPBO.Common;
using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
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

        public IEnumerable<ProductionAssembleStockInfoDTO> GetProductionAssembleStockInfoByQuery(long? floorId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string lessOrEq, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<ProductionAssembleStockInfoDTO>(QueryForProductionAssembleStockInfo(floorId, modelId, warehouseId, itemTypeId, itemId, lessOrEq, orgId)).ToList();
        }

        private string QueryForProductionAssembleStockInfo(long? floorId, long? modelId, long? warehouseId, long? itemTypeId, long ?itemId, string lessOrEq,long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;
            if (floorId != null && floorId > 0)
            {
                param += string.Format(@" and  pas.ProductionFloorId={0}", floorId);
            }
            if (modelId != null && modelId > 0)
            {
                param += string.Format(@" and pas.DescriptionId={0}", modelId);
            }
            if (warehouseId != null && warehouseId > 0)
            {
                param += string.Format(@" and pas.WarehouseId={0}", warehouseId);
            }
            if (itemTypeId != null && itemTypeId > 0)
            {
                param += string.Format(@" and pas.ItemTypeId={0}", itemTypeId);
            }
            if (itemId != null && itemId > 0)
            {
                param += string.Format(@" and pas.ItemId={0}", itemId);
            }
            if (!string.IsNullOrEmpty(lessOrEq) && lessOrEq.Trim() != "")
            {
                int qty = Utility.TryParseInt(lessOrEq);
                param += string.Format(@" and  (pas.StockInQty - pas.StockOutQty) <= {0}", qty);
            }

            query = string.Format(@"Select pas.ProductionFloorId,pl.LineNumber 'ProductionFloorName',
pas.DescriptionId,de.DescriptionName 'ModelName',pas.WarehouseId,w.WarehouseName,pas.ItemTypeId,it.ItemName 'ItemTypeName',pas.ItemId,i.ItemName,
pas.StockInQty,pas.StockOutQty,pas.UnitId,u.UnitSymbol 'UnitName'
From [Production].dbo.tblProductionAssembleStockInfo pas
Left Join [Production].dbo.tblProductionLines pl on pas.ProductionFloorId = pl.LineId
Left Join [Inventory].dbo.tblDescriptions de on pas.DescriptionId = de.DescriptionId
Left Join [Inventory].dbo.tblWarehouses w on pas.WarehouseId = w.Id
Left Join [Inventory].dbo.tblItemTypes it on pas.ItemTypeId = it.ItemId
Left Join [Inventory].dbo.tblItems i on pas.ItemId= i.ItemId
Left Join [Inventory].dbo.tblUnits u on pas.UnitId= u.UnitId Where 1=1  and pas.OrganizationId={0} {1}", orgId,Utility.ParamChecker(param));
            return query;
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
