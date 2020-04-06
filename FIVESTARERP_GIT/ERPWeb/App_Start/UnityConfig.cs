using ERPBLL.ControlPanel;
using ERPBLL.ControlPanel.Interface;
using ERPBLL.Inventory;
using ERPBLL.Inventory.Interface;
using ERPBLL.Production;
using ERPBLL.Production.Interface;
using ERPDAL.ControlPanelDAL;
using ERPDAL.InventoryDAL;
using ERPDAL.ProductionDAL;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace ERPWeb
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            // e.g. container.RegisterType<ITestService, TestService>();

            // Inventory Database
            container.RegisterType<IWarehouseStockDetailBusiness, WarehouseStockDetailBusiness>();
            container.RegisterType<IWarehouseStockInfoBusiness, WarehouseStockInfoBusiness>();
            container.RegisterType<IItemBusiness, ItemBusiness>();
            container.RegisterType<IUnitBusiness, UnitBusiness>();
            container.RegisterType<IItemTypeBusiness, ItemTypeBusiness>();
            container.RegisterType<IWarehouseBusiness, WarehouseBusiness>();
            container.RegisterType<IRepairStockInfoBusiness, RepairStockInfoBusiness>();
            container.RegisterType<IRepairStockDetailBusiness, RepairStockDetailBusiness>();
            container.RegisterType<IInventoryUnitOfWork, InventoryUnitOfWork>(); // database

            // Production Database
            container.RegisterType<IProductionLineBusiness, ProductionLineBusiness>();
            container.RegisterType<IRequsitionDetailBusiness, RequsitionDetailBusiness>();
            container.RegisterType<IRequsitionInfoBusiness, RequsitionInfoBusiness>();
            container.RegisterType<IProductionStockDetailBusiness, ProductionStockDetailBusiness>();
            container.RegisterType<IProductionStockInfoBusiness, ProductionStockInfoBusiness>();
            container.RegisterType<IItemReturnInfoBusiness, ItemReturnInfoBusiness>();
            container.RegisterType<IItemReturnDetailBusiness, ItemReturnDetailBusiness>();
            container.RegisterType<IDescriptionBusiness, DescriptionBusiness>();
            container.RegisterType<IProductionUnitOfWork, ProductionUnitOfWork>();

            // Control Panel Database
            container.RegisterType<IAppUserBusiness, AppUserBusiness>();
            container.RegisterType<IRoleBusiness, RoleBusiness>();
            container.RegisterType<IBranchBusiness, BranchBusiness>();
            container.RegisterType<IModuleBusiness, ModuleBusiness>();
            container.RegisterType<IOrganizationBusiness, OrganizationBusiness>();
            container.RegisterType<IControlPanelUnitOfWork, ControlPanelUnitOfWork>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}