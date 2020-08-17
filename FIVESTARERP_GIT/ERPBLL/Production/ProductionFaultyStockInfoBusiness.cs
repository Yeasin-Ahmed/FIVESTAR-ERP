using ERPBLL.Common;
using ERPBLL.Production.Interface;
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

        public IEnumerable<ProductionFaultyStockInfoDTO> GetProductionFaultyStockInfosByQuery(long? floorId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string lessOrEq, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<ProductionFaultyStockInfoDTO>(QueryForProductionFaultyStockInfos(floorId, modelId, warehouseId, itemTypeId, itemId, lessOrEq, orgId)).ToList();
        }

        private string QueryForProductionFaultyStockInfos(long? floorId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string lessOrEq, long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;

            param += string.Format(@" and stock.OrganizationId ={0}",orgId);
            if(floorId != null && floorId > 0)
            {
                param += string.Format(@" and pl.LineId ={0}", floorId);
            }
            if (modelId != null && modelId > 0)
            {
                param += string.Format(@" and de.DescriptionId ={0}", modelId);
            }
            if (warehouseId != null && warehouseId > 0)
            {
                param += string.Format(@" and w.Id ={0}", warehouseId);
            }
            if (itemTypeId != null && itemTypeId > 0)
            {
                param += string.Format(@" and it.ItemId ={0}", itemTypeId);
            }
            if (itemId != null && itemId > 0)
            {
                param += string.Format(@" and i.ItemId ={0}", itemId);
            }
            if (!string.IsNullOrEmpty(lessOrEq) && lessOrEq.Trim() != "")
            {
                int qty = Convert.ToInt32(lessOrEq);
                param += string.Format(@" and (stock.StockInQty - stock.StockOutQty) <= {0}", qty);
            }

            query = string.Format(@"Select pl.LineNumber 'ProductionFloorName',de.DescriptionName 'ModelName',w.WarehouseName,it.ItemName 'ItemTypeName',i.ItemName,stock.StockInQty,stock.StockOutQty,U.UnitSymbol 'UnitName',stock.Remarks
From [Production].dbo.tblProductionFaultyStockInfo stock
Inner Join [Production].dbo.tblProductionLines pl on stock.ProductionFloorId = pl.LineId
Left Join [Inventory].dbo.tblWarehouses w on stock.WarehouseId = w.Id
Left Join [Inventory].dbo.tblItemTypes it on stock.ItemTypeId = it.ItemId
Left Join [Inventory].dbo.tblItems i on stock.ItemId = i.ItemId
Left Join [Inventory].dbo.tblUnits u on stock.UnitId = u.UnitId
Left Join [Inventory].dbo.tblDescriptions de on stock.DescriptionId = de.DescriptionId
Where 1=1 {0}", Utility.ParamChecker(param));

            return query;
        }
    }
}
