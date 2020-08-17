using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IPackagingLineStockDetailBusiness
    {
        IEnumerable<PackagingLineStockDetail> GetPackagingLineStockDetails(long orgId);
        bool SavePackagingLineStockIn(List<PackagingLineStockDetailDTO> packagingLineStockDetailDTO, long userId, long orgId);
        bool SavePackagingLineStockInByQCLine(long transferId, string status, long orgId, long userId);
        bool SavePackagingLineStockOut(List<PackagingLineStockDetailDTO> packagingLineStockDetailDTO, long userId, long orgId, string flag);
        Task<bool> SavePackagingLineStockInAsync(List<PackagingLineStockDetailDTO> packagingLineStockDetailDTO, long userId, long orgId);
        Task<bool> SavePackagingLineStockOutAsync(List<PackagingLineStockDetailDTO> packagingLineStockDetailDTO, long userId, long orgId, string flag);

    }
}
