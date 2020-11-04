using ERPBO.SalesAndDistribution.DomainModels;
using ERPBO.SalesAndDistribution.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.SalesAndDistribution.Interface
{
    public interface IDivisionBusiness
    {
        IEnumerable<Division> GetDivisions(long orgId);
        Division GetDivisionById(long divisionId, long orgId);
        bool SaveDivision(DivisionDTO dto, long userId, long orgId);
    }
}
