using ERPBO.FrontDesk.DomainModels;
using ERPBO.FrontDesk.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.FrontDesk.Interface
{
   public interface IJobOrderServiceBusiness
    {
        bool SaveJobOrderServicve(List<JobOrderServiceDTO> jobOrderServices, long userId, long orgId);
        JobOrderService GetJobOrderServiceByJobId(long joborderId, long orgId);
       IEnumerable< JobOrderService> GetJobOrderServiceByJobOrderId(long joborderId, long orgId);
        bool IsDuplicateServicesName(long jobOrderId, long serviceId, long orgId);
    }
}
