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
   public class RepairBusiness: IRepairBusiness
    {
        private readonly IConfigurationUnitOfWork _configurationDb; // database
        private readonly RepairRepository repairRepository; // repo
        public RepairBusiness(IConfigurationUnitOfWork configurationDb)
        {
            this._configurationDb = configurationDb;
            repairRepository = new RepairRepository(this._configurationDb);
        }

        public bool DeleteRepair(long id, long orgId)
        {
            repairRepository.DeleteOneByOrg(r => r.RepairId == id && r.OrganizationId == orgId);
            return repairRepository.Save();
        }

        public IEnumerable<Repair> GetAllRepairByOrgId(long orgId)
        {
            return repairRepository.GetAll(s => s.OrganizationId == orgId).ToList();
        }

        public Repair GetRepairOneByOrgId(long id, long orgId)
        {
            return repairRepository.GetOneByOrg(f => f.RepairId == id && f.OrganizationId == orgId);
        }

        public bool IsDuplicateRepairName(string code, long id, long orgId)
        {
            return repairRepository.GetOneByOrg(f => f.RepairCode == code && f.RepairId != id && f.OrganizationId == orgId) != null ? true : false;
        }

        public bool SaveRepair(RepairDTO repairDTO, long userId, long orgId)
        {
            Repair repair = new Repair();
            if (repairDTO.RepairId == 0)
            {
                repair.RepairName = repairDTO.RepairName;
                repair.RepairCode = repairDTO.RepairCode;
                repair.Remarks = repairDTO.Remarks;
                repair.OrganizationId = orgId;
                repair.EUserId = userId;
                repair.EntryDate = DateTime.Now;
                repairRepository.Insert(repair);
            }
            else
            {
                repair = GetRepairOneByOrgId(repairDTO.RepairId, orgId);
                repair.RepairName = repairDTO.RepairName;
                repair.RepairCode = repairDTO.RepairCode;
                repair.Remarks = repairDTO.Remarks;
                repair.OrganizationId = orgId;
                repair.UpUserId = userId;
                repair.UpdateDate = DateTime.Now;
                repairRepository.Update(repair);
            }
            return repairRepository.Save();
        }
    }
}
