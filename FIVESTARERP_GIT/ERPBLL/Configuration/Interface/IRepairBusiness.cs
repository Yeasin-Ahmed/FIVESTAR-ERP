using ERPBO.Configuration.DomainModels;
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
    }
}
