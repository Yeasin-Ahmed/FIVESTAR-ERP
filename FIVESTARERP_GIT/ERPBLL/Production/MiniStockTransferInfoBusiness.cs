using ERPBLL.Common;
using ERPBLL.Inventory.Interface;
using ERPBLL.Production.Interface;
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
    public class MiniStockTransferInfoBusiness : IMiniStockTransferInfoBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly MiniStockTransferInfoRepository _miniStockTransferInfoRepository;
        private readonly IItemBusiness _itemBusiness;
        private readonly IMiniStockTransferDetailBusiness _miniStockTransferDetailBusiness;
        private readonly IProductionAssembleStockDetailBusiness _productionAssembleStockDetailBusiness;

        public MiniStockTransferInfoBusiness(IProductionUnitOfWork productionDb, IItemBusiness itemBusiness, IMiniStockTransferDetailBusiness miniStockTransferDetailBusiness, IProductionAssembleStockDetailBusiness productionAssembleStockDetailBusiness)
        {
            this._productionDb = productionDb;
            this._miniStockTransferInfoRepository = new MiniStockTransferInfoRepository(this._productionDb);
            this._itemBusiness = itemBusiness;
            this._miniStockTransferDetailBusiness = miniStockTransferDetailBusiness;
            this._productionAssembleStockDetailBusiness = productionAssembleStockDetailBusiness;
        }
        public IEnumerable<MiniStockTransferInfo> GetMiniStockTransferInfosByFloor(long floorId, long orgId)
        {
            return this._miniStockTransferInfoRepository.GetAll(s => s.FloorId == floorId && s.OrganizationId == orgId);
        }

        public MiniStockTransferInfo GetMiniStockTransferInfosById(long id, long orgId)
        {
            return this._miniStockTransferInfoRepository.GetOneByOrg(s => s.MSTInfoId == id && s.OrganizationId == orgId);
        }

        public IEnumerable<MiniStockTransferInfo> GetMiniStockTransferInfosByOrg(long orgId)
        {
            return this._miniStockTransferInfoRepository.GetAll(s => s.OrganizationId == orgId);
        }

        public IEnumerable<MiniStockTransferInfo> GetMiniStockTransferInfosByPackaging(long packagingId, long floorId, long orgId)
        {
            return this._miniStockTransferInfoRepository.GetAll(s => s.PackagingLineId == packagingId && s.FloorId == floorId && s.OrganizationId == orgId);
        }

        public IEnumerable<MiniStockTransferInfoDTO> GetMiniStockTransferInfosByQuery(long? packagingId, long? floorId,long? transferId, string transferCode, string status, string fromDate, string toDate, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<MiniStockTransferInfoDTO>(QueryForMiniStockTransferInfos(packagingId, floorId, transferId, transferCode, status, fromDate, toDate, orgId)).ToList();
        }

        private string QueryForMiniStockTransferInfos(long? packagingId, long? floorId, long? transferId, string transferCode, string status, string fromDate, string toDate, long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;

            if (transferId != null && transferId > 0)
            {
                param += string.Format(@" and msi.MSTInfoId={0}", transferId);
            }
            if (packagingId != null && packagingId > 0)
            {
                param += string.Format(@" and msi.PackagingLineId={0}", packagingId);
            }
            if (floorId != null && floorId > 0)
            {
                param += string.Format(@" and msi.FloorId={0}", floorId);
            }
            if (!string.IsNullOrEmpty(transferCode) && transferCode.Trim() != "")
            {
                param += string.Format(@" and msi.TransferCode Like '%{0}%'", transferCode.Trim());
            }
            if (!string.IsNullOrEmpty(status) && status.Trim() != "")
            {
                param += string.Format(@" and msi.StateStatus='{0}'", status.Trim());
            }
            if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "" && !string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(msi.EntryDate as date) between '{0}' and '{1}'", fDate.Trim(), tDate.Trim());
            }
            else if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(msi.EntryDate as date)='{0}'", fDate.Trim());
            }
            else if (!string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(msi.EntryDate as date)='{0}'", tDate.Trim());
            }

            query = string.Format(@"Select MSTInfoId,TransferCode,FloorId,FloorName,PackagingLineId,PackagingLineName,Substring(ModelNames,1,LEN(ModelNames)-1) 'ModelNames',StateStatus,EUserId,EntryUser,EntryDate,UpUserId,UpdateUser,UpdateDate From
(Select msi.MSTInfoId,msi.TransferCode,msi.FloorId,pl.LineNumber 'FloorName',msi.PackagingLineId,pac.PackagingLineName,
msi.StateStatus,
msi.EUserId,app.UserName 'EntryUser',msi.EntryDate,msi.UpUserId,(Select UserName From [ControlPanel].dbo.tblApplicationUsers Where UserId = msi.UpUserId and OrganizationId= msi.OrganizationId) 'UpdateUser',msi.UpdateDate,
-- Model Names --
(Select de.DescriptionName+' (Qty-' +CAST(SUM(msd.Quantity) as NVARCHAR(20))+'),' From tblMiniStockTransferDetail msd
Left Join [Inventory].dbo.tblDescriptions de on msd.DescriptionId = de.DescriptionId
Where MSTInfoId = msi.MSTInfoId
Group By msd.DescriptionId,de.DescriptionName
Order By de.DescriptionName For XML Path('')) 'ModelNames'
From tblMiniStockTransferInfo msi
Left Join tblPackagingLine pac on msi.PackagingLineId = pac.PackagingLineId
Left Join tblProductionLines pl on msi.FloorId  = pl.LineId
Left Join [ControlPanel].dbo.tblApplicationUsers app on msi.EUserId = app.UserId and app.OrganizationId= msi.OrganizationId
Where 1=1 and msi.OrganizationId={0} {1}) tbl
Order By EntryDate desc", orgId  ,Utility.ParamChecker(param));
            return query;
        }

        public bool SaveMiniStockTranferStatus(long transferId, string status, long userId, long orgId)
        {
            bool IsSuccess = false;
            var transferInfo = this.GetMiniStockTransferInfosById(transferId, orgId);
            var statusInDb = transferInfo.StateStatus;
            if (transferInfo != null)
            {
                transferInfo.StateStatus = status;
                transferInfo.UpUserId = userId;
                transferInfo.EntryDate = DateTime.Now;
                _miniStockTransferInfoRepository.Update(transferInfo);
            }

            if(transferInfo != null && statusInDb == "Pending" && status == "Send")
            {
               var miniStockTransferDetail = _miniStockTransferDetailBusiness.GetMiniStockTransfersByInfo(transferId, orgId);
                List<ProductionAssembleStockDetailDTO> stockDetailDTOs = new List<ProductionAssembleStockDetailDTO>();
                foreach (var item in miniStockTransferDetail)
                {
                    ProductionAssembleStockDetailDTO stockDetailDTO = new ProductionAssembleStockDetailDTO()
                    {
                        ProductionFloorId = transferInfo.FloorId,
                        DescriptionId = item.DescriptionId,
                        WarehouseId = item.WarehouseId,
                        ItemTypeId = item.ItemTypeId,
                        ItemId = item.ItemId,
                        Quantity = item.Quantity,
                        UnitId = item.UnitId,
                        RefferenceNumber = transferInfo.TransferCode,
                        OrganizationId = orgId,
                        EntryDate = DateTime.Now,
                        EUserId = userId,
                        PackagingLineId = transferInfo.PackagingLineId,
                        StockStatus = StockStatus.StockIn,
                        Remarks = "Stock In By Production Requisition"
                    };
                    stockDetailDTOs.Add(stockDetailDTO);
                }
                if (_miniStockTransferInfoRepository.Save())
                {
                    IsSuccess= _productionAssembleStockDetailBusiness.SaveProductionAssembleStockDetailStockOut(stockDetailDTOs, userId, orgId);
                }
                else
                {
                    IsSuccess = false;
                }
            }
            else
            {
                IsSuccess=_miniStockTransferInfoRepository.Save();
            }
            return IsSuccess;
        }

        public bool SaveMiniStockTransfer(MiniStockTransferInfoDTO model, long userId, long orgId)
        {
            var allItemsInDb = _itemBusiness.GetAllItemByOrgId(orgId);
            MiniStockTransferInfo miniStockTransferInfo = new MiniStockTransferInfo()
            {
                PackagingLineId = model.PackagingLineId,
                FloorId = model.FloorId,
                StateStatus = "Pending",
                TransferCode = "MST-" + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss"),
                OrganizationId = orgId,
                EUserId = userId,
                EntryDate = DateTime.Now,
                Remarks = "Mini-Stock Transfer"
            };
            List<MiniStockTransferDetail> miniStockTransferDetails = new List<MiniStockTransferDetail>();
            foreach (var item in model.MiniStockTransferDetails)
            {
                MiniStockTransferDetail miniStockTransferDetail = new MiniStockTransferDetail()
                {
                    DescriptionId = item.DescriptionId,
                    WarehouseId = item.WarehouseId,
                    ItemTypeId = item.ItemTypeId,
                    ItemId = item.ItemId,
                    UnitId = allItemsInDb.FirstOrDefault(s=> s.ItemId == item.ItemId).UnitId,
                    Quantity = item.Quantity,
                    OrganizationId = orgId,
                    EUserId = userId,
                    EntryDate = DateTime.Now,
                    Remarks ="Mini Stock Transfer"
                };
                miniStockTransferDetails.Add(miniStockTransferDetail);
            }
            miniStockTransferInfo.MiniStockTransferDetails = miniStockTransferDetails;
            _miniStockTransferInfoRepository.Insert(miniStockTransferInfo);
            return _miniStockTransferInfoRepository.Save();
        }
    }
}
