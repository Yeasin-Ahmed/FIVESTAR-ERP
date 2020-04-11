using ERPBLL.ControlPanel.Interface;
using ERPBO.ControlPanel.DomainModels;
using ERPBO.ControlPanel.DTOModels;
using ERPDAL.ControlPanelDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.ControlPanel
{
    public class AppUserBusiness : IAppUserBusiness
    {
        private readonly IControlPanelUnitOfWork _controlPanelUnitOfWork; // database
        private readonly AppUserRepository appUserRepository; // repo
        public AppUserBusiness(IControlPanelUnitOfWork controlPanelUnitOfWork)
        {
            this._controlPanelUnitOfWork = controlPanelUnitOfWork;
            appUserRepository = new AppUserRepository(this._controlPanelUnitOfWork);
        }
        public IEnumerable<AppUser> GetAllAppUserByOrgId(long orgId)
        {
            return appUserRepository.GetAll(user => user.OrganizationId == orgId).ToList();
        }

        public IEnumerable<AppUser> GetAllAppUsers()
        {
            return appUserRepository.GetAll().ToList();
        }

        public AppUser GetAppUserOneById(long id, long orgId)
        {
            return appUserRepository.GetOneByOrg(user => user.UserId == id && user.OrganizationId == orgId);
        }

        public bool IsDuplicateEmployeeId(string employeeId, long id, long orgId)
        {
            return appUserRepository.GetOneByOrg(user => user.EmployeeId == employeeId && user.UserId != id && user.OrganizationId == orgId) != null ? true : false;
        }

        public bool SaveAppUser(AppUserDTO appUserDTO, long userId, long orgId)
        {
            AppUser appUser = new AppUser();
            if (appUserDTO.UserId == 0)
            {
                appUser.EmployeeId = appUserDTO.EmployeeId;
                appUser.FullName = appUserDTO.FullName;
                appUser.MobileNo = appUserDTO.MobileNo;
                appUser.Address = appUserDTO.Address;
                appUser.Email = appUserDTO.Email;
                appUser.Desigation = appUserDTO.Desigation;
                appUser.UserName = appUserDTO.UserName;
                appUser.Password = appUserDTO.Password;
                appUser.ConfirmPassword = appUserDTO.ConfirmPassword;
                appUser.IsActive = appUserDTO.IsActive;
                appUser.IsRoleActive = appUserDTO.IsRoleActive;
                appUser.EUserId = userId;
                appUser.EntryDate = DateTime.Now;
                appUser.OrganizationId = orgId;
                appUser.BranchId = appUserDTO.BranchId;
                appUser.RoleId = appUserDTO.RoleId;
                appUserRepository.Insert(appUser);

            }
            else
            {
                appUser = GetAppUserOneById(appUserDTO.UserId, orgId);
                appUser.EmployeeId = appUserDTO.EmployeeId;
                appUser.FullName = appUserDTO.FullName;
                appUser.MobileNo = appUserDTO.MobileNo;
                appUser.Address = appUserDTO.Address;
                appUser.Email = appUserDTO.Email;
                appUser.Desigation = appUserDTO.Desigation;
                appUser.UserName = appUserDTO.UserName;
                appUser.Password = appUserDTO.Password;
                appUser.ConfirmPassword = appUserDTO.ConfirmPassword;
                appUser.IsActive = appUserDTO.IsActive;
                appUser.IsRoleActive = appUserDTO.IsRoleActive;
                appUser.UpUserId = userId;
                appUser.UpdateDate = DateTime.Now;
                appUser.OrganizationId = orgId;
                appUser.BranchId = appUserDTO.BranchId;
                appUser.RoleId = appUserDTO.RoleId;
                appUserRepository.Update(appUser);
            }
            return appUserRepository.Save();
        }
    }
}
