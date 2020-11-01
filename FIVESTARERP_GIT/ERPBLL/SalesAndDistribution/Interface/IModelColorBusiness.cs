using ERPBO.SalesAndDistribution.DomainModels;
using ERPBO.SalesAndDistribution.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.SalesAndDistribution.Interface
{
    public interface IModelColorBusiness
    {
        ModelColors GetModelColors(long modelId, long colroId, long orgId);
        IEnumerable<ModelColors> GetModelColorsByOrg(long orgId);
        IEnumerable<ModelColorDTO> GetModelColorsByModel(long modelId,long orgId);
        bool SaveModelColors(long modelId, List<long> colors, long userId, long orgId);
    }
}
