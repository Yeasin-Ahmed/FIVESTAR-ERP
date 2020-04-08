namespace ERPDAL.ControlPanelContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ControlPanelDbtblsubmenu : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblSubMenus",
                c => new
                    {
                        SubMenuId = c.Long(nullable: false, identity: true),
                        SubMenuName = c.String(maxLength: 100),
                        ControllerName = c.String(maxLength: 150),
                        ActionName = c.String(maxLength: 150),
                        IconClass = c.String(maxLength: 100),
                        IsViewable = c.Boolean(nullable: false),
                        IsActAsParent = c.Boolean(nullable: false),
                        ParentSubMenuId = c.Long(),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        MMId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.SubMenuId)
                .ForeignKey("dbo.tblMainMenus", t => t.MMId, cascadeDelete: true)
                .Index(t => t.MMId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblSubMenus", "MMId", "dbo.tblMainMenus");
            DropIndex("dbo.tblSubMenus", new[] { "MMId" });
            DropTable("dbo.tblSubMenus");
        }
    }
}
