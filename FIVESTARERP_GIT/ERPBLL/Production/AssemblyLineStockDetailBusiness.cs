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
    public class AssemblyLineStockDetailBusiness : IAssemblyLineStockDetailBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly AssemblyLineStockDetailRepository _assemblyLineStockDetailRepository;
        private readonly AssemblyLineStockInfoRepository _assemblyLineStockInfoRepository;
        private readonly ITransferStockToAssemblyDetailBusiness _transferStockToAssemblyDetailBusiness;
        private readonly ITransferStockToAssemblyInfoBusiness _transferStockToAssemblyInfoBusiness;
        private readonly IAssemblyLineStockInfoBusiness _assemblyLineStockInfoBusiness;
        private readonly IQRCodeTraceBusiness _qRCodeTraceBusiness;

        public AssemblyLineStockDetailBusiness(IProductionUnitOfWork productionDb, ITransferStockToAssemblyDetailBusiness transferStockToAssemblyDetailBusiness, ITransferStockToAssemblyInfoBusiness transferStockToAssemblyInfoBusiness, IAssemblyLineStockInfoBusiness assemblyLineStockInfoBusiness, IQRCodeTraceBusiness qRCodeTraceBusiness)
        {
            this._productionDb = productionDb;
            this._assemblyLineStockInfoRepository = new AssemblyLineStockInfoRepository(this._productionDb);
            this._assemblyLineStockDetailRepository = new AssemblyLineStockDetailRepository(this._productionDb);
            this._transferStockToAssemblyDetailBusiness = transferStockToAssemblyDetailBusiness;
            this._transferStockToAssemblyInfoBusiness = transferStockToAssemblyInfoBusiness;
            this._assemblyLineStockInfoBusiness = assemblyLineStockInfoBusiness;
            this._qRCodeTraceBusiness = qRCodeTraceBusiness;
        }

        public IEnumerable<AssemblyLineStockDetail> GetAssemblyLineStockDetails(long orgId)
        {
            throw new NotImplementedException();
        }

        public bool SaveAssemblyLineStockIn(List<AssemblyLineStockDetailDTO> assemblyLineStockDetailDTO, long userId, long orgId)
        {
            List<AssemblyLineStockDetail> assemblyLineStockDetail = new List<AssemblyLineStockDetail>();
            foreach (var item in assemblyLineStockDetailDTO)
            {
                AssemblyLineStockDetail stockDetail = new AssemblyLineStockDetail();
                stockDetail.AssemblyLineId = item.AssemblyLineId;
                stockDetail.ProductionLineId = item.ProductionLineId;
                stockDetail.DescriptionId = item.DescriptionId;
                stockDetail.WarehouseId = item.WarehouseId;

                stockDetail.ItemTypeId = item.ItemTypeId;
                stockDetail.ItemId = item.ItemId;
                stockDetail.Quantity = item.Quantity;
                stockDetail.OrganizationId = orgId;
                stockDetail.EUserId = userId;
                stockDetail.Remarks = item.Remarks;
                stockDetail.UnitId = item.UnitId;
                stockDetail.EntryDate = DateTime.Now;
                stockDetail.StockStatus = StockStatus.StockIn;
                stockDetail.RefferenceNumber = item.RefferenceNumber;

                var assemblyStockInfo = _assemblyLineStockInfoBusiness.GetAssemblyLineStockInfos(orgId).Where(o => o.ItemTypeId == item.ItemTypeId && o.ItemId == item.ItemId && o.ProductionLineId == item.ProductionLineId && o.DescriptionId == item.DescriptionId && o.AssemblyLineId == item.AssemblyLineId).FirstOrDefault();
                if (assemblyStockInfo != null)
                {
                    assemblyStockInfo.StockInQty += item.Quantity;
                    _assemblyLineStockInfoRepository.Update(assemblyStockInfo);
                }
                else
                {
                    AssemblyLineStockInfo info = new AssemblyLineStockInfo();
                    info.AssemblyLineId = item.AssemblyLineId;
                    info.ProductionLineId = item.ProductionLineId;
                    info.WarehouseId = item.WarehouseId;
                    info.DescriptionId = item.DescriptionId;

                    info.ItemTypeId = item.ItemTypeId;
                    info.ItemId = item.ItemId;
                    info.UnitId = stockDetail.UnitId;
                    info.StockInQty = item.Quantity;
                    info.StockOutQty = 0;
                    info.OrganizationId = orgId;
                    info.EUserId = userId;
                    info.EntryDate = DateTime.Now;
                    
                    _assemblyLineStockInfoRepository.Insert(info);
                }
                assemblyLineStockDetail.Add(stockDetail);
            }
            _assemblyLineStockDetailRepository.InsertAll(assemblyLineStockDetail);
            return _assemblyLineStockDetailRepository.Save();
        }

        public bool SaveAssemblyLineStockOut(List<AssemblyLineStockDetailDTO> assemblyLineStockDetailDTO, long userId, long orgId, string flag)
        {
            List<AssemblyLineStockDetail> assemblyLineStockDetail = new List<AssemblyLineStockDetail>();
            foreach (var item in assemblyLineStockDetailDTO)
            {
                AssemblyLineStockDetail stockDetail = new AssemblyLineStockDetail();
                stockDetail.AssemblyLineId = item.AssemblyLineId;
                stockDetail.ProductionLineId = item.ProductionLineId;
                stockDetail.DescriptionId = item.DescriptionId;
                stockDetail.WarehouseId = item.WarehouseId;

                stockDetail.ItemTypeId = item.ItemTypeId;
                stockDetail.ItemId = item.ItemId;
                stockDetail.Quantity = item.Quantity;
                stockDetail.OrganizationId = orgId;
                stockDetail.EUserId = userId;
                stockDetail.Remarks = item.Remarks;
                stockDetail.UnitId = item.UnitId;
                stockDetail.EntryDate = DateTime.Now;
                stockDetail.StockStatus = StockStatus.StockIn;
                stockDetail.RefferenceNumber = item.RefferenceNumber;

                var assemblyStockInfo = _assemblyLineStockInfoBusiness.GetAssemblyLineStockInfos(orgId).Where(o => o.ItemTypeId == item.ItemTypeId && o.ItemId == item.ItemId && o.ProductionLineId == item.ProductionLineId && o.DescriptionId == item.DescriptionId && o.AssemblyLineId == item.AssemblyLineId).FirstOrDefault();
                assemblyStockInfo.StockOutQty += item.Quantity;
                _assemblyLineStockInfoRepository.Update(assemblyStockInfo);
                assemblyLineStockDetail.Add(stockDetail);
            }
            _assemblyLineStockDetailRepository.InsertAll(assemblyLineStockDetail);
            return _assemblyLineStockDetailRepository.Save();
        }

        public bool SaveAssemblyStockInByProductionLine(long transferId, string status, long orgId, long userId)
        {
            bool IsSuccess = false;
            if(transferId > 0)
            {
                var info = _transferStockToAssemblyInfoBusiness.GetStockToAssemblyInfoById(transferId, orgId);
                if(info != null && info.StateStatus == RequisitionStatus.Approved)
                {
                    var details = _transferStockToAssemblyDetailBusiness.GetTransferStockToAssemblyDetailByInfo(transferId, orgId);
                    if (details.Count() > 0)
                    {
                        List<AssemblyLineStockDetailDTO> stockDetails = new List<AssemblyLineStockDetailDTO>();
                        List<QRCodeTraceDTO> qRCodes = GenerateQRCodeTraces(info.TSAInfoId, userId, orgId);
                        foreach (var item in details)
                        {
                            AssemblyLineStockDetailDTO detailItem = new AssemblyLineStockDetailDTO()
                            {
                                AssemblyLineId = info.AssemblyId,
                                ProductionLineId = info.LineId,
                                DescriptionId = info.DescriptionId,
                                WarehouseId = item.WarehouseId,
                                ItemTypeId= item.ItemTypeId,
                                ItemId = item.ItemId,
                                UnitId = item.UnitId,
                                Quantity = item.Quantity,
                                EUserId = userId,
                                EntryDate = DateTime.Now,
                                OrganizationId = orgId,
                                StockStatus = StockStatus.StockIn,
                                RefferenceNumber = info.TransferCode,
                                Remarks = "Stock In by Production Floor Line",
                            };
                            stockDetails.Add(detailItem);
                        }

                        if (_transferStockToAssemblyInfoBusiness.SaveTransferInfoStateStatus(transferId, status, userId, orgId))
                        {
                            if (SaveAssemblyLineStockIn(stockDetails, userId, orgId)) {
                                IsSuccess = _qRCodeTraceBusiness.SaveQRCodeTrace(qRCodes, userId, orgId);
                            };
                        }
                    }
                    // details
                }// info
            }
            return IsSuccess;
        }

        private List<QRCodeTraceDTO> GenerateQRCodeTraces(long refNo, long userId, long orgId)
        {
            List<QRCodeTraceDTO> qRCodeTraces = new List<QRCodeTraceDTO>();
            TransferStockToAssemblyInfoDTO reqDto = new TransferStockToAssemblyInfoDTO();
            reqDto = this._productionDb.Db.Database.SqlQuery<TransferStockToAssemblyInfoDTO>(string.Format(@"Select ta.TSAInfoId, ta.TransferCode, ta.DescriptionId, ta.LineId, ta.WarehouseId, ta.AssemblyId, ta.StateStatus, ta.Remarks, ta.OrganizationId,pl.LineNumber, ta.ItemTypeId, ta.ItemId, ta.ForQty,de.DescriptionName 'ModelName',wa.WarehouseName,it.ItemName 'ItemTypeName', i.ItemName ,al.AssemblyLineName 'AssemblyName'
from tblTransferStockToAssemblyInfo ta
Inner Join tblProductionLines pl on ta.LineId = pl.LineId
Inner Join tblAssemblyLines al on ta.AssemblyId = al.AssemblyLineId
Inner Join [Inventory].dbo.tblDescriptions de on  ta.DescriptionId = de.DescriptionId
Inner Join [Inventory].dbo.tblWarehouses wa on  ta.WarehouseId = wa.Id
Inner Join [Inventory].dbo.tblItemTypes it on ta.ItemTypeId = it.ItemId
Inner Join [Inventory].dbo.tblItems i on ta.ItemId = i.ItemId
Where ta.OrganizationId={0} and ta.TSAInfoId={1}", orgId, refNo)).SingleOrDefault();

            string tCode = reqDto.TransferCode.Substring(4);
            for (int i = 1; i <= reqDto.ForQty; i++)
            {
                QRCodeTraceDTO qRCode = new QRCodeTraceDTO
                {
                    ProductionFloorId = reqDto.LineId,
                    DescriptionId = reqDto.DescriptionId,
                    ItemTypeId = reqDto.ItemTypeId,
                    ItemId = reqDto.ItemId,
                    WarehouseId = reqDto.WarehouseId,
                    CodeId = 0,
                    ColorName = string.Empty,
                    OrganizationId = orgId,
                    EUserId = userId,
                    EntryDate = DateTime.Now,
                    ReferenceId = reqDto.TSAInfoId.ToString(),
                    ReferenceNumber = reqDto.TransferCode,
                    ColorId = 0,
                    ModelName = reqDto.ModelName,
                    WarehouseName= reqDto.WarehouseName,
                    ItemTypeName = reqDto.ItemTypeName,
                    ItemName = reqDto.ItemName,
                    ProductionFloorName = reqDto.LineNumber,
                    CodeNo = reqDto.ModelName + "-" + tCode + "-" + i.ToString(),
                    AssemblyId = reqDto.AssemblyId,
                    AssemblyLineName = reqDto.AssemblyName
                };
                qRCodeTraces.Add(qRCode);
            }
            return qRCodeTraces;
        }

    }
}
