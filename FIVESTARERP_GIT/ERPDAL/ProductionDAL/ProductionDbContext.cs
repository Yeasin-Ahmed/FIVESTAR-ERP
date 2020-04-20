
using System.Data.Entity;
using ERPBO.Production.DomainModels;

namespace ERPDAL.ProductionDAL
{
    public class ProductionDbContext : DbContext
    {
        public ProductionDbContext():base("Production")
        {

        }
        public DbSet<RequsitionInfo> tblRequsitionInfo { get; set; }
        public DbSet<RequsitionDetail> tblRequsitionDetails { get; set; }
        public DbSet<ProductionLine> tblProductionLines { get; set; }
        public DbSet<ProductionStockInfo> tblProductionStockInfo { get; set; }
        public DbSet<ProductionStockDetail> tblProductionStockDetail { get; set; }
        public DbSet<ItemReturnInfo> tblItemReturnInfo { get; set; }
        public DbSet<ItemReturnDetail> tblItemReturnDetail { get; set; }
        public DbSet<FinishGoodsInfo> tblFinishGoodsInfo { get; set; }
        public DbSet<FinishGoodsRowMaterial> tblFinishGoodsRowMaterial { get; set; }
        public DbSet<FinishGoodsStockInfo> tblFinishGoodsStockInfo { get; set; }
        public DbSet<FinishGoodsStockDetail> tblFinishGoodsStockDetail { get; set; }
        public DbSet<FinishGoodsSendToWarehouseInfo> tblFinishGoodsSendToWarehouseInfo { get; set; }
        public DbSet<FinishGoodsSendToWarehouseDetail> tblFinishGoodsSendToWarehouseDetail { get; set; }
    }
}
