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
    public class PackagingFaultyStockInfoBusiness : IPackagingFaultyStockInfoBusiness
    {
        // Database
        private readonly IProductionUnitOfWork _productionDb;
        // Repository
        private readonly PackagingFaultyStockInfoRepository _packagingFaultyStockInfoRepository;
        public PackagingFaultyStockInfoBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._packagingFaultyStockInfoRepository = new PackagingFaultyStockInfoRepository(this._productionDb);
        }

        public PackagingFaultyStockInfo GetPackagingFaultyStockInfoByPackagingLineAndItem(long packagingLineId, long itemId, long orgId)
        {
            return this._packagingFaultyStockInfoRepository.GetOneByOrg(s => s.PackagingLineId == packagingLineId && s.ItemId == itemId && s.OrganizationId == orgId);
        }

        public PackagingFaultyStockInfo GetPackagingFaultyStockInfoByPackagingLineAndModelAndItem(long packagingLineId, long modelId, long itemId, long orgId)
        {
            return this._packagingFaultyStockInfoRepository.GetOneByOrg(s => s.PackagingLineId == packagingLineId && s.DescriptionId == modelId && s.ItemId == itemId && s.OrganizationId == orgId);
        }

        public PackagingFaultyStockInfo GetPackagingFaultyStockInfoByRepairAndModelAndItemAndFultyType(long packagingLineId, long modelId, long itemId, bool isChinaFaulty, long orgId)
        {
            return this._packagingFaultyStockInfoRepository.GetOneByOrg(s => s.PackagingLineId == packagingLineId && s.DescriptionId == modelId && s.ItemId == itemId && s.IsChinaFaulty == isChinaFaulty && s.OrganizationId == orgId);
        }

        public IEnumerable<PackagingFaultyStockInfo> GetPackagingFaultyStockInfos(long orgId)
        {
            return this._packagingFaultyStockInfoRepository.GetAll(s => s.OrganizationId == orgId);
        }

        public IEnumerable<PackagingFaultyStockInfoDTO> GetPackagingFaultyStockInfosByQuery(long? floorId, long? packagingLineId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string lessOrEq, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<PackagingFaultyStockInfoDTO>(QueryForPackagingFaultyStockInfos(floorId, packagingLineId, modelId, warehouseId, itemTypeId, itemId, lessOrEq, orgId)).ToList();
        }

        private string QueryForPackagingFaultyStockInfos(long? floorId, long? packagingLineId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string lessOrEq, long orgId)
        {
            string param = string.Empty;
            string query = string.Empty;

            if (floorId != null && floorId > 0)
            {
                param += string.Format(@" and pfi.ProductionFloorId={0}", floorId);
            }
            if (packagingLineId != null && packagingLineId > 0)
            {
                param += string.Format(@" and pfi.PackagingLineId={0}", packagingLineId);
            }
            if (modelId != null && modelId > 0)
            {
                param += string.Format(@" and pfi.DescriptionId={0}", modelId);
            }
            if (warehouseId != null && warehouseId > 0)
            {
                param += string.Format(@" and pfi.WarehouseId={0}", warehouseId);
            }
            if (itemTypeId != null && itemTypeId > 0)
            {
                param += string.Format(@" and pfi.ItemTypeId={0}", itemTypeId);
            }
            if (itemId != null && itemId > 0)
            {
                param += string.Format(@" and pfi.ItemId={0}", itemId);
            }
            if (!string.IsNullOrEmpty(lessOrEq) && lessOrEq.Trim() != "")
            {
                int i = 0;
                if (int.TryParse(lessOrEq, out i) && i > 0)
                {
                    param += string.Format(@" and (pfi.StockInQty-pfi.StockOutQty) >{0}", i);
                };
            }

            query = string.Format(@"Select  pfi.PackagingFaultyStockInfoId,ISNULL(pl.LineNumber,'') 'ProductionFloorName',pfi.PackagingLineId,pac.PackagingLineName , ISNULL(de.DescriptionName,'') 'ModelName', 
ISNULL(w.WarehouseName,'') 'WarehouseName', ISNULL(it.ItemName,'') 'ItemTypeName', ISNULL(i.ItemName,'') 'ItemName',
ISNULL(u.UnitSymbol,'') 'UnitName',pfi.StockInQty,pfi.StockOutQty, pfi.IsChinaFaulty,(Case When pfi.IsChinaFaulty = 1 then 'China Made' else 'Man Made' End)'FaultyReason' 
From [Production].dbo.tblPackagingFaultyStockInfo pfi
Inner Join [Production].dbo.tblProductionLines pl on pfi.ProductionFloorId = pl.LineId
Inner Join [Production].dbo.tblPackagingLine pac on pfi.PackagingLineId = pac.PackagingLineId
Left Join [Inventory].dbo.tblDescriptions de on pfi.DescriptionId= de.DescriptionId
Left Join [Inventory].dbo.[tblWarehouses] w on pfi.WarehouseId = w.Id
Left Join [Inventory].dbo.[tblItemTypes] it on pfi.ItemTypeId = it.ItemId
Left Join [Inventory].dbo.[tblItems] i on pfi.ItemId = i.ItemId
Left Join [Inventory].dbo.[tblUnits] u on pfi.UnitId = u.UnitId 
Where 1=1 and pfi.OrganizationId={0} {1}", orgId, Utility.ParamChecker(param));

            return query;
        }
    }
}
