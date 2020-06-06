namespace ERPDAL.ConfigurationContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Configuration_DbtblServicesWarehouse : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblServiceWarehouses",
                c => new
                    {
                        SWarehouseId = c.Long(nullable: false, identity: true),
                        ServicesWarehouseName = c.String(),
                        Remarks = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.SWarehouseId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblServiceWarehouses");
        }
    }
}
