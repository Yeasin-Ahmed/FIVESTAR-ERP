using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration.Interface
{
   public interface IFaultBusiness
    {
        IEnumerable<Fault> GetAllFaultByOrgId(long orgId);
        bool SaveFault(FaultDTO faultDTO, long userId, long orgId);
        bool IsDuplicateFaultName(string name, long id, long orgId);
        Fault GetFaultOneByOrgId(long id, long orgId);
        bool DeleteFault(long id, long orgId);
    }
}
