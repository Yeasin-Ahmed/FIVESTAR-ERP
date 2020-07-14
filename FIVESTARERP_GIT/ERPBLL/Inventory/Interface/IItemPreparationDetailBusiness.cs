using ERPBO.Inventory.DomainModels;
using ERPBO.Inventory.DTOModel;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Inventory.Interface
{
    public interface IItemPreparationDetailBusiness
    {
        IEnumerable<ItemPreparationDetail> GetItemPreparationDetails(long orgId);
        ItemPreparationDetail GetItemPreparationDetailsById(long id,long orgId);
        IEnumerable<ItemPreparationDetail> GetItemPreparationDetailsByInfoId(long infoId, long orgId);
        IEnumerable<ItemPreparationDetail> GetItemPreparationDetailsByModelAndItem(long modelId, long itemId, long orgId);
        IEnumerable<ItemPreparationDetailWithInfoDTO> GetItemPreparationDetailWithInfo(long infoId, long orgId);
    }
}
