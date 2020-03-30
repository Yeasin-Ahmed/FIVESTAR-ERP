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
    public class RoleBusiness : IRoleBusiness
    {
        private readonly IControlPanelUnitOfWork _controlPanelUnitOfWork; // database
        private readonly RoleRepository roleRepository; // repo
        public RoleBusiness(IControlPanelUnitOfWork controlPanelUnitOfWork)
        {
            this._controlPanelUnitOfWork = controlPanelUnitOfWork;
            roleRepository = new RoleRepository(this._controlPanelUnitOfWork);
        }
        public IEnumerable<Role> GetAllRoleByOrgId(long orgId)
        {
            return roleRepository.GetAll(r => r.OrganizationId == orgId).ToList();
        }

        public Role GetRoleOneById(long id, long orgId)
        {
            return roleRepository.GetOneByOrg(role => role.RoleId == id && role.OrganizationId == orgId);
        }

        public bool IsDuplicateRoleName(string roleName, long id, long orgId)
        {
            return roleRepository.GetOneByOrg(role => role.RoleName == roleName && role.RoleId != id && role.OrganizationId == orgId) != null ? true : false;
        }

        public bool SaveRole(RoleDTO roleDTO, long userId, long orgId)
        {
            Role role = new Role();
            if (roleDTO.RoleId == 0)
            {
                role.RoleName = roleDTO.RoleName;
                role.EUserId = userId;
                role.OrganizationId = orgId;
                role.EntryDate = DateTime.Now;
                roleRepository.Insert(role);
            }
            else
            {
                role = GetRoleOneById(roleDTO.RoleId, orgId);
                role.RoleName = roleDTO.RoleName;
                role.UpUserId = userId;
                role.OrganizationId = orgId;
                role.UpdateDate = DateTime.Now;
                roleRepository.Update(role);
            }
            return roleRepository.Save();
        }
    }
}
