using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration.Interface
{
   public interface IModelSSBusiness
    {
        IEnumerable<ModelSS> GetAllModel(long orgId);
        ModelSS GetModelById(long modelId, long orgId);
        bool SaveModelSS(ModelSSDTO dto, long orgId, long branchId, long userId);
        bool IsDuplicateModelName(string modelName, long orgId);
    }
}
