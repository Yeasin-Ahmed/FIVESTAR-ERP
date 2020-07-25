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
    public class RepairItemStockInfoBusiness : IRepairItemStockInfoBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly RepairItemStockInfoRepository _repairItemStockInfoRepository;
        public RepairItemStockInfoBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._repairItemStockInfoRepository = new RepairItemStockInfoRepository(this._productionDb);
        }

        public RepairItemStockInfo GetRepairItem(long qcId, long repairLineId, long modelId, long itemId, long orgId)
        {
            return _repairItemStockInfoRepository.GetOneByOrg(d => d.QCId == qcId && d.RepairLineId == repairLineId && d.DescriptionId == modelId && d.ItemId == itemId && d.OrganizationId == orgId);
        }

        public async Task<RepairItemStockInfo> GetRepairItemAsync(long qcId, long repairLineId, long modelId, long itemId, long orgId)
        {
            return await _repairItemStockInfoRepository.GetOneByOrgAsync(d => d.QCId == qcId && d.RepairLineId == repairLineId && d.DescriptionId == modelId && d.ItemId == itemId && d.OrganizationId == orgId);
        }

        public IEnumerable<RepairItemStockInfo> GetRepairItemStockInfById(long repairLineId, long modelId, long itemId, long orgId)
        {
            return _repairItemStockInfoRepository.GetAll(d => d.RepairLineId == repairLineId && d.DescriptionId == modelId && d.ItemId == itemId && d.OrganizationId == orgId);
        }

        public IEnumerable<RepairItemStockInfo> GetRepairItemStockInfoByQC(long qcId, long modelId, long itemId, long orgId)
        {
            return _repairItemStockInfoRepository.GetAll(d => d.QCId == qcId && d.DescriptionId == modelId && d.ItemId == itemId && d.OrganizationId == orgId);
        }

        public IEnumerable<RepairItemStockInfoDTO> GetRepairItemStockInfosByQuery(long? floorId, long? modelId, long? qcId, long? repairId, long? warehouseId, long? itemTypeId, long? itemId, string lessOrEq,long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<RepairItemStockInfoDTO>(QueryForRepairItemStockInfos(floorId,modelId,qcId,repairId,warehouseId,itemTypeId,itemId,lessOrEq,orgId)).ToList();
        }

        private string QueryForRepairItemStockInfos(long? floorId, long? modelId, long? qcId, long? repairId, long? warehouseId, long? itemTypeId, long? itemId, string lessOrEq, long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;

            param += string.Format(@" and ri.OrganizationId={0}",orgId);
            if(floorId != null && floorId > 0)
            {
                param += string.Format(@" and ri.ProductionFloorId={0}", floorId);
            }
            if (modelId != null && modelId > 0)
            {
                param += string.Format(@" and ri.DescriptionId={0}", modelId);
            }
            if (qcId != null && qcId > 0)
            {
                param += string.Format(@" and ri.QCId={0}", qcId);
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
            if (!string.IsNullOrEmpty(lessOrEq) && lessOrEq.Trim() !="")
            {
                int qty = int.Parse(lessOrEq);
                param += string.Format(@" and  (ri.Quantity - ri.QCQty) <= {0}", qty);
            }

            query = string.Format(@"Select ri.ProductionFloorId,pl.LineNumber 'ProductionFloorName',ri.RepairLineId,rl.RepairLineName,
ri.QCId,qc.QCName,ri.DescriptionId,de.DescriptionName 'ModelName',ri.WarehouseId,w.WarehouseName,ri.ItemTypeId,it.ItemName 'ItemTypeName',ri.ItemId,i.ItemName,
ri.Quantity,ri.QCQty 
From [Production].dbo.tblRepairItemStockInfo ri
Inner Join [Production].dbo.tblProductionLines pl on ri.ProductionFloorId = pl.LineId
Inner Join [Production].dbo.tblRepairLine rl on ri.RepairLineId = rl.RepairLineId
Inner Join [Production].dbo.tblQualityControl qc on ri.QCId = qc.QCId
Inner Join [Inventory].dbo.tblDescriptions de on ri.DescriptionId = de.DescriptionId
Inner Join [Inventory].dbo.tblWarehouses w on ri.WarehouseId = w.Id
Inner Join [Inventory].dbo.tblItemTypes it on ri.ItemTypeId = it.ItemId
Inner Join [Inventory].dbo.tblItems i on ri.ItemId= i.ItemId 
Where 1= 1 {0}", Utility.ParamChecker(param));

            return query;
        }
        public IEnumerable<RepairItemStockInfo> GetRepairItemStocks(long orgId)
        {
            return _repairItemStockInfoRepository.GetAll(d => d.OrganizationId == orgId);
        }
    }
}
