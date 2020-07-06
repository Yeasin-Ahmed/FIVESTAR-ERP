namespace ERPDAL.ConfigurationContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Configuration_tblFaultServiceAdd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblFault",
                c => new
                    {
                        FaultId = c.Long(nullable: false, identity: true),
                        FaultName = c.String(),
                        FaultCode = c.String(),
                        Remarks = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.FaultId);
            
            CreateTable(
                "dbo.tblServices",
                c => new
                    {
                        ServiceId = c.Long(nullable: false, identity: true),
                        ServiceName = c.String(),
                        ServiceCode = c.String(),
                        Remarks = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ServiceId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblServices");
            DropTable("dbo.tblFault");
        }
    }
}
