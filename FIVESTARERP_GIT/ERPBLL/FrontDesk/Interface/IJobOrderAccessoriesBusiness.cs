using ERPBO.FrontDesk.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.FrontDesk.Interface
{
   public interface IJobOrderAccessoriesBusiness
    {
        JobOrderAccessories GetJobOrderAccessoriesById(long id, long orgId);

        IEnumerable<JobOrderAccessories> GetJobOrderAccessoriesByJobOrder(long jobOrderId, long orgId);


    }
}
