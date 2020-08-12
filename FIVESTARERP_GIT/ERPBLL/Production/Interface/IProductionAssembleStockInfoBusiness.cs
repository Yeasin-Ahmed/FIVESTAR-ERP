using ERPBO.Common;
using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IProductionAssembleStockInfoBusiness
    {
        IEnumerable<ProductionAssembleStockInfo> GetProductionAssembleStockInfos(long orgId);
        ProductionAssembleStockInfo productionAssembleStockInfoByFloorAndModelAndItem(long floorId, long modelId, long itemId, long orgId);
        List<Dropdown> GetAllItemsInStock(long orgId);

        IEnumerable<ProductionAssembleStockInfoDTO> GetProductionAssembleStockInfoByQuery(long? floorId, long? modelId, long? warehouse, long? itemTypeId, long? itemId, string lessOrEq, long orgId);
    }
}
