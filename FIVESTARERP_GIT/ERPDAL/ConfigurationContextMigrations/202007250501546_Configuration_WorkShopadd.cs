namespace ERPDAL.ConfigurationContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Configuration_WorkShopadd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblWorkShop",
                c => new
                    {
                        WorkShopId = c.Long(nullable: false, identity: true),
                        WorkShopName = c.String(),
                        Remarks = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        BranchId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.WorkShopId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblWorkShop");
        }
    }
}
