using ERPBLL.Inventory.Interface;
using ERPBO.Inventory.DomainModels;
using ERPDAL.InventoryDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Inventory
{
    public class RepairStockInfoBusiness : IRepairStockInfoBusiness
    {
        /// <summary>
        ///  BC Stands for          - Business Class
        ///  db Stands for          - Database
        ///  repo Stands for       - Repository
        /// </summary>
        private readonly IInventoryUnitOfWork _inventoryDb; // database
        private readonly RepairStockInfoRepository repairStockInfoRepository; // repo
        public RepairStockInfoBusiness(IInventoryUnitOfWork inventoryDb)
        {
            this._inventoryDb = inventoryDb;
            this.repairStockInfoRepository = new RepairStockInfoRepository(this._inventoryDb);
        }
        public RepairStockInfo GetRepairStockInfoById(long orgId, long stockInfoId)
        {
            return repairStockInfoRepository.GetAll(r => r.OrganizationId == orgId && r.RStockInfoId == stockInfoId).FirstOrDefault();
        }
        public IEnumerable<RepairStockInfo> GetRepairStockInfos(long orgId)
        {
            return repairStockInfoRepository.GetAll(r => r.OrganizationId == orgId).ToList();
        }
    }
}
