using ERPBO.Production.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDAL.ProductionDAL
{
    public class RequsitionInfoRepository : ProductionBaseRepository<RequsitionInfo>
    {
        public RequsitionInfoRepository(IProductionUnitOfWork productionUnitOfWork) : base(productionUnitOfWork) { }
    }
    public class RequsitionDetailRepository : ProductionBaseRepository<RequsitionDetail>
    {
        public RequsitionDetailRepository(IProductionUnitOfWork productionUnitOfWork) : base(productionUnitOfWork) { }
    }
    public class ProductionLineRepository : ProductionBaseRepository<ProductionLine>
    {
        public ProductionLineRepository(IProductionUnitOfWork productionUnitOfWork) : base(productionUnitOfWork) { }
    }
    public class ProductionStockInfoRepository : ProductionBaseRepository<ProductionStockInfo>
    {
        public ProductionStockInfoRepository(IProductionUnitOfWork productionUnitOfWork) : base(productionUnitOfWork) { }
    }
    public class ProductionStockDetailRepository : ProductionBaseRepository<ProductionStockDetail>
    {
        public ProductionStockDetailRepository(IProductionUnitOfWork productionUnitOfWork) : base(productionUnitOfWork) { }
    }
    public class ItemReturnInfoRepository : ProductionBaseRepository<ItemReturnInfo>
    {
        public ItemReturnInfoRepository(IProductionUnitOfWork productionUnitOfWork) : base(productionUnitOfWork) { }
    }
    public class ItemReturnDetailRepository : ProductionBaseRepository<ItemReturnDetail>
    {
        public ItemReturnDetailRepository(IProductionUnitOfWork productionUnitOfWork) : base(productionUnitOfWork) { }
    }
    public class FinishGoodsInfoRepository : ProductionBaseRepository<FinishGoodsInfo>
    {
        public FinishGoodsInfoRepository(IProductionUnitOfWork productionUnitOfWork) : base(productionUnitOfWork) { }
    }
    public class FinishGoodsRowMaterialRepository : ProductionBaseRepository<FinishGoodsRowMaterial>
    {
        public FinishGoodsRowMaterialRepository(IProductionUnitOfWork productionUnitOfWork) : base(productionUnitOfWork) { }
    }
    public class FinishGoodsStockInfoRepository : ProductionBaseRepository<FinishGoodsStockInfo>
    {
        public FinishGoodsStockInfoRepository(IProductionUnitOfWork productionUnitOfWork) : base(productionUnitOfWork) { }
    }
    public class FinishGoodsStockDetailRepository : ProductionBaseRepository<FinishGoodsStockDetail>
    {
        public FinishGoodsStockDetailRepository(IProductionUnitOfWork productionUnitOfWork) : base(productionUnitOfWork) { }
    }
    public class FinishGoodsSendToWarehouseInfoRepository : ProductionBaseRepository<FinishGoodsSendToWarehouseInfo>
    {
        public FinishGoodsSendToWarehouseInfoRepository(IProductionUnitOfWork productionUnitOfWork) : base(productionUnitOfWork) { }
    }
    public class FinishGoodsSendToWarehouseDetailRepository : ProductionBaseRepository<FinishGoodsSendToWarehouseDetail>
    {
        public FinishGoodsSendToWarehouseDetailRepository(IProductionUnitOfWork productionUnitOfWork) : base(productionUnitOfWork) { }
    }
    public class AssemblyLineRepository : ProductionBaseRepository<AssemblyLine>
    {
        public AssemblyLineRepository(IProductionUnitOfWork productionUnitOfWork) : base(productionUnitOfWork) { }
    }
    public class TransferStockToAssemblyInfoRepository : ProductionBaseRepository<TransferStockToAssemblyInfo>
    {
        public TransferStockToAssemblyInfoRepository(IProductionUnitOfWork productionUnitOfWork) : base(productionUnitOfWork) { }
    }
    public class TransferStockToAssemblyDetailRepository : ProductionBaseRepository<TransferStockToAssemblyDetail>
    {
        public TransferStockToAssemblyDetailRepository(IProductionUnitOfWork productionUnitOfWork) : base(productionUnitOfWork) { }
    }
    public class AssemblyLineStockInfoRepository : ProductionBaseRepository<AssemblyLineStockInfo>
    {
        public AssemblyLineStockInfoRepository(IProductionUnitOfWork productionUnitOfWork) : base(productionUnitOfWork) { }
    }
    public class AssemblyLineStockDetailRepository : ProductionBaseRepository<AssemblyLineStockDetail>
    {
        public AssemblyLineStockDetailRepository(IProductionUnitOfWork productionUnitOfWork) : base(productionUnitOfWork) { }
    }
    public class QualityControlRepository : ProductionBaseRepository<QualityControl>
    {
        public QualityControlRepository(IProductionUnitOfWork productionUnitOfWork) : base(productionUnitOfWork) { }
    }
    public class TransferStockToQCInfoRepository : ProductionBaseRepository<TransferStockToQCInfo>
    {
        public TransferStockToQCInfoRepository(IProductionUnitOfWork productionUnitOfWork) : base(productionUnitOfWork) { }
    }
    public class TransferStockToQCDetailRepository : ProductionBaseRepository<TransferStockToQCDetail>
    {
        public TransferStockToQCDetailRepository(IProductionUnitOfWork productionUnitOfWork) : base(productionUnitOfWork) { }
    }
    public class QualityControlLineStockInfoRepository : ProductionBaseRepository<QualityControlLineStockInfo>
    {
        public QualityControlLineStockInfoRepository(IProductionUnitOfWork productionUnitOfWork) : base(productionUnitOfWork) { }
    }
    public class QualityControlLineStockDetailRepository : ProductionBaseRepository<QualityControlLineStockDetail>
    {
        public QualityControlLineStockDetailRepository(IProductionUnitOfWork productionUnitOfWork) : base(productionUnitOfWork) { }
    }
    public class RepairLineRepository : ProductionBaseRepository<RepairLine>
    {
        public RepairLineRepository(IProductionUnitOfWork productionUnitOfWork) : base(productionUnitOfWork) { }
    }
    public class PackagingLineRepository : ProductionBaseRepository<PackagingLine>
    {
        public PackagingLineRepository(IProductionUnitOfWork productionUnitOfWork) : base(productionUnitOfWork) { }
    }
    public class TransferFromQCInfoRepository : ProductionBaseRepository<TransferFromQCInfo>
    {
        public TransferFromQCInfoRepository(IProductionUnitOfWork productionUnitOfWork) : base(productionUnitOfWork) { }
    }
    public class TransferFromQCDetailRepository : ProductionBaseRepository<TransferFromQCDetail>
    {
        public TransferFromQCDetailRepository(IProductionUnitOfWork productionUnitOfWork) : base(productionUnitOfWork) { }
    }
    public class PackagingLineStockInfoRepository : ProductionBaseRepository<PackagingLineStockInfo>
    {
        public PackagingLineStockInfoRepository(IProductionUnitOfWork productionUnitOfWork) : base(productionUnitOfWork) { }
    }
    public class PackagingLineStockDetailRepository : ProductionBaseRepository<PackagingLineStockDetail>
    {
        public PackagingLineStockDetailRepository(IProductionUnitOfWork productionUnitOfWork) : base(productionUnitOfWork) { }
    }
    public class TransferStockToPackagingLine2InfoRepository : ProductionBaseRepository<TransferStockToPackagingLine2Info>
    {
        public TransferStockToPackagingLine2InfoRepository(IProductionUnitOfWork productionUnitOfWork) : base(productionUnitOfWork) { }
    }
    public class TransferStockToPackagingLine2DetailRepository : ProductionBaseRepository<TransferStockToPackagingLine2Detail>
    {
        public TransferStockToPackagingLine2DetailRepository(IProductionUnitOfWork productionUnitOfWork) : base(productionUnitOfWork) { }
    }
    public class RepairLineStockInfoRepository : ProductionBaseRepository<RepairLineStockInfo>
    {
        public RepairLineStockInfoRepository(IProductionUnitOfWork productionUnitOfWork) : base(productionUnitOfWork) { }
    }
    public class RepairLineStockDetailRepository : ProductionBaseRepository<RepairLineStockDetail>
    {
        public RepairLineStockDetailRepository(IProductionUnitOfWork productionUnitOfWork) : base(productionUnitOfWork) { }
    }
    public class TransferRepairItemToQcInfoRepository : ProductionBaseRepository<TransferRepairItemToQcInfo>
    {
        public TransferRepairItemToQcInfoRepository(IProductionUnitOfWork productionUnitOfWork) : base(productionUnitOfWork) { }
    }
    public class TransferRepairItemToQcDetailRepository : ProductionBaseRepository<TransferRepairItemToQcDetail>
    {
        public TransferRepairItemToQcDetailRepository(IProductionUnitOfWork productionUnitOfWork) : base(productionUnitOfWork) { }
    }
}
