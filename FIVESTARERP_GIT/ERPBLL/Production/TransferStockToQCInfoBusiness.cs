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
    public class TransferStockToQCInfoBusiness : ITransferStockToQCInfoBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly IItemBusiness _itemBusiness;
        private readonly IAssemblyLineStockDetailBusiness _assemblyLineStockDetailBusiness;
        private readonly TransferStockToQCInfoRepository _transferStockToQCInfoRepository;

        public TransferStockToQCInfoBusiness(IProductionUnitOfWork productionDb, IItemBusiness itemBusiness, IAssemblyLineStockDetailBusiness assemblyLineStockDetailBusiness)
        {
            this._productionDb = productionDb;
            this._itemBusiness = itemBusiness;
            this._assemblyLineStockDetailBusiness = assemblyLineStockDetailBusiness;
            this._transferStockToQCInfoRepository = new TransferStockToQCInfoRepository(this._productionDb);
        }

        public TransferStockToQCInfo GetStockToQCInfoById(long id, long orgId)
        {
            return _transferStockToQCInfoRepository.GetOneByOrg(t => t.TSQInfoId == id && t.OrganizationId == orgId);
        }

        public IEnumerable<TransferStockToQCInfo> GetStockToQCInfos(long orgId)
        {
            return _transferStockToQCInfoRepository.GetAll(t => t.OrganizationId == orgId).ToList();
        }

        public bool SaveTransferInfoStateStatus(long transferId, string status, long userId, long orgId)
        {
            var info = GetStockToQCInfoById(transferId, orgId);
            if(info != null && info.StateStatus == RequisitionStatus.Approved)
            {
                info.StateStatus = RequisitionStatus.Accepted;
                info.UpUserId = userId;
                info.UpdateDate = DateTime.Now;
                _transferStockToQCInfoRepository.Update(info);
            }
            return _transferStockToQCInfoRepository.Save();
        }

        public bool SaveTransferStockQC(TransferStockToQCInfoDTO infoDto, List<TransferStockToQCDetailDTO> detailDto, long userId, long orgId)
        {
            bool IsSuccess = false;
            TransferStockToQCInfo info = new TransferStockToQCInfo
            {
                LineId = infoDto.LineId,
                AssemblyId =infoDto.AssemblyId,
                QCLineId = infoDto.QCLineId,
                DescriptionId = infoDto.DescriptionId,
                WarehouseId = infoDto.WarehouseId,
                OrganizationId = orgId,
                StateStatus = RequisitionStatus.Approved,
                Remarks = "",
                EUserId = userId,
                EntryDate = DateTime.Now,
                TransferCode = ("TSQ-" + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss")),
                ItemTypeId = infoDto.ItemTypeId,
                ItemId = infoDto.ItemId,
                ForQty = infoDto.ForQty
            };
            List<TransferStockToQCDetail> details = new List<TransferStockToQCDetail>();
            List<AssemblyLineStockDetailDTO> assemblyStockOutItems = new List<AssemblyLineStockDetailDTO>();

            foreach (var item in detailDto)
            {
                TransferStockToQCDetail detail = new TransferStockToQCDetail
                {
                    WarehouseId= item.WarehouseId,
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
                AssemblyLineStockDetailDTO stockOutItem = new AssemblyLineStockDetailDTO
                {
                    ProductionLineId = info.LineId,
                    DescriptionId = info.DescriptionId,
                    WarehouseId = item.WarehouseId,
                    AssemblyLineId = info.AssemblyId,
                    ItemTypeId = item.ItemTypeId,
                    ItemId = item.ItemId,
                    UnitId = detail.UnitId,
                    Quantity = item.Quantity,
                    Remarks =StockOutReason.StockOutByTransferToQC,
                    StockStatus = StockStatus.StockOut,
                    OrganizationId = orgId,
                    EntryDate = DateTime.Now,
                    EUserId = userId,
                    RefferenceNumber = info.TransferCode,
                    
                };
                assemblyStockOutItems.Add(stockOutItem);
            }

            info.TransferStockToQCDetails = details;
            _transferStockToQCInfoRepository.Insert(info);

            if(_transferStockToQCInfoRepository.Save())
            {
                IsSuccess=_assemblyLineStockDetailBusiness.SaveAssemblyLineStockOut(assemblyStockOutItems, userId, orgId, StockOutReason.StockOutByTransferToQC);
            }
            return IsSuccess;
        }
    }
}
