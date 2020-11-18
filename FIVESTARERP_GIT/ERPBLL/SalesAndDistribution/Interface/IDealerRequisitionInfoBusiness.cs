using ERPBO.SalesAndDistribution.DTOModels;
using ERPBO.SalesAndDistribution.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.SalesAndDistribution.Interface
{
    public interface IDealerRequisitionInfoBusiness
    {
        IEnumerable<DealerRequisitionInfoDTO> GetDealerRequisitionInfos(long? dealerId, long? srId,long? districtId,long? zoneId, string refNum, string status, string fromDate, string toDate,string flag,long orgId,string role,long? userId);
        bool SaveDealerRequisition(DealerRequisitionInfoDTO model, long userId, long orgId);
    }
}
