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
   public class WorkShopBusiness: IWorkShopBusiness
    {
        private readonly IConfigurationUnitOfWork _configurationDb; // database
        private readonly WorkShopRepository workShopRepository; // repo
        public WorkShopBusiness(IConfigurationUnitOfWork configurationDb)
        {
            this._configurationDb = configurationDb;
            workShopRepository = new WorkShopRepository(this._configurationDb);
        }

        public bool DeleteWorkShop(long id, long orgId, long branchId)
        {
            workShopRepository.DeleteOneByOrg(shop => shop.WorkShopId == id && shop.OrganizationId == orgId && shop.BranchId == branchId);
            return workShopRepository.Save();
        }

        public IEnumerable<WorkShop> GetAllWorkShopByOrgId(long orgId, long branchId)
        {
            return workShopRepository.GetAll(shop => shop.OrganizationId == orgId && shop.BranchId == branchId).ToList();
        }

        public WorkShop GetWorkShopOneByOrgId(long id, long orgId, long branchId)
        {
            return workShopRepository.GetOneByOrg(shop => shop.WorkShopId == id && shop.OrganizationId == orgId && shop.BranchId == branchId);
        }

        public bool IsDuplicateWorkShopName(string workshopName, long id, long orgId, long branchId)
        {
            throw new NotImplementedException();
        }

        public bool SaveWorkShop(WorkShopDTO workShopDTO, long userId, long orgId, long branchId)
        {
            WorkShop workShop = new WorkShop();
            if (workShopDTO.WorkShopId == 0)
            {
                workShop.WorkShopName = workShopDTO.WorkShopName;
                workShop.Remarks = workShopDTO.Remarks;
                workShop.OrganizationId = orgId;
                workShop.BranchId = branchId;
                workShop.EUserId = userId;
                workShop.EntryDate = DateTime.Now;
                workShopRepository.Insert(workShop);
            }
            else
            {
                workShop = GetWorkShopOneByOrgId(workShopDTO.WorkShopId, orgId, branchId);
                workShop.WorkShopName = workShopDTO.WorkShopName;
                workShop.Remarks = workShopDTO.Remarks;
                workShop.OrganizationId = orgId;
                workShop.BranchId = branchId;
                workShop.UpUserId = userId;
                workShop.UpdateDate = DateTime.Now;
                workShopRepository.Update(workShop);
            }
            return workShopRepository.Save();
        }
    }
}
