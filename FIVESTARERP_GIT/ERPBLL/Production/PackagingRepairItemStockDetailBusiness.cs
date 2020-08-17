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
    public class PackagingRepairItemStockDetailBusiness : IPackagingRepairItemStockDetailBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly PackagingRepairItemStockDetailRepository _packagingRepairItemStockDetailRepository;
        private readonly PackagingRepairItemStockInfoRepository _packagingRepairItemStockInfoRepository;
        private readonly IPackagingRepairItemStockInfoBusiness _packagingRepairItemStockInfoBusiness;

        public PackagingRepairItemStockDetailBusiness(IProductionUnitOfWork productionDb, IPackagingRepairItemStockInfoBusiness packagingRepairItemStockInfoBusiness, PackagingRepairItemStockInfoRepository packagingRepairItemStockInfoRepository)
        {
            this._productionDb = productionDb;
            this._packagingRepairItemStockDetailRepository = new PackagingRepairItemStockDetailRepository(this._productionDb);
            this._packagingRepairItemStockInfoBusiness = packagingRepairItemStockInfoBusiness;
            this._packagingRepairItemStockInfoRepository = new PackagingRepairItemStockInfoRepository(this._productionDb);
        }

        public IEnumerable<PackagingRepairItemStockDetailDTO> GetPackagingRepairItemStockDetailByQuery(long floorId, long packagingLine, long modelId, long warehouseId, long itemTypeId, long itemId, string refNum, string fromDate, string toDate, string status)
        {
            throw new NotImplementedException();
        }

        public bool SavePackagingRepairItemStockIn(List<PackagingRepairItemStockDetailDTO> stockDetailDTOs, long userId, long orgId)
        {
            List<PackagingRepairItemStockDetail> packagingItemStockDetails = new List<PackagingRepairItemStockDetail>();
            foreach (var item in stockDetailDTOs)
            {
                PackagingRepairItemStockDetail stockDetail = new PackagingRepairItemStockDetail();
                stockDetail.PackagingLineId = item.PackagingLineId;
                stockDetail.FloorId= item.FloorId;
                stockDetail.DescriptionId = item.DescriptionId;
                stockDetail.WarehouseId = item.WarehouseId;

                stockDetail.ItemTypeId = item.ItemTypeId;
                stockDetail.ItemId = item.ItemId;
                stockDetail.Quantity = item.Quantity;
                stockDetail.OrganizationId = orgId;
                stockDetail.EUserId = userId;
                stockDetail.Remarks = item.Remarks;
                stockDetail.UnitId = item.UnitId;
                stockDetail.EntryDate = DateTime.Now;
                stockDetail.StockStatus = StockStatus.StockIn;
                stockDetail.RefferenceNumber = item.RefferenceNumber;

                var packagingStockInfo = _packagingRepairItemStockInfoBusiness.GetPackagingRepairItemStockInfoByPackagingLineAndModelAndItem(item.FloorId.Value,item.PackagingLineId.Value, item.ItemId.Value, item.DescriptionId.Value, orgId);
                if (packagingStockInfo != null)
                {
                    packagingStockInfo.StockInQty += item.Quantity;
                    _packagingRepairItemStockInfoRepository.Update(packagingStockInfo);
                }
                else
                {
                    PackagingRepairItemStockInfo info = new PackagingRepairItemStockInfo();
                    info.PackagingLineId = item.PackagingLineId;
                    info.FloorId = item.FloorId;
                    info.WarehouseId = item.WarehouseId;
                    info.DescriptionId = item.DescriptionId;
                    info.ItemTypeId = item.ItemTypeId;
                    info.ItemId = item.ItemId;
                    info.UnitId = stockDetail.UnitId;
                    info.StockInQty = item.Quantity;
                    info.StockOutQty = 0;
                    info.OrganizationId = orgId;
                    info.EUserId = userId;
                    info.EntryDate = DateTime.Now;
                    _packagingRepairItemStockInfoRepository.Insert(info);
                }
                packagingItemStockDetails.Add(stockDetail);
            }
            _packagingRepairItemStockDetailRepository.InsertAll(packagingItemStockDetails);
            return _packagingRepairItemStockDetailRepository.Save();
        }

        public bool SavePackagingRepairItemStockOut(List<PackagingRepairItemStockDetailDTO> stockDetailDTOs, long userId, long orgId)
        {
            List<PackagingRepairItemStockDetail> packagingItemStockDetails = new List<PackagingRepairItemStockDetail>();
            foreach (var item in stockDetailDTOs)
            {
                PackagingRepairItemStockDetail stockDetail = new PackagingRepairItemStockDetail();
                stockDetail.PackagingLineId = item.PackagingLineId;
                stockDetail.FloorId = item.FloorId;
                stockDetail.DescriptionId = item.DescriptionId;
                stockDetail.WarehouseId = item.WarehouseId;
                stockDetail.ItemTypeId = item.ItemTypeId;
                stockDetail.ItemId = item.ItemId;
                stockDetail.Quantity = item.Quantity;
                stockDetail.OrganizationId = orgId;
                stockDetail.EUserId = userId;
                stockDetail.Remarks = item.Remarks;
                stockDetail.UnitId = item.UnitId;
                stockDetail.EntryDate = DateTime.Now;
                stockDetail.StockStatus = StockStatus.StockOut;
                stockDetail.RefferenceNumber = item.RefferenceNumber;

                var packagingStockInfo = _packagingRepairItemStockInfoBusiness.GetPackagingRepairItemStockInfoByPackagingLineAndModelAndItem(item.FloorId.Value,item.PackagingLineId.Value, item.ItemId.Value, item.DescriptionId.Value, orgId);

                packagingStockInfo.StockOutQty += item.Quantity;
                _packagingRepairItemStockInfoRepository.Update(packagingStockInfo);
                packagingItemStockDetails.Add(stockDetail);
            }
            _packagingRepairItemStockDetailRepository.InsertAll(packagingItemStockDetails);
            return _packagingRepairItemStockDetailRepository.Save();
        }
    }
}
