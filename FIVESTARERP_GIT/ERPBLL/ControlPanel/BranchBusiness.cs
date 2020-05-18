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
    public class BranchBusiness : IBranchBusiness
    {
        private readonly IControlPanelUnitOfWork _controlPanelUnitOfWork; // database
        private readonly BranchRepository branchRepository; // repo
        public BranchBusiness(IControlPanelUnitOfWork controlPanelUnitOfWork)
        {
            this._controlPanelUnitOfWork = controlPanelUnitOfWork;
            branchRepository = new BranchRepository(this._controlPanelUnitOfWork);
        }
        public IEnumerable<Branch> GetBranchByOrgId(long orgId)
        {
            return branchRepository.GetAll(br => br.OrganizationId == orgId).ToList();
        }

        public Branch GetBranchOneByOrgId(long id, long orgId)
        {
            return branchRepository.GetOneByOrg(br => br.BranchId == id && br.OrganizationId == orgId);
        }

        public bool IsDuplicateBrachName(string branchName, long id, long orgId)
        {
            return branchRepository.GetOneByOrg(br => br.BranchName == branchName && br.BranchId != id && br.OrganizationId == orgId ) != null ? true : false;
        }

        public bool SaveBranch(BranchDTO branchDTO, long userId, long orgId)
        {
            Branch branch = new Branch();
            if (branchDTO.BranchId == 0)
            {
                branch.BranchName = branchDTO.BranchName;
                branch.ShortName = branchDTO.ShortName;
                branch.MobileNo = branchDTO.MobileNo;
                branch.Email = branchDTO.Email;
                branch.PhoneNo = branchDTO.PhoneNo;
                branch.Fax = branchDTO.Fax;
                branch.IsActive = branchDTO.IsActive;
                branch.EntryDate = DateTime.Now;
                branch.EUserId = userId;
                branch.OrganizationId = branchDTO.OrgId;
                branch.Remarks = branchDTO.Remarks;
                branchRepository.Insert(branch);

            }
            else
            {
                branch = GetBranchOneByOrgId(branchDTO.BranchId, orgId);
                branch.BranchName = branchDTO.BranchName;
                branch.ShortName = branchDTO.ShortName;
                branch.MobileNo = branchDTO.MobileNo;
                branch.Email = branchDTO.Email;
                branch.PhoneNo = branchDTO.PhoneNo;
                branch.Fax = branchDTO.Fax;
                branch.IsActive = branchDTO.IsActive;
                branch.UpdateDate = DateTime.Now;
                branch.UpUserId = userId;
                branch.OrganizationId = branchDTO.OrgId;
                branch.Remarks = branchDTO.Remarks;
                branchRepository.Update(branch);
            }
            return branchRepository.Save();
        }
    }
}
