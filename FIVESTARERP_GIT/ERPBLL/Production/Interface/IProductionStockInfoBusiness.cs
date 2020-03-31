using ERPBO.Production.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IProductionStockInfoBusiness
    {
        IEnumerable<ProductionStockInfo> GetAllProductionStockInfoByOrgId(long orgId);
        ProductionStockInfo GetAllProductionStockInfoByItemLineId(long orgId,long itemId,long lineId);
        ProductionStockInfo GetAllProductionStockInfoByLineAndModelId(long orgId, long itemId, long lineId, long modelId);
    }
}
