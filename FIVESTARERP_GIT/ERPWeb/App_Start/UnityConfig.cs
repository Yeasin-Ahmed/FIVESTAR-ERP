using ERPBLL.Configuration;
using ERPBLL.Configuration.Interface;
using ERPBLL.ControlPanel;
using ERPBLL.ControlPanel.Interface;
using ERPBLL.Inventory;
using ERPBLL.Inventory.Interface;
using ERPBLL.Production;
using ERPBLL.Production.Interface;
using ERPBLL.Report;
using ERPBLL.Report.Interface;
using ERPDAL.ConfigurationDAL;
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
            #region Inventory
            container.RegisterType<IWarehouseStockDetailBusiness, WarehouseStockDetailBusiness>();
            container.RegisterType<IWarehouseStockInfoBusiness, WarehouseStockInfoBusiness>();
            container.RegisterType<IItemBusiness, ItemBusiness>();
            container.RegisterType<IUnitBusiness, UnitBusiness>();
            container.RegisterType<IItemTypeBusiness, ItemTypeBusiness>();
            container.RegisterType<IWarehouseBusiness, WarehouseBusiness>();
            container.RegisterType<IRepairStockInfoBusiness, RepairStockInfoBusiness>();
            container.RegisterType<IRepairStockDetailBusiness, RepairStockDetailBusiness>();
            container.RegisterType<IItemPreparationInfoBusiness, ItemPreparationInfoBusiness>();
            container.RegisterType<IItemPreparationDetailBusiness, ItemPreparationDetailBusiness>();
            container.RegisterType<IInventoryUnitOfWork, InventoryUnitOfWork>(); // database 
            #endregion

            // Production Database
            #region Production
            container.RegisterType<IProductionLineBusiness, ProductionLineBusiness>();
            container.RegisterType<IRequsitionDetailBusiness, RequsitionDetailBusiness>();
            container.RegisterType<IRequsitionInfoBusiness, RequsitionInfoBusiness>();
            container.RegisterType<IProductionStockDetailBusiness, ProductionStockDetailBusiness>();
            container.RegisterType<IProductionStockInfoBusiness, ProductionStockInfoBusiness>();
            container.RegisterType<IItemReturnInfoBusiness, ItemReturnInfoBusiness>();
            container.RegisterType<IItemReturnDetailBusiness, ItemReturnDetailBusiness>();
            container.RegisterType<IDescriptionBusiness, DescriptionBusiness>();
            container.RegisterType<IFinishGoodsInfoBusiness, FinishGoodsInfoBusiness>();
            container.RegisterType<IFinishGoodsRowMaterialBusiness, FinishGoodsRowMaterialBusiness>();
            container.RegisterType<IFinishGoodsStockDetailBusiness, FinishGoodsStockDetailBusiness>();
            container.RegisterType<IFinishGoodsStockInfoBusiness, FinishGoodsStockInfoBusiness>();
            container.RegisterType<IFinishGoodsStockDetailBusiness, FinishGoodsStockDetailBusiness>();
            container.RegisterType<IFinishGoodsSendToWarehouseInfoBusiness, FinishGoodsSendToWarehouseInfoBusiness>();
            container.RegisterType<IFinishGoodsSendToWarehouseDetailBusiness, FinishGoodsSendToWarehouseDetailBusiness>();
            container.RegisterType<IProductionUnitOfWork, ProductionUnitOfWork>();
            #endregion

            // ControlPanel Database
            #region ControlPanel
            container.RegisterType<ISubMenuBusiness, SubMenuBusiness>();
            container.RegisterType<IManiMenuBusiness, ManiMenuBusiness>();
            container.RegisterType<IAppUserBusiness, AppUserBusiness>();
            container.RegisterType<IRoleBusiness, RoleBusiness>();
            container.RegisterType<IBranchBusiness, BranchBusiness>();
            container.RegisterType<IModuleBusiness, ModuleBusiness>();
            container.RegisterType<IOrganizationBusiness, OrganizationBusiness>();
            container.RegisterType<IModuleBusiness, ModuleBusiness>();
            container.RegisterType<IOrganizationAuthBusiness, OrganizationAuthBusiness>();
            container.RegisterType<IUserAuthorizationBusiness, UserAuthorizationBusiness>();
            container.RegisterType<IRoleAuthorizationBusiness, RoleAuthorizationBusiness>();
            container.RegisterType<IControlPanelUnitOfWork, ControlPanelUnitOfWork>();
            #endregion

            // Configuration Database
            #region Configuration
            container.RegisterType<ICustomerServiceBusiness, CustomerServiceBusiness>();
            container.RegisterType<ITechnicalServiceBusiness, TechnicalServiceBusiness>();
            container.RegisterType<ICustomerBusiness, CustomerBusiness>();
            container.RegisterType<IMobilePartBusiness, MobilePartBusiness>();
            container.RegisterType<IClientProblemBusiness, ClientProblemBusiness>();
            container.RegisterType<IAccessoriesBusiness, AccessoriesBusiness>();
            container.RegisterType<IConfigurationUnitOfWork, ConfigurationUnitOfWork>();
            #endregion


            #region Report

            container.RegisterType<IProductionReportBusiness, ProductionReportBusiness>();

            #endregion

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}