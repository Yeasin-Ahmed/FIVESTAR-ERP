using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IStockItemReturnDetailBusiness
    {
        IEnumerable<StockItemReturnDetailDTO> GetStockItemReturnDetails(long infoId, long orgId);
        IEnumerable<StockItemReturnDetail> GetStockItemReturnDetailsByInfo(long infoId, long orgId);
    }
}
