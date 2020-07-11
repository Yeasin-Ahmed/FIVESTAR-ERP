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
    public class QCPassTransferInformationBusiness : IQCPassTransferInformationBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly QCPassTransferInformationRepository _qCPassTransferInformationRepository;

        private readonly IItemBusiness _itemBusiness;

        // Item Preparation //
        private readonly IItemPreparationInfoBusiness _itemPreparationInfoBusiness;
        private readonly IItemPreparationDetailBusiness _itemPreparationDetailBusiness;

        // Quality Control Stock//
        private readonly IQCLineStockDetailBusiness _qCLineStockDetailBusiness;
        private readonly QCItemStockDetailRepository _qCItemStockDetailRepository;

        // QC Item Stock
        private readonly IQCItemStockDetailBusiness _qCItemStockDetailBusiness;
        public QCPassTransferInformationBusiness(IProductionUnitOfWork productionDb, IItemPreparationInfoBusiness itemPreparationInfoBusiness, IItemPreparationDetailBusiness itemPreparationDetailBusiness, IQCLineStockDetailBusiness qCLineStockDetailBusiness, IItemBusiness itemBusiness, IQCItemStockDetailBusiness qCItemStockDetailBusiness)
        {
            this._productionDb = productionDb;
            this._qCPassTransferInformationRepository = new QCPassTransferInformationRepository(this._productionDb);
            this._itemPreparationInfoBusiness = itemPreparationInfoBusiness;
            this._itemPreparationDetailBusiness = itemPreparationDetailBusiness;
            this._qCLineStockDetailBusiness = qCLineStockDetailBusiness;
            this._qCItemStockDetailRepository = new QCItemStockDetailRepository(this._productionDb);
            this._itemBusiness = itemBusiness;
            this._qCItemStockDetailBusiness = qCItemStockDetailBusiness;
        }
        public IEnumerable<QCPassTransferInformation> GetQCPassTransferInformation(long orgId)
        {
            return _qCPassTransferInformationRepository.GetAll(s => s.OrganizationId == orgId);
        }

        public QCPassTransferInformation GetQCPassTransferInformationById(long qcPassId,long orgId)
        {
            return _qCPassTransferInformationRepository.GetOneByOrg(s => s.QPassId == qcPassId && s.OrganizationId == orgId);
        }

        public bool SaveQCPassTransferInformation(QCPassTransferInformationDTO qcPassInfo, long userId, long orgId)
        {
            QCPassTransferInformation info = new QCPassTransferInformation()
            {
                ProductionFloorId = qcPassInfo.ProductionFloorId,
                ProductionFloorName = qcPassInfo.ProductionFloorName,
                QCLineId = qcPassInfo.QCLineId,
                QCLineName = qcPassInfo.QCLineName,
                DescriptionId = qcPassInfo.DescriptionId,
                ModelName = qcPassInfo.ModelName,
                WarehouseId = qcPassInfo.WarehouseId,
                WarehouseName = qcPassInfo.WarehouseName,
                ItemTypeId = qcPassInfo.ItemTypeId,
                ItemTypeName = qcPassInfo.ItemTypeName,
                ItemId = qcPassInfo.ItemId,
                ItemName = qcPassInfo.ItemName,
                UnitId = _itemBusiness.GetItemById(qcPassInfo.ItemId, orgId).UnitId,
                UnitName = qcPassInfo.UnitName,
                OrganizationId = orgId,
                EUserId = qcPassInfo.EUserId,
                EntryDate = DateTime.Now,
                Quantity = qcPassInfo.Quantity,
                StateStatus = "Send By QC",
                Remarks = "QC Pass Items"
            };

            // Item preparation Info //

            var itemPreparationInfo = _itemPreparationInfoBusiness.GetPreparationInfoByModelAndItemAndType(ItemPreparationType.Production, qcPassInfo.DescriptionId, qcPassInfo.ItemId, orgId);

            // Item preparation Detail //
            var itemPreparationDetail = _itemPreparationDetailBusiness.GetItemPreparationDetailsByInfoId(itemPreparationInfo.PreparationInfoId, orgId);

            List<QCPassTransferDetail> details = new List<QCPassTransferDetail>();

            // QC Line Raw Stock
            List<QualityControlLineStockDetailDTO> qcLineStockDtos = new List<QualityControlLineStockDetailDTO>();
            foreach (var item in itemPreparationDetail)
            {
                QCPassTransferDetail detail = new QCPassTransferDetail()
                {
                    ProductionFloorId = info.ProductionFloorId,
                    ProductionFloorName = info.ProductionFloorName,
                    DescriptionId = info.DescriptionId,
                    ModelName = info.ModelName,
                    QCLineId = info.QCLineId,
                    QCLineName = info.QCLineName,
                    WarehouseId = item.WarehouseId,
                    ItemTypeId = item.ItemTypeId,
                    ItemId = item.ItemId,
                    UnitId = item.UnitId,
                    OrganizationId = orgId,
                    EUserId = item.EUserId,
                    EntryDate = DateTime.Now,
                    Quantity = (item.Quantity * info.Quantity),
                    Remarks = "QC Pass Item"
                };
                details.Add(detail);

                QualityControlLineStockDetailDTO qcStock = new QualityControlLineStockDetailDTO()
                {
                    ProductionLineId = info.ProductionFloorId,
                    QCLineId = info.QCLineId,
                    DescriptionId = info.DescriptionId,
                    WarehouseId = item.WarehouseId,
                    ItemTypeId = item.ItemTypeId,
                    ItemId = item.ItemId,
                    UnitId = item.UnitId,
                    OrganizationId =orgId,
                    EUserId = item.EUserId,
                    EntryDate = DateTime.Now,
                    Quantity = (item.Quantity * info.Quantity),
                    Remarks = "QC Pass Item",
                    StockStatus = StockStatus.StockOut,
                    RefferenceNumber = ""
                };
                qcLineStockDtos.Add(qcStock);
            }
            info.QCPassTransferDetails = details;


            if (_qCLineStockDetailBusiness.SaveQCLineStockOut(qcLineStockDtos, userId, orgId, string.Empty))
            {
                List<QCItemStockDetailDTO> stockDetailDTOs = new List<QCItemStockDetailDTO>() {
                new QCItemStockDetailDTO()
                {
                    ProductionFloorId = info.ProductionFloorId,
                    QCId = info.QCLineId,
                    DescriptionId = info.DescriptionId,
                    ItemTypeId= info.ItemTypeId,
                    ItemId = info.ItemId,
                    WarehouseId = info.WarehouseId,
                    Quantity = info.Quantity,
                    OrganizationId =orgId,
                    EntryDate = DateTime.Now,
                    EUserId =userId,StockStatus= StockStatus.StockOut,
                    Flag ="MiniStock",
                    ReferenceNumber =string.Empty,
                    Remarks =info.Remarks
                }
            };

                if (_qCItemStockDetailBusiness.SaveQCItemStockOut(stockDetailDTOs, userId, orgId))
                {
                    _qCPassTransferInformationRepository.Insert(info);
                    return _qCPassTransferInformationRepository.Save();
                }
            }

            // QC Item Stock






            return false;
        }
    }
}
