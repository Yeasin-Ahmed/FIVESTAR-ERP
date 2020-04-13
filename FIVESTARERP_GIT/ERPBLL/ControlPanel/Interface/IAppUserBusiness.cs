using ERPBO.ControlPanel.DomainModels;
using ERPBO.ControlPanel.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.ControlPanel.Interface
{
   public interface IAppUserBusiness
    {
        IEnumerable<AppUser> GetAllAppUserByOrgId(long orgId);
        bool SaveAppUser(AppUserDTO appUserDTO, long userId, long orgId);
        AppUser GetAppUserOneById(long id, long orgId);
        bool IsDuplicateEmployeeId(string employeeId, long id, long orgId);
        IEnumerable<AppUser> GetAllAppUsers();
    }
}
