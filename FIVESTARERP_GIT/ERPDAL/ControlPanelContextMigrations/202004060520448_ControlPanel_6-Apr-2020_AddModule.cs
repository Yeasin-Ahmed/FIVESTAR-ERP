namespace ERPDAL.ControlPanelContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ControlPanel_6Apr2020_AddModule : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblModules",
                c => new
                    {
                        MId = c.Long(nullable: false, identity: true),
                        ModuleName = c.String(),
                        IconName = c.String(),
                        IconColor = c.String(),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.MId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblModules");
        }
    }
}
