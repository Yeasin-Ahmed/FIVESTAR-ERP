using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPBO.Inventory.DomainModels;

namespace ERPDAL.InventoryDAL
{
    public class DescriptionRepository : InventoryBaseRepository<Description>
    {
        public DescriptionRepository(IInventoryUnitOfWork inventoryUnitOfWork) : base(inventoryUnitOfWork) { }
    }
    public class WarehouseRepository:InventoryBaseRepository<Warehouse>
    {
        public WarehouseRepository(IInventoryUnitOfWork inventoryUnitOfWork) : base(inventoryUnitOfWork) { }
    }
    public class ItemTypeRepository: InventoryBaseRepository<ItemType>
    {
        public ItemTypeRepository(IInventoryUnitOfWork inventoryUnitOfWork) : base(inventoryUnitOfWork) { }
    }
    public class UnitRepository : InventoryBaseRepository<Unit>
    {
        public UnitRepository(IInventoryUnitOfWork inventoryUnitOfWork) : base(inventoryUnitOfWork) { }
    }
    public class ItemRepository : InventoryBaseRepository<Item>
    {
        public ItemRepository(IInventoryUnitOfWork inventoryUnitOfWork) : base(inventoryUnitOfWork) { }
    }
    public class WarehouseStockInfoRepository : InventoryBaseRepository<WarehouseStockInfo>
    {
        public WarehouseStockInfoRepository(IInventoryUnitOfWork inventoryUnitOfWork) : base(inventoryUnitOfWork) { }
    }
    public class WarehouseStockDetailRepository : InventoryBaseRepository<WarehouseStockDetail>
    {
        public WarehouseStockDetailRepository(IInventoryUnitOfWork inventoryUnitOfWork) : base(inventoryUnitOfWork) { }
    }
    public class WarehouseFaultyInfoRepository : InventoryBaseRepository<WarehouseFaultyStockInfo>
    {
        public WarehouseFaultyInfoRepository(IInventoryUnitOfWork inventoryUnitOfWork) : base(inventoryUnitOfWork) { }
    }
    public class WarehouseFaultyStockDetailRepository : InventoryBaseRepository<WarehouseFaultyStockDetail>
    {
        public WarehouseFaultyStockDetailRepository(IInventoryUnitOfWork inventoryUnitOfWork) : base(inventoryUnitOfWork) { }
    }
    public class ItemPreparationInfoRepository : InventoryBaseRepository<ItemPreparationInfo>
    {
        public ItemPreparationInfoRepository(IInventoryUnitOfWork inventoryUnitOfWork) : base(inventoryUnitOfWork) { }
    }
    public class ItemPreparationDetailRepository : InventoryBaseRepository<ItemPreparationDetail>
    {
        public ItemPreparationDetailRepository(IInventoryUnitOfWork inventoryUnitOfWork) : base(inventoryUnitOfWork) { }
    }
    public class SupplierRepository :InventoryBaseRepository<Supplier>
    {
        public SupplierRepository(IInventoryUnitOfWork inventoryUnitOfWork) : base(inventoryUnitOfWork) { }
    }
    public class IQCRepository : InventoryBaseRepository<IQC>
    {
        public IQCRepository(IInventoryUnitOfWork inventoryUnitOfWork) : base (inventoryUnitOfWork) { }
    }
    public class IQCItemReqInfoListRepository : InventoryBaseRepository<IQCItemReqInfoList>
    {
        public IQCItemReqInfoListRepository(IInventoryUnitOfWork inventoryUnitOfWork) : base (inventoryUnitOfWork)
        {

        }
    }
    public class IQCItemReqDetailListRepository : InventoryBaseRepository<IQCItemReqDetailList>
    {
        public IQCItemReqDetailListRepository(IInventoryUnitOfWork inventoryUnitOfWork) : base(inventoryUnitOfWork)
        {

        }
    }
    public class IQCStockInfoRepository : InventoryBaseRepository<IQCStockInfo>
    {
        public IQCStockInfoRepository(IInventoryUnitOfWork inventoryUnitOfWork) : base(inventoryUnitOfWork)
        {

        }
    }
    public class IQCStockDetailRepository : InventoryBaseRepository<IQCStockDetail>
    {
        public IQCStockDetailRepository(IInventoryUnitOfWork inventoryUnitOfWork) : base(inventoryUnitOfWork)
        {

        }
    }
    public class CategoryRepository : InventoryBaseRepository<Category>
    {
        public CategoryRepository(IInventoryUnitOfWork inventoryUnitOfWork) :
            base(inventoryUnitOfWork)
        {

        }
    }
    public class BrandRepository : InventoryBaseRepository<Brand>
    {
        public BrandRepository(IInventoryUnitOfWork inventoryUnitOfWork) :
            base(inventoryUnitOfWork)
        {

        }
    }
    public class BrandCategoriesRepository : InventoryBaseRepository<BrandCategories>
    {
        public BrandCategoriesRepository(IInventoryUnitOfWork inventoryUnitOfWork) :
            base(inventoryUnitOfWork)
        {

        }
    }
    public class ColorRepository : InventoryBaseRepository<Color>
    {
        public ColorRepository(IInventoryUnitOfWork inventoryUnitOfWork) :
            base(inventoryUnitOfWork)
        {

        }
    }
    public class ModelColorsRepository : InventoryBaseRepository<ModelColors>
    {
        public ModelColorsRepository(IInventoryUnitOfWork inventoryUnitOfWork) :
            base(inventoryUnitOfWork)
        {

        }
    }
    public class HandSetStockRepository : InventoryBaseRepository<HandSetStock>
    {
        public HandSetStockRepository(IInventoryUnitOfWork inventoryUnitOfWork) :
            base(inventoryUnitOfWork)
        {

        }
    }
    public class STransferInfoMToMRepository : InventoryBaseRepository<StockTransferInfoMToM>
    {
        public STransferInfoMToMRepository(IInventoryUnitOfWork inventoryUnitOfWork) : base(inventoryUnitOfWork) { }
    }
    public class STransferDetailsMToMRepository : InventoryBaseRepository<StockTransferDetailsMToM>
    {
        public STransferDetailsMToMRepository(IInventoryUnitOfWork inventoryUnitOfWork) : base(inventoryUnitOfWork) { }
    }
}
