namespace ERPDAL.FrontDeskContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrontDesk_InvoiceInfoDetails : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InvoiceDetails",
                c => new
                    {
                        InvoiceDetailId = c.Long(nullable: false, identity: true),
                        PartsId = c.Long(nullable: false),
                        PartsName = c.Long(nullable: false),
                        Quantity = c.Int(nullable: false),
                        CostPrice = c.Double(nullable: false),
                        SellPrice = c.Double(nullable: false),
                        Discount = c.Double(nullable: false),
                        Total = c.Double(nullable: false),
                        Remarks = c.String(),
                        BranchId = c.Long(),
                        OrganizationId = c.Long(),
                        EUserId = c.Long(),
                        UpUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpdateDate = c.DateTime(),
                        InvoiceInfoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.InvoiceDetailId)
                .ForeignKey("dbo.tblInvoiceInfo", t => t.InvoiceInfoId, cascadeDelete: true)
                .Index(t => t.InvoiceInfoId);
            
            CreateTable(
                "dbo.tblInvoiceInfo",
                c => new
                    {
                        InvoiceInfoId = c.Long(nullable: false, identity: true),
                        InvoiceCode = c.String(),
                        JobOrderId = c.Long(nullable: false),
                        JobOrderCode = c.String(),
                        CustomerName = c.String(),
                        CustomerPhone = c.String(),
                        TotalSPAmount = c.Double(nullable: false),
                        LabourCharge = c.Double(nullable: false),
                        VAT = c.Double(nullable: false),
                        Tax = c.Double(nullable: false),
                        Discount = c.Double(nullable: false),
                        NetAmount = c.Double(nullable: false),
                        Remarks = c.String(),
                        BranchId = c.Long(),
                        OrganizationId = c.Long(),
                        EUserId = c.Long(),
                        UpUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.InvoiceInfoId);
            
            AddColumn("dbo.tblJobOrders", "InvoiceInfoId", c => c.Long(nullable: false));
            AddColumn("dbo.tblJobOrders", "InvoiceCode", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InvoiceDetails", "InvoiceInfoId", "dbo.tblInvoiceInfo");
            DropIndex("dbo.InvoiceDetails", new[] { "InvoiceInfoId" });
            DropColumn("dbo.tblJobOrders", "InvoiceCode");
            DropColumn("dbo.tblJobOrders", "InvoiceInfoId");
            DropTable("dbo.tblInvoiceInfo");
            DropTable("dbo.InvoiceDetails");
        }
    }
}
