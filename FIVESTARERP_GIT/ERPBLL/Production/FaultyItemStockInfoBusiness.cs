using ERPBLL.Production.Interface;
using ERPBO.Production.DomainModels;
using ERPDAL.ProductionDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production
{
    public class FaultyItemStockInfoBusiness : IFaultyItemStockInfoBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly FaultyItemStockInfoRepository _faultyItemStockInfoRepository;
        public FaultyItemStockInfoBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._faultyItemStockInfoRepository = new FaultyItemStockInfoRepository(this._productionDb);
        }
        public IEnumerable<FaultyItemStockInfo> GetFaultyItemByQCandRepair(long qcId, long repairLine, long orgId)
        {
            return this._faultyItemStockInfoRepository.GetAll(f => f.QCId == qcId && f.RepairLineId == repairLine && f.OrganizationId == orgId);
        }
        public FaultyItemStockInfo GetFaultyItemStockInfoByQCandRepairandItem(long qcId, long repairLine, long itemId, long orgId)
        {
            return this._faultyItemStockInfoRepository.GetOneByOrg(f => f.QCId== qcId && f.RepairLineId == repairLine && f.ItemId == itemId && f.OrganizationId == orgId);
        }

        public FaultyItemStockInfo GetFaultyItemStockInfoByRepairAndItem(long repairLine, long itemId, long orgId)
        {
            return this._faultyItemStockInfoRepository.GetOneByOrg(f => f.RepairLineId == repairLine && f.ItemId == itemId && f.OrganizationId == orgId);
        }

        public FaultyItemStockInfo GetFaultyItemStockInfoByRepairAndModelAndItem(long repairLine, long modelId, long itemId, long orgId)
        {
            return this._faultyItemStockInfoRepository.GetOneByOrg(f => f.RepairLineId == repairLine && f.DescriptionId == modelId && f.ItemId == itemId && f.OrganizationId == orgId);
        }

        public IEnumerable<FaultyItemStockInfo> GetFaultyItemStockInfos(long orgId)
        {
            return this._faultyItemStockInfoRepository.GetAll(f => f.OrganizationId == orgId);
        }


    }
}
