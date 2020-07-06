namespace ERPDAL.ConfigurationContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Configuration_Repair : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblRepair",
                c => new
                    {
                        RepairId = c.Long(nullable: false, identity: true),
                        RepairName = c.String(),
                        RepairCode = c.String(),
                        Remarks = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.RepairId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblRepair");
        }
    }
}
