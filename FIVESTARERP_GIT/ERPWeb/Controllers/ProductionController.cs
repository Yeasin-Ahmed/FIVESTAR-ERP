using ERPBLL.Common;
using ERPBLL.Inventory.Interface;
using ERPBLL.Production.Interface;
using ERPBO.Common;
using ERPBO.Inventory.DTOModel;
using ERPBO.Inventory.ViewModels;
using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using ERPBO.Production.ViewModels;
using ERPWeb.Filters;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ERPWeb.Controllers
{
    [CustomAuthorize]
    public class ProductionController : BaseController
    {
        // GET: Production
        #region Production Business Classes
        private readonly IRequsitionInfoBusiness _requsitionInfoBusiness;
        private readonly IRequsitionDetailBusiness _requsitionDetailBusiness;
        private readonly IProductionLineBusiness _productionLineBusiness;
        private readonly IProductionStockInfoBusiness _productionStockInfoBusiness;
        private readonly IProductionStockDetailBusiness _productionStockDetailBusiness;
        private readonly IItemReturnInfoBusiness _itemReturnInfoBusiness;
        private readonly IItemReturnDetailBusiness _itemReturnDetailBusiness;
        private readonly IFinishGoodsInfoBusiness _finishGoodsInfoBusiness;
        private readonly IFinishGoodsRowMaterialBusiness _finishGoodsRowMaterialBusiness;
        private readonly IFinishGoodsStockInfoBusiness _finishGoodsStockInfoBusiness;
        private readonly IFinishGoodsStockDetailBusiness _finishGoodsStockDetailBusiness;
        private readonly IFinishGoodsSendToWarehouseInfoBusiness _finishGoodsSendToWarehouseInfoBusiness;
        private readonly IFinishGoodsSendToWarehouseDetailBusiness _finishGoodsSendToWarehouseDetailBusiness;
        private readonly IAssemblyLineBusiness _assemblyLineBusiness;
        private readonly ITransferStockToAssemblyInfoBusiness _transferStockToAssemblyInfoBusiness;
        private readonly ITransferStockToAssemblyDetailBusiness _transferStockToAssemblyDetailBusiness;
        private readonly IAssemblyLineStockInfoBusiness _assemblyLineStockInfoBusiness;
        private readonly IAssemblyLineStockDetailBusiness _assemblyLineStockDetailBusiness;
        private readonly IQualityControlBusiness _qualityControlBusiness;
        private readonly ITransferStockToQCInfoBusiness _transferStockToQCInfoBusiness;
        private readonly ITransferStockToQCDetailBusiness _transferStockToQCDetailBusiness;
        private readonly IQCLineStockInfoBusiness _qCLineStockInfoBusiness;
        private readonly IQCLineStockDetailBusiness _qCLineStockDetailBusiness;
        private readonly IRepairLineBusiness _repairLineBusiness;
        private readonly IPackagingLineBusiness _packagingLineBusiness;
        private readonly ITransferFromQCInfoBusiness _transferFromQCInfoBusiness;
        private readonly ITransferFromQCDetailBusiness _transferFromQCDetailBusiness;
        private readonly IPackagingLineStockInfoBusiness _packagingLineStockInfoBusiness;
        private readonly IPackagingLineStockDetailBusiness _packagingLineStockDetailBusiness;
        private readonly ITransferStockToPackagingLine2InfoBusiness _transferStockToPackagingLine2InfoBusiness;
        private readonly ITransferStockToPackagingLine2DetailBusiness _transferStockToPackagingLine2DetailBusiness;
        private readonly IRepairLineStockInfoBusiness _repairLineStockInfoBusiness;
        private readonly IRepairLineStockDetailBusiness _repairLineStockDetailBusiness;
        private readonly ITransferRepairItemToQcInfoBusiness _transferRepairItemToQcInfoBusiness;
        private readonly ITransferRepairItemToQcDetailBusiness _transferRepairItemToQcDetailBusiness;

        private readonly IQCItemStockInfoBusiness _qCItemStockInfoBusiness;
        private readonly IQCItemStockDetailBusiness _qCItemStockDetailBusiness;

        private readonly IRepairItemStockInfoBusiness _repairItemStockInfoBusiness;
        private readonly IRepairItemStockDetailBusiness _repairItemStockDetailBusiness;
        private readonly IPackagingItemStockInfoBusiness _packagingItemStockInfoBusiness;
        private readonly IPackagingItemStockDetailBusiness _packagingItemStockDetailBusiness;
        private readonly IFaultyItemStockDetailBusiness _faultyItemStockDetailBusiness;
        private readonly IFaultyItemStockInfoBusiness _faultyItemStockInfoBusiness;
        private readonly IFaultyCaseBusiness _faultyCaseBusiness;
        private readonly IRepairItemBusiness _repairItemBusiness;
        private readonly IRepairSectionFaultyItemTransferInfoBusiness _repairSectionFaultyItemTransferInfoBusiness;
        private readonly IRepairSectionFaultyItemTransferDetailBusiness _repairSectionFaultyItemTransferDetailBusiness;
        private readonly IRepairSectionRequisitionInfoBusiness _repairSectionRequisitionInfoBusiness;
        private readonly IRepairSectionRequisitionDetailBusiness _repairSectionRequisitionDetailBusiness;
        private readonly IQRCodeTraceBusiness _qRCodeTraceBusiness;
        private readonly IProductionFaultyStockDetailBusiness _productionFaultyStockDetailBusiness;
        private readonly IProductionFaultyStockInfoBusiness _productionFaultyStockInfoBusiness;
        private readonly IQCPassTransferInformationBusiness _qCPassTransferInformationBusiness;
        private readonly IProductionAssembleStockDetailBusiness _productionAssembleStockDetailBusiness;
        private readonly IProductionAssembleStockInfoBusiness _productionAssembleStockInfoBusiness;
        private readonly IProductionToPackagingStockTransferInfoBusiness _productionToPackagingStockTransferInfoBusiness;
        private readonly IProductionToPackagingStockTransferDetailBusiness _productionToPackagingStockTransferDetailBusiness;
        private readonly IQRCodeTransferToRepairInfoBusiness _qRCodeTransferToRepairInfoBusiness;
        private readonly IQRCodeProblemBusiness _qRCodeProblemBusiness;
        private readonly IRequisitionItemInfoBusiness _requisitionItemInfoBusiness;
        private readonly IRequisitionItemDetailBusiness _requisitionItemDetailBusiness;
        private readonly ITempQRCodeTraceBusiness _tempQRCodeTraceBusiness;
        private readonly IQCPassTransferDetailBusiness _qCPassTransferDetailBusiness;
        private readonly IMiniStockTransferInfoBusiness _miniStockTransferInfoBusiness;
        private readonly IMiniStockTransferDetailBusiness _miniStockTransferDetailBusiness;
        private readonly IIMEITransferToRepairInfoBusiness _iMEITransferToRepairInfoBusiness;
        private readonly ITransferToPackagingRepairInfoBusiness _transferToPackagingRepairInfoBusiness;
        private readonly ITransferToPackagingRepairDetailBusiness _transferToPackagingRepairDetailBusiness;

        private readonly IPackagingRepairItemStockInfoBusiness _packagingRepairItemStockInfoBusiness;
        private readonly IPackagingRepairItemStockDetailBusiness _packagingRepairItemStockDetailBusiness;

        private readonly IPackagingRepairRawStockInfoBusiness _packagingRepairRawStockInfoBusiness;
        private readonly IPackagingRepairRawStockDetailBusiness _packagingRepairRawStockDetailBusiness;

        private readonly IIMEITransferToRepairDetailBusiness _iMEITransferToRepairDetailBusiness;
        private readonly ITransferPackagingRepairItemToQcInfoBusiness _transferPackagingRepairItemToQcInfoBusiness;
        private readonly ITransferPackagingRepairItemToQcDetailBusiness _transferPackagingRepairItemToQcDetailBusiness;

        private readonly IPackagingFaultyStockInfoBusiness _packagingFaultyStockInfoBusiness;
        private readonly IPackagingFaultyStockDetailBusiness _packagingFaultyStockDetailBusiness;
        private readonly IIMEIGenerator _iMEIGenerator;
        private readonly IGeneratedIMEIBusiness _generatedIMEIBusiness;
        private readonly IStockItemReturnInfoBusiness _stockItemReturnInfoBusiness;
        private readonly IStockItemReturnDetailBusiness _stockItemReturnDetailBusiness;
        private readonly ILotInLogBusiness _lotInLogBusiness;
        #endregion

        #region Inventory Business Classes
        private readonly IWarehouseBusiness _warehouseBusiness;
        private readonly IDescriptionBusiness _descriptionBusiness;
        private readonly IItemBusiness _itemBusiness;
        private readonly IItemTypeBusiness _itemTypeBusiness;
        private readonly IUnitBusiness _unitBusiness;
        private readonly IItemPreparationDetailBusiness _itemPreparationDetailBusiness;
        private readonly IItemPreparationInfoBusiness _itemPreparationInfoBusiness;
        private readonly IWarehouseStockDetailBusiness _warehouseStockDetailBusiness;
        #endregion

        public ProductionController(IRequsitionInfoBusiness requsitionInfoBusiness, IWarehouseBusiness warehouseBusiness, IRequsitionDetailBusiness requsitionDetailBusiness, IProductionLineBusiness productionLineBusiness, IItemBusiness itemBusiness, IItemTypeBusiness itemTypeBusiness, IUnitBusiness unitBusiness, IProductionStockDetailBusiness productionStockDetailBusiness, IProductionStockInfoBusiness productionStockInfoBusiness, IItemReturnInfoBusiness itemReturnInfoBusiness, IItemReturnDetailBusiness itemReturnDetailBusiness, IDescriptionBusiness descriptionBusiness, IFinishGoodsInfoBusiness finishGoodsInfoBusiness, IFinishGoodsRowMaterialBusiness finishGoodsRowMaterialBusiness, IFinishGoodsStockInfoBusiness finishGoodsStockInfoBusiness, IFinishGoodsStockDetailBusiness finishGoodsStockDetailBusiness, IFinishGoodsSendToWarehouseInfoBusiness finishGoodsSendToWarehouseInfoBusiness, IFinishGoodsSendToWarehouseDetailBusiness finishGoodsSendToWarehouseDetailBusiness, IItemPreparationDetailBusiness itemPreparationDetailBusiness, IItemPreparationInfoBusiness itemPreparationInfoBusiness, IAssemblyLineBusiness assemblyLineBusiness, ITransferStockToAssemblyInfoBusiness transferStockToAssemblyInfoBusiness, ITransferStockToAssemblyDetailBusiness transferStockToAssemblyDetailBusiness, IAssemblyLineStockInfoBusiness assemblyLineStockInfoBusiness, IAssemblyLineStockDetailBusiness assemblyLineStockDetailBusiness, IQualityControlBusiness qualityControlBusiness, ITransferStockToQCInfoBusiness transferStockToQCInfoBusiness, ITransferStockToQCDetailBusiness transferStockToQCDetailBusiness, IQCLineStockInfoBusiness qCLineStockInfoBusiness, IQCLineStockDetailBusiness qCLineStockDetailBusiness, IRepairLineBusiness repairLineBusiness, IPackagingLineBusiness packagingLineBusiness, ITransferFromQCInfoBusiness transferFromQCInfoBusiness, ITransferFromQCDetailBusiness transferFromQCDetailBusiness, IPackagingLineStockInfoBusiness packagingLineStockInfoBusiness, IPackagingLineStockDetailBusiness packagingLineStockDetailBusiness, ITransferStockToPackagingLine2InfoBusiness transferStockToPackagingLine2InfoBusiness, ITransferStockToPackagingLine2DetailBusiness transferStockToPackagingLine2DetailBusiness, IRepairLineStockInfoBusiness repairLineStockInfoBusiness, IRepairLineStockDetailBusiness repairLineStockDetailBusiness, ITransferRepairItemToQcInfoBusiness transferRepairItemToQcInfoBusiness, ITransferRepairItemToQcDetailBusiness transferRepairItemToQcDetailBusiness, IQCItemStockInfoBusiness qCItemStockInfoBusiness, IQCItemStockDetailBusiness qCItemStockDetailBusiness, IRepairItemStockInfoBusiness repairItemStockInfoBusiness, IRepairItemStockDetailBusiness repairItemStockDetailBusiness, IPackagingItemStockInfoBusiness packagingItemStockInfoBusiness, IPackagingItemStockDetailBusiness packagingItemStockDetailBusiness, IFaultyItemStockDetailBusiness faultyItemStockDetailBusiness, IFaultyItemStockInfoBusiness faultyItemStockInfoBusiness, IFaultyCaseBusiness faultyCaseBusiness, IRepairItemBusiness repairItemBusiness, IRepairSectionFaultyItemTransferInfoBusiness repairSectionFaultyItemTransferInfoBusiness, IRepairSectionRequisitionInfoBusiness repairSectionRequisitionInfoBusiness, IRepairSectionRequisitionDetailBusiness repairSectionRequisitionDetailBusiness, IQRCodeTraceBusiness qRCodeTraceBusiness, IProductionFaultyStockDetailBusiness productionFaultyStockDetailBusiness, IProductionFaultyStockInfoBusiness productionFaultyStockInfoBusiness, IRepairSectionFaultyItemTransferDetailBusiness repairSectionFaultyItemTransferDetailBusiness, IQCPassTransferInformationBusiness qCPassTransferInformationBusiness, IProductionAssembleStockDetailBusiness productionAssembleStockDetailBusiness, IProductionAssembleStockInfoBusiness productionAssembleStockInfoBusiness, IProductionToPackagingStockTransferInfoBusiness productionToPackagingStockTransferInfoBusiness, IProductionToPackagingStockTransferDetailBusiness productionToPackagingStockTransferDetailBusiness, IQRCodeTransferToRepairInfoBusiness qRCodeTransferToRepairInfoBusiness, IQRCodeProblemBusiness qRCodeProblemBusiness, IRequisitionItemInfoBusiness requisitionItemInfoBusiness, IRequisitionItemDetailBusiness requisitionItemDetailBusiness, ITempQRCodeTraceBusiness tempQRCodeTraceBusiness, IQCPassTransferDetailBusiness qCPassTransferDetailBusiness, IMiniStockTransferInfoBusiness miniStockTransferInfoBusiness, IMiniStockTransferDetailBusiness miniStockTransferDetailBusiness, IIMEITransferToRepairInfoBusiness iMEITransferToRepairInfoBusiness, ITransferToPackagingRepairInfoBusiness transferToPackagingRepairInfoBusiness, ITransferToPackagingRepairDetailBusiness transferToPackagingRepairDetailBusiness, IPackagingRepairRawStockInfoBusiness packagingRepairRawStockInfoBusiness, IPackagingRepairRawStockDetailBusiness packagingRepairRawStockDetailBusiness, IPackagingRepairItemStockInfoBusiness packagingRepairItemStockInfoBusiness, IPackagingRepairItemStockDetailBusiness packagingRepairItemStockDetailBusiness, IIMEITransferToRepairDetailBusiness iMEITransferToRepairDetailBusiness, ITransferPackagingRepairItemToQcInfoBusiness transferPackagingRepairItemToQcInfoBusiness, ITransferPackagingRepairItemToQcDetailBusiness transferPackagingRepairItemToQcDetailBusiness, IPackagingFaultyStockInfoBusiness packagingFaultyStockInfoBusiness, IPackagingFaultyStockDetailBusiness packagingFaultyStockDetailBusiness, IIMEIGenerator iMEIGenerator, IGeneratedIMEIBusiness generatedIMEIBusiness, IStockItemReturnInfoBusiness stockItemReturnInfoBusiness, IStockItemReturnDetailBusiness stockItemReturnDetailBusiness, IWarehouseStockDetailBusiness warehouseStockDetailBusiness, ILotInLogBusiness lotInLogBusiness)
        {
            #region Production
            this._requsitionInfoBusiness = requsitionInfoBusiness;
            this._requsitionDetailBusiness = requsitionDetailBusiness;
            this._productionLineBusiness = productionLineBusiness;
            this._productionStockDetailBusiness = productionStockDetailBusiness;
            this._productionStockInfoBusiness = productionStockInfoBusiness;
            this._itemReturnInfoBusiness = itemReturnInfoBusiness;
            this._itemReturnDetailBusiness = itemReturnDetailBusiness;
            this._finishGoodsInfoBusiness = finishGoodsInfoBusiness;
            this._finishGoodsRowMaterialBusiness = finishGoodsRowMaterialBusiness;
            this._finishGoodsStockInfoBusiness = finishGoodsStockInfoBusiness;
            this._finishGoodsStockDetailBusiness = finishGoodsStockDetailBusiness;
            this._finishGoodsSendToWarehouseInfoBusiness = finishGoodsSendToWarehouseInfoBusiness;
            this._finishGoodsSendToWarehouseDetailBusiness = finishGoodsSendToWarehouseDetailBusiness;
            this._assemblyLineBusiness = assemblyLineBusiness;
            this._transferStockToAssemblyInfoBusiness = transferStockToAssemblyInfoBusiness;
            this._transferStockToAssemblyDetailBusiness = transferStockToAssemblyDetailBusiness;
            this._assemblyLineStockInfoBusiness = assemblyLineStockInfoBusiness;
            this._assemblyLineStockDetailBusiness = assemblyLineStockDetailBusiness;
            this._qualityControlBusiness = qualityControlBusiness;
            this._transferStockToQCInfoBusiness = transferStockToQCInfoBusiness;
            this._transferStockToQCDetailBusiness = transferStockToQCDetailBusiness;
            this._qCLineStockInfoBusiness = qCLineStockInfoBusiness;
            this._qCLineStockDetailBusiness = qCLineStockDetailBusiness;
            this._repairLineBusiness = repairLineBusiness;
            this._packagingLineBusiness = packagingLineBusiness;
            this._transferFromQCInfoBusiness = transferFromQCInfoBusiness;
            this._transferFromQCDetailBusiness = transferFromQCDetailBusiness;
            this._packagingLineStockInfoBusiness = packagingLineStockInfoBusiness;
            this._packagingLineStockDetailBusiness = packagingLineStockDetailBusiness;
            this._transferStockToPackagingLine2InfoBusiness = transferStockToPackagingLine2InfoBusiness;
            this._transferStockToPackagingLine2DetailBusiness = transferStockToPackagingLine2DetailBusiness;
            this._repairLineStockInfoBusiness = repairLineStockInfoBusiness;
            this._repairLineStockDetailBusiness = repairLineStockDetailBusiness;
            this._transferRepairItemToQcInfoBusiness = transferRepairItemToQcInfoBusiness;
            this._transferRepairItemToQcDetailBusiness = transferRepairItemToQcDetailBusiness;
            this._qCItemStockInfoBusiness = qCItemStockInfoBusiness;
            this._qCItemStockDetailBusiness = qCItemStockDetailBusiness;
            this._repairItemStockInfoBusiness = repairItemStockInfoBusiness;
            this._repairItemStockDetailBusiness = repairItemStockDetailBusiness;
            this._packagingItemStockInfoBusiness = packagingItemStockInfoBusiness;
            this._packagingItemStockDetailBusiness = packagingItemStockDetailBusiness;
            this._faultyItemStockInfoBusiness = faultyItemStockInfoBusiness;
            this._faultyItemStockDetailBusiness = faultyItemStockDetailBusiness;
            this._faultyCaseBusiness = faultyCaseBusiness;
            this._repairItemBusiness = repairItemBusiness;
            this._repairSectionFaultyItemTransferInfoBusiness = repairSectionFaultyItemTransferInfoBusiness;
            this._repairSectionRequisitionInfoBusiness = repairSectionRequisitionInfoBusiness;
            this._repairSectionRequisitionDetailBusiness = repairSectionRequisitionDetailBusiness;
            this._qRCodeTraceBusiness = qRCodeTraceBusiness;
            this._productionFaultyStockInfoBusiness = productionFaultyStockInfoBusiness;
            this._productionFaultyStockDetailBusiness = productionFaultyStockDetailBusiness;
            this._repairSectionFaultyItemTransferDetailBusiness = repairSectionFaultyItemTransferDetailBusiness;
            this._qCPassTransferInformationBusiness = qCPassTransferInformationBusiness;
            this._productionAssembleStockDetailBusiness = productionAssembleStockDetailBusiness;
            this._productionAssembleStockInfoBusiness = productionAssembleStockInfoBusiness;
            this._productionToPackagingStockTransferInfoBusiness = productionToPackagingStockTransferInfoBusiness;
            this._productionToPackagingStockTransferDetailBusiness = productionToPackagingStockTransferDetailBusiness;
            this._qRCodeTransferToRepairInfoBusiness = qRCodeTransferToRepairInfoBusiness;
            this._qRCodeProblemBusiness = qRCodeProblemBusiness;
            this._requisitionItemInfoBusiness = requisitionItemInfoBusiness;
            this._requisitionItemDetailBusiness = requisitionItemDetailBusiness;
            this._tempQRCodeTraceBusiness = tempQRCodeTraceBusiness;
            this._qCPassTransferDetailBusiness = qCPassTransferDetailBusiness;
            this._miniStockTransferInfoBusiness = miniStockTransferInfoBusiness;
            this._miniStockTransferDetailBusiness = miniStockTransferDetailBusiness;
            this._iMEITransferToRepairInfoBusiness = iMEITransferToRepairInfoBusiness;
            this._transferToPackagingRepairInfoBusiness = transferToPackagingRepairInfoBusiness;
            this._transferToPackagingRepairDetailBusiness = transferToPackagingRepairDetailBusiness;
            this._packagingRepairItemStockInfoBusiness = packagingRepairItemStockInfoBusiness;
            this._packagingRepairItemStockDetailBusiness = packagingRepairItemStockDetailBusiness;
            this._packagingRepairRawStockInfoBusiness = packagingRepairRawStockInfoBusiness;
            this._packagingRepairRawStockDetailBusiness = packagingRepairRawStockDetailBusiness;
            this._iMEITransferToRepairDetailBusiness = iMEITransferToRepairDetailBusiness;
            this._transferPackagingRepairItemToQcInfoBusiness = transferPackagingRepairItemToQcInfoBusiness;
            this._transferPackagingRepairItemToQcDetailBusiness = transferPackagingRepairItemToQcDetailBusiness;

            this._packagingFaultyStockInfoBusiness = packagingFaultyStockInfoBusiness;
            this._packagingFaultyStockDetailBusiness = packagingFaultyStockDetailBusiness;
            this._iMEIGenerator = iMEIGenerator;
            this._generatedIMEIBusiness = generatedIMEIBusiness;
            this._stockItemReturnInfoBusiness = stockItemReturnInfoBusiness;
            this._stockItemReturnDetailBusiness = stockItemReturnDetailBusiness;
            this._lotInLogBusiness = lotInLogBusiness;
            #endregion

            #region Inventory
            this._warehouseBusiness = warehouseBusiness;
            this._itemBusiness = itemBusiness;
            this._itemTypeBusiness = itemTypeBusiness;
            this._unitBusiness = unitBusiness;
            this._descriptionBusiness = descriptionBusiness;
            this._itemPreparationDetailBusiness = itemPreparationDetailBusiness;
            this._itemPreparationInfoBusiness = itemPreparationInfoBusiness;
            this._warehouseStockDetailBusiness = warehouseStockDetailBusiness;
            #endregion
        }

        #region Production Belts Config

        public  ActionResult ProductionBeltConfig(string flag)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();
                return View();
            }
            else if (!string.IsNullOrEmpty(flag) && flag.Trim() =="floor")
            {
                //GetProductionLineList
                IEnumerable<ProductionLineDTO> dto = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new ProductionLineDTO
                {
                    LineId = line.LineId,
                    LineNumber = line.LineNumber,
                    LineIncharge = line.LineIncharge,
                    Remarks = line.Remarks,
                    StateStatus = (line.IsActive == true ? "Active" : "Inactive"),
                    OrganizationId = line.OrganizationId,
                    EntryDate = line.EntryDate,
                    UpdateDate = line.UpdateDate,
                    EntryUser = UserForEachRecord(line.EUserId.Value).UserName,
                    UpdateUser = (line.UpUserId == null || line.UpUserId == 0) ? "" : UserForEachRecord(line.UpUserId.Value).UserName
                }).OrderBy(line => line.LineId).ToList();
                IEnumerable<ProductionLineViewModel> viewModel = new List<ProductionLineViewModel>();
                AutoMapper.Mapper.Map(dto, viewModel);
                return PartialView("_floor",viewModel);
            }
            else if (!string.IsNullOrEmpty(flag) && flag.Trim() == "assembly")
            {
                //GetAssemblyLines
                var assemblyLinesDtos = _assemblyLineBusiness.GetAssemblyLines(User.OrgId).Select(a => new AssemblyLineDTO
                {
                    AssemblyLineId = a.AssemblyLineId,
                    AssemblyLineName = a.AssemblyLineName,
                    ProductionLineId = a.ProductionLineId,
                    LineNumber = _productionLineBusiness.GetProductionLineOneByOrgId(a.ProductionLineId, User.OrgId).LineNumber,
                    IsActive = a.IsActive,
                    OrganizationId = a.OrganizationId,
                    EntryDate = DateTime.Now,
                    Remarks = a.Remarks,
                    EntryUser = UserForEachRecord(a.EUserId.Value).UserName,
                    UpdateUser = (a.UpUserId == null || a.UpUserId == 0) ? "" : UserForEachRecord(a.UpUserId.Value).UserName
                }).ToList();
                IEnumerable<AssemblyLineViewModel> assemblyLineViewModels = new List<AssemblyLineViewModel>();
                AutoMapper.Mapper.Map(assemblyLinesDtos, assemblyLineViewModels);
                return PartialView("_assembly",assemblyLineViewModels);
            }
            else if (!string.IsNullOrEmpty(flag) && flag.Trim() == "qc")
            {
                //GetQualityControls
                var dto = _qualityControlBusiness.GetQualityControls(User.OrgId).Select(s => new QualityControlDTO
                {
                    QCId = s.QCId,
                    QCName = s.QCName,
                    IsActive = s.IsActive,
                    ProductionLineId = s.ProductionLineId,
                    LineNumber = _productionLineBusiness.GetProductionLineOneByOrgId(s.ProductionLineId, User.OrgId).LineNumber,
                    Remarks = s.Remarks,
                    EntryUser = UserForEachRecord(s.EUserId.Value).UserName,
                    UpdateUser = (s.UpUserId == null || s.UpUserId == 0) ? "" : UserForEachRecord(s.UpUserId.Value).UserName,
                    EntryDate = s.EntryDate,
                    UpdateDate = s.UpdateDate
                }).ToList();
                IEnumerable<QualityControlViewModel> viewModels = new List<QualityControlViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_qc",viewModels);
            }
            else if (!string.IsNullOrEmpty(flag) && flag.Trim() == "repair")
            {
                //GetRepairLines
                var dto = _repairLineBusiness.GetRepairLinesByOrgId(User.OrgId).Select(s => new RepairLineDTO
                {
                    RepairLineId = s.RepairLineId,
                    RepairLineName = s.RepairLineName,
                    IsActive = s.IsActive,
                    ProductionLineId = s.ProductionLineId,
                    ProductionLineName = _productionLineBusiness.GetProductionLineOneByOrgId(s.ProductionLineId, User.OrgId).LineNumber,
                    Remarks = s.Remarks,
                    EntryUser = UserForEachRecord(s.EUserId.Value).UserName,
                    UpdateUser = (s.UpUserId == null || s.UpUserId == 0) ? "" : UserForEachRecord(s.UpUserId.Value).UserName,
                    EntryDate = s.EntryDate,
                    UpdateDate = s.UpdateDate
                }).ToList();
                IEnumerable<RepairLineViewModel> viewModels = new List<RepairLineViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_repair",viewModels);
            }
            else if (!string.IsNullOrEmpty(flag) && flag.Trim() == "packaging")
            {
                //GetPackagingLines
                var dto = _packagingLineBusiness.GetPackagingLinesByOrgId(User.OrgId).Select(s => new PackagingLineDTO
                {
                    PackagingLineId = s.PackagingLineId,
                    PackagingLineName = s.PackagingLineName,
                    IsActive = s.IsActive,
                    ProductionLineId = s.ProductionLineId,
                    ProductionLineName = _productionLineBusiness.GetProductionLineOneByOrgId(s.ProductionLineId, User.OrgId).LineNumber,
                    Remarks = s.Remarks,
                    EntryUser = UserForEachRecord(s.EUserId.Value).UserName,
                    UpdateUser = (s.UpUserId == null || s.UpUserId == 0) ? "" : UserForEachRecord(s.UpUserId.Value).UserName,
                    EntryDate = s.EntryDate,
                    UpdateDate = s.UpdateDate
                }).ToList();
                IEnumerable<PackagingLineViewModel> viewModels = new List<PackagingLineViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_packaging", viewModels);
            }
            return Content("");
        }

        #endregion

        #region ProductionLine
        public ActionResult GetProductionLineList()
        {
            IEnumerable<ProductionLineDTO> dto = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new ProductionLineDTO
            {
                LineId = line.LineId,
                LineNumber = line.LineNumber,
                LineIncharge = line.LineIncharge,
                Remarks = line.Remarks,
                StateStatus = (line.IsActive == true ? "Active" : "Inactive"),
                OrganizationId = line.OrganizationId,
                EntryDate = line.EntryDate,
                UpdateDate = line.UpdateDate,
                EntryUser = UserForEachRecord(line.EUserId.Value).UserName,
                UpdateUser = (line.UpUserId == null || line.UpUserId == 0) ? "" : UserForEachRecord(line.UpUserId.Value).UserName
            }).OrderBy(line => line.LineId).ToList();
            IEnumerable<ProductionLineViewModel> viewModel = new List<ProductionLineViewModel>();
            ViewBag.UserPrivilege = UserPrivilege("Production", "GetProductionLineList");
            AutoMapper.Mapper.Map(dto, viewModel);
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult SaveProductionLine(ProductionLineViewModel productionLineViewModel)
        {
            bool isSuccess = false;
            var privilege = UserPrivilege("Production", "GetProductionLineList") ;
            //var permission = (productionLineViewModel.LineId == 0 && privilege.Add) || (productionLineViewModel.LineId > 0 && privilege.Edit);
            //&& permission
            if (ModelState.IsValid)
            {
                try
                {
                    ProductionLineDTO dto = new ProductionLineDTO();
                    AutoMapper.Mapper.Map(productionLineViewModel, dto);
                    isSuccess = _productionLineBusiness.SaveLine(dto, User.UserId, User.OrgId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }
        #endregion

        #region Requsition
        [HttpGet]
        public ActionResult GetReqInfoList()
        {
            ViewBag.UserPrivilege = UserPrivilege("Production", "GetReqInfoList");
            ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(ware => new SelectListItem { Text = ware.WarehouseName, Value = ware.Id.ToString() }).ToList();

            ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

            ViewBag.ddlStateStatus = Utility.ListOfReqStatus().Where(status => status.value != RequisitionStatus.Pending).Select(st => new SelectListItem
            {
                Text = st.text,
                Value = st.value
            }).ToList();

            ViewBag.ddlModelName = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).Select(des => new SelectListItem { Text = des.DescriptionName, Value = des.DescriptionId.ToString() }).ToList();

            ViewBag.ddlRequisitionType = Utility.ListOfRequisitionType().Select(r => new SelectListItem { Text = r.text, Value = r.value }).ToList();
            return View();
        }
        public ActionResult CreateRequsition()
        {
            ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.WarehouseName, Value = line.Id.ToString() }).ToList();
            ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();
            ViewBag.ddlModelName = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).Select(des => new SelectListItem { Text = des.DescriptionName, Value = des.DescriptionId.ToString() }).ToList();
            ViewBag.ddlRequisitionType = Utility.ListOfRequisitionType().Select(r => new SelectListItem { Text = r.text, Value = r.value }).ToList();
            ViewBag.ddlExecutionType = new List<SelectListItem>
            {
                new SelectListItem(){Text=RequisitionExecuationType.Single,Value=RequisitionExecuationType.Single },
                new SelectListItem(){Text=RequisitionExecuationType.Bundle,Value=RequisitionExecuationType.Bundle }
            };
            return View();
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveRequsition(VmReqInfo model)
        {
            bool isSuccess = false;
            //var privilege = UserPrivilege("Production", "CreateRequsition");
            //var permission = (model.ReqInfoId == 0 && privilege.Add) || (model.ReqInfoId > 0 && privilege.Edit); && permission
            if (ModelState.IsValid && model.ReqDetails.Count > 0)
            {
                try
                {
                    ReqInfoDTO dto = new ReqInfoDTO();
                    AutoMapper.Mapper.Map(model, dto);
                    if (model.ReqInfoId == 0)
                    {
                        isSuccess = _requsitionInfoBusiness.SaveRequisition(dto, User.UserId, User.OrgId);
                    }
                    else
                    {
                        isSuccess = _requsitionDetailBusiness.SaveRequisitionDetail(dto, User.UserId, User.OrgId);
                    }

                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }
        // Used By  GetReqInfoList ActionMethod
        public ActionResult GetReqInfoParitalList(string reqCode, long? warehouseId, string status, long? modelId, long? line, string fromDate, string toDate, string requisitionType, int page = 1)
        {
            ViewBag.UserPrivilege = UserPrivilege("Production", "GetReqInfoList");
            var descriptionData = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId);
            IEnumerable<RequsitionInfoDTO> dto = _requsitionInfoBusiness.GetAllReqInfoByOrgId(User.OrgId).Where(req =>
                req.Flag == Flag.Indirect &&
                (reqCode == null || reqCode.Trim() == "" || req.ReqInfoCode.Contains(reqCode)) &&
                (warehouseId == null || warehouseId <= 0 || req.WarehouseId == warehouseId) &&
                (status == null || status.Trim() == "" || req.StateStatus == status.Trim()) &&
                (requisitionType == null || requisitionType.Trim() == "" || req.RequisitionType == requisitionType.Trim()) &&
                (line == null || line <= 0 || req.LineId == line) &&
                (modelId == null || modelId <= 0 || req.DescriptionId == modelId) &&
                (
                    (fromDate == null && toDate == null)
                    ||
                     (fromDate == "" && toDate == "")
                    ||
                    (fromDate.Trim() != "" && toDate.Trim() != "" &&

                        req.EntryDate.Value.Date >= Convert.ToDateTime(fromDate).Date &&
                        req.EntryDate.Value.Date <= Convert.ToDateTime(toDate).Date)
                    ||
                    (fromDate.Trim() != "" && req.EntryDate.Value.Date == Convert.ToDateTime(fromDate).Date)
                    ||
                    (toDate.Trim() != "" && req.EntryDate.Value.Date == Convert.ToDateTime(toDate).Date)
                )
            ).Select(info => new RequsitionInfoDTO
            {
                ReqInfoId = info.ReqInfoId,
                ReqInfoCode = info.ReqInfoCode,
                LineId = info.LineId,
                LineNumber = (_productionLineBusiness.GetProductionLineOneByOrgId(info.LineId, User.OrgId).LineNumber),
                StateStatus = info.StateStatus,
                Remarks = info.Remarks,
                OrganizationId = info.OrganizationId,
                EntryDate = info.EntryDate,
                WarehouseId = info.WarehouseId,
                WarehouseName = (_warehouseBusiness.GetWarehouseOneByOrgId(info.WarehouseId, User.OrgId).WarehouseName),
                ModelName = descriptionData.FirstOrDefault(d => d.DescriptionId == info.DescriptionId).DescriptionName,
                Qty = _requsitionDetailBusiness.GetRequsitionDetailByReqId(info.ReqInfoId, User.OrgId).Select(s => s.ItemId).Distinct().Count(),
                RequisitionType = info.RequisitionType,
                EntryUser = UserForEachRecord(info.EUserId.Value).UserName,
                UpdateUser = (info.UpUserId == null || info.UpUserId == 0) ? "" : UserForEachRecord(info.UpUserId.Value).UserName
            }).OrderByDescending(s => s.ReqInfoId).ToList();
            // Pagination //
            ViewBag.PagerData = GetPagerData(dto.Count(), pageSize, page);
            dto = dto.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            //-----------------//

            List<RequsitionInfoViewModel> requsitionInfoViewModels = new List<RequsitionInfoViewModel>();
            AutoMapper.Mapper.Map(dto, requsitionInfoViewModels);
            return PartialView(requsitionInfoViewModels);
        }
        public ActionResult GetRequsitionDetails(long? reqId)
        {
            ViewBag.UserPrivilege = UserPrivilege("Production", "GetReqInfoList");
            IEnumerable<RequsitionDetailDTO> requsitionDetailDTO = _requsitionDetailBusiness.GetAllReqDetailByOrgId(User.OrgId).Where(rqd => reqId == null || reqId == 0 || rqd.ReqInfoId == reqId).Select(d => new RequsitionDetailDTO
            {
                ReqDetailId = d.ReqDetailId,
                ItemTypeId = d.ItemTypeId.Value,
                ItemTypeName = (_itemTypeBusiness.GetItemType(d.ItemTypeId.Value, User.OrgId).ItemName),
                ItemId = d.ItemId.Value,
                ItemName = (_itemBusiness.GetItemOneByOrgId(d.ItemId.Value, User.OrgId).ItemName),
                Quantity = d.Quantity.Value,
                UnitName = (_unitBusiness.GetUnitOneByOrgId(d.UnitId.Value, User.OrgId).UnitSymbol)
            }).ToList();
            List<RequsitionDetailViewModel> requsitionDetailViewModels = new List<RequsitionDetailViewModel>();
            AutoMapper.Mapper.Map(requsitionDetailDTO, requsitionDetailViewModels);

            ViewBag.RequisitionStatus = _requsitionInfoBusiness.GetRequisitionById(reqId.Value, User.OrgId).StateStatus;

            return PartialView("_GetRequsitionDetails", requsitionDetailViewModels);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveRequisitionStatus(long reqId, string status)
        {
            bool IsSuccess = false;
            var privilege = UserPrivilege("Production", "GetReqInfoList");
            var permission = (reqId > 0 && privilege != null && privilege.Edit);
            if (permission)
            {
                if (reqId > 0 && !string.IsNullOrEmpty(status) && status == RequisitionStatus.Accepted)
                {
                    IsSuccess = _productionStockDetailBusiness.SaveProductionStockInByProductionRequistion(reqId, status, User.OrgId, User.UserId);
                }
                else
                {
                    IsSuccess = _requsitionInfoBusiness.SaveRequisitionStatus(reqId, status, User.OrgId, User.UserId);
                }
            }
            return Json(IsSuccess);
        }
        public ActionResult GetRequsitionDetailsEdit(long reqId)
        {
            var items = _itemBusiness.GetAllItemByOrgId(User.OrgId);
            var itemTypes = _itemTypeBusiness.GetAllItemTypeByOrgId(User.OrgId);
            var units = _unitBusiness.GetAllUnitByOrgId(User.OrgId);

            IEnumerable<RequsitionDetailDTO> requsitionDetailDTO = _requsitionDetailBusiness.GetAllReqDetailByOrgId(User.OrgId).Where(r => r.ReqInfoId == reqId).Select(d => new RequsitionDetailDTO
            {
                ReqDetailId = d.ReqDetailId,
                ReqInfoId = d.ReqInfoId,
                ItemTypeId = d.ItemTypeId.Value,
                ItemTypeName = (_itemTypeBusiness.GetItemType(d.ItemTypeId.Value, User.OrgId).ItemName),
                ItemId = d.ItemId.Value,
                ItemName = (_itemBusiness.GetItemOneByOrgId(d.ItemId.Value, User.OrgId).ItemName),
                Quantity = d.Quantity.Value,
                UnitName = (_unitBusiness.GetUnitOneByOrgId(d.UnitId.Value, User.OrgId).UnitSymbol)
            }).ToList();
            var info = _requsitionInfoBusiness.GetRequisitionById(reqId, User.OrgId);
            RequsitionInfoViewModel requsitionInfoView = new RequsitionInfoViewModel()
            {
                ReqInfoId = info.ReqInfoId,
                ReqInfoCode = info.ReqInfoCode,
                LineNumber = _productionLineBusiness.GetProductionLineOneByOrgId(info.LineId, User.OrgId).LineNumber,
                LineId = info.LineId,
                WarehouseName = _warehouseBusiness.GetWarehouseOneByOrgId(info.WarehouseId, User.OrgId).WarehouseName,
                WarehouseId = info.WarehouseId,
                ModelName = _descriptionBusiness.GetDescriptionOneByOrdId(info.DescriptionId, User.OrgId).DescriptionName,
                DescriptionId = info.DescriptionId,
                RequisitionType = info.RequisitionType,
                RequisitionFor = info.RequisitionFor
            };

            ViewBag.RequsitionInfoViewModel = requsitionInfoView;

            List<RequsitionDetailViewModel> requsitionDetailViewModels = new List<RequsitionDetailViewModel>();
            AutoMapper.Mapper.Map(requsitionDetailDTO, requsitionDetailViewModels);
            return PartialView("GetRequsitionDetailsEdit", requsitionDetailViewModels);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult GetBundleItems(string type, long modelId, long itemId)
        {
            var info = _itemPreparationInfoBusiness.GetPreparationInfoByModelAndItemAndType(type, modelId, itemId, User.OrgId);
            ItemPreparationInfoViewModel infoData = new ItemPreparationInfoViewModel();
            List<ItemPreparationDetailViewModel> details = new List<ItemPreparationDetailViewModel>();
            if (info != null)
            {
                var warehouses = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).ToList();
                var itemTypes = _itemTypeBusiness.GetAllItemTypeByOrgId(User.OrgId).ToList();
                var items = _itemBusiness.GetAllItemByOrgId(User.OrgId).ToList();
                var units = _unitBusiness.GetAllUnitByOrgId(User.OrgId).ToList();
                var mobileModels = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).ToList();

                infoData.DescriptionId = info.DescriptionId;
                infoData.WarehouseId = info.WarehouseId;
                infoData.ItemTypeId = info.ItemTypeId;
                infoData.ItemId = info.ItemId;

                infoData.ModelName = _descriptionBusiness.GetDescriptionOneByOrdId(info.DescriptionId, User.OrgId).DescriptionName;
                infoData.WarehouseName = warehouses.FirstOrDefault(w => w.Id == infoData.WarehouseId).WarehouseName;
                infoData.ItemTypeName = itemTypes.FirstOrDefault(it => it.ItemId == infoData.ItemTypeId).ItemName;
                infoData.ItemName = itemTypes.FirstOrDefault(it => it.ItemId == infoData.ItemTypeId).ItemName;

                details = _itemPreparationDetailBusiness.GetItemPreparationDetailsByInfoId(info.PreparationInfoId, User.OrgId).Select(i => new ItemPreparationDetailViewModel
                {
                    WarehouseId = i.WarehouseId,
                    WarehouseName = warehouses.FirstOrDefault(w => w.Id == i.WarehouseId).WarehouseName,
                    ItemTypeId = i.ItemTypeId,
                    ItemTypeName = itemTypes.FirstOrDefault(it => it.ItemId == i.ItemTypeId).ItemName,
                    ItemId = i.ItemId,
                    ItemName = items.FirstOrDefault(it => it.ItemId == i.ItemId).ItemName,
                    UnitName = units.FirstOrDefault(u => u.UnitId == i.UnitId).UnitSymbol,
                    Quantity = i.Quantity,
                    Remarks = i.Remarks
                }).ToList();
            }
            return Json(new { info = infoData, details = details });
        }
        #endregion

        #region Production-Stock


        // Production Spare parts stock group
        [HttpGet]
        public ActionResult GetProductionStockInfoList()
        {
            ViewBag.UserPrivilege = UserPrivilege("Production", "GetProductionStockInfoList");

            ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

            ViewBag.ddlModelName = _descriptionBusiness.GetAllDescriptionsInProductionStock(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();

            ViewBag.ddlItems = _itemBusiness.GetItemDetails(User.OrgId).Select(s => new SelectListItem { Text = s.ItemName, Value = s.ItemId }).ToList();

            ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(ware => new SelectListItem
            {
                Text = ware.WarehouseName,
                Value = ware.Id.ToString()
            }).ToList();

            ViewBag.ddlStateStatus = Utility.ListOfReqStatus().Where(s => s.value == RequisitionStatus.Approved || s.value == RequisitionStatus.Accepted).Select(st => new SelectListItem
            {
                Text = st.text,
                Value = st.value
            }).ToList();

            return View();
        }

        [HttpGet]
        public ActionResult GetProductionStockInfoPartialList(long? WarehouseId, long? ItemTypeId, long? ItemId, long? LineId, long? ModelId, string lessOrEq, int page = 1)
        {
            IEnumerable<ProductionStockInfoDTO> dto = _productionStockInfoBusiness.GetAllProductionStockInfoByOrgId(User.OrgId).Select(info => new ProductionStockInfoDTO
            {
                StockInfoId = info.ProductionStockInfoId,
                LineId = info.LineId.Value,
                LineNumber = _productionLineBusiness.GetProductionLineOneByOrgId(info.LineId.Value, User.OrgId).LineNumber,
                WarehouseId = info.WarehouseId,
                Warehouse = (_warehouseBusiness.GetWarehouseOneByOrgId(info.WarehouseId.Value, User.OrgId).WarehouseName),
                ItemTypeId = info.ItemTypeId,
                ItemType = (_itemTypeBusiness.GetItemType(info.ItemTypeId.Value, User.OrgId).ItemName),
                ItemId = info.ItemId,
                Item = (_itemBusiness.GetItemOneByOrgId(info.ItemId.Value, User.OrgId).ItemName),
                UnitId = info.UnitId,
                Unit = (_unitBusiness.GetUnitOneByOrgId(info.UnitId.Value, User.OrgId).UnitSymbol),
                StockInQty = info.StockInQty,
                StockOutQty = info.StockOutQty,
                Remarks = info.Remarks,
                OrganizationId = info.OrganizationId,
                DescriptionId = info.DescriptionId,
                ModelName = (_descriptionBusiness.GetDescriptionOneByOrdId(info.DescriptionId.Value, info.OrganizationId).DescriptionName)
            }).AsEnumerable();

            dto = dto.Where(ws =>
            (WarehouseId == null || WarehouseId == 0 || ws.WarehouseId == WarehouseId)
            && (ItemTypeId == null || ItemTypeId == 0 || ws.ItemTypeId == ItemTypeId)
            && (ItemId == null || ItemId == 0 || ws.ItemId == ItemId)
            && (LineId == null || LineId == 0 || ws.LineId == LineId)
            && (ModelId == null || ModelId == 0 || ws.DescriptionId == ModelId)
            && (string.IsNullOrEmpty(lessOrEq) || (ws.StockInQty - ws.StockOutQty) <= Convert.ToInt32(lessOrEq))
            ).OrderByDescending(s => s.StockInfoId).ToList();

            // Pagination //
            ViewBag.PagerData = GetPagerData(dto.Count(), pageSize, page);
            dto = dto.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            //-----------------//

            List<ProductionStockInfoViewModel> productionStockInfoViews = new List<ProductionStockInfoViewModel>();
            AutoMapper.Mapper.Map(dto, productionStockInfoViews);
            return PartialView("_productionStockInfoList", productionStockInfoViews);
        }

        public ActionResult GetProductionStockDetailInfoList(string flag, long? lineId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string stockStatus, string fromDate, string toDate, string refNum, int page = 1)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ListOfLine = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(l => new SelectListItem
                {
                    Text = l.LineNumber,
                    Value = l.LineId.ToString()
                }).ToList();

                ViewBag.ListOfWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(w => new SelectListItem
                {
                    Text = w.WarehouseName,
                    Value = w.Id.ToString()
                }).ToList();

                ViewBag.ddlStockStatus = Utility.ListOfStockStatus().Select(s => new SelectListItem() { Text = s.text, Value = s.value }).ToList();

                ViewBag.ddlModelName = _descriptionBusiness.GetAllDescriptionsInProductionStock(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();
                return View();
            }
            else
            {
                var dto = _productionStockDetailBusiness.GetProductionStockDetailInfoList(lineId, modelId, warehouseId, itemTypeId, itemId, stockStatus, fromDate, toDate, refNum, User.OrgId).OrderByDescending(s => s.StockDetailId).ToList();

                ViewBag.PagerData = GetPagerData(dto.Count(), 15, page);
                dto = dto.Skip((page - 1) * 15).Take(15).ToList();
                IEnumerable<ProductionStockDetailInfoListViewModel> viewModels = new List<ProductionStockDetailInfoListViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetProductionStockDetailInfoList", viewModels);
            }
        }

        // Transfer stock to Assembly
        public ActionResult GetFloorStockTransferList(string flag, long? floorId, string transferFor, long? repairLineId, long? assemblyId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string status, string transferCode, string fromDate, string toDate, long? transferInfoId, int page = 1)
        {
            //ViewBag.UserPrivilege = UserPrivilege("Production", "GetFloorStockTransferList");
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlStateStatus = Utility.ListOfReqStatus().Where(s => s.value == RequisitionStatus.Approved || s.value == RequisitionStatus.Accepted).Select(st => new SelectListItem
                {
                    Text = st.text,
                    Value = st.value
                }).ToList();

                ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

                ViewBag.ddlModelName = _descriptionBusiness.GetAllDescriptionsInProductionStock(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();

                ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(ware => new SelectListItem
                {
                    Text = ware.WarehouseName,
                    Value = ware.Id.ToString()
                }).ToList();

                return View();
            }
            else if (flag.Trim().ToLower() == Flag.View.ToLower() || flag.Trim().ToLower() == Flag.Search.ToLower())
            {
                //
                var dto = _transferStockToAssemblyInfoBusiness.GetFloorStockTransferInfobyQuery(floorId, transferFor, repairLineId, assemblyId, modelId, warehouseId, itemTypeId, itemId, status, transferCode, fromDate, toDate, User.OrgId);

                // Pagination //
                ViewBag.PagerData = GetPagerData(dto.Count(), pageSize, page);
                dto = dto.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                //-----------------//

                IEnumerable<TransferStockToAssemblyInfoViewModel> viewModels = new List<TransferStockToAssemblyInfoViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetFloorStockTransferList", viewModels);
            }
            else
            {
                // Details Part

                //TransferStockToAssemblyInfoDTO info;
                //List<TransferStockToAssemblyDetailDTO> details;
                //ProductionTransferInfoAndDetail(User.OrgId, transferInfoId.Value, out info, out details);

                var dto = _transferStockToAssemblyInfoBusiness.GetTransferStockToAssemblyInfoAndDetailsByQuery(transferInfoId.Value, User.OrgId);
                TransferStockToAssemblyInfoViewModel viewModel = new TransferStockToAssemblyInfoViewModel();
                AutoMapper.Mapper.Map(dto, viewModel);
                return PartialView("_GetFloorStockTransferDetail", viewModel);
            }
        }

        //==================//s
        // Obsolete Action
        public ActionResult CreateFloorStockTransfer()
        {
            ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

            ViewBag.ddlModelName = _descriptionBusiness.GetAllDescriptionsInProductionStock(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();

            ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(ware => new SelectListItem
            {
                Text = ware.WarehouseName,
                Value = ware.Id.ToString()
            }).ToList();
            return View();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveTransferStockAssembly(TransferStockToAssemblyInfoViewModel info, List<TransferStockToAssemblyDetailViewModel> details)
        {
            bool IsSuccess = false;
            //var privilege = UserPrivilege("Production", "GetFloorStockTransferList");
            //var permission = (info.TSAInfoId == 0 && privilege.Add) || (info.TSAInfoId > 0 && privilege.Edit); //&& permission
            if (ModelState.IsValid && details.Count() > 0)
            {
                TransferStockToAssemblyInfoDTO dtoInfo = new TransferStockToAssemblyInfoDTO();
                List<TransferStockToAssemblyDetailDTO> dtoDetail = new List<TransferStockToAssemblyDetailDTO>();
                AutoMapper.Mapper.Map(info, dtoInfo);
                AutoMapper.Mapper.Map(details, dtoDetail);
                IsSuccess = _transferStockToAssemblyInfoBusiness.SaveTransferStockAssembly(dtoInfo, dtoDetail, User.UserId, User.OrgId);
            }
            return Json(IsSuccess);
        }

        [NonAction]
        private IEnumerable<TransferStockToAssemblyInfoDTO> ProductionTransferList(long orgId, long? lineId, long? assemblyId, long? modelId, long? warehouseId, string status, string transferCode, string fromDate, string toDate)
        {
            var dto = _transferStockToAssemblyInfoBusiness.GetStockToAssemblyInfos(orgId).Select(t => new TransferStockToAssemblyInfoDTO
            {
                TSAInfoId = t.TSAInfoId,
                LineId = t.LineId,
                LineNumber = _productionLineBusiness.GetProductionLineOneByOrgId(t.LineId.Value, User.OrgId).LineNumber,
                AssemblyId = t.AssemblyId,
                AssemblyName = _assemblyLineBusiness.GetAssemblyLineById(t.AssemblyId.Value, User.OrgId).AssemblyLineName,
                DescriptionId = t.DescriptionId,
                ModelName = _descriptionBusiness.GetDescriptionOneByOrdId(t.DescriptionId.Value, User.OrgId).DescriptionName,
                WarehouseId = t.WarehouseId,
                WarehouseName = _warehouseBusiness.GetWarehouseOneByOrgId(t.WarehouseId.Value, User.OrgId).WarehouseName,
                ItemTypeId = t.ItemTypeId,
                ItemTypeName = _itemTypeBusiness.GetItemTypeOneByOrgId(t.ItemTypeId.Value, t.WarehouseId.Value, User.OrgId).ItemName,
                ItemId = t.ItemId,
                ItemName = _itemBusiness.GetItemOneByOrgId(t.ItemId.Value, User.OrgId).ItemName,
                ForQty = t.ForQty.Value,
                EntryUser = UserForEachRecord(t.EUserId.Value).UserName,
                UpdateUser = (t.UpUserId == null || t.UpUserId == 0) ? "" : UserForEachRecord(t.UpUserId.Value).UserName,
                EntryDate = t.EntryDate,
                Remarks = t.Remarks,
                TransferCode = t.TransferCode,
                StateStatus = t.StateStatus,
                ItemCount = _transferStockToAssemblyDetailBusiness.GetTransferStockToAssemblyDetailByInfo(t.TSAInfoId, User.OrgId).Count()
            }).AsEnumerable();

            dto = dto.Where(t =>
            (assemblyId == null || assemblyId <= 0 || t.AssemblyId == assemblyId) &&
            (modelId == null || modelId <= 0 || t.DescriptionId == modelId) &&
            (lineId == null || lineId <= 0 || t.LineId == lineId) &&
            (warehouseId == null || warehouseId <= 0 || t.WarehouseId == warehouseId) &&
            (transferCode == null || transferCode.Trim() == "" || t.TransferCode.Contains(transferCode)) &&
            (status == null || status.Trim() == "" || t.StateStatus == status.Trim()) &&
            (
                (fromDate == null && toDate == null)
                ||
                 (fromDate == "" && toDate == "")
                ||
                (fromDate.Trim() != "" && toDate.Trim() != "" &&

                    t.EntryDate.Value.Date >= Convert.ToDateTime(fromDate).Date &&
                    t.EntryDate.Value.Date <= Convert.ToDateTime(toDate).Date)
                ||
                (fromDate.Trim() != "" && t.EntryDate.Value.Date == Convert.ToDateTime(fromDate).Date)
                ||
                (toDate.Trim() != "" && t.EntryDate.Value.Date == Convert.ToDateTime(toDate).Date)
            )).OrderByDescending(t => t.EntryDate).ToList();

            return dto;
        }

        [NonAction]
        private void ProductionTransferInfoAndDetail(long orgId, long transferId, out TransferStockToAssemblyInfoDTO info, out List<TransferStockToAssemblyDetailDTO> detail)
        {
            var infoDomain = _transferStockToAssemblyInfoBusiness.GetStockToAssemblyInfoById(transferId, User.OrgId);

            info = new TransferStockToAssemblyInfoDTO
            {
                AssemblyName = _assemblyLineBusiness.GetAssemblyLineById(infoDomain.AssemblyId.Value, User.OrgId).AssemblyLineName,
                TSAInfoId = infoDomain.TSAInfoId,
                TransferCode = infoDomain.TransferCode,
                ModelName = _descriptionBusiness.GetDescriptionOneByOrdId(infoDomain.DescriptionId.Value, User.OrgId).DescriptionName,
                LineNumber = _productionLineBusiness.GetProductionLineOneByOrgId(infoDomain.LineId.Value, User.OrgId).LineNumber,
                WarehouseName = _warehouseBusiness.GetWarehouseOneByOrgId(infoDomain.WarehouseId.Value, User.OrgId).WarehouseName,
                ItemTypeName = _itemTypeBusiness.GetItemTypeOneByOrgId(infoDomain.ItemTypeId.Value, infoDomain.WarehouseId.Value, User.OrgId).ItemName,
                ItemName = _itemBusiness.GetItemOneByOrgId(infoDomain.ItemId.Value, User.OrgId).ItemName,
                StateStatus = infoDomain.StateStatus,
                ForQty = infoDomain.ForQty
            };

            detail = _transferStockToAssemblyDetailBusiness.GetTransferStockToAssemblyDetailByInfo(transferId, User.OrgId)
                .Select(s => new TransferStockToAssemblyDetailDTO
                {
                    WarehouseName = _warehouseBusiness.GetWarehouseOneByOrgId(s.WarehouseId.Value, User.OrgId).WarehouseName,
                    ItemTypeName = _itemTypeBusiness.GetItemTypeOneByOrgId(s.ItemTypeId.Value, s.WarehouseId.Value, User.OrgId).ItemName,
                    ItemName = _itemBusiness.GetItemOneByOrgId(s.ItemId.Value, User.OrgId).ItemName,
                    Quantity = s.Quantity,
                    UnitName = _unitBusiness.GetUnitOneByOrgId(s.UnitId.Value, User.OrgId).UnitSymbol,
                    Remarks = s.Remarks
                }).ToList();
        }

        public ActionResult GetRepairSectionRequisitionChecking(string flag, long? repairLineId, long? packagingLineId, long? modelId, long? warehouseId, string status, string requisitionCode, string fromDate, string toDate, long? reqInfoId,string reqFor, int page = 1)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.WarehouseName, Value = line.Id.ToString() }).ToList();

                ViewBag.ddlRepairLine = _repairLineBusiness.GetRepairLineWithFloor(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();

                ViewBag.ddlModelName = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).Select(des => new SelectListItem { Text = des.DescriptionName, Value = des.DescriptionId.ToString() }).ToList();

                ViewBag.ddlStateStatus = Utility.ListOfReqStatus().Select(st => new SelectListItem
                {
                    Text = st.text,
                    Value = st.value
                }).ToList();

                ViewBag.ddlPackagingLine = _packagingLineBusiness.GetPackagingLinesWithProduction(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value.ToString() }).ToList();

                return View();
            }
            else if (!string.IsNullOrEmpty(flag) && (flag.Trim().ToLower() == Flag.Search.ToLower() || flag.Trim().ToLower() == Flag.View.ToLower()))
            {
                var dto = _repairSectionRequisitionInfoBusiness.GetRepairSectionRequisitionInfoList(repairLineId, packagingLineId, modelId, warehouseId, status, requisitionCode, fromDate, toDate, "Production",string.Empty, User.OrgId);
                // Pagination //
                ViewBag.PagerData = GetPagerData(dto.Count(), pageSize, page);
                dto = dto.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                //-----------------//
                List<RepairSectionRequisitionInfoViewModel> viewModels = new List<RepairSectionRequisitionInfoViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetRepairSectionRequisitionChecking", viewModels);
            }
            else
            {
                var info = _repairSectionRequisitionInfoBusiness.GetRepairSectionRequisitionById(reqInfoId.Value, User.OrgId);

                var detailDto = _repairSectionRequisitionDetailBusiness.GetRepairSectionRequisitionDetailByInfoId(reqInfoId.Value, User.OrgId).Select(d => new RepairSectionRequisitionDetailDTO
                {
                    RSRDetailId = d.RSRDetailId,
                    ItemName = d.ItemName,
                    ItemTypeName = d.ItemTypeName,
                    RequestQty = d.RequestQty,
                    IssueQty = d.IssueQty,
                    UnitName = d.UnitName
                }).ToList();

                var infoDTO = new RepairSectionRequisitionInfoDTO
                {
                    RSRInfoId = info.RSRInfoId,
                    RequisitionCode = info.RequisitionCode,
                    RepairLineName = info.RepairLineName + " [" + info.ProductionFloorName + "]",
                    PackagingLineName = info.PackagingLineName + " [" + info.ProductionFloorName + "]",
                    ModelName = info.ModelName,
                    WarehouseName = info.WarehouseName,
                    StateStatus = info.StateStatus,
                    ReqFor = info.ReqFor
                };

                IEnumerable<RepairSectionRequisitionDetailViewModel> detailViewModels = new List<RepairSectionRequisitionDetailViewModel>();
                RepairSectionRequisitionInfoViewModel infoViewModel = new RepairSectionRequisitionInfoViewModel();
                AutoMapper.Mapper.Map(detailDto, detailViewModels);
                AutoMapper.Mapper.Map(infoDTO, infoViewModel);

                ViewBag.Info = infoViewModel;

                return PartialView("_GetRepairSectionRequisitionCheckingDetail", detailViewModels);
            }
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveRepairSectionRequisitionStateChecking(long reqInfoId, string status)
        {
            bool IsSuccess = false;
            if (reqInfoId > 0 && !string.IsNullOrEmpty(status))
            {
                IsSuccess = _repairSectionRequisitionInfoBusiness.SaveRepairSectionRequisitionStatus(reqInfoId, status, User.OrgId, User.UserId);
            }
            return Json(IsSuccess);
        }
        public ActionResult GetRepairSectionFaultyItemTransferReceive(string flag, long? floorId, long? repairLineId, string transferCode, string status, string fromDate, string toDate, long? transferId, int page = 1)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlProductionFloor = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();
                ViewBag.ddlStateStatus = Utility.ListOfReqStatus().Where(s => s.text == RequisitionStatus.Accepted || s.text == RequisitionStatus.Approved).Select(st => new SelectListItem
                {
                    Text = st.text,
                    Value = st.value
                }).ToList();
                return View();
            }
            else if (!string.IsNullOrEmpty(flag) && (flag.Trim().ToLower() == Flag.Search.ToLower() || flag.Trim().ToLower() == Flag.View.ToLower()))
            {
                var dto = _repairSectionFaultyItemTransferInfoBusiness.GetRepairSectionFaultyItemTransferInfoList(floorId, repairLineId, transferCode, status, fromDate, toDate, User.OrgId);

                // Pagination //
                ViewBag.PagerData = GetPagerData(dto.Count(), pageSize, page);
                dto = dto.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                //-----------------//

                List<RepairSectionFaultyItemTransferInfoViewModel> viewModels = new List<RepairSectionFaultyItemTransferInfoViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetRepairSectionFaultyItemTransferList", viewModels);
            }
            else
            {
                // Details
                var info = _repairSectionFaultyItemTransferInfoBusiness.GetRepairSectionFaultyTransferInfoById(transferId.Value, User.OrgId);
                var detailDTO = _repairSectionFaultyItemTransferDetailBusiness.GetRepairSectionFaultyItemTransferDetailByInfo(transferId.Value, User.OrgId).Select(s => new RepairSectionFaultyItemTransferDetailDTO()
                {
                    ProductionFloorName = s.ProductionFloorName,
                    RepairLineName = s.RepairLineName,
                    ModelName = s.ModelName,
                    WarehouseName = s.WarehouseName,
                    ItemTypeName = s.ItemTypeName,
                    ItemName = s.ItemName,
                    UnitName = s.UnitName,
                    FaultyQty = s.FaultyQty,
                    TransferCode = s.TransferCode,
                    ReferenceNumber = info.TransferCode
                }).ToList();

                RepairSectionFaultyItemTransferInfoDTO infoDto = new RepairSectionFaultyItemTransferInfoDTO
                {
                    RSFIRInfoId = info.RSFIRInfoId,
                    ProductionFloorName = info.ProductionFloorName,
                    RepairLineName = info.RepairLineName,
                    TransferCode = info.TransferCode,
                    StateStatus = info.StateStatus,
                    EntryUser = UserForEachRecord(info.EUserId.Value).UserName,
                    EntryDate = info.EntryDate.Value
                };
                RepairSectionFaultyItemTransferInfoViewModel viewModelInfo = new RepairSectionFaultyItemTransferInfoViewModel();
                IEnumerable<RepairSectionFaultyItemTransferDetailViewModel> viewModelDetail = new List<RepairSectionFaultyItemTransferDetailViewModel>();

                AutoMapper.Mapper.Map(infoDto, viewModelInfo);
                AutoMapper.Mapper.Map(detailDTO, viewModelDetail);

                ViewBag.Info = viewModelInfo;
                return PartialView("_GetRepairSectionFaultyItemTransferDetail", viewModelDetail);
            }

        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveRepairSectionFaultyItemReceive(long transferId, string status)
        {

            bool IsSuccess = false;
            if (transferId > 0 && !string.IsNullOrEmpty(status) && status == RequisitionStatus.Accepted)
            {
                IsSuccess = _productionFaultyStockDetailBusiness.StockInByRepairSection(transferId, status, User.OrgId, User.UserId);
            }
            return Json(IsSuccess);

        }

        public ActionResult GetProductionFaultyStockInfo(string flag, long? floorId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string lessOrEq, int page = 1)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

                ViewBag.ddlModelName = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).Select(des => new SelectListItem { Text = des.DescriptionName, Value = des.DescriptionId.ToString() }).OrderBy(s => s.Text).ToList();

                ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(ware => new SelectListItem
                {
                    Text = ware.WarehouseName,
                    Value = ware.Id.ToString()
                }).ToList();

                ViewBag.ddlStateStatus = Utility.ListOfReqStatus().Where(s => s.text == RequisitionStatus.Accepted || s.text == RequisitionStatus.Approved).Select(st => new SelectListItem
                {
                    Text = st.text,
                    Value = st.value
                }).ToList();

                return View();
            }
            else
            {
                var dto = _productionFaultyStockInfoBusiness.GetProductionFaultyStockInfosByQuery(floorId, modelId, warehouseId, itemTypeId, itemId, lessOrEq, User.OrgId);

                ViewBag.PagerData = GetPagerData(dto.Count(), 15, page);
                dto = dto.Skip((page - 1) * 15).Take(15).ToList();
                IEnumerable<ProductionFaultyStockInfoViewModel> viewModels = new List<ProductionFaultyStockInfoViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);

                return PartialView("_GetProductionFaultyStockInfo", viewModels);
            }

        }

        // Production Assemble Stock Info
        public ActionResult GetProductionAssembleItemStockInfo(string flag, long? floorId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string lessOrEq)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

                ViewBag.ddlModelName = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).Select(des => new SelectListItem { Text = des.DescriptionName, Value = des.DescriptionId.ToString() }).ToList();

                ViewBag.ddlItems = _productionAssembleStockInfoBusiness.GetAllItemsInStock(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();

                ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(ware => new SelectListItem
                {
                    Text = ware.WarehouseName,
                    Value = ware.Id.ToString()
                }).ToList();

                ViewBag.ddlStateStatus = Utility.ListOfReqStatus().Where(s => s.value == RequisitionStatus.Approved || s.value == RequisitionStatus.Accepted).Select(st => new SelectListItem
                {
                    Text = st.text,
                    Value = st.value
                }).ToList();
                return View();
            }
            else
            {
                //GetProductionAssembleStockInfoByQuery
                var dto = _productionAssembleStockInfoBusiness.GetProductionAssembleStockInfoByQuery(floorId, modelId, warehouseId, itemTypeId, itemId, lessOrEq, User.OrgId);

                //.GetProductionAssembleStockInfos(User.OrgId).Select(s => new ProductionAssembleStockInfoDTO
                //{
                //    ProductionFloorId = s.ProductionFloorId,
                //    ProductionFloorName = _productionLineBusiness.GetProductionLineOneByOrgId(s.ProductionFloorId, User.OrgId).LineNumber,
                //    DescriptionId = s.DescriptionId,
                //    ModelName = _descriptionBusiness.GetDescriptionOneByOrdId(s.DescriptionId, User.OrgId).DescriptionName,
                //    WarehouseId = s.WarehouseId,
                //    WarehouseName = _warehouseBusiness.GetWarehouseOneByOrgId(s.WarehouseId, User.OrgId).WarehouseName,
                //    ItemTypeId = s.ItemTypeId,
                //    ItemTypeName = _itemTypeBusiness.GetItemTypeOneByOrgId(s.ItemTypeId, s.WarehouseId, User.OrgId).ItemName,
                //    ItemId = s.ItemId,
                //    ItemName = _itemBusiness.GetItemOneByOrgId(s.ItemId, User.OrgId).ItemName,
                //    StockInQty = s.StockInQty,
                //    StockOutQty = s.StockOutQty,
                //    UnitId = s.UnitId,
                //    UnitName = _unitBusiness.GetUnitOneByOrgId(s.UnitId, User.OrgId).UnitSymbol
                //}).ToList();

                // dto = dto.Where(t =>
                //(floorId == null || floorId <= 0 || t.ProductionFloorId == floorId) &&
                //(modelId == null || modelId <= 0 || t.DescriptionId == modelId) &&
                //(warehouseId == null || warehouseId <= 0 || t.WarehouseId == warehouseId) &&
                //(itemTypeId == null || itemTypeId <= 0 || t.ItemTypeId == itemTypeId) &&
                //(itemId == null || itemId <= 0 || t.ItemId == itemId) &&
                //(string.IsNullOrEmpty(lessOrEq) || (t.StockInQty - t.StockOutQty) <= Convert.ToInt32(lessOrEq))
                //).OrderByDescending(t => t.EntryDate).ToList();

                // Pagination //
                //ViewBag.PagerData = GetPagerData(dto.Count(), pageSize, page);
                //dto = dto.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                //-----------------//

                List<ProductionAssembleStockInfoViewModel> viewModels = new List<ProductionAssembleStockInfoViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetProductionAssembleItemStockInfo", viewModels);
            }
        }
        // MiniStock Transfer //
        public ActionResult CreateMiniStockTransfer()
        {
            ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

            ViewBag.ddlPackagingLine = _packagingLineBusiness.GetPackagingLinesWithProduction(User.OrgId).Select(line => new SelectListItem { Text = line.text, Value = line.value.ToString() }).ToList();

            ViewBag.ddlModelName = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).Select(des => new SelectListItem { Text = des.DescriptionName, Value = des.DescriptionId.ToString() }).ToList();

            ViewBag.ddlItem = _productionAssembleStockInfoBusiness.GetAllItemsInStock(User.OrgId).Select(ware => new SelectListItem
            {
                Text = ware.text,
                Value = ware.value
            }).ToList();
            return View();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveMiniStockTransfer(MiniStockTransferInfoViewModel model)
        {
            bool IsSuccess = false;
            if (ModelState.IsValid)
            {
                MiniStockTransferInfoDTO dto = new MiniStockTransferInfoDTO();
                AutoMapper.Mapper.Map(model, dto);
                IsSuccess = _miniStockTransferInfoBusiness.SaveMiniStockTransfer(dto, User.UserId, User.OrgId);
            }
            return Json(IsSuccess);
        }

        public ActionResult GetMiniStockTransferInfos(string flag, long? floorId, long? packagingLineId, string status, string fromDate, string toDate, string transferCode, long? transferId)
        {
            if (!string.IsNullOrEmpty(flag) && flag.Trim().ToLower() == "view")
            {
                var dto = _miniStockTransferInfoBusiness.GetMiniStockTransferInfosByQuery(packagingLineId, floorId, 0, transferCode, status, fromDate, toDate, User.OrgId);
                List<MiniStockTransferInfoViewModel> viewModels = new List<MiniStockTransferInfoViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetMiniStockTransferInfo", viewModels);
            }
            else if (!string.IsNullOrEmpty(flag) && flag.Trim().ToLower() == "detail")
            {
                var dto = _miniStockTransferInfoBusiness.GetMiniStockTransferInfosByQuery(0, 0, transferId, string.Empty, string.Empty, string.Empty, string.Empty, User.OrgId).FirstOrDefault();

                dto.MiniStockTransferDetails = _miniStockTransferDetailBusiness.GetMiniStockTransfersDetailByQuery(null, null, null, null, transferId, User.OrgId).ToList();

                MiniStockTransferInfoViewModel viewModel = new MiniStockTransferInfoViewModel();
                AutoMapper.Mapper.Map(dto, viewModel);
                return PartialView("_GetMiniStockTransferDetail", viewModel);
            }
            return View();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveMiniTrasferStatus(long transferId, string status)
        {
            bool IsSuccess = false;
            if (transferId > 0 && !string.IsNullOrEmpty(status) && status.Trim() != "")
            {
                if (status == "Send")
                {
                    IsSuccess = _miniStockTransferInfoBusiness.SaveMiniStockTranferStatus(transferId, status, User.UserId, User.OrgId);
                }
                if (status == FinishGoodsSendStatus.Received)
                {
                    IsSuccess = _packagingItemStockDetailBusiness.SavePackagingItemStockInByMiniStockTransfer(transferId, status, User.UserId, User.OrgId);
                }
            }
            return Json(IsSuccess);
        }

        // Packaging Stock Transfer
        public ActionResult CreateProductinAssembleStockTransfer()
        {
            ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

            ViewBag.ddlModelName = _descriptionBusiness.GetAllDescriptionsInProductionStock(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();

            ViewBag.ddlItem = _productionAssembleStockInfoBusiness.GetAllItemsInStock(User.OrgId).Select(ware => new SelectListItem
            {
                Text = ware.text,
                Value = ware.value
            }).ToList();
            return View();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveProductinAssembleStockTransfer(ProductionToPackagingStockTransferInfoViewModel info)
        {
            bool IsSuccess = false;
            if (ModelState.IsValid && info.ProductionToPackagingStockTransferDetails.Count > 0)
            {
                ProductionToPackagingStockTransferInfoDTO dto = new ProductionToPackagingStockTransferInfoDTO();
                AutoMapper.Mapper.Map(info, dto);
                IsSuccess = _productionToPackagingStockTransferInfoBusiness.SaveProductionToPackagingStockTransfer(dto, User.UserId, User.OrgId);
            }
            return Json(IsSuccess);
        }

        public ActionResult GetProductionAssembleStockTransferList(string flag, long? floorId, long? packagingLineId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, long? transferInfoId, string status)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlStateStatus = Utility.ListOfReqStatus().Where(s => s.value == RequisitionStatus.Approved || s.value == RequisitionStatus.Accepted).Select(st => new SelectListItem
                {
                    Text = st.text,
                    Value = st.value
                }).ToList();

                ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

                ViewBag.ddlModelName = _descriptionBusiness.GetAllDescriptionsInProductionStock(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();

                ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(ware => new SelectListItem
                {
                    Text = ware.WarehouseName,
                    Value = ware.Id.ToString()
                }).ToList();

                return View();
            }
            else if (!string.IsNullOrEmpty(flag) && flag.ToLower() == Flag.View.ToLower() || flag.ToLower() == Flag.Search.ToLower())
            {
                var dto = _productionToPackagingStockTransferInfoBusiness.GetProductionToPackagingStockTransferInfosByQuery(floorId, packagingLineId, modelId, warehouseId, itemTypeId, itemId, status, User.OrgId);

                // Pagination //
                //ViewBag.PagerData = GetPagerData(dto.Count(), pageSize, page);
                //dto = dto.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                //-----------------//
                List<ProductionToPackagingStockTransferInfoViewModel> viewModels = new List<ProductionToPackagingStockTransferInfoViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);

                return PartialView("_GetProductionAssembleStockTransferList", viewModels);
            }
            else
            {
                var dto = _productionToPackagingStockTransferDetailBusiness.GetProductionToPackagingStockTransferDetailsByQuery(transferInfoId.Value, User.OrgId);
                List<ProductionToPackagingStockTransferDetailViewModel> viewModels = new List<ProductionToPackagingStockTransferDetailViewModel>();
                ViewBag.Status = _productionToPackagingStockTransferInfoBusiness.GetProductionToPackagingStockTransferInfoById(transferInfoId.Value, User.OrgId).StateStatus;

                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetProductionAssembleStockTransferListDetail", viewModels);
            }
        }

        // Received Production Stocks by Assembly/Repair
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveProductionStockReceiveInByAssemblyOrRepair(long transferId, string status)
        {
            bool IsSuccess = false;
            ///var privilege = UserPrivilege("Production", "GetReceiveStockFromFloor");
            if (transferId > 0 && !string.IsNullOrEmpty(status) && status.Trim() != "" && status == RequisitionStatus.Accepted)
            {
                IsSuccess = _transferStockToAssemblyInfoBusiness.SaveProductionStockReceiveInByAssemblyOrRepair(transferId, status, User.OrgId, User.UserId);
            }
            return Json(IsSuccess);
        }



        #endregion

        #region Item Return
        public ActionResult GetItemReturnList(string flag, string code, long? lineId, long? warehouseId, string status, string returnType, string faultyCase, string fromDate, string toDate, long? modelId, int page = 1)
        {
            ViewBag.UserPrivilege = UserPrivilege("Production", "GetItemReturnList");
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ReturnType = Utility.ListOfReturnType().Select(s => new SelectListItem() { Text = s.text, Value = s.value }).ToList();

                ViewBag.FaultyCase = Utility.ListOfFaultyCase().Select(s => new SelectListItem() { Text = s.text, Value = s.value }).ToList();

                ViewBag.ListOfLine = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(l => new SelectListItem
                {
                    Text = l.LineNumber,
                    Value = l.LineId.ToString()
                }).ToList();

                ViewBag.ListOfWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(w => new SelectListItem
                {
                    Text = w.WarehouseName,
                    Value = w.Id.ToString()
                }).ToList();

                ViewBag.Status = Utility.ListOfReqStatus().Where(s => s.text == RequisitionStatus.Approved || s.text == RequisitionStatus.Accepted || s.text == RequisitionStatus.Canceled).Select(s => new SelectListItem() { Text = s.text, Value = s.value }).ToList();

                ViewBag.ddlModelName = _descriptionBusiness.GetAllDescriptionsInProductionStock(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();

                return View();
            }
            else
            {
                var warehouses = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).ToList();
                var lines = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).ToList();
                var descriptions = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).ToList();
                IEnumerable<ItemReturnInfoDTO> dto = _itemReturnInfoBusiness.GetItemReturnInfos(User.OrgId).Where(i => 1 == 1
                && (code == null || code.Trim() == "" || i.IRCode.Contains(code))
                && (lineId == null || lineId <= 0 || i.LineId == lineId)
                && (warehouseId == null || warehouseId <= 0 || i.WarehouseId == warehouseId)
                && (modelId == null || modelId <= 0 || i.DescriptionId == modelId)
                && (status == null || status.Trim() == "" || i.StateStatus == status.Trim())
                && (returnType == null || returnType.Trim() == "" || i.ReturnType == returnType.Trim())
                && (faultyCase == null || faultyCase.Trim() == "" || i.FaultyCase == faultyCase.Trim())
                &&
                (
                    (fromDate == null && toDate == null)
                    ||
                     (fromDate == "" && toDate == "")
                    ||
                    (fromDate.Trim() != "" && toDate.Trim() != "" &&

                        i.EntryDate.Value.Date >= Convert.ToDateTime(fromDate).Date &&
                        i.EntryDate.Value.Date <= Convert.ToDateTime(toDate).Date)
                    ||
                    (fromDate.Trim() != "" && i.EntryDate.Value.Date == Convert.ToDateTime(fromDate).Date)
                    ||
                    (toDate.Trim() != "" && i.EntryDate.Value.Date == Convert.ToDateTime(toDate).Date)
                )
                ).Select(i => new ItemReturnInfoDTO
                {
                    IRInfoId = i.IRInfoId,
                    IRCode = i.IRCode,
                    ReturnType = i.ReturnType,
                    FaultyCase = i.FaultyCase,
                    LineId = lines.Where(l => l.LineId == i.LineId).FirstOrDefault().LineId,
                    LineNumber = lines.Where(l => l.LineId == i.LineId).FirstOrDefault().LineNumber,
                    WarehouseId = warehouses.Where(w => w.Id == i.WarehouseId).FirstOrDefault().Id,
                    WarehouseName = warehouses.Where(w => w.Id == i.WarehouseId).FirstOrDefault().WarehouseName,
                    Qty = _itemReturnDetailBusiness.GetItemReturnDetailsByReturnInfoId(User.OrgId, i.IRInfoId).Count(),
                    StateStatus = i.StateStatus,
                    EntryDate = i.EntryDate,
                    DescriptionId = i.DescriptionId,
                    Model = descriptions.FirstOrDefault(d => d.DescriptionId == i.DescriptionId).DescriptionName,
                    EntryUser = UserForEachRecord(i.EUserId.Value).UserName,
                    UpdateUser = (i.UpUserId == null || i.UpUserId == 0) ? "" : UserForEachRecord(i.UpUserId.Value).UserName
                }).OrderByDescending(s => s.IRInfoId).ToList();

                // Pagination //
                ViewBag.PagerData = GetPagerData(dto.Count(), pageSize, page);
                dto = dto.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                //-----------------//

                List<ItemReturnInfoViewModel> itemReturnInfoViewModels = new List<ItemReturnInfoViewModel>();
                AutoMapper.Mapper.Map(dto, itemReturnInfoViewModels);
                return PartialView("_GetItemReturnList", itemReturnInfoViewModels);
            }
        }

        [HttpGet]
        public ActionResult CreateFaultyGoodReturn()
        {
            ViewBag.ReturnType = Utility.ListOfReturnType().Select(s => new SelectListItem() { Text = s.text, Value = s.value }).ToList();

            ViewBag.FaultyCase = Utility.ListOfFaultyCase().Select(s => new SelectListItem() { Text = s.text, Value = s.value }).ToList();

            ViewBag.ListOfLine = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(l => new SelectListItem
            {
                Text = l.LineNumber,
                Value = l.LineId.ToString()
            }).ToList();

            ViewBag.ListOfWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(w => new SelectListItem
            {
                Text = w.WarehouseName,
                Value = w.Id.ToString()
            }).ToList();

            ViewBag.ddlModelName = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).Select(des => new SelectListItem { Text = des.DescriptionName, Value = des.DescriptionId.ToString() }).ToList();

            return View();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveFaultyItemOrGoodsReturn(ItemReturnInfoViewModel info, List<ItemReturnDetailViewModel> details)
        {
            bool IsSuccess = false;
            var privilege = UserPrivilege("Production", "GetItemReturnList");
            var permission = (info.IRInfoId == 0 && privilege.Add) || (info.IRInfoId > 0 && privilege.Edit);
            if (ModelState.IsValid && details.Count > 0 && permission)
            {
                var dtoInfo = new ItemReturnInfoDTO();
                AutoMapper.Mapper.Map(info, dtoInfo);
                dtoInfo.OrganizationId = User.OrgId;
                dtoInfo.EUserId = User.UserId;
                var dtoDetail = new List<ItemReturnDetailDTO>();
                AutoMapper.Mapper.Map(details, dtoDetail);
                IsSuccess = _itemReturnInfoBusiness.SaveFaultyItemOrGoodsReturn(dtoInfo, dtoDetail);
            }
            return Json(IsSuccess);
        }

        public ActionResult GetProductionItemReturnDetails(long itemReturnInfoId)
        {
            var items = _itemBusiness.GetAllItemByOrgId(User.OrgId);
            var itemTypes = _itemTypeBusiness.GetAllItemTypeByOrgId(User.OrgId);
            var units = _unitBusiness.GetAllUnitByOrgId(User.OrgId);
            IEnumerable<ItemReturnDetailDTO> itemReturnDetailDTOs = _itemReturnDetailBusiness.GetItemReturnDetailsByReturnInfoId(User.OrgId, itemReturnInfoId).Select(s => new ItemReturnDetailDTO
            {
                IRDetailId = s.IRDetailId,
                ItemTypeId = s.ItemTypeId,
                ItemTypeName = itemTypes.FirstOrDefault(i => i.ItemId == s.ItemTypeId).ItemName,
                ItemId = s.ItemId,
                ItemName = items.FirstOrDefault(i => i.ItemId == s.ItemId).ItemName,
                UnitId = s.UnitId,
                UnitName = units.FirstOrDefault(i => i.UnitId == s.UnitId).UnitName,
                Quantity = s.Quantity,
                Remarks = s.Remarks
            }).OrderByDescending(s => s.IRDetailId).ToList();

            var info = _itemReturnInfoBusiness.GetItemReturnInfo(User.OrgId, itemReturnInfoId);
            ItemReturnInfoViewModel itemReturnInfoViewModel = new ItemReturnInfoViewModel()
            {
                IRCode = info.IRCode,
                LineNumber = _productionLineBusiness.GetProductionLineOneByOrgId(info.LineId.Value, User.OrgId).LineNumber,
                WarehouseName = _warehouseBusiness.GetWarehouseOneByOrgId(info.WarehouseId.Value, User.OrgId).WarehouseName
            };

            ViewBag.ReturnInfoViewModel = itemReturnInfoViewModel;

            IEnumerable<ItemReturnDetailViewModel> itemReturnDetailViews = new List<ItemReturnDetailViewModel>();
            AutoMapper.Mapper.Map(itemReturnDetailDTOs, itemReturnDetailViews);
            return PartialView("_GetProductionItemReturnDetails", itemReturnDetailViews);
        }

        public ActionResult GetProductionFaultyOrReturnItemDetailList(string flag, string refNum, string returnType, string faultyCase, long? lineId, long? warehouseId, string status, long? itemTypeId, long? itemId, string fromDate, string toDate, long? modelId, int page = 1)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ReturnType = Utility.ListOfReturnType().Select(s => new SelectListItem() { Text = s.text, Value = s.value }).ToList();

                ViewBag.FaultyCase = Utility.ListOfFaultyCase().Select(s => new SelectListItem() { Text = s.text, Value = s.value }).ToList();

                ViewBag.ListOfLine = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(l => new SelectListItem
                {
                    Text = l.LineNumber,
                    Value = l.LineId.ToString()
                }).ToList();

                ViewBag.ListOfWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(w => new SelectListItem
                {
                    Text = w.WarehouseName,
                    Value = w.Id.ToString()
                }).ToList();

                ViewBag.Status = Utility.ListOfReqStatus().Where(s => s.text == RequisitionStatus.Approved || s.text == RequisitionStatus.Accepted || s.text == RequisitionStatus.Canceled).Select(s => new SelectListItem() { Text = s.text, Value = s.value }).ToList();

                ViewBag.ddlModelName = _descriptionBusiness.GetAllDescriptionsInProductionStock(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();

                return View();
            }
            else
            {
                IEnumerable<ItemReturnDetailListDTO> dto = _itemReturnDetailBusiness.GetItemReturnDetailList(refNum, returnType, faultyCase, lineId, warehouseId, status, itemTypeId, itemId, fromDate, toDate, modelId, User.OrgId).OrderByDescending(s => s.IRDetailId).ToList();

                // Pagination //
                ViewBag.PagerData = GetPagerData(dto.Count(), pageSize, page);
                dto = dto.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                //-----------------//

                IEnumerable<ItemReturnDetailListViewModel> listViewModels = new List<ItemReturnDetailListViewModel>();
                AutoMapper.Mapper.Map(dto, listViewModels);
                return PartialView("_GetProductionFaultyOrReturnItemDetailList", listViewModels);
            }
        }
        #endregion

        #region Finish Goods Stock

        [HttpGet]
        public ActionResult CreateFinishGoods()
        {
            ViewBag.ddlProductionLines = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(l => new SelectListItem
            {
                Text = l.LineNumber,
                Value = l.LineId.ToString()
            }).ToList();

            ViewBag.ddlModel = _descriptionBusiness.GetAllDescriptionsInProductionStock(User.OrgId).Select(des => new SelectListItem
            {
                Text = des.text,
                Value = des.value
            }).ToList();

            ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(ware => new SelectListItem
            {
                Text = ware.WarehouseName,
                Value = ware.Id.ToString()
            }).ToList();

            ViewBag.ddlProductionTypes = Utility.ListOfProductionType().Select(s => new SelectListItem
            {
                Text = s.text,
                Value = s.value
            }).ToList();

            return View();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveFinishGoods(FinishGoodsInfoViewModel info, List<FinishGoodsRowMaterialViewModel> details)
        {
            bool IsSucess = false;
            var privilege = UserPrivilege("Production", "GetFinishGoodsList");
            var permission = (info.FinishGoodsInfoId == 0 && privilege.Add) || (info.FinishGoodsInfoId > 0 && privilege.Edit);
            if (ModelState.IsValid && details.Count() > 0 && permission)
            {
                FinishGoodsInfoDTO finishGoodsInfoDTO = new FinishGoodsInfoDTO();
                List<FinishGoodsRowMaterialDTO> finishGoodsRowMaterialDTOs = new List<FinishGoodsRowMaterialDTO>();
                AutoMapper.Mapper.Map(info, finishGoodsInfoDTO);
                AutoMapper.Mapper.Map(details, finishGoodsRowMaterialDTOs);
                IsSucess = _finishGoodsInfoBusiness.SaveFinishGoods(finishGoodsInfoDTO, finishGoodsRowMaterialDTOs, User.UserId, User.OrgId);
            }
            return Json(IsSucess);
        }

        public ActionResult GetFinishGoodsList(string flag, long? lineId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string finishQty, string fromDate, string toDate, int page = 1)
        {
            ViewBag.UserPrivilege = UserPrivilege("Production", "GetFinishGoodsList");
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

                ViewBag.ddlModelName = _descriptionBusiness.GetAllDescriptionsInProductionStock(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();

                ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(ware => new SelectListItem
                {
                    Text = ware.WarehouseName,
                    Value = ware.Id.ToString()
                }).ToList();
                return View();
            }
            else
            {
                IEnumerable<FinishGoodsInfoDTO> dto = _finishGoodsInfoBusiness.GetFinishGoodsByOrg(User.OrgId).Select(info => new FinishGoodsInfoDTO
                {
                    FinishGoodsInfoId = info.FinishGoodsInfoId,
                    ProductionLineId = info.ProductionLineId,
                    LineNumber = _productionLineBusiness.GetProductionLineOneByOrgId(info.ProductionLineId, User.OrgId).LineNumber,
                    WarehouseId = info.WarehouseId,
                    WarehouseName = (_warehouseBusiness.GetWarehouseOneByOrgId(info.WarehouseId, User.OrgId).WarehouseName),
                    ItemTypeId = info.ItemTypeId,
                    ItemTypeName = (_itemTypeBusiness.GetItemType(info.ItemTypeId, User.OrgId).ItemName),
                    ItemId = info.ItemId,
                    ItemName = (_itemBusiness.GetItemOneByOrgId(info.ItemId, User.OrgId).ItemName),
                    UnitId = info.UnitId,
                    UnitName = (_unitBusiness.GetUnitOneByOrgId(info.UnitId, User.OrgId).UnitSymbol),
                    Quanity = info.Quanity,
                    OrganizationId = info.OrganizationId,
                    DescriptionId = info.DescriptionId,
                    ModelName = (_descriptionBusiness.GetDescriptionOneByOrdId(info.DescriptionId, info.OrganizationId).DescriptionName),
                    EntryDate = info.EntryDate
                }).AsEnumerable();

                dto = dto.Where(fg =>
               (warehouseId == null || warehouseId == 0 || fg.WarehouseId == warehouseId)
               && (itemTypeId == null || itemTypeId == 0 || fg.ItemTypeId == itemTypeId)
               && (itemId == null || itemId == 0 || fg.ItemId == itemId)
               && (lineId == null || lineId == 0 || fg.ProductionLineId == lineId)
               && (modelId == null || modelId == 0 || fg.DescriptionId == modelId)
               && (string.IsNullOrEmpty(finishQty) || fg.Quanity <= Convert.ToInt32(finishQty))
               ).OrderByDescending(s => s.FinishGoodsInfoId).ToList();

                // Pagination //
                ViewBag.PagerData = GetPagerData(dto.Count(), pageSize, page);
                dto = dto.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                //-----------------//

                IEnumerable<FinishGoodsInfoViewModel> finishGoodsInfoViewModels = new List<FinishGoodsInfoViewModel>();
                AutoMapper.Mapper.Map(dto, finishGoodsInfoViewModels);
                return PartialView("_GetFinishGoodsList", finishGoodsInfoViewModels);
            }
        }

        public ActionResult GetFinishGoodsMaterialDetails(long finishGoodsInfoId)
        {
            IEnumerable<FinishGoodsRowMaterialViewModel> viewModels = new List<FinishGoodsRowMaterialViewModel>();
            if (finishGoodsInfoId > 0)
            {
                IEnumerable<FinishGoodsRowMaterialDTO> finishGoodsRowMaterialDTO = _finishGoodsRowMaterialBusiness.GetGoodsRowMaterialsByOrgAndFinishInfoId(User.OrgId, finishGoodsInfoId).Select(r => new FinishGoodsRowMaterialDTO
                {
                    WarehouseName = _warehouseBusiness.GetWarehouseOneByOrgId(r.WarehouseId, User.OrgId).WarehouseName,
                    ItemTypeName = _itemTypeBusiness.GetItemType(r.ItemTypeId, User.OrgId).ItemName,
                    ItemName = _itemBusiness.GetItemOneByOrgId(r.ItemId, User.OrgId).ItemName,
                    UnitName = _unitBusiness.GetUnitOneByOrgId(r.UnitId, User.OrgId).UnitSymbol,
                    Quanity = r.Quanity
                }).ToList();
                AutoMapper.Mapper.Map(finishGoodsRowMaterialDTO, viewModels);
            }
            return PartialView("_GetFinishGoodsMaterialDetails", viewModels);
        }

        public ActionResult GetFinishGoodsStockInfo(string flag, long? WarehouseId, long? ItemTypeId, long? ItemId, long? LineId, long? ModelId, string lessOrEq, int page = 1)
        {
            ViewBag.UserPrivilege = UserPrivilege("Production", "GetFinishGoodsStockInfo");
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

                ViewBag.ddlModelName = _descriptionBusiness.GetAllDescriptionsInProductionStock(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();

                ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(ware => new SelectListItem
                {
                    Text = ware.WarehouseName,
                    Value = ware.Id.ToString()
                }).ToList();
                return View();
            }
            else
            {
                IEnumerable<FinishGoodsStockInfoDTO> dto = _finishGoodsStockInfoBusiness.GetAllFinishGoodsStockInfoByOrgId(User.OrgId).Select(info => new FinishGoodsStockInfoDTO
                {
                    FinishGoodsStockInfoId = info.FinishGoodsStockInfoId,
                    LineId = info.LineId.Value,
                    LineNumber = _productionLineBusiness.GetProductionLineOneByOrgId(info.LineId.Value, User.OrgId).LineNumber,
                    WarehouseId = info.WarehouseId,
                    WarehouseName = (_warehouseBusiness.GetWarehouseOneByOrgId(info.WarehouseId.Value, User.OrgId).WarehouseName),
                    ItemTypeId = info.ItemTypeId,
                    ItemTypeName = (_itemTypeBusiness.GetItemType(info.ItemTypeId.Value, User.OrgId).ItemName),
                    ItemId = info.ItemId,
                    ItemName = (_itemBusiness.GetItemOneByOrgId(info.ItemId.Value, User.OrgId).ItemName),
                    UnitId = info.UnitId,
                    UnitName = (_unitBusiness.GetUnitOneByOrgId(info.UnitId.Value, User.OrgId).UnitSymbol),
                    StockInQty = info.StockInQty,
                    StockOutQty = info.StockOutQty,
                    Remarks = info.Remarks,
                    OrganizationId = info.OrganizationId,
                    DescriptionId = info.DescriptionId,
                    ModelName = (_descriptionBusiness.GetDescriptionOneByOrdId(info.DescriptionId.Value, info.OrganizationId).DescriptionName)
                }).AsEnumerable();

                dto = dto.Where(fs =>
                (WarehouseId == null || WarehouseId == 0 || fs.WarehouseId == WarehouseId)
                && (ItemTypeId == null || ItemTypeId == 0 || fs.ItemTypeId == ItemTypeId)
                && (ItemId == null || ItemId == 0 || fs.ItemId == ItemId)
                && (LineId == null || LineId == 0 || fs.LineId == LineId)
                && (ModelId == null || ModelId == 0 || fs.DescriptionId == ModelId)
                && (string.IsNullOrEmpty(lessOrEq) || (fs.StockInQty - fs.StockOutQty) <= Convert.ToInt32(lessOrEq))
                ).ToList();

                // Pagination //
                ViewBag.PagerData = GetPagerData(dto.Count(), pageSize, page);
                dto = dto.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                //-----------------//

                List<FinishGoodsStockInfoViewModel> finishGoodsStockInfoViewModels = new List<FinishGoodsStockInfoViewModel>();
                AutoMapper.Mapper.Map(dto, finishGoodsStockInfoViewModels);
                return PartialView("_GetFinishGoodsStockInfo", finishGoodsStockInfoViewModels);
            }
        }

        public ActionResult GetFinishGoodsStockDetailInfoList(string flag, long? lineId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string stockStatus, string fromDate, string toDate, string refNum, int page = 1)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ListOfLine = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(l => new SelectListItem
                {
                    Text = l.LineNumber,
                    Value = l.LineId.ToString()
                }).ToList();

                ViewBag.ListOfWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(w => new SelectListItem
                {
                    Text = w.WarehouseName,
                    Value = w.Id.ToString()
                }).ToList();

                ViewBag.ddlStockStatus = Utility.ListOfStockStatus().Select(s => new SelectListItem() { Text = s.text, Value = s.value }).ToList();

                ViewBag.ddlModelName = _descriptionBusiness.GetAllDescriptionsInProductionStock(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();
                return View();
            }
            else
            {
                var dto = _finishGoodsStockDetailBusiness.GetFinishGoodsStockDetailInfoList(lineId, modelId, warehouseId, itemTypeId, itemId, stockStatus, fromDate, toDate, refNum).OrderByDescending(s => s.StockDetailId).ToList();

                // Pagination //
                ViewBag.PagerData = GetPagerData(dto.Count(), pageSize, page);
                dto = dto.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                //-----------------//

                IEnumerable<FinishGoodsStockDetailInfoListViewModel> viewModels = new List<FinishGoodsStockDetailInfoListViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetFinishGoodsStockDetailInfoList", viewModels);
            }
        }
        #endregion

        #region Finish Goods Send To Warehouse
        public ActionResult GetFinishGoodsSendToWarehouse(string flag, long? lineId, long? warehouseId, long? modelId, string status, string fromDate, string toDate, string refNo, int page = 1)
        {
            ViewBag.UserPrivilege = UserPrivilege("Production", "GetFinishGoodsSendToWarehouse");
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

                ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Where(w => w.WarehouseName == "Warehouse 2" || w.WarehouseName == "Warehouse 3").Select(ware => new SelectListItem
                {
                    Text = ware.WarehouseName,
                    Value = ware.Id.ToString()
                }).ToList();

                ViewBag.ddlModelName = _descriptionBusiness.GetAllDescriptionsInProductionStock(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();

                ViewBag.ddlSendStatus = Utility.ListOfFinishGoodsSendStatus().Select(s => new SelectListItem
                {
                    Text = s.text,
                    Value = s.value
                }).ToList();

                return View();
            }
            else
            {

                var tblwarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId);
                var tblLine = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId);
                var tblModel = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId);

                List<FinishGoodsSendToWarehouseInfoDTO> dto = _finishGoodsSendToWarehouseInfoBusiness.GetFinishGoodsSendToWarehouseList(User.OrgId)
                     .Where(f => 1 == 1 &&
                         (refNo == null || refNo.Trim() == "" || f.RefferenceNumber.Contains(refNo)) &&
                         (warehouseId == null || warehouseId <= 0 || f.WarehouseId == warehouseId) &&
                         (status == null || status.Trim() == "" || f.StateStatus == status.Trim()) &&
                         (lineId == null || lineId <= 0 || f.LineId == lineId) &&
                         (modelId == null || modelId <= 0 || f.DescriptionId == modelId) &&
                         (
                             (fromDate == null && toDate == null)
                             ||
                              (fromDate == "" && toDate == "")
                             ||
                             (fromDate.Trim() != "" && toDate.Trim() != "" &&

                                 f.EntryDate.Value.Date >= Convert.ToDateTime(fromDate).Date &&
                                 f.EntryDate.Value.Date <= Convert.ToDateTime(toDate).Date)
                             ||
                             (fromDate.Trim() != "" && f.EntryDate.Value.Date == Convert.ToDateTime(fromDate).Date)
                             ||
                             (toDate.Trim() != "" && f.EntryDate.Value.Date == Convert.ToDateTime(toDate).Date)
                         )
                     )
                     .Select(f => new FinishGoodsSendToWarehouseInfoDTO
                     {
                         SendId = f.SendId,
                         LineNumber = _productionLineBusiness.GetProductionLineOneByOrgId(f.LineId, User.OrgId).LineNumber,
                         WarehouseName = (_warehouseBusiness.GetWarehouseOneByOrgId(f.WarehouseId, User.OrgId).WarehouseName),
                         ModelName = _descriptionBusiness.GetDescriptionOneByOrdId(f.DescriptionId, User.OrgId).DescriptionName,
                         StateStatus = f.StateStatus,
                         ItemCount = this._finishGoodsSendToWarehouseDetailBusiness.GetFinishGoodsDetailByInfoId(f.SendId, User.OrgId).Count(),
                         Remarks = f.Remarks,
                         EntryDate = f.EntryDate,
                         RefferenceNumber = f.RefferenceNumber,
                         EntryUser = UserForEachRecord(f.EUserId.Value).UserName,
                         UpdateUser = (f.UpUserId == null || f.UpUserId == 0) ? "" : UserForEachRecord(f.UpUserId.Value).UserName
                     }).OrderByDescending(s => s.SendId).ToList();

                // Pagination //
                ViewBag.PagerData = GetPagerData(dto.Count(), pageSize, page);
                dto = dto.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                //-----------------//

                List<FinishGoodsSendToWarehouseInfoViewModel> listOfFinishGoodsSendToWarehouseInfoViewModels = new List<FinishGoodsSendToWarehouseInfoViewModel>();
                AutoMapper.Mapper.Map(dto, listOfFinishGoodsSendToWarehouseInfoViewModels);
                return PartialView("_GetFinishGoodsSendToWarehouse", listOfFinishGoodsSendToWarehouseInfoViewModels);
            }
        }

        [HttpGet]
        public ActionResult CreateFinishGoodsToWarehouse()
        {
            ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

            ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Where(w => w.WarehouseName == "Warehouse 2" || w.WarehouseName == "Warehouse 3").Select(ware => new SelectListItem
            {
                Text = ware.WarehouseName,
                Value = ware.Id.ToString()
            }).ToList();

            ViewBag.ddlModelName = _descriptionBusiness.GetAllDescriptionsInProductionStock(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();

            return View();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveFinishGoodsSendToWarehouse(FinishGoodsSendToWarehouseInfoViewModel info, List<FinishGoodsSendToWarehouseDetailViewModel> detail)
        {
            bool IsSucess = false;
            var privilege = UserPrivilege("Production", "GetFinishGoodsSendToWarehouse");
            var permission = (info.SendId == 0 && privilege.Add) || (info.SendId > 0 && privilege.Edit);

            if (ModelState.IsValid && detail.Count() > 0)
            {
                FinishGoodsSendToWarehouseInfoDTO dtoInfo = new FinishGoodsSendToWarehouseInfoDTO();
                List<FinishGoodsSendToWarehouseDetailDTO> dtoDetail = new List<FinishGoodsSendToWarehouseDetailDTO>();
                AutoMapper.Mapper.Map(info, dtoInfo);
                AutoMapper.Mapper.Map(detail, dtoDetail);
                IsSucess = _finishGoodsSendToWarehouseInfoBusiness.SaveFinishGoodsSendToWarehouse(dtoInfo, dtoDetail, User.UserId, User.OrgId);
            }
            return Json(IsSucess);
        }
        public ActionResult GetFinishGoodsSendItemDetail(long sendId)
        {
            List<FinishGoodsSendToWarehouseDetailViewModel> viewModels = new List<FinishGoodsSendToWarehouseDetailViewModel>();
            if (sendId > 0)
            {
                var info = _finishGoodsSendToWarehouseInfoBusiness.GetFinishGoodsSendToWarehouseById(sendId, User.OrgId);
                FinishGoodsSendToWarehouseInfoViewModel infoViewModel = new FinishGoodsSendToWarehouseInfoViewModel
                {
                    SendId = info.SendId,
                    LineNumber = _productionLineBusiness.GetProductionLineOneByOrgId(info.LineId, User.OrgId).LineNumber,
                    RefferenceNumber = info.RefferenceNumber,
                    WarehouseId = info.WarehouseId,
                    WarehouseName = _warehouseBusiness.GetWarehouseOneByOrgId(info.WarehouseId, User.OrgId).WarehouseName,
                    DescriptionId = info.DescriptionId,
                    ModelName = _descriptionBusiness.GetDescriptionOneByOrdId(info.DescriptionId, User.OrgId).DescriptionName
                };

                ViewBag.FinishGoodsSendInfo = infoViewModel;
                List<FinishGoodsSendToWarehouseDetailDTO> dtos = new List<FinishGoodsSendToWarehouseDetailDTO>();
                dtos = _finishGoodsSendToWarehouseDetailBusiness.GetFinishGoodsDetailByInfoId(sendId, User.OrgId).Select(f => new FinishGoodsSendToWarehouseDetailDTO
                {
                    SendDetailId = f.SendDetailId,
                    ItemTypeName = _itemTypeBusiness.GetItemType(f.ItemTypeId, User.OrgId).ItemName,
                    ItemName = _itemBusiness.GetItemById(f.ItemId, User.OrgId).ItemName,
                    UnitName = _unitBusiness.GetUnitOneByOrgId(f.UnitId, User.OrgId).UnitSymbol,
                    Quantity = f.Quantity
                }).ToList();

                AutoMapper.Mapper.Map(dtos, viewModels);
            }
            return PartialView("_GetFinishGoodsSendItemDetail", viewModels);
        }
        public ActionResult GetFinishGoodsSendItemDetailList(string flag, long? lineId, long? warehouseId, long? modelId, long? itemTypeId, long? itemId, string status, string refNum, string fromDate, string toDate, int page = 1)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();
                ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Where(w => w.WarehouseName == "Warehouse 2" || w.WarehouseName == "Warehouse 3").Select(ware => new SelectListItem
                {
                    Text = ware.WarehouseName,
                    Value = ware.Id.ToString()
                }).ToList();
                ViewBag.ddlModelName = _descriptionBusiness.GetAllDescriptionsInProductionStock(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();
                ViewBag.ddlSendStatus = Utility.ListOfFinishGoodsSendStatus().Select(s => new SelectListItem
                {
                    Text = s.text,
                    Value = s.value
                }).ToList();
                return View();
            }
            else
            {
                IEnumerable<FinishGoodsSendDetailListDTO> dto = _finishGoodsSendToWarehouseDetailBusiness.GetGoodsSendDetailList(lineId, warehouseId, modelId, itemTypeId, itemId, status, refNum, User.OrgId, fromDate, toDate).OrderByDescending(s => s.SendDetailId).ToList();

                // Pagination //
                ViewBag.PagerData = GetPagerData(dto.Count(), pageSize, page);
                dto = dto.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                //-----------------//

                IEnumerable<FinishGoodsSendDetailListViewModel> viewModels = new List<FinishGoodsSendDetailListViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetFinishGoodsSendItemDetailList", viewModels);
            }
        }

        #endregion

        #region Assembly
        public ActionResult GetAssemblyLines()
        {
            var assemblyLinesDtos = _assemblyLineBusiness.GetAssemblyLines(User.OrgId).Select(a => new AssemblyLineDTO
            {
                AssemblyLineId = a.AssemblyLineId,
                AssemblyLineName = a.AssemblyLineName,
                ProductionLineId = a.ProductionLineId,
                LineNumber = _productionLineBusiness.GetProductionLineOneByOrgId(a.ProductionLineId, User.OrgId).LineNumber,
                IsActive = a.IsActive,
                OrganizationId = a.OrganizationId,
                EntryDate = DateTime.Now,
                Remarks = a.Remarks,
                EntryUser = UserForEachRecord(a.EUserId.Value).UserName,
                UpdateUser = (a.UpUserId == null || a.UpUserId == 0) ? "" : UserForEachRecord(a.UpUserId.Value).UserName
            }).ToList();

            ViewBag.UserPrivilege = UserPrivilege("Production", "GetAssemblyLines");

            ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

            IEnumerable<AssemblyLineViewModel> assemblyLineViewModels = new List<AssemblyLineViewModel>();

            AutoMapper.Mapper.Map(assemblyLinesDtos, assemblyLineViewModels);
            return View(assemblyLineViewModels);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveAssemblyLine(AssemblyLineViewModel model)
        {
            bool IsSuccess = false;
            var privilege = UserPrivilege("Production", "GetAssemblyLines");
            //var permission = (model.AssemblyLineId == 0 && privilege.Add) || (model.AssemblyLineId > 0 && privilege.Edit);
            //&& permission
            if (ModelState.IsValid )
            {
                AssemblyLineDTO dto = new AssemblyLineDTO();
                AutoMapper.Mapper.Map(model, dto);
                IsSuccess = _assemblyLineBusiness.SaveAssembly(dto, User.UserId, User.OrgId);
            }
            return Json(IsSuccess);
        }

        // Assembly Stock Info
        #region  Assembly Stock Info
        public ActionResult GetAssemblyLineStockInfo(string flag, long? lineId, long? assemblyId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string lessOrEq, int page = 1)
        {
            //ViewBag.UserPrivilege = UserPrivilege("Production", "GetAssemblyLineStockInfo");
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

                ViewBag.ddlModelName = _descriptionBusiness.GetAllDescriptionsInProductionStock(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();

                ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(ware => new SelectListItem
                {
                    Text = ware.WarehouseName,
                    Value = ware.Id.ToString()
                }).ToList();

                ViewBag.ddlItems = _itemBusiness.GetItemDetails(User.OrgId).Select(s => new SelectListItem { Text = s.ItemName, Value = s.ItemId }).ToList();

                ViewBag.ddlStateStatus = Utility.ListOfReqStatus().Where(s => s.value == RequisitionStatus.Approved || s.value == RequisitionStatus.Accepted).Select(st => new SelectListItem
                {
                    Text = st.text,
                    Value = st.value
                }).ToList();

                return View();
            }
            else
            {
                IEnumerable<AssemblyLineStockInfoDTO> dto = _assemblyLineStockInfoBusiness.GetAssemblyLineStockInfosByQuery(lineId, assemblyId, modelId, warehouseId, itemTypeId, itemId, lessOrEq, User.OrgId);
                //// Pagination //
                //ViewBag.PagerData = GetPagerData(dto.Count(), pageSize, page);
                //dto = dto.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                ////-----------------//
                List<AssemblyLineStockInfoViewModel> viewModels = new List<AssemblyLineStockInfoViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetAssemblyLineStockInfo", viewModels);
            }
        }

        // Obsolete
        // Receive Stock From Floor
        public ActionResult GetReceiveStockFromFloor(string flag, long? lineId, long? assemblyId, long? modelId, long? warehouseId, string status, string transferCode, string fromDate, string toDate, long? transferInfoId, int page = 1)
        {
            //ViewBag.UserPrivilege = UserPrivilege("Production", "GetReceiveStockFromFloor");
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlStateStatus = Utility.ListOfReqStatus().Where(s => s.value == RequisitionStatus.Approved || s.value == RequisitionStatus.Accepted).Select(st => new SelectListItem
                {
                    Text = st.text,
                    Value = st.value
                }).ToList();

                ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

                ViewBag.ddlModelName = _descriptionBusiness.GetAllDescriptionsInProductionStock(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();

                ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(ware => new SelectListItem
                {
                    Text = ware.WarehouseName,
                    Value = ware.Id.ToString()
                }).ToList();

                return View();
            }
            else if (flag.Trim().ToLower() == Flag.View.ToLower() || flag.Trim().ToLower() == Flag.Search.ToLower())
            {
                var dto = ProductionTransferList(User.OrgId, lineId, assemblyId, modelId, warehouseId, status, transferCode, fromDate, toDate);

                // Pagination //
                //ViewBag.PagerData = GetPagerData(dto.Count(), pageSize, page);
                //dto = dto.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                //-----------------//


                IEnumerable<TransferStockToAssemblyInfoViewModel> viewModels = new List<TransferStockToAssemblyInfoViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);

                return PartialView("_GetFloorStockTransferList", viewModels);
            }
            else
            {
                // Details Part
                TransferStockToAssemblyInfoDTO info;
                List<TransferStockToAssemblyDetailDTO> details;
                ProductionTransferInfoAndDetail(User.OrgId, transferInfoId.Value, out info, out details);

                TransferStockToAssemblyInfoViewModel viewModel = new TransferStockToAssemblyInfoViewModel();

                IEnumerable<TransferStockToAssemblyDetailViewModel> list = new List<TransferStockToAssemblyDetailViewModel>();

                AutoMapper.Mapper.Map(info, viewModel);
                AutoMapper.Mapper.Map(details, list);

                ViewBag.Info = viewModel;
                return PartialView("_GetReceiveStockFromFloorDetail", list);
            }
        }

        #endregion

        // Assembly Stock Transfer
        public ActionResult GetAssemblyStockTransferList(string flag, long? lineId, long? assemblyId, long? qcId, long? modelId, long? warehouseId, string status, string transferCode, string fromDate, string toDate, long? transferInfoId, int page = 1)
        {
            ViewBag.UserPrivilege = UserPrivilege("Production", "GetAssemblyStockTransferList");
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlStateStatus = Utility.ListOfReqStatus().Where(s => s.value == RequisitionStatus.Approved || s.value == RequisitionStatus.Accepted).Select(st => new SelectListItem
                {
                    Text = st.text,
                    Value = st.value
                }).ToList();

                ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

                ViewBag.ddlModelName = _descriptionBusiness.GetAllDescriptionsInProductionStock(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();

                ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(ware => new SelectListItem
                {
                    Text = ware.WarehouseName,
                    Value = ware.Id.ToString()
                }).ToList();

                return View();
            }
            else if (flag.Trim().ToLower() == Flag.View.ToLower() || flag.Trim().ToLower() == Flag.Search.ToLower())
            {
                var dto = AssemblyTransferList(User.OrgId, lineId, assemblyId, qcId, modelId, warehouseId, status, transferCode, fromDate, toDate);

                // Pagination //
                ViewBag.PagerData = GetPagerData(dto.Count(), pageSize, page);
                dto = dto.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                //-----------------//

                IEnumerable<TransferStockToQCInfoViewModel> viewModels = new List<TransferStockToQCInfoViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetAssemblyStockTransferList", viewModels);
            }
            else
            {
                // Details Part
                TransferStockToQCInfoDTO info;
                List<TransferStockToQCDetailDTO> details;
                AssemblyTransferInfoAndDetail(User.OrgId, transferInfoId.Value, out info, out details);

                TransferStockToQCInfoViewModel viewModel = new TransferStockToQCInfoViewModel();
                IEnumerable<TransferStockToQCDetailViewModel> list = new List<TransferStockToQCDetailViewModel>();

                AutoMapper.Mapper.Map(info, viewModel);
                AutoMapper.Mapper.Map(details, list);

                ViewBag.Info = viewModel;
                return PartialView("_GetAssemblyStockTransferDetail", list);
            }
        }

        public ActionResult CreateAssemblyStockTransfer()
        {
            ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

            ViewBag.ddlModelName = _descriptionBusiness.GetAllDescriptionsInProductionStock(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();

            ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(ware => new SelectListItem
            {
                Text = ware.WarehouseName,
                Value = ware.Id.ToString()
            }).ToList();

            return View();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveAssemblyStockTransfer(TransferStockToQCInfoViewModel info, List<TransferStockToQCDetailViewModel> details)
        {
            bool IsSuccess = false;
            if (ModelState.IsValid && details.Count > 0)
            {
                TransferStockToQCInfoDTO dtoInfo = new TransferStockToQCInfoDTO();
                List<TransferStockToQCDetailDTO> dtoDetails = new List<TransferStockToQCDetailDTO>();
                AutoMapper.Mapper.Map(info, dtoInfo);
                AutoMapper.Mapper.Map(details, dtoDetails);
                IsSuccess = _transferStockToQCInfoBusiness.SaveTransferStockQC(dtoInfo, dtoDetails, User.UserId, User.OrgId);
            }
            return Json(IsSuccess);
        }

        [NonAction]
        private IEnumerable<TransferStockToQCInfoDTO> AssemblyTransferList(long orgId, long? lineId, long? assemblyId, long? qcId, long? modelId, long? warehouseId, string status, string transferCode, string fromDate, string toDate)
        {

            var dto = new List<TransferStockToQCInfoDTO>();

            try
            {
                var data = _transferStockToQCInfoBusiness.GetStockToQCInfos(orgId);
                dto = data.Select(t => new TransferStockToQCInfoDTO
                {
                    TSQInfoId = t.TSQInfoId,
                    LineId = t.LineId,
                    ProductionLineName = _productionLineBusiness.GetProductionLineOneByOrgId(t.LineId.Value, User.OrgId).LineNumber,
                    AssemblyId = t.AssemblyId,
                    AssemblyLineName = _assemblyLineBusiness.GetAssemblyLineById(t.AssemblyId.Value, User.OrgId).AssemblyLineName,
                    QCLineId = t.QCLineId,
                    QCLineName = _qualityControlBusiness.GetQualityControlById(t.QCLineId.Value, User.OrgId).QCName,
                    DescriptionId = t.DescriptionId,
                    ModelName = _descriptionBusiness.GetDescriptionOneByOrdId(t.DescriptionId.Value, User.OrgId).DescriptionName,
                    WarehouseId = t.WarehouseId,
                    WarehouseName = _warehouseBusiness.GetWarehouseOneByOrgId(t.WarehouseId.Value, User.OrgId).WarehouseName,
                    EntryUser = UserForEachRecord(t.EUserId.Value).UserName,
                    UpdateUser = (t.UpUserId == null || t.UpUserId == 0) ? "" : UserForEachRecord(t.UpUserId.Value).UserName,
                    EntryDate = t.EntryDate,
                    Remarks = t.Remarks,
                    TransferCode = t.TransferCode,
                    StateStatus = t.StateStatus,
                    ItemCount = _transferStockToQCDetailBusiness.GetTransferStockToQCDetailByInfo(t.TSQInfoId, User.OrgId).Count(),
                    ItemTypeId = t.ItemTypeId,
                    ItemTypeName = _itemTypeBusiness.GetItemTypeOneByOrgId(t.ItemTypeId.Value, t.WarehouseId.Value, User.OrgId).ItemName,
                    ItemId = t.ItemId,
                    ItemName = _itemBusiness.GetItemOneByOrgId(t.ItemId.Value, User.OrgId).ItemName,
                    ForQty = t.ForQty.Value,
                }).ToList();
                dto = dto.Where(t =>
                (assemblyId == null || assemblyId <= 0 || t.AssemblyId == assemblyId) &&
                (qcId == null || qcId <= 0 || t.QCLineId == qcId) &&
                (modelId == null || modelId <= 0 || t.DescriptionId == modelId) &&
                (lineId == null || lineId <= 0 || t.LineId == lineId) &&
                (warehouseId == null || warehouseId <= 0 || t.WarehouseId == warehouseId) &&
                (transferCode == null || transferCode.Trim() == "" || t.TransferCode.Contains(transferCode)) &&
                (status == null || status.Trim() == "" || t.StateStatus == status.Trim()) &&
                (
                    (fromDate == null && toDate == null)
                    ||
                    (fromDate == "" && toDate == "")
                    ||
                    (fromDate.Trim() != "" && toDate.Trim() != "" &&

                        t.EntryDate.Value.Date >= Convert.ToDateTime(fromDate).Date &&
                        t.EntryDate.Value.Date <= Convert.ToDateTime(toDate).Date)
                    ||
                    (fromDate.Trim() != "" && t.EntryDate.Value.Date == Convert.ToDateTime(fromDate).Date)
                    ||
                    (toDate.Trim() != "" && t.EntryDate.Value.Date == Convert.ToDateTime(toDate).Date)
                )).OrderByDescending(t => t.EntryDate).ToList();
            }
            catch (Exception ex)
            {

                throw;
            }


            return dto;
        }
        [NonAction]
        private void AssemblyTransferInfoAndDetail(long orgId, long transferInfoId, out TransferStockToQCInfoDTO info, out List<TransferStockToQCDetailDTO> detail)
        {
            var infoDomain = _transferStockToQCInfoBusiness.GetStockToQCInfoById(transferInfoId, User.OrgId);
            info = new TransferStockToQCInfoDTO
            {
                AssemblyLineName = _assemblyLineBusiness.GetAssemblyLineById(infoDomain.AssemblyId.Value, User.OrgId).AssemblyLineName,
                TSQInfoId = infoDomain.TSQInfoId,
                TransferCode = infoDomain.TransferCode,
                ModelName = _descriptionBusiness.GetDescriptionOneByOrdId(infoDomain.DescriptionId.Value, User.OrgId).DescriptionName,
                ProductionLineName = _productionLineBusiness.GetProductionLineOneByOrgId(infoDomain.LineId.Value, User.OrgId).LineNumber,
                WarehouseName = _warehouseBusiness.GetWarehouseOneByOrgId(infoDomain.WarehouseId.Value, User.OrgId).WarehouseName,
                StateStatus = infoDomain.StateStatus,
                QCLineName = _qualityControlBusiness.GetQualityControlById(infoDomain.QCLineId.Value, User.OrgId).QCName,
                ItemTypeName = _itemTypeBusiness.GetItemTypeOneByOrgId(infoDomain.ItemTypeId.Value, infoDomain.WarehouseId.Value, User.OrgId).ItemName,
                ItemName = _itemBusiness.GetItemOneByOrgId(infoDomain.ItemId.Value, User.OrgId).ItemName,
                ForQty = infoDomain.ForQty
            };
            detail = _transferStockToQCDetailBusiness.GetTransferStockToQCDetailByInfo(transferInfoId, User.OrgId)
                .Select(s => new TransferStockToQCDetailDTO
                {
                    TSQDetailId = s.TSQDetailId,
                    WarehouseName = _warehouseBusiness.GetWarehouseOneByOrgId(s.WarehouseId.Value, User.OrgId).WarehouseName,
                    ItemTypeName = _itemTypeBusiness.GetItemTypeOneByOrgId(s.ItemTypeId.Value, s.WarehouseId.Value, User.OrgId).ItemName,
                    ItemName = _itemBusiness.GetItemOneByOrgId(s.ItemId.Value, User.OrgId).ItemName,
                    Quantity = s.Quantity,
                    UnitName = _unitBusiness.GetUnitOneByOrgId(s.UnitId.Value, User.OrgId).UnitSymbol,
                    Remarks = s.Remarks
                }).ToList();
        }

        #endregion

        #region Quality Control
        public ActionResult GetQualityControls()
        {
            var dto = _qualityControlBusiness.GetQualityControls(User.OrgId).Select(s => new QualityControlDTO
            {
                QCId = s.QCId,
                QCName = s.QCName,
                IsActive = s.IsActive,
                ProductionLineId = s.ProductionLineId,
                LineNumber = _productionLineBusiness.GetProductionLineOneByOrgId(s.ProductionLineId, User.OrgId).LineNumber,
                Remarks = s.Remarks,
                EntryUser = UserForEachRecord(s.EUserId.Value).UserName,
                UpdateUser = (s.UpUserId == null || s.UpUserId == 0) ? "" : UserForEachRecord(s.UpUserId.Value).UserName,
                EntryDate = s.EntryDate,
                UpdateDate = s.UpdateDate
            }).ToList();

            ViewBag.UserPrivilege = UserPrivilege("Production", "GetQualityControls");
            ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

            IEnumerable<QualityControlViewModel> viewModels = new List<QualityControlViewModel>();
            AutoMapper.Mapper.Map(dto, viewModels);
            return View(viewModels);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveQualityControl(QualityControlViewModel model)
        {
            bool IsSuccess = false;
            if (ModelState.IsValid)
            {
                QualityControlDTO dto = new QualityControlDTO();
                AutoMapper.Mapper.Map(model, dto);
                IsSuccess = _qualityControlBusiness.SaveQualityControl(dto, User.UserId, User.OrgId);
            }
            return Json(IsSuccess);
        }

        public ActionResult GetQCItemStockList(string flag, long? floorId, long? assemblyId, long? modelId, long? qcId, long? warehouseId, long? itemTypeId, long? itemId, int page = 1)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

                ViewBag.ddlModelName = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).Select(des => new SelectListItem { Text = des.DescriptionName, Value = des.DescriptionId.ToString() }).ToList();

                ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(ware => new SelectListItem
                {
                    Text = ware.WarehouseName,
                    Value = ware.Id.ToString()
                }).ToList();

                ViewBag.ddlItems = _itemBusiness.GetItemDetails(User.OrgId).Where(s => s.ItemName.Contains("Warehouse 3")).Select(s => new SelectListItem { Text = s.ItemName, Value = s.ItemId }).ToList();

                ViewBag.ddlStateStatus = Utility.ListOfReqStatus().Where(s => s.value == RequisitionStatus.Approved || s.value == RequisitionStatus.Accepted).Select(st => new SelectListItem
                {
                    Text = st.text,
                    Value = st.value
                }).ToList();

                return View();
            }
            else
            {
                var dto = _qCItemStockInfoBusiness.GetQCItemStockInfosByQuery(floorId, assemblyId, qcId, modelId, warehouseId, itemTypeId, itemId, User.OrgId);

                //    .GetQCItemStocks(User.OrgId).Select(d => new QCItemStockInfoDTO
                //{
                //    ProductionFloorId = d.ProductionFloorId,
                //    ProductionFloorName = _productionLineBusiness.GetProductionLineOneByOrgId(d.ProductionFloorId.Value, User.OrgId).LineNumber,
                //    QCId = d.QCId,
                //    QCName = _qualityControlBusiness.GetQualityControlById(d.QCId.Value, User.OrgId).QCName,
                //    DescriptionId = d.DescriptionId,
                //    ModelName = _descriptionBusiness.GetDescriptionOneByOrdId(d.DescriptionId.Value, User.OrgId).DescriptionName,
                //    WarehouseId = d.WarehouseId,
                //    WarehouseName = _warehouseBusiness.GetWarehouseOneByOrgId(d.WarehouseId.Value, User.OrgId).WarehouseName,
                //    ItemTypeId = d.ItemTypeId,
                //    ItemTypeName = _itemTypeBusiness.GetItemTypeOneByOrgId(d.ItemTypeId.Value, d.WarehouseId.Value, User.OrgId).ItemName,
                //    ItemId = d.ItemId,
                //    ItemName = _itemBusiness.GetItemOneByOrgId(d.ItemId.Value, User.OrgId).ItemName,
                //    Quantity = d.Quantity,
                //    RepairQty = d.RepairQty,
                //    MiniStockQty = d.MiniStockQty,
                //    LabQty = d.LabQty
                //}).ToList();

                //    dto = dto.Where(t =>
                //(qcId == null || qcId <= 0 || t.QCId == qcId) &&
                //(modelId == null || modelId <= 0 || t.DescriptionId == modelId) &&
                //(floorId == null || floorId <= 0 || t.ProductionFloorId == floorId) &&
                //(warehouseId == null || warehouseId <= 0 || t.WarehouseId == warehouseId) &&
                //(itemTypeId == null || itemTypeId <= 0 || t.ItemTypeId == itemTypeId) &&
                //(itemId == null || itemId <= 0 || t.ItemId == itemId) &&
                //(string.IsNullOrEmpty(lessOrEq) || (t.Quantity - (t.RepairQty + t.MiniStockQty + t.LabQty)) <= Convert.ToInt32(lessOrEq))).OrderByDescending(t => t.EntryDate).ToList();

                // Pagination //
                ViewBag.PagerData = GetPagerData(dto.Count(), pageSize, page);
                dto = dto.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                //-----------------//
                IEnumerable<QCItemStockInfoViewModel> viewModels = new List<QCItemStockInfoViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);

                return PartialView("_GetQCItemStockList", viewModels);
            }
        }

        // QC Stock Info
        public ActionResult GetQualityControlLineStockInfo(string flag, long? lineId, long? assemblyId, long? qcId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string lessOrEq, int page = 1)
        {
            ViewBag.UserPrivilege = UserPrivilege("Production", "GetQualityControlLineStockInfo");
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

                ViewBag.ddlModelName = _descriptionBusiness.GetAllDescriptionsInProductionStock(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();

                ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(ware => new SelectListItem
                {
                    Text = ware.WarehouseName,
                    Value = ware.Id.ToString()
                }).ToList();

                return View();
            }
            else
            {
                IEnumerable<QualityControlLineStockInfoDTO> dto = _qCLineStockInfoBusiness.GetQCLineStockInfos(User.OrgId).Select(info => new QualityControlLineStockInfoDTO
                {
                    QCStockInfoId = info.QCStockInfoId,
                    ProductionLineId = info.ProductionLineId.Value,
                    ProductionLineName = _productionLineBusiness.GetProductionLineOneByOrgId(info.ProductionLineId.Value, User.OrgId).LineNumber,
                    AssemblyLineId = info.AssemblyLineId,
                    AssemblyLineName = _assemblyLineBusiness.GetAssemblyLineById(info.AssemblyLineId.Value, User.OrgId).AssemblyLineName,
                    QCLineId = info.QCLineId,
                    QCLineName = _qualityControlBusiness.GetQualityControlById(info.QCLineId.Value, User.OrgId).QCName,
                    DescriptionId = info.DescriptionId,
                    ModelName = (_descriptionBusiness.GetDescriptionOneByOrdId(info.DescriptionId.Value, info.OrganizationId).DescriptionName),
                    WarehouseId = info.WarehouseId,
                    WarehouseName = (_warehouseBusiness.GetWarehouseOneByOrgId(info.WarehouseId.Value, User.OrgId).WarehouseName),
                    ItemTypeId = info.ItemTypeId,
                    ItemTypeName = (_itemTypeBusiness.GetItemType(info.ItemTypeId.Value, User.OrgId).ItemName),
                    ItemId = info.ItemId,
                    ItemName = (_itemBusiness.GetItemOneByOrgId(info.ItemId.Value, User.OrgId).ItemName),
                    UnitId = info.UnitId,
                    UnitName = (_unitBusiness.GetUnitOneByOrgId(info.UnitId.Value, User.OrgId).UnitSymbol),
                    StockInQty = info.StockInQty,
                    StockOutQty = info.StockOutQty,
                    Remarks = info.Remarks,
                    OrganizationId = info.OrganizationId

                }).AsEnumerable();

                dto = dto.Where(ws =>
                (lineId == null || lineId == 0 || ws.ProductionLineId == lineId)
                && (assemblyId == null || assemblyId == 0 || ws.AssemblyLineId == assemblyId)
                && (qcId == null || qcId == 0 || ws.QCLineId == qcId)
                && (modelId == null || modelId == 0 || ws.DescriptionId == modelId)
                && (warehouseId == null || warehouseId == 0 || ws.WarehouseId == warehouseId)
                && (itemTypeId == null || itemTypeId == 0 || ws.ItemTypeId == itemTypeId)
                && (itemId == null || itemId == 0 || ws.ItemId == itemId)
                && (string.IsNullOrEmpty(lessOrEq) || (ws.StockInQty - ws.StockOutQty) <= Convert.ToInt32(lessOrEq))
                ).OrderByDescending(s => s.QCStockInfoId).ToList();

                // Pagination //
                ViewBag.PagerData = GetPagerData(dto.Count(), pageSize, page);
                dto = dto.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                //-----------------//

                List<QualityControlLineStockInfoViewModel> viewModels = new List<QualityControlLineStockInfoViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetQualityControlLineStockInfo", viewModels);
            }
        }

        // Receive Stock From Assembly
        public ActionResult GetReceiveStockFromAssembly(string flag, long? lineId, long? assemblyId, long? qcId, long? modelId, long? warehouseId, string status, string transferCode, string fromDate, string toDate, long? transferInfoId, int page = 1)
        {
            ViewBag.UserPrivilege = UserPrivilege("Production", "GetReceiveStockFromAssembly");
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlStateStatus = Utility.ListOfReqStatus().Where(s => s.value == RequisitionStatus.Approved || s.value == RequisitionStatus.Accepted).Select(st => new SelectListItem
                {
                    Text = st.text,
                    Value = st.value
                }).ToList();

                ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

                ViewBag.ddlModelName = _descriptionBusiness.GetAllDescriptionsInProductionStock(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();

                ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(ware => new SelectListItem
                {
                    Text = ware.WarehouseName,
                    Value = ware.Id.ToString()
                }).ToList();

                return View();
            }
            else if (flag.Trim().ToLower() == Flag.View.ToLower() || flag.Trim().ToLower() == Flag.Search.ToLower())
            {
                var dto = AssemblyTransferList(User.OrgId, lineId, assemblyId, qcId, modelId, warehouseId, status, transferCode, fromDate, toDate);

                // Pagination //
                ViewBag.PagerData = GetPagerData(dto.Count(), pageSize, page);
                dto = dto.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                //-----------------//

                IEnumerable<TransferStockToQCInfoViewModel> viewModels = new List<TransferStockToQCInfoViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetAssemblyStockTransferList", viewModels);
            }
            else
            {
                // Details Part
                TransferStockToQCInfoDTO info;
                List<TransferStockToQCDetailDTO> details;
                AssemblyTransferInfoAndDetail(User.OrgId, transferInfoId.Value, out info, out details);

                TransferStockToQCInfoViewModel viewModel = new TransferStockToQCInfoViewModel();
                IEnumerable<TransferStockToQCDetailViewModel> list = new List<TransferStockToQCDetailViewModel>();

                AutoMapper.Mapper.Map(info, viewModel);
                AutoMapper.Mapper.Map(details, list);

                ViewBag.Info = viewModel;
                return PartialView("_GetAssemblyStockTransferDetail", list);
            }
        }

        // Save Receive Stock
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveAssemblyStockTransferStatus(long transferId, string status)
        {
            bool IsSuccess = false;
            if (transferId > 0 && !string.IsNullOrEmpty(status) && status.Trim() != "" && status == RequisitionStatus.Accepted)
            {
                IsSuccess = _qCLineStockDetailBusiness.SaveQCStockInByAssemblyLine(transferId, status, User.OrgId, User.UserId);
            }
            return Json(IsSuccess);
        }

        // Transfer to Repair
        public ActionResult GetQCStockTransferList(string flag, long? lineId, long? qcId, long? modelId, long? warehouseId, string status, string transferCode, string fromDate, string toDate, long? transferInfoId, string transferFor, long? repairLineId, long? packagingLineId, int page = 1)
        {
            ViewBag.UserPrivilege = UserPrivilege("Production", "GetQCStockTransferList");
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlStateStatus = Utility.ListOfReqStatus().Where(s => s.value == RequisitionStatus.Approved || s.value == RequisitionStatus.Accepted).Select(st => new SelectListItem
                {
                    Text = st.text,
                    Value = st.value
                }).ToList();

                ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

                ViewBag.ddlModelName = _descriptionBusiness.GetAllDescriptionsInProductionStock(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();

                ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(ware => new SelectListItem
                {
                    Text = ware.WarehouseName,
                    Value = ware.Id.ToString()
                }).ToList();

                // New //
                ViewBag.ddlTransferFor = new List<SelectListItem>
                {
                    new SelectListItem(){Text="Repair Line",Value="Repair Line"},
                    new SelectListItem(){Text="Packaging Line",Value="Packaging Line"},
                };

                return View();
            }
            else if (flag.ToLower() == Flag.View.ToLower() || flag.ToLower() == Flag.Search.ToLower())
            {
                var dto = QCStockTransferList(lineId, qcId, modelId, warehouseId, status, transferCode, fromDate, toDate, transferInfoId, transferFor, repairLineId, packagingLineId, string.Empty);

                // Pagination //
                ViewBag.PagerData = GetPagerData(dto.Count(), pageSize, page);
                dto = dto.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                //-----------------//

                IEnumerable<TransferFromQCInfoViewModel> viewModels = new List<TransferFromQCInfoViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetQCStockTransferList", viewModels);
            }
            else
            {
                TransferFromQCInfoDTO info;
                List<TransferFromQCDetailDTO> details;

                QCStockTransferDetail(transferInfoId.Value, out info, out details);
                TransferFromQCInfoViewModel viewModel = new TransferFromQCInfoViewModel();

                IEnumerable<TransferFromQCDetailViewModel> list = new List<TransferFromQCDetailViewModel>();

                AutoMapper.Mapper.Map(info, viewModel);
                AutoMapper.Mapper.Map(details, list);

                ViewBag.Info = viewModel;
                return PartialView("_GetQCStockTransferDetailList", list);
            }
        }

        public ActionResult CreateQCStockTransfer()
        {
            ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

            ViewBag.ddlModelName = _descriptionBusiness.GetAllDescriptionsInProductionStock(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();

            ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(ware => new SelectListItem
            {
                Text = ware.WarehouseName,
                Value = ware.Id.ToString()
            }).ToList();

            ViewBag.ddlTransferFor = new List<SelectListItem>
                {
                    new SelectListItem(){Text="Repair Line",Value="Repair Line"},
                    new SelectListItem(){Text="Packaging Line",Value="Packaging Line"},
                };
            return View();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveTransferStockFromQC(TransferFromQCInfoViewModel info, List<TransferFromQCDetailViewModel> details)
        {
            bool IsSuccess = false;
            if (ModelState.IsValid && details.Count() > 0)
            {
                TransferFromQCInfoDTO infoDTO = new TransferFromQCInfoDTO();
                List<TransferFromQCDetailDTO> detailDTO = new List<TransferFromQCDetailDTO>();
                AutoMapper.Mapper.Map(info, infoDTO);
                AutoMapper.Mapper.Map(details, detailDTO);
                IsSuccess = _transferFromQCInfoBusiness.SaveTransfer(infoDTO, detailDTO, User.UserId, User.OrgId);
            }
            return Json(IsSuccess);
        }

        public ActionResult GetReceiveItemFormRepairList(string flag, long? lineId, long? qcId, long? modelId, long? warehouseId, string status, string transferCode, string fromDate, string toDate, long? transferInfoId, long? repairLineId, int page = 1)
        {
            ViewBag.UserPrivilege = UserPrivilege("Production", "GetReceiveItemFormRepairList");
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlStateStatus = Utility.ListOfReqStatus().Where(s => s.value == RequisitionStatus.Approved || s.value == RequisitionStatus.Accepted).Select(st => new SelectListItem
                {
                    Text = st.text,
                    Value = st.value
                }).ToList();

                ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

                ViewBag.ddlModelName = _descriptionBusiness.GetAllDescriptionsInProductionStock(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();

                ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(ware => new SelectListItem
                {
                    Text = ware.WarehouseName,
                    Value = ware.Id.ToString()
                }).ToList();
                return View();
            }
            else if (flag.ToLower() == Flag.View.ToLower() || flag.ToLower() == Flag.Search.ToLower())
            {
                var data = _transferRepairItemToQcInfoBusiness.GetTransferRepairItemToQcInfos(User.OrgId);

                var dto = data.Select(s => new TransferRepairItemToQcInfoDTO
                {
                    TRQInfoId = s.TRQInfoId,
                    TransferCode = s.TransferCode,
                    LineName = _productionLineBusiness.GetProductionLineOneByOrgId(s.LineId.Value, User.OrgId).LineNumber,
                    ModelName = _descriptionBusiness.GetDescriptionOneByOrdId(s.DescriptionId.Value, User.OrgId).DescriptionName,
                    WarehouseName = _warehouseBusiness.GetWarehouseOneByOrgId(s.WarehouseId.Value, User.OrgId).WarehouseName,
                    QCLineName = _qualityControlBusiness.GetQualityControlById(s.QCLineId.Value, User.OrgId).QCName,
                    RepairLineName = _repairLineBusiness.GetRepairLineById(s.RepairLineId.Value, User.OrgId).RepairLineName,
                    StateStatus = s.StateStatus,
                    Remarks = s.Remarks,
                    EntryUser = UserForEachRecord(s.EUserId.Value).UserName,
                    UpdateUser = (s.UpUserId == null || s.UpUserId == 0) ? "" : UserForEachRecord(s.UpUserId.Value).UserName,
                    EntryDate = s.EntryDate,
                    ItemCount = _transferRepairItemToQcDetailBusiness.GetTransferRepairItemToQcDetailByInfo(s.TRQInfoId, User.OrgId).Count(),
                    ItemTypeId = s.ItemTypeId.Value,
                    ItemTypeName = _itemTypeBusiness.GetItemTypeOneByOrgId(s.ItemTypeId.Value, s.WarehouseId.Value, User.OrgId).ItemName,
                    ItemId = s.ItemId.Value,
                    ItemName = _itemBusiness.GetItemOneByOrgId(s.ItemId.Value, User.OrgId).ItemName,
                    ForQty = s.ForQty.Value,
                }).AsEnumerable();
                dto = dto.Where(t =>
                (repairLineId == null || repairLineId <= 0 || t.RepairLineId == repairLineId) &&
                (qcId == null || qcId <= 0 || t.QCLineId == qcId) &&
                (modelId == null || modelId <= 0 || t.DescriptionId == modelId) &&
                (lineId == null || lineId <= 0 || t.LineId == lineId) &&
                (warehouseId == null || warehouseId <= 0 || t.WarehouseId == warehouseId) &&
                (transferCode == null || transferCode.Trim() == "" || t.TransferCode.Contains(transferCode)) &&
                (status == null || status.Trim() == "" || t.StateStatus == status.Trim()) &&
                (
                    (fromDate == null && toDate == null)
                    ||
                     (fromDate == "" && toDate == "")
                    ||
                    (fromDate.Trim() != "" && toDate.Trim() != "" &&

                        t.EntryDate.Value.Date >= Convert.ToDateTime(fromDate).Date &&
                        t.EntryDate.Value.Date <= Convert.ToDateTime(toDate).Date)
                    ||
                    (fromDate.Trim() != "" && t.EntryDate.Value.Date == Convert.ToDateTime(fromDate).Date)
                    ||
                    (toDate.Trim() != "" && t.EntryDate.Value.Date == Convert.ToDateTime(toDate).Date)
                )).OrderByDescending(t => t.EntryDate).ToList();


                // Pagination //
                ViewBag.PagerData = GetPagerData(dto.Count(), pageSize, page);
                dto = dto.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                //-----------------//

                IEnumerable<TransferRepairItemToQcInfoViewModel> viewModels = new List<TransferRepairItemToQcInfoViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetTransferItemForQCList", viewModels);
            }
            else
            {
                var infoDomain = _transferRepairItemToQcInfoBusiness.GetTransferRepairItemToQcInfoById(transferInfoId.Value, User.OrgId);

                TransferRepairItemToQcInfoDTO info = new TransferRepairItemToQcInfoDTO
                {
                    TRQInfoId = infoDomain.TRQInfoId,
                    TransferCode = infoDomain.TransferCode,
                    ModelName = _descriptionBusiness.GetDescriptionOneByOrdId(infoDomain.DescriptionId.Value, User.OrgId).DescriptionName,
                    LineName = _productionLineBusiness.GetProductionLineOneByOrgId(infoDomain.LineId.Value, User.OrgId).LineNumber,
                    WarehouseName = _warehouseBusiness.GetWarehouseOneByOrgId(infoDomain.WarehouseId.Value, User.OrgId).WarehouseName,
                    StateStatus = infoDomain.StateStatus,
                    QCLineName = _qualityControlBusiness.GetQualityControlById(infoDomain.QCLineId.Value, User.OrgId).QCName,
                    ItemTypeName = _itemTypeBusiness.GetItemTypeOneByOrgId(infoDomain.ItemTypeId.Value, infoDomain.WarehouseId.Value, User.OrgId).ItemName,
                    ItemName = _itemBusiness.GetItemOneByOrgId(infoDomain.ItemId.Value, User.OrgId).ItemName,
                    ForQty = infoDomain.ForQty,
                    RepairLineName = _repairLineBusiness.GetRepairLineById(infoDomain.RepairLineId.Value, User.OrgId).RepairLineName
                };

                var details = _transferRepairItemToQcDetailBusiness.GetTransferRepairItemToQcDetailByInfo(transferInfoId.Value, User.OrgId)
                    .Select(s => new TransferRepairItemToQcDetailDTO
                    {
                        TRQDetailId = s.TRQDetailId,
                        WarehouseName = _warehouseBusiness.GetWarehouseOneByOrgId(s.WarehouseId.Value, User.OrgId).WarehouseName,
                        ItemTypeName = _itemTypeBusiness.GetItemTypeOneByOrgId(s.ItemTypeId.Value, s.WarehouseId.Value, User.OrgId).ItemName,
                        ItemName = _itemBusiness.GetItemOneByOrgId(s.ItemId.Value, User.OrgId).ItemName,
                        Quantity = s.Quantity,
                        UnitName = _unitBusiness.GetUnitOneByOrgId(s.UnitId.Value, User.OrgId).UnitSymbol,
                        Remarks = s.Remarks
                    }).ToList();

                TransferRepairItemToQcInfoViewModel viewModel = new TransferRepairItemToQcInfoViewModel();

                IEnumerable<TransferRepairItemToQcDetailViewModel> list = new List<TransferRepairItemToQcDetailViewModel>();

                AutoMapper.Mapper.Map(info, viewModel);
                AutoMapper.Mapper.Map(details, list);

                ViewBag.Info = viewModel;
                return PartialView("_GetTransferItemForQCDetail", list);
            }
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveTransferStockFromRepairStatus(long transferId, string status)
        {
            bool IsSuccess = false;
            if (transferId > 0 && !string.IsNullOrEmpty(status) && status.Trim() == RequisitionStatus.Accepted)
            {
                IsSuccess = _transferRepairItemToQcInfoBusiness.SaveTransferInfoStateStatus(transferId, status, User.UserId, User.OrgId);
            }
            return Json(IsSuccess);
        }



        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveQCPassQuantity(QCPassTransferInformationViewModel model)
        {
            bool IsSuccess = false;
            if (ModelState.IsValid)
            {
                QCPassTransferInformationDTO dTO = new QCPassTransferInformationDTO();
                AutoMapper.Mapper.Map(model, dTO);
                IsSuccess = _qCPassTransferInformationBusiness.SaveQCPassTransferInformation(dTO, User.UserId, User.OrgId);
            }
            return Json(IsSuccess);
        }

        public ActionResult GetQCPassList(string flag, long? floorId, long? assemblyId, long? modelId, long? qcId, long? warehouseId, long? itemTypeId, long? itemId, string status, string qcPassCode, long? qcPassId, string fromDate, string toDate, int page = 1)
        {
            if (string.IsNullOrEmpty(flag))
            {
                return new EmptyResult();
            }
            else if (!string.IsNullOrEmpty(flag) && flag.ToLower() == Flag.Search.ToLower())
            {
                var dto = _qCPassTransferInformationBusiness.GetQCPassTransferInformationsByQuery(floorId, assemblyId, qcId, modelId, warehouseId, itemTypeId, itemId, qcPassCode, status, fromDate, toDate, User.OrgId);

                // Pagination //
                //ViewBag.PagerData = GetPagerData(dto.Count(), pageSize, page);
                //dto = dto.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                //-----------------//
                List<QCPassTransferInformationViewModel> viewModels = new List<QCPassTransferInformationViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetQCPassList", viewModels);
            }
            else if (!string.IsNullOrEmpty(flag) && flag.ToLower() == Flag.Detail.ToLower())
            {
                var dto = _qCPassTransferDetailBusiness.GetQCPassTransferDetailsByQuery(floorId, assemblyId, qcId, string.Empty, qcPassCode, status, string.Empty, qcPassId, null, User.OrgId);

                List<QCPassTransferDetailViewModel> viewModels = new List<QCPassTransferDetailViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetQCPassQrCodes", viewModels);
            }
            return new EmptyResult();
        }


        [NonAction]
        private IEnumerable<TransferFromQCInfoDTO> QCStockTransferList(long? lineId, long? qcId, long? modelId, long? warehouseId, string status, string transferCode, string fromDate, string toDate, long? transferInfoId, string transferFor, long? repairLineId, long? packagingLineId, string transferReason)
        {
            var dto = _transferFromQCInfoBusiness.GetTransferFromQCInfos(User.OrgId).Select(s => new TransferFromQCInfoDTO
            {
                TFQInfoId = s.TFQInfoId,
                TransferCode = s.TransferCode,
                LineId = s.LineId,
                LineName = _productionLineBusiness.GetProductionLineOneByOrgId(s.LineId.Value, User.OrgId).LineNumber,
                DescriptionId = s.DescriptionId,
                ModelName = _descriptionBusiness.GetDescriptionOneByOrdId(s.DescriptionId.Value, User.OrgId).DescriptionName,
                WarehouseId = s.WarehouseId,
                WarehouseName = _warehouseBusiness.GetWarehouseOneByOrgId(s.WarehouseId.Value, User.OrgId).WarehouseName,
                QCLineId = s.LineId,
                QCLineName = _qualityControlBusiness.GetQualityControlById(s.QCLineId.Value, User.OrgId).QCName,
                RepairLineId = s.RepairLineId,
                RepairLineName = (s.RepairLineId == null || s.RepairLineId <= 0) ? "" : _repairLineBusiness.GetRepairLineById(s.RepairLineId.Value, User.OrgId).RepairLineName,
                PackagingLineId = s.PackagingLineId,
                PackagingLineName = (s.PackagingLineId == null || s.PackagingLineId <= 0) ? "" : _packagingLineBusiness.GetPackagingLineById(s.PackagingLineId.Value, User.OrgId).PackagingLineName,
                TransferFor = s.TransferFor,
                StateStatus = s.StateStatus,
                Remarks = s.Remarks,
                EntryUser = UserForEachRecord(s.EUserId.Value).UserName,
                UpdateUser = (s.UpUserId == null || s.UpUserId == 0) ? "" : UserForEachRecord(s.UpUserId.Value).UserName,
                EntryDate = s.EntryDate,
                ItemCount = _transferFromQCDetailBusiness.GetTransferFromQCDetailByInfo(s.TFQInfoId, User.OrgId).Count(),
                ItemTypeId = s.ItemTypeId,
                ItemTypeName = _itemTypeBusiness.GetItemTypeOneByOrgId(s.ItemTypeId.Value, s.WarehouseId.Value, User.OrgId).ItemName,
                ItemId = s.ItemId,
                ItemName = _itemBusiness.GetItemOneByOrgId(s.ItemId.Value, User.OrgId).ItemName,
                ForQty = s.ForQty.Value,
                RepairTransferReason = s.RepairTransferReason
            }).AsEnumerable();
            dto = dto.Where(t =>
            (transferFor == null || transferFor.Trim() == "" || t.TransferFor == transferFor.Trim()) &&
            (repairLineId == null || repairLineId <= 0 || t.RepairLineId == repairLineId) &&
            (packagingLineId == null || packagingLineId <= 0 || t.PackagingLineId == packagingLineId) &&
            (qcId == null || qcId <= 0 || t.QCLineId == qcId) &&
            (modelId == null || modelId <= 0 || t.DescriptionId == modelId) &&
            (lineId == null || lineId <= 0 || t.LineId == lineId) &&
            (warehouseId == null || warehouseId <= 0 || t.WarehouseId == warehouseId) &&
            (transferCode == null || transferCode.Trim() == "" || t.TransferCode.Contains(transferCode)) &&
            (transferReason == null || transferReason.Trim() == "" || t.RepairTransferReason == transferReason) &&
            (status == null || status.Trim() == "" || t.StateStatus == status.Trim()) &&
            (
                (fromDate == null && toDate == null)
                ||
                 (fromDate == "" && toDate == "")
                ||
                (fromDate.Trim() != "" && toDate.Trim() != "" &&

                    t.EntryDate.Value.Date >= Convert.ToDateTime(fromDate).Date &&
                    t.EntryDate.Value.Date <= Convert.ToDateTime(toDate).Date)
                ||
                (fromDate.Trim() != "" && t.EntryDate.Value.Date == Convert.ToDateTime(fromDate).Date)
                ||
                (toDate.Trim() != "" && t.EntryDate.Value.Date == Convert.ToDateTime(toDate).Date)
            )).OrderByDescending(t => t.EntryDate).ToList();

            return dto;
        }

        [NonAction]
        private void QCStockTransferDetail(long transferInfoId, out TransferFromQCInfoDTO info, out List<TransferFromQCDetailDTO> detail)
        {
            var infoDomain = _transferFromQCInfoBusiness.GetTransferFromQCInfoById(transferInfoId, User.OrgId);
            info = new TransferFromQCInfoDTO
            {
                TFQInfoId = infoDomain.TFQInfoId,
                TransferCode = infoDomain.TransferCode,
                ModelName = _descriptionBusiness.GetDescriptionOneByOrdId(infoDomain.DescriptionId.Value, User.OrgId).DescriptionName,
                LineName = _productionLineBusiness.GetProductionLineOneByOrgId(infoDomain.LineId.Value, User.OrgId).LineNumber,
                WarehouseName = _warehouseBusiness.GetWarehouseOneByOrgId(infoDomain.WarehouseId.Value, User.OrgId).WarehouseName,
                StateStatus = infoDomain.StateStatus,
                QCLineName = _qualityControlBusiness.GetQualityControlById(infoDomain.QCLineId.Value, User.OrgId).QCName,
                RepairLineId = infoDomain.RepairLineId,
                RepairLineName = _repairLineBusiness.GetRepairLineById(infoDomain.RepairLineId.Value, User.OrgId).RepairLineName,
                TransferFor = (infoDomain.TransferFor == "Repair Line" ? (_repairLineBusiness.GetRepairLineById(infoDomain.RepairLineId.Value, User.OrgId).RepairLineName) : _packagingLineBusiness.GetPackagingLineById(infoDomain.PackagingLineId.Value, User.OrgId).PackagingLineName),
                ItemTypeName = _itemTypeBusiness.GetItemTypeOneByOrgId(infoDomain.ItemTypeId.Value, infoDomain.WarehouseId.Value, User.OrgId).ItemName,
                ItemName = _itemBusiness.GetItemOneByOrgId(infoDomain.ItemId.Value, User.OrgId).ItemName,
                ForQty = infoDomain.ForQty,
                RepairTransferReason = infoDomain.RepairTransferReason,
            };

            detail = _transferFromQCDetailBusiness.GetTransferFromQCDetailByInfo(transferInfoId, User.OrgId)
            .Select(s => new TransferFromQCDetailDTO
            {
                TFQDetailId = s.TFQDetailId,
                WarehouseName = _warehouseBusiness.GetWarehouseOneByOrgId(s.WarehouseId.Value, User.OrgId).WarehouseName,
                ItemTypeName = _itemTypeBusiness.GetItemTypeOneByOrgId(s.ItemTypeId.Value, s.WarehouseId.Value, User.OrgId).ItemName,
                ItemName = _itemBusiness.GetItemOneByOrgId(s.ItemId.Value, User.OrgId).ItemName,
                Quantity = s.Quantity,
                UnitName = _unitBusiness.GetUnitOneByOrgId(s.UnitId.Value, User.OrgId).UnitSymbol,
                Remarks = s.Remarks
            }).ToList();
        }


        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveQCPassItemReceive(long qcPassId, string status)
        {
            bool IsSuccess = false;
            if (qcPassId > 0 && !string.IsNullOrWhiteSpace(status) && status == "Received By Production")
            {
                IsSuccess = _productionAssembleStockDetailBusiness.SaveReceiveQCItems(qcPassId, status, User.UserId, User.OrgId);
            }
            return Json(IsSuccess);
        }

        #endregion

        #region Packaging Line
        public ActionResult GetPackagingLines()
        {
            var dto = _packagingLineBusiness.GetPackagingLinesByOrgId(User.OrgId).Select(s => new PackagingLineDTO
            {
                PackagingLineId = s.PackagingLineId,
                PackagingLineName = s.PackagingLineName,
                IsActive = s.IsActive,
                ProductionLineId = s.ProductionLineId,
                ProductionLineName = _productionLineBusiness.GetProductionLineOneByOrgId(s.ProductionLineId, User.OrgId).LineNumber,
                Remarks = s.Remarks,
                EntryUser = UserForEachRecord(s.EUserId.Value).UserName,
                UpdateUser = (s.UpUserId == null || s.UpUserId == 0) ? "" : UserForEachRecord(s.UpUserId.Value).UserName,
                EntryDate = s.EntryDate,
                UpdateDate = s.UpdateDate
            }).ToList();
            ViewBag.UserPrivilege = UserPrivilege("Production", "GetPackagingLines");
            ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

            IEnumerable<PackagingLineViewModel> viewModels = new List<PackagingLineViewModel>();
            AutoMapper.Mapper.Map(dto, viewModels);
            return View(viewModels);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SavePackagingLines(PackagingLineViewModel model)
        {
            bool IsSuccess = false;
            if (ModelState.IsValid)
            {
                PackagingLineDTO dto = new PackagingLineDTO();
                AutoMapper.Mapper.Map(model, dto);
                IsSuccess = _packagingLineBusiness.SavePackagingLine(dto, User.UserId, User.OrgId);
            }
            return Json(IsSuccess);
        }

        // Receive Stock From QC
        public ActionResult GetReceiveStockFromQC(string flag, long? lineId, long? qcId, long? modelId, long? warehouseId, string status, string transferCode, string fromDate, string toDate, long? transferInfoId, long? packagingLineId, int page = 1)
        {
            ViewBag.UserPrivilege = UserPrivilege("Production", "GetReceiveStockFromQC");
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlStateStatus = Utility.ListOfReqStatus().Where(s => s.value == RequisitionStatus.Approved || s.value == RequisitionStatus.Accepted).Select(st => new SelectListItem
                {
                    Text = st.text,
                    Value = st.value
                }).ToList();

                ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

                ViewBag.ddlModelName = _descriptionBusiness.GetAllDescriptionsInProductionStock(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();

                ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(ware => new SelectListItem
                {
                    Text = ware.WarehouseName,
                    Value = ware.Id.ToString()
                }).ToList();
                return View();
            }
            else if (flag.ToLower() == Flag.View.ToLower() || flag.ToLower() == Flag.Search.ToLower())
            {
                var dto = QCStockTransferList(lineId, qcId, modelId, warehouseId, status, transferCode, fromDate, toDate, transferInfoId, "Packaging Line", null, packagingLineId, string.Empty);

                // Pagination //
                ViewBag.PagerData = GetPagerData(dto.Count(), pageSize, page);
                dto = dto.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                //-----------------//

                IEnumerable<TransferFromQCInfoViewModel> viewModels = new List<TransferFromQCInfoViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetReceiveStockFromQC", viewModels);
            }
            else
            {
                TransferFromQCInfoDTO info;
                List<TransferFromQCDetailDTO> details;

                QCStockTransferDetail(transferInfoId.Value, out info, out details);
                TransferFromQCInfoViewModel viewModel = new TransferFromQCInfoViewModel();

                IEnumerable<TransferFromQCDetailViewModel> list = new List<TransferFromQCDetailViewModel>();

                AutoMapper.Mapper.Map(info, viewModel);
                AutoMapper.Mapper.Map(details, list);

                ViewBag.Info = viewModel;
                return PartialView("_GetReceiveStockFromQCDetail", list);
            }
        }

        public ActionResult GetPackagingLineStockInfo(string flag, long? lineId, long? packagingId, long? qcId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string lessOrEq,long? returnId,string returnCode,string status,string fromDate,string toDate, int page = 1)
        {
            //ViewBag.UserPrivilege = UserPrivilege("Production", "GetPackagingLineStockInfo");
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlPackagingLineWithProduction = _packagingLineBusiness.GetPackagingLinesWithProduction(User.OrgId).Select(line => new SelectListItem { Text = line.text, Value = line.value.ToString() }).ToList();

                ViewBag.ddlModelName = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).Select(des => new SelectListItem { Text = des.DescriptionName, Value = des.DescriptionId.ToString() }).ToList();

                var allItems = _itemBusiness.GetItemDetails(User.OrgId);
                ViewBag.ddlItems = allItems.Where(s => !s.ItemName.Contains("Warehouse 3")).Select(s => new SelectListItem { Text = s.ItemName, Value = s.ItemId }).ToList();


                ViewBag.ddlHandSetItems = allItems.Where(s => s.ItemName.Contains("Warehouse 3")).Select(s => new SelectListItem { Text = s.ItemName, Value = s.ItemId }).ToList();

                ViewBag.ddlStateStatus = Utility.ListOfReqStatus().Where(s => s.value == RequisitionStatus.Approved || s.value == RequisitionStatus.Accepted).Select(st => new SelectListItem
                {
                    Text = st.text,
                    Value = st.value
                }).ToList();

                return View();
            }
            else if (!string.IsNullOrEmpty(flag) && flag.Trim() != "" && flag == "view")
            {
                //IEnumerable<PackagingLineStockInfoDTO> dto = _packagingLineStockInfoBusiness.GetPackagingLineStockInfos(User.OrgId).Select(info => new PackagingLineStockInfoDTO
                //{
                //    PLStockInfoId = info.PLStockInfoId,
                //    ProductionLineId = info.ProductionLineId.Value,
                //    ProductionLineName = _productionLineBusiness.GetProductionLineOneByOrgId(info.ProductionLineId.Value, User.OrgId).LineNumber,
                //    PackagingLineId = info.PackagingLineId,
                //    PackagingLineName = _packagingLineBusiness.GetPackagingLineById(info.PackagingLineId.Value, User.OrgId).PackagingLineName,
                //    //QCLineId = info.QCLineId,
                //    //QCLineName = _qualityControlBusiness.GetQualityControlById(info.QCLineId.Value, User.OrgId).QCName,
                //    DescriptionId = info.DescriptionId,
                //    ModelName = (_descriptionBusiness.GetDescriptionOneByOrdId(info.DescriptionId.Value, info.OrganizationId).DescriptionName),
                //    WarehouseId = info.WarehouseId,
                //    WarehouseName = (_warehouseBusiness.GetWarehouseOneByOrgId(info.WarehouseId.Value, User.OrgId).WarehouseName),
                //    ItemTypeId = info.ItemTypeId,
                //    ItemTypeName = (_itemTypeBusiness.GetItemType(info.ItemTypeId.Value, User.OrgId).ItemName),
                //    ItemId = info.ItemId,
                //    ItemName = (_itemBusiness.GetItemOneByOrgId(info.ItemId.Value, User.OrgId).ItemName),
                //    UnitId = info.UnitId,
                //    UnitName = (_unitBusiness.GetUnitOneByOrgId(info.UnitId.Value, User.OrgId).UnitSymbol),
                //    StockInQty = info.StockInQty,
                //    StockOutQty = info.StockOutQty,
                //    Remarks = info.Remarks,
                //    OrganizationId = info.OrganizationId

                //}).AsEnumerable();

                IEnumerable<PackagingLineStockInfoDTO> dto = _packagingLineStockInfoBusiness.GetPackagingLineStockInfosQuery(lineId, modelId, packagingId, warehouseId, itemTypeId, itemId, lessOrEq, User.OrgId);

                //dto = dto.Where(ws =>
                //(lineId == null || lineId == 0 || ws.ProductionLineId == lineId)
                //&& (packagingId == null || packagingId == 0 || ws.PackagingLineId == packagingId)
                //&& (qcId == null || qcId == 0 || ws.QCLineId == qcId)
                //&& (modelId == null || modelId == 0 || ws.DescriptionId == modelId)
                //&& (warehouseId == null || warehouseId == 0 || ws.WarehouseId == warehouseId)
                //&& (itemTypeId == null || itemTypeId == 0 || ws.ItemTypeId == itemTypeId)
                //&& (itemId == null || itemId == 0 || ws.ItemId == itemId)
                //&& (string.IsNullOrEmpty(lessOrEq) || (ws.StockInQty - ws.StockOutQty) <= Convert.ToInt32(lessOrEq))
                //).OrderByDescending(s => s.PLStockInfoId).ToList();

                //// Pagination //
                //ViewBag.PagerData = GetPagerData(dto.Count(), pageSize, page);
                //dto = dto.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                ////-----------------//
                List<PackagingLineStockInfoViewModel> viewModels = new List<PackagingLineStockInfoViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetPackagingLineStockInfo", viewModels);
            }
            else if (!string.IsNullOrEmpty(flag) && flag.Trim() != "" && flag == "StockReturnList")
            {
                var dto = _stockItemReturnInfoBusiness.GetStockItemReturnInfosByQuery(modelId, lineId, null, null, packagingId, warehouseId, returnId, returnCode, StockRetunFlag.PackagingLine, status, fromDate, toDate, User.OrgId);

                List<StockItemReturnInfoViewModel> viewModels = new List<StockItemReturnInfoViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);

                return PartialView("_GetStockReturnList", viewModels);
            }

            return View();
        }

        public ActionResult ReceiveSparePartsFromProductionRequisitionInPackaging(string flag, long? floorId, long? packagingId, long? warehouseId, long? modelId, string reqCode, string reqType, string reqFor, string fromDate, string toDate, string status, long? reqInfoId, string reqFlag)
        {
            status = ((status == null || status.Trim() == "") || (!status.Contains("Approved") && !status.Contains("Accepted"))) ? string.Format(@"'Approved','Accepted'") : string.Format(@"'{0}'", status);
            reqFor = reqFor == null ? "Packaging" : "Packaging";
            if (!string.IsNullOrEmpty(flag) && (flag.Trim().ToLower() == Flag.Info.ToLower()))
            {
                var dto = _requsitionInfoBusiness.GetRequsitionInfosByQuery(floorId, 0, packagingId, 0, warehouseId, modelId, reqCode, reqType, reqFor, fromDate, toDate, status, reqFlag, reqInfoId, User.OrgId);

                IEnumerable<RequsitionInfoViewModel> viewModels = new List<RequsitionInfoViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetRequisitionByItemInfo", viewModels);
            }
            else if (!string.IsNullOrEmpty(flag) && (flag.Trim().ToLower() == Flag.Detail.ToLower()))
            {
                var dto = _requisitionItemInfoBusiness.GetRequsitionInfoModalProcessData(floorId, null, packagingId, null, null, null, null, null, null, null, null, null, null, reqInfoId, User.OrgId);
                RequsitionInfoViewModel viewModel = new RequsitionInfoViewModel();
                ViewBag.Level = "Packaging";
                AutoMapper.Mapper.Map(dto, viewModel);
                return PartialView("_GetRequisitionItemDetail", viewModel);
            }
            return new EmptyResult();
        }

        public ActionResult CreatePackagingStockReturn()
        {
            ViewBag.ddlPackagingLineWithProduction = _packagingLineBusiness.GetPackagingLinesWithProduction(User.OrgId).Select(s => new SelectListItem
            {
                Text= s.text,
                Value = s.value
            }).ToList();

            ViewBag.ddlModelName = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).Select(s => new SelectListItem {
                Text = s.DescriptionName,
                Value = s.DescriptionId.ToString()
            }).ToList();
            return View();
        }

        public ActionResult GetPackagingStockItemsForReturn(long packagingLine,long floorId, long modelId)
        {
            var data = _packagingLineStockInfoBusiness.GetPackagingLineStocksForReturnStock(packagingLine, floorId, modelId, User.OrgId);
            IEnumerable<PackagingLineStockInfoViewModel> viewModels = new List<PackagingLineStockInfoViewModel>();
            AutoMapper.Mapper.Map(data, viewModels);
            return PartialView(viewModels);
        }


        public ActionResult CreatePackgingStockTransfer()
        {
            ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

            ViewBag.ddlModelName = _descriptionBusiness.GetAllDescriptionsInProductionStock(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();

            ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(ware => new SelectListItem
            {
                Text = ware.WarehouseName,
                Value = ware.Id.ToString()
            }).ToList();
            return View();
        }

        public ActionResult GetPackagingListTransferList(string flag, long? lineId, long? modelId, long? warehouseId, string status, string transferCode, string fromDate, string toDate, long? transferInfoId, long? packagingIdTo, long? packagingIdFrom, int page = 1)
        {
            ViewBag.UserPrivilege = UserPrivilege("Production", "GetPackagingListTransferList");
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlStateStatus = Utility.ListOfReqStatus().Where(s => s.value == RequisitionStatus.Approved || s.value == RequisitionStatus.Accepted).Select(st => new SelectListItem
                {
                    Text = st.text,
                    Value = st.value
                }).ToList();

                ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

                ViewBag.ddlModelName = _descriptionBusiness.GetAllDescriptionsInProductionStock(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();

                ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(ware => new SelectListItem
                {
                    Text = ware.WarehouseName,
                    Value = ware.Id.ToString()
                }).ToList();
                return View();
            }
            else if (flag.ToLower() == Flag.View.ToLower() || flag.ToLower() == Flag.Search.ToLower())
            {
                var dto = _transferStockToPackagingLine2InfoBusiness.GetStockToPL2Infos(User.OrgId).Select(s => new TransferStockToPackagingLine2InfoDTO
                {
                    TP2InfoId = s.TP2InfoId,
                    TransferCode = s.TransferCode,
                    LineName = _productionLineBusiness.GetProductionLineOneByOrgId(s.LineId.Value, User.OrgId).LineNumber,
                    ModelName = _descriptionBusiness.GetDescriptionOneByOrdId(s.DescriptionId.Value, User.OrgId).DescriptionName,
                    WarehouseName = _warehouseBusiness.GetWarehouseOneByOrgId(s.WarehouseId.Value, User.OrgId).WarehouseName,
                    PackagingLineNameFrom = _packagingLineBusiness.GetPackagingLineById(s.PackagingLineFromId.Value, User.OrgId).PackagingLineName,
                    PackagingLineNameTo = _packagingLineBusiness.GetPackagingLineById(s.PackagingLineToId.Value, User.OrgId).PackagingLineName,
                    StateStatus = s.StateStatus,
                    Remarks = s.Remarks,
                    EntryUser = UserForEachRecord(s.EUserId.Value).UserName,
                    UpdateUser = (s.UpUserId == null || s.UpUserId == 0) ? "" : UserForEachRecord(s.UpUserId.Value).UserName,
                    EntryDate = s.EntryDate,
                    ItemCount = _transferStockToPackagingLine2DetailBusiness.GetTransferFromPLDetailByInfo(s.TP2InfoId, User.OrgId).Count(),
                    ItemTypeId = s.ItemTypeId,
                    ItemTypeName = _itemTypeBusiness.GetItemTypeOneByOrgId(s.ItemTypeId.Value, s.WarehouseId.Value, User.OrgId).ItemName,
                    ItemId = s.ItemId,
                    ItemName = _itemBusiness.GetItemOneByOrgId(s.ItemId.Value, User.OrgId).ItemName,
                    ForQty = s.ForQty.Value,
                }).AsEnumerable();

                dto = dto.Where(t =>
                (packagingIdTo == null || packagingIdTo <= 0 || t.PackagingLineToId == packagingIdTo) &&
                (packagingIdFrom == null || packagingIdFrom <= 0 || t.PackagingLineFromId == packagingIdFrom) &&
                (modelId == null || modelId <= 0 || t.DescriptionId == modelId) &&
                (lineId == null || lineId <= 0 || t.LineId == lineId) &&
                (warehouseId == null || warehouseId <= 0 || t.WarehouseId == warehouseId) &&
                (transferCode == null || transferCode.Trim() == "" || t.TransferCode.Contains(transferCode)) &&
                (status == null || status.Trim() == "" || t.StateStatus == status.Trim()) &&
                (
                    (fromDate == null && toDate == null)
                    ||
                     (fromDate == "" && toDate == "")
                    ||
                    (fromDate.Trim() != "" && toDate.Trim() != "" &&

                        t.EntryDate.Value.Date >= Convert.ToDateTime(fromDate).Date &&
                        t.EntryDate.Value.Date <= Convert.ToDateTime(toDate).Date)
                    ||
                    (fromDate.Trim() != "" && t.EntryDate.Value.Date == Convert.ToDateTime(fromDate).Date)
                    ||
                    (toDate.Trim() != "" && t.EntryDate.Value.Date == Convert.ToDateTime(toDate).Date)
                )).OrderByDescending(t => t.EntryDate).ToList();

                // Pagination //
                ViewBag.PagerData = GetPagerData(dto.Count(), pageSize, page);
                dto = dto.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                //-----------------//

                IEnumerable<TransferStockToPackagingLine2InfoViewModel> viewModels = new List<TransferStockToPackagingLine2InfoViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetPackagingListTransferList", viewModels);
            }
            else
            {
                var infoDomain = _transferStockToPackagingLine2InfoBusiness.GetStockToPL2InfoById(transferInfoId.Value, User.OrgId);
                TransferStockToPackagingLine2InfoDTO info = new TransferStockToPackagingLine2InfoDTO
                {
                    TP2InfoId = infoDomain.TP2InfoId,
                    TransferCode = infoDomain.TransferCode,
                    ModelName = _descriptionBusiness.GetDescriptionOneByOrdId(infoDomain.DescriptionId.Value, User.OrgId).DescriptionName,
                    LineName = _productionLineBusiness.GetProductionLineOneByOrgId(infoDomain.LineId.Value, User.OrgId).LineNumber,
                    WarehouseName = _warehouseBusiness.GetWarehouseOneByOrgId(infoDomain.WarehouseId.Value, User.OrgId).WarehouseName,
                    StateStatus = infoDomain.StateStatus,
                    PackagingLineNameFrom = _packagingLineBusiness.GetPackagingLineById(infoDomain.PackagingLineFromId.Value, User.OrgId).PackagingLineName,
                    PackagingLineNameTo = _packagingLineBusiness.GetPackagingLineById(infoDomain.PackagingLineToId.Value, User.OrgId).PackagingLineName,
                    ItemTypeName = _itemTypeBusiness.GetItemTypeOneByOrgId(infoDomain.ItemTypeId.Value, infoDomain.WarehouseId.Value, User.OrgId).ItemName,
                    ItemName = _itemBusiness.GetItemOneByOrgId(infoDomain.ItemId.Value, User.OrgId).ItemName,
                    ForQty = infoDomain.ForQty,
                    Remarks = infoDomain.Remarks
                };

                var details = _transferStockToPackagingLine2DetailBusiness.GetTransferFromPLDetailByInfo(transferInfoId.Value, User.OrgId)
                    .Select(s => new TransferStockToPackagingLine2DetailDTO
                    {
                        TP2DetailId = s.TP2DetailId,
                        WarehouseName = _warehouseBusiness.GetWarehouseOneByOrgId(s.WarehouseId.Value, User.OrgId).WarehouseName,
                        ItemTypeName = _itemTypeBusiness.GetItemTypeOneByOrgId(s.ItemTypeId.Value, s.WarehouseId.Value, User.OrgId).ItemName,
                        ItemName = _itemBusiness.GetItemOneByOrgId(s.ItemId.Value, User.OrgId).ItemName,
                        Quantity = s.Quantity,
                        UnitName = _unitBusiness.GetUnitOneByOrgId(s.UnitId.Value, User.OrgId).UnitSymbol,
                        Remarks = s.Remarks
                    }).ToList();

                TransferStockToPackagingLine2InfoViewModel viewModel = new TransferStockToPackagingLine2InfoViewModel();

                IEnumerable<TransferStockToPackagingLine2DetailViewModel> list = new List<TransferStockToPackagingLine2DetailViewModel>();

                AutoMapper.Mapper.Map(info, viewModel);
                AutoMapper.Mapper.Map(details, list);

                ViewBag.Info = viewModel;
                return PartialView("_GetPackagingListTransferDetail", list);
            }
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SavePackagingTransfer(TransferStockToPackagingLine2InfoViewModel info, List<TransferStockToPackagingLine2DetailViewModel> details)
        {
            bool IsSuccess = false;
            if (ModelState.IsValid && details.Count > 0)
            {
                TransferStockToPackagingLine2InfoDTO infoDto = new TransferStockToPackagingLine2InfoDTO();
                List<TransferStockToPackagingLine2DetailDTO> detailDTO = new List<TransferStockToPackagingLine2DetailDTO>();
                AutoMapper.Mapper.Map(info, infoDto);
                AutoMapper.Mapper.Map(details, detailDTO);

                IsSuccess = _transferStockToPackagingLine2InfoBusiness.SaveTransferStockToPackaging2(infoDto, detailDTO, User.UserId, User.OrgId);
            }
            return Json(IsSuccess);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SavePackagingTransferStatus(long transferId, string status)
        {
            bool IsSuccess = false;
            if (transferId > 0 && !string.IsNullOrEmpty(status) && status.Trim() == RequisitionStatus.Accepted)
            {
                IsSuccess = _transferStockToPackagingLine2InfoBusiness.SaveTransferInfoStateStatus(transferId, status, User.UserId, User.OrgId);
            }
            return Json(IsSuccess);
        }

        public ActionResult GetPackagingItemStockList(string flag, long? floorId, long? modelId, long? packagingLineId, long? warehouseId, long? itemTypeId, long? itemId, string lessOrEq, int page = 1)
        {
            //ViewBag.UserPrivilege = UserPrivilege("Production", "GetPackagingItemStockList");
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

                ViewBag.ddlModelName = _descriptionBusiness.GetAllDescriptionsInProductionStock(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();

                ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(ware => new SelectListItem
                {
                    Text = ware.WarehouseName,
                    Value = ware.Id.ToString()
                }).ToList();
                return View();
            }
            else
            {

                var dto = _packagingItemStockInfoBusiness.GetPackagingItemStockInfosByQuery(floorId, packagingLineId, modelId, warehouseId, itemTypeId, itemId, lessOrEq, User.OrgId);
                // var dto = _packagingItemStockInfoBusiness.GetPackagingItemStocks(User.OrgId).Select(s => new PackagingItemStockInfoDTO
                // {
                //     ProductionFloorId = s.ProductionFloorId.Value,
                //     ProductionFloorName = _productionLineBusiness.GetProductionLineOneByOrgId(s.ProductionFloorId.Value, User.OrgId).LineNumber,
                //     DescriptionId = s.DescriptionId.Value,
                //     ModelName = _descriptionBusiness.GetDescriptionOneByOrdId(s.DescriptionId.Value, User.OrgId).DescriptionName,
                //     PackagingLineId = s.PackagingLineId.Value,
                //     PackagingLineName = _packagingLineBusiness.GetPackagingLineById(s.PackagingLineId.Value, User.OrgId).PackagingLineName,
                //     WarehouseId = s.WarehouseId.Value,
                //     WarehouseName = _warehouseBusiness.GetWarehouseOneByOrgId(s.WarehouseId.Value, User.OrgId).WarehouseName,
                //     ItemTypeId = s.ItemTypeId.Value,
                //     ItemTypeName = _itemTypeBusiness.GetItemTypeOneByOrgId(s.ItemTypeId.Value, s.WarehouseId.Value, User.OrgId).ItemName,
                //     ItemId = s.ItemId.Value,
                //     ItemName = _itemBusiness.GetItemById(s.ItemId.Value, User.OrgId).ItemName,
                //     Quantity = s.Quantity,
                //     TransferQty = s.TransferQty

                // }).ToList();

                // dto = dto.Where(ws =>
                //(floorId == null || floorId == 0 || ws.ProductionFloorId == floorId)
                //&& (modelId == null || modelId == 0 || ws.DescriptionId == modelId)
                //&& (packagingLineId == null || packagingLineId == 0 || ws.PackagingLineId == packagingLineId)
                //&& (warehouseId == null || warehouseId == 0 || ws.WarehouseId == warehouseId)
                //&& (itemTypeId == null || itemTypeId == 0 || ws.ItemTypeId == itemTypeId)
                //&& (itemId == null || itemId == 0 || ws.ItemId == itemId)
                //&& (string.IsNullOrEmpty(lessOrEq) || (ws.Quantity - ws.TransferQty) <= Convert.ToInt32(lessOrEq))
                //).OrderByDescending(s => s.PItemStockInfoId).ToList();

                // Pagination //
                //ViewBag.PagerData = GetPagerData(dto.Count(), pageSize, page);
                //dto = dto.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                //-----------------//
                List<PackagingItemStockInfoViewModel> viewModels = new List<PackagingItemStockInfoViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetPackagingItemStockList", viewModels);
            }
        }

        public ActionResult GetProductionAssembleStockTransferReceiveList(string flag, long? floorId, long? packagingLineId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, long? transferInfoId, string status)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlStateStatus = Utility.ListOfReqStatus().Where(s => s.value == RequisitionStatus.Approved || s.value == RequisitionStatus.Accepted).Select(st => new SelectListItem
                {
                    Text = st.text,
                    Value = st.value
                }).ToList();

                ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

                ViewBag.ddlModelName = _descriptionBusiness.GetAllDescriptionsInProductionStock(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();

                ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(ware => new SelectListItem
                {
                    Text = ware.WarehouseName,
                    Value = ware.Id.ToString()
                }).ToList();

                return View();
            }
            else if (!string.IsNullOrEmpty(flag) && flag.ToLower() == Flag.View.ToLower() || flag.ToLower() == Flag.Search.ToLower())
            {
                var dto = _productionToPackagingStockTransferInfoBusiness.GetProductionToPackagingStockTransferInfosByQuery(floorId, packagingLineId, modelId, warehouseId, itemTypeId, itemId, status, User.OrgId);

                // Pagination //
                //ViewBag.PagerData = GetPagerData(dto.Count(), pageSize, page);
                //dto = dto.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                //-----------------//
                List<ProductionToPackagingStockTransferInfoViewModel> viewModels = new List<ProductionToPackagingStockTransferInfoViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);

                return PartialView("_GetProductionAssembleStockTransferList", viewModels);
            }
            else
            {
                var dto = _productionToPackagingStockTransferDetailBusiness.GetProductionToPackagingStockTransferDetailsByQuery(transferInfoId.Value, User.OrgId);
                List<ProductionToPackagingStockTransferDetailViewModel> viewModels = new List<ProductionToPackagingStockTransferDetailViewModel>();
                ViewBag.Status = _productionToPackagingStockTransferInfoBusiness.GetProductionToPackagingStockTransferInfoById(transferInfoId.Value, User.OrgId).StateStatus;

                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetProductionAssembleStockTransferListDetail", viewModels);
            }
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveReceiveProductionAssembleStockForPackaging(long transferId, string status)
        {
            bool IsSuccess = false;
            if (transferId > 0 && !string.IsNullOrEmpty(status) && status == RequisitionStatus.Accepted)
            {
                IsSuccess = _productionToPackagingStockTransferInfoBusiness.SaveProductionToPackagingStockTransferState(transferId, status, User.UserId, User.OrgId);
            }
            return Json(IsSuccess);
        }

        #endregion

        #region Repair Line
        public ActionResult GetRepairLines()
        {
            var dto = _repairLineBusiness.GetRepairLinesByOrgId(User.OrgId).Select(s => new RepairLineDTO
            {
                RepairLineId = s.RepairLineId,
                RepairLineName = s.RepairLineName,
                IsActive = s.IsActive,
                ProductionLineId = s.ProductionLineId,
                ProductionLineName = _productionLineBusiness.GetProductionLineOneByOrgId(s.ProductionLineId, User.OrgId).LineNumber,
                Remarks = s.Remarks,
                EntryUser = UserForEachRecord(s.EUserId.Value).UserName,
                UpdateUser = (s.UpUserId == null || s.UpUserId == 0) ? "" : UserForEachRecord(s.UpUserId.Value).UserName,
                EntryDate = s.EntryDate,
                UpdateDate = s.UpdateDate
            }).ToList();

            ViewBag.UserPrivilege = UserPrivilege("Production", "GetRepairLines");
            ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

            IEnumerable<RepairLineViewModel> viewModels = new List<RepairLineViewModel>();
            AutoMapper.Mapper.Map(dto, viewModels);
            return View(viewModels);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveRepairLine(RepairLineViewModel model)
        {
            bool IsSuccess = false;
            if (ModelState.IsValid)
            {
                RepairLineDTO dto = new RepairLineDTO();
                AutoMapper.Mapper.Map(model, dto);
                IsSuccess = _repairLineBusiness.SaveRepairLine(dto, User.UserId, User.OrgId);
            }
            return Json(IsSuccess);
        }

        #region Repair Item Stock Group
        public ActionResult GetRepairItemStockList(string flag, long? floorId, long? assembly, long? modelId, long? qcId, long? repairId, long? warehouseId, long? itemTypeId, long? itemId, string lessOrEq, int page = 1)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlFloor = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

                ViewBag.ddlModelName = _descriptionBusiness.GetAllDescriptionsInProductionStock(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();

                ViewBag.ddlItems = _itemBusiness.GetItemDetails(User.OrgId).Select(s => new SelectListItem { Text = s.ItemName, Value = s.ItemId }).ToList();

                ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(ware => new SelectListItem
                {
                    Text = ware.WarehouseName,
                    Value = ware.Id.ToString()
                }).ToList();

                ViewBag.ddlStateStatus = Utility.ListOfReqStatus().Where(s => s.value == RequisitionStatus.Approved || s.value == RequisitionStatus.Accepted).Select(st => new SelectListItem
                {
                    Text = st.text,
                    Value = st.value
                }).ToList();
                return View();
            }
            else
            {
                var dto = _repairItemStockInfoBusiness.GetRepairItemStockInfosByQuery(floorId, assembly, modelId, qcId, repairId, warehouseId, itemTypeId, itemId, lessOrEq, User.OrgId);

                // Pagination //
                ViewBag.PagerData = GetPagerData(dto.Count(), pageSize, page);
                dto = dto.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                //-----------------//
                IEnumerable<RepairItemStockInfoViewModel> viewModels = new List<RepairItemStockInfoViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);

                return PartialView("_GetRepairItemStockList", viewModels);
            }
        }

        // Receive QC Transfer List. // Child Action
        public ActionResult GetReceiveStockForRepairFromQC(string flag, long? lineId, long? qcId, long? modelId, long? warehouseId, string status, string transferCode, string fromDate, string toDate, long? transferInfoId, long? repairLineId, string transferReason, int page = 1)
        {
            ViewBag.UserPrivilege = UserPrivilege("Production", "GetReceiveStockForRepairFromQC");
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlStateStatus = Utility.ListOfReqStatus().Where(s => s.value == RequisitionStatus.Approved || s.value == RequisitionStatus.Accepted).Select(st => new SelectListItem
                {
                    Text = st.text,
                    Value = st.value
                }).ToList();

                ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

                ViewBag.ddlModelName = _descriptionBusiness.GetAllDescriptionsInProductionStock(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();

                ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(ware => new SelectListItem
                {
                    Text = ware.WarehouseName,
                    Value = ware.Id.ToString()
                }).ToList();
                return View();
            }
            else if (flag.ToLower() == Flag.View.ToLower() || flag.ToLower() == Flag.Search.ToLower())
            {
                //var dto = QCStockTransferList(lineId, qcId, modelId, warehouseId, status, transferCode, fromDate, toDate, transferInfoId, "Repair Line", repairLineId, null, transferReason);

                var dto = _transferFromQCInfoBusiness.GetTransferFromQCInfos(lineId, qcId, repairLineId, modelId, warehouseId, null, null, status, fromDate, toDate, transferCode, transferInfoId, User.OrgId);

                //_qCItemStockInfoBusiness.GetQCItemStockInfoByFloorAndQcAndModelAndItemAsync(,)

                // Pagination //
                ViewBag.PagerData = GetPagerData(dto.Count(), pageSize, page);
                dto = dto.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                //-----------------//

                IEnumerable<TransferFromQCInfoViewModel> viewModels = new List<TransferFromQCInfoViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetReceiveStockForRepairFromQC", viewModels);
            }
            else
            {

                TransferFromQCInfoDTO info = _transferFromQCInfoBusiness.GetTransferFromQCInfos(null, null, null, null, null, null, null, null, null, null, null, transferInfoId, User.OrgId).FirstOrDefault();

                IEnumerable<TransferFromQCDetailDTO> details = _transferFromQCDetailBusiness.GetTransferFromQCDetailDTO(transferInfoId.Value, User.OrgId);

                IEnumerable<QRCodeTransferToRepairInfoDTO> qrCodeDetail = _qRCodeTransferToRepairInfoBusiness.GetQRCodeTransferToRepairInfoByTransferIdByQuery(transferInfoId.Value, User.OrgId);

                TransferFromQCInfoViewModel viewModel = new TransferFromQCInfoViewModel();

                IEnumerable<TransferFromQCDetailViewModel> list = new List<TransferFromQCDetailViewModel>();

                IEnumerable<QRCodeTransferToRepairInfoViewModel> qrCodeViewModel = new List<QRCodeTransferToRepairInfoViewModel>();

                AutoMapper.Mapper.Map(info, viewModel);
                AutoMapper.Mapper.Map(details, list);
                AutoMapper.Mapper.Map(qrCodeDetail, qrCodeViewModel);

                ViewBag.Info = viewModel;
                ViewBag.QrCodeDetail = qrCodeViewModel;
                return PartialView("_GetReceiveStockForRepairFromQCDetail", list);
            }
        }

        // Receive QC Item
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveQCTransferStatus(long transferId, string status)
        {
            bool IsSuccess = false;
            if (transferId > 0 && status == RequisitionStatus.Accepted)
            {
                //IsSuccess = _transferFromQCInfoBusiness.SaveTransferInfoStateStatus(transferId, status, User.UserId, User.OrgId);
                IsSuccess = _qRCodeTransferToRepairInfoBusiness.SaveQRCodeStatusByTrasnferInfoId(transferId, status, User.UserId, User.OrgId);
            }
            return Json(IsSuccess);
        }

        // Transfer List // Child Action
        public ActionResult GetRepairItemTransferInfoAndDetailByQRCode(string flag, long? floorId, long? assemblyId, long? repairLineId, long? qcLineId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string status, string transferCode, string fromDate, string toDate, long? transferInfoId)
        {
            if (!string.IsNullOrEmpty(flag) && flag == Flag.View)
            {
                var dto = _transferRepairItemToQcInfoBusiness.GetTransferRepairItemToQcInfosByQuery(floorId, assemblyId, repairLineId, qcLineId, modelId, warehouseId, itemTypeId, itemId, status, transferCode, fromDate, toDate, User.OrgId);
                IEnumerable<TransferRepairItemToQcInfoViewModel> viewModels = new List<TransferRepairItemToQcInfoViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetRepairItemTransferInfoByQRCode", viewModels);
            }
            else
            {
                ViewBag.StateStatus = _transferRepairItemToQcInfoBusiness.GetTransferRepairItemToQcInfoById(transferInfoId.Value, User.OrgId).StateStatus;
                var dto = _transferRepairItemToQcDetailBusiness.GetTransferRepairItemToQcDetailByQuery(transferInfoId.Value, User.OrgId);
                IEnumerable<TransferRepairItemToQcDetailViewModel> viewModels = new List<TransferRepairItemToQcDetailViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetRepairItemTransferDetailByQRCode", viewModels);
            }
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public async Task<ActionResult> SaveTransferInfoStateStatusAsync(long transferId, string status)
        {
            bool IsSuccess = false;
            if (transferId > 0 && !string.IsNullOrEmpty(status) && !string.IsNullOrWhiteSpace(status))
            {
                IsSuccess = await _transferRepairItemToQcInfoBusiness.SaveTransferInfoStateStatusAsync(transferId, status, User.UserId, User.OrgId);
            }

            return Json(IsSuccess);
        }
        #endregion

        #region Repair Spare parts group
        public ActionResult GetRepairLineStockInfo(string flag, long? lineId, long? repairId, long? qcId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string lessOrEq,long? returnId, string status,string returnCode, string fromDate,string toDate, int page = 1)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

                ViewBag.ddlModelName = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).Select(des => new SelectListItem { Text = des.DescriptionName, Value = des.DescriptionId.ToString() }).ToList();

                ViewBag.ddlItems = _itemBusiness.GetItemDetails(User.OrgId).Where(s => !s.ItemName.Contains("Warehouse 3")).Select(s => new SelectListItem { Text = s.ItemName, Value = s.ItemId }).ToList();

                ViewBag.ddlMobileItems = _itemBusiness.GetItemDetails(User.OrgId).Where(s => s.ItemName.Contains("Warehouse 3")).Select(s => new SelectListItem { Text = s.ItemName, Value = s.ItemId }).ToList();

                ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(ware => new SelectListItem
                {
                    Text = ware.WarehouseName,
                    Value = ware.Id.ToString()
                }).ToList();

                ViewBag.ddlRepairLine = _repairLineBusiness.GetRepairLineWithFloor(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();

                ViewBag.ddlStateStatus = Utility.ListOfReqStatus().Where(s => s.value == RequisitionStatus.Approved || s.value == RequisitionStatus.Accepted).Select(st => new SelectListItem
                {
                    Text = st.text,
                    Value = st.value
                }).ToList();

                return View();
            }
            else if(!string.IsNullOrEmpty(flag) && flag=="search")
            {
                IEnumerable<RepairLineStockInfoDTO> dto = _repairLineStockInfoBusiness.GetRepairLineStockInfosQuery(lineId, modelId, qcId, repairId, warehouseId, itemTypeId, itemId, lessOrEq, User.OrgId);
                // Pagination //
                //ViewBag.PagerData = GetPagerData(dto.Count(), pageSize, page);
                //dto = dto.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                //-----------------//
                List<RepairLineStockInfoViewModel> viewModels = new List<RepairLineStockInfoViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetRepairLineStockInfo", viewModels);
            }
            else if (!string.IsNullOrEmpty(flag) && flag == "StockReturnList")
            {
                var dto = _stockItemReturnInfoBusiness.GetStockItemReturnInfosByQuery(modelId, lineId, repairId, null, null, warehouseId, returnId, returnCode, StockRetunFlag.AssemblyRepair, status, fromDate, toDate, User.OrgId);

                List<StockItemReturnInfoViewModel> viewModels = new List<StockItemReturnInfoViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);

                return PartialView("_GetStockReturnList", viewModels);
            }
            return View();
        }

        public ActionResult CreateRepairStockReturn()
        {
            ViewBag.ddlRepairLineWithProduction = _repairLineBusiness.GetRepairLineWithFloor(User.OrgId).Select(s => new SelectListItem
            {
                Text = s.text,
                Value = s.value
            }).ToList();
            ViewBag.ddlModelName = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).Select(s => new SelectListItem
            {
                Text = s.DescriptionName,
                Value = s.DescriptionId.ToString()
            }).ToList();
            return View();
        }
        
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult GetRepairStockItemsForReturn(long repairLinesId, long floorId, long modelId)
        {
            var data = _repairLineStockInfoBusiness.GetRepairLineStocksForReturnStock(repairLinesId, floorId, modelId, User.OrgId);
            IEnumerable<RepairLineStockInfoViewModel> viewModels = new List<RepairLineStockInfoViewModel>();
            AutoMapper.Mapper.Map(data, viewModels);
            return PartialView(viewModels);
        }

        #endregion

        public ActionResult CreateRepairStockTransfer()
        {
            ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

            ViewBag.ddlModelName = _descriptionBusiness.GetAllDescriptionsInProductionStock(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();

            ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(ware => new SelectListItem
            {
                Text = ware.WarehouseName,
                Value = ware.Id.ToString()
            }).ToList();
            return View();
        }

        // Obsolete
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveTransferStockRepairItemToQC(TransferRepairItemToQcInfoViewModel info, List<TransferRepairItemToQcDetailViewModel> details)
        {
            bool IsSuccess = false;
            if (ModelState.IsValid && details.Count() > 0)
            {
                TransferRepairItemToQcInfoDTO infoDTO = new TransferRepairItemToQcInfoDTO();
                List<TransferRepairItemToQcDetailDTO> detailDTO = new List<TransferRepairItemToQcDetailDTO>();
                AutoMapper.Mapper.Map(info, infoDTO);
                AutoMapper.Mapper.Map(details, detailDTO);
                IsSuccess = _transferRepairItemToQcInfoBusiness.SaveTransfer(infoDTO, detailDTO, User.UserId, User.OrgId);
            }
            return Json(IsSuccess);
        }

        public ActionResult GetTransferItemForQCList(string flag, long? lineId, long? qcId, long? modelId, long? warehouseId, string status, string transferCode, string fromDate, string toDate, long? transferInfoId, long? repairLineId, int page = 1)
        {
            ViewBag.UserPrivilege = UserPrivilege("Production", "GetTransferItemForQCList");
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlStateStatus = Utility.ListOfReqStatus().Where(s => s.value == RequisitionStatus.Approved || s.value == RequisitionStatus.Accepted).Select(st => new SelectListItem
                {
                    Text = st.text,
                    Value = st.value
                }).ToList();

                ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

                ViewBag.ddlModelName = _descriptionBusiness.GetAllDescriptionsInProductionStock(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();

                ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(ware => new SelectListItem
                {
                    Text = ware.WarehouseName,
                    Value = ware.Id.ToString()
                }).ToList();
                return View();
            }
            else if (flag.ToLower() == Flag.View.ToLower() || flag.ToLower() == Flag.Search.ToLower())
            {
                var data = _transferRepairItemToQcInfoBusiness.GetTransferRepairItemToQcInfos(User.OrgId);

                var dto = data.Select(s => new TransferRepairItemToQcInfoDTO
                {
                    TRQInfoId = s.TRQInfoId,
                    TransferCode = s.TransferCode,
                    LineName = _productionLineBusiness.GetProductionLineOneByOrgId(s.LineId.Value, User.OrgId).LineNumber,
                    ModelName = _descriptionBusiness.GetDescriptionOneByOrdId(s.DescriptionId.Value, User.OrgId).DescriptionName,
                    WarehouseName = _warehouseBusiness.GetWarehouseOneByOrgId(s.WarehouseId.Value, User.OrgId).WarehouseName,
                    QCLineName = _qualityControlBusiness.GetQualityControlById(s.QCLineId.Value, User.OrgId).QCName,
                    RepairLineName = _repairLineBusiness.GetRepairLineById(s.RepairLineId.Value, User.OrgId).RepairLineName,
                    StateStatus = s.StateStatus,
                    Remarks = s.Remarks,
                    EntryUser = UserForEachRecord(s.EUserId.Value).UserName,
                    UpdateUser = (s.UpUserId == null || s.UpUserId == 0) ? "" : UserForEachRecord(s.UpUserId.Value).UserName,
                    EntryDate = s.EntryDate,
                    ItemCount = _transferRepairItemToQcDetailBusiness.GetTransferRepairItemToQcDetailByInfo(s.TRQInfoId, User.OrgId).Count(),
                    ItemTypeId = s.ItemTypeId.Value,
                    ItemTypeName = _itemTypeBusiness.GetItemTypeOneByOrgId(s.ItemTypeId.Value, s.WarehouseId.Value, User.OrgId).ItemName,
                    ItemId = s.ItemId.Value,
                    ItemName = _itemBusiness.GetItemOneByOrgId(s.ItemId.Value, User.OrgId).ItemName,
                    ForQty = s.ForQty.Value,
                }).AsEnumerable();
                dto = dto.Where(t =>
                (repairLineId == null || repairLineId <= 0 || t.RepairLineId == repairLineId) &&
                (qcId == null || qcId <= 0 || t.QCLineId == qcId) &&
                (modelId == null || modelId <= 0 || t.DescriptionId == modelId) &&
                (lineId == null || lineId <= 0 || t.LineId == lineId) &&
                (warehouseId == null || warehouseId <= 0 || t.WarehouseId == warehouseId) &&
                (transferCode == null || transferCode.Trim() == "" || t.TransferCode.Contains(transferCode)) &&
                (status == null || status.Trim() == "" || t.StateStatus == status.Trim()) &&
                (
                    (fromDate == null && toDate == null)
                    ||
                     (fromDate == "" && toDate == "")
                    ||
                    (fromDate.Trim() != "" && toDate.Trim() != "" &&

                        t.EntryDate.Value.Date >= Convert.ToDateTime(fromDate).Date &&
                        t.EntryDate.Value.Date <= Convert.ToDateTime(toDate).Date)
                    ||
                    (fromDate.Trim() != "" && t.EntryDate.Value.Date == Convert.ToDateTime(fromDate).Date)
                    ||
                    (toDate.Trim() != "" && t.EntryDate.Value.Date == Convert.ToDateTime(toDate).Date)
                )).OrderByDescending(t => t.EntryDate).ToList();


                // Pagination //
                ViewBag.PagerData = GetPagerData(dto.Count(), pageSize, page);
                dto = dto.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                //-----------------//

                IEnumerable<TransferRepairItemToQcInfoViewModel> viewModels = new List<TransferRepairItemToQcInfoViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetTransferItemForQCList", viewModels);
            }
            else
            {
                var infoDomain = _transferRepairItemToQcInfoBusiness.GetTransferRepairItemToQcInfoById(transferInfoId.Value, User.OrgId);

                TransferRepairItemToQcInfoDTO info = new TransferRepairItemToQcInfoDTO
                {
                    TRQInfoId = infoDomain.TRQInfoId,
                    TransferCode = infoDomain.TransferCode,
                    ModelName = _descriptionBusiness.GetDescriptionOneByOrdId(infoDomain.DescriptionId.Value, User.OrgId).DescriptionName,
                    LineName = _productionLineBusiness.GetProductionLineOneByOrgId(infoDomain.LineId.Value, User.OrgId).LineNumber,
                    WarehouseName = _warehouseBusiness.GetWarehouseOneByOrgId(infoDomain.WarehouseId.Value, User.OrgId).WarehouseName,
                    StateStatus = infoDomain.StateStatus,
                    QCLineName = _qualityControlBusiness.GetQualityControlById(infoDomain.QCLineId.Value, User.OrgId).QCName,
                    ItemTypeName = _itemTypeBusiness.GetItemTypeOneByOrgId(infoDomain.ItemTypeId.Value, infoDomain.WarehouseId.Value, User.OrgId).ItemName,
                    ItemName = _itemBusiness.GetItemOneByOrgId(infoDomain.ItemId.Value, User.OrgId).ItemName,
                    ForQty = infoDomain.ForQty,
                    RepairLineName = _repairLineBusiness.GetRepairLineById(infoDomain.RepairLineId.Value, User.OrgId).RepairLineName
                };

                var details = _transferRepairItemToQcDetailBusiness.GetTransferRepairItemToQcDetailByInfo(transferInfoId.Value, User.OrgId)
                    .Select(s => new TransferRepairItemToQcDetailDTO
                    {
                        TRQDetailId = s.TRQDetailId,
                        WarehouseName = _warehouseBusiness.GetWarehouseOneByOrgId(s.WarehouseId.Value, User.OrgId).WarehouseName,
                        ItemTypeName = _itemTypeBusiness.GetItemTypeOneByOrgId(s.ItemTypeId.Value, s.WarehouseId.Value, User.OrgId).ItemName,
                        ItemName = _itemBusiness.GetItemOneByOrgId(s.ItemId.Value, User.OrgId).ItemName,
                        Quantity = s.Quantity,
                        UnitName = _unitBusiness.GetUnitOneByOrgId(s.UnitId.Value, User.OrgId).UnitSymbol,
                        Remarks = s.Remarks
                    }).ToList();

                TransferRepairItemToQcInfoViewModel viewModel = new TransferRepairItemToQcInfoViewModel();

                IEnumerable<TransferRepairItemToQcDetailViewModel> list = new List<TransferRepairItemToQcDetailViewModel>();

                AutoMapper.Mapper.Map(info, viewModel);
                AutoMapper.Mapper.Map(details, list);

                ViewBag.Info = viewModel;
                return PartialView("_GetTransferItemForQCDetail", list);
            }
        }

        #endregion

        #region Faulty Stock By Repair

        #region Faulty Item Stock
        public ActionResult GetFaultyItemStockInfo(string flag, long? lineId, long? repairId, long? qcId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string lessOrEq,string reqFor, int page = 1)
        {
            //ViewBag.UserPrivilege = UserPrivilege("Production", "GetFaultyItemStockInfo");
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

                ViewBag.ddlModelName = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).Select(des => new SelectListItem { Text = des.DescriptionName, Value = des.DescriptionId.ToString() }).OrderBy(s => s.Text).ToList();

                ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(ware => new SelectListItem
                {
                    Text = ware.WarehouseName,
                    Value = ware.Id.ToString()
                }).ToList();

                ViewBag.ddlItems = _itemBusiness.GetItemDetails(User.OrgId).Select(s => new SelectListItem { Text = s.ItemName, Value = s.ItemId }).ToList();

                ViewBag.ddlStateStatus = Utility.ListOfReqStatus().Where(s => s.text == RequisitionStatus.Accepted || s.text == RequisitionStatus.Approved).Select(st => new SelectListItem
                {
                    Text = st.text,
                    Value = st.value
                }).ToList();

                return View();
            }
            else
            {
                lessOrEq = "0";
                IEnumerable<FaultyItemStockInfoDTO> dto = _faultyItemStockInfoBusiness.GetFaultyItemStockInfosByQuery(lineId, repairId, modelId, warehouseId, itemTypeId, itemId, lessOrEq,reqFor, User.OrgId);

                // Pagination //
                //ViewBag.PagerData = GetPagerData(dto.Count(), pageSize, page);
                //dto = dto.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                //-----------------//
                List<FaultyItemStockInfoViewModel> viewModels = new List<FaultyItemStockInfoViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetFaultyItemStockInfo", viewModels);
            }
        }

        public ActionResult CreateRepairFaultyStock()
        {

            ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

            ViewBag.ddlModelName = _descriptionBusiness.GetAllDescriptionsInProductionStock(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();

            ViewBag.ddlItem = _itemBusiness.GetItemDetails(User.OrgId).Select(i => new SelectListItem() { Text = i.ItemName, Value = i.ItemId.ToString() }).ToList();

            return View();
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveFaultyItemStock(List<FaultyItemStockDetailViewModel> details)
        {
            bool IsSuccess = false;
            if (ModelState.IsValid && details.Count > 0)
            {
                List<FaultyItemStockDetailDTO> dto = new List<FaultyItemStockDetailDTO>();
                AutoMapper.Mapper.Map(details, dto);
                IsSuccess = _repairLineStockDetailBusiness.StockOutByFaultyItem(dto, User.UserId, User.OrgId);
            }
            return Json(IsSuccess);
        }
        #endregion

        #region Adding Faulty Item By QRCode 
        public ActionResult GetFaultyItemByQRCodeList(string flag, string repairCode, long? floorId, long? repairLineId, long? qcLineId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string fromDate, string toDate, int page = 1)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlRepairLine = _repairLineBusiness.GetRepairLineWithFloor(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();
                ViewBag.ddlModelName = _descriptionBusiness.GetAllDescriptionsInProductionStock(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value.ToString() }).ToList();
                return View();
            }
            else
            {
                var dto = _repairItemBusiness.GetRepairItemInfoList(repairCode, floorId, repairLineId, qcLineId, modelId, warehouseId, itemTypeId, itemId, fromDate, toDate, User.OrgId);
                // Pagination //
                ViewBag.PagerData = GetPagerData(dto.Count(), pageSize, page);
                dto = dto.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                //-----------------//
                List<RepairItemViewModel> viewModels = new List<RepairItemViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetFaultyItemByQRCodeList", viewModels);
            }
        }
        public ActionResult CreateFaultyByQRCode()
        {
            ViewBag.ddlProductionFloor = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

            ViewBag.ddlProblems = _faultyCaseBusiness.GetFaultyCases(User.OrgId).Select(f => new SelectListItem
            {
                Text = f.ProblemDescription,
                Value = f.CaseId.ToString()
            }).ToList();
            return View();
        }


        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveFaultyByQRCode(RepairItemViewModel repairItem)
        {
            bool IsSuccess = false;
            if (ModelState.IsValid)
            {
                RepairItemDTO repairItemDTO = new RepairItemDTO();
                AutoMapper.Mapper.Map(repairItem, repairItemDTO);
                IsSuccess = _repairItemBusiness.SaveRepairItem(repairItemDTO, User.UserId, User.OrgId);
            }
            return Json(IsSuccess);
        }
        #endregion

        #region Repair Section Faulty Item Transfer

        public ActionResult CreateRepairSectionFaultyItemTransfer()
        {
            ViewBag.ddlRepairLine = _repairLineBusiness.GetRepairLineWithFloor(User.OrgId).Select(line => new SelectListItem { Text = line.text, Value = line.value }).ToList();

            ViewBag.ddlModelName = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).Select(des => new SelectListItem { Text = des.DescriptionName, Value = des.DescriptionId.ToString() }).ToList();

            return View();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveRepairSectionFaultyItemTransfer(RepairSectionFaultyItemTransferInfoViewModel info)
        {
            bool IsSuccess = false;
            if (ModelState.IsValid && info.RepairSectionFaultyItemRequisitionDetails.Count > 0)
            {
                RepairSectionFaultyItemTransferInfoDTO dto = new RepairSectionFaultyItemTransferInfoDTO();
                AutoMapper.Mapper.Map(info, dto);
                IsSuccess = _repairSectionFaultyItemTransferInfoBusiness.SaveRepairSectionFaultyItemTransfer(dto, User.OrgId, User.UserId);
            }
            return Json(IsSuccess);
        }

        //[HttpPost, ValidateJsonAntiForgeryToken]
        //public ActionResult SaveReceiveRepairSectionFaulty(long transferId, string status)
        //{
        //    bool IsSuccess = false;
        //    if(transferId > 0 && string.IsNullOrEmpty(status))
        //    {
        //        IsSuccess=_repairSectionFaultyItemTransferInfoBusiness.SaveReceiveRepairFaultyTransfer(transferId, status, User.UserId, User.OrgId);
        //    }
        //    return Json(IsSuccess);
        //}

        public ActionResult GetRepairSectionFaultyItemTransferList(string flag, long? floorId, long? repairLineId, string transferCode, long? transferId, string status, string fromDate, string toDate, int page = 1)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlProductionFloor = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();
                ViewBag.ddlStateStatus = Utility.ListOfReqStatus().Where(s => s.text == RequisitionStatus.Accepted || s.text == RequisitionStatus.Approved).Select(st => new SelectListItem
                {
                    Text = st.text,
                    Value = st.value
                }).ToList();
                return View();
            }
            else if (!string.IsNullOrEmpty(flag) && flag.Trim().ToLower() == Flag.View.ToLower())
            {
                var dto = _repairSectionFaultyItemTransferInfoBusiness.GetRepairSectionFaultyItemTransferInfoList(floorId, repairLineId, transferCode, status, fromDate, toDate, User.OrgId);

                // Pagination //
                //ViewBag.PagerData = GetPagerData(dto.Count(), pageSize, page);
                //dto = dto.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                //-----------------//

                List<RepairSectionFaultyItemTransferInfoViewModel> viewModels = new List<RepairSectionFaultyItemTransferInfoViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetRepairSectionFaultyItemTransferList", viewModels);
            }
            else if (!string.IsNullOrEmpty(flag) && flag.Trim().ToLower() == Flag.Detail.ToLower())
            {
                var dto = _repairSectionFaultyItemTransferDetailBusiness.GetRepairSectionFaultyItemTransferDetailsByQuery(floorId, repairLineId, status, transferId, User.OrgId);

                ViewBag.Info = new RepairSectionFaultyItemTransferInfoViewModel
                {
                    RSFIRInfoId = dto.FirstOrDefault().RSFIRInfoId,
                    ProductionFloorName = dto.FirstOrDefault().ProductionFloorName,
                    RepairLineName = dto.FirstOrDefault().RepairLineName,
                    TransferCode = dto.FirstOrDefault().TransferCode,
                    StateStatus = dto.FirstOrDefault().StateStatus,
                    EntryUser = dto.FirstOrDefault().EntryUser,
                    EntryDate = dto.FirstOrDefault().EntryDate
                };
                List<RepairSectionFaultyItemTransferDetailViewModel> viewModels = new List<RepairSectionFaultyItemTransferDetailViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetRepairSectionFaultyItemTransferDetail", viewModels);
            }
            return View();
        }

        #endregion

        #region Repair Section Requisition
        public ActionResult CreateRepairSectionRequisition(string reqFor)
        {
            ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Where(s=> !s.WarehouseName.Contains("Warehouse 3")).Select(line => new SelectListItem { Text = line.WarehouseName, Value = line.Id.ToString() }).ToList();

            ViewBag.ddlRepairLine = _repairLineBusiness.GetRepairLineWithFloor(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();

            ViewBag.ddlModelName = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).Select(des => new SelectListItem { Text = des.DescriptionName, Value = des.DescriptionId.ToString() }).ToList();

            ViewBag.ddlPackagingLine = _packagingLineBusiness.GetPackagingLinesWithProduction(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value.ToString() }).ToList();
            return View();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveRepairSectionRequisition(RepairSectionRequisitionInfoViewModel model)
        {
            bool IsSuccess = false;
            if (ModelState.IsValid && model.RepairSectionRequisitionDetails.Count > 0)
            {
                RepairSectionRequisitionInfoDTO dto = new RepairSectionRequisitionInfoDTO();
                AutoMapper.Mapper.Map(model, dto);
                IsSuccess = _repairSectionRequisitionInfoBusiness.SaveRepairSectionRequisition(dto, User.UserId, User.OrgId);
            }
            return Json(IsSuccess);
        }

        public ActionResult GetRepairSectionRequisitionInfoList(string flag, long? repairLineId, long? modelId, long? warehouseId, string status, string requisitionCode, string fromDate, string toDate, long? reqInfoId,string reqFor, int page = 1)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.WarehouseName, Value = line.Id.ToString() }).ToList();

                ViewBag.ddlRepairLine = _repairLineBusiness.GetRepairLineWithFloor(User.OrgId).Select(des => new SelectListItem { Text = des.text, Value = des.value }).ToList();

                ViewBag.ddlModelName = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).Select(des => new SelectListItem { Text = des.DescriptionName, Value = des.DescriptionId.ToString() }).ToList();

                ViewBag.ddlStateStatus = Utility.ListOfReqStatus().Select(st => new SelectListItem
                {
                    Text = st.text,
                    Value = st.value
                }).ToList();

                return View();
            }
            else if (!string.IsNullOrEmpty(flag) && (flag.Trim().ToLower() == Flag.Search.ToLower() || flag.Trim().ToLower() == Flag.View.ToLower()))
            {
                var dto = _repairSectionRequisitionInfoBusiness.GetRepairSectionRequisitionInfoList(repairLineId,null, modelId, warehouseId, status, requisitionCode, fromDate, toDate, "Repair", reqFor, User.OrgId);
                // Pagination //
                ViewBag.PagerData = GetPagerData(dto.Count(), pageSize, page);
                dto = dto.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                //-----------------//
                List<RepairSectionRequisitionInfoViewModel> viewModels = new List<RepairSectionRequisitionInfoViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetRepairSectionRequisitionInfoList", viewModels);
            }
            else
            {
                var info = _repairSectionRequisitionInfoBusiness.GetRepairSectionRequisitionById(reqInfoId.Value, User.OrgId);

                var detailDto = _repairSectionRequisitionDetailBusiness.GetRepairSectionRequisitionDetailByInfoId(reqInfoId.Value, User.OrgId).Select(d => new RepairSectionRequisitionDetailDTO
                {
                    RSRDetailId = d.RSRDetailId,
                    ItemName = d.ItemName,
                    ItemTypeName = d.ItemTypeName,
                    RequestQty = d.RequestQty,
                    IssueQty = d.IssueQty,
                    UnitName = d.UnitName
                }).ToList();

                var infoDTO = new RepairSectionRequisitionInfoDTO
                {
                    RSRInfoId = info.RSRInfoId,
                    RequisitionCode = info.RequisitionCode,
                    RepairLineName = info.RepairLineName + " [" + info.ProductionFloorName + "]",
                    ModelName = info.ModelName,
                    WarehouseName = info.WarehouseName,
                    StateStatus = info.StateStatus
                };

                IEnumerable<RepairSectionRequisitionDetailViewModel> detailViewModels = new List<RepairSectionRequisitionDetailViewModel>();
                RepairSectionRequisitionInfoViewModel infoViewModel = new RepairSectionRequisitionInfoViewModel();
                AutoMapper.Mapper.Map(detailDto, detailViewModels);
                AutoMapper.Mapper.Map(infoDTO, infoViewModel);

                ViewBag.Info = infoViewModel;

                return PartialView("_GetRepairSectionRequisitionDetailList", detailViewModels);
            }
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveRepairSectionRequisitionState(long requisitionId, string status)
        {
            bool IsSuccess = false;
            if (requisitionId > 0 && !string.IsNullOrEmpty(status))
            {
                if (status == RequisitionStatus.Accepted)
                {
                    var reqInfo = _repairSectionRequisitionInfoBusiness.GetRepairSectionRequisitionById(requisitionId, User.OrgId);
                    if(reqInfo.ReqFor == "Repair")
                    {
                        IsSuccess = _repairLineStockDetailBusiness.StockInByRepairSectionRequisition(requisitionId, status, User.UserId, User.OrgId);
                    }
                    else
                    {
                        // Packaging Section Requisition //
                        IsSuccess = _packagingRepairRawStockDetailBusiness.StockInByPackagingSectionRequisition(requisitionId, status, User.UserId, User.OrgId);
                    }
                }
            }
            return Json(IsSuccess);
        }

        #endregion

        #region QRCode

        public ActionResult GetQRCodeList(string flag)
        {
            if (string.IsNullOrEmpty(flag))
            {
                return View();
            }
            else
            {
                var dto = _qRCodeTraceBusiness.GetQRCodeTraceByOrg(User.OrgId).Select(s => new QRCodeTraceDTO
                {
                    ProductionFloorName = s.ProductionFloorName,
                    AssemblyLineName = s.AssemblyLineName,
                    ModelName = s.ModelName,
                    WarehouseName = s.WarehouseName,
                    ReferenceNumber = s.ReferenceNumber,
                    ItemTypeName = s.ItemTypeName,
                    ItemName = s.ItemName,
                    CodeNo = s.CodeNo
                }).OrderByDescending(s => s.CodeId).ToList();
                IEnumerable<QRCodeTraceViewModel> viewModels = new List<QRCodeTraceViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetQRCodeList", viewModels);
            }
        }
        #endregion

        #endregion //Region End

        #region QRCode Wise QC Item Passing

        public ActionResult GetQRCodeScanningInQC(string flag, long? floorId, long? assemblyId, long? qcLineId, long? repairLineId, string qrCode, string transferCode, string status, string date)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlProductionFloor = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();
            }
            else if (!string.IsNullOrEmpty(flag) && flag.Trim() != "" && flag == "Problems")
            {
                var dto = _faultyCaseBusiness.GetFaultyCases(User.OrgId).Select(s => new FaultyCaseDTO
                {
                    CaseId = s.CaseId,
                    ProblemDescription = s.ProblemDescription,
                    QRCode=s.QRCode
                }).ToList();

                List<FaultyCaseViewModel> viewModels = new List<FaultyCaseViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetFaultyCases", viewModels);
            }
            else if (!string.IsNullOrEmpty(flag) && flag.Trim() != "" && flag == "QRCodes")
            {
                date = string.IsNullOrEmpty(date) || date.Trim() == "" ? DateTime.Now.ToString("dd-MMM-yyyy") : date;
                var dto = _qRCodeTransferToRepairInfoBusiness.GetQRCodeTransferToRepairInfosByQuery(floorId, assemblyId, qcLineId, repairLineId, qrCode, transferCode, status, date, User.UserId, User.OrgId);
                IEnumerable<QRCodeTransferToRepairInfoViewModel> viewModels = new List<QRCodeTransferToRepairInfoViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetRepairableQRCode", viewModels);
            }
            else if (!string.IsNullOrEmpty(flag) && flag.Trim() != "" && flag == "QCPass")
            {
                date = string.IsNullOrEmpty(date) || date.Trim() == "" ? DateTime.Now.ToString("dd-MMM-yyyy") : date;
                var dto = _qCPassTransferDetailBusiness.GetQCPassTransferDetailsByQuery(floorId, assemblyId, qcLineId, qrCode, transferCode, status, date, null, User.UserId, User.OrgId);
                IEnumerable<QCPassTransferDetailViewModel> viewModels = new List<QCPassTransferDetailViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetQCPassQrCodes", viewModels);
            }
            return View();
        }

        public ActionResult CreateQRCodeWiseQcItemTransfer()
        {
            ViewBag.ddlProductionFloor = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();
            return View();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public async Task<ActionResult> SaveQRCodeWiseQcItemTransfer(QRCodeTransferToRepairInfoViewModel model)
        {
            bool IsSuccess = false;
            var IsExist = _tempQRCodeTraceBusiness.IsExistQRCodeWithStatus(model.QRCode, QRCodeStatus.LotIn, User.OrgId);
            //_qRCodeTransferToRepairInfoBusiness.IsQRCodeExistInTransferWithStatus(model.QRCode, string.Format(@"'Receiced','Send'"), User.OrgId);
            var msg = (IsExist == false ? "This QRCode already has been transfered To Repair/MiniStock" : "");
            if (IsExist && ModelState.IsValid)
            {
                if (model.QRCodeProblems != null && model.QRCodeProblems.Count > 0)
                {
                    QRCodeTransferToRepairInfoDTO dto = new QRCodeTransferToRepairInfoDTO();
                    AutoMapper.Mapper.Map(model, dto);
                    IsSuccess = await _qRCodeTransferToRepairInfoBusiness.SaveQRCodeTransferToRepairAsync(dto, User.UserId, User.OrgId);
                }
                else
                {
                    QCPassTransferInformationDTO dto = new QCPassTransferInformationDTO()
                    {
                        ProductionFloorId = model.FloorId,
                        ProductionFloorName = model.FloorName,
                        AssemblyLineId = model.AssemblyLineId,
                        AssemblyLineName = model.AssemblyLineName,
                        QCLineId = model.QCLineId,
                        QCLineName = model.QCLineName,
                        DescriptionId = model.DescriptionId,
                        WarehouseId = model.WarehouseId.Value,
                        ItemTypeId = model.ItemTypeId.Value,
                        ItemId = model.ItemId.Value,
                        Quantity = 1

                    };
                    IsSuccess = await _qCPassTransferInformationBusiness.SaveQCPassTransferToMiniStockByQRCodeAsync(dto, model.QRCode, User.UserId, User.OrgId);
                }
            }
            return Json(new { IsSuccess = IsSuccess, Msg = msg });
        }

        #endregion

        #region Repair an item By Scaning QRCode
        public ActionResult CreateRepairAnItemByQRCodeScaning()
        {
            return View();
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveFaultyItemByQRCodeScaning(FaultyInfoByQRCodeViewModel model)
        {
            bool IsSuccess = false;
            if (ModelState.IsValid && model.FaultyItems.Count > 0)
            {
                FaultyInfoByQRCodeDTO dto = new FaultyInfoByQRCodeDTO();
                AutoMapper.Mapper.Map(model, dto);
                IsSuccess = _qRCodeTransferToRepairInfoBusiness.StockOutByAddingFaultyWithQRCode(dto, User.UserId, User.OrgId);
            }
            return Json(IsSuccess);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public async Task<ActionResult> SaveRepairItemByQRCodeScaning(TransferRepairItemByQRCodeScanningViewModel model)
        {
            bool IsSuccess = false;
            var stockAvailable = _qRCodeTransferToRepairInfoBusiness.CheckingAvailabilityOfSparepartsWithRepairLineStock(model.ModelId, model.ItemId, model.RepairLineId, User.OrgId);
            if (ModelState.IsValid && stockAvailable.isSuccess)
            {
                TransferRepairItemByQRCodeScanningDTO dto = new TransferRepairItemByQRCodeScanningDTO();
                AutoMapper.Mapper.Map(model, dto);
                IsSuccess = await _transferRepairItemToQcInfoBusiness.SaveTransferByQRCodeScanningAsync(dto, User.UserId, User.OrgId);
            }
            return Json(new { IsSuccess = IsSuccess, Msg = stockAvailable.text });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult GetRepairItemDetailsByQRCode(string qrCode)
        {
            var QrCodeItemInfo = _qRCodeTransferToRepairInfoBusiness.GetQRCodeWiseItemInfo(qrCode, string.Format(@"'Received'"), User.OrgId);

            if (QrCodeItemInfo != null && QrCodeItemInfo.StateStatus == "Received")
            {
                //QrCodeItemInfo.TransferId
                var QrCodeItemProblems = _qRCodeProblemBusiness.GetQRCodeProblemDTOByQuery(0, qrCode, User.OrgId);

                List<Dropdown> dropdowns = new List<Dropdown>();
                dropdowns = _itemBusiness.GetItemPreparationItems(QrCodeItemInfo.DescriptionId, QrCodeItemInfo.ItemId.Value, ItemPreparationType.Production, User.OrgId).Select(i => new Dropdown { text = i.ItemName, value = i.ItemId }).ToList();

                // QRCode Details
                QRCodeTransferToRepairInfoViewModel qrCodeItemInfoViewModel = new QRCodeTransferToRepairInfoViewModel();
                AutoMapper.Mapper.Map(QrCodeItemInfo, qrCodeItemInfoViewModel);

                // QRCode Problems
                IEnumerable<QRCodeProblemViewModel> qRCodeProblemViewModel = new List<QRCodeProblemViewModel>();
                AutoMapper.Mapper.Map(QrCodeItemProblems, qRCodeProblemViewModel);

                // QRCode Faulty Items
                IEnumerable<FaultyItemStockDetailViewModel> faultyItemsViewModel = new List<FaultyItemStockDetailViewModel>();
                var faultyItemsDto = _faultyItemStockDetailBusiness.GetFaultyItemStockDetailsByQrCode(QrCodeItemInfo.QRCode, QrCodeItemInfo.TransferId, User.OrgId);
                AutoMapper.Mapper.Map(faultyItemsDto, faultyItemsViewModel);

                return Json(new { info = qrCodeItemInfoViewModel, problems = qRCodeProblemViewModel, faultyItems = faultyItemsViewModel, items = dropdowns });
            }
            return Json(new { info = new { } });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public  ActionResult SaveVoidAFaultyItem(long transferId, string qrCode, long itemId)
        {
            bool IsSuccess = false;
            if(transferId > 0 && !string.IsNullOrEmpty(qrCode) &&  itemId > 0)
            {
                IsSuccess= _repairLineStockDetailBusiness.SaveVoidAFaultyItem(transferId, qrCode, itemId, User.UserId, User.OrgId);
            }
            return Json(IsSuccess);
        }
        #endregion

        public ActionResult GetQRCode()
        {
            byte[] qrCode = Utility.GenerateQRCode("Yeasin Ahmed");
            var base64 = Convert.ToBase64String(qrCode);
            var fs = String.Format("data:application/png;base64,{0}", base64);
            ViewBag.Base64 = fs;
            return View();
        }

        // FiveStar - 
        #region Production - Requisition - New [21-July-2020] // FiveStar
        public ActionResult GetRequisitionByItemInfoAndDetail(string flag, long? floorId, long? assemblyId, long? packagingId, long? repairLineId, long? warehouseId, long? modelId, string reqCode, string reqType, string reqFor, string fromDate, string toDate, string status, long? reqInfoId, string reqFlag)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlProductionFloor = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(s => new SelectListItem
                {
                    Text = s.LineNumber,
                    Value = s.LineId.ToString()
                }).ToList();

                ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(ware => new SelectListItem { Text = ware.WarehouseName, Value = ware.Id.ToString() }).ToList();

                var allItems = _itemBusiness.GetItemDetails(User.OrgId);

                ViewBag.ddlItems = allItems.Where(s => s.ItemName.Contains("Warehouse 3")).Select(s => new SelectListItem { Text = s.ItemName, Value = s.ItemId.ToString() }).ToList();

                ViewBag.ddlItemsForSingleReq = allItems.Where(s => !s.ItemName.Contains("Warehouse 3")).Select(s => new SelectListItem { Text = s.ItemName, Value = s.ItemId.ToString() }).ToList();

                ViewBag.ddlAssemblyLineWithProduction = _assemblyLineBusiness.GetAssemblyLinesWithProduction(User.OrgId).Select(s => new SelectListItem { Text = s.text, Value = s.value.ToString() }).ToList();

                ViewBag.ddlPackagingLineWithProduction = _packagingLineBusiness.GetPackagingLinesWithProduction(User.OrgId).Select(s => new SelectListItem { Text = s.text, Value = s.value.ToString() }).ToList();

                ViewBag.ddlStateStatus = Utility.ListOfReqStatus().Where(s => s.value != RequisitionStatus.Pending).Select(st => new SelectListItem
                {
                    Text = st.text,
                    Value = st.value
                }).ToList();

                ViewBag.ddlModelName = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).Select(des => new SelectListItem { Text = des.DescriptionName, Value = des.DescriptionId.ToString() }).ToList();

                ViewBag.ddlRequisitionType = Utility.ListOfRequisitionType().Select(r => new SelectListItem { Text = r.text, Value = r.value }).ToList();

                return View();
            }
            else if (!string.IsNullOrEmpty(flag) && (flag.ToLower() == Flag.View.ToLower() || flag.ToLower() == Flag.Search.ToLower()))
            {
                status = (!string.IsNullOrEmpty(status) && status.Trim() != "") ? string.Format(@"'{0}'", status) : null;
                var dto = _requsitionInfoBusiness.GetRequsitionInfosByQuery(floorId, assemblyId, packagingId, repairLineId, warehouseId, modelId, reqCode, reqType, reqFor, fromDate, toDate, status, reqFlag, reqInfoId, User.OrgId);

                IEnumerable<RequsitionInfoViewModel> viewModels = new List<RequsitionInfoViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetRequisitionByItemInfo", viewModels);
            }
            else if (!string.IsNullOrEmpty(flag) && (flag.ToLower() == Flag.Detail.ToLower() || flag.ToLower() == Flag.Detail.ToLower()))
            {
                var dto = _requisitionItemInfoBusiness.GetRequsitionInfoModalProcessData(null, null, null, null, null, null, null, null, null, null, null, null, null, reqInfoId, User.OrgId);
                RequsitionInfoViewModel viewModel = new RequsitionInfoViewModel();
                ViewBag.Level = "Production";
                AutoMapper.Mapper.Map(dto, viewModel);
                return PartialView("_GetRequisitionItemDetail", viewModel);
            }
            return View();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveRequisitionWithItemInfoAndDetail(RequsitionInfoViewModel model)
        {
            bool IsSuccess = false;
            if (ModelState.IsValid && model.RequisitionDetails.Count > 0 && model.RequisitionItemInfos.Count > 0)
            {
                RequsitionInfoDTO dto = new RequsitionInfoDTO();
                AutoMapper.Mapper.Map(model, dto);
                IsSuccess = _requsitionInfoBusiness.SaveRequisitionWithItemInfoAndDetail(dto, User.UserId, User.OrgId);
            }
            return Json(IsSuccess);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveRequisitionItemsByAssemblyOrRepairOrPackaging(long reqInfoId, string status)
        {
            bool IsSuccess = false;
            if (reqInfoId > 0 && !string.IsNullOrEmpty(status) && !string.IsNullOrWhiteSpace(status))
            {
                IsSuccess = _requisitionItemInfoBusiness.SaveRequisitionItemStocksInAssemblyOrRepairOrPackaging(reqInfoId, status, User.UserId, User.OrgId);
            }
            return Json(IsSuccess);
        }
        #endregion

        #region Assembly-QC
        public ActionResult GetAssemblyLineStockInfoFS(string flag, long? lineId, long? assemblyId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string lessOrEq, long? reqInfoId,long? returnId,string returnCode,string status,string fromDate,string toDate, int page = 1)
        {
            //ViewBag.UserPrivilege = UserPrivilege("Production", "GetAssemblyLineStockInfo");
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlAssemblyLineWithProduction = _assemblyLineBusiness.GetAssemblyLinesWithProduction(User.OrgId).Select(s => new SelectListItem { Text = s.text, Value = s.value.ToString() }).ToList();

                ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

                ViewBag.ddlModelName = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).Select(des => new SelectListItem { Text = des.DescriptionName, Value = des.DescriptionId.ToString() }).ToList();

                ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Select(ware => new SelectListItem
                {
                    Text = ware.WarehouseName,
                    Value = ware.Id.ToString()
                }).ToList();

                ViewBag.ddlItems = _itemBusiness.GetItemDetails(User.OrgId).Select(s => new SelectListItem { Text = s.ItemName, Value = s.ItemId }).ToList();

                ViewBag.ddlStateStatus = Utility.ListOfReqStatus().Where(s => s.value == RequisitionStatus.Approved || s.value == RequisitionStatus.Accepted).Select(st => new SelectListItem
                {
                    Text = st.text,
                    Value = st.value
                }).ToList();

                return View();
            }
            else if (!string.IsNullOrEmpty(flag) && (flag.Trim() == Flag.View.ToLower() || flag.Trim() == Flag.Search.ToLower()))
            {
                IEnumerable<AssemblyLineStockInfoDTO> dto = _assemblyLineStockInfoBusiness.GetAssemblyLineStockInfosByQuery(lineId, assemblyId, modelId, warehouseId, itemTypeId, itemId, lessOrEq, User.OrgId);
                //// Pagination //
                //ViewBag.PagerData = GetPagerData(dto.Count(), pageSize, page);
                //dto = dto.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                ////-----------------//
                List<AssemblyLineStockInfoViewModel> viewModels = new List<AssemblyLineStockInfoViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetAssemblyLineStockInfoFS", viewModels);
            }
            else if (!string.IsNullOrEmpty(flag) && flag.Trim() !="" && flag == "StockReturnList")
            {
               var dto = _stockItemReturnInfoBusiness.GetStockItemReturnInfosByQuery(modelId, lineId, assemblyId, null, null, warehouseId, returnId, returnCode, StockRetunFlag.AssemblyLine, status, fromDate, toDate, User.OrgId);

                List<StockItemReturnInfoViewModel> viewModels = new List<StockItemReturnInfoViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);

                return PartialView("_GetStockReturnList",viewModels);
            }
            return View();
        }

        // Child Action Of GetAssemblyLineStockInfoFS
        public ActionResult ReceiveSparePartsFromProductionRequisitionInAssembly(string flag, long? floorId, long? assemblyId, long? warehouseId, long? modelId, string reqCode, string reqType, string reqFor, string fromDate, string toDate, string status, long? reqInfoId, string reqFlag)
        {
            status = ((status == null || status.Trim() == "") || (!status.Contains("Approved") && !status.Contains("Accepted"))) ? string.Format(@"'Approved','Accepted'") : string.Format(@"'{0}'", status);
            reqFor = reqFor == null ? "Assembly" : "Assembly";
            if (!string.IsNullOrEmpty(flag) && (flag.Trim().ToLower() == Flag.Info.ToLower()))
            {
                var dto = _requsitionInfoBusiness.GetRequsitionInfosByQuery(floorId, assemblyId, 0, 0, warehouseId, modelId, reqCode, reqType, reqFor, fromDate, toDate, status, reqFlag, reqInfoId, User.OrgId);

                IEnumerable<RequsitionInfoViewModel> viewModels = new List<RequsitionInfoViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetRequisitionByItemInfo", viewModels);
            }
            else if (!string.IsNullOrEmpty(flag) && (flag.Trim().ToLower() == Flag.Detail.ToLower()))
            {
                var dto = _requisitionItemInfoBusiness.GetRequsitionInfoModalProcessData(floorId, assemblyId, null, null, null, null, null, null, null, null, null, null, null, reqInfoId, User.OrgId);
                RequsitionInfoViewModel viewModel = new RequsitionInfoViewModel();
                ViewBag.Level = "Assembly";
                AutoMapper.Mapper.Map(dto, viewModel);
                return PartialView("_GetRequisitionItemDetail", viewModel);
            }
            return new EmptyResult();
        }

        public ActionResult CreateAssemblyStockReturn()
        {
            ViewBag.ddlAssemblyLineWithProduction = _assemblyLineBusiness.GetAssemblyLinesWithProduction(User.OrgId).Select(s => new SelectListItem
            {
                Text = s.text,
                Value = s.value
            }).ToList();
            ViewBag.ddlModelName = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).Select(s => new SelectListItem
            {
                Text = s.DescriptionName,
                Value = s.DescriptionId.ToString()
            }).ToList();
            return View();
        }
        [HttpPost,ValidateJsonAntiForgeryToken]
        public ActionResult GetAssemblyStockItemsForReturn(long assemblyId, long floorId, long modelId)
        {
            var data = _assemblyLineStockInfoBusiness.GetAssemblyLineStocksForReturnStock(assemblyId, floorId, modelId, User.OrgId);
            IEnumerable<AssemblyLineStockInfoViewModel> viewModels = new List<AssemblyLineStockInfoViewModel>();
            AutoMapper.Mapper.Map(data, viewModels);
            return PartialView(viewModels);
        }

        [HttpPost,ValidateJsonAntiForgeryToken]
        public ActionResult SaveStockReturnItems(List<StockItemReturnDetailViewModel> model)
        {
            bool IsSuccess = false;
            if (ModelState.IsValid && model.Count > 0)
            {
                List<StockItemReturnDetailDTO> dto = new List<StockItemReturnDetailDTO>();
                AutoMapper.Mapper.Map(model, dto);
                IsSuccess =_stockItemReturnInfoBusiness.SaveStockItemReturn(dto, User.UserId, User.OrgId);
            }
            return Json(IsSuccess);
        }
        #endregion

        #region IMEI Write/Update against *QRCODE* In Packaging Section
        public ActionResult IMEIWriteOrUpdate()
        {
            ViewBag.ddlPackagingLine = _packagingLineBusiness.GetPackagingLinesWithProduction(User.OrgId).Select(s => new SelectListItem
            {
                Text = s.text,
                Value = s.value
            }).ToList();
            return View();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public async Task<ActionResult> SaveQRCodeIEMIAsync(IMEIWriteViewModel model)
        {
            var IsSuccess = false;
            if (ModelState.IsValid)
            {
                IMEIWriteDTO dto = new IMEIWriteDTO();
                AutoMapper.Mapper.Map(model, dto);
                IsSuccess = await _tempQRCodeTraceBusiness.SaveQRCodeIEMIAsync(dto, User.UserId, User.OrgId);
            }
            return Json(IsSuccess);
        }

        [HttpPost,ValidateJsonAntiForgeryToken]
        public ActionResult GenerateIMEI(string qrCode, int noOfSim)
        {
            List<string> IMEIs = new List<string>();
            if(!string.IsNullOrEmpty(qrCode) && qrCode.Trim() !="" && noOfSim > 0)
            {
                IMEIs= _iMEIGenerator.IMEIGenerateByQRCode(qrCode, noOfSim, User.UserId, User.OrgId);
            }
            return Json(IMEIs);
        }

        public ActionResult GetGeneratedIMEIList()
        {
            var dto = _generatedIMEIBusiness.GetGeneratedIMEIDTOs(User.UserId,User.OrgId);
            List<GeneratedIMEIViewModel> generatedIMEIViews = new List<GeneratedIMEIViewModel>();
            AutoMapper.Mapper.Map(dto, generatedIMEIViews);
            return PartialView("_GetGeneratedIMEIList", generatedIMEIViews);
        }

        [HttpPost]
        public async Task<ActionResult> GetTempQRCodeTraceByCodeWithFloorAsync(string code, long? floorId)
        {
            var status = string.Format(@"'MiniStock','PackagingLine'");
            var data = await _tempQRCodeTraceBusiness.GetTempQRCodeTraceByCodeWithFloorAsync(Utility.ParamChecker(code), status, floorId, User.OrgId);
            if(data == null)
            {
                data = new TempQRCodeTrace();
            }
            return Json(data);
        }

        
        #endregion

        #region Battery Code //
        public ActionResult BatteryWrite()
        {
            ViewBag.ddlPackagingLine = _packagingLineBusiness.GetPackagingLinesWithProduction(User.OrgId).Select(s => new SelectListItem
            {
                Text = s.text,
                Value = s.value
            }).ToList();
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> GetIMEIinQRCode(string imei, long? floorId, long? packagingId)
        {
            var status = string.Format(@"'PackagingLine'");
            var data = await _tempQRCodeTraceBusiness.GetIMEIinQRCode(Utility.ParamChecker(imei), status, floorId.Value, packagingId.Value, User.OrgId);
            return Json(data);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public async Task<ActionResult> SaveBatteryCodeAsync(BatteryWriteViewModel model)
        {
            var IsSuccess = false;
            if (ModelState.IsValid)
            {
                BatteryWriteDTO dto = new BatteryWriteDTO();
                AutoMapper.Mapper.Map(model, dto);
                IsSuccess = await _tempQRCodeTraceBusiness.SaveBatteryCodeAsync(dto, User.UserId, User.OrgId);
            }
            return Json(IsSuccess);
        }
        #endregion

        #region Packaging QC By IMEI
        public ActionResult GetIMEIScanningInQC(string flag, long? floorId, long? assemblyId, long? qcLineId, long? repairLineId, string qrCode, string transferCode, string status, string date)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlProductionFloor = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();
            }
            else if (!string.IsNullOrEmpty(flag) && flag.Trim() != "" && flag == "Problems")
            {
                var dto = _faultyCaseBusiness.GetFaultyCases(User.OrgId).Select(s => new FaultyCaseDTO
                {
                    CaseId = s.CaseId,
                    ProblemDescription = s.ProblemDescription
                }).ToList();

                List<FaultyCaseViewModel> viewModels = new List<FaultyCaseViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetFaultyCases", viewModels);
            }
            else if (!string.IsNullOrEmpty(flag) && flag.Trim() != "" && flag == "QRCodes")
            {
                date = string.IsNullOrEmpty(date) || date.Trim() == "" ? DateTime.Now.ToString("dd-MMM-yyyy") : date;
                var dto = _qRCodeTransferToRepairInfoBusiness.GetQRCodeTransferToRepairInfosByQuery(floorId, assemblyId, qcLineId, repairLineId, qrCode, transferCode, status, date, User.UserId, User.OrgId);
                IEnumerable<QRCodeTransferToRepairInfoViewModel> viewModels = new List<QRCodeTransferToRepairInfoViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetRepairableQRCode", viewModels);
            }
            else if (!string.IsNullOrEmpty(flag) && flag.Trim() != "" && flag == "QCPass")
            {
                date = string.IsNullOrEmpty(date) || date.Trim() == "" ? DateTime.Now.ToString("dd-MMM-yyyy") : date;
                var dto = _qCPassTransferDetailBusiness.GetQCPassTransferDetailsByQuery(floorId, assemblyId, qcLineId, qrCode, transferCode, status, date, null, User.UserId, User.OrgId);
                IEnumerable<QCPassTransferDetailViewModel> viewModels = new List<QCPassTransferDetailViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetQCPassQrCodes", viewModels);
            }
            return View();
        }

        #endregion

        #region Packaging QC Process
        public ActionResult GetPackagingLineQC(string flag)
        {
            if (string.IsNullOrEmpty(flag))
            {
                return View();
            }
            else if (!string.IsNullOrEmpty(flag) && flag.Trim() != "" && flag == "Problems")
            {
                var dto = _faultyCaseBusiness.GetFaultyCases(User.OrgId).Select(s => new FaultyCaseDTO
                {
                    CaseId = s.CaseId,
                    ProblemDescription = s.ProblemDescription
                }).ToList();

                List<FaultyCaseViewModel> viewModels = new List<FaultyCaseViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetFaultyCases", viewModels);
            }
            return View();
        }

        public ActionResult GetPackagingQCProcess(string flag, long? floorId, long? packagingLineId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string status, string fromDate, string toDate, string transferCode, long? transferId, string qrCode, string imei)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlPackagingLine = _packagingLineBusiness.GetPackagingLinesWithProduction(User.OrgId).Select(s => new SelectListItem
                {
                    Text = s.text,
                    Value = s.value
                }).ToList();

                ViewBag.ddlModels = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).Select(s => new SelectListItem
                {
                    Text = s.DescriptionName,
                    Value = s.DescriptionId.ToString()
                }).ToList();

                var allItemInDb = _itemBusiness.GetItemDetails(User.OrgId).ToList();

                ViewBag.ddlPartsItems = allItemInDb.Where(s => !s.ItemName.Contains("Warehouse 3")).Select(s => new SelectListItem
                {
                    Text = s.ItemName,
                    Value = s.ItemId
                }).ToList();

                ViewBag.ddlHandsetItems = allItemInDb.Where(s => s.ItemName.Contains("Warehouse 3")).Select(s => new SelectListItem
                {
                    Text = s.ItemName,
                    Value = s.ItemId
                }).ToList();

                ViewBag.ddlStateStatus = Utility.ListOfReqStatus().Where(s => s.text == "Approved" || s.text == "Accepted").Select(s => new SelectListItem
                {
                    Text = s.text,
                    Value = s.value
                }).ToList();

                return View();
            }
            else if (!string.IsNullOrEmpty(flag) && flag.Trim() == "ReceiveRepairableItems")
            {
                var dtoInfo = _transferToPackagingRepairInfoBusiness.GetTransferToPackagingRepairInfosByQuery(floorId, packagingLineId, modelId, warehouseId, itemTypeId, itemId, status, fromDate, toDate, transferCode, transferId, User.OrgId);

                List<TransferToPackagingRepairInfoViewModel> viewModels = new List<TransferToPackagingRepairInfoViewModel>();
                AutoMapper.Mapper.Map(dtoInfo, viewModels);
                return PartialView("_GetPackagingRepairItemReceiveInfo", viewModels);
            }
            else if (!string.IsNullOrEmpty(flag) && flag.Trim() == "ReceiveRepairableItemDetail")
            {
                var dtoInfo = _transferToPackagingRepairInfoBusiness.GetTransferToPackagingRepairInfosByQuery(null, null, null, null, null, null, null, null, null, null, transferId, User.OrgId).FirstOrDefault();
                if (dtoInfo != null)
                {
                    dtoInfo.TransferToPackagingRepairDetails = _transferToPackagingRepairDetailBusiness.GetTransferToPackagingRepairDetailsByQuery(transferId, null, null, modelId, null, null, null, null, User.OrgId).ToList();

                    dtoInfo.IMEITransferToRepairInfos = _iMEITransferToRepairInfoBusiness.GetIMEITransferToRepairInfosByQuery(transferId, null, null, null, null, null, null, null, null, null, null, User.OrgId).ToList();
                }
                TransferToPackagingRepairInfoViewModel viewModel = new TransferToPackagingRepairInfoViewModel();
                AutoMapper.Mapper.Map(dtoInfo, viewModel);
                return PartialView("_GetPackagingRepairItemReceiveDetail", viewModel);
            }
            else if (!string.IsNullOrEmpty(flag) && flag.Trim() == "TransferFromRepairList")
            {
                var dto = _transferPackagingRepairItemToQcInfoBusiness.GetTransferPackagingRepairItemToQcInfosByQuery(floorId, packagingLineId, modelId, warehouseId, itemTypeId, itemId, status, transferCode, fromDate, toDate, User.OrgId);
                List<TransferPackagingRepairItemToQcInfoViewModel> viewModels = new List<TransferPackagingRepairItemToQcInfoViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetPackagingRepairItemTransferToQCList", viewModels);
            }
            else if (!string.IsNullOrEmpty(flag) && flag.Trim() == "TransferFromRepairDetail")
            {
                var dto = _transferPackagingRepairItemToQcDetailBusiness.GetTransferPackagingRepairItemToQcDetailByQuery(null, null, transferId, User.OrgId);
                ViewBag.StateStatus = dto.FirstOrDefault().StateStatus;
                List<TransferPackagingRepairItemToQcDetailViewModel> viewModels = new List<TransferPackagingRepairItemToQcDetailViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetPackagingRepairItemTransferToQCDetail", viewModels);
            }
            return View();
        }

        // Finish Goods  By IMEI Scaning //
        [HttpPost, ValidateJsonAntiForgeryToken]
        public async Task<ActionResult> SaveIMEITransferByPackagingQC(string imei, List<QRCodeProblemViewModel> problems)
        {
            ExecutionStateWithText executionState = new ExecutionStateWithText();
            executionState.text = "Something went wrong";
            imei = Utility.ParamChecker(imei);
            var IsExist = _tempQRCodeTraceBusiness.IsExistIMEIWithStatus(imei, QRCodeStatus.Packaging, User.OrgId);
            executionState.text = (IsExist == false ? "This IMEI already has been transfered To Repair/Finish Goods" : "");
            //imei.Trim().Length == 15
            if (!string.IsNullOrEmpty(imei) && IsExist && problems.Count > 0)
            {
                // QC Fail
                List<QRCodeProblemDTO> problemDTO = new List<QRCodeProblemDTO>();
                AutoMapper.Mapper.Map(problems, problemDTO);
                executionState.isSuccess = await _iMEITransferToRepairInfoBusiness.SaveIMEITransferToRepairInfoAsync(imei, problemDTO, User.UserId, User.OrgId);
            }
            return Json(executionState);
        }

       
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SavePackagingQCTransferItemInRepair(long transferId, string status)
        {
            bool IsSuccess = false;
            if (transferId > 0 && !string.IsNullOrEmpty(status) && status.Trim() != "" && status.Trim() == RequisitionStatus.Accepted)
            {
                IsSuccess = _iMEITransferToRepairInfoBusiness.SaveIMEIStatusByTransferInfoId(transferId, status, User.UserId, User.OrgId);
            }
            return Json(IsSuccess);
        }


        //SaveFinishGoodsByIMEI
        [HttpPost, ValidateJsonAntiForgeryToken]
        public async Task<ActionResult> SaveFinishGoodsByIMEI(string imei)
        {
            ExecutionStateWithText executionState = new ExecutionStateWithText();
            executionState.text = "Something went wrong";
            imei = Utility.ParamChecker(imei);
            var IsExist = _tempQRCodeTraceBusiness.IsExistIMEIWithStatus(imei, QRCodeStatus.Packaging, User.OrgId);
            executionState.text = (IsExist == false ? "This IMEI already has been transfered To Repair/Finish Goods" : "");
            if (!string.IsNullOrEmpty(imei) && imei.Trim() != "" && IsExist)
            {
                executionState.isSuccess= await _finishGoodsStockDetailBusiness.SaveFinishGoodsByIMEIAsync(imei, User.UserId, User.OrgId);
            }
            return Json(executionState);
        }

        #endregion

        #region Packaging Repair IMEI Scanning
        public ActionResult CreatePackagingRepairIMEIScanning()
        {
            return View();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult GetPackagingRepairItemDetailsByIMEI(string imei)
        {
            var imeiItemInfo = _iMEITransferToRepairInfoBusiness.GetIMEIWiseItemInfo(imei, string.Empty, string.Format(@"'Received'"), User.OrgId);

            if (imeiItemInfo != null && imeiItemInfo.StateStatus == "Received")
            {
                //TransferId
                var imeiItemInfoItemProblems = _iMEITransferToRepairDetailBusiness.GetIMEIProblemDTOByQuery(imeiItemInfo.IMEITRInfoId, imeiItemInfo.QRCode, string.Empty, User.OrgId);

                List<Dropdown> dropdowns = new List<Dropdown>();
                dropdowns = _itemBusiness.GetItemPreparationItems(imeiItemInfo.DescriptionId, imeiItemInfo.ItemId.Value, ItemPreparationType.Packaging, User.OrgId).Select(i => new Dropdown { text = i.ItemName, value = i.ItemId }).ToList();

                // IMEI Details
                IMEITransferToRepairInfoViewModel imeiItemInfoViewModel = new IMEITransferToRepairInfoViewModel();
                AutoMapper.Mapper.Map(imeiItemInfo, imeiItemInfoViewModel);

                // IMEI Problems
                IEnumerable<IMEITransferToRepairDetailViewModel> imeiProblemViewModel = new List<IMEITransferToRepairDetailViewModel>();
                AutoMapper.Mapper.Map(imeiItemInfoItemProblems, imeiProblemViewModel);

                // IMEI Faulty Items
                IEnumerable<PackagingFaultyStockDetailViewModel> faultyItemsViewModel = new List<PackagingFaultyStockDetailViewModel>();
                var faultyItemsDto = _packagingFaultyStockDetailBusiness.GetPackagingFaultyItemStockDetailsByQrCode(imeiItemInfo.QRCode, string.Empty,imeiItemInfo.TransferId, User.OrgId);
                AutoMapper.Mapper.Map(faultyItemsDto, faultyItemsViewModel);

                return Json(new { info = imeiItemInfoViewModel, problems = imeiProblemViewModel, faultyItems = faultyItemsViewModel, items = dropdowns });
            }
            return Json(new { info = new { } });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public async Task<ActionResult> SavePackgingRepairItemTransferByIMEIScaning(TransferPackagigRepairItemByIMEIScanningViewModel model)
        {
            bool IsSuccess = false;
            var stockAvailable = _iMEITransferToRepairInfoBusiness.CheckingAvailabilityOfPackagingRepairRawStock(model.ModelId, model.ItemId, model.PackagingLineId, User.OrgId);
            if (ModelState.IsValid && stockAvailable.isSuccess)
            {
                TransferPackagigRepairItemByIMEIScanningDTO dto = new TransferPackagigRepairItemByIMEIScanningDTO();
                AutoMapper.Mapper.Map(model, dto);
                IsSuccess = await _transferPackagingRepairItemToQcInfoBusiness.SaveTransferByIMEIScanningAsync(dto, User.UserId, User.OrgId);
            }
            return Json(new { IsSuccess = IsSuccess, Msg = stockAvailable.text });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public async Task<ActionResult> SavePackagingRapairToQCTransferInfoStateStatus(long transferId, string status)
        {
            bool IsSuccess = false;
            if (transferId > 0 && status.Trim() != "" && status == RequisitionStatus.Accepted)
            {
                IsSuccess = await _transferPackagingRepairItemToQcInfoBusiness.SavePackagingRapairToQCTransferInfoStateStatusAsync(transferId, status, User.UserId, User.OrgId);
            }
            return Json(IsSuccess);
        }

        // Packaging Repair Faulty Adding //
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SavePackagingRepairAddingFaultyWithQRCode(FaultyInfoByQRCodeViewModel model)
        {
            bool IsSuccess = false;
            if(ModelState.IsValid && model.FaultyItems.Count > 0)
            {
                FaultyInfoByQRCodeDTO dto = new FaultyInfoByQRCodeDTO();
                AutoMapper.Mapper.Map(model, dto);
                IsSuccess= _iMEITransferToRepairInfoBusiness.PackagingRepairAddingFaultyWithQRCode(dto, User.UserId, User.OrgId);
            }
            return Json(IsSuccess);
        }
        #endregion

        #region Pakaging Repair Process
        // Pakaging Repair Process //
        public ActionResult GetPackagingRepairProcess(string flag, long? floorId, long? packagingLineId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string status, string fromDate, string toDate, string transferCode, long? transferId,long? reqInfoId, string qrCode, string imei, string lessOrEq,string requisitionCode,long? returnId, string returnCode, int page=1)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlPackagingLine = _packagingLineBusiness.GetPackagingLinesWithProduction(User.OrgId).Select(s => new SelectListItem
                {
                    Text = s.text,
                    Value = s.value
                }).ToList();

                ViewBag.ddlModels = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).Select(s => new SelectListItem
                {
                    Text = s.DescriptionName,
                    Value = s.DescriptionId.ToString()
                }).ToList();

                var allItemInDb = _itemBusiness.GetItemDetails(User.OrgId).ToList();

                ViewBag.ddlPartsItems = allItemInDb.Where(s => !s.ItemName.Contains("Warehouse 3")).Select(s => new SelectListItem
                {
                    Text = s.ItemName,
                    Value = s.ItemId
                }).ToList();

                ViewBag.ddlHandsetItems = allItemInDb.Where(s => s.ItemName.Contains("Warehouse 3")).Select(s => new SelectListItem
                {
                    Text = s.ItemName,
                    Value = s.ItemId
                }).ToList();

                ViewBag.ddlStateStatus = Utility.ListOfReqStatus().Where(s => s.text == "Approved" || s.text == "Accepted").Select(s => new SelectListItem
                {
                    Text = s.text,
                    Value = s.value
                }).ToList();

                ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId).Where(s=> !s.WarehouseName.Contains("Warehouse 3")).Select(s => new SelectListItem
                {
                    Text = s.WarehouseName,
                    Value = s.Id.ToString()
                }).ToList();

                ViewBag.ddlStateStatus2 = Utility.ListOfReqStatus().Select(st => new SelectListItem
                {
                    Text = st.text,
                    Value = st.value
                }).ToList();

                return View();
            }
            else if (!string.IsNullOrEmpty(flag) && flag.Trim() == "ReceiveRepairableItems")
            {
                var dtoInfo = _transferToPackagingRepairInfoBusiness.GetTransferToPackagingRepairInfosByQuery(floorId, packagingLineId, modelId, warehouseId, itemTypeId, itemId, status, fromDate, toDate, transferCode, transferId, User.OrgId);

                List<TransferToPackagingRepairInfoViewModel> viewModels = new List<TransferToPackagingRepairInfoViewModel>();
                AutoMapper.Mapper.Map(dtoInfo, viewModels);
                return PartialView("_GetPackagingRepairItemReceiveInfo", viewModels);
            }
            else if (!string.IsNullOrEmpty(flag) && flag.Trim() == "ReceiveRepairableItemDetail")
            {
                var dtoInfo = _transferToPackagingRepairInfoBusiness.GetTransferToPackagingRepairInfosByQuery(null, null, null, null, null, null, null, null, null, null, transferId, User.OrgId).FirstOrDefault();

                if (dtoInfo != null)
                {
                    dtoInfo.TransferToPackagingRepairDetails = _transferToPackagingRepairDetailBusiness.GetTransferToPackagingRepairDetailsByQuery(transferId, null, null, modelId, null, null, null, null, User.OrgId).ToList();

                    dtoInfo.IMEITransferToRepairInfos = _iMEITransferToRepairInfoBusiness.GetIMEITransferToRepairInfosByQuery(transferId, null, null, null, null, null, null, null, null, null, null, User.OrgId).ToList();
                }
                TransferToPackagingRepairInfoViewModel viewModel = new TransferToPackagingRepairInfoViewModel();
                AutoMapper.Mapper.Map(dtoInfo, viewModel);
                return PartialView("_GetPackagingRepairItemReceiveDetail", viewModel);
            }
            else if (!string.IsNullOrEmpty(flag) && flag.Trim() == "ItemStock")
            {
                var dto = _packagingRepairItemStockInfoBusiness.GetPackagingRepairItemStockInfosByQuery(floorId, packagingLineId, modelId, warehouseId, itemTypeId, itemId, lessOrEq, User.OrgId);
                List<PackagingRepairItemStockInfoViewModel> viewModels = new List<PackagingRepairItemStockInfoViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetPackagingRepairItemStockInfo", viewModels);
            }
            else if (!string.IsNullOrEmpty(flag) && flag.Trim() == "RawStock")
            {
                var dto = _packagingRepairRawStockInfoBusiness.GetPackagingRepairRawStockInfosByQuery(floorId, packagingLineId, modelId, warehouseId, itemTypeId, itemId, lessOrEq, User.OrgId);
                List<PackagingRepairRawStockInfoViewModel> viewModels = new List<PackagingRepairRawStockInfoViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetPackagingRepairRawStockInfo", viewModels);
            }
            else if (!string.IsNullOrEmpty(flag) && flag.Trim() == "TransferList")
            {
                var dto = _transferPackagingRepairItemToQcInfoBusiness.GetTransferPackagingRepairItemToQcInfosByQuery(floorId, packagingLineId, modelId, warehouseId, itemTypeId, itemId, status, transferCode, fromDate, toDate, User.OrgId);
                List<TransferPackagingRepairItemToQcInfoViewModel> viewModels = new List<TransferPackagingRepairItemToQcInfoViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetPackagingRepairItemTransferToQCList", viewModels);
            }
            else if (!string.IsNullOrEmpty(flag) && flag.Trim() == "TransferDetail")
            {
                var dto = _transferPackagingRepairItemToQcDetailBusiness.GetTransferPackagingRepairItemToQcDetailByQuery(null, null, transferId, User.OrgId);
                ViewBag.StateStatus = dto.FirstOrDefault().StateStatus;
                List<TransferPackagingRepairItemToQcDetailViewModel> viewModels = new List<TransferPackagingRepairItemToQcDetailViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetPackagingRepairItemTransferToQCDetail", viewModels);
            }
            else if(!string.IsNullOrEmpty(flag) && flag.Trim() == "FaultyStock")
            {
                var dto = _packagingFaultyStockInfoBusiness.GetPackagingFaultyStockInfosByQuery(floorId, packagingLineId, modelId, warehouseId, itemTypeId, itemId, lessOrEq, User.OrgId);
                List<PackagingFaultyStockInfoViewModel> viewModels = new List<PackagingFaultyStockInfoViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetPackagingFaultyStockInfo", viewModels);
            }
            else if (!string.IsNullOrEmpty(flag) && flag.Trim() == "RequisitionList")
            {
                var dto = _repairSectionRequisitionInfoBusiness.GetRepairSectionRequisitionInfoList(null,packagingLineId, modelId, warehouseId, status, requisitionCode, fromDate, toDate, "Packaging", "Packaging", User.OrgId);
                // Pagination //
                //ViewBag.PagerData = GetPagerData(dto.Count(), pageSize, page);
                //dto = dto.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                //-----------------//
                List<RepairSectionRequisitionInfoViewModel> viewModels = new List<RepairSectionRequisitionInfoViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetRepairSectionRequisitionInfoList", viewModels);
            }
            else if (!string.IsNullOrEmpty(flag) && flag.Trim() == "RequisitionDetail")
            {
                var info = _repairSectionRequisitionInfoBusiness.GetRepairSectionRequisitionById(reqInfoId.Value, User.OrgId);

                var detailDto = _repairSectionRequisitionDetailBusiness.GetRepairSectionRequisitionDetailByInfoId(reqInfoId.Value, User.OrgId).Select(d => new RepairSectionRequisitionDetailDTO
                {
                    RSRDetailId = d.RSRDetailId,
                    ItemName = d.ItemName,
                    ItemTypeName = d.ItemTypeName,
                    RequestQty = d.RequestQty,
                    IssueQty = d.IssueQty,
                    UnitName = d.UnitName
                }).ToList();

                var infoDTO = new RepairSectionRequisitionInfoDTO
                {
                    RSRInfoId = info.RSRInfoId,
                    RequisitionCode = info.RequisitionCode,
                    RepairLineName = info.RepairLineName + " [" + info.ProductionFloorName + "]",
                    PackagingLineName = info.PackagingLineName + " [" + info.ProductionFloorName + "]",
                    ModelName = info.ModelName,
                    WarehouseName = info.WarehouseName,
                    StateStatus = info.StateStatus,
                    ReqFor = info.ReqFor
                };

                List<RepairSectionRequisitionDetailViewModel> detailViewModels = new List<RepairSectionRequisitionDetailViewModel>();
                RepairSectionRequisitionInfoViewModel infoViewModel = new RepairSectionRequisitionInfoViewModel();
                AutoMapper.Mapper.Map(detailDto, detailViewModels);
                AutoMapper.Mapper.Map(infoDTO, infoViewModel);

                ViewBag.Info = infoViewModel;

                return PartialView("_GetRepairSectionRequisitionDetailList", detailViewModels);
            }
            else if (!string.IsNullOrEmpty(flag) && flag.Trim() != "" && flag == "StockReturnList")
            {
                var dto = _stockItemReturnInfoBusiness.GetStockItemReturnInfosByQuery(modelId, floorId, null, null, packagingLineId, warehouseId, returnId, returnCode, StockRetunFlag.PackagingRepair, status, fromDate, toDate, User.OrgId);
                List<StockItemReturnInfoViewModel> viewModels = new List<StockItemReturnInfoViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetStockReturnList", viewModels);
            }
            return View();
        }

        public ActionResult CreatePackagingRepairStockReturn()
        {
            ViewBag.ddlPackagingLineWithProduction = _packagingLineBusiness.GetPackagingLinesWithProduction(User.OrgId).Select(s => new SelectListItem
            {
                Text = s.text,
                Value = s.value
            }).ToList();

            ViewBag.ddlModelName = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).Select(s => new SelectListItem
            {
                Text = s.DescriptionName,
                Value = s.DescriptionId.ToString()
            }).ToList();
            return View();
        }

        public ActionResult GetPackagingRepairStockItemsForReturn(long packagingLine, long floorId, long modelId)
        {
            var data = _packagingRepairRawStockInfoBusiness.GetPackagingRepairStocksForReturnStock(packagingLine, floorId, modelId, User.OrgId);
            IEnumerable<PackagingRepairRawStockInfoViewModel> viewModels = new List<PackagingRepairRawStockInfoViewModel>();
            AutoMapper.Mapper.Map(data, viewModels);
            return PartialView(viewModels);
        }

        #endregion

        #region Packaging Finish Goods Process
        public ActionResult GetPackagingFinishGoodsProcess(string flag, long? floorId,long? packagingLineId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string lessOrEq,string status,string transferCode, long? transferId,string fromDate, string toDate, int page = 1)
        {
            if (string.IsNullOrEmpty(flag))
            {
                ViewBag.ddlProductionFloor = _productionLineBusiness.GetAllProductionLineByOrgId(User.OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

                ViewBag.ddlPackagingLine = _packagingLineBusiness.GetPackagingLinesWithProduction(User.OrgId).Select(s => new SelectListItem
                {
                    Text = s.text,
                    Value = s.value
                }).ToList();

                ViewBag.ddlModelName = _descriptionBusiness.GetDescriptionByOrgId(User.OrgId).Select(des => new SelectListItem { Text = des.DescriptionName, Value = des.DescriptionId.ToString() }).ToList();

                var allItems = _itemBusiness.GetItemDetails(User.OrgId);

                ViewBag.ddlItems = allItems.Where(s => s.ItemName.Contains("Warehouse 3")).Select(s => new SelectListItem { Text = s.ItemName, Value = s.ItemId.ToString() }).ToList();

                var warehouse = _warehouseBusiness.GetAllWarehouseByOrgId(User.OrgId);

                ViewBag.ddlWarehouse3 = warehouse.Where(s=> s.WarehouseName.Contains("Warehouse 3")).Select(s => new SelectListItem { Text = s.WarehouseName, Value = s.Id.ToString() }).ToList();
            }
            if (!string.IsNullOrEmpty(flag) && flag.Trim()!="" && flag.ToLower() =="stockinfo")
            {
                var dto =_finishGoodsStockInfoBusiness.GetFinishGoodsStockInfosQuery(floorId, packagingLineId, modelId, warehouseId, itemTypeId, itemId, lessOrEq, User.OrgId);
                List<FinishGoodsStockInfoViewModel> viewModels = new List<FinishGoodsStockInfoViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetPackagingFinishGoodsStock", viewModels);
            }
            if (!string.IsNullOrEmpty(flag) && flag.Trim() != "" && flag.ToLower() == "transferinfo")
            {
                var dto =_finishGoodsSendToWarehouseInfoBusiness.GetFinishGoodsSendToWarehouseInfosByQuery(floorId, packagingLineId, warehouseId, modelId, status, transferCode, fromDate, toDate, transferId, User.OrgId);
                List<FinishGoodsSendToWarehouseInfoViewModel> viewModels = new List<FinishGoodsSendToWarehouseInfoViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);

                return PartialView("_GetFinishGoodsTransferInfo", viewModels);
            }
            if (!string.IsNullOrEmpty(flag) && flag.Trim() != "" && flag.ToLower() == "transferdetail")
            {
                var dto = _finishGoodsSendToWarehouseDetailBusiness.GetFinishGoodsSendToWarehouseDetailsByQuery(null, null,null,string.Empty,string.Empty,transferId,string.Empty, User.OrgId);
                List<FinishGoodsSendToWarehouseDetailViewModel> viewModels = new List<FinishGoodsSendToWarehouseDetailViewModel>();
                AutoMapper.Mapper.Map(dto, viewModels);
                return PartialView("_GetFinishGoodsTransferDetail", viewModels);
            }
            return View();
        }

        [HttpPost,ValidateJsonAntiForgeryToken]
        public async Task<ActionResult> SaveFinishGoodsForPacking(FinishGoodsSendToWarehouseInfoViewModel model)
        {
            bool IsSuccess = false;
            if (ModelState.IsValid)
            {
                FinishGoodsSendToWarehouseInfoDTO dto = new FinishGoodsSendToWarehouseInfoDTO();
                AutoMapper.Mapper.Map(model, dto);
                IsSuccess = await _finishGoodsSendToWarehouseInfoBusiness.SaveFinishGoodsCartonAsync(dto, User.UserId, User.OrgId);
            }
            return Json(IsSuccess);
        }

        #endregion

        #region LotIn
        public ActionResult CreateLot()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SaveQRCodeScanningForLotIn(string qrCode)
        {
            bool isSuccess = false;
            if (!string.IsNullOrEmpty(qrCode))
            {
                isSuccess = _tempQRCodeTraceBusiness.SaveQRCodeStatusByLotIn(qrCode, User.OrgId, User.UserId);
            }
            return Json(isSuccess);
        }
        public ActionResult GetLotInListByUserId(string qrCode, string fromDate, string toDate, int page = 1)
        {
            DateTime date = DateTime.Today;
            var dto = _lotInLogBusiness.GetAllLotInByToDay(User.UserId, User.OrgId, date).Select(s => new LotInLogDTO
            {
                LotInLogId = s.LotInLogId,
                CodeNo = s.CodeNo,
                EntryDate = s.EntryDate,
                EUserName = UserForEachRecord(User.UserId).UserName,
                StateStatus = s.StateStatus,
                ReferenceNumber = s.ReferenceNumber,
                Remarks = s.Remarks,
            }).OrderByDescending(s => s.LotInLogId).ToList();

            dto = dto.Where(s => (qrCode == null) || string.IsNullOrEmpty(qrCode) || s.CodeNo.Contains(qrCode.Trim())).ToList();

            if (!string.IsNullOrEmpty(fromDate) || !string.IsNullOrEmpty(toDate))
            {
                dto = _lotInLogBusiness.GetAllDataByDateWise(fromDate, toDate, qrCode).OrderByDescending(s => s.LotInLogId).ToList();
            }

            ViewBag.PagerData = GetPagerData(dto.Count(), 15, page);
            dto = dto.Skip((page - 1) * 15).Take(15).ToList();

            IEnumerable<LotInLogViewModel> viewModel = new List<LotInLogViewModel>();
            AutoMapper.Mapper.Map(dto, viewModel);
            return PartialView("_GetLotInList", viewModel);
        }
        #endregion

        public ActionResult GetStockReturnItemsDetail(long returnId)
        {
            var dto =_stockItemReturnDetailBusiness.GetStockItemReturnDetails(returnId, User.OrgId);
            List <StockItemReturnDetailViewModel> viewModel = new List<StockItemReturnDetailViewModel>();
            AutoMapper.Mapper.Map(dto, viewModel);
            return PartialView(viewModel);
        }

        public ActionResult GetStickerOneData()
        {
            var data = Utility.StrickerOne(10, 2);
            return View(data);
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}