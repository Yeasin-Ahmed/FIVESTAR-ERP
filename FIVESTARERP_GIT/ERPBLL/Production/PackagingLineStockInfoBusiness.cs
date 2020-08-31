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

        public IEnumerable<PackagingLineStockInfoDTO> GetPackagingLineStockInfosQuery(long? floorId, long? modelId, long? packagingId, long? warehouseId, long? itemTypeId, long? itemId,string lessOrEq, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<PackagingLineStockInfoDTO>(QueryForPackagingLineStockInfos(floorId, modelId, packagingId, warehouseId, itemTypeId, itemId, lessOrEq, orgId)).ToList();
        }

        public IEnumerable<PackagingLineStockInfoDTO> GetPackagingLineStocksForReturnStock(long packagingLine, long floorId, long modelId, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<PackagingLineStockInfoDTO>(string.Format(@"Select ps.PLStockInfoId,ps.WarehouseId,w.WarehouseName,ps.ItemTypeId,it.ItemName'ItemTypeName',ps.ItemId,i.ItemName,ps.UnitId ,u.UnitSymbol 'UnitName',ps.StockInQty,ps.StockOutQty From tblPackagingLineStockInfo ps
Inner Join [Inventory].dbo.tblWarehouses w on ps.WarehouseId = w.Id
Inner Join [Inventory].dbo.tblItemTypes it on ps.ItemTypeId = it.ItemId
Inner Join [Inventory].dbo.tblItems i on ps.ItemId = i.ItemId
Inner Join [Inventory].dbo.tblUnits u on ps.UnitId = u.UnitId
Where 1=1 and ps.OrganizationId={0} and ps.ProductionLineId={1} and ps.PackagingLineId={2} and ps.DescriptionId= {3}  and (ps.StockInQty-ps.StockOutQty) > 0", orgId, floorId, packagingLine, modelId)).ToList();
        }

        private string QueryForPackagingLineStockInfos(long? floorId, long? modelId, long? packagingId, long? warehouseId, long? itemTypeId, long? itemId,string lessOrEq, long orgId)
        {
            string param = string.Empty;
            string query = string.Empty;
            param += string.Format(@" and psi.OrganizationId={0}",orgId);
            if(floorId != null && floorId > 0)
            {
                param += string.Format(@" and psi.ProductionLineId={0}", floorId);
            }
            if (modelId != null && modelId > 0)
            {
                param += string.Format(@" and psi.DescriptionId={0}", modelId);
            }
            if (packagingId != null && packagingId > 0)
            {
                param += string.Format(@" and psi.PackagingLineId={0}", packagingId);
            }
            if (warehouseId != null && warehouseId > 0)
            {
                param += string.Format(@" and psi.WarehouseId={0}", warehouseId);
            }
            if (itemTypeId != null && itemTypeId > 0)
            {
                param += string.Format(@" and psi.ItemTypeId={0}", itemTypeId);
            }
            if (itemId != null && itemId > 0)
            {
                param += string.Format(@" and psi.ItemId={0}", itemId);
            }
            int lessEq = Utility.TryParseInt(lessOrEq);
            if (lessEq > 0)
            {
                param += string.Format(@" and (psi.StockInQty- psi.StockOutQty) <= {0}",lessEq);
            }

            query = string.Format(@"
Select psi.PLStockInfoId,pl.LineNumber 'ProductionLineName', psi.PackagingLineId,pac.PackagingLineName,
psi.DescriptionId,de.DescriptionName 'ModelName',psi.WarehouseId,w.WarehouseName,psi.ItemTypeId,it.ItemName 'ItemTypeName',psi.ItemId,i.ItemName,psi.StockInQty,psi.StockOutQty
From [Production].dbo.tblPackagingLineStockInfo psi
Inner Join [Production].dbo.tblProductionLines pl on psi.ProductionLineId = pl.LineId
Left Join [Production].dbo.tblPackagingLine pac on psi.PackagingLineId = pac.PackagingLineId
Left Join [Inventory].dbo.tblDescriptions de on psi.DescriptionId = de.DescriptionId
Left Join [Inventory].dbo.tblWarehouses w on psi.WarehouseId = w.Id
Left Join [Inventory].dbo.tblItemTypes it on psi.ItemTypeId = it.ItemId
Left Join [Inventory].dbo.tblItems i on psi.ItemId = i.ItemId Where 1=1 {0}", Utility.ParamChecker(param));
            return query;
        }
    }
}
