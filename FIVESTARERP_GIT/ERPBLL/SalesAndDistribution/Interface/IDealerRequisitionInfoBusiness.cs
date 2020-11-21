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
        IEnumerable<DealerRequisitionInfoViewModel> GetDealerUnVerifiedPO(long? orgId,long? dealerId,long? districtId,long? zoneId,string refNum,string fromDate, string toDate);
        IEnumerable<DealerRequisitionInfoViewModel> GetDealerVerifiedPO(long? orgId, long? dealerId, long? districtId, long? zoneId, string refNum, string fromDate, string toDate);
        IEnumerable<DealerRequisitionInfoDTO> GetDealerRequisitionInfos(long? dealerId, long? srId,long? districtId,long? zoneId, string refNum, string status, string fromDate, string toDate,string flag,long orgId,string role,long? userId, long? reqInfoId);
        bool SaveDealerRequisition(DealerRequisitionInfoDTO model, long userId, long orgId);
        bool SaveVerifiedDPO(long[] id,long userId, long orgId);
        IEnumerable<DealerRequisitionInfoViewModel> GetDealerUnApprovedPO(long? orgId, long? dealerId, long? districtId, long? zoneId, string refNum, string fromDate, string toDate);
        IEnumerable<DealerRequisitionInfoViewModel> GetDealerApprovedPO(long? orgId, long? dealerId, long? districtId, long? zoneId, string refNum, string fromDate, string toDate);
        bool SaveApprovalDPO(long[] id, long userId, long orgId);
    }
}
