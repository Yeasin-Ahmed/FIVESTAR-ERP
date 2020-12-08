using ERPBO.Configuration.DomainModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDAL.ConfigurationDAL
{
   public class ConfigurationDbContext:DbContext
    {
        public ConfigurationDbContext() : base("Configuration")
        {

        }
        public DbSet<StockTransferInfoModelToModel> tblStockTransferInfoModelToModel { get; set; }
        public DbSet<StockTransferDetailModelToModel> tblStockTransferDetailModelToModel { get; set; }
        public DbSet<MissingStock> tblMissingStock { get; set; }
        public DbSet<HandSetStock> tblHandSetStock { get; set; }
        public DbSet<Accessories> tblAccessories { get; set; }
        public DbSet<ClientProblem> tblClientProblems { get; set; }
        public DbSet<MobilePart> tblMobileParts { get; set; }
        public DbSet<Customer> tblCustomers { get; set; }
        public DbSet<TechnicalServiceEng> tblTechnicalServiceEngs { get; set; }
        public DbSet<CustomerService> tblCustomerServices { get; set; }
        public DbSet<ServiceWarehouse> tblServicesWarehouses { get; set; }
        public DbSet<MobilePartStockInfo> tblMobilePartStockInfo { get; set; }
        public DbSet<MobilePartStockDetail> tblMobilePartStockDetails { get; set; }
        public DbSet<Branch> tblBranches { get; set; }
        public DbSet<TransferInfo> tblTransferInfo { get; set; }
        public DbSet<TransferDetail> tblTransferDetails { get; set; }
        public DbSet<Fault> tblFault { get; set; }
        public DbSet<Service> tblServices { get; set; }
        public DbSet<Repair> tblRepair { get; set; }
        public DbSet<WorkShop> tblWorkShop { get; set; }
        public DbSet<FaultyStockDetails> tblFaultyStockDetails { get; set; }
        public DbSet<FaultyStockInfo> tblFaultyStockInfo { get; set; }
    }
}
