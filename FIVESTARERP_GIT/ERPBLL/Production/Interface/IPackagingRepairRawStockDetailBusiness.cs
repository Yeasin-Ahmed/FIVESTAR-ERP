using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IPackagingRepairRawStockDetailBusiness
    {
        IEnumerable<PackagingRepairRawStockDetailDTO> GetPackagingRepairRawStockDetailByQuery(long floorId, long packagingLine, long modelId, long warehouseId, long itemTypeId, long itemId, string refNum, string fromDate, string toDate, string status);
        bool SavePackagingRepairRawStockIn(List<PackagingRepairRawStockDetailDTO> stockDetailDTOs, long userId, long orgId);
        bool SavePackagingRepairRawStockOut(List<PackagingRepairRawStockDetailDTO> stockDetailDTOs, long userId, long orgId);
        Task<bool> SavePackagingRepairRawStockInAsync(List<PackagingRepairRawStockDetailDTO> stockDetailDTOs, long userId, long orgId);
        Task<bool> SavePackagingRepairRawStockOutAsync(List<PackagingRepairRawStockDetailDTO> stockDetailDTOs, long userId, long orgId);
        bool StockInByPackagingSectionRequisition(long reqId, string status, long userId, long orgId);

        bool SavePackagingRepairRawStockReturn(List<PackagingRepairRawStockDetailDTO> stockDetailDTOs, long userId, long orgId);
    }
}
