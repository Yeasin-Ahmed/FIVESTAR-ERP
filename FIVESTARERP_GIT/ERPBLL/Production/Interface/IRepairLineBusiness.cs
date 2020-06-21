using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IRepairLineBusiness
    {
        IEnumerable<RepairLine> GetRepairLinesByOrgId(long orgId);
        RepairLine GetRepairLineById(long id, long orgId);
        bool SaveRepairLine(RepairLineDTO dto, long userId, long orgId);
    }
}
