using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IRepairItemBusiness
    {
        bool SaveRepairItem(RepairItemDTO repairItemDTO, long userId, long orgId);
        IEnumerable<RepairItemDTO> GetRepairItemInfoList(string repairCode, long? floorId, long? repairLineId, long? qcLineId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId,string fromDate, string toDate,long orgId);
    }
}
