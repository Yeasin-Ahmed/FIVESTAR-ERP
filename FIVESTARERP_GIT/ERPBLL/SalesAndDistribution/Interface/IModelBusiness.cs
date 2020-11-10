using ERPBO.SalesAndDistribution.DomainModels;
using ERPBO.SalesAndDistribution.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.SalesAndDistribution.Interface
{
    public interface IModelBusiness
    {
        List<ModelColor> GetModelColors(long modelId, long orgId);
        IEnumerable<Description> GetModels(long orgId);
        Description GetModelById(long id, long orgId);
        bool SaveModel(DescriptionDTO dto, long userId, long orgId);

    }
}
