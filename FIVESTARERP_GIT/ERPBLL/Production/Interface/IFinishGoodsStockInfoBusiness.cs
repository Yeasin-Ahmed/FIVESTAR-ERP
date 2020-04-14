using ERPBO.Production.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IFinishGoodsStockInfoBusiness
    {
        IEnumerable<FinishGoodsStockInfo> GetAllFinishGoodsStockInfoByOrgId(long orgId);
        FinishGoodsStockInfo GetAllFinishGoodsStockInfoByItemLineId(long orgId, long itemId, long lineId);
        FinishGoodsStockInfo GetAllFinishGoodsStockInfoByLineAndModelId(long orgId, long itemId, long lineId, long modelId);
    }
}
