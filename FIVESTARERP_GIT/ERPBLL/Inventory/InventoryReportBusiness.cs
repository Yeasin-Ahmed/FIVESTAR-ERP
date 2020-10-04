using ERPBLL.Inventory.Interface;
using ERPBO.Inventory.ReportModels;
using ERPDAL.InventoryDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Inventory
{
    public class InventoryReportBusiness : IInventoryReportBusiness
    {
        private readonly IInventoryUnitOfWork _inventoryDb;
        public InventoryReportBusiness(IInventoryUnitOfWork inventoryDb)
        {
            this._inventoryDb = inventoryDb;
        }
        public IEnumerable<DailyStock> GetDailyItemStocks(long orgId)
        {
            return this._inventoryDb.Db.Database.SqlQuery<DailyStock>(string.Format(@"Exec spGetDailyStock {0}",orgId)).ToList();
        }
        public IEnumerable<DailyStock> GetModelWiseDailyItemStocks(long orgId, long modelId)
        {
            return this._inventoryDb.Db.Database.SqlQuery<DailyStock>(string.Format(@"Exec spGetModelWiseDailyItemStock {0} ,{1}", orgId,modelId)).ToList();
        }
    }
}
