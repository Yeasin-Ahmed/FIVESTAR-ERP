using ERPBO.Production.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface ITransferStockToQCDetailBusiness
    {
        IEnumerable<TransferStockToQCDetail> GetTransferStockToQCDetails(long orgId);
        IEnumerable<TransferStockToQCDetail> GetTransferStockToQCDetailByInfo(long infoId,long orgId);
    }
}
