using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IRepairItemStockDetailBusiness
    {
        IEnumerable<RepairItemStockDetail> GetQCItemStockDetails(long orgId);
        bool SaveRepairItemStockOut(List<RepairItemStockDetailDTO> items, long userId, long orgId);
        bool SaveRepairItemStockIn(List<RepairItemStockDetailDTO> items, long userId, long orgId);
        Task<bool> SaveRepairItemStockOutAsync(List<RepairItemStockDetailDTO> items, long userId, long orgId);
    }
}
