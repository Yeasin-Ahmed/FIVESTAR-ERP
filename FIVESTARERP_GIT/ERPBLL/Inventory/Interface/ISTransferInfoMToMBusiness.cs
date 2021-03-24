using ERPBO.Inventory.DomainModels;
using ERPBO.Inventory.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Inventory.Interface
{
   public interface ISTransferInfoMToMBusiness
    {
        bool SaveStockTransferMToM(StockTransferInfoMToMDTO dto, long userId, long orgId);
        IEnumerable<StockTransferInfoMToMDTO> GetAllTransfer(long orgId);
        StockTransferInfoMToM GetDataOneByOrgId(long id, long orgId);
    }
}
