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
   public class TransferInfoBusiness: ITransferInfoBusiness
    {
        private readonly IConfigurationUnitOfWork _configurationDb; // database
        private readonly TransferInfoRepository transferInfoRepository; // repo
        private readonly IMobilePartStockDetailBusiness _mobilePartStockDetailBusiness;
        public TransferInfoBusiness(IConfigurationUnitOfWork configurationDb,IMobilePartStockDetailBusiness mobilePartStockDetailBusiness)
        {
            this._mobilePartStockDetailBusiness = mobilePartStockDetailBusiness;
            this._configurationDb = configurationDb;
            transferInfoRepository = new TransferInfoRepository(this._configurationDb);
        }

        public IEnumerable<TransferInfo> GetAllStockTransferByOrgId(long orgId, long branchId)
        {
            return transferInfoRepository.GetAll(ts => ts.OrganizationId == orgId && ts.BranchId==branchId).ToList();
        }

        public TransferInfo GetStockTransferInfoById(long id, long orgId, long branchId)
        {
            return transferInfoRepository.GetOneByOrg(t => t.TransferInfoId == id && t.OrganizationId == orgId && t.BranchId==branchId);
        }

        public bool SaveTransferInfoStateStatus(long transferId, string status, long userId, long orgId, long branchId)
        {
            var info = GetStockTransferInfoById(transferId, orgId,branchId);
            if (info != null && info.StateStatus == RequisitionStatus.Approved)
            {
                info.StateStatus = RequisitionStatus.Accepted;
                info.UpUserId = userId;
                info.UpdateDate = DateTime.Now;
                transferInfoRepository.Update(info);
            }
            return transferInfoRepository.Save();
        }

        public bool SaveTransferStockInfo(TransferInfoDTO info, List<TransferDetailDTO> detailDTO, long userId, long orgId, long branchId)
        {
            bool IsSuccess = false;
            TransferInfo transferInfo = new TransferInfo
            {
                TransferInfoId = info.TransferInfoId,
                BranchTo=info.BranchTo,
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
                    Quantity = item.Quantity,
                    Remarks = item.Remarks,
                    OrganizationId = orgId,
                    EUserId = userId,
                    EntryDate = DateTime.Now,
                };
                details.Add(detail);
                MobilePartStockDetailDTO stockOutItem = new MobilePartStockDetailDTO
                {
                    SWarehouseId = info.SWarehouseId,
                    MobilePartId = item.PartsId,
                    Quantity = item.Quantity,
                    Remarks = transferInfo.TransferCode,
                    StockStatus = StockStatus.StockOut,
                    OrganizationId = orgId,
                    EntryDate = DateTime.Now,
                    EUserId = userId,
                    BranchId = branchId
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
