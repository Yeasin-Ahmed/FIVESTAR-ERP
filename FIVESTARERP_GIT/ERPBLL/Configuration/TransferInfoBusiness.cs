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
    public class TransferInfoBusiness : ITransferInfoBusiness
    {
        private readonly IConfigurationUnitOfWork _configurationDb; // database
        private readonly TransferInfoRepository transferInfoRepository; // repo
        private readonly IMobilePartStockDetailBusiness _mobilePartStockDetailBusiness;

        private readonly ITransferDetailBusiness _transferDetailBusiness;
        public TransferInfoBusiness(IConfigurationUnitOfWork configurationDb, IMobilePartStockDetailBusiness mobilePartStockDetailBusiness, ITransferDetailBusiness transferDetailBusiness)
        {
            this._mobilePartStockDetailBusiness = mobilePartStockDetailBusiness;
            this._transferDetailBusiness = transferDetailBusiness;
            this._configurationDb = configurationDb;
            transferInfoRepository = new TransferInfoRepository(this._configurationDb);
        }

        public IEnumerable<TransferInfo> GetAllStockTransferByOrgId(long orgId)
        {
            return transferInfoRepository.GetAll(ts => ts.OrganizationId == orgId).ToList();
        }

        public IEnumerable<TransferInfo> GetAllStockTransferByOrgIdAndBranch(long orgId, long branchId)
        {
            return transferInfoRepository.GetAll(ts => ts.OrganizationId == orgId && ts.BranchId == branchId).ToList();
        }

        public TransferInfo GetStockTransferInfoById(long id, long orgId, long branchId)
        {
            return transferInfoRepository.GetOneByOrg(t => t.TransferInfoId == id && t.OrganizationId == orgId && t.BranchId == branchId);
        }

        public TransferInfo GetStockTransferInfoById(long id, long orgId)
        {
            return transferInfoRepository.GetOneByOrg(t => t.TransferInfoId == id && t.OrganizationId == orgId);
        }

        public bool SaveTransferInfoStateStatus(long transferId,long swarehouse, string status, long userId, long orgId, long branchId)
        {
            bool IsSuccess = false;
            var info = GetStockTransferInfoById(transferId, orgId);
            if (info != null && info.StateStatus == RequisitionStatus.Approved)
            {
                info.WarehouseIdTo = swarehouse;
                info.StateStatus = RequisitionStatus.Accepted;
                info.UpUserId = userId;
                info.UpdateDate = DateTime.Now;
                transferInfoRepository.Update(info);
                if (transferInfoRepository.Save())
                {
                    var details = _transferDetailBusiness.GetAllTransferDetailByInfoId(transferId,orgId);
                    if (details.Count() > 0)
                    {
                        List<MobilePartStockDetailDTO> stockDetails = new List<MobilePartStockDetailDTO>();
                        foreach (var item in details)
                        {
                            MobilePartStockDetailDTO detailItem = new MobilePartStockDetailDTO()
                            {
                                BranchFrom = info.BranchId,
                                BranchId = info.BranchTo.Value,
                                SWarehouseId = swarehouse,
                                MobilePartId = item.PartsId,
                                StockStatus = StockStatus.StockIn,
                                Quantity = item.Quantity,
                                CostPrice=item.CostPrice,
                                SellPrice=item.SellPrice,
                                EUserId = userId,
                                EntryDate = DateTime.Now,
                                OrganizationId = orgId,
                                ReferrenceNumber = info.TransferCode,
                                Remarks = "Stock In By Another Branch ("+info.TransferCode+")",
                            };
                            stockDetails.Add(detailItem);
                        }
                        IsSuccess = _mobilePartStockDetailBusiness.SaveMobilePartStockIn(stockDetails, userId, orgId, info.BranchTo.Value);
                    }
                }
            }

            return IsSuccess;
        }

        public bool SaveTransferStockInfo(TransferInfoDTO info, List<TransferDetailDTO> detailDTO, long userId, long orgId, long branchId)
        {
            bool IsSuccess = false;
            TransferInfo transferInfo = new TransferInfo
            {
                DescriptionId = info.DescriptionId, //Nishad
                TransferInfoId = info.TransferInfoId,
                BranchTo = info.BranchTo,
                BranchId = branchId,
                WarehouseId = info.SWarehouseId,
                OrganizationId = orgId,
                StateStatus = RequisitionStatus.Approved,
                Remarks = "",
                EUserId = userId,
                EntryDate = DateTime.Now,
                TransferCode = ("TSB-" + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss"))
            };
            List<TransferDetail> details = new List<TransferDetail>();
            List<MobilePartStockDetailDTO> TransferStockOutItems = new List<MobilePartStockDetailDTO>();

            foreach (var item in detailDTO)
            {
                TransferDetail detail = new TransferDetail
                {
                    PartsId = item.PartsId,
                    CostPrice=item.CostPrice,
                    SellPrice=item.SellPrice,
                    Quantity = item.Quantity,
                    Remarks = item.Remarks,
                    BranchTo=info.BranchTo,
                    BranchId= branchId,
                    OrganizationId = orgId,
                    EUserId = userId,
                    EntryDate = DateTime.Now,
                    DescriptionId = info.DescriptionId // Nishad
                };
                details.Add(detail);
                MobilePartStockDetailDTO stockOutItem = new MobilePartStockDetailDTO
                {
                    SWarehouseId = info.SWarehouseId,
                    MobilePartId = item.PartsId,
                    CostPrice=item.CostPrice,
                    SellPrice=item.SellPrice,
                    Quantity = item.Quantity,
                    Remarks = transferInfo.TransferCode,
                    StockStatus = StockStatus.StockOut,
                    OrganizationId = orgId,
                    EntryDate = DateTime.Now,
                    EUserId = userId,
                    BranchId = branchId,
                    ReferrenceNumber= transferInfo.TransferCode,
                    DescriptionId = info.DescriptionId //Nishad
                };
                TransferStockOutItems.Add(stockOutItem);
            }

            transferInfo.TransferDetails = details;
            transferInfoRepository.Insert(transferInfo);

            if (transferInfoRepository.Save())
            {
                IsSuccess = _mobilePartStockDetailBusiness.SaveMobilePartStockOut(TransferStockOutItems, userId, orgId, branchId);
            }
            return IsSuccess;
        }
    }
}
