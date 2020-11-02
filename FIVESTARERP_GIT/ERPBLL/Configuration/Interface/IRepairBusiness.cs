using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration.Interface
{
   public interface IRepairBusiness
    {
        IEnumerable<Repair> GetAllRepairByOrgId(long orgId);
        Repair GetRepairOneByOrgId(long id, long orgId);
        bool SaveRepair(RepairDTO repairDTO, long userId, long orgId);
        bool IsDuplicateRepairName(string repairName, long id, long orgId);
        bool DeleteRepair(long id, long orgId);
    }
}
