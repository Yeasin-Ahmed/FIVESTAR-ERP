using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration.Interface
{
   public interface IColorSSBusiness
    {
        IEnumerable<ColorSS> GetAllColorsByOrgId(long orgId);
        ColorSS GetOneColorsById(long colorId, long orgId);
        bool SaveColorSS(ColorSSDTO dto, long orgId, long branchId, long userId);
        bool IsDuplicateColor(string color, long orgId);
    }
}
