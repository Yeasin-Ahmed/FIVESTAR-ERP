using ERPBLL.Common;
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
    public class TransferToPackagingRepairInfoBusiness : ITransferToPackagingRepairInfoBusiness
    {
        // Db Context //
        private readonly IProductionUnitOfWork _productionDb;
        // Business Class //
        private readonly ITransferToPackagingRepairDetailBusiness _transferToPackagingRepairDetailBusiness;
        private readonly IPackagingRepairItemStockDetailBusiness _packagingRepairItemStockDetailBusiness;
        private readonly IPackagingRepairRawStockDetailBusiness _packagingRepairRawStockDetailBusiness;
        // Repository //
        private readonly TransferToPackagingRepairInfoRepository _transferToPackagingRepairInfoRepository;
        public TransferToPackagingRepairInfoBusiness(IProductionUnitOfWork productionDb, ITransferToPackagingRepairDetailBusiness transferToPackagingRepairDetailBusiness, IPackagingRepairItemStockDetailBusiness packagingRepairItemStockDetailBusiness, IPackagingRepairRawStockDetailBusiness packagingRepairRawStockDetailBusiness)
        {
            // Database//
            this._productionDb = productionDb;
            // Business Class //
            this._transferToPackagingRepairDetailBusiness = transferToPackagingRepairDetailBusiness;
            this._packagingRepairItemStockDetailBusiness = packagingRepairItemStockDetailBusiness;
            this._packagingRepairRawStockDetailBusiness = packagingRepairRawStockDetailBusiness;
            // Repositorys //
            this._transferToPackagingRepairInfoRepository = new TransferToPackagingRepairInfoRepository(this._productionDb);
        }
        public async Task<TransferToPackagingRepairInfo> GetNonReceivedTransferToPackagingRepairInfoAsync(long floorId, long packagingLineId, long modelId, long itemId, long orgId)
        {
            return await _transferToPackagingRepairInfoRepository.GetOneByOrgAsync(s => s.ProductionFloorId == floorId && s.PackagingLineId == packagingLineId && s.DescriptionId == modelId && s.ItemId == itemId && s.OrganizationId == orgId && s.StateStatus == "Approved");
        }

        public TransferToPackagingRepairInfo GetPackagingRepairInfoById(long transferId, long orgId)
        {
            return this._transferToPackagingRepairInfoRepository.GetOneByOrg(s => s.TPRInfoId == transferId && s.OrganizationId == orgId);
        }

        public IEnumerable<TransferToPackagingRepairInfoDTO> GetTransferToPackagingRepairInfosByQuery(long? floorId, long? packagingLineId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string status, string fromDate, string toDate, string transferCode, long? transferId, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<TransferToPackagingRepairInfoDTO>(QueryForTransferToPackagingRepairInfos(floorId, packagingLineId, modelId, warehouseId, itemTypeId, itemId, status, fromDate, toDate, transferCode, transferId, orgId)).ToList();
        }

        public bool SavePakagingQCTransferStateStatus(long transferId, string status, long userId, long orgId)
        {
            bool IsSuccess = false;
            var transferInfo = GetPackagingRepairInfoById(transferId, orgId);
            if (transferInfo != null && transferInfo.StateStatus == RequisitionStatus.Approved && status == RequisitionStatus.Accepted)
            {
                transferInfo.StateStatus = status;
                transferInfo.UpUserId = userId;
                transferInfo.UpdateDate = DateTime.Now;

                // Transfer Info Status //
                if (_transferToPackagingRepairInfoRepository.Save())
                {
                    var transferDetails = _transferToPackagingRepairDetailBusiness.GetTransferToPackagingRepairDetailsByTransferId(transferId, orgId);
                    // Packaging Repair Item Stock //
                    List<PackagingRepairItemStockDetailDTO> packagingRepairItems = new List<PackagingRepairItemStockDetailDTO>() {
                        new PackagingRepairItemStockDetailDTO(){
                            FloorId = transferInfo.ProductionFloorId,
                            PackagingLineId = transferInfo.PackagingLineId,
                            DescriptionId = transferInfo.DescriptionId,
                            WarehouseId = transferInfo.WarehouseId,
                            ItemTypeId = transferInfo.ItemTypeId,
                            ItemId= transferInfo.ItemId,
                            UnitId = transferInfo.UnitId,
                            Quantity = transferInfo.Quantity,
                            EUserId = userId,
                            EntryDate = DateTime.Now,
                            StockStatus = StockStatus.StockIn,
                            OrganizationId = orgId,
                            RefferenceNumber = transferInfo.TransferCode,
                            Remarks = "Stock In By Packaging QC Transfer"
                        }
                    };

                    // Packaging Repair Raw Stock //
                    List<PackagingRepairRawStockDetailDTO> packagingRepairRawStocks = new List<PackagingRepairRawStockDetailDTO>();
                    foreach (var item in transferDetails)
                    {
                        PackagingRepairRawStockDetailDTO packagingRepairRawStockDetail = new PackagingRepairRawStockDetailDTO()
                        {
                            FloorId = item.ProductionFloorId,
                            PackagingLineId= item.PackagingLineId,
                            DescriptionId = item.DescriptionId,
                            WarehouseId = item.WarehouseId,
                            ItemTypeId = item.ItemTypeId,
                            ItemId = item.ItemId,
                            Quantity = (item.Quantity * transferInfo.Quantity),
                            UnitId = item.UnitId,
                            EUserId = userId,
                            EntryDate = DateTime.Now,
                            OrganizationId = orgId,
                            RefferenceNumber = transferInfo.TransferCode,
                            StockStatus = StockStatus.StockIn,
                            Remarks = "Stock In By Packaging QC Transfer"
                        };
                        packagingRepairRawStocks.Add(packagingRepairRawStockDetail);
                    }

                    if (_packagingRepairItemStockDetailBusiness.SavePackagingRepairItemStockIn(packagingRepairItems, userId, orgId))
                    {
                        IsSuccess = _packagingRepairRawStockDetailBusiness.SavePackagingRepairRawStockIn(packagingRepairRawStocks, userId, orgId);
                    }

                }
            }
            return IsSuccess;
        }

        private string QueryForTransferToPackagingRepairInfos(long? floorId, long? packagingLineId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string status, string fromDate, string toDate, string transferCode, long? transferInfoId, long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;

            if (transferInfoId != null && transferInfoId > 0)
            {
                param += string.Format(@" and info.TPRInfoId={0}", transferInfoId);
            }
            if (!string.IsNullOrEmpty(transferCode) && transferCode.Trim() != "")
            {
                param += string.Format(@" and info.TransferCode Like'%{0}%'", transferCode);
            }
            if (floorId != null && floorId > 0)
            {
                param += string.Format(@" and info.ProductionFloorId={0}", floorId);
            }
            if (packagingLineId != null && packagingLineId > 0)
            {
                param += string.Format(@" and info.PackagingLineId={0}", packagingLineId);
            }
            if (modelId != null && modelId > 0)
            {
                param += string.Format(@" and info.DescriptionId={0}", modelId);
            }
            if (warehouseId != null && warehouseId > 0)
            {
                param += string.Format(@" and info.WarehouseId={0}", warehouseId);
            }
            if (itemTypeId != null && itemTypeId > 0)
            {
                param += string.Format(@" and info.ItemTypeId={0}", itemTypeId);
            }
            if (itemId != null && itemId > 0)
            {
                param += string.Format(@" and info.ItemId={0}", itemId);
            }
            if (!string.IsNullOrEmpty(status) && status.Trim() != "")
            {
                param += string.Format(@" and info.StateStatus='{0}'", status);
            }
            if (!string.IsNullOrEmpty(transferCode) && transferCode.Trim() != "")
            {
                param += string.Format(@" and info.TransferCode Like'%{0}%'", transferCode);
            }
            if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "" && !string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(info.EntryDate as date) between '{0}' and '{1}'", fDate, tDate);
            }
            else if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(info.EntryDate as date)='{0}'", fDate);
            }
            else if (!string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(info.EntryDate as date)='{0}'", tDate);
            }

            query = string.Format(@"Select info.TPRInfoId,info.TransferCode,info.DescriptionId,de.DescriptionName 'ModelName',info.ProductionFloorId, pl.LineNumber 'ProductionFloorName',info.PackagingLineId,pac.PackagingLineName,info.WarehouseId,wa.WarehouseName,info.ItemTypeId,it.ItemName 'ItemTypeName',info.ItemId,i.ItemName,info.Quantity,info.EntryDate,app.UserName 'EntryUser',info.StateStatus,
info.UpdateDate,(Select UserName From [ControlPanel].dbo.tblApplicationUsers Where UserId= info.UpUserId) 'UpdateUser'
From [Production].dbo.tblTransferToPackagingRepairInfo info
Left Join [Production].dbo.tblProductionLines pl on info.ProductionFloorId = pl.LineId
Left Join [Production].dbo.tblPackagingLine  pac on info.PackagingLineId = pac.PackagingLineId
Left Join [Inventory].dbo.tblDescriptions de on info.DescriptionId = de.DescriptionId
Left Join [Inventory].dbo.tblWarehouses wa on info.WarehouseId = wa.Id
Left Join [Inventory].dbo.tblItemTypes it on info.ItemTypeId = it.ItemId
Left Join [Inventory].dbo.tblItems i on info.ItemId = i.ItemId
Inner Join [ControlPanel].dbo.tblApplicationUsers app on info.EUserId = app.UserId
Where 1=1 and info.OrganizationId={0} {1}", orgId, Utility.ParamChecker(param));
            return query;
        }
    }
}
