namespace ERPDAL.ConfigurationContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Configuration_add_prop_in_TransferInfo_Detail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblTransferDetails", "DescriptionId", c => c.Long());
            AddColumn("dbo.tblTransferInfo", "DescriptionId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblTransferInfo", "DescriptionId");
            DropColumn("dbo.tblTransferDetails", "DescriptionId");
        }
    }
}
