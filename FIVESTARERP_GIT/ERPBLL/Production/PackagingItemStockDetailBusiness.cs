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
    public class PackagingItemStockDetailBusiness : IPackagingItemStockDetailBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly PackagingItemStockDetailRepository _packagingItemStockDetailRepository;
        private readonly PackagingItemStockInfoRepository _packagingItemStockInfoRepository;
        private readonly IPackagingItemStockInfoBusiness _packagingItemStockInfoBusiness;
        public PackagingItemStockDetailBusiness(IProductionUnitOfWork productionDb, IPackagingItemStockInfoBusiness packagingItemStockInfoBusiness)
        {
            this._productionDb = productionDb;
            this._packagingItemStockInfoBusiness = packagingItemStockInfoBusiness;
            this._packagingItemStockInfoRepository = new PackagingItemStockInfoRepository(this._productionDb);
            this._packagingItemStockDetailRepository = new PackagingItemStockDetailRepository(this._productionDb);

        }

        public IEnumerable<PackagingItemStockDetail> GetPackagingItemStockDetails(long orgId)
        {
            return _packagingItemStockDetailRepository.GetAll(p => p.OrganizationId == orgId).ToList();
        }

        public bool SavePackagingItemStockIn(List<PackagingItemStockDetailDTO> stockDetails, long userId, long orgId)
        {
            bool IsSuccess = false;
            List<PackagingItemStockDetail> list = new List<PackagingItemStockDetail>();
            foreach (var item in stockDetails)
            {
                PackagingItemStockDetail stock = new PackagingItemStockDetail
                {
                    ProductionFloorId = item.ProductionFloorId,
                    QCId = item.QCId,
                    PackagingLineId = item.PackagingLineId,
                    DescriptionId = item.DescriptionId,
                    WarehouseId = item.WarehouseId,
                    ItemTypeId = item.ItemTypeId,
                    ItemId = item.ItemId,
                    OrganizationId = orgId,
                    EUserId = userId,
                    PackagingLineToId = item.PackagingLineToId,
                    Quantity = item.Quantity,
                    ReferenceNumber = item.ReferenceNumber,
                    StockStatus = StockStatus.StockIn,
                    Remarks = item.Remarks,
                    EntryDate = DateTime.Now
                };

                var packagingStockInDb = _packagingItemStockInfoBusiness.GetPackagingItemStockInfoByPackagingId(item.PackagingLineId.Value, item.DescriptionId.Value, item.ItemId.Value, orgId);
                if (packagingStockInDb != null)
                {
                    packagingStockInDb.Quantity += item.Quantity;
                    packagingStockInDb.UpdateDate = DateTime.Now;
                    this._packagingItemStockInfoRepository.Update(packagingStockInDb);
                }
                else
                {
                    PackagingItemStockInfo stockInfo = new PackagingItemStockInfo
                    {
                        ProductionFloorId = item.ProductionFloorId,
                        DescriptionId = item.DescriptionId,
                        WarehouseId = item.WarehouseId,
                        ItemTypeId = item.ItemTypeId,
                        ItemId = item.ItemId,
                        PackagingLineId = item.PackagingLineId,
                        EUserId = userId,
                        EntryDate = DateTime.Now,
                        OrganizationId = orgId,
                        Quantity = item.Quantity,
                        TransferQty = 0,
                        Remarks = ""
                    };
                    this._packagingItemStockInfoRepository.Insert(stockInfo);
                };
                list.Add(stock);
            }
            if (list.Count > 0)
            {
                _packagingItemStockDetailRepository.InsertAll(list);
                IsSuccess = _packagingItemStockDetailRepository.Save();
            }

            return IsSuccess;
        }

        public bool SavePackagingItemStockOut(List<PackagingItemStockDetailDTO> stockDetails, long userId, long orgId)
        {
            bool IsSuccess = false;
            List<PackagingItemStockDetail> list = new List<PackagingItemStockDetail>();
            foreach (var item in stockDetails)
            {
                PackagingItemStockDetail stock = new PackagingItemStockDetail
                {
                    ProductionFloorId = item.ProductionFloorId,
                    QCId = item.QCId,
                    PackagingLineId = item.PackagingLineId,
                    DescriptionId = item.DescriptionId,
                    WarehouseId = item.WarehouseId,
                    ItemTypeId = item.ItemTypeId,
                    ItemId = item.ItemId,
                    OrganizationId = orgId,
                    EUserId = userId,
                    PackagingLineToId = item.PackagingLineToId,
                    Quantity = item.Quantity,
                    ReferenceNumber = item.ReferenceNumber,
                    StockStatus = StockStatus.StockOut,
                    Remarks = item.Remarks,
                    EntryDate = DateTime.Now
                };

                var packagingStockInDb = _packagingItemStockInfoBusiness.GetPackagingItemStockInfoByPackagingId(item.PackagingLineId.Value, item.DescriptionId.Value, item.ItemId.Value, orgId);
                packagingStockInDb.TransferQty += item.Quantity;
                packagingStockInDb.UpdateDate = DateTime.Now;
                this._packagingItemStockInfoRepository.Update(packagingStockInDb);
                list.Add(stock);
            }
            if (list.Count > 0)
            {
                _packagingItemStockDetailRepository.InsertAll(list);
                IsSuccess = _packagingItemStockDetailRepository.Save();
            }

            return IsSuccess;
        }
    }
}
