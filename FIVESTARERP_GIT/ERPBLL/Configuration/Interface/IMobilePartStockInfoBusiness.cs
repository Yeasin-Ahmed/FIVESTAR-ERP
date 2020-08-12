using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration.Interface
{
   public interface IMobilePartStockInfoBusiness
    {
        IEnumerable<MobilePartStockInfo> GetAllMobilePartStockInfoByOrgId(long orgId,long branchId);
        IEnumerable<MobilePartStockInfo> GetAllMobilePartStockInfoById(long orgId, long branchId);
        MobilePartStockInfo GetAllMobilePartStockInfoByInfoId(long warehouseId, long partsId, double cprice, long orgId, long branchId);
        MobilePartStockInfo GetAllMobilePartStockInfoBySellPrice(long warehouseId, long partsId, double sprice, long orgId, long branchId);
        MobilePartStockInfo GetAllMobilePartStockById(long orgId, long branchId);
       IEnumerable<MobilePartStockInfo> GetAllMobilePartStockByParts(long warehouseId, long partsId,long orgId, long branchId);

        IEnumerable<MobilePartStockInfoDTO> GetCurrentStock(long orgId, long branchId);
    }
}
