using ERPBO.Inventory.DomainModels;
using ERPBO.Inventory.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Inventory.Interface
{
    public interface IIQCBusiness
    {
        IEnumerable<IQC> GetAllIQCByOrgId(long orgId);
        bool SaveIQC(IQCDTO iQCDTO, long userId, long orgId);
        IQC GetIQCOneByOrgId(long id, long orgId);
        bool IsDuplicateIQCName(long id, long orgId, string iqcName);
    }
}
