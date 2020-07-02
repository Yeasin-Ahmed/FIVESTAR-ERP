using ERPBLL.Common;
using ERPBLL.Inventory.Interface;
using ERPBLL.Production.Interface;
using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using ERPDAL.ProductionDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production
{
    public class RepairItemBusiness : IRepairItemBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly RepairItemRepository _repairItemRepository;
        private readonly IRepairLineStockDetailBusiness _repairLineStockDetailBusiness;
        private readonly IItemBusiness _itemBusiness;
        private readonly IUnitBusiness _unitBusiness;

        public RepairItemBusiness(IProductionUnitOfWork productionDb, IItemBusiness itemBusiness, IRepairLineStockDetailBusiness repairLineStockDetailBusiness, IUnitBusiness unitBusiness)
        {
            this._productionDb = productionDb;
            this._itemBusiness = itemBusiness;
            this._repairItemRepository = new RepairItemRepository(this._productionDb);
            this._repairLineStockDetailBusiness = repairLineStockDetailBusiness;
            this._unitBusiness = unitBusiness;
        }

        public bool SaveRepairItem(RepairItemDTO dto, long userId, long orgId)
        {
            bool IsSuccess = false;
            var unit = _itemBusiness.GetItemById(dto.ItemId.Value, orgId);
            RepairItem repairItem = new RepairItem
            {
                ProductionFloorId = dto.ProductionFloorId,
                RepairLineId = dto.RepairLineId,
                QCLineId = dto.QCLineId,
                QRCodeId = dto.QRCodeId,
                QRCode = dto.QRCode,
                RepairReason = dto.RepairReason,
                WarehouseId = dto.WarehouseId,
                ItemTypeId = dto.ItemTypeId,
                ItemId = dto.ItemId,
                DescriptionId = dto.DescriptionId,
                UnitId = unit.UnitId,
                UnitName = _unitBusiness.GetUnitOneByOrgId(unit.UnitId,orgId).UnitSymbol,
                EUserId = userId,
                EntryDate = DateTime.Now,
                OrganizationId = orgId,
                Remarks = dto.Remarks,
                ProductionFloorName=dto.ProductionFloorName,
                WarehouseName = dto.WarehouseName,
                ItemTypeName = dto.ItemTypeName,
                ItemName = dto.ItemName,
                ModelName= dto.ModelName,
                QCLineName = dto.QCLineName,
                RepairLineName = dto.RepairLineName,
                RepairCode = ("RPC-" + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss"))
            };
            List<RepairItemProblem> repairItemProblems = new List<RepairItemProblem>();
            List<FaultyItemStockDetailDTO> faultyItemStocks = new List<FaultyItemStockDetailDTO>();

            foreach (var item in dto.RepairItemProblems)
            {
                RepairItemProblem problem = new RepairItemProblem
                {
                    QRCode = item.QRCode,
                    QRCodeId = item.QRCodeId,
                    ProblemId = item.ProblemId.Value,
                    Problem = item.Problem,
                    EUserId = userId,
                    EntryDate = DateTime.Now,
                    OrganizationId = orgId,
                    ReferenceNumber = repairItem.RepairCode
                };
                repairItemProblems.Add(problem);
            }
            repairItem.RepairItemProblems = repairItemProblems;

            List<RepairItemParts> repairItemParts = new List<RepairItemParts>();

            foreach (var item in dto.RepairItemParts)
            {
                RepairItemParts parts = new RepairItemParts()
                {
                    QRCode = item.QRCode,
                    QRCodeId = item.QRCodeId,
                    Qty = item.Qty,
                    WarehouseId = item.WarehouseId,
                    ItemTypeId = item.ItemTypeId,
                    ItemId = item.ItemId,
                    OrganizationId = orgId,
                    EUserId = userId,
                    EntryDate = DateTime.Now,
                    ReferenceNumber = repairItem.RepairCode,
                    UnitId = _itemBusiness.GetItemById(dto.ItemId.Value, orgId).UnitId
                };
                repairItemParts.Add(parts);

                if (repairItem.RepairReason != RepairReason.SoftwareProblem)
                {
                    FaultyItemStockDetailDTO faultyItem = new FaultyItemStockDetailDTO()
                    {
                        ProductionFloorId = repairItem.ProductionFloorId,
                        QCId = repairItem.QCLineId,
                        RepairLineId = repairItem.RepairLineId,
                        WarehouseId = item.WarehouseId,
                        ItemTypeId = item.ItemTypeId,
                        ItemId = item.ItemId,
                        UnitId = _itemBusiness.GetItemById(item.ItemId.Value, orgId).UnitId,
                        DescriptionId = repairItem.DescriptionId,
                        OrganizationId = orgId,
                        EUserId = userId,
                        EntryDate = DateTime.Now,
                        Quantity = item.Qty,
                        ReferenceNumber = repairItem.RepairCode,
                        Remarks = "Faulty Stock In By QRCode",
                        StockStatus = StockStatus.StockIn
                    };
                    faultyItemStocks.Add(faultyItem);
                }
            }
            repairItem.RepairItemParts = repairItemParts;

            this._repairItemRepository.Insert(repairItem);
            if (this._repairItemRepository.Save())
            {
                IsSuccess = true;
                if (repairItem.RepairReason != RepairReason.SoftwareProblem)
                {
                    IsSuccess=this._repairLineStockDetailBusiness.StockOutByFaultyItem(faultyItemStocks, userId, orgId);
                }
            }
            return IsSuccess;
        }
    }
}
