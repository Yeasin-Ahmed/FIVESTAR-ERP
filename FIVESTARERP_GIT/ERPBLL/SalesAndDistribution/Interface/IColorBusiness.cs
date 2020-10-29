using ERPBO.SalesAndDistribution.DomainModels;
using ERPBO.SalesAndDistribution.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.SalesAndDistribution.Interface
{
    public interface IColorBusiness
    {
        IEnumerable<Color> GetColors(long orgId);
        Color GetColorById(long colorId, long orgId);
        bool SaveColor(ColorDTO dto, long userId, long orgId);
    }
}
