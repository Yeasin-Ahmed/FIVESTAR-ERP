namespace ERPDAL.AccountsContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Accounts_SupplierCustomers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblCustomers",
                c => new
                    {
                        CustomerId = c.Long(nullable: false, identity: true),
                        CustomerName = c.String(),
                        Email = c.String(),
                        Address = c.String(),
                        PhoneNumber = c.String(),
                        MobileNumber = c.String(),
                        Remarks = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CustomerId);
            
            CreateTable(
                "dbo.tblSuppliers",
                c => new
                    {
                        SupplierId = c.Long(nullable: false, identity: true),
                        SupplierName = c.String(),
                        Email = c.String(),
                        Address = c.String(),
                        PhoneNumber = c.String(),
                        MobileNumber = c.String(),
                        Remarks = c.String(),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.SupplierId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblSuppliers");
            DropTable("dbo.tblCustomers");
        }
    }
}
