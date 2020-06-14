using ERPBLL.Configuration.Interface;
using ERPBO.Configuration.DomainModels;
using ERPDAL.ConfigurationDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration
{
   public class MobilePartStockInfoBusiness: IMobilePartStockInfoBusiness
    {
        private readonly IConfigurationUnitOfWork _configurationDb; // database
        private readonly MobilePartStockInfoRepository mobilePartStockInfoRepository; // repo
        public MobilePartStockInfoBusiness(IConfigurationUnitOfWork configurationDb)
        {
            this._configurationDb = configurationDb;
            mobilePartStockInfoRepository = new MobilePartStockInfoRepository(this._configurationDb);
        }

        public IEnumerable<MobilePartStockInfo> GetAllMobilePartStockInfoByOrgId(long orgId,long branchId)
        {
            return mobilePartStockInfoRepository.GetAll(info => info.OrganizationId == orgId && info.BranchId==branchId).ToList();
        }
    }
}
