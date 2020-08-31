using ERPBO.Inventory.DTOModel;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IGeneratedIMEIBusiness
    {
        IEnumerable<GeneratedIMEIDTO> GetGeneratedIMEIDTOs(long userId, long orgId);
        bool SaveGeneratedIMEI(List<GeneratedIMEIDTO> model, long userId, long orgId);

        bool SaveGeneratedIMEIByUser(CommitGeneratedIMEIDTO model, long userId, long orgId);
    }
}
