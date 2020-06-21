using ERPBO.Production.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface ITransferStockToPackagingLine2DetailBusiness
    {
        IEnumerable<TransferStockToPackagingLine2Detail> GetTransferFromPLDetails(long orgId);
        IEnumerable<TransferStockToPackagingLine2Detail> GetTransferFromPLDetailByInfo(long infoId,long orgId);
    }
}
