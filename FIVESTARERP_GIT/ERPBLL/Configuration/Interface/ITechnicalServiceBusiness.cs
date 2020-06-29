using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration.Interface
{
   public interface ITechnicalServiceBusiness
    {
        IEnumerable<TechnicalServiceEng> GetAllTechnicalServiceByOrgId(long orgId,long branchId);
        bool SaveTechnicalService(TechnicalServiceEngDTO technicalServiceEngDTO, long userId, long orgId, long branchId);
        bool IsDuplicateTechnicalName(string name, long id, long orgId, long branchId);
        TechnicalServiceEng GetTechnicalServiceOneByOrgId(long id, long orgId, long branchId);
        bool DeleteTechnicalServiceEng(long id, long orgId, long branchId);
    }
}
