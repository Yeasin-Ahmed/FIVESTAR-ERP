using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration.Interface
{
   public interface IServiceBusiness
    {
        IEnumerable<Service> GetAllServiceByOrgId(long orgId);
        bool SaveService(ServiceDTO serviceDTO, long userId, long orgId);
        bool IsDuplicateServiceName(string name, long id, long orgId);
        Service GetServiceOneByOrgId(long id, long orgId);
        bool DeleteService(long id, long orgId);
    }
}
