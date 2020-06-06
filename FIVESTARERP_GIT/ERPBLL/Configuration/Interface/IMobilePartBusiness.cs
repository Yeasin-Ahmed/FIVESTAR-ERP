﻿using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration.Interface
{
   public interface IMobilePartBusiness
    {
        IEnumerable<MobilePart> GetAllMobilePartByOrgId(long orgId);
        bool SaveMobile(MobilePartDTO mobilePartDTO, long userId, long orgId);
        bool IsDuplicateMobilePart(string mobilePartName, long id, long orgId);
        MobilePart GetMobilePartOneByOrgId(long id, long orgId);
        bool DeleteMobilePart(long id, long orgId);
    }
}