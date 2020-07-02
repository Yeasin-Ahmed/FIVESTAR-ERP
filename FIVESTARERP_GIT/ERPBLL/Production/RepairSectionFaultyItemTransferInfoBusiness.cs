using ERPBLL.Common;
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
    public class RepairSectionFaultyItemTransferInfoBusiness : IRepairSectionFaultyItemTransferInfoBusiness
    {
        private readonly IProductionUnitOfWork _production;
        private readonly RepairSectionFaultyItemTransferInfoRepository _repairSectionFaultyItemTransferInfoRepository;
        private readonly IFaultyItemStockDetailBusiness _faultyItemStockDetailBusiness;
        public RepairSectionFaultyItemTransferInfoBusiness(IProductionUnitOfWork production, IFaultyItemStockDetailBusiness faultyItemStockDetailBusiness)
        {
            this._production = production;
            this._repairSectionFaultyItemTransferInfoRepository = new RepairSectionFaultyItemTransferInfoRepository(this._production);
            this._faultyItemStockDetailBusiness = faultyItemStockDetailBusiness;
        }

        public IEnumerable<RepairSectionFaultyItemTransferInfo> GetRepairSectionFaultyItemTransferInfoByFloor(long floorId, long orgId)
        {
            return _repairSectionFaultyItemTransferInfoRepository.GetAll(f => f.ProductionFloorId == floorId && f.OrganizationId == orgId);
        }

        public IEnumerable<RepairSectionFaultyItemTransferInfo> GetRepairSectionFaultyItemTransferInfoByOrg(long orgId)
        {
            return _repairSectionFaultyItemTransferInfoRepository.GetAll(f => f.OrganizationId == orgId);
        }

        public IEnumerable<RepairSectionFaultyItemTransferInfo> GetRepairSectionFaultyItemTransferInfoByRepairLine(long floorId, long RepairLine, long orgId)
        {
            return _repairSectionFaultyItemTransferInfoRepository.GetAll(f => f.ProductionFloorId == floorId && f.RepairLineId == RepairLine && f.OrganizationId == orgId);
        }

        public bool SaveRepairSectionFaultyItemTransfer(RepairSectionFaultyItemTransferInfoDTO faultyItems, long orgId, long userId)
        {
            bool IsSuccess = false;
            string code = (DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss"));
            RepairSectionFaultyItemTransferInfo info = new RepairSectionFaultyItemTransferInfo
            {
                ProductionFloorId = faultyItems.ProductionFloorId,
                ProductionFloorName = faultyItems.ProductionFloorName,
                RepairLineId = faultyItems.RepairLineId,
                RepairLineName = faultyItems.RepairLineName,
                StateStatus = RequisitionStatus.Approved,
                OrganizationId = orgId,
                EUserId = userId,
                EntryDate = DateTime.Now,
                TransferCode = "RSFIT-"+ code,
                Remarks = faultyItems.Remarks
            };
            List<RepairSectionFaultyItemTransferDetail> details = new List<RepairSectionFaultyItemTransferDetail>();
            List<FaultyItemStockDetailDTO> faultyItemStocks = new List<FaultyItemStockDetailDTO>();
            foreach (var item in faultyItems.RepairSectionFaultyItemRequisitionDetails)
            {
                RepairSectionFaultyItemTransferDetail detail = new RepairSectionFaultyItemTransferDetail
                {
                    ProductionFloorId = item.ProductionFloorId,
                    ProductionFloorName = item.ProductionFloorName,
                    RepairLineId = item.RepairLineId,
                    RepairLineName = item.RepairLineName,
                    QCLineId = item.QCLineId,
                    QCLineName = item.QCLineName,
                    DescriptionId = item.DescriptionId,
                    ModelName = item.ModelName,
                    WarehouseId = item.WarehouseId,
                    WarehouseName = item.WarehouseName,
                    ItemTypeId = item.ItemTypeId,
                    ItemTypeName = item.ItemTypeName,
                    ItemId = item.ItemId,
                    ItemName = item.ItemName,
                    UnitId = item.UnitId,
                    UnitName = item.UnitName,
                    FaultyQty = item.FaultyQty,
                    OrganizationId = orgId,
                    EUserId = userId,
                    EntryDate = DateTime.Now,
                    ReferenceNumber = faultyItems.TransferCode
                };
                details.Add(detail);

                FaultyItemStockDetailDTO faulty = new FaultyItemStockDetailDTO {
                    ProductionFloorId = item.ProductionFloorId,
                    RepairLineId = item.RepairLineId,
                    QCId = item.QCLineId,
                    DescriptionId = item.DescriptionId,
                    WarehouseId = item.WarehouseId,
                    ItemTypeId = item.ItemTypeId,
                    ItemId = item.ItemId,
                    UnitId = item.UnitId,
                    ReferenceNumber = info.TransferCode,
                    Quantity = item.FaultyQty,
                    OrganizationId= orgId,
                    EUserId = userId,
                    EntryDate = DateTime.Now,
                    StockStatus = StockStatus.StockOut
                };
                faultyItemStocks.Add(faulty);
            }

            var totalUnit = details.Select(d => d.FaultyQty).Sum();
            info.TotalUnit = totalUnit;
            info.RepairSectionFaultyItemRequisitionDetails = details;

            _repairSectionFaultyItemTransferInfoRepository.Insert(info);

            if (_repairSectionFaultyItemTransferInfoRepository.Save()) {
                // Faulty Item Stock Out .....
                IsSuccess= this._faultyItemStockDetailBusiness.SaveFaultyItemStockOut(faultyItemStocks,userId,orgId);
            }
            return IsSuccess;
        }
    }
}
