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
    public class PackagingRepairItemStockInfoBusiness : IPackagingRepairItemStockInfoBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly PackagingRepairItemStockInfoRepository _packagingRepairItemStockInfoRepository;

        public PackagingRepairItemStockInfoBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._packagingRepairItemStockInfoRepository = new PackagingRepairItemStockInfoRepository(this._productionDb);
        }
        public PackagingRepairItemStockInfo GetPackagingRepairItemStockInfoByPackagingLineAndModelAndItem(long floorId, long packagingLine, long itemId, long modelId, long orgId)
        {
            return this._packagingRepairItemStockInfoRepository.GetOneByOrg(s => s.FloorId == floorId && s.PackagingLineId == packagingLine && s.ItemId == itemId && s.DescriptionId == modelId && s.OrganizationId == orgId);
        }

        public async Task<PackagingRepairItemStockInfo> GetPackagingRepairItemStockInfoByPackagingLineAndModelAndItemAsync(long floorId, long packagingLine, long itemId, long modelId, long orgId)
        {
            return await this._packagingRepairItemStockInfoRepository.GetOneByOrgAsync(s => s.FloorId == floorId && s.PackagingLineId == packagingLine && s.ItemId == itemId && s.DescriptionId == modelId && s.OrganizationId == orgId);
        }

        public IEnumerable<PackagingRepairItemStockInfoDTO> GetPackagingRepairItemStockInfosByQuery(long? floorId, long ? packagingLine, long? modelId, long ?warehouseId, long ?itemTypeId, long ?itemId, string lessOrEq, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<PackagingRepairItemStockInfoDTO>(QueryForPackagingRepairItemStockInfos(floorId, packagingLine, modelId, warehouseId, itemTypeId, itemId, lessOrEq, orgId)).ToList();
        }

        private string QueryForPackagingRepairItemStockInfos(long? floorId, long? packagingLine, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string lessOrEq, long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;

            if (floorId != null && floorId > 0)
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

            query = string.Format(@"Select pl.LineNumber 'ProductionFloorName',pac.PackagingLineName,de.DescriptionName 'ModelName',w.WarehouseName,it.ItemName 'ItemTypeName',i.ItemName,stock.StockInQty,stock.StockOutQty,U.UnitSymbol 'UnitName',stock.Remarks
From [Production].dbo.tblPackagingRepairItemStockInfo stock
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
