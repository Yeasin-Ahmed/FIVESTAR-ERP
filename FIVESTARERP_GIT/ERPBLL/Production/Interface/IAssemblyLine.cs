using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IAssemblyLineBusiness
    {
        IEnumerable<AssemblyLine> GetAssemblyLines(long orgId);
        AssemblyLine GetAssemblyLineById(long id,long orgId);
        bool SaveAssembly(AssemblyLineDTO line, long userId, long orgId);
    }
}
