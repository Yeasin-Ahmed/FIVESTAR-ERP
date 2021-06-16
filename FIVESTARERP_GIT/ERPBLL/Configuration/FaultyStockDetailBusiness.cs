using ERPBLL.Common;
using ERPBLL.Configuration.Interface;
using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using ERPBO.FrontDesk.DTOModels;
using ERPDAL.ConfigurationDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration
{
    public class FaultyStockDetailBusiness : IFaultyStockDetailBusiness
    {
        private readonly IConfigurationUnitOfWork _configurationDb;
        private readonly IFaultyStockInfoBusiness _faultyStockInfoBusiness;
        private readonly FaultyStockDetailRepository _faultyStockDetailRepository;
        private readonly FaultyStockInfoRepository _faultyStockInfoRepository;
        // private readonly IMobilePartStockDetailBusiness _mobilePartStockDetailBusiness;
        private readonly IMobilePartStockInfoBusiness _mobilePartStockInfoBusiness;
        private readonly MobilePartStockDetailRepository mobilePartStockDetailRepository; // repo
        private readonly MobilePartStockInfoRepository mobilePartStockInfoRepository; //repo
        public FaultyStockDetailBusiness(IConfigurationUnitOfWork configurationDb, IFaultyStockInfoBusiness faultyStockInfoBusiness, IMobilePartStockInfoBusiness mobilePartStockInfoBusiness)
        {
            this._configurationDb = configurationDb;
            this._faultyStockInfoBusiness = faultyStockInfoBusiness;
            _faultyStockInfoRepository = new FaultyStockInfoRepository(this._configurationDb);
            _faultyStockDetailRepository = new FaultyStockDetailRepository(this._configurationDb);
            mobilePartStockDetailRepository = new MobilePartStockDetailRepository(this._configurationDb);
            mobilePartStockInfoRepository = new MobilePartStockInfoRepository(this._configurationDb);
            //this._mobilePartStockDetailBusiness = mobilePartStockDetailBusiness;
            this._mobilePartStockInfoBusiness = mobilePartStockInfoBusiness;
        }

        public bool SaveFaultyStock(List<ERPBO.Configuration.DTOModels.FaultyStockDetailDTO> faultyStocksDto, long userId, long orgId, long branchId)
        {
            bool isSuccess = false;
            List<MobilePartStockDetailDTO> stockdto = new List<MobilePartStockDetailDTO>();
            List<FaultyStockDetails> faultyStockDetails = new List<FaultyStockDetails>();
            FaultyStockDetails faultyStock = new FaultyStockDetails();
            FaultyStockInfo faultyInfo = new FaultyStockInfo();
            
            foreach (var item in faultyStocksDto)
            {
                var getPrice = _mobilePartStockInfoBusiness.GetPriceByModel(item.DescriptionId.Value, item.PartsId.Value, orgId, branchId);
                faultyStock = new FaultyStockDetails()
                {
                    BranchId = branchId,
                    CostPrice = getPrice.CostPrice,
                    SellPrice = getPrice.SellPrice,
                    StateStatus = StockStatus.StockIn,
                    SWarehouseId = item.SWarehouseId,
                    EUserId = userId,
                    OrganizationId = orgId,
                    EntryDate = DateTime.Now,
                    JobOrderId = item.JobOrderId,
                    DescriptionId = item.DescriptionId,
                    PartsId = item.PartsId,
                    Quantity = item.Quantity,
                    Remarks = "Faulty By Main Stock",
                    TSId = 0,

                };
                MobilePartStockDetailDTO stock = new MobilePartStockDetailDTO
                {
                    MobilePartId = item.PartsId,
                    SWarehouseId = item.SWarehouseId,
                    DescriptionId = item.DescriptionId,
                    CostPrice = getPrice.CostPrice,
                    SellPrice = getPrice.SellPrice,
                    Quantity = item.Quantity,
                    Remarks = "Stock-Out By Faulty",
                    OrganizationId = orgId,
                    BranchId = branchId,
                    EUserId = userId,
                    EntryDate = DateTime.Now,
                    StockStatus = StockStatus.StockOut,
                };
                stockdto.Add(stock);
                var faultyStockInfo = _faultyStockInfoBusiness.GetAllFaultyStockByStockIn(item.DescriptionId.Value, item.PartsId.Value, orgId, branchId);
                if (faultyStockInfo != null)
                {
                    faultyStockInfo.StockInQty += item.Quantity;
                    faultyStockInfo.UpUserId = userId;
                    faultyStockInfo.UpdateDate = DateTime.Now;
                    _faultyStockInfoRepository.Update(faultyStockInfo);
                    //FaultyStockInfoId//
                    faultyStock.FaultyStockInfoId = faultyStockInfo.FaultyStockInfoId;
                }
                else
                {
                    faultyInfo = new FaultyStockInfo()
                    {
                        BranchId = branchId,
                        CostPrice = getPrice.CostPrice,
                        SellPrice = getPrice.SellPrice,
                        SWarehouseId = item.SWarehouseId,
                        EUserId = userId,
                        OrganizationId = orgId,
                        EntryDate = DateTime.Now,
                        JobOrderId = item.JobOrderId,
                        DescriptionId = item.DescriptionId,
                        PartsId = item.PartsId,
                        StockInQty = item.Quantity,
                        StockOutQty = 0,
                        Remarks = "",
                    };
                    _faultyStockInfoRepository.Insert(faultyInfo);
                    if (_faultyStockInfoRepository.Save())
                    {
                        faultyStock.FaultyStockInfoId = faultyInfo.FaultyStockInfoId;
                    }
                }
                faultyStockDetails.Add(faultyStock);
                
            }
            _faultyStockDetailRepository.InsertAll(faultyStockDetails);

            if (_faultyStockDetailRepository.Save() == true)
            {
                isSuccess = SaveMobilePartStockOut(stockdto, userId, orgId, branchId);
            }
            return isSuccess;
        }

        public bool SaveFaultyStockIn(List<ERPBO.Configuration.DTOModels.FaultyStockDetailDTO> faultyStocksDto, long userId, long orgId, long branchId)
        {
            List<FaultyStockDetails> faultyStockDetails = new List<FaultyStockDetails>();
            FaultyStockDetails faultyStock = new FaultyStockDetails();
            FaultyStockInfo faultyInfo = new FaultyStockInfo();
            foreach (var item in faultyStocksDto)
            {
                faultyStock = new FaultyStockDetails()
                {
                    BranchId = branchId,
                    CostPrice = item.CostPrice,
                    SellPrice = item.SellPrice,
                    StateStatus = StockStatus.StockIn,
                    SWarehouseId = item.SWarehouseId,
                    EUserId = userId,
                    OrganizationId = orgId,
                    EntryDate = DateTime.Now,
                    JobOrderId = item.JobOrderId,
                    DescriptionId = item.DescriptionId,
                    PartsId = item.PartsId,
                    Quantity = item.Quantity,
                    Remarks = "Faulty By TS",
                    TSId = item.TSId,
                    
                };
                var faultyStockInfo = _faultyStockInfoBusiness.GetAllFaultyStockInfoByModelAndPartsIdAndCostPrice(item.DescriptionId.Value,item.PartsId.Value,item.CostPrice,orgId,branchId);
                if (faultyStockInfo != null)
                {
                    faultyStockInfo.StockInQty += item.Quantity;
                    faultyStockInfo.UpUserId = userId;
                    faultyStockInfo.UpdateDate = DateTime.Now;
                   _faultyStockInfoRepository.Update(faultyStockInfo);
                    //FaultyStockInfoId//
                    faultyStock.FaultyStockInfoId = faultyStockInfo.FaultyStockInfoId;
                }
                else
                {
                    faultyInfo = new FaultyStockInfo()
                    {
                        BranchId = branchId,
                        CostPrice = item.CostPrice,
                        SellPrice = item.SellPrice,
                        SWarehouseId = item.SWarehouseId,
                        EUserId = userId,
                        OrganizationId = orgId,
                        EntryDate = DateTime.Now,
                        JobOrderId = item.JobOrderId,
                        DescriptionId = item.DescriptionId,
                        PartsId = item.PartsId,
                        StockInQty = item.Quantity,
                        StockOutQty = 0,
                        Remarks = "",                        
                    };
                    _faultyStockInfoRepository.Insert(faultyInfo);
                    if (_faultyStockInfoRepository.Save())
                    {
                        faultyStock.FaultyStockInfoId = faultyInfo.FaultyStockInfoId;
                    }
                }
                faultyStockDetails.Add(faultyStock);
            }
            
            _faultyStockDetailRepository.InsertAll(faultyStockDetails);
            return _faultyStockDetailRepository.Save();
        }

        public bool SaveMobilePartStockOut(List<MobilePartStockDetailDTO> mobilePartStockDetailDTO, long userId, long orgId, long branchId)
        {
            List<MobilePartStockDetail> mobilePartStockDetails = new List<MobilePartStockDetail>();
            foreach (var item in mobilePartStockDetailDTO)
            {
                MobilePartStockDetail StockDetail = new MobilePartStockDetail();
                StockDetail.MobilePartStockDetailId = item.MobilePartStockDetailId;
                StockDetail.MobilePartId = item.MobilePartId;
                StockDetail.SWarehouseId = item.SWarehouseId;
                StockDetail.CostPrice = item.CostPrice;
                StockDetail.SellPrice = item.SellPrice;
                StockDetail.Quantity = item.Quantity;
                StockDetail.Remarks = item.Remarks;
                StockDetail.OrganizationId = orgId;
                StockDetail.BranchId = branchId;
                StockDetail.EUserId = userId;
                StockDetail.EntryDate = DateTime.Now;
                StockDetail.StockStatus = StockStatus.StockOut;
                StockDetail.ReferrenceNumber = item.ReferrenceNumber;
                StockDetail.DescriptionId = item.DescriptionId; //Nishad

                var warehouse = _mobilePartStockInfoBusiness.GetMobilePartStockInfoByModelAndMobilePartsAndCostPrice(item.DescriptionId.Value, item.MobilePartId.Value, item.CostPrice, orgId, branchId);

                warehouse.StockOutQty += item.Quantity;
                warehouse.UpUserId = userId;
                mobilePartStockInfoRepository.Update(warehouse);
                mobilePartStockDetails.Add(StockDetail);
            }
            mobilePartStockDetailRepository.InsertAll(mobilePartStockDetails);
            return mobilePartStockDetailRepository.Save();
        }
    }
}
