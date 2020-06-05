using ERPBO.FrontDesk.DomainModels;
using ERPBO.FrontDesk.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.FrontDesk.Interface
{
    public interface IJobOrderBusiness
    {
        IEnumerable<JobOrderDTO> GetJobOrders(string mobileNo, long? modelId, string status,long? jobOrderId, string jobCode , long orgId);
        JobOrder GetJobOrderById(long jobOrderId,long orgId);
        bool SaveJobOrder(JobOrderDTO jobOrder, List<JobOrderAccessoriesDTO> jobOrderAccessories,List<JobOrderProblemDTO> jobOrderProblems, long userId, long orgId);
        bool UpdateJobOrderStatus(long jobOrderId, string status, string type, long userId, long orgId);
    }
}
