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
   public class ModelSSBusiness: IModelSSBusiness
    {
        private readonly IConfigurationUnitOfWork _configurationDb; // database
        private readonly ModelSSRepository modelSSRepository; // repo
        public ModelSSBusiness(IConfigurationUnitOfWork configurationDb)
        {
            this._configurationDb = configurationDb;
            modelSSRepository = new ModelSSRepository(this._configurationDb);
        }

        public IEnumerable<ModelSS> GetAllModel(long orgId)
        {
            return modelSSRepository.GetAll(m => m.OrganizationId == orgId).ToList();
        }

        public ModelSS GetModelById(long modelId, long orgId)
        {
            return modelSSRepository.GetOneByOrg(m => m.ModelId == modelId && m.OrganizationId == orgId);
        }

        public bool IsDuplicateModelName(string modelName, long orgId)
        {
            throw new NotImplementedException();
        }

        public bool SaveModelSS(ModelSSDTO dto, long orgId, long branchId, long userId)
        {
            ModelSS model = new ModelSS();
            if (dto.ModelId == 0)
            {
                model.ModelName = dto.ModelName;
                model.Remarks = dto.Remarks;
            }
            else
            {

            }
            return modelSSRepository.Save();
        }
    }
}
