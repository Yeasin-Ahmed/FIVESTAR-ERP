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
   public class BrandSSBusiness: IBrandSSBusiness
    {
        private readonly IConfigurationUnitOfWork _configurationDb; // database
        private readonly BrandSSRepository brandSSRepository; // repo
        public BrandSSBusiness(IConfigurationUnitOfWork configurationDb)
        {
            this._configurationDb = configurationDb;
            brandSSRepository = new BrandSSRepository(this._configurationDb);
        }

        public IEnumerable<BrandSS> GetAllBrandByOrgId(long orgId)
        {
            return brandSSRepository.GetAll(b => b.OrganizationId==orgId).ToList();
        }

        public BrandSS GetOneBrandById(long brandId, long orgId)
        {
            return brandSSRepository.GetOneByOrg(b => b.BrandId == brandId && b.OrganizationId == orgId);
        }

        public bool IsDuplicateBrandName(string name, long brandId, long orgId)
        {
            throw new NotImplementedException();
        }

        public bool SaveBrandSS(BrandSSDTO dto, long orgId, long branchId, long userId)
        {
            BrandSS brand = new BrandSS();
            if (dto.BrandId == 0)
            {
                brand.BrandName = dto.BrandName;
                brand.Remarks = dto.Remarks;
                brand.Flag = dto.Flag;
                brand.EUserId = userId;
                brand.EntryDate = DateTime.Now;
                brand.OrganizationId = orgId;
                brand.BranchId = branchId;
                brandSSRepository.Insert(brand);
            }
            else
            {
                brand = GetOneBrandById(dto.BrandId, orgId);
                brand.BrandName = dto.BrandName;
                brand.Remarks = dto.Remarks;
                brand.Flag = dto.Flag;
                brand.UpUserId = userId;
                brand.UpdateDate = DateTime.Now;
                brandSSRepository.Update(brand);
            }
            return brandSSRepository.Save();
        }
    }
}
