using ERPBO.FrontDesk.DomainModels;
using ERPBO.FrontDesk.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.FrontDesk.Interface
{
   public interface IJobOrderFaultBusiness
    {
        bool SaveJobOrderFault(List<JobOrderFaultDTO> jobOrderFaults, long userId, long orgId);
        JobOrderFault GetJobOrderFaultByJobId(long joborderId, long orgId);
        IEnumerable<JobOrderFault> GetJobOrderFaultByJobOrderId(long joborderId, long orgId);
    }
}
