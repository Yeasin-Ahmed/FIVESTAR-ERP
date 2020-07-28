using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration.Interface
{
   public interface IWorkShopBusiness
    {
        IEnumerable<WorkShop> GetAllWorkShopByOrgId(long orgId, long branchId);
        bool SaveWorkShop(WorkShopDTO workShopDTO, long userId, long orgId, long branchId);
        bool IsDuplicateWorkShopName(string workshopName, long id, long orgId, long branchId);
        WorkShop GetWorkShopOneByOrgId(long id, long orgId, long branchId);
        bool DeleteWorkShop(long id, long orgId, long branchId);
    }
}
