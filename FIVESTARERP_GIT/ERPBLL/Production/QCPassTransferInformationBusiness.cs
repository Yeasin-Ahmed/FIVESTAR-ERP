using ERPBLL.Common;
using ERPBLL.Inventory.Interface;
using ERPBLL.Production.Interface;
using ERPBO.Inventory.DomainModels;
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
        private readonly QCPassTransferDetailRepository _qCPassTransferDetailRepository;

        private readonly IItemBusiness _itemBusiness;

        // Item Preparation //
        private readonly IItemPreparationInfoBusiness _itemPreparationInfoBusiness;
        private readonly IItemPreparationDetailBusiness _itemPreparationDetailBusiness;

        // Quality Control Stock//
        private readonly IQCLineStockDetailBusiness _qCLineStockDetailBusiness;
        private readonly QCItemStockDetailRepository _qCItemStockDetailRepository;

        // QC Item Stock
        private readonly IQCItemStockDetailBusiness _qCItemStockDetailBusiness;

        // Assembly Stock //
        private readonly IAssemblyLineStockDetailBusiness _assemblyLineStockDetailBusiness;

        // Temp QRCode //
        private readonly ITempQRCodeTraceBusiness _tempQRCodeTraceBusiness;
        public QCPassTransferInformationBusiness(IProductionUnitOfWork productionDb, IItemPreparationInfoBusiness itemPreparationInfoBusiness, IItemPreparationDetailBusiness itemPreparationDetailBusiness, IQCLineStockDetailBusiness qCLineStockDetailBusiness, IItemBusiness itemBusiness, IQCItemStockDetailBusiness qCItemStockDetailBusiness, ITempQRCodeTraceBusiness tempQRCodeTraceBusiness, QCPassTransferDetailRepository qCPassTransferDetailRepository, IAssemblyLineStockDetailBusiness assemblyLineStockDetailBusiness)
        {
            this._productionDb = productionDb;
            this._qCPassTransferInformationRepository = new QCPassTransferInformationRepository(this._productionDb);
            this._itemPreparationInfoBusiness = itemPreparationInfoBusiness;
            this._itemPreparationDetailBusiness = itemPreparationDetailBusiness;
            this._qCLineStockDetailBusiness = qCLineStockDetailBusiness;
            this._qCItemStockDetailRepository = new QCItemStockDetailRepository(this._productionDb);
            this._itemBusiness = itemBusiness;
            this._qCItemStockDetailBusiness = qCItemStockDetailBusiness;
            this._tempQRCodeTraceBusiness = tempQRCodeTraceBusiness;
            this._qCPassTransferDetailRepository = qCPassTransferDetailRepository;
            this._assemblyLineStockDetailBusiness = assemblyLineStockDetailBusiness;
        }
        public IEnumerable<QCPassTransferInformation> GetQCPassTransferInformation(long orgId)
        {
            return _qCPassTransferInformationRepository.GetAll(s => s.OrganizationId == orgId);
        }

        public async Task<QCPassTransferInformation> GetQCPassTransferInformationByFloorAssemblyQcModelItemTypeItem(long floorId, long assembly, long qc, long model, long itemType, long item, string status, long orgId)
        {
            return await _qCPassTransferInformationRepository.GetOneByOrgAsync(s => s.OrganizationId == orgId && s.ProductionFloorId == floorId && s.AssemblyLineId == assembly && s.QCLineId == qc && s.DescriptionId == model && s.ItemTypeId == itemType && s.ItemId == item && s.StateStatus == status);
        }

        public QCPassTransferInformation GetQCPassTransferInformationById(long qcPassId, long orgId)
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
                    OrganizationId = orgId,
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

        public async Task<bool> SaveQCPassTransferToMiniStockByQRCodeAsync(QCPassTransferInformationDTO qcPassInfo, string qrCode, long userId, long orgId)
        {
            // Previous Pending QCPass Info
            var qcPassInfoInDb = await GetQCPassTransferInformationByFloorAssemblyQcModelItemTypeItem(qcPassInfo.ProductionFloorId, qcPassInfo.AssemblyLineId, qcPassInfo.QCLineId, qcPassInfo.DescriptionId, qcPassInfo.ItemTypeId, qcPassInfo.ItemId, RequisitionStatus.Approved, orgId);

            // Item Preparation Info //
            var itemPreparationInfo = await _itemPreparationInfoBusiness.GetPreparationInfoByModelAndItemAndTypeAsync(ItemPreparationType.Production, qcPassInfo.DescriptionId, qcPassInfo.ItemId, orgId);

            // Item Preparation Detail //
            var itemPreparationDetail = (List<ItemPreparationDetail>)await _itemPreparationDetailBusiness.GetItemPreparationDetailsByInfoIdAsync(itemPreparationInfo.PreparationInfoId, orgId);

            string code = string.Empty;

            List<QCPassTransferDetail> qCPassTransferDetail = new List<QCPassTransferDetail>() {
                new QCPassTransferDetail(){
                    ProductionFloorId = qcPassInfo.ProductionFloorId,
                    ProductionFloorName = qcPassInfo.ProductionFloorName,
                    AssemblyLineId = qcPassInfo.AssemblyLineId,
                    AssemblyLineName = qcPassInfo.AssemblyLineName,
                    QCLineId = qcPassInfo.QCLineId,
                    QCLineName = qcPassInfo.QCLineName,
                    DescriptionId = qcPassInfo.DescriptionId,
                    Quantity = 1,
                    WarehouseId = qcPassInfo.WarehouseId,
                    ItemTypeId = qcPassInfo.ItemTypeId,
                    ItemId = qcPassInfo.ItemId,
                    OrganizationId = orgId,
                    EUserId = userId,
                    EntryDate = DateTime.Now,
                    QRCode = qrCode,
                    Remarks ="Item In By QRCode Scaning"
                }
            };
            if (qcPassInfoInDb != null)
            {
                code = qcPassInfoInDb.QCPassCode;
                // Exist
                qcPassInfoInDb.Quantity += 1;
                qcPassInfoInDb.UpdateDate = DateTime.Now;
                qcPassInfoInDb.UpUserId = userId;
                foreach (var item in qCPassTransferDetail)
                {
                    item.QPassId = qcPassInfoInDb.QPassId;
                }
            }
            else
            {
                var unitInDb = _itemBusiness.GetItemOneByOrgId(qcPassInfo.ItemId, orgId);
                code = "QCP-" + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss");
                qcPassInfoInDb = new QCPassTransferInformation()
                {
                    ProductionFloorId = qcPassInfo.ProductionFloorId,
                    ProductionFloorName = qcPassInfo.ProductionFloorName,
                    AssemblyLineId = qcPassInfo.AssemblyLineId,
                    AssemblyLineName = qcPassInfo.AssemblyLineName,
                    QCLineId = qcPassInfo.QCLineId,
                    QCLineName = qcPassInfo.QCLineName,
                    DescriptionId = qcPassInfo.DescriptionId,
                    Quantity = 1,
                    WarehouseId = qcPassInfo.WarehouseId,
                    ItemTypeId = qcPassInfo.ItemTypeId,
                    ItemId = qcPassInfo.ItemId,
                    StateStatus = "Send By QC",
                    OrganizationId = orgId,
                    EUserId = userId,
                    EntryDate = DateTime.Now,
                    Remarks ="Item In By QRCode Scaning",
                    QCPassCode = code,
                    UnitId= unitInDb.UnitId
                };
                qcPassInfoInDb.QCPassTransferDetails = qCPassTransferDetail;
            }

            // QC Item //
            List<QCItemStockDetailDTO> qCItemStockDetails = new List<QCItemStockDetailDTO>() {
                new QCItemStockDetailDTO(){
                    ProductionFloorId = qcPassInfoInDb.ProductionFloorId,
                    AssemblyLineId = qcPassInfoInDb.AssemblyLineId,
                    QCId = qcPassInfoInDb.QCLineId,
                    DescriptionId = qcPassInfoInDb.DescriptionId,
                    WarehouseId = qcPassInfoInDb.WarehouseId,
                    ItemTypeId = qcPassInfoInDb.ItemTypeId,
                    ItemId = qcPassInfoInDb.ItemId,
                    Quantity = 1,
                    Flag ="MiniStock",
                    StockStatus = StockStatus.StockOut,
                    OrganizationId = orgId,
                    EUserId = userId,
                    ReferenceNumber = code,
                    EntryDate = DateTime.Now,
                    Remarks = "Stock Out By QRCode Scanning QC Pass",
                }
            };

            // Assembly Stock //
            List<AssemblyLineStockDetailDTO> stockDetailDTOs = new List<AssemblyLineStockDetailDTO>();

            foreach (var item in itemPreparationDetail)
            {
                AssemblyLineStockDetailDTO assemblyStock = new AssemblyLineStockDetailDTO
                {
                    ProductionLineId = qcPassInfo.ProductionFloorId,
                    AssemblyLineId = qcPassInfo.AssemblyLineId,
                    DescriptionId = qcPassInfo.DescriptionId,
                    WarehouseId = item.WarehouseId,
                    ItemTypeId = item.ItemTypeId,
                    ItemId = item.ItemId,
                    OrganizationId = orgId,
                    EUserId = orgId,
                    Quantity = item.Quantity,
                    EntryDate = DateTime.Now,
                    UnitId = item.UnitId,
                    RefferenceNumber = code,
                    StockStatus = StockStatus.StockIn
                };
                stockDetailDTOs.Add(assemblyStock);
            }

            // Temp QRCode Status Change //
            if(qcPassInfoInDb.QPassId == 0)
            {
                _qCPassTransferInformationRepository.Insert(qcPassInfoInDb);
            }
            else
            {
                _qCPassTransferInformationRepository.Update(qcPassInfoInDb);
                _qCPassTransferDetailRepository.InsertAll(qCPassTransferDetail);
                //_qCPassTransferDetailRepository.Insert(qcPassInfoInDb);
            }
            if(await _qCPassTransferInformationRepository.SaveAsync())
            {
                if (await _tempQRCodeTraceBusiness.UpdateQRCodeStatusAsync(qrCode, QRCodeStatus.MiniStock, orgId))
                {
                    if(await _qCItemStockDetailBusiness.SaveQCItemStockOutAsync(qCItemStockDetails, userId, orgId))
                    {
                        return await _assemblyLineStockDetailBusiness.SaveAssemblyLineStockOutAsync(stockDetailDTOs, userId, orgId, string.Empty);
                    }
                }
            }
            return false;
        }

        public IEnumerable<QCPassTransferInformationDTO> GetQCPassTransferInformationsByQuery(long? floorId, long? assemblyId, long? qcLineId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId,string qcPassCode, string status, string fromDate, string toDate, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<QCPassTransferInformationDTO>(QueryQCPassTransferInformation(floorId, assemblyId, qcLineId, modelId, warehouseId, itemTypeId, itemId, qcPassCode, status, fromDate, toDate, orgId)).ToList();
        }

        private string QueryQCPassTransferInformation(long? floorId, long? assemblyId, long? qcLineId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId,string qcPassCode, string status, string fromDate, string toDate, long orgId)
        {
            string param = string.Empty;
            string query = string.Empty;

            param += string.Format(@" and qp.OrganizationId={0}", orgId);
            if (floorId != null && floorId > 0)
            {
                param += string.Format(@" and qp.ProductionFloorId={0}", floorId);
            }
            if (assemblyId != null && assemblyId > 0)
            {
                param += string.Format(@" and qp.AssemblyLineId={0}", assemblyId);
            }
            if (qcLineId != null && qcLineId > 0)
            {
                param += string.Format(@" and qp.QCLineId={0}", qcLineId);
            }
            if (modelId != null && modelId > 0)
            {
                param += string.Format(@" and qp.DescriptionId={0}", modelId);
            }
            if (warehouseId != null && warehouseId > 0)
            {
                param += string.Format(@" and qp.WarehouseId={0}", warehouseId);
            }
            if (itemTypeId != null && itemTypeId > 0)
            {
                param += string.Format(@" and qp.ItemTypeId={0}", itemTypeId);
            }
            if (itemId != null && itemId > 0)
            {
                param += string.Format(@" and qp.ItemId={0}", itemId);
            }
            if (!string.IsNullOrEmpty(qcPassCode) && qcPassCode != "")
            {
                param += string.Format(@" and qp.QCPassCode Like '%{0}%'", qcPassCode);
            }
            if (!string.IsNullOrEmpty(status) && status.Trim() != "")
            {
                param += string.Format(@" and qp.StateStatus ='{0}'", status);
            }
            if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "" && !string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(qp.EntryDate as date) between '{0}' and '{1}'", fDate, tDate);
            }
            else if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(qp.EntryDate as date)='{0}'", fDate);
            }
            else if (!string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(qp.EntryDate as date)='{0}'", tDate);
            }

            query = string.Format(@"Select qp.QPassId,qp.QCPassCode,qp.ProductionFloorId,pl.LineNumber 'ProductionFloorName',qp.QCLineId,qc.QCName 'QCLineName',al.AssemblyLineName
,al.AssemblyLineName,qp.DescriptionId,de.DescriptionName 'ModelName',qp.WarehouseId,w.WarehouseName,qp.ItemTypeId,it.ItemName 'ItemTypeName', qp.ItemId, i.ItemName,
qp.Quantity,qp.StateStatus,qp.EntryDate,app.UserName 'EntryUser',
qp.UpdateDate,(Select UserName From [ControlPanel].dbo.tblApplicationUsers  Where UserId = qp.UpUserId and OrganizationId= qp.OrganizationId) 'UpdateUser'
From [Production].dbo.tblQCPassTransferInformation qp
Inner Join [Production].dbo.tblProductionLines pl on qp.ProductionFloorId = pl.LineId
Inner Join [Production].dbo.tblQualityControl qc on qp.QCLineId = qc.QCId
Inner Join [Production].dbo.tblAssemblyLines al on qp.AssemblyLineId = al.AssemblyLineId
Inner Join [Inventory].dbo.tblDescriptions de on qp.DescriptionId = de.DescriptionId
Inner Join [Inventory].dbo.tblWarehouses w on qp.WarehouseId = w.Id
Inner Join [Inventory].dbo.tblItemTypes it on qp.ItemTypeId = it.ItemId
Inner Join [Inventory].dbo.tblItems i on qp.ItemId = i.ItemId
Inner Join [ControlPanel].dbo.tblApplicationUsers app on qp.EUserId = app.UserId
Where 1=1 {0} Order By qp.EntryDate desc", Utility.ParamChecker(param));

            return query;
        }

    }
}
