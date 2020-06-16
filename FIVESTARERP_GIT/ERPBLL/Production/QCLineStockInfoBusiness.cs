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
    public class QCLineStockInfoBusiness : IQCLineStockInfoBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly QualityControlLineStockInfoRepository _qualityControlLineStockInfoRepository;

        public QCLineStockInfoBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._qualityControlLineStockInfoRepository = new QualityControlLineStockInfoRepository(this._productionDb);
        }

        public QualityControlLineStockInfo GetQCLineStockInfoByQCAndItemAndModelId(long qcId, long itemId, long modelId, long orgId)
        {
            return _qualityControlLineStockInfoRepository.GetOneByOrg(s => s.OrganizationId == orgId && s.QCLineId == qcId && s.ItemId == itemId && s.DescriptionId == modelId);
        }

        public IEnumerable<QualityControlLineStockInfo> GetQCLineStockInfoByQCAndItemId(long qcId, long itemId, long orgId)
        {
            return _qualityControlLineStockInfoRepository.GetAll(s => s.OrganizationId == orgId && s.QCLineId == qcId && s.ItemId == itemId);
        }

        public IEnumerable<QualityControlLineStockInfo> GetQCLineStockInfos(long orgId)
        {
            return _qualityControlLineStockInfoRepository.GetAll(s => s.OrganizationId == orgId);
        }

    }
}
