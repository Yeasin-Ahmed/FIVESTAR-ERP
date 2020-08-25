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
        public FinishGoodsStockInfo GetFinishGoodsStockInfoByAll(long orgId, long lineId, long warehouseId, long itemId, long modelId)
        {
            return _finishGoodsStockInfoRepository.GetOneByOrg(f => f.OrganizationId == orgId && f.ItemId == itemId && f.LineId == lineId && f.DescriptionId == modelId && f.WarehouseId == warehouseId);
        }

        public async Task<FinishGoodsStockInfo> GetAllFinishGoodsStockInfoByLineAndModelIdAsync(long orgId, long itemId, long lineId,long packagingLineId, long modelId)
        {
            return await _finishGoodsStockInfoRepository.GetOneByOrgAsync(ware => ware.OrganizationId == orgId && ware.ItemId == itemId && ware.LineId == lineId && ware.PackagingLineId== packagingLineId  && ware.DescriptionId == modelId);
        }

        public IEnumerable<FinishGoodsStockInfoDTO> GetFinishGoodsStockInfosQuery(long? floorId, long? packagingLineId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string lessOrEq, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<FinishGoodsStockInfoDTO>(QueryForFinishGoodsStockInfos(floorId, packagingLineId, modelId, warehouseId, itemTypeId, itemId, lessOrEq, orgId)).ToList();
        }

        private string QueryForFinishGoodsStockInfos(long? floorId,long? packagingLineId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string lessOrEq, long orgId)
        {
            string param = string.Empty;
            string query = string.Empty;
            param += string.Format(@" and fsi.OrganizationId={0}", orgId);
            if (floorId != null && floorId > 0)
            {
                param += string.Format(@" and fsi.LineId={0}", floorId);
            }
            if (packagingLineId != null && packagingLineId > 0)
            {
                param += string.Format(@" and fsi.PackagingLineId={0}", packagingLineId);
            }
            if (modelId != null && modelId > 0)
            {
                param += string.Format(@" and fsi.DescriptionId={0}", modelId);
            }
            if (warehouseId != null && warehouseId > 0)
            {
                param += string.Format(@" and fsi.WarehouseId={0}", warehouseId);
            }
            if (itemTypeId != null && itemTypeId > 0)
            {
                param += string.Format(@" and fsi.ItemTypeId={0}", itemTypeId);
            }
            if (itemId != null && itemId > 0)
            {
                param += string.Format(@" and fsi.ItemId={0}", itemId);
            }
            if (!string.IsNullOrEmpty(lessOrEq) && lessOrEq.Trim() !="")
            {
                int lessEq = Utility.TryParseInt(lessOrEq);
                param += string.Format(@" and (fsi.StockInQty- fsi.StockOutQty) <= {0}", lessEq);
            }

            query = string.Format(@"
Select fsi.FinishGoodsStockInfoId,pl.LineNumber 'LineNumber',
fsi.DescriptionId,de.DescriptionName 'ModelName',fsi.WarehouseId,w.WarehouseName,fsi.ItemTypeId,it.ItemName 'ItemTypeName',fsi.ItemId,i.ItemName,fsi.StockInQty,fsi.StockOutQty,fsi.PackagingLineId,pac.PackagingLineName,u.UnitSymbol 'UnitName'
From [Production].dbo.tblFinishGoodsStockInfo fsi
Inner Join [Production].dbo.tblProductionLines pl on fsi.LineId = pl.LineId
Inner Join [Production].dbo.tblPackagingLine pac on fsi.PackagingLineId = pac.PackagingLineId
Left Join [Inventory].dbo.tblDescriptions de on fsi.DescriptionId = de.DescriptionId
Left Join [Inventory].dbo.tblWarehouses w on fsi.WarehouseId = w.Id
Left Join [Inventory].dbo.tblItemTypes it on fsi.ItemTypeId = it.ItemId
Left Join [Inventory].dbo.tblItems i on fsi.ItemId = i.ItemId
Left Join [Inventory].dbo.tblUnits u on fsi.UnitId = u.UnitId
Where 1=1 {0}", Utility.ParamChecker(param));
            return query;
        }
    }
}
