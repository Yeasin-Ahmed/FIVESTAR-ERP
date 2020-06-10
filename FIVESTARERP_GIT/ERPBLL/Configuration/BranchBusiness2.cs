using ERPBLL.Configuration.Interface;
using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using ERPDAL.ConfigurationDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration
{
   public class BranchBusiness2:IBranchBusiness2
    {
        private readonly IConfigurationUnitOfWork _configurationDb; // database
        private readonly BranchRepository branchRepository; // repo
        public BranchBusiness2(IConfigurationUnitOfWork configurationDb)
        {
            this._configurationDb = configurationDb;
            branchRepository = new BranchRepository(this._configurationDb);
        }

        public bool DeleteBranch(long id, long orgId)
        {
            branchRepository.DeleteOneByOrg(b => b.BranchId == id && b.OrganizationId == orgId);
            return branchRepository.Save();
        }

        public IEnumerable<Branch> GetAllBranchByOrgId(long orgId)
        {
            return branchRepository.GetAll(branch => branch.OrganizationId == orgId).ToList();
        }

        public Branch GetBranchOneByOrgId(long id, long orgId)
        {
            return branchRepository.GetOneByOrg(branch => branch.BranchId == id && branch.OrganizationId == orgId);
        }

        public bool IsDuplicateBranchName(string branchName, long id, long orgId)
        {
            return branchRepository.GetOneByOrg(branch => branch.BranchName == branchName && branch.BranchId != id && branch.OrganizationId == orgId) != null ? true : false;
        }

        public bool SaveBranch(BranchDTO branchDTO, long userId, long orgId)
        {
            Branch branch = new Branch();
            if (branchDTO.BranchId == 0)
            {
                branch.BranchName = branchDTO.BranchName;
                branch.BranchAddress = branchDTO.BranchAddress;
                branch.Remarks = branchDTO.Remarks;
                branch.EUserId = userId;
                branch.EntryDate = DateTime.Now;
                branch.OrganizationId = orgId;
                branchRepository.Insert(branch);
            }
            else
            {
                branch = GetBranchOneByOrgId(branchDTO.BranchId, orgId);
                branch.BranchName = branchDTO.BranchName;
                branch.BranchAddress = branchDTO.BranchAddress;
                branch.Remarks = branchDTO.Remarks;
                branch.UpUserId = userId;
                branch.UpdateDate = DateTime.Now;
                branch.OrganizationId = orgId;
                branchRepository.Update(branch);
            }
            return branchRepository.Save();
        }
    }
}
