using ERPBO.Inventory.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Inventory.Interface
{
    public interface IRepairStockInfoBusiness
    {
        IEnumerable<RepairStockInfo> GetRepairStockInfos(long orgId);
        RepairStockInfo GetRepairStockInfoById(long orgId, long stockInfoId);
    }
}
