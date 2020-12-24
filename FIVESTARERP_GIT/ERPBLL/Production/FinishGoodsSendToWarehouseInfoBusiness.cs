using ERPBLL.Common;
using ERPBLL.Inventory.Interface;
using ERPBLL.Production.Interface;
using ERPBO.Inventory.DTOModel;
using ERPBO.Inventory.DTOModels;
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
    public class FinishGoodsSendToWarehouseInfoBusiness : IFinishGoodsSendToWarehouseInfoBusiness
    {
        // Business cls
        private readonly IProductionUnitOfWork _productionDb; // database
        private readonly TempQRCodeTraceBusiness _tempQRCodeTraceBusiness;
        private readonly QRCodeTraceBusiness _qRCodeTraceBusiness;
        private readonly IFinishGoodsStockDetailBusiness _finishGoodsStockDetailBusiness;
        private readonly IItemBusiness _itemBusiness;
        private readonly IFinishGoodsSendToWarehouseDetailBusiness _finishGoodsSendToWarehouseDetailBusiness;
        private readonly IWarehouseStockDetailBusiness _warehouseStockDetailBusiness;
        private readonly IHandSetStockBusiness _handSetStockBusiness;
        // Repository //
        public readonly FinishGoodsSendToWarehouseInfoRepository _finishGoodsSendToWarehouseInfoRepository;
        public FinishGoodsSendToWarehouseInfoBusiness(IProductionUnitOfWork productionDb, IFinishGoodsStockDetailBusiness finishGoodsStockDetailBusiness, IItemBusiness itemBusiness, IFinishGoodsSendToWarehouseDetailBusiness finishGoodsSendToWarehouseDetailBusiness, IWarehouseStockDetailBusiness warehouseStockDetailBusiness,TempQRCodeTraceBusiness tempQRCodeTraceBusiness, QRCodeTraceBusiness qRCodeTraceBusiness, IHandSetStockBusiness handSetStockBusiness)
        {
            this._productionDb = productionDb;
            this._finishGoodsSendToWarehouseInfoRepository = new FinishGoodsSendToWarehouseInfoRepository(this._productionDb);
            this._finishGoodsStockDetailBusiness = finishGoodsStockDetailBusiness;
            this._itemBusiness = itemBusiness;
            this._finishGoodsSendToWarehouseDetailBusiness = finishGoodsSendToWarehouseDetailBusiness;
            this._warehouseStockDetailBusiness = warehouseStockDetailBusiness;
            this._tempQRCodeTraceBusiness = tempQRCodeTraceBusiness;
            this._qRCodeTraceBusiness = qRCodeTraceBusiness;
            this._handSetStockBusiness = handSetStockBusiness;
        }

        public IEnumerable<FinishGoodsSendToWarehouseInfo> GetFinishGoodsSendToWarehouseList(long orgId)
        {
            return _finishGoodsSendToWarehouseInfoRepository.GetAll(f => f.OrganizationId == orgId).ToList();
        }

        public bool SaveFinishGoodsSendToWarehouse(FinishGoodsSendToWarehouseInfoDTO info, List<FinishGoodsSendToWarehouseDetailDTO> detail, long userId, long orgId)
        {
            bool IsSuccess = false;
            FinishGoodsSendToWarehouseInfo domainInfo = new FinishGoodsSendToWarehouseInfo
            {
                LineId = info.LineId,
                WarehouseId = info.WarehouseId,
                DescriptionId = info.DescriptionId,
                PackagingLineId = info.PackagingLineId,
                Flag = ProductionTypes.SKD,
                Remarks = "",
                EUserId = userId,
                OrganizationId = orgId,
                EntryDate = DateTime.Now,
                StateStatus = FinishGoodsSendStatus.Send,
                RefferenceNumber = ("STW-" + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss"))
            };
            List<FinishGoodsSendToWarehouseDetail> domainDetails = new List<FinishGoodsSendToWarehouseDetail>();
            List<FinishGoodsStockDetailDTO> stockDetails = new List<FinishGoodsStockDetailDTO>();
            foreach (var item in detail)
            {
                FinishGoodsSendToWarehouseDetail dtl = new FinishGoodsSendToWarehouseDetail
                {
                    ItemTypeId = item.ItemTypeId,
                    ItemId = item.ItemId,
                    Quantity = item.Quantity,
                    UnitId = _itemBusiness.GetItemOneByOrgId(item.ItemId, orgId).UnitId,
                    EUserId = userId,
                    OrganizationId = orgId,
                    EntryDate = DateTime.Now,
                    Remarks = domainInfo.Flag,
                    Flag = domainInfo.Flag,
                    RefferenceNumber = domainInfo.RefferenceNumber
                };
                FinishGoodsStockDetailDTO stock = new FinishGoodsStockDetailDTO()
                {
                    LineId = info.LineId,
                    PackagingLineId = info.PackagingLineId,
                    WarehouseId = info.WarehouseId,
                    DescriptionId = info.DescriptionId,
                    ItemTypeId = item.ItemTypeId,
                    ItemId = item.ItemId,
                    Quantity = item.Quantity,
                    UnitId = dtl.UnitId,
                    EUserId = userId,
                    OrganizationId = orgId,
                    EntryDate = DateTime.Now,
                    Remarks = domainInfo.Flag,
                    RefferenceNumber = domainInfo.RefferenceNumber
                };
                domainDetails.Add(dtl);
                stockDetails.Add(stock);
            }
            domainInfo.FinishGoodsSendToWarehouseDetails = domainDetails;
            _finishGoodsSendToWarehouseInfoRepository.Insert(domainInfo);
            if (_finishGoodsSendToWarehouseInfoRepository.Save() == true)
            {
                IsSuccess = this._finishGoodsStockDetailBusiness.SaveFinishGoodsStockOut(stockDetails, userId, orgId);
            }
            return IsSuccess;
        }

        public FinishGoodsSendToWarehouseInfo GetFinishGoodsSendToWarehouseById(long id, long orgId)
        {
            return _finishGoodsSendToWarehouseInfoRepository.GetOneByOrg(f => f.SendId == id && f.OrganizationId == orgId);
        }

        public bool SaveFinishGoodsStatus(long sendId, long userId, long orgId)
        {
            bool IsSuccess = false;
            var info = GetFinishGoodsSendToWarehouseById(sendId, orgId);
            List<WarehouseStockDetailDTO> detailDTOs = new List<WarehouseStockDetailDTO>();
            List<HandSetStockDTO> handSetStocks = new List<HandSetStockDTO>();
            if (info != null)
            {
                info.StateStatus = FinishGoodsSendStatus.Received;
                info.UpdateDate = DateTime.Now;
                info.UpUserId = userId;
                _finishGoodsSendToWarehouseInfoRepository.Update(info);
                var details = this._finishGoodsSendToWarehouseDetailBusiness.GetFinishGoodsDetailByInfoId(info.SendId, orgId);
                foreach (var item in details)
                {
                    WarehouseStockDetailDTO warehouse = new WarehouseStockDetailDTO()
                    {
                        WarehouseId = info.WarehouseId,
                        DescriptionId = info.DescriptionId,
                        ItemTypeId = item.ItemTypeId,
                        ItemId = item.ItemId,
                        UnitId = item.UnitId,
                        Quantity = item.Quantity,
                        EUserId = userId,
                        OrganizationId = orgId,
                        EntryDate = DateTime.Now,
                        RefferenceNumber = info.RefferenceNumber,
                        Remarks = info.CartoonNo,
                        StockStatus = StockStatus.StockIn
                    };
                    detailDTOs.Add(warehouse);

                    HandSetStockDTO handSetStock = new HandSetStockDTO()
                    {
                        CartoonId = info.SendId,
                        CartoonNo = info.CartoonNo,
                        WarehouseId = info.WarehouseId,
                        ItemTypeId = item.ItemTypeId,
                        ItemId = item.ItemId,
                        CategoryId = 0,
                        BrandId = 0,
                        ModelId = info.DescriptionId,
                        ColorId = 0,
                        IMEI = item.IMEI,
                        AllIMEI = item.AllIMEI,
                        OrganizationId = orgId,
                        BranchId = 0,
                        EntryDate = DateTime.Now,
                        EUserId = userId,
                        Remarks = "Stock In By Finish Goods",
                        StockStatus = StockStatus.StockIn
                    };
                    handSetStocks.Add(handSetStock);
                }
            }
            if (_finishGoodsSendToWarehouseInfoRepository.Save() == true)
            {
                if (_warehouseStockDetailBusiness.SaveWarehouseStockIn(detailDTOs, userId, orgId)) {
                    IsSuccess = _handSetStockBusiness.SaveHandSetItemStockIn(handSetStocks,userId,0,orgId);
                };
            }
            return IsSuccess;
        }

        public async Task<bool> SaveFinishGoodsCartonAsync(FinishGoodsSendToWarehouseInfoDTO dto, long userId, long orgId)
        {
            var qrCodesFromUI = dto.FinishGoodsSendToWarehouseDetails.Select(s => s.QRCode).Distinct().ToList();
            var qrCodeInDb = await _tempQRCodeTraceBusiness.GetTempQRCodeTracesByQRCodesAsync(qrCodesFromUI, orgId);
            qrCodeInDb = qrCodeInDb.Where(s => s.StateStatus == QRCodeStatus.Finish).ToList();
            if (qrCodeInDb.Count() > 0)
            {
                // Finish Goods Send Info //
                FinishGoodsSendToWarehouseInfo info = new FinishGoodsSendToWarehouseInfo()
                {
                    DescriptionId = dto.DescriptionId,
                    WarehouseId = dto.WarehouseId,
                    PackagingLineId = qrCodeInDb.FirstOrDefault().PackagingLineId.Value,
                    LineId = qrCodeInDb.FirstOrDefault().ProductionFloorId.Value,
                    TotalQty = qrCodeInDb.Count(),
                    CartoonNo = dto.CartoonNo,
                    StateStatus = FinishGoodsSendStatus.Send,
                    Width = dto.Width,
                    Height = dto.Height,
                    EntryDate = DateTime.Now,
                    EUserId = userId,
                    NetWeight = dto.NetWeight,
                    OrganizationId = orgId,
                    RefferenceNumber = "FGT-" + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss"),

                };

                // Finish Goods Send Detail //
                List<FinishGoodsSendToWarehouseDetail> details = new List<FinishGoodsSendToWarehouseDetail>();

                // Finish Goods Stock Out //
                List<FinishGoodsStockDetailDTO> finishGoodsStocks = new List<FinishGoodsStockDetailDTO>();
                var allItemInDb = _itemBusiness.GetAllItemByOrgId(orgId).ToList();
                foreach (var item in qrCodeInDb)
                {
                    FinishGoodsStockDetailDTO finishGoodsStock = new FinishGoodsStockDetailDTO()
                    {
                        LineId = item.ProductionFloorId,
                        PackagingLineId = item.PackagingLineId,
                        DescriptionId = item.DescriptionId,
                        WarehouseId = item.WarehouseId,
                        ItemTypeId = item.ItemTypeId,
                        ItemId = item.ItemId,
                        Quantity = 1,
                        StockStatus = StockStatus.StockOut,
                        RefferenceNumber = item.IMEI,
                        Remarks = item.CodeNo,
                        UnitId = allItemInDb.FirstOrDefault(s => s.ItemId == item.ItemId).UnitId,
                        EntryDate = DateTime.Now,
                        EUserId = userId
                    };
                    finishGoodsStocks.Add(finishGoodsStock);

                    var imei = dto.FinishGoodsSendToWarehouseDetails.FirstOrDefault(s => s.QRCode == item.CodeNo);
                    FinishGoodsSendToWarehouseDetail detail = new FinishGoodsSendToWarehouseDetail()
                    {
                        DescriptionId = item.DescriptionId.Value,
                        WarehouseId = item.WarehouseId.Value,
                        ItemTypeId = item.ItemTypeId.Value,
                        ItemId = item.ItemId.Value,
                        QRCode = item.CodeNo,
                        IMEI = imei.IMEI,
                        AllIMEI = item.IMEI,
                        EntryDate = DateTime.Now,
                        EUserId = userId,
                        Quantity = 1,
                        RefferenceNumber = info.RefferenceNumber,
                        OrganizationId = orgId,
                        UnitId = finishGoodsStock.UnitId.Value,
                        Remarks = ""

                    };
                    details.Add(detail);

                    item.StateStatus = QRCodeStatus.Carton;
                    item.CartonNo = dto.CartoonNo;
                }

                info.FinishGoodsSendToWarehouseDetails = details;

                // Temp QRCode Status Update //
                if (await _tempQRCodeTraceBusiness.UpdateQRCodeBatchAsync(qrCodeInDb.ToList(), orgId))
                {
                    // Stock Out
                    if (await _finishGoodsStockDetailBusiness.SaveFinishGoodsStockOutAsync(finishGoodsStocks, userId, orgId))
                    {
                        // Transfer 
                        _finishGoodsSendToWarehouseInfoRepository.Insert(info);
                        return await _finishGoodsSendToWarehouseInfoRepository.SaveAsync();
                    }
                }
            }
            return false;
        }

        public IEnumerable<FinishGoodsSendToWarehouseInfoDTO> GetFinishGoodsSendToWarehouseInfosByQuery(long? floorId, long? packagingLineId, long? warehouseId, long? modelId, string status, string transferCode, string fromDate, string toDate, long? transferId, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<FinishGoodsSendToWarehouseInfoDTO>(QueryForFinishGoodsSendToWarehouseInfo(floorId, packagingLineId, warehouseId, modelId, status, transferCode,fromDate,toDate, transferId, orgId)).ToList();
        }

        private string QueryForFinishGoodsSendToWarehouseInfo(long? floorId, long? packagingLineId, long? warehouseId, long? modelId, string status, string transferCode, string fromDate, string toDate, long? transferId, long orgId)
        {
            string param = string.Empty;
            string query = string.Empty;
            if(floorId != null && floorId > 0)
            {
                param += string.Format(@" and fgs.LineId={0}",floorId);
            }
            if (packagingLineId != null && packagingLineId > 0)
            {
                param += string.Format(@" and fgs.PackagingLineId={0}", packagingLineId);
            }
            if (warehouseId != null && warehouseId > 0)
            {
                param += string.Format(@" and fgs.WarehouseId={0}", warehouseId);
            }
            if (modelId != null && modelId > 0)
            {
                param += string.Format(@" and fgs.DescriptionId={0}", modelId);
            }
            if (transferId != null && transferId > 0)
            {
                param += string.Format(@" and fgs.SendId={0}", transferId);
            }
            if (!string.IsNullOrEmpty(status) && status.Trim() !="") 
            {
                param += string.Format(@" and fgs.StateStatus='{0}'", status);
            }
            if (!string.IsNullOrEmpty(transferCode) && transferCode.Trim() != "")
            {
                param += string.Format(@" and fgs.RefferenceNumber LIKE'%{0}%'", floorId);
            }
            if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "" && !string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(fgs.EntryDate as date) between '{0}' and '{1}'", fDate.Trim(), tDate.Trim());
            }
            else if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(fgs.EntryDate as date)='{0}'", fDate.Trim());
            }
            else if (!string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(fgs.EntryDate as date)='{0}'", tDate.Trim());
            }
            query = string.Format(@"Select fgs.SendId,fgs.RefferenceNumber,fgs.LineId,pl.LineNumber,fgs.PackagingLineId,pac.PackagingLineName,fgs.WarehouseId,
w.WarehouseName,fgs.DescriptionId,de.DescriptionName 'ModelName',fgs.StateStatus,fgs.CartoonNo,fgs.NetWeight,fgs.TotalQty 
From [Production].dbo.tblFinishGoodsSendToWarehouseInfo fgs
Inner Join [Production].dbo.tblProductionLines pl on fgs.LineId = pl.LineId
Inner Join [Production].dbo.tblPackagingLine pac on fgs.PackagingLineId = pac.PackagingLineId
Inner Join [Inventory].dbo.tblWarehouses w on fgs.WarehouseId = w.Id
Inner Join [Inventory].dbo.tblDescriptions de on fgs.DescriptionId = de.DescriptionId
Where 1=1 and fgs.TotalQty > 0 and fgs.OrganizationId={0} {1} Order by fgs.SendId desc", orgId,Utility.ParamChecker(param));
            return query;
        }

        public IEnumerable<FinishGoodsSendToWarehouseInfoDTO> GetFinishGoodSendInfomations(long? lineId, long? warehouseId, long? modelId, string status, string fromDate, string toDate, string refNo, long orgId)
        {
            string param = string.Empty;
            string query = string.Empty;
            if(lineId != null && lineId.Value > 0)
            {
                param += string.Format(@" and fsi.LineId={0}",lineId);
            }
            if (warehouseId != null && warehouseId.Value > 0)
            {
                param += string.Format(@" and fsi.WarehouseId={0}", warehouseId);
            }
            if (modelId != null && modelId.Value > 0)
            {
                param += string.Format(@" and fsi.DescriptionId={0}", modelId);
            }
            if (!string.IsNullOrEmpty(status) && status.Trim() !="")
            {
                param += string.Format(@" and fsi.StateStatus='{0}'", status.Trim());
            }
            if (!string.IsNullOrEmpty(refNo) && refNo.Trim() != "")
            {
                param += string.Format(@" and CartoonNo LIKE '%{0}%'", refNo.Trim());
            }
            if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "" && !string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(fsi.EntryDate as date) between '{0}' and '{1}'", fDate, tDate);
            }
            else if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(fsi.EntryDate as date)='{0}'", fDate);
            }
            else if (!string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(fsi.EntryDate as date)='{0}'", tDate);
            }

            query = string.Format(@"Select fsi.SendId,fsi.LineId,pl.LineNumber,fsi.WarehouseId,w.WarehouseName,fsi.DescriptionId,de.DescriptionName 'ModelName',fsi.Remarks,fsi.Flag,fsi.StateStatus,fsi.EUserId,fsi.EntryDate,app.UserName 'EntryUser',fsi.UpUserId,fsi.UpdateDate,
(Select UserName From [ControlPanel].dbo.tblApplicationUsers Where UserId=ISNULL(fsi.UpUserId,0)) 'UpdateUser',fsi.CartoonNo,fsi.Width,fsi.Height,fsi.GrossWeight,fsi.NetWeight,(Select Count(*) From tblFinishGoodsSendToWarehouseDetail Where SendId=fsi.SendId and OrganizationId={0}) as 'TotalQty'
From tblFinishGoodsSendToWarehouseInfo fsi
Inner Join [ControlPanel].dbo.tblApplicationUsers app on fsi.EUserId= app.UserId
Inner Join tblProductionLines pl  on fsi.LineId =pl.LineId
Inner Join tblPackagingLine pac on fsi.PackagingLineId =pac.PackagingLineId
Inner Join [Inventory].dbo.tblWarehouses w on fsi.WarehouseId =w.Id
Inner Join [Inventory].dbo.tblDescriptions de on fsi.DescriptionId =de.DescriptionId
Where 1=1 and fsi.OrganizationId={0} {1}", orgId,Utility.ParamChecker(param));
            return _productionDb.Db.Database.SqlQuery<FinishGoodsSendToWarehouseInfoDTO>(query).ToList();
        }
    }
}
