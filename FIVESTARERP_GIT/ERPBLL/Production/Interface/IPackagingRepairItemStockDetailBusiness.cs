using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IPackagingRepairItemStockDetailBusiness
    {
        IEnumerable<PackagingRepairItemStockDetailDTO> GetPackagingRepairItemStockDetailByQuery(long floorId, long packagingLine, long modelId, long warehouseId, long itemTypeId, long itemId, string refNum, string fromDate, string toDate, string status);
        bool SavePackagingRepairItemStockIn(List<PackagingRepairItemStockDetailDTO> stockDetailDTOs, long userId, long orgId);
        bool SavePackagingRepairItemStockOut(List<PackagingRepairItemStockDetailDTO> stockDetailDTOs, long userId, long orgId);
        Task<bool> SavePackagingRepairItemStockInAsync(List<PackagingRepairItemStockDetailDTO> stockDetailDTOs, long userId, long orgId);
        Task<bool> SavePackagingRepairItemStockOutAsync(List<PackagingRepairItemStockDetailDTO> stockDetailDTOs, long userId, long orgId);
    }
}
