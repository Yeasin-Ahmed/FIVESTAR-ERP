using ERPBO.Inventory.ReportModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Inventory.Interface
{
    public interface IInventoryReportBusiness
    {
        IEnumerable<DailyStock> GetDailyItemStocks(long orgId);
        IEnumerable<DailyStock> GetModelWiseDailyItemStocks(long orgId, long modelId);
    }
}
