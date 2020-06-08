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
    public class AccessoriesBusiness : IAccessoriesBusiness
    {
        private readonly IConfigurationUnitOfWork _configurationDb; // database
        private readonly AccessoriesRepository accessoriesRepository; // repo
        public AccessoriesBusiness(IConfigurationUnitOfWork configurationDb)
        {
            this._configurationDb = configurationDb;
            accessoriesRepository = new AccessoriesRepository(this._configurationDb);
        }

        public bool DeleteAccessories(long id, long orgId)
        {
            accessoriesRepository.DeleteOneByOrg(a => a.AccessoriesId == id && a.OrganizationId == orgId);
            return accessoriesRepository.Save();
        }

        public Accessories GetAccessoriesOneByOrgId(long id, long orgId)
        {
            return accessoriesRepository.GetOneByOrg(access => access.AccessoriesId == id && access.OrganizationId == orgId);
        }

        public IEnumerable<Accessories> GetAllAccessoriesByOrgId(long orgId)
        {
            return accessoriesRepository.GetAll(access => access.OrganizationId == orgId).ToList();
        }

        public bool IsDuplicateAccessoriesName(string accessoriesName, long id, long orgId)
        {
            return accessoriesRepository.GetOneByOrg(access => access.AccessoriesName == accessoriesName && access.AccessoriesId != id && access.OrganizationId == orgId) != null ? true : false;
        }

        public bool SaveAccessories(AccessoriesDTO accessoriesDTO, long userId, long orgId)
        {
            Accessories accessories = new Accessories();
            if (accessoriesDTO.AccessoriesId == 0)
            {
                accessories.AccessoriesName = accessoriesDTO.AccessoriesName;
                accessories.AccessoriesCode = accessoriesDTO.AccessoriesCode;
                accessories.Remarks = accessoriesDTO.Remarks;
                accessories.EUserId = userId;
                accessories.EntryDate = DateTime.Now;
                accessories.OrganizationId = orgId;
                accessoriesRepository.Insert(accessories);
            }
            else
            {
                accessories = GetAccessoriesOneByOrgId(accessoriesDTO.AccessoriesId, orgId);
                accessories.AccessoriesName = accessoriesDTO.AccessoriesName;
                accessories.AccessoriesCode = accessoriesDTO.AccessoriesCode;
                accessories.Remarks = accessoriesDTO.Remarks;
                accessories.UpUserId = userId;
                accessories.UpdateDate = DateTime.Now;
                accessories.OrganizationId = orgId;
                accessoriesRepository.Update(accessories);
            }
            return accessoriesRepository.Save();
        }
    }
}
