using ERPBLL.Configuration;
using ERPBLL.Configuration.Interface;
using ERPBLL.ControlPanel;
using ERPBLL.ControlPanel.Interface;
using ERPBLL.FrontDesk;
using ERPBLL.FrontDesk.Interface;
using ERPBLL.Inventory;
using ERPBLL.Inventory.Interface;
using ERPBLL.Production;
using ERPBLL.Production.Interface;
using ERPBLL.Report;
using ERPBLL.Report.Interface;
using ERPDAL.ConfigurationDAL;
using ERPDAL.ControlPanelDAL;
using ERPDAL.FrontDeskDAL;
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
            container.RegisterType<ISupplierBusiness,SupplierBusiness>();
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
            container.RegisterType<IAssemblyLineBusiness, AssemblyLineBusiness>();
            container.RegisterType<ITransferStockToAssemblyInfoBusiness, TransferStockToAssemblyInfoBusiness>();
            container.RegisterType<ITransferStockToAssemblyDetailBusiness, TransferStockToAssemblyDetailBusiness>();
            container.RegisterType<IAssemblyLineStockInfoBusiness, AssemblyLineStockInfoBusiness>();
            container.RegisterType<IAssemblyLineStockDetailBusiness, AssemblyLineStockDetailBusiness>();
            container.RegisterType<IQualityControlBusiness, QualityControlBusiness>();
            container.RegisterType<ITransferStockToQCInfoBusiness, TransferStockToQCInfoBusiness>();
            container.RegisterType<ITransferStockToQCDetailBusiness, TransferStockToQCDetailBusiness>();
            container.RegisterType<IQCLineStockInfoBusiness, QCLineStockInfoBusiness>();
            container.RegisterType<IQCLineStockDetailBusiness, QCLineStockDetailBusiness>();
            container.RegisterType<IRepairLineBusiness, RepairLineBusiness>();
            container.RegisterType<IPackagingLineBusiness, PackagingLineBusiness>();
            container.RegisterType<ITransferFromQCInfoBusiness, TransferFromQCInfoBusiness>();
            container.RegisterType<ITransferFromQCDetailBusiness, TransferFromQCDetailBusiness>();
            container.RegisterType<IPackagingLineStockInfoBusiness, PackagingLineStockInfoBusiness>();
            container.RegisterType<IPackagingLineStockDetailBusiness, PackagingLineStockDetailBusiness>();
            container.RegisterType<ITransferStockToPackagingLine2InfoBusiness, TransferStockToPackagingLine2InfoBusiness>();
            container.RegisterType<ITransferStockToPackagingLine2DetailBusiness, TransferStockToPackagingLine2DetailBusiness>();
            container.RegisterType<IRepairLineStockInfoBusiness, RepairLineStockInfoBusiness>();
            container.RegisterType<IRepairLineStockDetailBusiness, RepairLineStockDetailBusiness>();
            container.RegisterType<ITransferRepairItemToQcInfoBusiness, TransferRepairItemToQcInfoBusiness>();
            container.RegisterType<ITransferRepairItemToQcDetailBusiness, TransferRepairItemToQcDetailBusiness>();
            container.RegisterType<IQCItemStockInfoBusiness, QCItemStockInfoBusiness>();
            container.RegisterType<IQCItemStockDetailBusiness, QCItemStockDetailBusiness>();
            container.RegisterType<IRepairItemStockInfoBusiness, RepairItemStockInfoBusiness>();
            container.RegisterType<IRepairItemStockDetailBusiness, RepairItemStockDetailBusiness>();
            container.RegisterType<IPackagingItemStockInfoBusiness, PackagingItemStockInfoBusiness>();
            container.RegisterType<IPackagingItemStockDetailBusiness, PackagingItemStockDetailBusiness>();
            container.RegisterType<IQRCodeTraceBusiness, QRCodeTraceBusiness>();
            container.RegisterType<IFaultyItemStockInfoBusiness, FaultyItemStockInfoBusiness>();
            container.RegisterType<IFaultyItemStockDetailBusiness, FaultyItemStockDetailBusiness>();
            container.RegisterType<IFaultyCaseBusiness, FaultyCaseBusiness>();
            container.RegisterType<IRepairItemBusiness, RepairItemBusiness>();
            container.RegisterType<IRepairSectionFaultyItemTransferInfoBusiness, RepairSectionFaultyItemTransferInfoBusiness>();            
            container.RegisterType<IRepairSectionRequisitionInfoBusiness, RepairSectionRequisitionInfoBusiness>();
            container.RegisterType<IRepairSectionRequisitionDetailBusiness, RepairSectionRequisitionDetailBusiness>();
            container.RegisterType<IProductionFaultyStockDetailBusiness, ProductionFaultyStockDetailBusiness>();
            container.RegisterType<IProductionFaultyStockInfoBusiness, ProductionFaultyStockInfoBusiness>();
            container.RegisterType<IRepairSectionFaultyItemTransferDetailBusiness, RepairSectionFaultyItemTransferDetailBusiness>();
            container.RegisterType<IQCPassTransferInformationBusiness, QCPassTransferInformationBusiness>();
            container.RegisterType<IProductionAssembleStockDetailBusiness, ProductionAssembleStockDetailBusiness>();
            container.RegisterType<IProductionAssembleStockInfoBusiness, ProductionAssembleStockInfoBusiness>();
            container.RegisterType<IProductionToPackagingStockTransferInfoBusiness, ProductionToPackagingStockTransferInfoBusiness>();
            container.RegisterType<IProductionToPackagingStockTransferDetailBusiness, ProductionToPackagingStockTransferDetailBusiness>();
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
            container.RegisterType<IRepairBusiness, RepairBusiness>();
            container.RegisterType<IServiceBusiness, ServiceBusiness>();
            container.RegisterType<IFaultBusiness, FaultBusiness>();
            container.RegisterType<ITransferDetailBusiness, TransferDetailBusiness>();
            container.RegisterType<ITransferInfoBusiness, TransferInfoBusiness>();
            container.RegisterType<IBranchBusiness2, BranchBusiness2>();
            container.RegisterType<IMobilePartStockDetailBusiness, MobilePartStockDetailBusiness>();
            container.RegisterType<IMobilePartStockInfoBusiness, MobilePartStockInfoBusiness>();
            container.RegisterType<IServicesWarehouseBusiness, ServicesWarehouseBusiness>();
            container.RegisterType<ICustomerServiceBusiness, CustomerServiceBusiness>();
            container.RegisterType<ITechnicalServiceBusiness, TechnicalServiceBusiness>();
            container.RegisterType<ICustomerBusiness, CustomerBusiness>();
            container.RegisterType<IMobilePartBusiness, MobilePartBusiness>();
            container.RegisterType<IClientProblemBusiness, ClientProblemBusiness>();
            container.RegisterType<IAccessoriesBusiness, AccessoriesBusiness>();
            container.RegisterType<IConfigurationUnitOfWork, ConfigurationUnitOfWork>();
            #endregion

            // FrontDesk Database
            #region FrontDesk
            container.RegisterType<IJobOrderRepairBusiness, JobOrderRepairBusiness>();
            container.RegisterType<IJobOrderServiceBusiness, JobOrderServiceBusiness>();
            container.RegisterType<IJobOrderFaultBusiness, JobOrderFaultBusiness>();
            container.RegisterType<IJobOrderProblemBusiness, JobOrderProblemBusiness>();
            container.RegisterType<IJobOrderAccessoriesBusiness, JobOrderAccessoriesBusiness>();
            container.RegisterType<IJobOrderTSBusiness, JobOrderTSBusiness>();
            container.RegisterType<ITechnicalServicesStockBusiness, TechnicalServicesStockBusiness>();
            container.RegisterType<IRequsitionDetailForJobOrderBusiness,RequsitionDetailForJobOrderBusiness>();
            container.RegisterType<IRequsitionInfoForJobOrderBusiness, RequsitionInfoForJobOrderBusiness>();
            container.RegisterType<IJobOrderBusiness, JobOrderBusiness>();
            container.RegisterType<IFrontDeskUnitOfWork, FrontDeskUnitOfWork>();
            #endregion

            #region Report

            container.RegisterType<IProductionReportBusiness, ProductionReportBusiness>();

            #endregion

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}