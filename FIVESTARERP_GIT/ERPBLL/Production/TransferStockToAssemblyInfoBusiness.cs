using ERPBLL.Common;
using ERPBLL.Inventory.Interface;
using ERPBLL.Production.Interface;
using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using ERPBO.Production.ViewModels;
using ERPDAL.ProductionDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production
{
    public class TransferStockToAssemblyInfoBusiness : ITransferStockToAssemblyInfoBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly IItemBusiness _itemBusiness;
        private readonly IProductionStockDetailBusiness _productionStockDetailBusiness;
        private readonly TransferStockToAssemblyInfoRepository _transferStockToAssemblyInfoRepository;
        private readonly ITransferStockToAssemblyDetailBusiness _transferStockToAssemblyDetailBusiness;
        private readonly IAssemblyLineStockDetailBusiness _assemblyLineStockDetailBusiness;
        private readonly IRepairLineStockDetailBusiness _repairLineStockDetailBusiness;
        private readonly IQRCodeTraceBusiness _qRCodeTraceBusiness;

        public TransferStockToAssemblyInfoBusiness(IProductionUnitOfWork productionDb, IItemBusiness itemBusiness, IProductionStockDetailBusiness productionStockDetailBusiness, ITransferStockToAssemblyDetailBusiness transferStockToAssemblyDetailBusiness, IAssemblyLineStockDetailBusiness assemblyLineStockDetailBusiness, IQRCodeTraceBusiness qRCodeTraceBusiness, IRepairLineStockDetailBusiness repairLineStockDetailBusiness)
        {
            this._productionDb = productionDb;
            this._itemBusiness = itemBusiness;
            this._productionStockDetailBusiness = productionStockDetailBusiness;
            this._transferStockToAssemblyInfoRepository = new TransferStockToAssemblyInfoRepository(this._productionDb);
            this._transferStockToAssemblyDetailBusiness = transferStockToAssemblyDetailBusiness;
            this._assemblyLineStockDetailBusiness = assemblyLineStockDetailBusiness;
            this._qRCodeTraceBusiness = qRCodeTraceBusiness;
            this._repairLineStockDetailBusiness = repairLineStockDetailBusiness;
        }

        public IEnumerable<TransferStockToAssemblyInfoDTO> GetFloorStockTransferInfobyQuery(long? floorId, string transferFor, long? repairLineId, long? assemblyId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string status, string transferCode, string fromDate, string toDate, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<TransferStockToAssemblyInfoDTO>(QueryForFloorStockTransferInfo(floorId,transferFor,repairLineId,assemblyId,modelId,warehouseId,itemTypeId,itemId,status,transferCode,fromDate,toDate,orgId)).ToList();
        }

        private string QueryForFloorStockTransferInfo(long? floorId, string transferFor, long? repairLineId, long? assemblyId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string status, string transferCode, string fromDate, string toDate, long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;
            if (floorId != null && floorId > 0)
            {
                param += string.Format(@" and ti.LineId={0}", floorId);
            }
            if(!string.IsNullOrEmpty(transferFor) && transferFor.Trim() != "")
            {
                param += string.Format(@" and ti.TransferFor='{0}'", transferFor);
            }
            if (modelId != null && modelId > 0)
            {
                param += string.Format(@" and ti.DescriptionId={0}", modelId);
            }
            if (assemblyId != null && assemblyId > 0)
            {
                param += string.Format(@" and ti.AssemblyId={0}", assemblyId);
            }
            if (repairLineId != null && repairLineId > 0)
            {
                param += string.Format(@" and ti.RepairLineId={0}", repairLineId);
            }
            if (warehouseId != null && warehouseId > 0)
            {
                param += string.Format(@" and ti.WarehouseId={0}", warehouseId);
            }
            if (itemTypeId != null && itemTypeId > 0)
            {
                param += string.Format(@" and ti.ItemTypeId={0}", itemTypeId);
            }
            if (itemId != null && itemId > 0)
            {
                param += string.Format(@" and ti.ItemId={0}", itemId);
            }
            if (!string.IsNullOrEmpty(status) && status != "")
            {
                param += string.Format(@" and ti.StateStatus='{0}'", status);
            }
            if (!string.IsNullOrEmpty(transferCode) && transferCode != "")
            {
                param += string.Format(@" and ti.TransferCode Like '%{0}%'", transferCode);
            }
            if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "" && !string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(ti.EntryDate as date) between '{0}' and '{1}'", fDate, tDate);
            }
            else if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(ti.EntryDate as date)='{0}'", fDate);
            }
            else if (!string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(ti.EntryDate as date)='{0}'", tDate);
            }

            query = string.Format(@"Select ti.TSAInfoId,ti.TransferCode,ti.LineId,pl.LineNumber 'LineNumber',ISNULL(ti.RepairLineId,0) 'RepairLineId',ISNULL(rl.RepairLineName,'') 'RepairLineName',
ISNULL(ti.AssemblyId,0) 'AssemblyId',ISNULL(al.AssemblyLineName,'') 'AssemblyName',ti.DescriptionId,de.DescriptionName 'ModelName',ti.WarehouseId,w.WarehouseName,ti.ItemTypeId,it.ItemName 'ItemTypeName',ti.ItemId,i.ItemName,ti.StateStatus,ti.TransferFor,
app.UserName 'EntryUser',ti.EntryDate,(Select UserName from [ControlPanel].dbo.tblApplicationUsers where UserId = ti.UpUserId) 'UpdateUser',ti.ForQty,(Select COUNT(*)  From [Production].dbo.tblTransferStockToAssemblyDetail Where TSAInfoId=ti.TSAInfoId) 'ItemCount',
ti.UpdateDate From [Production].dbo.tblTransferStockToAssemblyInfo ti
Inner Join [Production].dbo.tblProductionLines pl on ti.LineId = pl.LineId
Left Join [Production].dbo.tblRepairLine rl on ti.RepairLineId = rl.RepairLineId
Left Join [Production].dbo.tblAssemblyLines al on ti.AssemblyId = al.AssemblyLineId
Inner Join [Inventory].dbo.tblDescriptions de on ti.DescriptionId = de.DescriptionId
Inner Join [Inventory].dbo.tblWarehouses w on ti.WarehouseId = w.Id
Inner Join [Inventory].dbo.tblItemTypes it on ti.ItemTypeId = it.ItemId
Inner Join [ControlPanel].dbo.tblApplicationUsers app on ti.EUserId = app.UserId
Inner Join [Inventory].dbo.tblItems i on ti.ItemId= i.ItemId Where 1=1 {0}", Utility.ParamChecker(param));

            return query;
        }
        public TransferStockToAssemblyInfo GetStockToAssemblyInfoById(long id, long orgId)
        {
            return _transferStockToAssemblyInfoRepository.GetOneByOrg(t => t.TSAInfoId == id && t.OrganizationId == orgId);
        }

        public IEnumerable<TransferStockToAssemblyInfo> GetStockToAssemblyInfos(long orgId)
        {
            return _transferStockToAssemblyInfoRepository.GetAll(t => t.OrganizationId == orgId).ToList();
        }

        public bool SaveTransferInfoStateStatus(long transferId, string status, long userId, long orgId)
        {
            var info = GetStockToAssemblyInfoById(transferId, orgId);
            if(info != null && info.StateStatus == RequisitionStatus.Approved)
            {
                info.StateStatus = RequisitionStatus.Accepted;
                info.UpUserId = userId;
                info.UpdateDate = DateTime.Now;
                _transferStockToAssemblyInfoRepository.Update(info);
            }
            return _transferStockToAssemblyInfoRepository.Save();
        }

        public bool SaveTransferStockAssembly(TransferStockToAssemblyInfoDTO infoDto, List<TransferStockToAssemblyDetailDTO> detailDto, long userId, long orgId)
        {
            bool IsSuccess = false;
            long? assemblyId = infoDto.TransferFor == "Assembly" ? infoDto.AssemblyId : 0;
            long ? repairLineId = infoDto.TransferFor == "Repair" ? infoDto.RepairLineId : 0;
            TransferStockToAssemblyInfo info = new TransferStockToAssemblyInfo
            {
                LineId = infoDto.LineId,
                AssemblyId = assemblyId,
                RepairLineId = repairLineId,
                DescriptionId = infoDto.DescriptionId,
                WarehouseId = infoDto.WarehouseId,
                OrganizationId = orgId,
                StateStatus = RequisitionStatus.Approved,
                ItemTypeId = infoDto.ItemTypeId,
                ItemId = infoDto.ItemId,
                ForQty = infoDto.ForQty,
                TransferFor = infoDto.TransferFor,
                Remarks = "",
                EUserId = userId,
                EntryDate = DateTime.Now,
                TransferCode = ("TSA-" + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss"))
            };
            List<TransferStockToAssemblyDetail> details = new List<TransferStockToAssemblyDetail>();
            List<ProductionStockDetailDTO> floorStockOutItems = new List<ProductionStockDetailDTO>();
            

            foreach (var item in detailDto)
            {
                TransferStockToAssemblyDetail detail = new TransferStockToAssemblyDetail
                {
                    WarehouseId = item.WarehouseId,
                    ItemTypeId = item.ItemTypeId,
                    ItemId = item.ItemId,
                    UnitId = _itemBusiness.GetItemOneByOrgId(item.ItemId.Value, orgId).UnitId,
                    Quantity = item.Quantity,
                    OrganizationId = orgId,
                    EUserId = userId,
                    EntryDate = DateTime.Now,
                    Remarks = item.Remarks
                };
                details.Add(detail);
                ProductionStockDetailDTO stockOutItem = new ProductionStockDetailDTO
                {
                    LineId = info.LineId,
                    DescriptionId = info.DescriptionId,
                    WarehouseId = item.WarehouseId,
                    ItemTypeId = item.ItemTypeId,
                    ItemId = item.ItemId,
                    UnitId = detail.UnitId,
                    Quantity = item.Quantity,
                    Remarks =StockOutReason.StockOutByTransferToAssembly,
                    StockStatus = StockStatus.StockOut,
                    OrganizationId = orgId,
                    EntryDate = DateTime.Now,
                    EUserId = userId,
                    RefferenceNumber = info.TransferCode,
                    StockFor = ItemPreparationType.Production
                };
                floorStockOutItems.Add(stockOutItem);
            }

            info.TransferStockToAssemblyDetails = details;
            _transferStockToAssemblyInfoRepository.Insert(info);

            if(_transferStockToAssemblyInfoRepository.Save())
            {
                IsSuccess = _productionStockDetailBusiness.SaveProductionStockOut(floorStockOutItems, userId, orgId, StockOutReason.StockOutByTransferToAssembly);
            }
            return IsSuccess;
        }

        public TransferStockToAssemblyInfoDTO GetTransferStockToAssemblyInfoAndDetailsByQuery(long transferId, long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;
            param += string.Format(@" and ti.OrganizationId={0} and ti.TSAInfoId={1}", orgId,transferId);
            TransferStockToAssemblyInfoDTO dto = new TransferStockToAssemblyInfoDTO();

            query = string.Format(@"Select ti.TSAInfoId,ti.TransferCode,ti.LineId,pl.LineNumber 'LineNumber',ISNULL(ti.RepairLineId,0) 'RepairLineId',ISNULL(rl.RepairLineName,'') 'RepairLineName',
ISNULL(ti.AssemblyId,0) 'AssemblyId',ISNULL(al.AssemblyLineName,'') 'AssemblyName',ti.DescriptionId,de.DescriptionName 'ModelName',ti.WarehouseId,w.WarehouseName,ti.ItemTypeId,it.ItemName 'ItemTypeName',ti.ItemId,i.ItemName,ti.StateStatus,ti.TransferFor,
app.UserName 'EntryUser',ti.EntryDate,(Select UserName from [ControlPanel].dbo.tblApplicationUsers where UserId = ti.UpUserId) 'UpdateUser',ti.ForQty,(Select COUNT(*)  From [Production].dbo.tblTransferStockToAssemblyDetail Where TSAInfoId=ti.TSAInfoId) 'ItemCount',
ti.UpdateDate From [Production].dbo.tblTransferStockToAssemblyInfo ti
Inner Join [Production].dbo.tblProductionLines pl on ti.LineId = pl.LineId
Left Join [Production].dbo.tblRepairLine rl on ti.RepairLineId = rl.RepairLineId
Left Join [Production].dbo.tblAssemblyLines al on ti.AssemblyId = al.AssemblyLineId
Inner Join [Inventory].dbo.tblDescriptions de on ti.DescriptionId = de.DescriptionId
Inner Join [Inventory].dbo.tblWarehouses w on ti.WarehouseId = w.Id
Inner Join [Inventory].dbo.tblItemTypes it on ti.ItemTypeId = it.ItemId
Inner Join [ControlPanel].dbo.tblApplicationUsers app on ti.EUserId = app.UserId
Inner Join [Inventory].dbo.tblItems i on ti.ItemId= i.ItemId 
Where 1=1 {0}", Utility.ParamChecker(param));

            dto = this._productionDb.Db.Database.SqlQuery<TransferStockToAssemblyInfoDTO>(query).FirstOrDefault();

            if(dto != null)
            {
                query = string.Format(@"
Select td.TSADetailId,td.TSAInfoId,td.WarehouseId,w.WarehouseName,td.ItemTypeId,it.ItemName 'ItemTypeName',
i.ItemId,i.ItemName,td.UnitId,u.UnitSymbol 'UnitName',td.Quantity
From tblTransferStockToAssemblyDetail td
Inner Join tblTransferStockToAssemblyInfo ti on td.TSAInfoId = ti.TSAInfoId
Inner Join [Inventory].dbo.tblWarehouses w on td.WarehouseId = w.Id
Inner Join [Inventory].dbo.tblItemTypes it on td.ItemTypeId = it.ItemId
Inner Join [Inventory].dbo.tblItems i on td.ItemId= i.ItemId 
Inner Join [Inventory].dbo.tblUnits u on td.UnitId= u.UnitId Where 1= 1 {0}", Utility.ParamChecker(param));

                dto.TransferStockToAssemblyDetails = this._productionDb.Db.Database.SqlQuery<TransferStockToAssemblyDetailDTO>(query).ToList();
            }

            return dto;
        }

        //================== //
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
                    WarehouseName = reqDto.WarehouseName,
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

        public bool SaveProductionStockReceiveInByAssemblyOrRepair(long transferId, string status, long orgId, long userId)
        {
            bool IsSuccess = false;
            if (transferId > 0)
            {
                var info = GetStockToAssemblyInfoById(transferId, orgId);
                if (info != null && info.StateStatus == RequisitionStatus.Approved)
                {
                    var details = _transferStockToAssemblyDetailBusiness.GetTransferStockToAssemblyDetailByInfo(transferId, orgId);
                    if (details.Count() > 0)
                    {
                        List<AssemblyLineStockDetailDTO> assemblyStockDetails = new List<AssemblyLineStockDetailDTO>();// Assembly
                        List<QRCodeTraceDTO> qRCodes = new List<QRCodeTraceDTO>();// Assembly
                        List<RepairLineStockDetailDTO> repairStockDetails = new List<RepairLineStockDetailDTO>();// Repair
                        // Assembly
                        if (info.TransferFor == "Assembly")
                        {
                            qRCodes = GenerateQRCodeTraces(info.TSAInfoId, userId, orgId);
                            foreach (var item in details)
                            {
                                AssemblyLineStockDetailDTO detailItem = new AssemblyLineStockDetailDTO()
                                {
                                    AssemblyLineId = info.AssemblyId,
                                    ProductionLineId = info.LineId,
                                    DescriptionId = info.DescriptionId,
                                    WarehouseId = item.WarehouseId,
                                    ItemTypeId = item.ItemTypeId,
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
                                assemblyStockDetails.Add(detailItem);
                            }
                        }
                        // Repair
                        else
                        {
                            foreach (var item in details)
                            {
                                RepairLineStockDetailDTO detailItem = new RepairLineStockDetailDTO()
                                {
                                    RepairLineId = info.RepairLineId,
                                    ProductionLineId = info.LineId,
                                    DescriptionId = info.DescriptionId,
                                    WarehouseId = item.WarehouseId,
                                    ItemTypeId = item.ItemTypeId,
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
                                repairStockDetails.Add(detailItem);
                            }
                        }
                        if (SaveTransferInfoStateStatus(transferId, status, userId, orgId))
                        {
                            if (info.TransferFor == "Assembly")
                            {
                                if (_assemblyLineStockDetailBusiness.SaveAssemblyLineStockIn(assemblyStockDetails, userId, orgId))
                                {
                                    IsSuccess = _qRCodeTraceBusiness.SaveQRCodeTrace(qRCodes, userId, orgId);
                                };
                            }
                            else
                            {
                                IsSuccess = _repairLineStockDetailBusiness.SaveRepairLineStockIn(repairStockDetails, userId, orgId);
                            }
                        }
                    }
                    // details
                }// info
            }
            return IsSuccess;
        }
    }
}
