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
        public DbSet<Description> tblDescriptions { get; set; }
        public DbSet<Color> tblColors { get; set; }
        public DbSet<ModelColors> tblModelColors { get; set; }
        public DbSet<RSM> tblRMS { get; set; }
        public DbSet<Division> tblDivision { get; set; }
        public DbSet<District> tblDistrict { get; set; }
        public DbSet<Zone> tblZone { get; set; }
        public DbSet<ASM> tblAZM { get; set; }
        public DbSet<TSE> tblTSE { get; set; }
        public DbSet<SalesRepresentative> tblSalesRepresentatives { get; set; }
        public DbSet<StoreMasterStock> tblStoreMasterStock { get; set; }
    }
}
