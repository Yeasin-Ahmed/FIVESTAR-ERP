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
    public class AssemblyLineStockDetailBusiness : IAssemblyLineStockDetailBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly AssemblyLineStockDetailRepository _assemblyLineStockDetailRepository;
        private readonly AssemblyLineStockInfoRepository _assemblyLineStockInfoRepository;
        private readonly IAssemblyLineStockInfoBusiness _assemblyLineStockInfoBusiness;

        public AssemblyLineStockDetailBusiness(IProductionUnitOfWork productionDb, IAssemblyLineStockInfoBusiness assemblyLineStockInfoBusiness)
        {
            this._productionDb = productionDb;
            this._assemblyLineStockInfoRepository = new AssemblyLineStockInfoRepository(this._productionDb);
            this._assemblyLineStockDetailRepository = new AssemblyLineStockDetailRepository(this._productionDb);
            this._assemblyLineStockInfoBusiness = assemblyLineStockInfoBusiness;
        }

        public IEnumerable<AssemblyLineStockDetail> GetAssemblyLineStockDetails(long orgId)
        {
            throw new NotImplementedException();
        }

        public bool SaveAssemblyLineStockIn(List<AssemblyLineStockDetailDTO> assemblyLineStockDetailDTO, long userId, long orgId)
        {
            List<AssemblyLineStockDetail> assemblyLineStockDetail = new List<AssemblyLineStockDetail>();
            foreach (var item in assemblyLineStockDetailDTO)
            {
                AssemblyLineStockDetail stockDetail = new AssemblyLineStockDetail();
                stockDetail.AssemblyLineId = item.AssemblyLineId;
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

                var assemblyStockInfo = _assemblyLineStockInfoBusiness.GetAssemblyLineStockInfos(orgId).Where(o => o.ItemTypeId == item.ItemTypeId && o.ItemId == item.ItemId && o.ProductionLineId == item.ProductionLineId && o.DescriptionId == item.DescriptionId && o.AssemblyLineId == item.AssemblyLineId).FirstOrDefault();
                if (assemblyStockInfo != null)
                {
                    assemblyStockInfo.StockInQty += item.Quantity;
                    _assemblyLineStockInfoRepository.Update(assemblyStockInfo);
                }
                else
                {
                    AssemblyLineStockInfo info = new AssemblyLineStockInfo();
                    info.AssemblyLineId = item.AssemblyLineId;
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
                    
                    _assemblyLineStockInfoRepository.Insert(info);
                }
                assemblyLineStockDetail.Add(stockDetail);
            }
            _assemblyLineStockDetailRepository.InsertAll(assemblyLineStockDetail);
            return _assemblyLineStockDetailRepository.Save();
        }

        public bool SaveAssemblyLineStockOut(List<AssemblyLineStockDetailDTO> assemblyLineStockDetailDTO, long userId, long orgId, string flag)
        {
            List<AssemblyLineStockDetail> assemblyLineStockDetail = new List<AssemblyLineStockDetail>();
            foreach (var item in assemblyLineStockDetailDTO)
            {
                AssemblyLineStockDetail stockDetail = new AssemblyLineStockDetail();
                stockDetail.AssemblyLineId = item.AssemblyLineId;
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

                var assemblyStockInfo = _assemblyLineStockInfoBusiness.GetAssemblyLineStockInfos(orgId).Where(o => o.ItemTypeId == item.ItemTypeId && o.ItemId == item.ItemId && o.ProductionLineId == item.ProductionLineId && o.DescriptionId == item.DescriptionId && o.AssemblyLineId == item.AssemblyLineId).FirstOrDefault();
                assemblyStockInfo.StockOutQty += item.Quantity;
                _assemblyLineStockInfoRepository.Update(assemblyStockInfo);
                assemblyLineStockDetail.Add(stockDetail);
            }
            _assemblyLineStockDetailRepository.InsertAll(assemblyLineStockDetail);
            return _assemblyLineStockDetailRepository.Save();
        }

    }
}
