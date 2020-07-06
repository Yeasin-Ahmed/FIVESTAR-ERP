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
   public class FaultBusiness: IFaultBusiness
    {
        private readonly IConfigurationUnitOfWork _configurationDb; // database
        private readonly FaultRepository faultRepository; // repo
        public FaultBusiness(IConfigurationUnitOfWork configurationDb)
        {
            this._configurationDb = configurationDb;
            faultRepository = new FaultRepository(this._configurationDb);
        }

        public bool DeleteFault(long id, long orgId)
        {
            faultRepository.DeleteOneByOrg(f => f.FaultId == id && f.OrganizationId == orgId );
            return faultRepository.Save();
        }

        public IEnumerable<Fault> GetAllFaultByOrgId(long orgId)
        {
            return faultRepository.GetAll(f => f.OrganizationId == orgId).ToList();
        }

        public Fault GetFaultOneByOrgId(long id, long orgId)
        {
            return faultRepository.GetOneByOrg(f => f.FaultId == id && f.OrganizationId == orgId);
        }

        public bool IsDuplicateFaultName(string name, long id, long orgId)
        {
            return faultRepository.GetOneByOrg(f => f.FaultName == name && f.FaultId != id && f.OrganizationId == orgId) != null ? true : false;
        }

        public bool SaveFault(FaultDTO faultDTO, long userId, long orgId)
        {
            Fault fault = new Fault();
            if (faultDTO.FaultId == 0)
            {
                fault.FaultName = faultDTO.FaultName;
                fault.FaultCode = faultDTO.FaultCode;
                fault.Remarks = faultDTO.Remarks;
                fault.OrganizationId = orgId;
                fault.EUserId = userId;
                fault.EntryDate = DateTime.Now;
                faultRepository.Insert(fault);
            }
            else
            {
                fault = GetFaultOneByOrgId(faultDTO.FaultId, orgId);
                fault.FaultName = faultDTO.FaultName;
                fault.FaultCode = faultDTO.FaultCode;
                fault.Remarks = faultDTO.Remarks;
                fault.OrganizationId = orgId;
                fault.UpUserId = userId;
                fault.UpdateDate = DateTime.Now;
                faultRepository.Update(fault);
            }
            return faultRepository.Save();
        }
    }
}
