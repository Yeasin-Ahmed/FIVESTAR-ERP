﻿using ERPBO.ControlPanel.DomainModels;
using ERPBO.ControlPanel.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.ControlPanel.Interface
{
   public interface IRoleBusiness
    {
        IEnumerable<Role> GetAllRoleByOrgId(long orgId);
        bool SaveRole(RoleDTO roleDTO, long userId, long orgId);
        bool IsDuplicateRoleName(string roleName, long id, long orgId);
        Role GetRoleOneById(long id, long orgId);
    }
}
