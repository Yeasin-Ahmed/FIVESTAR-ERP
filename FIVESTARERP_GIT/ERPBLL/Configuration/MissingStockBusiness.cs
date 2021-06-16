using ERPBLL.Common;
using ERPBLL.Configuration.Interface;
using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using ERPDAL.ConfigurationDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration
{
    public class MissingStockBusiness : IMissingStockBusiness
    {
        private readonly IConfigurationUnitOfWork _configDb;
        private readonly MissingStockRepository _missingStockRepository;
        private readonly IMobilePartStockInfoBusiness _mobilePartStockInfoBusiness;
        private readonly MobilePartStockDetailRepository _mobilePartStockDetailRepository; // repo
        private readonly MobilePartStockInfoRepository _mobilePartStockInfoRepository; //repo
        private readonly IFaultyStockInfoBusiness _faultyStockInfoBusiness;
        private readonly FaultyStockInfoRepository _faultyStockInfoRepository;
        private readonly FaultyStockDetailRepository _faultyStockDetailRepository;
        private readonly HandSetStockRepository _handSetStockRepository;
        private readonly IHandSetStockBusiness _handSetStockBusiness;
        public MissingStockBusiness(IConfigurationUnitOfWork configDb, IMobilePartStockInfoBusiness mobilePartStockInfoBusiness, IFaultyStockInfoBusiness faultyStockInfoBusiness, IHandSetStockBusiness handSetStockBusiness)
        {
            this._configDb = configDb;
            _missingStockRepository = new MissingStockRepository(this._configDb);
            this._mobilePartStockInfoBusiness = mobilePartStockInfoBusiness;
            this._mobilePartStockDetailRepository = new MobilePartStockDetailRepository(this._configDb);
            this._mobilePartStockInfoRepository = new MobilePartStockInfoRepository(this._configDb);
            this._faultyStockInfoBusiness = faultyStockInfoBusiness;
            _faultyStockInfoRepository = new FaultyStockInfoRepository(this._configDb);
            _faultyStockDetailRepository = new FaultyStockDetailRepository(this._configDb);
            _handSetStockRepository = new HandSetStockRepository(this._configDb);
            this._handSetStockBusiness = handSetStockBusiness;
        }

        public MissingStock GetMissingStockOneById(long id, long orgId)
        {
            return _missingStockRepository.GetOneByOrg(s => s.OrganizationId == orgId && s.MissingStockId == id);
        }
        public MissingStock GetMissingStockOneByModelAndColorAndPartsAndStockType(long model,long color, long parts, string type, long orgId)
        {
            return _missingStockRepository.GetOneByOrg(s => s.DescriptionId == model && s.ColorId == color && s.PartsId == parts && s.OrganizationId == orgId);
        }
        public bool SaveMissingStock(MissingStockDTO dto, long userId, long branchId, long orgId)
        {
            bool isSuccess = false;
            if (dto.MissingStockId > 0)
            {
                var stockIsExists = GetMissingStockOneByModelAndColorAndPartsAndStockType(dto.DescriptionId, dto.ColorId, dto.PartsId, dto.StockType, orgId);
                if (stockIsExists!=null)
                {
                    return false;
                }
            }
            var stockInfo = GetMissingStockOneByModelAndColorAndPartsAndStockType(dto.DescriptionId, dto.ColorId, dto.PartsId, dto.StockType, orgId);
            if (stockInfo != null)
            {
                stockInfo.Quantity += dto.Quantity;
                stockInfo.UpdateDate = DateTime.Now;
                stockInfo.UpUserId = userId;
                _missingStockRepository.Update(stockInfo);
            }
            else
            {
                if (dto.MissingStockId == 0)
                {
                    MissingStock stock = new MissingStock()
                    {
                        BranchId = branchId,
                        ColorId = dto.ColorId,
                        DescriptionId = dto.DescriptionId,
                        EntryDate = DateTime.Now,
                        EUserId = userId,
                        OrganizationId = orgId,
                        PartsId = dto.PartsId,
                        Quantity = dto.Quantity,
                        StockType = dto.StockType,
                        IMEI = dto.IMEI,
                        Remarks = "",
                    };
                    _missingStockRepository.Insert(stock);
                }
                else
                {
                    var missingStock = GetMissingStockOneById(dto.MissingStockId, orgId);
                    missingStock.PartsId = dto.PartsId;
                    missingStock.Quantity = dto.Quantity;
                    missingStock.Remarks = dto.Remarks;
                    missingStock.StockType = dto.StockType;
                    missingStock.IMEI = dto.IMEI;
                    missingStock.UpdateDate = DateTime.Now;
                    missingStock.UpUserId = userId;
                    missingStock.DescriptionId = dto.DescriptionId;
                    missingStock.ColorId = dto.ColorId;
                    _missingStockRepository.Update(missingStock);
                }
            }
            if(_missingStockRepository.Save() == true)
            {
                isSuccess = StockOutByMissingStock(dto, orgId, branchId, userId);
            }
            return isSuccess;
        }

        public IEnumerable<MissingStockDTO> GetMissingStockInfoByQuery(long? modelId, long? colorId, long? partsId, string stockType,long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;

            if (modelId != null && modelId.Value > 0)
            {
                param += string.Format(@" and stock.DescriptionId={0}", modelId);
            }
            if (colorId != null && colorId.Value > 0)
            {
                param += string.Format(@" and stock.ColorId={0}", colorId);
            }
            if (partsId != null && partsId.Value > 0)
            {
                param += string.Format(@" and stock.PartsId={0}", partsId);
            }
            if (!string.IsNullOrEmpty(stockType) && stockType.Trim() != "")
            {
                param += string.Format(@" and stock.StockType='{0}'", stockType.Trim());
            }

            query = string.Format(@"Select stock.MissingStockId,stock.ColorId,co.ColorName,stock.Quantity,
stock.StockType,stock.Remarks,stock.OrganizationId,stock.DescriptionId,de.ModelName,mp.MobilePartName 'PartsName', stock.PartsId, stock.IMEI 
From tblMissingStock stock
Left Join [Configuration].dbo.tblColorSS co  on stock.ColorId = co.ColorId and stock.OrganizationId =co.OrganizationId
Left Join [Configuration].dbo.tblModelSS de  on stock.DescriptionId = de.ModelId and stock.OrganizationId =de.OrganizationId
Left Join [Configuration].dbo.tblMobileParts mp  on stock.PartsId = mp.MobilePartId and stock.OrganizationId =mp.OrganizationId
Where 1=1 and stock.OrganizationId={0} {1}", orgId, Utility.ParamChecker(param));

            return _configDb.Db.Database.SqlQuery<MissingStockDTO>(query).ToList();
        }

        public bool StockOutByMissingStock(MissingStockDTO dto, long orgId, long branchId, long userId)
        {
            bool isSuccess = false;
            FaultyStockDetails faultyStockDetails = new FaultyStockDetails();
            HandSetStock handSetStocks = new HandSetStock();
            MobilePartStockDetail stockDetails = new MobilePartStockDetail();
            if (dto.StockType == "Good")
            {
                var reqQty = dto.Quantity;
                var partsInStock = _mobilePartStockInfoBusiness.GetAllMobilePartStockInfoByOrgId(orgId, branchId).Where(i => i.MobilePartId == dto.PartsId && i.DescriptionId == dto.DescriptionId && (i.StockInQty - i.StockOutQty) > 0).OrderBy(i => i.MobilePartStockInfoId).ToList();

                if (partsInStock.Count() > 0)
                {
                    int remainQty = reqQty;
                    foreach (var stock in partsInStock)
                    {

                        var totalStockqty = (stock.StockInQty - stock.StockOutQty); // total stock
                        var stockOutQty = 0;
                        if (totalStockqty <= remainQty)
                        {
                            stock.StockOutQty += totalStockqty;
                            stockOutQty = totalStockqty.Value;
                            remainQty -= totalStockqty.Value;
                        }
                        else
                        {
                            stockOutQty = remainQty;
                            stock.StockOutQty += remainQty;
                            remainQty = 0;
                        }

                        MobilePartStockDetail stockDetail = new MobilePartStockDetail()
                        {
                            SWarehouseId = stock.SWarehouseId,
                            MobilePartId = dto.PartsId,
                            CostPrice = stock.CostPrice,
                            SellPrice = stock.SellPrice,
                            Quantity = stockOutQty,
                            Remarks = dto.Remarks,
                            OrganizationId = orgId,
                            BranchId = branchId,
                            EUserId = userId,
                            EntryDate = DateTime.Now,
                            StockStatus = StockStatus.StockOut,
                            ReferrenceNumber = dto.StockType,
                            DescriptionId = stock.DescriptionId,

                        };
                        _mobilePartStockInfoRepository.Update(stock);
                        _mobilePartStockDetailRepository.Insert(stockDetail);

                        if (remainQty == 0)
                        {
                            break;
                        }
                    }
                }
                isSuccess = _mobilePartStockDetailRepository.Save();
            }
            if (dto.StockType == "Faulty")
            {
                var reqQty = dto.Quantity;
                var partsInStock = _faultyStockInfoBusiness.GetAllFaultyStockInfoByOrgId(orgId, branchId).Where(i => i.PartsId == dto.PartsId && i.DescriptionId == dto.DescriptionId && (i.StockInQty - i.StockOutQty) > 0).OrderBy(i => i.FaultyStockInfoId).ToList();

                if (partsInStock.Count() > 0)
                {
                    int remainQty = reqQty;
                    foreach (var stock in partsInStock)
                    {

                        var totalStockqty = (stock.StockInQty - stock.StockOutQty); // total stock
                        var stockOutQty = 0;
                        if (totalStockqty <= remainQty)
                        {
                            stock.StockOutQty += totalStockqty;
                            stockOutQty = totalStockqty;
                            remainQty -= totalStockqty;
                        }
                        else
                        {
                            stockOutQty = remainQty;
                            stock.StockOutQty += remainQty;
                            remainQty = 0;
                        }

                        FaultyStockDetails stockDetail = new FaultyStockDetails()
                        {
                            SWarehouseId = stock.SWarehouseId,
                            PartsId = dto.PartsId,
                            CostPrice = stock.CostPrice,
                            SellPrice = stock.SellPrice,
                            Quantity = stockOutQty,
                            Remarks = dto.Remarks,
                            OrganizationId = orgId,
                            BranchId = branchId,
                            EUserId = userId,
                            EntryDate = DateTime.Now,
                            StateStatus = StockStatus.StockOut,
                            DescriptionId = stock.DescriptionId,
                            JobOrderId = stock.JobOrderId,
                            FaultyStockInfoId = stock.FaultyStockInfoId,
                        };
                        _faultyStockInfoRepository.Update(stock);
                        _faultyStockDetailRepository.Insert(stockDetail);

                        if (remainQty == 0)
                        {
                            break;
                        }
                    }
                }
                isSuccess = _faultyStockDetailRepository.Save();
            }
            if (dto.StockType == "Handset")
            {
                var reqQty = dto.Quantity;
                var imeiInStock = _handSetStockBusiness.GetIMEI2ByIMEI1(dto.IMEI,branchId,orgId);

                if (imeiInStock.IMEI1 != null)
                {
                    imeiInStock.StateStatus = StockStatus.StockOut;
                    imeiInStock.Flag = "MissingStock";
                    imeiInStock.UpdateDate = DateTime.Now;
                    imeiInStock.UpUserId = userId;

                    _handSetStockRepository.Update(imeiInStock);
                }
                isSuccess = _handSetStockRepository.Save();
            }
            return isSuccess;
        }

        public bool UpdateMissingStock(MissingStockDTO dto, long userId, long branchId, long orgId)
        {
            bool isSuccess = false;
            //var stockInfo = GetMissingStockOneByModelAndColorAndPartsAndStockType(dto.DescriptionId, dto.ColorId, dto.PartsId, dto.StockType, orgId);
            if (dto.StockType == "Handset")
            {
                var handset = GetMissingByHandsetStock(dto.DescriptionId, dto.IMEI, dto.ColorId, dto.StockType, orgId, branchId);
                if (handset != null)
                {
                    handset.IMEI = "Stock Return";
                    handset.Quantity -= dto.Quantity;
                    handset.UpdateDate = DateTime.Now;
                    handset.UpUserId = userId;
                    _missingStockRepository.Update(handset);
                }
            }
            else
            {
                var othStock = GetMissingGoodOrFaultyStock(dto.DescriptionId, dto.PartsId, dto.StockType, orgId, branchId);
                if(othStock != null)
                {
                    othStock.Quantity -= dto.Quantity;
                    othStock.UpdateDate = DateTime.Now;
                    othStock.UpUserId = userId;
                    _missingStockRepository.Update(othStock);
                }
            }
            if (_missingStockRepository.Save() == true)
            {
                isSuccess = StockInByMissingStock(dto, orgId, branchId, userId);
            }
            return isSuccess;
        }
        public bool StockInByMissingStock(MissingStockDTO dto, long orgId, long branchId, long userId)
        {
            bool isSuccess = false;
            FaultyStockDetails faultyStockDetails = new FaultyStockDetails();
            HandSetStock handSetStocks = new HandSetStock();
            MobilePartStockDetail stockDetails = new MobilePartStockDetail();
            if (dto.StockType == "Good")
            {
                var reqQty = dto.Quantity;
                var partsInStock = _mobilePartStockInfoBusiness.GetAllMobilePartStockInfoByOrgId(orgId, branchId).Where(i => i.MobilePartId == dto.PartsId && i.DescriptionId == dto.DescriptionId && (i.StockInQty - i.StockOutQty) > 0).OrderBy(i => i.MobilePartStockInfoId).ToList();

                if (partsInStock.Count() > 0)
                {
                    int remainQty = reqQty;
                    foreach (var stock in partsInStock)
                    {

                        if(stock != null)
                        {
                            stock.StockOutQty -= remainQty;
                            stock.UpdateDate = DateTime.Now;
                            stock.UpUserId = userId;
                        }
                        MobilePartStockDetail stockDetail = new MobilePartStockDetail()
                        {
                            SWarehouseId = stock.SWarehouseId,
                            MobilePartId = dto.PartsId,
                            CostPrice = stock.CostPrice,
                            SellPrice = stock.SellPrice,
                            Quantity = dto.Quantity,
                            Remarks = "Stock-In By Missing",
                            OrganizationId = orgId,
                            BranchId = branchId,
                            EUserId = userId,
                            EntryDate = DateTime.Now,
                            StockStatus = StockStatus.StockIn,
                            ReferrenceNumber = dto.StockType,
                            DescriptionId = stock.DescriptionId,

                        };
                        _mobilePartStockInfoRepository.Update(stock);
                        _mobilePartStockDetailRepository.Insert(stockDetail);

                        if (remainQty == 0)
                        {
                            break;
                        }
                    }
                }
                isSuccess = _mobilePartStockDetailRepository.Save();
            }
            if (dto.StockType == "Faulty")
            {
                var reqQty = dto.Quantity;
                var partsInStock = _faultyStockInfoBusiness.GetAllFaultyStockInfoByOrgId(orgId, branchId).Where(i => i.PartsId == dto.PartsId && i.DescriptionId == dto.DescriptionId && (i.StockInQty - i.StockOutQty) > 0).OrderBy(i => i.FaultyStockInfoId).ToList();

                if (partsInStock.Count() > 0)
                {
                    int remainQty = reqQty;
                    foreach (var stock in partsInStock)
                    {

                        if (stock != null)
                        {
                            stock.StockOutQty -= remainQty;
                            stock.UpdateDate = DateTime.Now;
                            stock.UpUserId = userId;
                        }

                        FaultyStockDetails stockDetail = new FaultyStockDetails()
                        {
                            SWarehouseId = stock.SWarehouseId,
                            PartsId = dto.PartsId,
                            CostPrice = stock.CostPrice,
                            SellPrice = stock.SellPrice,
                            Quantity = dto.Quantity,
                            Remarks = dto.Remarks,
                            OrganizationId = orgId,
                            BranchId = branchId,
                            EUserId = userId,
                            EntryDate = DateTime.Now,
                            StateStatus = StockStatus.StockIn,
                            DescriptionId = stock.DescriptionId,
                            JobOrderId = stock.JobOrderId,
                            FaultyStockInfoId = stock.FaultyStockInfoId,
                        };
                        _faultyStockInfoRepository.Update(stock);
                        _faultyStockDetailRepository.Insert(stockDetail);

                        if (remainQty == 0)
                        {
                            break;
                        }
                    }
                }
                isSuccess = _faultyStockDetailRepository.Save();
            }
            if (dto.StockType == "Handset")
            {
                var reqQty = dto.Quantity;
                var imeiInStock = _handSetStockBusiness.GetIMEI2ByIMEI1(dto.IMEI, branchId, orgId);

                if (imeiInStock.IMEI1 != null)
                {
                    imeiInStock.StateStatus = StockStatus.StockIn;
                    imeiInStock.Flag = "MissingStockReturn";
                    imeiInStock.UpdateDate = DateTime.Now;
                    imeiInStock.UpUserId = userId;
                    _handSetStockRepository.Update(imeiInStock);
                }
                isSuccess = _handSetStockRepository.Save();
            }
            return isSuccess;
        }

        public MissingStock GetMissingGoodOrFaultyStock(long model, long parts, string type, long orgId, long branchId)
        {
            return _missingStockRepository.GetOneByOrg(s => s.DescriptionId == model  && s.PartsId == parts && s.StockType==type && s.OrganizationId == orgId && s.BranchId==branchId);
        }

        public MissingStock GetMissingByHandsetStock(long model, string imei, long colorId, string type, long orgId, long branchId)
        {
            return _missingStockRepository.GetOneByOrg(s => s.DescriptionId == model && s.IMEI==imei && s.ColorId == colorId && s.StockType == type && s.OrganizationId == orgId && s.BranchId==branchId);
        }
    }
}
