namespace ERPDAL.SalesAndDistributionContextMigrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ERPDAL.SalesAndDistributionDAL.SalesAndDistributionDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"SalesAndDistributionContextMigrations";
        }

        protected override void Seed(ERPDAL.SalesAndDistributionDAL.SalesAndDistributionDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
