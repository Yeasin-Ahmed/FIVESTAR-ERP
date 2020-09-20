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
   public class ClientProblemBusiness: IClientProblemBusiness
    {
        private readonly IConfigurationUnitOfWork _configurationDb; // database
        private readonly ClientProblemRepository clientProblemRepository; // repo
        public ClientProblemBusiness(IConfigurationUnitOfWork configurationDb)
        {
            this._configurationDb = configurationDb;
            clientProblemRepository = new ClientProblemRepository(this._configurationDb);
        }

        public ClientProblem GetClientProblemOneByOrgId(long id, long orgId)
        {
            return clientProblemRepository.GetOneByOrg(prob => prob.ProblemId == id && prob.OrganizationId == orgId);
        }

        public IEnumerable<ClientProblem> GetAllClientProblemByOrgId(long orgId)
        {
            return clientProblemRepository.GetAll(prob => prob.OrganizationId == orgId).ToList();
        }

        public bool IsDuplicateProblemName(string problemName, long id, long orgId)
        {
            return clientProblemRepository.GetOneByOrg(client => client.ProblemName == problemName && client.ProblemId != id && client.OrganizationId == orgId) != null ? true : false;
        }

        public bool SaveClientProblem(ClientProblemDTO clientProblemDTO, long userId, long orgId)
        {
            ClientProblem clientProblem = new ClientProblem();
            if (clientProblemDTO.ProblemId == 0)
            {
                clientProblem.ProblemName = clientProblemDTO.ProblemName;
                clientProblem.Remarks = clientProblemDTO.Remarks;
                clientProblem.OrganizationId = orgId;
                clientProblem.EUserId = userId;
                clientProblem.EntryDate = DateTime.Now;
                clientProblemRepository.Insert(clientProblem);
            }
            else
            {
                clientProblem = GetClientProblemOneByOrgId(clientProblemDTO.ProblemId, orgId);
                clientProblem.ProblemName = clientProblemDTO.ProblemName;
                clientProblem.Remarks = clientProblemDTO.Remarks;
                clientProblem.OrganizationId = orgId;
                clientProblem.UpUserId = userId;
                clientProblem.UpdateDate = DateTime.Now;
                clientProblemRepository.Update(clientProblem);
            }
            return clientProblemRepository.Save();
        }

        public bool DeleteClientProblem(long id, long orgId)
        {
            clientProblemRepository.DeleteOneByOrg(client => client.ProblemId == id && client.OrganizationId == orgId);
            return clientProblemRepository.Save();
        }

        public IEnumerable<ClientProblemDTO> GetClientProblemByOrgId(long orgId)
        {
            return _configurationDb.Db.Database.SqlQuery<ClientProblemDTO>(string.Format(@"select * from [Configuration].dbo.tblClientProblems
where OrganizationId={0}",orgId)).ToList();
        }
    }
}
