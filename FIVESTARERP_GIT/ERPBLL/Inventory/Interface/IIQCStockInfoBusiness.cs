using ERPBO.Inventory.DomainModels;
using ERPBO.Inventory.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Inventory.Interface
{
    public interface IIQCStockInfoBusiness
    {
        IEnumerable<IQCStockInfo> GetAllIQCStockInfoByOrgId(long orgId);
        IEnumerable<IQCStockInfoDTO> GetAllIQCStockInformationList(long? warehouseId, long? modelId, long? itemTypeId, long? itemId, string lessOrEq, long orgId);
    }
}
