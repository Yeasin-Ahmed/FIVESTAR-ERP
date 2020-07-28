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
    public class AssemblyLineStockInfoBusiness : IAssemblyLineStockInfoBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly AssemblyLineStockInfoRepository _assemblyLineStockInfoRepository;

        public AssemblyLineStockInfoBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._assemblyLineStockInfoRepository = new AssemblyLineStockInfoRepository(this._productionDb);
        }

        public AssemblyLineStockInfo GetAssemblyLineStockInfoByAssemblyAndItemAndModelId(long assemblyId, long itemId, long modelId, long orgId)
        {
            return _assemblyLineStockInfoRepository.GetOneByOrg(s => s.OrganizationId == orgId && s.AssemblyLineId == assemblyId && s.ItemId == itemId && s.DescriptionId == modelId);
        }

        public async Task<AssemblyLineStockInfo> GetAssemblyLineStockInfoByAssemblyAndItemAndModelIdAsync(long assemblyId, long itemId, long modelId, long orgId)
        {
            return await _assemblyLineStockInfoRepository.GetOneByOrgAsync(s => s.OrganizationId == orgId && s.AssemblyLineId == assemblyId && s.ItemId == itemId && s.DescriptionId == modelId);
        }

        public IEnumerable<AssemblyLineStockInfo> GetAssemblyLineStockInfoByAssemblyAndItemId(long assemblyId, long itemId, long orgId)
        {
            return _assemblyLineStockInfoRepository.GetAll(s => s.OrganizationId == orgId && s.AssemblyLineId == assemblyId && s.ItemId == itemId);
        }

        public IEnumerable<AssemblyLineStockInfo> GetAssemblyLineStockInfos(long orgId)
        {
            return _assemblyLineStockInfoRepository.GetAll(s => s.OrganizationId == orgId);
        }

        public IEnumerable<AssemblyLineStockInfoDTO> GetAssemblyLineStockInfosByQuery(long? floorId, long? assemblyId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string lessOrEq, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<AssemblyLineStockInfoDTO>(QueryForAssemblyLineStockInfos(floorId, assemblyId,modelId,warehouseId,itemTypeId,itemId,lessOrEq,orgId)).ToList();
        }

        private string QueryForAssemblyLineStockInfos(long? floorId, long? assemblyId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string lessOrEq, long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;

            param += string.Format(@" and ai.OrganizationId={0}",orgId);
            if(floorId !=null && floorId > 0)
            {
                param += string.Format(@" and ai.ProductionLineId={0}", floorId);
            }
            if (assemblyId != null && assemblyId > 0)
            {
                param += string.Format(@" and ai.AssemblyLineId={0}", assemblyId);
            }
            if (modelId != null && modelId > 0)
            {
                param += string.Format(@" and ai.DescriptionId={0}", modelId);
            }
            if (warehouseId != null && warehouseId > 0)
            {
                param += string.Format(@" and ai.WarehouseId={0}", warehouseId);
            }
            if (itemTypeId != null && itemTypeId > 0)
            {
                param += string.Format(@" and ai.ItemTypeId={0}", itemTypeId);
            }
            if (itemId != null && itemId > 0)
            {
                param += string.Format(@" and ai.ItemId={0}", itemTypeId);
            }
            if (!string.IsNullOrEmpty(lessOrEq) && lessOrEq.Trim() != "")
            {
                int qty = int.Parse(lessOrEq);
                param += string.Format(@" and  (ai.StockInQty - ai.StockOutQty) <= {0}", qty);
            }
            query = string.Format(@"Select ai.ProductionLineId,pl.LineNumber 'ProductionLineName',ai.AssemblyLineId,al.AssemblyLineName,
ai.DescriptionId,de.DescriptionName 'ModelName',ai.WarehouseId,w.WarehouseName,ai.ItemTypeId,it.ItemName 'ItemTypeName',ai.ItemId,i.ItemName,
ai.StockInQty,ai.StockOutQty,ai.UnitId,u.UnitSymbol 'UnitName'
From [Production].dbo.tblAssemblyLineStockInfo ai
Inner Join [Production].dbo.tblProductionLines pl on ai.ProductionLineId = pl.LineId
Inner Join [Production].dbo.tblAssemblyLines al on ai.AssemblyLineId = al.AssemblyLineId
Inner Join [Inventory].dbo.tblDescriptions de on ai.DescriptionId = de.DescriptionId
Inner Join [Inventory].dbo.tblWarehouses w on ai.WarehouseId = w.Id
Inner Join [Inventory].dbo.tblItemTypes it on ai.ItemTypeId = it.ItemId
Inner Join [Inventory].dbo.tblItems i on ai.ItemId= i.ItemId
Inner Join [Inventory].dbo.tblUnits u on ai.UnitId= u.UnitId
Where 1=1 {0}", Utility.ParamChecker(param));
            return query;
        }
    }
}
