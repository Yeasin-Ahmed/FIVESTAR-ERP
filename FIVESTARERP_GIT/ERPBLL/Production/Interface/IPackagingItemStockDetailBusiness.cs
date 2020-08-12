using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IPackagingItemStockDetailBusiness
    {
        IEnumerable<PackagingItemStockDetail> GetPackagingItemStockDetails(long orgId);
        bool SavePackagingItemStockOut(List<PackagingItemStockDetailDTO> stockDetails, long userId, long orgId);
        bool SavePackagingItemStockIn(List<PackagingItemStockDetailDTO> stockDetails, long userId, long orgId);
        bool SavePackagingItemStockInByMiniStockTransfer(long transferId, string status, long userId, long orgId);
    }
}
