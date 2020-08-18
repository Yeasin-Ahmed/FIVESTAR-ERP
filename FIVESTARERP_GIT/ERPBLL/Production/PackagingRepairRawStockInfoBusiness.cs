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
    public class PackagingRepairRawStockInfoBusiness : IPackagingRepairRawStockInfoBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly PackagingRepairRawStockInfoRepository _packagingRepairRawStockInfoRepository;
        public PackagingRepairRawStockInfoBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._packagingRepairRawStockInfoRepository = new PackagingRepairRawStockInfoRepository(this._productionDb);
        }
        public PackagingRepairRawStockInfo GetPackagingRepairRawStockInfoByPackagingLineAndModelAndItem(long floorId, long packagingLine, long itemId, long modelId, long orgId)
        {
            return this._packagingRepairRawStockInfoRepository.GetOneByOrg(s => s.FloorId == floorId && s.PackagingLineId == packagingLine && s.ItemId == itemId && s.DescriptionId == modelId && s.OrganizationId == orgId);
        }

        public PackagingRepairRawStockInfo GetPackagingRepairRawStockInfoByPackagingLineAndModelAndItem(long packagingLine, long itemId, long modelId, long orgId)
        {
            return this._packagingRepairRawStockInfoRepository.GetOneByOrg(s =>  s.PackagingLineId == packagingLine && s.ItemId == itemId && s.DescriptionId == modelId && s.OrganizationId == orgId);
        }

        public IEnumerable<PackagingRepairRawStockInfoDTO> GetPackagingRepairRawStockInfosByQuery(long? floorId, long? packagingLine, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string lessOrEq, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<PackagingRepairRawStockInfoDTO>(QueryForPackagingRepairRawStockInfos(floorId, packagingLine, modelId, warehouseId, itemTypeId, itemId, lessOrEq, orgId)).ToList();
        }

        private string QueryForPackagingRepairRawStockInfos(long? floorId, long? packagingLine, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string lessOrEq, long orgId)
        {
            string param = string.Empty;
            string query = string.Empty;
            if (floorId != null && floorId > 0)
            {
                param += string.Format(@" and stock.FloorId ={0}", floorId);
            }
            if (modelId != null && modelId > 0)
            {
                param += string.Format(@" and stock.DescriptionId ={0}", modelId);
            }
            if (warehouseId != null && warehouseId > 0)
            {
                param += string.Format(@" and stock.WarehouseId ={0}", warehouseId);
            }
            if (itemTypeId != null && itemTypeId > 0)
            {
                param += string.Format(@" and stock.ItemId ={0}", itemTypeId);
            }
            if (itemId != null && itemId > 0)
            {
                param += string.Format(@" and stock.ItemId ={0}", itemId);
            }
            if (!string.IsNullOrEmpty(lessOrEq) && lessOrEq.Trim() != "")
            {
                int qty = Convert.ToInt32(lessOrEq);
                param += string.Format(@" and (stock.StockInQty - stock.StockOutQty) <= {0}", qty);
            }

            query = string.Format(@"
Select pl.LineNumber 'ProductionFloorName',pac.PackagingLineName,de.DescriptionName 'ModelName',w.WarehouseName,it.ItemName 'ItemTypeName',i.ItemName,stock.StockInQty,stock.StockOutQty,U.UnitSymbol 'UnitName',stock.Remarks
From [Production].dbo.tblPackagingRepairRawStockInfo stock
Inner Join [Production].dbo.tblProductionLines pl on stock.FloorId = pl.LineId
Left Join [Production].dbo.tblPackagingLine pac on stock.PackagingLineId = pac.PackagingLineId
Left Join [Inventory].dbo.tblWarehouses w on stock.WarehouseId = w.Id
Left Join [Inventory].dbo.tblItemTypes it on stock.ItemTypeId = it.ItemId
Left Join [Inventory].dbo.tblItems i on stock.ItemId = i.ItemId
Left Join [Inventory].dbo.tblUnits u on stock.UnitId = u.UnitId
Left Join [Inventory].dbo.tblDescriptions de on stock.DescriptionId = de.DescriptionId
Where 1=1 and stock.OrganizationId ={0} {1}", orgId, Utility.ParamChecker(param));
            return query;
        }
    }
}
