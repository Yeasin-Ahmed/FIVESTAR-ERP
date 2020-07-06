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
   public class RepairBusiness: IRepairBusiness
    {
        private readonly IConfigurationUnitOfWork _configurationDb; // database
        private readonly RepairRepository repairRepository; // repo
        public RepairBusiness(IConfigurationUnitOfWork configurationDb)
        {
            this._configurationDb = configurationDb;
            repairRepository = new RepairRepository(this._configurationDb);
        }

        public IEnumerable<Repair> GetAllRepairByOrgId(long orgId)
        {
            return repairRepository.GetAll(s => s.OrganizationId == orgId).ToList();
        }

        public Repair GetRepairOneByOrgId(long id, long orgId)
        {
            return repairRepository.GetOneByOrg(f => f.RepairId == id && f.OrganizationId == orgId);
        }
    }
}
