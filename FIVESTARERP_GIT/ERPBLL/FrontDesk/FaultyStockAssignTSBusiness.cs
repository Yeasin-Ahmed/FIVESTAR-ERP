using ERPBLL.Common;
using ERPBLL.Configuration.Interface;
using ERPBLL.FrontDesk.Interface;
using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using ERPBO.FrontDesk.DomainModels;
using ERPBO.FrontDesk.DTOModels;
using ERPDAL.ConfigurationDAL;
using ERPDAL.FrontDeskDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.FrontDesk
{
    public class FaultyStockAssignTSBusiness : IFaultyStockAssignTSBusiness
    {
        private readonly IFrontDeskUnitOfWork _fronDeskDb;
        private readonly IConfigurationUnitOfWork _configDb;
        private readonly IFaultyStockInfoBusiness _faultyStockInfoBusiness;
        private readonly FaultyStockInfoRepository _faultyStockInfoRepository;
        private readonly FaultyStockDetailRepository _faultyStockDetailRepository;
        private readonly FaultyStockAssignTSRepository _faultyStockAssignTSRepository;
        private readonly IMobilePartStockDetailBusiness _mobilePartStockDetailBusiness;
        private readonly IScrapStockDetailBusiness _scrapStockDetailBusiness;
        public FaultyStockAssignTSBusiness(IFrontDeskUnitOfWork fronDeskDb, IFaultyStockInfoBusiness faultyStockInfoBusiness, IConfigurationUnitOfWork configDb, IMobilePartStockDetailBusiness mobilePartStockDetailBusiness, IScrapStockDetailBusiness scrapStockDetailBusiness)
        {
            this._fronDeskDb = fronDeskDb;
            this._configDb = configDb;
            this._faultyStockInfoBusiness = faultyStockInfoBusiness;
            this._faultyStockInfoRepository = new FaultyStockInfoRepository(this._configDb);
            this._faultyStockDetailRepository = new FaultyStockDetailRepository(this._configDb);
            this._faultyStockAssignTSRepository = new FaultyStockAssignTSRepository(this._fronDeskDb);
            this._mobilePartStockDetailBusiness = mobilePartStockDetailBusiness;
            this._scrapStockDetailBusiness = scrapStockDetailBusiness;
        }
        public bool SaveFaultyStockAssignTS(long ts, long[] jobAssign, long userId, long orgId, long branchId)
        {
            List<FaultyStockAssignTS> faultyStockAssignTs = new List<FaultyStockAssignTS>();
            List<FaultyStockDetails> faultyStockDetails = new List<FaultyStockDetails>();
            foreach (var item in jobAssign)
            {
                var stockInfo = _faultyStockInfoBusiness.GetStockInfoOneById(item, orgId);

                FaultyStockAssignTS assignTS = new FaultyStockAssignTS()
                {
                    BranchId = branchId,
                    CostPrice = stockInfo.CostPrice,
                    SellPrice = stockInfo.SellPrice,
                    DescriptionId = stockInfo.DescriptionId,
                    TSId = ts,
                    FaultyStockInfoId = stockInfo.FaultyStockInfoId,
                    Quantity = stockInfo.StockInQty - stockInfo.StockOutQty,
                    PartsId = stockInfo.PartsId,
                    OrganizationId = orgId,
                    EntryDate = DateTime.Now,
                    EUserId = userId,
                    StateStatus = "Send",
                    RepairedQuantity = 0,
                    ScrapQuantity = 0,
                };
                faultyStockAssignTs.Add(assignTS);

                FaultyStockDetails stockDetails = new FaultyStockDetails()
                {
                    BranchId = branchId,
                    CostPrice = stockInfo.CostPrice,
                    SellPrice = stockInfo.SellPrice,
                    DescriptionId = stockInfo.DescriptionId,
                    TSId = ts,
                    StateStatus = StockStatus.StockOut,
                    SWarehouseId = stockInfo.SWarehouseId,
                    EntryDate = DateTime.Now,
                    EUserId = userId,
                    FaultyStockInfoId = stockInfo.FaultyStockInfoId,
                    JobOrderId = stockInfo.JobOrderId,
                    OrganizationId = orgId,
                    PartsId = stockInfo.PartsId,
                    Quantity = stockInfo.StockInQty - stockInfo.StockOutQty,
                    Remarks = "AssignToTS",
                };
                faultyStockDetails.Add(stockDetails);

                stockInfo.UpdateDate = DateTime.Now;
                stockInfo.UpUserId = userId;
                stockInfo.StockOutQty += stockInfo.StockInQty - stockInfo.StockOutQty;

                _faultyStockInfoRepository.Update(stockInfo);
            }
            _faultyStockDetailRepository.InsertAll(faultyStockDetails);
            if (_faultyStockDetailRepository.Save())
            {
                _faultyStockAssignTSRepository.InsertAll(faultyStockAssignTs);
            }
            return _faultyStockAssignTSRepository.Save();
        }
        public IEnumerable<FaultyStockAssignTSDTO> GetFaultyStockAssignTsByOrgId(long orgId, long branchId)
        {
            return this._fronDeskDb.Db.Database.SqlQuery<FaultyStockAssignTSDTO>(string.Format(@"Select fsa.FaultyStockAssignTSId,fsa.FaultyStockInfoId,fsa.DescriptionId,fsa.PartsId,fsa.TSId,fsa.Quantity,fsa.CostPrice,fsa.SellPrice,mp.MobilePartName,des.DescriptionName 'ModelName',fsa.StateStatus,fsa.RepairedQuantity,fsa.ScrapQuantity
From tblFaultyStockAssignTS fsa
Left Join [Inventory].dbo.tblDescriptions des on fsa.DescriptionId = des.DescriptionId and fsa.OrganizationId = des.OrganizationId
Left Join [Configuration].dbo.tblMobileParts mp on fsa.PartsId = mp.MobilePartId and fsa.OrganizationId = mp.OrganizationId
where fsa.OrganizationId={0} and fsa.BranchId={1}", orgId, branchId)).ToList();
        }
        public FaultyStockAssignTS GetFaultyStockAssignTsOneById(long id, long orgId, long branchId)
        {
            return _faultyStockAssignTSRepository.GetOneByOrg(s=> s.FaultyStockAssignTSId == id && s.OrganizationId == orgId && s.BranchId == branchId);
        }
        public bool SaveFaultyStockItemsByAssignTS(List<FaultyStockAssignTSDTO> dto,long userId, long orgId, long branchId)
        {
            foreach (var item in dto)
            {
                var assignTS = GetFaultyStockAssignTsOneById(item.FaultyStockAssignTSId, orgId, branchId);
                assignTS.StateStatus = "Received";
                assignTS.Quantity = 0;
                assignTS.RepairedQuantity = item.RepairedQuantity;
                assignTS.ScrapQuantity = item.ScrapQuantity;
                assignTS.UpdateDate = DateTime.Now;
                assignTS.UpUserId = userId;
                _faultyStockAssignTSRepository.Update(assignTS);
            }
            return _faultyStockAssignTSRepository.Save();
        }

        public bool SaveFaultyStockRepairedItems(List<FaultyStockAssignTSDTO> dto, long userId, long orgId, long branchId)
        {
            foreach (var item in dto)
            {
                var assignTS = GetFaultyStockAssignTsOneById(item.FaultyStockAssignTSId, orgId, branchId);
                assignTS.StateStatus = "Received"; 
                assignTS.Quantity = 0;
                assignTS.RepairedQuantity = item.RepairedQuantity;
                assignTS.ScrapQuantity = item.ScrapQuantity;
                assignTS.UpdateDate = DateTime.Now;
                assignTS.UpUserId = userId;
                _faultyStockAssignTSRepository.Update(assignTS);
            }
            return _faultyStockAssignTSRepository.Save();
        }
        public bool SaveFaultyStockRepairedItems(List<int> dto, long userId, long orgId, long branchId)
        {
            List<MobilePartStockDetailDTO> mobilePartStockDetails = new List<MobilePartStockDetailDTO>();
            List<ScrapStockDetailDTO> scrapStockDetails = new List<ScrapStockDetailDTO>();
            foreach (var item in dto)
            {
                var assignTS = GetFaultyStockAssignTsOneById(item, orgId, branchId);
                if (assignTS.RepairedQuantity > 0)
                {
                    MobilePartStockDetailDTO detail = new MobilePartStockDetailDTO()
                    {
                        BranchId = branchId,
                        CostPrice = assignTS.CostPrice,
                        SellPrice = assignTS.SellPrice,
                        DescriptionId = assignTS.DescriptionId,
                        EntryDate = DateTime.Now,
                        EUserId = userId,
                        MobilePartId = assignTS.PartsId,
                        OrganizationId = orgId,
                        Quantity = assignTS.RepairedQuantity,
                        StockStatus = StockStatus.StockIn,
                        Remarks = "Repaired-Done",
                    };
                    mobilePartStockDetails.Add(detail);
                }
                if (assignTS.ScrapQuantity > 0)
                {
                    ScrapStockDetailDTO scrapStock = new ScrapStockDetailDTO()
                    {
                        BranchId = branchId,
                        CostPrice = assignTS.CostPrice,
                        DescriptionId = assignTS.DescriptionId,
                        PartsId = assignTS.PartsId,
                        TSId = assignTS.TSId,
                        FaultyStockAssignTSId = assignTS.FaultyStockAssignTSId,
                        EntryDate = DateTime.Now,
                        EUserId = userId,
                        FaultyStockInfoId = assignTS.FaultyStockInfoId,
                        OrganizationId = orgId,
                        Quantity = assignTS.ScrapQuantity,
                        RepairedQuantity = assignTS.RepairedQuantity,
                        SellPrice = assignTS.SellPrice,
                        StockStatus = StockStatus.StockIn,
                    };
                    scrapStockDetails.Add(scrapStock);
                }

                assignTS.StateStatus = "Return-Received";
                assignTS.UpdateDate = DateTime.Now;
                assignTS.UpUserId = userId;
                _faultyStockAssignTSRepository.Update(assignTS);
            }
            if (mobilePartStockDetails.Count > 0)
            {
                _mobilePartStockDetailBusiness.SaveMobilePartStockIn(mobilePartStockDetails, userId, orgId, branchId);
            }
            if (scrapStockDetails.Count > 0)
            {
                _scrapStockDetailBusiness.SaveScrapStockIn(scrapStockDetails, userId, orgId, branchId);
            }
            return _faultyStockAssignTSRepository.Save();
        }
    }
}
