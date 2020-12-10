using ERPBLL.Configuration.Interface;
using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using ERPDAL.ConfigurationDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration
{
    public class ScrapStockInfoBusiness : IScrapStockInfoBusiness
    {
        private readonly IConfigurationUnitOfWork _configurationDb;
        private readonly ScrapStockInfoRepository  _scrapStockInfoRepository;
        public ScrapStockInfoBusiness(IConfigurationUnitOfWork configurationDb)
        {
            this._configurationDb = configurationDb;
            _scrapStockInfoRepository = new ScrapStockInfoRepository(this._configurationDb);
        }
        public ScrapStockInfo GetScrapStockInfoByModelAndCostPriceAndSellPrice(long modelId, long partsId, double cPrice, double sPrice, long orgId, long branchId)
        {
            return _scrapStockInfoRepository.GetOneByOrg(s => s.DescriptionId == modelId && s.PartsId == partsId && s.CostPrice == cPrice && s.SellPrice == sPrice && s.OrganizationId == orgId && s.BranchId == branchId);
        }
        public IEnumerable<ScrapStockInfoDTO> GetScrapStockByOrgId(long orgId, long branchId)
        {
            return this._configurationDb.Db.Database.SqlQuery<ScrapStockInfoDTO>(string.Format(@"Select fsa.ScrapStockInfoId,fsa.DescriptionId,fsa.PartsId,fsa.CostPrice,fsa.SellPrice,mp.MobilePartName'PartsName',des.DescriptionName 'ModelName',fsa.ScrapQuantity
From tblScrapStockInfo fsa
Left Join [Inventory].dbo.tblDescriptions des on fsa.DescriptionId = des.DescriptionId and fsa.OrganizationId = des.OrganizationId
Left Join [Configuration].dbo.tblMobileParts mp on fsa.PartsId = mp.MobilePartId and fsa.OrganizationId = mp.OrganizationId
where fsa.OrganizationId={0} and fsa.BranchId={1}", orgId, branchId)).ToList();
        }
    }
}
