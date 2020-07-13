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

        public MobilePartStockInfo GetAllMobilePartStockById(long orgId, long branchId)
        {
            return mobilePartStockInfoRepository.GetOneByOrg(info => info.OrganizationId == orgId && info.BranchId == branchId);
        }

        public IEnumerable<MobilePartStockInfo> GetAllMobilePartStockByParts(long warehouseId, long partsId, long orgId, long branchId)
        {
            return mobilePartStockInfoRepository.GetAll(info => info.SWarehouseId == warehouseId && info.MobilePartId == partsId && info.OrganizationId == orgId && info.BranchId == branchId).ToList();
        }

        public IEnumerable<MobilePartStockInfo> GetAllMobilePartStockInfoById(long orgId, long branchId)
        {
            return mobilePartStockInfoRepository.GetAll(info => info.OrganizationId == orgId && info.BranchId == branchId).ToList();
        }

        public MobilePartStockInfo GetAllMobilePartStockInfoByInfoId(long warehouseId, long partsId, double cprice, long orgId, long branchId)
        {
            return mobilePartStockInfoRepository.GetOneByOrg(info =>info.SWarehouseId==warehouseId && info.MobilePartId==partsId && info.CostPrice == cprice && info.OrganizationId == orgId && info.BranchId == branchId);
        }

        public IEnumerable<MobilePartStockInfo> GetAllMobilePartStockInfoByOrgId(long orgId,long branchId)
        {
            var  data = mobilePartStockInfoRepository.GetAll(info => info.OrganizationId == orgId && info.BranchId == branchId).ToList();
            return data;
        }

        public MobilePartStockInfo GetAllMobilePartStockInfoBySellPrice(long warehouseId, long partsId, double sprice, long orgId, long branchId)
        {
            return mobilePartStockInfoRepository.GetOneByOrg(info => info.SWarehouseId == warehouseId && info.MobilePartId == partsId && info.SellPrice == sprice && info.OrganizationId == orgId && info.BranchId == branchId);
        }
    }
}
