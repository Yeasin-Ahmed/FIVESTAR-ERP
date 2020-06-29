using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IRepairItemBusiness
    {
        bool SaveRepairItem(RepairItemDTO repairItemDTO, long userId, long orgId);
    }
}
