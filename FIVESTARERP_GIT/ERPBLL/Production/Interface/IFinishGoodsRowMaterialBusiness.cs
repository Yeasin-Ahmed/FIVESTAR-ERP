using ERPBO.Production.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IFinishGoodsRowMaterialBusiness
    {
        IEnumerable<FinishGoodsRowMaterial> GetGoodsRowMaterialsByOrg(long orgId);
        IEnumerable<FinishGoodsRowMaterial> GetGoodsRowMaterialsByOrgAndFinishInfoId(long orgId, long finishGoodsInfoId);
    }
}
