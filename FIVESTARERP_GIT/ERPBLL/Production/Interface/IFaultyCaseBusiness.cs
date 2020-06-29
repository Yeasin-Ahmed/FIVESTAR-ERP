using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IFaultyCaseBusiness
    {
        IEnumerable<FaultyCase> GetFaultyCases(long orgId);
        FaultyCase GetFaultyById(long faultyId, long orgId);
        bool SaveFaultyCase(FaultyCaseDTO faulty, long userId,long orgId);
    }
}
