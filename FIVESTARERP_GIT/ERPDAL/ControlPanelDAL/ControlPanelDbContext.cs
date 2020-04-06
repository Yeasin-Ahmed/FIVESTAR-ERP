using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPBO.ControlPanel.DomainModels;

namespace ERPDAL.ControlPanelDAL
{
    public class ControlPanelDbContext : DbContext
    {
        public ControlPanelDbContext():base("ControlPanel")
        {

        }
        //tables
        public DbSet<Organization> tblOrganizations { get; set; }
        public DbSet<Branch> tblBranch { get; set; }
        public DbSet<AppUser> tblApplicationUsers { get; set; }
        public DbSet<Role> tblRoles { get; set; }
        public DbSet<Module> tblModules { get; set; }
    }
}
