using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPBO.Inventory.DomainModels;

namespace ERPDAL.InventoryDAL
{
    public class InventoryDbContext : DbContext
    {
        public InventoryDbContext():base("Inventory")
        {

        }
        public DbSet<Description> tblDescriptions { get; set; }
        public DbSet<Warehouse> tblWarehouses { get; set; }
        public DbSet<ItemType> tblItemTypes { get; set; }
        public DbSet<Unit> tblUnits { get; set; }
        public DbSet<Item> tblItems { get; set; }
        public DbSet<WarehouseStockInfo> tblWarehouseStockInfo { get; set; }
        public DbSet<WarehouseStockDetail> tblWarehouseStockDetails { get; set; }
        public DbSet<RepairStockInfo> tblRepairStockInfo { get; set; }
        public DbSet<RepairStockDetail> tblRepairStockDetails { get; set; }
        public DbSet<ItemPreparationInfo> tblItemPreparationInfo { get; set; }
        public DbSet<ItemPreparationDetail> tblItemPreparationDetail { get; set; }
        public DbSet<Supplier> tblSupplier { get; set; }
    }
}
