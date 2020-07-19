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
    public class RepairLineStockInfoBusiness : IRepairLineStockInfoBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly RepairLineStockInfoRepository _repairLineStockInfoRepository;

        public RepairLineStockInfoBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._repairLineStockInfoRepository = new RepairLineStockInfoRepository(this._productionDb);
        }
        public RepairLineStockInfo GetRepairLineStockInfoByRepairAndItemAndModelId(long repairId, long itemId, long modelId, long orgId)
        {
            return _repairLineStockInfoRepository.GetOneByOrg(s=> s.RepairLineId == repairId && s.ItemId == itemId && s.DescriptionId == modelId && s.OrganizationId == orgId);
        }

        public async Task<RepairLineStockInfo> GetRepairLineStockInfoByRepairAndItemAndModelIdAsync(long repairId, long itemId, long modelId, long orgId)
        {
            return await _repairLineStockInfoRepository.GetOneByOrgAsync(s => s.RepairLineId == repairId && s.ItemId == itemId && s.DescriptionId == modelId && s.OrganizationId == orgId);
        }

        public IEnumerable<RepairLineStockInfo> GetRepairLineStockInfoByRepairAndItemId(long repairId, long itemId, long orgId)
        {
            return _repairLineStockInfoRepository.GetAll(s => s.RepairLineId == repairId && s.ItemId == itemId && s.OrganizationId == orgId);
        }

        public RepairLineStockInfo GetRepairLineStockInfoByRepairQCAndItemAndModelId(long repairId, long itemId, long qcId, long modelId, long orgId)
        {
            return _repairLineStockInfoRepository.GetOneByOrg(s => s.RepairLineId == repairId && s.ItemId == itemId && s.DescriptionId == modelId && s.OrganizationId == orgId && s.QCLineId == qcId);
        }

        public IEnumerable<RepairLineStockInfo> GetRepairLineStockInfos(long orgId)
        {
            return _repairLineStockInfoRepository.GetAll(s => s.OrganizationId == orgId);
        }
    }
}
