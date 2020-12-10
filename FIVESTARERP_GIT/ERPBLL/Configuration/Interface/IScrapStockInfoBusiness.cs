using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration.Interface
{
    public interface IScrapStockInfoBusiness
    {
        ScrapStockInfo GetScrapStockInfoByModelAndCostPriceAndSellPrice(long modelId, long partsId, double cPrice, double sPrice, long orgId, long branchId);
        IEnumerable<ScrapStockInfoDTO> GetScrapStockByOrgId(long orgId, long branchId);
    }
}
