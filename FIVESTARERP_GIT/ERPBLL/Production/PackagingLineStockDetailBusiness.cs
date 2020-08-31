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
    public class PackagingLineStockDetailBusiness : IPackagingLineStockDetailBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly PackagingLineStockDetailRepository _packagingLineStockDetailRepository;
        private readonly PackagingLineStockInfoRepository _packagingLineStockInfoRepository;
        private readonly IPackagingLineStockInfoBusiness _packagingLineStockInfoBusiness;

        public PackagingLineStockDetailBusiness(IProductionUnitOfWork productionDb, IPackagingLineStockInfoBusiness packagingLineStockInfoBusiness)
        {
            this._productionDb = productionDb;
            this._packagingLineStockInfoRepository = new PackagingLineStockInfoRepository(this._productionDb);
            this._packagingLineStockDetailRepository = new PackagingLineStockDetailRepository(this._productionDb);
            this._packagingLineStockInfoBusiness = packagingLineStockInfoBusiness;
        }
        public IEnumerable<PackagingLineStockDetail> GetPackagingLineStockDetails(long orgId)
        {
            return _packagingLineStockDetailRepository.GetAll(s => s.OrganizationId == orgId).ToList();
        }

        public bool SavePackagingLineStockIn(List<PackagingLineStockDetailDTO> packagingLineStockDetailDTO, long userId, long orgId)
        {
            List<PackagingLineStockDetail> packagingLineStockDetails = new List<PackagingLineStockDetail>();
            foreach (var item in packagingLineStockDetailDTO)
            {
                PackagingLineStockDetail stockDetail = new PackagingLineStockDetail();
                stockDetail.PackagingLineId = item.PackagingLineId;
                stockDetail.QCLineId = item.QCLineId;
                stockDetail.ProductionLineId = item.ProductionLineId;
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

                var packagingStockInfo = _packagingLineStockInfoBusiness.GetPackagingLineStockInfoByPackagingAndItemAndModelId(item.ProductionLineId.Value, item.ItemId.Value, item.DescriptionId.Value, orgId);
                if (packagingStockInfo != null)
                {
                    packagingStockInfo.StockInQty += item.Quantity;
                    _packagingLineStockInfoRepository.Update(packagingStockInfo);
                }
                else
                {
                    PackagingLineStockInfo info = new PackagingLineStockInfo();
                    info.PackagingLineId = item.PackagingLineId;
                    info.QCLineId = item.QCLineId;
                    info.ProductionLineId = item.ProductionLineId;
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

                    _packagingLineStockInfoRepository.Insert(info);
                }
                packagingLineStockDetails.Add(stockDetail);
            }
            _packagingLineStockDetailRepository.InsertAll(packagingLineStockDetails);
            return _packagingLineStockDetailRepository.Save();
        }

        public async Task<bool> SavePackagingLineStockInAsync(List<PackagingLineStockDetailDTO> packagingLineStockDetailDTO, long userId, long orgId)
        {
            List<PackagingLineStockDetail> packagingLineStockDetails = new List<PackagingLineStockDetail>();
            foreach (var item in packagingLineStockDetailDTO)
            {
                PackagingLineStockDetail stockDetail = new PackagingLineStockDetail();
                stockDetail.PackagingLineId = item.PackagingLineId;
                stockDetail.QCLineId = item.QCLineId;
                stockDetail.ProductionLineId = item.ProductionLineId;
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

                var packagingStockInfo = _packagingLineStockInfoBusiness.GetPackagingLineStockInfoByPackagingAndItemAndModelId(item.ProductionLineId.Value, item.ItemId.Value, item.DescriptionId.Value, orgId);
                if (packagingStockInfo != null)
                {
                    packagingStockInfo.StockInQty += item.Quantity;
                    _packagingLineStockInfoRepository.Update(packagingStockInfo);
                }
                else
                {
                    PackagingLineStockInfo info = new PackagingLineStockInfo();
                    info.PackagingLineId = item.PackagingLineId;
                    info.QCLineId = item.QCLineId;
                    info.ProductionLineId = item.ProductionLineId;
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

                    _packagingLineStockInfoRepository.Insert(info);
                }
                packagingLineStockDetails.Add(stockDetail);
            }
            _packagingLineStockDetailRepository.InsertAll(packagingLineStockDetails);
            return await _packagingLineStockDetailRepository.SaveAsync();
        }

        public bool SavePackagingLineStockInByQCLine(long transferId, string status, long orgId, long userId)
        {
            throw new NotImplementedException();
        }

        public bool SavePackagingLineStockOut(List<PackagingLineStockDetailDTO> packagingLineStockDetailDTO, long userId, long orgId, string flag)
        {
            List<PackagingLineStockDetail> packagingLineStockDetails = new List<PackagingLineStockDetail>();
            foreach (var item in packagingLineStockDetailDTO)
            {
                PackagingLineStockDetail stockDetail = new PackagingLineStockDetail();
                stockDetail.PackagingLineId = item.PackagingLineId;
                stockDetail.ProductionLineId = item.ProductionLineId;
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

                var packagingStockInfo = _packagingLineStockInfoBusiness.GetPackagingLineStockInfoByPackagingAndItemAndModelId(item.ProductionLineId.Value, item.ItemId.Value, item.DescriptionId.Value, orgId);

                packagingStockInfo.StockOutQty += item.Quantity;
                _packagingLineStockInfoRepository.Update(packagingStockInfo);
                packagingLineStockDetails.Add(stockDetail);
            }
            _packagingLineStockDetailRepository.InsertAll(packagingLineStockDetails);
            return _packagingLineStockDetailRepository.Save();
        }

        public async Task<bool> SavePackagingLineStockOutAsync(List<PackagingLineStockDetailDTO> packagingLineStockDetailDTO, long userId, long orgId, string flag)
        {
            List<PackagingLineStockDetail> packagingLineStockDetails = new List<PackagingLineStockDetail>();
            foreach (var item in packagingLineStockDetailDTO)
            {
                PackagingLineStockDetail stockDetail = new PackagingLineStockDetail();
                stockDetail.PackagingLineId = item.PackagingLineId;
                stockDetail.ProductionLineId = item.ProductionLineId;
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

                var packagingStockInfo = _packagingLineStockInfoBusiness.GetPackagingLineStockInfoByPackagingAndItemAndModelId(item.ProductionLineId.Value, item.ItemId.Value, item.DescriptionId.Value, orgId);

                packagingStockInfo.StockOutQty += item.Quantity;
                _packagingLineStockInfoRepository.Update(packagingStockInfo);
                packagingLineStockDetails.Add(stockDetail);
            }
            _packagingLineStockDetailRepository.InsertAll(packagingLineStockDetails);
            return await _packagingLineStockDetailRepository.SaveAsync();
        }

        public bool SavePackagingLineStockReturn(List<PackagingLineStockDetailDTO> packagingLineStockDetailDTO, long userId, long orgId, string flag)
        {
            List<PackagingLineStockDetail> packagingLineStockDetails = new List<PackagingLineStockDetail>();
            foreach (var item in packagingLineStockDetailDTO)
            {
                PackagingLineStockDetail stockDetail = new PackagingLineStockDetail();
                stockDetail.PackagingLineId = item.PackagingLineId;
                stockDetail.ProductionLineId = item.ProductionLineId;
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
                stockDetail.StockStatus = StockStatus.StockReturn;
                stockDetail.RefferenceNumber = item.RefferenceNumber;

                var packagingStockInfo = _packagingLineStockInfoBusiness.GetPackagingLineStockInfoByPackagingAndItemAndModelId(item.ProductionLineId.Value, item.ItemId.Value, item.DescriptionId.Value, orgId);

                packagingStockInfo.StockOutQty += item.Quantity;
                _packagingLineStockInfoRepository.Update(packagingStockInfo);
                packagingLineStockDetails.Add(stockDetail);
            }
            _packagingLineStockDetailRepository.InsertAll(packagingLineStockDetails);
            return _packagingLineStockDetailRepository.Save();
        }
    }
}
