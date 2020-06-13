using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IQualityControlBusiness
    {
        IEnumerable<QualityControl> GetQualityControls(long orgId);
        QualityControl GetQualityControlById(long id,long orgId);
        bool SaveQualityControl(QualityControlDTO dto, long userId, long orgId);
    }
}
