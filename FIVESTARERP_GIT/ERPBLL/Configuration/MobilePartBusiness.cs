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
   public class MobilePartBusiness: IMobilePartBusiness
    {
        private readonly IConfigurationUnitOfWork _configurationDb; // database
        private readonly MobilePartRepository mobilePartRepository; // repo
        public MobilePartBusiness(IConfigurationUnitOfWork configurationDb)
        {
            this._configurationDb = configurationDb;
            mobilePartRepository = new MobilePartRepository(this._configurationDb);
        }

        public bool DeleteMobilePart(long id, long orgId)
        {
            mobilePartRepository.DeleteOneByOrg(part => part.MobilePartId == id && part.OrganizationId == orgId);
            return mobilePartRepository.Save();
        }

        public IEnumerable<MobilePartDTO> GetAllMobilePartAndCode(long orgId)
        {
                return this._configurationDb.Db.Database.SqlQuery<MobilePartDTO>(
                    string.Format(@"SELECT MobilePartId,CAST(MobilePartName AS VARCHAR(25)) +'_'+ CAST(MobilePartCode AS VARCHAR(10)) 'MobilePartName'
FROM [Configuration].dbo.tblMobileParts
where OrganizationId={0}", orgId)).ToList();
        }

        public IEnumerable<MobilePart> GetAllMobilePartByOrgId(long orgId)
        {
            return mobilePartRepository.GetAll(part => part.OrganizationId == orgId).ToList();
        }

        public MobilePart GetMobilePartOneByOrgId(long id, long orgId)
        {
            return mobilePartRepository.GetOneByOrg(part => part.MobilePartId == id && part.OrganizationId == orgId);
        }

        public bool IsDuplicateMobilePartCode(string partsCode, long id, long orgId)
        {
            return mobilePartRepository.GetOneByOrg(part => part.MobilePartCode == partsCode && part.MobilePartId != id && part.OrganizationId == orgId) != null ? true : false;
        }

        public bool SaveMobile(MobilePartDTO mobilePartDTO, long userId, long orgId)
        {
            MobilePart mobilePart = new MobilePart();
            if (mobilePartDTO.MobilePartId == 0)
            {
                mobilePart.MobilePartName = mobilePartDTO.MobilePartName;
                mobilePart.MobilePartCode = mobilePartDTO.MobilePartCode;
                mobilePart.Remarks = mobilePartDTO.Remarks;
                mobilePart.OrganizationId = orgId;
                mobilePart.EUserId = userId;
                mobilePart.EntryDate = DateTime.Now;
                mobilePartRepository.Insert(mobilePart);
            }
            else
            {
                mobilePart = GetMobilePartOneByOrgId(mobilePartDTO.MobilePartId, orgId);
                mobilePart.MobilePartName = mobilePartDTO.MobilePartName;
                mobilePart.MobilePartCode = mobilePartDTO.MobilePartCode;
                mobilePart.Remarks = mobilePartDTO.Remarks;
                mobilePart.OrganizationId = orgId;
                mobilePart.UpUserId = userId;
                mobilePart.UpdateDate = DateTime.Now;
                mobilePartRepository.Update(mobilePart);
            }
            return mobilePartRepository.Save();
        }
    }
}
