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
        public DbSet<Accessories> tblAccessories { get; set; }
        public DbSet<ClientProblem> tblClientProblems { get; set; }
        public DbSet<MobilePart> tblMobileParts { get; set; }
    }
}
