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
    public class RepairItemStockInfoBusiness : IRepairItemStockInfoBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly RepairItemStockInfoRepository _repairItemStockInfoRepository;
        public RepairItemStockInfoBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._repairItemStockInfoRepository = new RepairItemStockInfoRepository(this._productionDb);
        }

        public RepairItemStockInfo GetRepairItem(long qcId, long repairLineId, long modelId, long itemId, long orgId)
        {
            return _repairItemStockInfoRepository.GetOneByOrg(d => d.QCId == qcId && d.RepairLineId == repairLineId && d.DescriptionId == modelId && d.ItemId == itemId && d.OrganizationId == orgId);
        }

        public async Task<RepairItemStockInfo> GetRepairItemAsync(long qcId, long repairLineId, long modelId, long itemId, long orgId)
        {
            return await _repairItemStockInfoRepository.GetOneByOrgAsync(d => d.QCId == qcId && d.RepairLineId == repairLineId && d.DescriptionId == modelId && d.ItemId == itemId && d.OrganizationId == orgId);
        }

        public IEnumerable<RepairItemStockInfo> GetRepairItemStockInfById(long repairLineId, long modelId, long itemId, long orgId)
        {
            return _repairItemStockInfoRepository.GetAll(d => d.RepairLineId == repairLineId && d.DescriptionId == modelId && d.ItemId == itemId && d.OrganizationId == orgId);
        }

        public IEnumerable<RepairItemStockInfo> GetRepairItemStockInfoByQC(long qcId, long modelId, long itemId, long orgId)
        {
            return _repairItemStockInfoRepository.GetAll(d => d.QCId == qcId && d.DescriptionId == modelId && d.ItemId == itemId && d.OrganizationId == orgId);
        }

        public IEnumerable<RepairItemStockInfo> GetRepairItemStocks(long orgId)
        {
            return _repairItemStockInfoRepository.GetAll(d => d.OrganizationId == orgId);
        }
    }
}
