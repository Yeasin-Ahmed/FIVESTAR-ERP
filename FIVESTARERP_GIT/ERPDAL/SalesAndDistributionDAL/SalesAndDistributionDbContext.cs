using ERPBO.SalesAndDistribution.DomainModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDAL.SalesAndDistributionDAL
{
    public class SalesAndDistributionDbContext : DbContext
    {
        public SalesAndDistributionDbContext() : base("SalesAndDistribution")
        {

        }
        public DbSet<Dealer> tblDealer { get; set; }
        public DbSet<BTRCApprovedIMEI> tblBTRCApprovedIMEI { get; set; }
        public DbSet<ItemStock> tblItemStock { get; set; }
        public DbSet<Category> tblCategory { get; set; }
        public DbSet<Brand> tblBrand { get; set; }
        public DbSet<BrandCategories> tblBrandCategories { get; set; }
    }
}
