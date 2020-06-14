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
    public class AssemblyLineStockInfoBusiness : IAssemblyLineStockInfoBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly AssemblyLineStockInfoRepository _assemblyLineStockInfoRepository;

        public AssemblyLineStockInfoBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._assemblyLineStockInfoRepository = new AssemblyLineStockInfoRepository(this._productionDb);
        }

        public AssemblyLineStockInfo GetAssemblyLineStockInfoByAssemblyAndItemAndModelId(long assemblyId, long itemId, long modelId, long orgId)
        {
            return _assemblyLineStockInfoRepository.GetOneByOrg(s => s.OrganizationId == orgId && s.AssemblyLineId == assemblyId && s.ItemId == itemId && s.DescriptionId == modelId);
        }

        public IEnumerable<AssemblyLineStockInfo> GetAssemblyLineStockInfoByAssemblyAndItemId(long assemblyId, long itemId, long orgId)
        {
            return _assemblyLineStockInfoRepository.GetAll(s => s.OrganizationId == orgId && s.AssemblyLineId == assemblyId && s.ItemId == itemId);
        }

        public IEnumerable<AssemblyLineStockInfo> GetAssemblyLineStockInfos(long orgId)
        {
            return _assemblyLineStockInfoRepository.GetAll(s => s.OrganizationId == orgId);
        }


    }
}
