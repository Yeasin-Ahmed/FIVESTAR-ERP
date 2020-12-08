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
    public class StockTransferInfoModelToModelBusiness : IStockTransferInfoModelToModelBusiness
    {
        private readonly IConfigurationUnitOfWork _configDb;
        private readonly StockTransferInfoModelToModelRepository _stockTransferInfoModelToModelRepository;
       // private readonly StockTransferDetailModelToModelRepository _stockTransferDetailModelToModelRepository;
        private readonly IMobilePartStockDetailBusiness _mobilePartStockDetailBusiness;
        public StockTransferInfoModelToModelBusiness(IConfigurationUnitOfWork configDb, IMobilePartStockDetailBusiness mobilePartStockDetailBusiness)
        {
            this._configDb = configDb;
            this._mobilePartStockDetailBusiness = mobilePartStockDetailBusiness;
            _stockTransferInfoModelToModelRepository = new StockTransferInfoModelToModelRepository(this._configDb);
            //_stockTransferDetailModelToModelRepository = new StockTransferDetailModelToModelRepository(this._configDb);
        }
        public bool SaveStockTransferModelToModel(StockTransferInfoModelToModelDTO dto, long userId, long branchId, long orgId)
        {
            bool IsSuccess = false;

            List<StockTransferDetailModelToModel> details = new List<StockTransferDetailModelToModel>();
            List<MobilePartStockDetailDTO> partsDetailsForStockOut = new List<MobilePartStockDetailDTO>();
            List<MobilePartStockDetailDTO> partsDetailsForStockIn = new List<MobilePartStockDetailDTO>();

            StockTransferInfoModelToModel info = new StockTransferInfoModelToModel()
            {
                BranchId = branchId,
                DescriptionId = dto.DescriptionId,
                ToDescriptionId = dto.ToDescriptionId,
                EntryDate = DateTime.Now,
                EUserId = userId,
                OrganizationId = orgId,
                Remarks = dto.Remarks,
                StateStatus = RequisitionStatus.Approved,
                TransferCode = ("STM-" + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss")),
                WarehouseId = dto.WarehouseId,
            };
            foreach (var item in dto.StockTransferDetailModelToModels)
            {
                StockTransferDetailModelToModel detail = new StockTransferDetailModelToModel()
                {
                    BranchId = branchId,
                    CostPrice = item.CostPrice,
                    SellPrice = item.SellPrice,
                    DescriptionId = dto.DescriptionId,
                    ToDescriptionId = dto.ToDescriptionId,
                    EntryDate = DateTime.Now,
                    EUserId = userId,
                    OrganizationId = orgId,
                    PartsId = item.PartsId,
                    Quantity = item.Quantity,
                    Remarks = item.Remarks,
                };
                details.Add(detail);
                MobilePartStockDetailDTO stockOutItem = new MobilePartStockDetailDTO
                {
                    SWarehouseId = dto.WarehouseId,
                    MobilePartId = item.PartsId,
                    CostPrice = item.CostPrice,
                    SellPrice = item.SellPrice,
                    Quantity = item.Quantity,
                    Remarks = info.TransferCode,
                    StockStatus = StockStatus.StockOut,
                    OrganizationId = orgId,
                    EntryDate = DateTime.Now,
                    EUserId = userId,
                    BranchId = branchId,
                    ReferrenceNumber = info.TransferCode,
                    DescriptionId = info.DescriptionId
                };
                partsDetailsForStockOut.Add(stockOutItem);

                MobilePartStockDetailDTO stockInItem = new MobilePartStockDetailDTO
                {
                    SWarehouseId = dto.WarehouseId,
                    MobilePartId = item.PartsId,
                    CostPrice = item.CostPrice,
                    SellPrice = item.SellPrice,
                    Quantity = item.Quantity,
                    Remarks = info.TransferCode,
                    StockStatus = StockStatus.StockIn,
                    OrganizationId = orgId,
                    EntryDate = DateTime.Now,
                    EUserId = userId,
                    BranchId = branchId,
                    ReferrenceNumber = info.TransferCode,
                    DescriptionId = info.ToDescriptionId
                };
                partsDetailsForStockIn.Add(stockInItem);
            }

            info.StockTransferDetailModelToModels = details;
            _stockTransferInfoModelToModelRepository.Insert(info);

            if (_stockTransferInfoModelToModelRepository.Save())
            {

                if(_mobilePartStockDetailBusiness.SaveMobilePartStockOut(partsDetailsForStockOut, userId, orgId, branchId))
                {
                    IsSuccess = _mobilePartStockDetailBusiness.SaveMobilePartStockIn(partsDetailsForStockIn, userId, orgId, branchId);
                }
            }
            return IsSuccess;
        }
        public IEnumerable<StockTransferInfoModelToModel> GetAllStockTransferInfoModelToModelByOrgIdAndBranch(long orgId, long branchId)
        {
            return _stockTransferInfoModelToModelRepository.GetAll(ts => ts.OrganizationId == orgId && ts.BranchId == branchId).ToList();
        }
        public StockTransferInfoModelToModel GetStockTransferMMInfoById(long id, long orgId)
        {
            return _stockTransferInfoModelToModelRepository.GetOneByOrg(t => t.TransferInfoModelToModelId == id && t.OrganizationId == orgId);
        }
    }
}
