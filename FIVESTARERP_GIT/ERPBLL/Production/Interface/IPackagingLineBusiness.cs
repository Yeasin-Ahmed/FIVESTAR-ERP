using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IPackagingLineBusiness
    {
        IEnumerable<PackagingLine> GetPackagingLinesByOrgId(long orgId);
        PackagingLine GetPackagingLineById(long id, long orgId);
        bool SavePackagingLine(PackagingLineDTO dto, long userId, long orgId);
    }
}
