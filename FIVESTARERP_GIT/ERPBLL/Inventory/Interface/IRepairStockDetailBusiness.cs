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
    }
}
