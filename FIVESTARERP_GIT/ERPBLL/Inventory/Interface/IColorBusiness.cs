using ERPBO.Inventory.DomainModels;
using ERPBO.Inventory.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Inventory.Interface
{
    public interface IColorBusiness
    {
        bool SaveColor(ColorDTO dto, long userId, long orgId);
        bool IsDuplicateColorName(string colorName, long id, long orgId);
        IEnumerable<Color> GetAllColorByOrgId(long orgId);
        Color GetColorOneByOrgId(long id, long orgId);
        bool IsDuplicateColor(long colorId, string colorName, long orgId);
    }
}
