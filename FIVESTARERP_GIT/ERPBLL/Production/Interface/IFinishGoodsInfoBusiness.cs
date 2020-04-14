using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IFinishGoodsInfoBusiness
    {
        IEnumerable<FinishGoodsInfo> GetFinishGoodsByOrg(long orgId);
        bool SaveFinishGoods(FinishGoodsInfoDTO infos, List<FinishGoodsRowMaterialDTO> details, long userId, long orgId);
    }
}
