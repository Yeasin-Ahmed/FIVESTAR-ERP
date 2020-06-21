using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IRepairLineStockDetailBusiness
    {
        IEnumerable<RepairLineStockDetail> GetRepairLineStockDetails(long orgId);
        bool SaveRepairLineStockIn(List<RepairLineStockDetailDTO> repairLineStockDetailDTO, long userId, long orgId);
        bool SaveRepairLineStockInByQCLine(long transferId, string status, long orgId, long userId);
        bool SaveRepairLineStockOut(List<RepairLineStockDetailDTO> repairLineStockDetailDTO, long userId, long orgId, string flag);
    }
}
