using ERPBO.SalesAndDistribution.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.SalesAndDistribution.Interface
{
    public interface IDealerRequisitionDetailBusiness
    {
        IEnumerable<DealerRequisitionDetailDTO> GetDealerRequisitionDetails(long reqInfoId,long orgId);
    }
}
