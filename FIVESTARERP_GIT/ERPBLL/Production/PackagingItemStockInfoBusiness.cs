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
    public class PackagingItemStockInfoBusiness : IPackagingItemStockInfoBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly PackagingItemStockInfoRepository _packagingItemStockInfoRepository;
        public PackagingItemStockInfoBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._packagingItemStockInfoRepository = new PackagingItemStockInfoRepository(this._productionDb);
        }
        public PackagingItemStockInfo GetPackagingItemStockInfoByPackagingId(long packagingLineId, long modelId, long itemId, long orgId)
        {
            return _packagingItemStockInfoRepository.GetOneByOrg(d => d.PackagingLineId == packagingLineId && d.DescriptionId == modelId && d.ItemId == itemId && d.OrganizationId == orgId);
        }
        public IEnumerable<PackagingItemStockInfo> GetPackagingItemStockInfoByModelAndItem(long modelId, long itemId, long orgId)
        {
            return _packagingItemStockInfoRepository.GetAll(d => d.DescriptionId == modelId && d.ItemId == itemId && d.OrganizationId == orgId);
        }
        public IEnumerable<PackagingItemStockInfo> GetPackagingItemStocks(long orgId)
        {
            var data= _packagingItemStockInfoRepository.GetAll(d => d.OrganizationId == orgId);
            return data;
        }
        public IEnumerable<PackagingItemStockInfoDTO> GetPackagingItemStockInfosByQuery(long? floorId, long? packagingId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string lessOrEq,long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<PackagingItemStockInfoDTO>(QueryForPackagingItemStockInfos(floorId, packagingId, modelId, warehouseId, itemTypeId, itemId, lessOrEq, orgId)).ToList();
        }
        private string QueryForPackagingItemStockInfos(long? floorId, long? packagingId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string lessOrEq, long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;
            param += string.Format(@" ", orgId);
            if (floorId != null && floorId > 0)
            {
                param += string.Format(@" and pis.ProductionFloorId={0}", floorId);
            }
            if (modelId != null && modelId > 0)
            {
                param += string.Format(@" and pis.DescriptionId={0}", modelId);
            }
            if (packagingId != null && packagingId > 0)
            {
                param += string.Format(@" and pis.PackagingLineId={0}", packagingId);
            }
            if (warehouseId != null && warehouseId > 0)
            {
                param += string.Format(@" and pis.WarehouseId={0}", warehouseId);
            }
            if (itemTypeId != null && itemTypeId > 0)
            {
                param += string.Format(@" and pis.ItemTypeId={0}", itemTypeId);
            }
            if (itemId != null && itemId > 0)
            {
                param += string.Format(@" and pis.ItemId={0}", itemId);
            }
            if (!string.IsNullOrEmpty(lessOrEq) && lessOrEq.Trim() != "")
            {
                int lessEq = Utility.TryParseInt(lessOrEq);
                param += string.Format(@" and (pis.Quantity- pis.TransferQty) <= {0}", lessEq);
            }
            query = string.Format(@"Select pis.PItemStockInfoId,pl.LineNumber 'ProductionFloorName', pis.PackagingLineId,pac.PackagingLineName,
pis.DescriptionId,de.DescriptionName 'ModelName',pis.WarehouseId,w.WarehouseName,pis.ItemTypeId,it.ItemName 'ItemTypeName',pis.ItemId,i.ItemName,pis.Quantity,pis.TransferQty
From [Production].dbo.tblPackagignItemStockInfo pis
Inner Join [Production].dbo.tblProductionLines pl on pis.PackagingLineId = pl.LineId
Left Join [Production].dbo.tblPackagingLine pac on pis.PackagingLineId = pac.PackagingLineId
Left Join [Inventory].dbo.tblDescriptions de on pis.DescriptionId = de.DescriptionId
Left Join [Inventory].dbo.tblWarehouses w on pis.WarehouseId = w.Id
Left Join [Inventory].dbo.tblItemTypes it on pis.ItemTypeId = it.ItemId
Left Join [Inventory].dbo.tblItems i on pis.ItemId = i.ItemId where 1=1 and pis.OrganizationId={0} {1}", orgId, Utility.ParamChecker(param));
            return query;
        }
    }
}
