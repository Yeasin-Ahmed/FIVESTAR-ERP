namespace ERPDAL.ControlPanelContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ControlPanel_RoleAuth : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblRoleAuthorization",
                c => new
                    {
                        TaskId = c.Long(nullable: false, identity: true),
                        RoleId = c.Long(nullable: false),
                        ModuleId = c.Long(nullable: false),
                        MainmenuId = c.Long(nullable: false),
                        SubmenuId = c.Long(nullable: false),
                        ParentSubmenuId = c.Long(nullable: false),
                        Add = c.Boolean(nullable: false),
                        Edit = c.Boolean(nullable: false),
                        Detail = c.Boolean(nullable: false),
                        Delete = c.Boolean(nullable: false),
                        Approval = c.Boolean(nullable: false),
                        Report = c.Boolean(nullable: false),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.TaskId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblRoleAuthorization");
        }
    }
}
