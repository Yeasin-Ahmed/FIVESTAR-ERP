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
    public class ProductionAssembleStockDetailBusiness : IProductionAssembleStockDetailBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly IProductionAssembleStockInfoBusiness _productionAssembleStockInfoBusiness;
        private readonly ProductionAssembleStockDetailRepository _productionAssembleStockDetailRepository;
        private readonly ProductionAssembleStockInfoRepository _productionAssembleStockInfoRepository;
        private readonly IQCPassTransferInformationBusiness _qCPassTransferInformationBusiness;
        private readonly QCPassTransferInformationRepository _qCPassTransferInformationRepository;

        public ProductionAssembleStockDetailBusiness(IProductionUnitOfWork productionDb, IProductionAssembleStockInfoBusiness productionAssembleStockInfoBusiness, IQCPassTransferInformationBusiness qCPassTransferInformationBusiness)
        {
            this._productionDb = productionDb;
            this._productionAssembleStockDetailRepository = new ProductionAssembleStockDetailRepository(this._productionDb);
            this._productionAssembleStockInfoBusiness = productionAssembleStockInfoBusiness;
            this._productionAssembleStockInfoRepository = new ProductionAssembleStockInfoRepository(this._productionDb);
            this._qCPassTransferInformationBusiness = qCPassTransferInformationBusiness;
            this._qCPassTransferInformationRepository = new QCPassTransferInformationRepository(this._productionDb);
        }
        public IEnumerable<ProductionAssembleStockDetail> GetProductionAssembleStockDetails(long orgId)
        {
            return _productionAssembleStockDetailRepository.GetAll(s => s.OrganizationId == orgId);
        }
        public bool SaveProductionAssembleStockDetailStockIn(List<ProductionAssembleStockDetailDTO> stockDetailDTOs, long userId, long orgId)
        {
            List<ProductionAssembleStockDetail> stockDetails = new List<ProductionAssembleStockDetail>();

            foreach (var item in stockDetailDTOs)
            {
                ProductionAssembleStockDetail stockDetail = new ProductionAssembleStockDetail()
                {
                    ProductionFloorId = item.ProductionFloorId,
                    DescriptionId = item.DescriptionId,
                    WarehouseId = item.WarehouseId,
                    ItemTypeId = item.ItemTypeId,
                    ItemId = item.ItemId,
                    PackagingLineId= item.PackagingLineId,
                    PackagingLineName = item.PackagingLineName,
                    QCLineId = item.QCLineId,
                    QCLineName = item.QCLineName,
                    UnitId = item.UnitId,
                    UnitName = item.UnitName,
                    OrganizationId = orgId,
                    RefferenceNumber = item.RefferenceNumber,
                    Quantity = item.Quantity,
                    StockStatus = StockStatus.StockIn,
                    Remarks = "",
                    EntryDate = DateTime.Now,
                    EUserId = userId
                };
                stockDetails.Add(stockDetail);

                var info = _productionAssembleStockInfoBusiness.productionAssembleStockInfoByFloorAndModelAndItem(item.ProductionFloorId, item.DescriptionId, item.ItemId, orgId);

                if (info != null)
                {
                    info.StockInQty += item.Quantity;
                    info.UpUserId = userId;
                    info.UpdateDate = DateTime.Now;
                    this._productionAssembleStockInfoRepository.Update(info);
                }
                else
                {
                    ProductionAssembleStockInfo newInfo = new ProductionAssembleStockInfo()
                    {
                        ProductionFloorId = item.ProductionFloorId,
                        DescriptionId = item.DescriptionId,
                        WarehouseId = item.WarehouseId,
                        ItemTypeId = item.ItemTypeId,
                        ItemId = item.ItemId,
                        UnitId = item.UnitId,
                        OrganizationId = orgId,
                        EntryDate = DateTime.Now,
                        EUserId = userId,
                        StockInQty = item.Quantity,
                        Remarks = "Faulty Stock In By Repair Section"
                    };
                    this._productionAssembleStockInfoRepository.Insert(newInfo);
                }
            }
            this._productionAssembleStockDetailRepository.InsertAll(stockDetails);
            return this._productionAssembleStockDetailRepository.Save();

        }
        public bool SaveProductionAssembleStockDetailStockOut(List<ProductionAssembleStockDetailDTO> stockDetailDTOs, long userId, long orgId)
        {
            List<ProductionAssembleStockDetail> stockDetails = new List<ProductionAssembleStockDetail>();

            foreach (var item in stockDetailDTOs)
            {
                ProductionAssembleStockDetail stockDetail = new ProductionAssembleStockDetail()
                {
                    ProductionFloorId = item.ProductionFloorId,
                    DescriptionId = item.DescriptionId,
                    WarehouseId = item.WarehouseId,
                    ItemTypeId = item.ItemTypeId,
                    ItemId = item.ItemId,
                    PackagingLineId = item.PackagingLineId,
                    PackagingLineName = item.PackagingLineName,
                    QCLineId = item.QCLineId,
                    QCLineName = item.QCLineName,
                    UnitId = item.UnitId,
                    UnitName = item.UnitName,
                    OrganizationId = orgId,
                    RefferenceNumber = item.RefferenceNumber,
                    Quantity = item.Quantity,
                    StockStatus = StockStatus.StockOut,
                    Remarks = "",
                    EntryDate = DateTime.Now,
                    EUserId = userId
                };
                stockDetails.Add(stockDetail);

                var info = _productionAssembleStockInfoBusiness.productionAssembleStockInfoByFloorAndModelAndItem(item.ProductionFloorId, item.DescriptionId, item.ItemId, orgId);

                    info.StockOutQty += item.Quantity;
                    info.UpUserId = userId;
                    info.UpdateDate = DateTime.Now;
                    this._productionAssembleStockInfoRepository.Update(info);
                
            }
            this._productionAssembleStockDetailRepository.InsertAll(stockDetails);
            return this._productionAssembleStockDetailRepository.Save();
        }

        public bool SaveReceiveQCItems(long qcPassId, string status,long userId,long orgId)
        {
            var info = _qCPassTransferInformationBusiness.GetQCPassTransferInformationById(qcPassId, orgId);
            if(info.StateStatus == "Send By QC")
            {
                info.StateStatus = status;
                info.UpUserId = userId;
                info.UpdateDate = DateTime.Now;
                _qCPassTransferInformationRepository.Update(info);
                List<ProductionAssembleStockDetailDTO> stockDetails = new List<ProductionAssembleStockDetailDTO>()
                {
                    new ProductionAssembleStockDetailDTO {ProductionFloorId= info.ProductionFloorId,WarehouseId=info.WarehouseId,QCLineId=info.QCLineId,DescriptionId=info.DescriptionId,ItemTypeId=info.ItemTypeId, ItemId =info.ItemId,Quantity=info.Quantity,StockStatus=StockStatus.StockIn,OrganizationId=orgId,EUserId=userId,UnitId=info.UnitId,RefferenceNumber="",Remarks="Stock In By QC Pass" }
                };
                if (_qCPassTransferInformationRepository.Save())
                {
                    return SaveProductionAssembleStockDetailStockIn(stockDetails, userId, orgId);
                }
            }
            return false;
        }

    }
}
