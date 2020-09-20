using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration.Interface
{
   public interface IClientProblemBusiness
    {
        IEnumerable<ClientProblem> GetAllClientProblemByOrgId(long orgId);
        bool SaveClientProblem(ClientProblemDTO clientProblemDTO, long userId, long orgId);
        bool IsDuplicateProblemName(string problemName, long id, long orgId);
        ClientProblem GetClientProblemOneByOrgId(long id, long orgId);
        bool DeleteClientProblem(long id, long orgId);
        IEnumerable<ClientProblemDTO> GetClientProblemByOrgId(long orgId);
    }
}
