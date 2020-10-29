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
        IEnumerable<Model> GetModels(long orgId);
        Model GetModelById(long id, long orgId);
        bool SaveModel(ModelDTO dto,long [] colors, long userId, long orgId);
    }
}
