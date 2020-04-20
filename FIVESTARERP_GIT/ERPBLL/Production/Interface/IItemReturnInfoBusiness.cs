using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IItemReturnInfoBusiness
    {
        IEnumerable<ItemReturnInfo> GetItemReturnInfos(long OrgId);
        ItemReturnInfo GetItemReturnInfo(long OrgId, long infoId);
        bool SaveFaultyItemOrGoodsReturn(ItemReturnInfoDTO info, List<ItemReturnDetailDTO> details);
        bool SaveItemReturnStatus(long irInfoId, string status, long orgId);

        IEnumerable<DashboardFacultyWiseProductionDTO> DashboardFacultyDayWiseProduction(long orgId);
        IEnumerable<DashboardFacultyWiseProductionDTO> DashboardFacultyOverAllWiseProduction(long orgId);
    }
}
