using ERPBLL.Common;
using ERPBLL.Configuration.Interface;
using ERPBLL.FrontDesk.Interface;
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
    public class MobilePartStockDetailBusiness : IMobilePartStockDetailBusiness
    {
        private readonly IConfigurationUnitOfWork _configurationDb; // database
        private readonly MobilePartStockDetailRepository mobilePartStockDetailRepository; // repo
        private readonly MobilePartStockInfoRepository mobilePartStockInfoRepository; //repo
        private readonly IServicesWarehouseBusiness _servicesWarehouseBusiness;
        private readonly IMobilePartBusiness _mobilePartBusiness;
        private readonly IMobilePartStockInfoBusiness _mobilePartStockInfoBusiness;
        private readonly IRequsitionInfoForJobOrderBusiness _requsitionInfoForJobOrderBusiness;
        private readonly IRequsitionDetailForJobOrderBusiness _requsitionDetailForJobOrderBusiness;
        private readonly ITechnicalServicesStockBusiness _technicalServicesStockBusiness;
        private readonly ITsStockReturnInfoBusiness _tsStockReturnInfoBusiness;
        public MobilePartStockDetailBusiness(IConfigurationUnitOfWork configurationDb, IServicesWarehouseBusiness servicesWarehouseBusiness, IMobilePartBusiness mobilePartBusiness, IMobilePartStockInfoBusiness mobilePartStockInfoBusiness, IRequsitionInfoForJobOrderBusiness requsitionInfoForJobOrderBusiness, IRequsitionDetailForJobOrderBusiness requsitionDetailForJobOrderBusiness, ITechnicalServicesStockBusiness technicalServicesStockBusiness, ITsStockReturnInfoBusiness tsStockReturnInfoBusiness)
        {
            this._configurationDb = configurationDb;
            mobilePartStockDetailRepository = new MobilePartStockDetailRepository(this._configurationDb);
            mobilePartStockInfoRepository = new MobilePartStockInfoRepository(this._configurationDb);
            this._servicesWarehouseBusiness = servicesWarehouseBusiness;
            this._mobilePartBusiness = mobilePartBusiness;
            this._mobilePartStockInfoBusiness = mobilePartStockInfoBusiness;
            this._requsitionInfoForJobOrderBusiness = requsitionInfoForJobOrderBusiness;
            this._requsitionDetailForJobOrderBusiness = requsitionDetailForJobOrderBusiness;
            this._technicalServicesStockBusiness = technicalServicesStockBusiness;
            this._tsStockReturnInfoBusiness = tsStockReturnInfoBusiness;

        }

        public IEnumerable<MobilePartStockDetail> GelAllMobilePartStockDetailByOrgId(long orgId, long branchId)
        {
            return mobilePartStockDetailRepository.GetAll(detail => detail.OrganizationId == orgId && detail.BranchId == branchId).ToList();
        }

        public bool SaveMobilePartsStockOutByTSRequistion(long reqId, string status, long orgId, long userId, long branchId)
        {
            var reqInfo = _requsitionInfoForJobOrderBusiness.GetAllRequsitionInfoForJobOrderId(reqId, orgId);
            var reqDetail = _requsitionDetailForJobOrderBusiness.GetAllRequsitionDetailForJobOrderId(reqId, orgId, branchId);
            if (reqInfo != null && reqDetail.Count() > 0)
            {
                List<MobilePartStockDetailDTO> stockDetailDTOs = new List<MobilePartStockDetailDTO>();
                foreach (var item in reqDetail)
                {
                    MobilePartStockDetailDTO stockDetailDTO = new MobilePartStockDetailDTO
                    {
                        SWarehouseId = reqInfo.SWarehouseId,
                        MobilePartId = item.PartsId,
                        OrganizationId = item.OrganizationId,
                        CostPrice = item.CostPrice,
                        SellPrice = item.SellPrice,
                        Quantity = (int)item.Quantity,
                        EUserId = userId,
                        EntryDate = DateTime.Now,
                        BranchId = branchId,
                        Remarks = "Stock Out By Production Requistion " + "(" + reqInfo.RequsitionCode + ")",
                        ReferrenceNumber = reqInfo.RequsitionCode,
                        StockStatus = StockStatus.StockOut
                    };
                    stockDetailDTOs.Add(stockDetailDTO);
                }
                if (SaveMobilePartStockOut(stockDetailDTOs, userId, orgId, branchId) == true)
                {
                    return _requsitionInfoForJobOrderBusiness.SaveRequisitionStatus(reqId, status, userId, orgId, branchId);
                }
            }
            return false;
        }

        public bool SaveMobilePartStockIn(List<MobilePartStockDetailDTO> mobilePartStockDetailDTO, long userId, long orgId, long branchId)
        {
            List<MobilePartStockDetail> mobilePartStockDetails = new List<MobilePartStockDetail>();
            foreach (var item in mobilePartStockDetailDTO)
            {
                MobilePartStockDetail StockDetail = new MobilePartStockDetail();
                StockDetail.MobilePartStockDetailId = item.MobilePartStockDetailId;
                StockDetail.MobilePartId = item.MobilePartId;
                StockDetail.SWarehouseId = item.SWarehouseId;
                StockDetail.DescriptionId = item.DescriptionId;
                StockDetail.CostPrice = item.CostPrice;
                StockDetail.SellPrice = item.SellPrice;
                StockDetail.Quantity = item.Quantity;
                StockDetail.Remarks = item.Remarks;
                StockDetail.OrganizationId = orgId;
                StockDetail.BranchId = branchId;
                StockDetail.EUserId = userId;
                StockDetail.EntryDate = DateTime.Now;
                StockDetail.StockStatus = StockStatus.StockIn;
                StockDetail.BranchFrom = item.BranchFrom;
                StockDetail.ReferrenceNumber = item.ReferrenceNumber;

                var warehouseInfo = _mobilePartStockInfoBusiness.GetMobilePartStockInfoByModelAndMobilePartsAndCostPrice(item.DescriptionId.Value,item.MobilePartId.Value, item.CostPrice, orgId, branchId);
                
                if (warehouseInfo != null)
                {
                    warehouseInfo.StockInQty += item.Quantity;
                    warehouseInfo.UpUserId = userId;
                    mobilePartStockInfoRepository.Update(warehouseInfo);
                }
                else
                {
                    MobilePartStockInfo mobilePartStockInfo = new MobilePartStockInfo();
                    mobilePartStockInfo.SWarehouseId = item.SWarehouseId;
                    mobilePartStockInfo.MobilePartId = item.MobilePartId;
                    mobilePartStockInfo.DescriptionId = item.DescriptionId;
                    mobilePartStockInfo.CostPrice = item.CostPrice;
                    mobilePartStockInfo.SellPrice = item.SellPrice;
                    mobilePartStockInfo.StockInQty = item.Quantity;
                    mobilePartStockInfo.StockOutQty = 0;
                    mobilePartStockInfo.OrganizationId = orgId;
                    mobilePartStockInfo.BranchId = branchId;
                    mobilePartStockInfo.EUserId = userId;
                    mobilePartStockInfo.EntryDate = DateTime.Now;
                    mobilePartStockInfoRepository.Insert(mobilePartStockInfo);
                }
                mobilePartStockDetails.Add(StockDetail);
            }
            mobilePartStockDetailRepository.InsertAll(mobilePartStockDetails);
            return mobilePartStockDetailRepository.Save();
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

                var warehouseInfo = _mobilePartStockInfoBusiness.GetMobilePartStockInfoByModelAndMobilePartsAndCostPrice(item.DescriptionId.Value,item.MobilePartId.Value,item.CostPrice,orgId,branchId);  //_mobilePartStockInfoBusiness.GetAllMobilePartStockInfoById(orgId, branchId).Where(o => item.SWarehouseId == item.SWarehouseId && o.MobilePartId == item.MobilePartId && o.CostPrice == item.CostPrice).FirstOrDefault();
                warehouseInfo.StockOutQty += item.Quantity;
                warehouseInfo.UpUserId = userId;
                mobilePartStockInfoRepository.Update(warehouseInfo);
                mobilePartStockDetails.Add(StockDetail);

            }
            mobilePartStockDetailRepository.InsertAll(mobilePartStockDetails);
            return mobilePartStockDetailRepository.Save();
        }

        public bool SaveMobilePartStockOutByReq(long reqId,string status, long orgId, long branchId, long userId)
        {
            bool IsSuccess = false;
            var reqInfo = _requsitionInfoForJobOrderBusiness.GetAllRequsitionInfoForJobOrderId(reqId, orgId);
            var reqDetails = _requsitionDetailForJobOrderBusiness.GetAllRequsitionDetailForJobOrderId(reqId, orgId, reqInfo.BranchId.Value);
            List<MobilePartStockDetail> stockDetails = new List<MobilePartStockDetail>();
            List<TechnicalServicesStockDTO> servicesStockDTOs = new List<TechnicalServicesStockDTO>();

            foreach (var item in reqDetails)
            {
                var reqQty = item.Quantity;
                var partsInStock = _mobilePartStockInfoBusiness.GetAllMobilePartStockInfoByOrgId(orgId, reqInfo.BranchId.Value).Where(i => i.MobilePartId == item.PartsId && (i.StockInQty - i.StockOutQty) > 0).OrderBy(i => i.MobilePartStockInfoId).ToList();

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
                            SWarehouseId = item.SWarehouseId,
                            MobilePartId = item.PartsId,
                            CostPrice = stock.CostPrice,
                            SellPrice = stock.SellPrice,
                            Quantity = stockOutQty,
                            Remarks = item.Remarks,
                            OrganizationId = orgId,
                            BranchId = reqInfo.BranchId.Value,
                            EUserId = userId,
                            EntryDate = DateTime.Now,
                            StockStatus = StockStatus.StockOut,
                            ReferrenceNumber = reqInfo.RequsitionCode
                        };
                        TechnicalServicesStockDTO tsStock = new TechnicalServicesStockDTO()
                        {
                            JobOrderId = item.JobOrderId,
                            SWarehouseId = item.SWarehouseId,
                            RequsitionInfoForJobOrderId = item.RequsitionInfoForJobOrderId,
                            PartsId = item.PartsId,
                            CostPrice = stock.CostPrice,
                            SellPrice = stock.SellPrice,
                            Quantity = stockOutQty,
                            StateStatus = "Stock-Open",
                            UsedQty = 0,
                            ReturnQty = 0,
                            Remarks = item.Remarks,
                            OrganizationId = orgId,
                            BranchId = branchId,
                            EUserId = userId,
                            EntryDate = DateTime.Now,
                        };
                        servicesStockDTOs.Add(tsStock);
                        stockDetails.Add(stockDetail);
                        mobilePartStockInfoRepository.Update(stock);
                        if (remainQty == 0)
                        {
                            break;
                        }
                    }
                }
            }
            mobilePartStockDetailRepository.InsertAll(stockDetails);
            if (mobilePartStockDetailRepository.Save())
            {
                IsSuccess = _technicalServicesStockBusiness.SaveTechnicalServicesStockIn(servicesStockDTOs, userId, orgId, branchId);
                if (IsSuccess == true)
                {
                    return _requsitionInfoForJobOrderBusiness.SaveRequisitionStatus(reqId, status, userId, orgId, reqInfo.BranchId.Value);
                }
            }
            return IsSuccess;
        }

        //public bool StockOutAccessoriesSells(long invoiceId, long orgId, long branchId, long userId)
        //{
        //    bool IsSuccess = false;
        //    var invInfo = _invoiceInfoBusiness.GetAllInvoiceByOrgId(invoiceId, orgId, branchId);
        //    var invDetail = _invoiceDetailBusiness.GetAllDetailByInfoId(invoiceId, orgId, branchId);
        //    List<MobilePartStockDetail> stockDetails = new List<MobilePartStockDetail>();
        //    foreach (var item in invDetail)
        //    {
        //        var reqQty = item.Quantity;
        //        var partsInStock = _mobilePartStockInfoBusiness.GetAllMobilePartStockInfoByOrgId(orgId, branchId).Where(i => i.MobilePartId == item.PartsId && (i.StockInQty - i.StockOutQty) > 0).OrderBy(i => i.MobilePartStockInfoId).ToList();

        //        if (partsInStock.Count() > 0)
        //        {
        //            int remainQty = reqQty;
        //            foreach (var stock in partsInStock)
        //            {

        //                var totalStockqty = (stock.StockInQty - stock.StockOutQty); // total stock
        //                var stockOutQty = 0;
        //                if (totalStockqty <= remainQty)
        //                {
        //                    stock.StockOutQty += totalStockqty;
        //                    stockOutQty = totalStockqty.Value;
        //                    remainQty -= totalStockqty.Value;
        //                }
        //                else
        //                {
        //                    stockOutQty = remainQty;
        //                    stock.StockOutQty += remainQty;
        //                    remainQty = 0;
        //                }


        //                MobilePartStockDetail stockDetail = new MobilePartStockDetail()
        //                {
        //                    MobilePartId = item.PartsId,
        //                    CostPrice = stock.CostPrice,
        //                    SellPrice = stock.SellPrice,
        //                    Quantity = stockOutQty,
        //                    Remarks = item.Remarks,
        //                    OrganizationId = orgId,
        //                    BranchId = branchId,
        //                    EUserId = userId,
        //                    EntryDate = DateTime.Now,
        //                    StockStatus = StockStatus.StockOut,
        //                    ReferrenceNumber = invInfo.InvoiceCode
        //                };
        //                stockDetails.Add(stockDetail);
        //                mobilePartStockInfoRepository.Update(stock);
        //                if (remainQty == 0)
        //                {
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //    mobilePartStockDetailRepository.InsertAll(stockDetails);
        //    mobilePartStockDetailRepository.Save();
        //    return IsSuccess;
        //}

        public bool SaveReturnPartsStockIn(List<MobilePartStockDetailDTO> mobilePartStockDetailDTO,long returnInfoId,string status, long userId, long orgId, long branchId)
        {
            List<MobilePartStockDetail> mobilePartStockDetails = new List<MobilePartStockDetail>();
            foreach (var item in mobilePartStockDetailDTO)
            {
                var warehouseInfo = _mobilePartStockInfoBusiness.GetAllMobilePartStockInfoByOrgId(orgId, branchId).Where(o => o.MobilePartId == item.MobilePartId).LastOrDefault();
                warehouseInfo.StockInQty += item.Quantity;
                warehouseInfo.UpUserId = userId;
                warehouseInfo.UpdateDate = DateTime.Now;
                mobilePartStockInfoRepository.Update(warehouseInfo);

                MobilePartStockDetail StockDetail = new MobilePartStockDetail();
                StockDetail.MobilePartId = item.MobilePartId;
                StockDetail.SWarehouseId = warehouseInfo.SWarehouseId;
                StockDetail.CostPrice = warehouseInfo.CostPrice;
                StockDetail.SellPrice = warehouseInfo.SellPrice;
                StockDetail.Quantity = item.Quantity;
                StockDetail.Remarks = item.Remarks;
                StockDetail.OrganizationId = orgId;
                StockDetail.BranchId = branchId;
                StockDetail.EUserId = userId;
                StockDetail.EntryDate = DateTime.Now;
                StockDetail.StockStatus = StockStatus.StockIn;
                StockDetail.ReferrenceNumber = item.ReferrenceNumber;
                mobilePartStockDetails.Add(StockDetail);
            }
            mobilePartStockDetailRepository.InsertAll(mobilePartStockDetails);
            if (mobilePartStockDetailRepository.Save() == true)
            {
                return _tsStockReturnInfoBusiness.UpdateReturnInfoStatus(returnInfoId, status, userId,orgId,branchId);
            }
            return false;
        }

        public bool StockInByBranchTransferApproval(long transferId, string status, long userId, long branchId, long orgId)
        {
            bool IsSuccess = false;

            return IsSuccess;

        }
    }
}
