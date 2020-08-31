using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration.Interface
{
   public interface IServicesWarehouseBusiness
    {
        IEnumerable<ServiceWarehouse> GetAllServiceWarehouseByOrgId(long orgId,long branchId);
        bool SaveServiceWarehouse(ServicesWarehouseDTO servicesWarehouseDTO, long userId, long orgId,long branchId);
        bool IsDuplicateServicesWarehouseName(string sWName, long id, long orgId, long branchId);
        ServiceWarehouse GetServiceWarehouseOneByOrgId(long id, long orgId, long branchId);
        bool DeleteServicesWarehouse(long id, long orgId, long branchId);
        ServiceWarehouse GetWarehouseOneByOrgId(long orgId, long branchId);
    }
}
