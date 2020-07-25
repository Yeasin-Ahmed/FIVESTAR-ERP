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
    public class RepairLineStockInfoBusiness : IRepairLineStockInfoBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly RepairLineStockInfoRepository _repairLineStockInfoRepository;

        public RepairLineStockInfoBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._repairLineStockInfoRepository = new RepairLineStockInfoRepository(this._productionDb);
        }
        public RepairLineStockInfo GetRepairLineStockInfoByRepairAndItemAndModelId(long repairId, long itemId, long modelId, long orgId)
        {
            return _repairLineStockInfoRepository.GetOneByOrg(s=> s.RepairLineId == repairId && s.ItemId == itemId && s.DescriptionId == modelId && s.OrganizationId == orgId);
        }

        public async Task<RepairLineStockInfo> GetRepairLineStockInfoByRepairAndItemAndModelIdAsync(long repairId, long itemId, long modelId, long orgId)
        {
            return await _repairLineStockInfoRepository.GetOneByOrgAsync(s => s.RepairLineId == repairId && s.ItemId == itemId && s.DescriptionId == modelId && s.OrganizationId == orgId);
        }

        public IEnumerable<RepairLineStockInfo> GetRepairLineStockInfoByRepairAndItemId(long repairId, long itemId, long orgId)
        {
            return _repairLineStockInfoRepository.GetAll(s => s.RepairLineId == repairId && s.ItemId == itemId && s.OrganizationId == orgId);
        }

        public RepairLineStockInfo GetRepairLineStockInfoByRepairQCAndItemAndModelId(long repairId, long itemId, long qcId, long modelId, long orgId)
        {
            return _repairLineStockInfoRepository.GetOneByOrg(s => s.RepairLineId == repairId && s.ItemId == itemId && s.DescriptionId == modelId && s.OrganizationId == orgId && s.QCLineId == qcId);
        }

        public IEnumerable<RepairLineStockInfo> GetRepairLineStockInfos(long orgId)
        {
            return _repairLineStockInfoRepository.GetAll(s => s.OrganizationId == orgId);
        }

        public IEnumerable<RepairLineStockInfoDTO> GetRepairLineStockInfosQuery(long? floorId, long? modelId, long? qcId, long? repairId, long? warehouseId, long? itemTypeId, long? itemId, string lessOrEq, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<RepairLineStockInfoDTO>(QueryForRepairLineStockInfos(floorId, modelId, qcId, repairId, warehouseId, itemTypeId, itemId, lessOrEq, orgId)).ToList();
        }

        private string QueryForRepairLineStockInfos(long? floorId, long? modelId, long? qcId, long? repairId, long? warehouseId, long? itemTypeId, long? itemId, string lessOrEq, long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;

            param += string.Format(@" and ri.OrganizationId={0}", orgId);
            if (floorId != null && floorId > 0)
            {
                param += string.Format(@" and ri.ProductionLineId={0}", floorId);
            }
            if (modelId != null && modelId > 0)
            {
                param += string.Format(@" and ri.DescriptionId={0}", modelId);
            }
            if (qcId != null && qcId > 0)
            {
                param += string.Format(@" and ri.QCLineId={0}", qcId);
            }
            if (repairId != null && repairId > 0)
            {
                param += string.Format(@" and ri.RepairLineId={0}", repairId);
            }
            if (warehouseId != null && warehouseId > 0)
            {
                param += string.Format(@" and ri.WarehouseId={0}", warehouseId);
            }
            if (itemTypeId != null && itemTypeId > 0)
            {
                param += string.Format(@" and ri.ItemTypeId={0}", itemTypeId);
            }
            if (itemId != null && itemId > 0)
            {
                param += string.Format(@" and ri.ItemId={0}", itemId);
            }
            if (!string.IsNullOrEmpty(lessOrEq) && lessOrEq.Trim() != "")
            {
                int qty = int.Parse(lessOrEq);
                param += string.Format(@" and  (ri.StockInQty - ri.StockOutQty) <= {0}", qty);
            }

            query = string.Format(@"Select ri.ProductionLineId,pl.LineNumber 'ProductionLineName',ri.RepairLineId,rl.RepairLineName,
ri.QCLineId,qc.QCName 'QCLineName',ri.DescriptionId,de.DescriptionName 'ModelName',ri.WarehouseId,w.WarehouseName,ri.ItemTypeId,it.ItemName 'ItemTypeName',ri.ItemId,i.ItemName,
u.UnitSymbol 'UnitName',ri.StockInQty,ri.StockOutQty  
From [Production].dbo.tblRepairLineStockInfo ri
Inner Join [Production].dbo.tblProductionLines pl on ri.ProductionLineId = pl.LineId
Inner Join [Production].dbo.tblRepairLine rl on ri.RepairLineId = rl.RepairLineId
Inner Join [Production].dbo.tblQualityControl qc on ri.QCLineId = qc.QCId
Inner Join [Inventory].dbo.tblDescriptions de on ri.DescriptionId = de.DescriptionId
Inner Join [Inventory].dbo.tblWarehouses w on ri.WarehouseId = w.Id
Inner Join [Inventory].dbo.tblItemTypes it on ri.ItemTypeId = it.ItemId
Inner Join [Inventory].dbo.tblItems i on ri.ItemId= i.ItemId
Inner Join [Inventory].dbo.tblUnits u on ri.Unitid =u.UnitId
Where 1= 1 {0}", Utility.ParamChecker(param));

            return query;
        }
    }
}
