using ERPBLL.Common;
using ERPBLL.Configuration.Interface;
using ERPBLL.FrontDesk.Interface;
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
   public class MobilePartStockDetailBusiness: IMobilePartStockDetailBusiness
    {
        private readonly IConfigurationUnitOfWork _configurationDb; // database
        private readonly MobilePartStockDetailRepository mobilePartStockDetailRepository; // repo
        private readonly MobilePartStockInfoRepository mobilePartStockInfoRepository; //repo
        private readonly IServicesWarehouseBusiness _servicesWarehouseBusiness;
        private readonly IMobilePartBusiness _mobilePartBusiness;
        private readonly IMobilePartStockInfoBusiness _mobilePartStockInfoBusiness;
        private readonly IRequsitionInfoForJobOrderBusiness _requsitionInfoForJobOrderBusiness;
        private readonly IRequsitionDetailForJobOrderBusiness _requsitionDetailForJobOrderBusiness;
        public MobilePartStockDetailBusiness(IConfigurationUnitOfWork configurationDb,IServicesWarehouseBusiness servicesWarehouseBusiness,IMobilePartBusiness mobilePartBusiness,IMobilePartStockInfoBusiness mobilePartStockInfoBusiness, IRequsitionInfoForJobOrderBusiness requsitionInfoForJobOrderBusiness, IRequsitionDetailForJobOrderBusiness requsitionDetailForJobOrderBusiness)
        {
            this._configurationDb = configurationDb;
            mobilePartStockDetailRepository = new MobilePartStockDetailRepository(this._configurationDb);
            mobilePartStockInfoRepository = new MobilePartStockInfoRepository(this._configurationDb);
            this._servicesWarehouseBusiness = servicesWarehouseBusiness;
            this._mobilePartBusiness = mobilePartBusiness;
            this._mobilePartStockInfoBusiness = mobilePartStockInfoBusiness;
            this._requsitionInfoForJobOrderBusiness = requsitionInfoForJobOrderBusiness;
            this._requsitionDetailForJobOrderBusiness = requsitionDetailForJobOrderBusiness;
        }

        public IEnumerable<MobilePartStockDetail> GelAllMobilePartStockDetailByOrgId(long orgId, long branchId)
        {
            return mobilePartStockDetailRepository.GetAll(detail => detail.OrganizationId == orgId && detail.BranchId== branchId).ToList();
        }

        public bool SaveMobilePartsStockOutByTSRequistion(long reqId, string status, long orgId, long userId, long branchId)
        {
            var reqInfo = _requsitionInfoForJobOrderBusiness.GetAllRequsitionInfoForJobOrderId(reqId, orgId);
            var reqDetail = _requsitionDetailForJobOrderBusiness.GetAllRequsitionDetailForJobOrderId(reqId, orgId,branchId);
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
                        Quantity = (int)item.Quantity,
                        EUserId = userId,
                        EntryDate = DateTime.Now,
                        BranchId=branchId,
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
            foreach(var item in mobilePartStockDetailDTO)
            {
                MobilePartStockDetail StockDetail = new MobilePartStockDetail();
                StockDetail.MobilePartStockDetailId = item.MobilePartStockDetailId;
                StockDetail.MobilePartId = item.MobilePartId;
                StockDetail.SWarehouseId = item.SWarehouseId;
                StockDetail.Quantity = item.Quantity;
                StockDetail.Remarks = item.Remarks;
                StockDetail.OrganizationId = orgId;
                StockDetail.BranchId = branchId;
                StockDetail.EUserId = userId;
                StockDetail.EntryDate = DateTime.Now;
                StockDetail.StockStatus = StockStatus.StockIn;
                StockDetail.BranchFrom = item.BranchFrom;
                StockDetail.ReferrenceNumber = item.ReferrenceNumber;

                var warehouseInfo = _mobilePartStockInfoBusiness.GetAllMobilePartStockInfoByOrgId(orgId,branchId).Where(o => o.MobilePartId == item.MobilePartId ).FirstOrDefault();
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
                StockDetail.Quantity = item.Quantity;
                StockDetail.Remarks = item.Remarks;
                StockDetail.OrganizationId = orgId;
                StockDetail.BranchId = branchId;
                StockDetail.EUserId = userId;
                StockDetail.EntryDate = DateTime.Now;
                StockDetail.StockStatus = StockStatus.StockOut;
                StockDetail.ReferrenceNumber = item.ReferrenceNumber;

                var warehouseInfo = _mobilePartStockInfoBusiness.GetAllMobilePartStockInfoByOrgId(orgId, branchId).Where(o => item.SWarehouseId == item.SWarehouseId && o.MobilePartId == item.MobilePartId).FirstOrDefault();
                warehouseInfo.StockOutQty += item.Quantity;
                warehouseInfo.UpUserId = userId;
                mobilePartStockInfoRepository.Update(warehouseInfo);
                mobilePartStockDetails.Add(StockDetail);

            }
            mobilePartStockDetailRepository.InsertAll(mobilePartStockDetails);
            return mobilePartStockDetailRepository.Save();
        }

        public bool StockInByBranchTransferApproval(long transferId, string status, long userId, long branchId, long orgId)
        {
            bool IsSuccess = false;
            
            return IsSuccess;

        }
    }
}
