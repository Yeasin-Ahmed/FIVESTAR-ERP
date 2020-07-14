using ERPBO.Inventory.DomainModel;
using ERPBO.Inventory.DomainModels;
using ERPBO.Inventory.DTOModel;
using ERPBO.Inventory.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Inventory.Interface
{
    public interface IItemPreparationInfoBusiness
    {
        IEnumerable<ItemPreparationInfo> GetItemPreparationInfosByOrgId(long orgId);
        ItemPreparationInfo GetItemPreparationInfoOneByOrgId(long id,long orgId);
        bool SaveItemPreparations(ItemPreparationInfoDTO info, List<ItemPreparationDetailDTO> details, long userId, long orgId);
        ItemPreparationInfo IsDuplicationItemPreparation(long itemId, long modelId, long orgId);
        bool DeleteItemPreparation(long id, long userId, long orgId);
        ItemPreparationInfo GetPreparationInfoByModelAndItem(long modelId, long itemId, long orgId);

        ItemPreparationInfo GetPreparationInfoByModelAndItemAndType(string type,long modelId, long itemId, long orgId);
        Task<ItemPreparationInfo> GetPreparationInfoByModelAndItemAndTypeAsync(string type, long modelId, long itemId, long orgId);
        bool IsItemPreparationExistWithThistype(string type,long modelId, long itemId, long orgId);
    }
}
