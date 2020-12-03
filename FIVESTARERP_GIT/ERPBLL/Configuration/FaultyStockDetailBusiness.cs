using ERPBLL.Common;
using ERPBLL.Configuration.Interface;
using ERPBO.Configuration.DomainModels;
using ERPBO.FrontDesk.DTOModels;
using ERPDAL.ConfigurationDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration
{
    public class FaultyStockDetailBusiness : IFaultyStockDetailBusiness
    {
        private readonly IConfigurationUnitOfWork _configurationDb;
        private readonly IFaultyStockInfoBusiness _faultyStockInfoBusiness;
        private readonly FaultyStockDetailRepository _faultyStockDetailRepository;
        private readonly FaultyStockInfoRepository _faultyStockInfoRepository;
        public FaultyStockDetailBusiness(IConfigurationUnitOfWork configurationDb, IFaultyStockInfoBusiness faultyStockInfoBusiness)
        {
            this._configurationDb = configurationDb;
            this._faultyStockInfoBusiness = faultyStockInfoBusiness;
            _faultyStockInfoRepository = new FaultyStockInfoRepository(this._configurationDb);
            _faultyStockDetailRepository = new FaultyStockDetailRepository(this._configurationDb);
        }

        public bool SaveFaultyStockIn(List<FaultyStockDetailsDTO> faultyStocksDto, long userId, long orgId, long branchId)
        {
            List<FaultyStockDetails> faultyStockDetails = new List<FaultyStockDetails>();
            FaultyStockDetails faultyStock = new FaultyStockDetails();
            FaultyStockInfo faultyInfo = new FaultyStockInfo();
            foreach (var item in faultyStocksDto)
            {
                faultyStock = new FaultyStockDetails()
                {
                    BranchId = branchId,
                    CostPrice = item.CostPrice,
                    SellPrice = item.SellPrice,
                    StateStatus = StockStatus.StockIn,
                    SWarehouseId = item.SWarehouseId,
                    EUserId = userId,
                    OrganizationId = orgId,
                    EntryDate = DateTime.Now,
                    JobOrderId = item.JobOrderId,
                    DescriptionId = item.DescriptionId,
                    PartsId = item.PartsId,
                    Quantity = item.Quantity,
                    Remarks = "",
                    TSId = item.TSId,
                    
                };
                var faultyStockInfo = _faultyStockInfoBusiness.GetAllFaultyStockInfoByModelAndPartsIdAndCostPrice(item.DescriptionId,item.PartsId.Value,item.CostPrice,orgId,branchId);
                if (faultyStockInfo != null)
                {
                    faultyStockInfo.StockInQty += item.Quantity;
                    faultyStockInfo.UpUserId = userId;
                    faultyStockInfo.UpdateDate = DateTime.Now;
                   _faultyStockInfoRepository.Update(faultyStockInfo);
                    //FaultyStockInfoId//
                    faultyStock.FaultyStockInfoId = faultyStockInfo.FaultyStockInfoId;
                }
                else
                {
                    faultyInfo = new FaultyStockInfo()
                    {
                        BranchId = branchId,
                        CostPrice = item.CostPrice,
                        SellPrice = item.SellPrice,
                        SWarehouseId = item.SWarehouseId,
                        EUserId = userId,
                        OrganizationId = orgId,
                        EntryDate = DateTime.Now,
                        JobOrderId = item.JobOrderId,
                        DescriptionId = item.DescriptionId,
                        PartsId = item.PartsId,
                        StockInQty = item.Quantity,
                        StockOutQty = 0,
                        Remarks = "",                        
                    };
                    _faultyStockInfoRepository.Insert(faultyInfo);
                    if (_faultyStockInfoRepository.Save())
                    {
                        faultyStock.FaultyStockInfoId = faultyInfo.FaultyStockInfoId;
                    }
                }
                faultyStockDetails.Add(faultyStock);
            }
            
            _faultyStockDetailRepository.InsertAll(faultyStockDetails);
            return _faultyStockDetailRepository.Save();
        }
    }
}
