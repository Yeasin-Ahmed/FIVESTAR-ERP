using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IPackagingFaultyStockDetailBusiness
    {
        IEnumerable<PackagingFaultyStockDetail> GetPackagingFaultyItemStocks(long orgId);
        bool SavePackagingFaultyItemStockIn(List<PackagingFaultyStockDetailDTO> stockDetails, long userId, long orgId);
        bool SavePackagingFaultyItemStockOut(List<PackagingFaultyStockDetailDTO> stockDetails, long userId, long orgId);
        IEnumerable<PackagingFaultyStockDetailDTO> GetPackagingFaultyItemStockDetailsByQrCode(string QRCode,string imei, long transferId, long orgId);
    }
}
