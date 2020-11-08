namespace ERPDAL.SalesAndDistributionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SalesAndDistribution_TblASM : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblASM",
                c => new
                    {
                        ASMID = c.Long(nullable: false, identity: true),
                        RSMID = c.Long(nullable: false),
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
                        Remarks = c.String(maxLength: 150),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ASMID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblASM");
        }
    }
}
