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
        public bool DeleteTechnicalServiceEng(long id, long orgId, long branchId)
        {
            technicalServiceRepository.DeleteOneByOrg(ts => ts.EngId == id && ts.OrganizationId == orgId && ts.BranchId==branchId);
            return technicalServiceRepository.Save();
        }

        public IEnumerable<TechnicalServiceEng> GetAllTechnicalServiceByOrgId(long orgId, long branchId)
        {
            return technicalServiceRepository.GetAll(ts => ts.OrganizationId == orgId && ts.BranchId==branchId).ToList();
        }

        public TechnicalServiceEng GetTechnicalServiceOneByOrgId(long id, long orgId, long branchId)
        {
            return technicalServiceRepository.GetOneByOrg(ts => ts.EngId == id && ts.OrganizationId == orgId && ts.BranchId == branchId);
        }

        public bool IsDuplicateTechnicalName(string name, long id, long orgId, long branchId)
        {
            return technicalServiceRepository.GetOneByOrg(ts => ts.Name == name && ts.EngId != id && ts.OrganizationId == orgId && ts.BranchId == branchId) != null ? true : false;
        }

        public bool SaveTechnicalService(TechnicalServiceEngDTO technicalServiceEngDTO, long userId, long orgId, long branchId)
        {
            TechnicalServiceEng technicalServiceEng = new TechnicalServiceEng();
            if (technicalServiceEngDTO.EngId == 0)
            {
                technicalServiceEng.Name = technicalServiceEngDTO.Name;
                technicalServiceEng.TsCode = technicalServiceEngDTO.TsCode;
                technicalServiceEng.Address = technicalServiceEngDTO.Address;
                technicalServiceEng.PhoneNumber = technicalServiceEngDTO.PhoneNumber;
                technicalServiceEng.UserName = technicalServiceEngDTO.UserName;
                technicalServiceEng.Password = technicalServiceEngDTO.Password;
                technicalServiceEng.Remarks = technicalServiceEngDTO.Remarks;
                technicalServiceEng.OrganizationId = orgId;
                technicalServiceEng.BranchId = branchId;
                technicalServiceEng.EUserId = userId;
                technicalServiceEng.EntryDate = DateTime.Now;
                technicalServiceRepository.Insert(technicalServiceEng);
            }
            else
            {
                technicalServiceEng = GetTechnicalServiceOneByOrgId(technicalServiceEngDTO.EngId, orgId,branchId);
                technicalServiceEng.Name = technicalServiceEngDTO.Name;
                technicalServiceEng.TsCode = technicalServiceEngDTO.TsCode;
                technicalServiceEng.Address = technicalServiceEngDTO.Address;
                technicalServiceEng.PhoneNumber = technicalServiceEngDTO.PhoneNumber;
                technicalServiceEng.UserName = technicalServiceEngDTO.UserName;
                technicalServiceEng.Password = technicalServiceEngDTO.Password;
                technicalServiceEng.Remarks = technicalServiceEngDTO.Remarks;
                technicalServiceEng.OrganizationId = orgId;
                technicalServiceEng.BranchId = branchId;
                technicalServiceEng.UpUserId = userId;
                technicalServiceEng.UpdateDate = DateTime.Now;
                technicalServiceRepository.Update(technicalServiceEng);
            }
            return technicalServiceRepository.Save();
        }
    }
}
