using ERPBO.Configuration.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration.Interface
{
    public interface IStockTransferDetailModelToModelBusiness
    {
        IEnumerable<StockTransferDetailModelToModel> GetAllTransferDetailMMByInfoId(long transferId, long orgId);
    }
}
