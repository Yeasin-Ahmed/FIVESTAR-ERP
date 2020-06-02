namespace ERPDAL.ConfigurationContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Configuration_DbtblCS : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblCustomerServices",
                c => new
                    {
                        CsId = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Address = c.String(),
                        PhoneNumber = c.String(),
                        Remarks = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CsId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblCustomerServices");
        }
    }
}
