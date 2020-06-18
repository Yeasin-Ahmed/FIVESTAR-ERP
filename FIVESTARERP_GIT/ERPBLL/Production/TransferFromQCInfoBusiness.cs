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
    public class TransferFromQCInfoBusiness : ITransferFromQCInfoBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly TransferFromQCInfoRepository _transferFromQCInfoRepository;
        private readonly IQCLineStockDetailBusiness _qCLineStockDetailBusiness;
        private readonly IItemBusiness _itemBusiness;

        public TransferFromQCInfoBusiness(IProductionUnitOfWork productionDb, IQCLineStockDetailBusiness qCLineStockDetailBusiness, IItemBusiness itemBusiness)
        {
            this._productionDb = productionDb;
            this._transferFromQCInfoRepository = new TransferFromQCInfoRepository(this._productionDb);
            this._qCLineStockDetailBusiness = qCLineStockDetailBusiness;
            this._itemBusiness = itemBusiness;
        }

        public TransferFromQCInfo GetTransferFromQCInfoById(long transferId, long orgId)
        {
            return _transferFromQCInfoRepository.GetOneByOrg(t => t.TFQInfoId == transferId && t.OrganizationId == orgId);
        }
        public IEnumerable<TransferFromQCInfo> GetTransferFromQCInfos(long orgId)
        {
            return _transferFromQCInfoRepository.GetAll(t =>t.OrganizationId == orgId);
        }
        public bool SaveTransferInfoStateStatus(long transferId, string status, long userId, long orgId)
        {
            var transferInDb = GetTransferFromQCInfoById(transferId, orgId);
            if(transferInDb != null)
            {
                transferInDb.StateStatus = RequisitionStatus.Accepted;
                transferInDb.UpUserId = userId;
                transferInDb.UpdateDate = DateTime.Now;
                _transferFromQCInfoRepository.Update(transferInDb);
            }
            return _transferFromQCInfoRepository.Save();
        }
        public bool SaveTransfer(TransferFromQCInfoDTO infoDto, List<TransferFromQCDetailDTO> detailDto, long userId, long orgId)
        {
            bool IsSuccess = false;
            TransferFromQCInfo info = new TransferFromQCInfo
            {
                TransferCode = ("TFQ-" + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss")),
                LineId = infoDto.LineId,
                DescriptionId = infoDto.DescriptionId,
                WarehouseId = infoDto.WarehouseId,
                QCLineId = infoDto.QCLineId,
                RepairLineId = infoDto.RepairLineId,
                PackagingLineId = infoDto.PackagingLineId,
                TransferFor = infoDto.TransferFor,
                StateStatus = RequisitionStatus.Approved,
                Remarks = infoDto.Remarks,
                OrganizationId = orgId,
                EUserId = userId,
                EntryDate = DateTime.Now
            };
            List<TransferFromQCDetail> listOfDetail = new List<TransferFromQCDetail>();
            List<QualityControlLineStockDetailDTO> stockDetail = new List<QualityControlLineStockDetailDTO>();
            foreach (var item in detailDto)
            {
                TransferFromQCDetail detail = new TransferFromQCDetail
                {
                    ItemTypeId = item.ItemTypeId,
                    ItemId = item.ItemId,
                    UnitId = _itemBusiness.GetItemOneByOrgId(item.ItemId.Value,orgId).UnitId,
                    Quantity = item.Quantity,
                    Remarks = item.Remarks,
                    OrganizationId = orgId,
                    EUserId = userId,
                    EntryDate =  DateTime.Now
                };
                listOfDetail.Add(detail);
                QualityControlLineStockDetailDTO stock = new QualityControlLineStockDetailDTO
                {
                    DescriptionId = info.DescriptionId,
                    ProductionLineId = info.LineId,
                    ItemTypeId = item.ItemTypeId,
                    ItemId = item.ItemId,
                    UnitId = item.UnitId,
                    WarehouseId = info.WarehouseId,
                    QCLineId = info.QCLineId,
                    RefferenceNumber = info.TransferCode,
                    Quantity = item.Quantity,
                    Remarks = "Stock Out For "+ info.TransferFor + " ("+ info.TransferCode+")",
                    EUserId = userId,
                    EntryDate = DateTime.Now,
                    OrganizationId = orgId,
                    StockStatus = StockStatus.StockOut
                };
                stockDetail.Add(stock);
            }
            info.TransferFromQCDetails = listOfDetail;
            _transferFromQCInfoRepository.Insert(info);
            if (_transferFromQCInfoRepository.Save())
            {
                IsSuccess = _qCLineStockDetailBusiness.SaveQCLineStockOut(stockDetail, userId, orgId, "Stock Out By QC Transfer");
            }
            return IsSuccess;
        }       
    }
}
