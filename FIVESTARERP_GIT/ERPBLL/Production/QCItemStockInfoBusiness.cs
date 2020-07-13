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
    public class QCItemStockInfoBusiness : IQCItemStockInfoBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly QCItemStockInfoRepository _qCItemStockInfoRepository;
        public QCItemStockInfoBusiness(IProductionUnitOfWork productionDb, QCItemStockInfoRepository qCItemStockInfoRepository)
        {
            this._productionDb = productionDb;
            this._qCItemStockInfoRepository = new QCItemStockInfoRepository(this._productionDb);
        }
        public QCItemStockInfo GetQCItemStockInfById(long qcId, long modelId, long itemId, long orgId)
        {
            return _qCItemStockInfoRepository.GetOneByOrg(d=> d.QCId== qcId && d.DescriptionId == modelId && d.ItemId == itemId && d.OrganizationId == orgId);
        }

        public QCItemStockInfo GetQCItemStockInfoByFloorAndQcAndModelAndItem(long floorId, long qcId, long modelId, long itemId, long orgId)
        {
            return _qCItemStockInfoRepository.GetOneByOrg(d =>d.ProductionFloorId == floorId && d.QCId == qcId && d.DescriptionId == modelId && d.ItemId == itemId && d.OrganizationId == orgId);
        }

        public IEnumerable<QCItemStockInfo> GetQCItemStocks(long orgId)
        {
            return _qCItemStockInfoRepository.GetAll(d => d.OrganizationId == orgId);
        }
    }
}
