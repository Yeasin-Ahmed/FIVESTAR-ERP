using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IQCItemStockDetailBusiness
    {
        IEnumerable<QCItemStockDetail> GetQCItemStockDetails(long orgId);
        bool SaveQCItemStockOut(List<QCItemStockDetailDTO> items,long userId, long orgId);
        bool SaveQCItemStockIn(List<QCItemStockDetailDTO> items, long userId, long orgId);
    }
}
