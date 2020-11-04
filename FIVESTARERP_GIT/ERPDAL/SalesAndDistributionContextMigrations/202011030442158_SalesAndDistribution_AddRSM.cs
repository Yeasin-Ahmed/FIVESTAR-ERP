namespace ERPDAL.SalesAndDistributionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SalesAndDistribution_AddRSM : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblRSM",
                c => new
                    {
                        RSMID = c.Long(nullable: false, identity: true),
                        FullName = c.String(maxLength: 150),
                        DivisionId = c.Long(nullable: false),
                        DistrictId = c.Long(nullable: false),
                        IsAllowToLogIn = c.Boolean(nullable: false),
                        Email = c.String(maxLength: 150),
                        IsActive = c.Boolean(nullable: false),
                        BranchId = c.Long(nullable: false),
                        MobileNo = c.String(),
                        Address = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        UserId = c.Long(),
                        UserName = c.String(),
                        Password = c.String(),
                        ConfirmPassword = c.String(),
                        Remarks = c.String(maxLength: 150),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.RSMID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblRSM");
        }
    }
}
