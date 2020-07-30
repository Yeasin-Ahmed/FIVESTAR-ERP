using ERPBLL.Inventory.Interface;
using ERPBO.Inventory.DomainModels;
using ERPBO.Inventory.DTOModel;
using ERPDAL.InventoryDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Inventory
{
    public class IQCBusiness : IIQCBusiness
    {
        private readonly IQCRepository iQCRepository;//Repository
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;//DB
        public IQCBusiness(IInventoryUnitOfWork inventoryUnitOfWork)
        {
            this._inventoryUnitOfWork = inventoryUnitOfWork;
            iQCRepository = new IQCRepository(this._inventoryUnitOfWork);
        }
        public IEnumerable<IQC> GetAllIQCByOrgId(long orgId)
        {
            return iQCRepository.GetAll(s => s.OrganizationId == orgId);
        }

        public bool SaveIQC(IQCDTO iQCDTO, long userId, long orgId)
        {
            IQC iQC = new IQC();
            if(iQCDTO.Id == 0)
            {
                iQC.IQCName = iQCDTO.IQCName;
                iQC.IsActive = iQCDTO.IsActive;
                iQC.Remarks = iQCDTO.Remarks;
                iQC.EUserId = userId;
                iQC.EntryDate = DateTime.Now;
                iQC.OrganizationId = orgId;
                iQCRepository.Insert(iQC);
            }
            else
            {
                iQC = GetIQCOneByOrgId(iQCDTO.Id, orgId);
                iQC.IQCName = iQCDTO.IQCName;
                iQC.IsActive = iQCDTO.IsActive;
                iQC.Remarks = iQCDTO.Remarks;
                iQC.UpUserId = userId;
                iQC.UpdateDate = DateTime.Now;
                iQCRepository.Update(iQC);
            }
            return iQCRepository.Save();
        }

        public IQC GetIQCOneByOrgId(long id, long orgId)
        {
            return iQCRepository.GetOneByOrg(s => s.Id == id && s.OrganizationId == orgId);
        }

        public bool IsDuplicateIQCName(long id, long orgId, string iqcName)
        {
            return iQCRepository.GetOneByOrg(s => s.Id != id && s.OrganizationId == orgId && s.IQCName == iqcName) != null ? true : false;
        }
    }
}
