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

        public TransferStockToAssemblyInfoBusiness(IProductionUnitOfWork productionDb, IItemBusiness itemBusiness, IProductionStockDetailBusiness productionStockDetailBusiness)
        {
            this._productionDb = productionDb;
            this._itemBusiness = itemBusiness;
            this._productionStockDetailBusiness = productionStockDetailBusiness;
            this._transferStockToAssemblyInfoRepository = new TransferStockToAssemblyInfoRepository(this._productionDb);
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
            TransferStockToAssemblyInfo info = new TransferStockToAssemblyInfo
            {
                LineId = infoDto.LineId,
                AssemblyId = infoDto.AssemblyId,
                DescriptionId = infoDto.DescriptionId,
                WarehouseId = infoDto.WarehouseId,
                OrganizationId = orgId,
                StateStatus = RequisitionStatus.Approved,
                ItemTypeId = infoDto.ItemTypeId,
                ItemId = infoDto.ItemId,
                ForQty = infoDto.ForQty,
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
                    Remarks = item.Remarks,
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
                    RefferenceNumber = info.TransferCode
                };
                floorStockOutItems.Add(stockOutItem);
            }

            info.TransferStockToAssemblyDetails = details;
            _transferStockToAssemblyInfoRepository.Insert(info);

            if(_transferStockToAssemblyInfoRepository.Save())
            {
                IsSuccess=_productionStockDetailBusiness.SaveProductionStockOut(floorStockOutItems, userId, orgId, StockOutReason.StockOutByTransferToAssembly);
            }
            return IsSuccess;
        }
    }
}
