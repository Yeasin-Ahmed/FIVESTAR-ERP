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
    public class TechnicalServiceBusiness : ITechnicalServiceBusiness
    {
        private readonly IConfigurationUnitOfWork _configurationDb; // database
        private readonly TechnicalServiceRepository technicalServiceRepository; // repo
        public TechnicalServiceBusiness(IConfigurationUnitOfWork configurationDb)
        {
            this._configurationDb = configurationDb;
            technicalServiceRepository = new TechnicalServiceRepository(this._configurationDb);
        }
        public bool DeleteTechnicalServiceEng(long id, long orgId)
        {
            technicalServiceRepository.DeleteOneByOrg(ts => ts.EngId == id && ts.OrganizationId == orgId);
            return technicalServiceRepository.Save();
        }

        public IEnumerable<TechnicalServiceEng> GetAllTechnicalServiceByOrgId(long orgId)
        {
            return technicalServiceRepository.GetAll(ts => ts.OrganizationId == orgId).ToList();
        }

        public TechnicalServiceEng GetTechnicalServiceOneByOrgId(long id, long orgId)
        {
            return technicalServiceRepository.GetOneByOrg(ts => ts.EngId == id && ts.OrganizationId == orgId);
        }

        public bool IsDuplicateTechnicalName(string name, long id, long orgId)
        {
            return technicalServiceRepository.GetOneByOrg(ts => ts.Name == name && ts.EngId != id && ts.OrganizationId == orgId) != null ? true : false;
        }

        public bool SaveTechnicalService(TechnicalServiceEngDTO technicalServiceEngDTO, long userId, long orgId)
        {
            TechnicalServiceEng technicalServiceEng = new TechnicalServiceEng();
            if (technicalServiceEngDTO.EngId == 0)
            {
                technicalServiceEng.Name = technicalServiceEngDTO.Name;
                technicalServiceEng.Address = technicalServiceEngDTO.Address;
                technicalServiceEng.PhoneNumber = technicalServiceEngDTO.PhoneNumber;
                technicalServiceEng.Remarks = technicalServiceEngDTO.Remarks;
                technicalServiceEng.OrganizationId = orgId;
                technicalServiceEng.EUserId = userId;
                technicalServiceEng.EntryDate = DateTime.Now;
                technicalServiceRepository.Insert(technicalServiceEng);
            }
            else
            {
                technicalServiceEng = GetTechnicalServiceOneByOrgId(technicalServiceEngDTO.EngId, orgId);
                technicalServiceEng.Name = technicalServiceEngDTO.Name;
                technicalServiceEng.Address = technicalServiceEngDTO.Address;
                technicalServiceEng.PhoneNumber = technicalServiceEngDTO.PhoneNumber;
                technicalServiceEng.Remarks = technicalServiceEngDTO.Remarks;
                technicalServiceEng.OrganizationId = orgId;
                technicalServiceEng.UpUserId = userId;
                technicalServiceEng.UpdateDate = DateTime.Now;
                technicalServiceRepository.Update(technicalServiceEng);
            }
            return technicalServiceRepository.Save();
        }
    }
}
