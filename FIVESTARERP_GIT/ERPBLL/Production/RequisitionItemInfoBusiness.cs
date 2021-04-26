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
    public class RequisitionItemInfoBusiness : IRequisitionItemInfoBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly IRequsitionInfoBusiness _requsitionInfoBusiness;
        private readonly IRequsitionDetailBusiness _requsitionDetailBusiness;
        private readonly IRequisitionItemDetailBusiness _requisitionItemDetailBusiness;
        private readonly RequisitionItemInfoRepository _requisitionItemInfoRepository;
        private readonly IQRCodeTraceBusiness _qRCodeTraceBusiness;
        private readonly IAssemblyLineStockDetailBusiness _assemblyLineStockDetailBusiness;
        private readonly IPackagingLineStockDetailBusiness _packagingLineStockDetailBusiness;
        public RequisitionItemInfoBusiness(IProductionUnitOfWork productionDb, RequisitionItemInfoRepository requisitionItemInfoRepository, IRequsitionInfoBusiness requsitionInfoBusiness, IRequsitionDetailBusiness requsitionDetailBusiness, IQRCodeTraceBusiness qRCodeTraceBusiness, IAssemblyLineStockDetailBusiness assemblyLineStockDetailBusiness, IRequisitionItemDetailBusiness requisitionItemDetailBusiness, IPackagingLineStockDetailBusiness packagingLineStockDetailBusiness)
        {
            this._productionDb = productionDb;
            this._requisitionItemInfoRepository = requisitionItemInfoRepository;
            this._requsitionInfoBusiness = requsitionInfoBusiness;
            this._requsitionDetailBusiness = requsitionDetailBusiness;
            this._qRCodeTraceBusiness = qRCodeTraceBusiness;
            this._assemblyLineStockDetailBusiness = assemblyLineStockDetailBusiness;
            this._requisitionItemDetailBusiness = requisitionItemDetailBusiness;
            this._packagingLineStockDetailBusiness = packagingLineStockDetailBusiness;
        }
        public IEnumerable<RequisitionItemInfo> GetRequisitionItemInfos(long orgId)
        {
            return this._requisitionItemInfoRepository.GetAll(s => s.OrganizationId == orgId).ToList();
        }
        public IEnumerable<RequisitionItemInfoDTO> GetRequisitionItemInfosByQuery(long? reqItemIfoId, long? floorId, long? assembly,long? packagingLine, long? repairLine, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, long? reqInfoId, string status, string reqCode, string fromDate, string toDate, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<RequisitionItemInfoDTO>(QueryFortRequisitionItemInfos(reqItemIfoId, floorId, assembly,packagingLine,repairLine, modelId, warehouseId, itemTypeId, itemId, reqInfoId, status, reqCode, fromDate, toDate, orgId)).ToList();
        }
        private string QueryFortRequisitionItemInfos(long? reqItemIfoId, long? floorId, long? assembly, long? packagingLine, long? repairLine, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, long? reqInfoId, string status, string reqCode, string fromDate, string toDate, long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;

            param += string.Format(@" and rii.OrganizationId={0}", orgId);
            if (reqItemIfoId != null && reqItemIfoId > 0)
            {
                param += string.Format(@" and rii.ReqItemInfoId={0}", reqItemIfoId);
            }
            if (reqInfoId != null && reqInfoId > 0)
            {
                param += string.Format(@" and rii.ReqInfoId={0}", reqInfoId);
            }
            if (floorId != null && floorId > 0)
            {
                param += string.Format(@" and rii.ReqInfoId={0}", floorId);
            }
            if (assembly != null && assembly > 0)
            {
                param += string.Format(@" and rii.AssemblyLineId={0}", assembly);
            }
            if (packagingLine != null && packagingLine > 0)
            {
                param += string.Format(@" and rii.PackagingLineId={0}", packagingLine);
            }
            if (repairLine != null && repairLine > 0)
            {
                param += string.Format(@" and rii.RepairLineId={0}", assembly);
            }
            if (modelId != null && modelId > 0)
            {
                param += string.Format(@" and rii.DescriptionId={0}", modelId);
            }
            if (warehouseId != null && warehouseId > 0)
            {
                param += string.Format(@" and rii.WarehouseId={0}", warehouseId);
            }
            if (itemTypeId != null && itemTypeId > 0)
            {
                param += string.Format(@" and rii.ItemTypeId={0}", itemTypeId);
            }
            if (itemId != null && itemId > 0)
            {
                param += string.Format(@" and rii.ItemId={0}", itemId);
            }
            if (!string.IsNullOrEmpty(status) && status.Trim() != "")
            {
                param += string.Format(@" and ri.StateStatus ='{0}'", status);
            }
            if (!string.IsNullOrEmpty(reqCode) && reqCode.Trim() !="")
            {
                param += string.Format(@" and ri.ReqInfoCode LIKE'%{0}%'", reqCode);
            }

            if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "" && !string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(ri.EntryDate as date) between '{0}' and '{1}'", fDate, tDate);
            }
            else if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(ri.EntryDate as date)='{0}'", fDate);
            }
            else if (!string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(ri.EntryDate as date)='{0}'", tDate);
            }

            query = string.Format(@"Select ri.ReqInfoId,rii.ReqItemInfoId,rii.FloorId,pl.LineNumber 'FloorName',rii.AssemblyLineId, al.AssemblyLineName,rii.PackagingLineId, pac.PackagingLineName,
rii.DescriptionId,de.DescriptionName 'ModelName',rii.WarehouseId,w.WarehouseName,
rii.ItemTypeId,it.ItemName 'ItemTypeName',rii.ItemId,i.ItemName,rii.UnitId,un.UnitSymbol 'UnitName',
rii.Quantity,rii.EntryDate,app.UserName 'EntryUser',rii.UpdateDate,(Select UserName from [ControlPanel].dbo.tblApplicationUsers Where UserId = rii.UpUserId) 'UpdateUser'
From [Production].dbo.tblRequisitionItemInfo rii
Inner Join [Production].dbo.tblRequsitionInfo ri on rii.ReqInfoId = ri.ReqInfoId
Inner Join [Production].dbo.tblProductionLines pl on rii.FloorId = pl.LineId
Left Join [Production].dbo.tblAssemblyLines al on rii.AssemblyLineId = al.AssemblyLineId
Left Join [Production].dbo.tblPackagingLine pac on ri.PackagingLineId = pac.PackagingLineId
Inner Join [Inventory].dbo.tblDescriptions de on rii.DescriptionId = de.DescriptionId
Inner Join [Inventory].dbo.tblWarehouses w on rii.WarehouseId = w.Id
Inner Join [Inventory].dbo.tblItemTypes it on rii.ItemTypeId = it.ItemId
Inner Join [Inventory].dbo.tblItems i on rii.ItemId = i.ItemId
Inner Join [Inventory].dbo.tblUnits un on rii.UnitId = un.UnitId
Inner Join [ControlPanel].dbo.tblApplicationUsers app on rii.EUserId = app.UserId
Where 1=1 {0}", Utility.ParamChecker(param));
            return query;
        }
        public IEnumerable<RequisitionItemInfo> GetRequisitionItemInfosByReqInfoId(long reqInfoId, long orgId)
        {
            return this._requisitionItemInfoRepository.GetAll(s => s.OrganizationId == orgId && s.ReqInfoId == reqInfoId).ToList();
        }
        public bool SaveRequisitionItemStocksInAssemblyOrRepairOrPackaging(long reqInfoId, string status, long userId, long orgId)
        {
            var reqInfo = _requsitionInfoBusiness.GetRequisitionById(reqInfoId, orgId);
            if (reqInfo.StateStatus == RequisitionStatus.Approved && status == RequisitionStatus.Accepted)
            {
                var reqDetail = _requsitionDetailBusiness.GetRequsitionDetailByReqId(reqInfoId, orgId).ToList();

                List<AssemblyLineStockDetailDTO> assemblyStocks = new List<AssemblyLineStockDetailDTO>();
                List<PackagingLineStockDetailDTO> packagingStocks = new List<PackagingLineStockDetailDTO>();
                foreach (var detail in reqDetail)
                {
                    if (reqInfo.RequisitionFor == "Assembly")
                    {
                        AssemblyLineStockDetailDTO assemblyStock = new AssemblyLineStockDetailDTO()
                        {
                            ProductionLineId = reqInfo.LineId,
                            AssemblyLineId = reqInfo.AssemblyLineId,
                            DescriptionId = reqInfo.DescriptionId,
                            WarehouseId = reqInfo.WarehouseId,
                            ItemTypeId = detail.ItemTypeId,
                            ItemId = detail.ItemId,
                            Quantity = (int)detail.Quantity.Value,
                            OrganizationId = orgId,
                            EUserId = userId,
                            EntryDate = DateTime.Now,
                            RefferenceNumber = reqInfo.ReqInfoCode,
                            UnitId = detail.UnitId,
                            StockStatus = StockStatus.StockIn,
                            Remarks = "Stock In By Production Requisition"
                        };
                        assemblyStocks.Add(assemblyStock);
                    }
                    else if (reqInfo.RequisitionFor == "Packaging")
                    {
                        PackagingLineStockDetailDTO packagingStock = new PackagingLineStockDetailDTO()
                        {
                            ProductionLineId = reqInfo.LineId,
                            PackagingLineId = reqInfo.PackagingLineId,
                            DescriptionId = reqInfo.DescriptionId,
                            WarehouseId = reqInfo.WarehouseId,
                            ItemTypeId = detail.ItemTypeId,
                            ItemId = detail.ItemId,
                            Quantity = (int)detail.Quantity.Value,
                            OrganizationId = orgId,
                            EUserId = userId,
                            EntryDate = DateTime.Now,
                            RefferenceNumber = reqInfo.ReqInfoCode,
                            UnitId = detail.UnitId,
                            StockStatus = StockStatus.StockIn,
                            Remarks = "Stock In By Production Requisition"
                        };
                        packagingStocks.Add(packagingStock);
                    }
                }
                if (_requsitionInfoBusiness.SaveRequisitionStatus(reqInfo.ReqInfoId, RequisitionStatus.Accepted, orgId, userId))
                {
                    if (reqInfo.RequisitionFor == "Assembly")
                    {
                        if (_assemblyLineStockDetailBusiness.SaveAssemblyLineStockIn(assemblyStocks, userId, orgId))
                        {
                            if (reqInfo.IsBundle)
                            {
                                var qrCodes = GenerateQRCodeTraces(reqInfo.ReqInfoId, userId, orgId);
                                return _qRCodeTraceBusiness.SaveQRCodeTrace(qrCodes, userId, orgId);
                            }
                            else
                            {
                                return true;
                            }
                        };
                    }
                    else if (reqInfo.RequisitionFor == "Packaging")
                    {
                        return _packagingLineStockDetailBusiness.SavePackagingLineStockIn(packagingStocks, userId, orgId);
                    }
                }
            }
            return false;
        }
        private List<QRCodeTraceDTO> GenerateQRCodeTraces(long refNo, long userId, long orgId)
        {
            List<QRCodeTraceDTO> qRCodeTraces = new List<QRCodeTraceDTO>();
            List<RequisitionItemInfoDTO> reqItemDto = new List<RequisitionItemInfoDTO>();
            reqItemDto = this._productionDb.Db.Database.SqlQuery<RequisitionItemInfoDTO>(string.Format(@"Select rii.ReqItemInfoId,ri.ReqInfoId,ri.ReqInfoCode,ri.LineId 'FloorId',pl.LineNumber 'FloorName',ri.AssemblyLineId,al.AssemblyLineName,
ri.DescriptionId,
de.DescriptionName 'ModelName',rii.WarehouseId,w.WarehouseName,
rii.ItemTypeId,it.ItemName 'ItemTypeName',rii.ItemId,i.ItemName,rii.Quantity,(Select SUM(Quantity) From tblRequisitionItemInfo Where ReqInfoId = ri.ReqInfoId) 'TotalItems'
From [Production].dbo.tblRequsitionInfo ri
Inner Join [Production].dbo.tblRequisitionItemInfo rii on ri.ReqInfoId = rii.ReqInfoId
Inner Join [Production].dbo.tblProductionLines pl on ri.LineId = pl.LineId
Inner Join [Production].dbo.tblAssemblyLines al on ri.AssemblyLineId = al.AssemblyLineId and ri.RequisitionFor='Assembly'
Inner Join [Inventory].dbo.tblDescriptions de on ri.DescriptionId = de.DescriptionId
Inner Join [Inventory].dbo.tblItems i on rii.ItemId = i.ItemId
Inner Join [Inventory].dbo.tblItemTypes it on rii.ItemTypeId = it.ItemId
Inner Join [Inventory].dbo.tblWarehouses w on rii.WarehouseId=w.Id
Where 1= 1 and ri.OrganizationId={0} and ri.ReqInfoId = {1}", orgId, refNo)).ToList();

            string tCode = reqItemDto.FirstOrDefault().ReqInfoCode.Substring(4);
            var query = string.Format(@"Select Count(*) From tblTempQRCodeTrace Where AssemblyId={0} and Cast(EntryDate as date) = Cast(GetDate() as date) and OrganizationId={1}", reqItemDto.FirstOrDefault().AssemblyLineId, orgId);

            var itemCount = this._productionDb.Db.Database.SqlQuery<int>(query).FirstOrDefault();

            string prefix = reqItemDto.FirstOrDefault().AssemblyLineId.ToString() + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd");
            int sl = 1;
            for (int i = 0; i < reqItemDto.Count; i++)
            {
                for (int j = 0; j < reqItemDto[i].Quantity; j++)
                {
                    itemCount++;
                    var year = Convert.ToInt32(DateTime.Now.ToString("yy"));
                    var month = Convert.ToInt32(DateTime.Now.ToString("MM"));
                    var day = Convert.ToInt32(DateTime.Now.ToString("dd"));
                    var hour = Convert.ToInt32(DateTime.Now.ToString("hh"));
                    var min = Convert.ToInt32(DateTime.Now.ToString("mm"));
                    var sec = Convert.ToInt32(DateTime.Now.ToString("ss"));
                    var mili = Convert.ToInt32(DateTime.Now.ToString("fffffff"));
                    var val = (year + month + day + hour + min + sec + mili + j + userId).ToString();
                    val = val.PadLeft(11, '0');
                    QRCodeTraceDTO qRCode = new QRCodeTraceDTO
                    {
                        ProductionFloorId = reqItemDto[i].FloorId,
                        DescriptionId = reqItemDto[i].DescriptionId,
                        ItemTypeId = reqItemDto[i].ItemTypeId,
                        ItemId = reqItemDto[i].ItemId,
                        WarehouseId = reqItemDto[i].WarehouseId,
                        CodeId = 0,
                        ColorName = string.Empty,
                        OrganizationId = orgId,
                        EUserId = userId,
                        EntryDate = DateTime.Now,
                        ReferenceId = reqItemDto[i].ReqInfoId.ToString(),
                        ReferenceNumber = reqItemDto[i].ReqInfoCode,
                        ColorId = 0,
                        ModelName = reqItemDto[i].ModelName,
                        WarehouseName = reqItemDto[i].WarehouseName,
                        ItemTypeName = reqItemDto[i].ItemTypeName,
                        ItemName = reqItemDto[i].ItemName,
                        ProductionFloorName = reqItemDto[i].FloorName,
                        CodeNo = 1+val,//(prefix + itemCount.ToString()),//reqItemDto[i].ModelName + "-" + tCode + "-" + sl.ToString(),
                        AssemblyId = reqItemDto[i].AssemblyLineId,
                        AssemblyLineName = reqItemDto[i].AssemblyLineName
                    };
                    qRCodeTraces.Add(qRCode);
                    sl++;
                }
            } 
            return qRCodeTraces;
        }

        public RequsitionInfoDTO GetRequsitionInfoModalProcessData(long? floorId, long? assemblyId, long? packagingLine, long? repairLine, long? warehouseId, long? modelId, string reqCode, string reqType, string reqFor, string fromDate, string toDate, string status, string reqFlag, long? reqInfoId, long orgId)
        {
            var reqInfoDto = _requsitionInfoBusiness.GetRequsitionInfosByQuery(null, null,null,null, null, null, null, null, null, null, null, null, null, reqInfoId, orgId).FirstOrDefault();
            if (reqInfoDto != null)
            {
                // Requisition Details
                reqInfoDto.RequisitionDetails = _requsitionDetailBusiness.GetRequisitionDetailsByQuery(reqInfoId, null, null, null, orgId).ToList();

                // Requisition Item Infos
                reqInfoDto.RequisitionItemInfos = GetRequisitionItemInfosByQuery(null, null,null,null, null, null, null, null, null, reqInfoId, null, null, null, null, orgId).ToList();

                // Requisition Item Details By ReqInfo
                var reqItemDetailDtos = _requisitionItemDetailBusiness.GetRequisitionItemDetailsByQuery(reqInfoId, null, null, null, null, orgId);

                foreach (var rii in reqInfoDto.RequisitionItemInfos)
                {
                    rii.RequisitionItemDetails = reqItemDetailDtos.Where(s => s.ReqItemInfoId == rii.ReqItemInfoId).ToList();
                }
            }
            return reqInfoDto;
        }
    }
}
