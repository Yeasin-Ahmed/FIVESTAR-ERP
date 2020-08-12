
using System.Data.Entity;
using ERPBO.Production.DomainModels;

namespace ERPDAL.ProductionDAL
{
    public class ProductionDbContext : DbContext
    {
        public ProductionDbContext():base("Production")
        {

        }
        public DbSet<RequsitionInfo> tblRequsitionInfo { get; set; }
        public DbSet<RequsitionDetail> tblRequsitionDetails { get; set; }
        public DbSet<ProductionLine> tblProductionLines { get; set; }
        public DbSet<ProductionStockInfo> tblProductionStockInfo { get; set; }
        public DbSet<ProductionStockDetail> tblProductionStockDetail { get; set; }
        public DbSet<ItemReturnInfo> tblItemReturnInfo { get; set; }
        public DbSet<ItemReturnDetail> tblItemReturnDetail { get; set; }
        public DbSet<FinishGoodsInfo> tblFinishGoodsInfo { get; set; }
        public DbSet<FinishGoodsRowMaterial> tblFinishGoodsRowMaterial { get; set; }
        public DbSet<FinishGoodsStockInfo> tblFinishGoodsStockInfo { get; set; }
        public DbSet<FinishGoodsStockDetail> tblFinishGoodsStockDetail { get; set; }
        public DbSet<FinishGoodsSendToWarehouseInfo> tblFinishGoodsSendToWarehouseInfo { get; set; }
        public DbSet<FinishGoodsSendToWarehouseDetail> tblFinishGoodsSendToWarehouseDetail { get; set; }
        public DbSet<AssemblyLine> tblAssemblyLines { get; set; }
        public DbSet<TransferStockToAssemblyInfo> tblTransferStockToAssemblyInfo { get; set; }
        public DbSet<TransferStockToAssemblyDetail> tblTransferStockToAssemblyDetail { get; set; }
        public DbSet<AssemblyLineStockInfo> tblAssemblyLineStockInfo { get; set; }
        public DbSet<AssemblyLineStockDetail> tblAssemblyLineStockDetail { get; set; }
        public DbSet<QualityControl> tblQualityControl { get; set; }
        public DbSet<TransferStockToQCInfo> tblTransferStockToQCInfo { get; set; }
        public DbSet<TransferStockToQCDetail> tblTransferStockToQCDetail { get; set; }
        public DbSet<QualityControlLineStockInfo> tblQualityControlLineStockInfo { get; set; }
        public DbSet<QualityControlLineStockDetail> tblQualityControlLineStockDetail { get; set; }
        public DbSet<RepairLine> tblRepairLine { get; set; }
        public DbSet<PackagingLine> tblPackagingLine { get; set; }
        public DbSet<TransferFromQCInfo> tblTransferFromQCInfo { get; set; }
        public DbSet<TransferFromQCDetail> tblTransferFromQCDetail { get; set; }
        public DbSet<PackagingLineStockInfo> tblPackagingLineStockInfo { get; set; }
        public DbSet<PackagingLineStockDetail> tblPackagingLineStockDetail { get; set; }
        public DbSet<TransferStockToPackagingLine2Info> tblTransferStockToPackagingLine2Info { get; set; }
        public DbSet<TransferStockToPackagingLine2Detail> tblTransferStockToPackagingLine2Detail { get; set; }
        public DbSet<RepairLineStockInfo> tblRepairLineStockInfo { get; set; }
        public DbSet<RepairLineStockDetail> tblRepairLineStockDetail { get; set; }
        public DbSet<TransferRepairItemToQcInfo> tblTransferRepairItemToQcInfo { get; set; }
        public DbSet<TransferRepairItemToQcDetail> tblTransferRepairItemToQcDetail { get; set; }
        public DbSet<QCItemStockInfo> tblQCItemStockInfo { get; set; }
        public DbSet<QCItemStockDetail> tblQCItemStockDetail { get; set; }
        public DbSet<RepairItemStockInfo> tblRepairItemStockInfo { get; set; }
        public DbSet<RepairItemStockDetail> tblRepairItemStockDetail { get; set; }
        public DbSet<PackagingItemStockInfo> tblPackagingItemStockInfo { get; set; }
        public DbSet<PackagingItemStockDetail> tblPackagingItemStockDetail { get; set; }
        public DbSet<QRCodeTrace> tblQRCodeTrace { get; set; }
        public DbSet<FaultyItemStockInfo> tblFaultyItemStockInfo { get; set; }
        public DbSet<FaultyItemStockDetail> tblFaultyItemStockDetail { get; set; }
        public DbSet<RepairItem> tblRepairItem { get; set; }
        public DbSet<RepairItemParts> tblRepairItemParts { get; set; }
        public DbSet<RepairItemProblem> tblRepairItemProblem { get; set; }
        public DbSet<FaultyCase> tblFaultyCase { get; set; }
        public DbSet<RepairSectionFaultyItemTransferInfo> tblRepairSectionFaultyItemTransferInfo { get; set; }
        public DbSet<RepairSectionFaultyItemTransferDetail> tblRepairSectionFaultyItemTransferDetail { get; set; }
        public DbSet<RepairSectionRequisitionInfo> tblRepairSectionRequisitionInfo { get; set; }
        public DbSet<RepairSectionRequisitionDetail> tblRepairSectionRequisitionDetail { get; set; }
        public DbSet<ProductionFaultyStockInfo> tblProductionFaultyStockInfo { get; set; }
        public DbSet<ProductionFaultyStockDetail> tblProductionFaultyStockDetail { get; set; }
        public DbSet<ProductionAssembleStockInfo> tblProductionAssembleStockInfo { get; set; }
        public DbSet<ProductionAssembleStockDetail> tblProductionAssembleStockDetail { get; set; }
        public DbSet<QCPassTransferInformation> tblQCPassTransferInformation { get; set; }
        public DbSet<ProductionToPackagingStockTransferInfo> tblProductionToPackagingStockTransferInfo { get; set; }
        public DbSet<ProductionToPackagingStockTransferDetail> tblProductionToPackagingStockTransferDetail { get; set; }
        public DbSet<QRCodeProblem> tblQRCodeProblem { get; set; }
        public DbSet<QRCodeTransferToRepairInfo> tblQRCodeTransferToRepairInfo { get; set; }
        public DbSet<RequisitionItemInfo> tblRequisitionItemInfo { get; set; }
        public DbSet<RequisitionItemDetail> tblRequisitionItemDetail { get; set; }
        public DbSet<TempQRCodeTrace> tblTempQRCodeTrace { get; set; }
        public DbSet<MiniStockTransferInfo> tblMiniStockTransferInfo { get; set; }
        public DbSet<MiniStockTransferDetail> tblMiniStockTransferDetail { get; set; }
    }
}
