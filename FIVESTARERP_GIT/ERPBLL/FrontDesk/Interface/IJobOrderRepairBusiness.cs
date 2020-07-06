using ERPBO.FrontDesk.DomainModels;
using ERPBO.FrontDesk.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.FrontDesk.Interface
{
   public interface IJobOrderRepairBusiness
    {
        bool SaveJobOrderRepair(List<JobOrderRepairDTO> jobOrderRepairs, long userId, long orgId);
        JobOrderRepair GetJobOrderRepairByJobId(long joborderId, long orgId);
       IEnumerable< JobOrderRepair> GetJobOrderRepairByJobOrderId(long joborderId, long orgId);
    }
}
