using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IItemReturnDetailBusiness
    {
        IEnumerable<ItemReturnDetail> GetItemReturnDetails(long OrgId);
        IEnumerable<ItemReturnDetail> GetItemReturnDetailsByReturnInfoId(long OrgId,long returnInfoId);
        IEnumerable<ItemReturnDetailListDTO> GetItemReturnDetailList(string refNum, string returnType, string faultyCase, long? lineId, long? warehouseId, string status, long? itemTypeId, long? itemId, string fromDate, string toDate);
    }
}
