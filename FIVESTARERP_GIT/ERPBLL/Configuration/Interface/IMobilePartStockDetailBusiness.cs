using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration.Interface
{
   public interface IMobilePartStockDetailBusiness
    {
        IEnumerable<MobilePartStockDetail> GelAllMobilePartStockDetailByOrgId(long orgId,long branchId);
        bool SaveMobilePartStockIn(List<MobilePartStockDetailDTO> mobilePartStockDetailDTO, long userId, long orgId, long branchId);
        bool SaveMobilePartStockOut(List<MobilePartStockDetailDTO> mobilePartStockDetailDTO, long userId, long orgId, long branchId);
        bool StockInByBranchTransferApproval(long transferId, string status, long userId, long branchId, long orgId);
    }
}
