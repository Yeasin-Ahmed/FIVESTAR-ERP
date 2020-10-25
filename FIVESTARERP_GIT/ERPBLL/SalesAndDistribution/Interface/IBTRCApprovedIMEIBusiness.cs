using ERPBO.Common;
using ERPBO.SalesAndDistribution.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.SalesAndDistribution.Interface
{
    public interface IBTRCApprovedIMEIBusiness
    {
        ExecutionStateWithText UploadIMEI(List<ApprovedIMEI> iMEIs, long userId, long orgId);
        IEnumerable<BTRCApprovedIMEIDTO> GetBTRCApprovedIMEIs(long orgId, string stateStatus, long modelId = 0,string fromDate ="" , string toDate="");
    }
}
