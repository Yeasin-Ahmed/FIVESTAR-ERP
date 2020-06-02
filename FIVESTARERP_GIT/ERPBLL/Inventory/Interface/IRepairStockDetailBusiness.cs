using ERPBO.Inventory.DomainModels;
using ERPBO.Inventory.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Inventory.Interface
{
    public interface IRepairStockDetailBusiness
    {
        IEnumerable<RepairStockDetail> GetRepairStockDetails(long orgId);
        RepairStockDetail GetRepairStockDetailById(long orgId,long stockDetailId);
        bool SaveRepairStockIn(List<RepairStockDetailDTO> repairStockDetailDTOs,long orgId, long userId);
        IEnumerable<RepairStockDetailListDTO> GetRepairStockDetailList(long? lineId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string stockStatus, string fromDate, string toDate,string refNum,long orgId, string returnType, string faultyCase);
    }
}
