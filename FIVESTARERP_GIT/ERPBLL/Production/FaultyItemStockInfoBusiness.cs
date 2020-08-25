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
    public class FaultyItemStockInfoBusiness : IFaultyItemStockInfoBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly FaultyItemStockInfoRepository _faultyItemStockInfoRepository;
        public FaultyItemStockInfoBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._faultyItemStockInfoRepository = new FaultyItemStockInfoRepository(this._productionDb);
        }
        public IEnumerable<FaultyItemStockInfo> GetFaultyItemByQCandRepair(long qcId, long repairLine, long orgId)
        {
            return this._faultyItemStockInfoRepository.GetAll(f => f.QCId == qcId && f.RepairLineId == repairLine && f.OrganizationId == orgId);
        }
        public FaultyItemStockInfo GetFaultyItemStockInfoByQCandRepairandItem(long qcId, long repairLine, long itemId, long orgId)
        {
            return this._faultyItemStockInfoRepository.GetOneByOrg(f => f.QCId== qcId && f.RepairLineId == repairLine && f.ItemId == itemId && f.OrganizationId == orgId);
        }

        public FaultyItemStockInfo GetFaultyItemStockInfoByRepairAndItem(long repairLine, long itemId, long orgId)
        {
            return this._faultyItemStockInfoRepository.GetOneByOrg(f => f.RepairLineId == repairLine && f.ItemId == itemId && f.OrganizationId == orgId);
        }

        public FaultyItemStockInfo GetFaultyItemStockInfoByRepairAndModelAndItem(long repairLine, long modelId, long itemId, long orgId)
        {
            return this._faultyItemStockInfoRepository.GetOneByOrg(f => f.RepairLineId == repairLine && f.DescriptionId == modelId && f.ItemId == itemId && f.OrganizationId == orgId);
        }

        public FaultyItemStockInfo GetFaultyItemStockInfoByRepairAndModelAndItemAndFultyType(long repairLine, long modelId, long itemId, bool isChinaFaulty, long orgId)
        {
            return this._faultyItemStockInfoRepository.GetOneByOrg(f => f.RepairLineId == repairLine && f.DescriptionId == modelId && f.ItemId == itemId && f.IsChinaFaulty == isChinaFaulty && f.OrganizationId == orgId);
        }

        public IEnumerable<FaultyItemStockInfo> GetFaultyItemStockInfos(long orgId)
        {
            return this._faultyItemStockInfoRepository.GetAll(f => f.OrganizationId == orgId);
        }

        public IEnumerable<FaultyItemStockInfoDTO> GetFaultyItemStockInfosByQuery(long? floorId, long? repairId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string lessOrEq,string reqFor, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<FaultyItemStockInfoDTO>(QueryForFaultyItemStockInfos(floorId,repairId,modelId,warehouseId,itemTypeId,itemId,lessOrEq,reqFor,orgId)).ToList();
        }

        private string QueryForFaultyItemStockInfos(long? floorId, long? repairId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string lessOrEq, string reqFor, long orgId)
        {
            string param = string.Empty;
            string query = string.Empty;

            param += string.Format(@" and fs.OrganizationId={0}", orgId);
            if (floorId != null && floorId > 0)
            {
                param += string.Format(@" and fs.ProductionFloorId={0}", floorId);
            }
            if (repairId != null && repairId > 0)
            {
                param += string.Format(@" and fs.RepairLineId={0}", repairId);
            }
            if (modelId != null && modelId > 0)
            {
                param += string.Format(@" and fs.DescriptionId={0}", modelId);
            }
            if (warehouseId != null && warehouseId > 0)
            {
                param += string.Format(@" and fs.WarehouseId={0}", warehouseId);
            }
            if (itemTypeId != null && itemTypeId > 0)
            {
                param += string.Format(@" and fs.ItemTypeId={0}", itemTypeId);
            }
            if (itemId != null && itemId > 0)
            {
                param += string.Format(@" and fs.ItemId={0}", itemId);
            }
            if(!string.IsNullOrEmpty(lessOrEq) && lessOrEq.Trim() != "")
            {
                int i = 0;
                if (int.TryParse(lessOrEq, out i) && i > 0) {
                    param += string.Format(@" and (fs.StockInQty-fs.StockOutQty) >{0}", i);
                };
            }

            query = string.Format(@"Select fs.FaultyItemStockInfoId,ISNULL(pl.LineNumber,'') 'ProductionFloorName',fs.RepairLineId,rl.RepairLineName 'RepairName', ISNULL(de.DescriptionName,'') 'ModelName', 
ISNULL(w.WarehouseName,'') 'WarehouseName', ISNULL(it.ItemName,'') 'ItemTypeName', ISNULL(i.ItemName,'') 'ItemName',
ISNULL(u.UnitSymbol,'') 'UnitName',fs.StockInQty,fs.StockOutQty, fs.IsChinaFaulty,(Case When fs.IsChinaFaulty = 1 then 'China Made' else 'Man Made' End)'FaultyReason'
From tblFaultyItemStockInfo fs
Left Join tblProductionLines pl on fs.ProductionFloorId= pl.LineId
Left Join tblRepairLine rl on fs.RepairLineId= rl.RepairLineId
Left Join [Inventory].dbo.tblDescriptions de on fs.DescriptionId= de.DescriptionId
Left Join [Inventory].dbo.[tblWarehouses] w on fs.WarehouseId = w.Id
Left Join [Inventory].dbo.[tblItemTypes] it on fs.ItemTypeId = it.ItemId
Left Join [Inventory].dbo.[tblItems] i on fs.ItemId = i.ItemId
Left Join [Inventory].dbo.[tblUnits] u on fs.UnitId = u.UnitId
Where 1=1 {0}", Utility.ParamChecker(param));
            return query;
        }
    }
}
