using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration.Interface
{
   public interface IBranchBusiness2
    {
        IEnumerable<Branch> GetAllBranchByOrgId(long orgId);
        bool SaveBranch(BranchDTO branchDTO, long userId, long orgId);
        bool IsDuplicateBranchName(string branchName, long id, long orgId);
        Branch GetBranchOneByOrgId(long id, long orgId);
        bool DeleteBranch(long id, long orgId);
    }
}
