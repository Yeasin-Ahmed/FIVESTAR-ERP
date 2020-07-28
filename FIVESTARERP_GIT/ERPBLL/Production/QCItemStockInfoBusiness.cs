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
    public class QCItemStockInfoBusiness : IQCItemStockInfoBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly QCItemStockInfoRepository _qCItemStockInfoRepository;
        public QCItemStockInfoBusiness(IProductionUnitOfWork productionDb, QCItemStockInfoRepository qCItemStockInfoRepository)
        {
            this._productionDb = productionDb;
            this._qCItemStockInfoRepository = new QCItemStockInfoRepository(this._productionDb);
        }
        public QCItemStockInfo GetQCItemStockInfById(long qcId, long modelId, long itemId, long orgId)
        {
            return _qCItemStockInfoRepository.GetOneByOrg(d=> d.QCId== qcId && d.DescriptionId == modelId && d.ItemId == itemId && d.OrganizationId == orgId);
        }

        public QCItemStockInfo GetQCItemStockInfoByFloorAndQcAndModelAndItem(long floorId, long assemblyId, long qcId, long modelId, long itemId, long orgId)
        {
            return _qCItemStockInfoRepository.GetOneByOrg(d =>d.ProductionFloorId == floorId && d.AssemblyLineId == assemblyId && d.QCId == qcId && d.DescriptionId == modelId && d.ItemId == itemId && d.OrganizationId == orgId);
        }

        // Async Method
        public async Task<QCItemStockInfo> GetQCItemStockInfoByFloorAndQcAndModelAndItemAsync(long floorId, long assemblyId, long qcId, long modelId, long itemId, long orgId)
        {
            return await _qCItemStockInfoRepository.GetOneByOrgAsync(d => d.ProductionFloorId == floorId && d.AssemblyLineId == assemblyId && d.QCId == qcId && d.DescriptionId == modelId && d.ItemId == itemId && d.OrganizationId == orgId);
        }

        public IEnumerable<QCItemStockInfoDTO> GetQCItemStockInfosByQuery(long? floorId, long? assemblyId, long? qCId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<QCItemStockInfoDTO>(QueryForQCItemStockInfos(floorId,assemblyId,qCId,modelId,warehouseId,itemTypeId,itemId,orgId)).ToList();
        }

        private string QueryForQCItemStockInfos(long? floorId, long? assemblyId, long? qCId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId,long orgId)
        {
            string param = string.Empty;
            string query = string.Empty;

            param += string.Format(@" and qi.OrganizationId={0}", orgId);
            if (floorId != null && floorId > 0)
            {
                param += string.Format(@" and qi.ProductionFloorId={0}", floorId);
            }
            if (assemblyId != null && assemblyId > 0)
            {
                param += string.Format(@" and qi.AssemblyLineId={0}", assemblyId);
            }
            if (modelId != null && modelId > 0)
            {
                param += string.Format(@" and qi.DescriptionId={0}", modelId);
            }
            if (warehouseId != null && warehouseId > 0)
            {
                param += string.Format(@" and qi.WarehouseId={0}", warehouseId);
            }
            if (itemTypeId != null && itemTypeId > 0)
            {
                param += string.Format(@" and qi.ItemTypeId={0}", itemTypeId);
            }
            if (itemId != null && itemId > 0)
            {
                param += string.Format(@" and qi.ItemId={0}", itemId);
            }
            query = string.Format(@"Select qi.ProductionFloorId,pl.LineNumber 'ProductionFloorName', qi.AssemblyLineId,al.AssemblyLineName,
qi.QCId,qc.QCName,qi.DescriptionId,de.DescriptionName 'ModelName',qi.WarehouseId,w.WarehouseName,qi.ItemTypeId,it.ItemName 'ItemTypeName',qi.ItemId,i.ItemName,
qi.Quantity,qi.RepairQty,qi.MiniStockQty
From [Production].dbo.tblQCItemStockInfo qi
Inner Join [Production].dbo.tblProductionLines pl on qi.ProductionFloorId = pl.LineId
Inner Join [Production].dbo.tblAssemblyLines al on qi.AssemblyLineId = al.AssemblyLineId
Inner Join [Production].dbo.tblQualityControl qc on qi.QCId = qc.QCId
Inner Join [Inventory].dbo.tblDescriptions de on qi.DescriptionId = de.DescriptionId
Inner Join [Inventory].dbo.tblWarehouses w on qi.WarehouseId = w.Id
Inner Join [Inventory].dbo.tblItemTypes it on qi.ItemTypeId = it.ItemId
Inner Join [Inventory].dbo.tblItems i on qi.ItemId = i.ItemId
Where 1 = 1 {0}",Utility.ParamChecker(param));

            return query;
        }

        public IEnumerable<QCItemStockInfo> GetQCItemStocks(long orgId)
        {
            return _qCItemStockInfoRepository.GetAll(d => d.OrganizationId == orgId);
        }
    }
}
