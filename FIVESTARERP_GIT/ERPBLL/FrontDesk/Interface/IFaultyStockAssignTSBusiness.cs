using ERPBO.FrontDesk.DomainModels;
using ERPBO.FrontDesk.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.FrontDesk.Interface
{
    public interface IFaultyStockAssignTSBusiness
    {
        bool SaveFaultyStockAssignTS(long ts, long[] jobAssign, long userId, long orgId, long branchId);
        IEnumerable<FaultyStockAssignTSDTO> GetFaultyStockAssignTsByOrgId(long orgId, long branchId);
        FaultyStockAssignTS GetFaultyStockAssignTsOneById(long id, long orgId, long branchId);
        bool SaveFaultyStockItemsByAssignTS(List<FaultyStockAssignTSDTO> dto, long userId, long orgId, long branchId);
        bool SaveFaultyStockRepairedItems(List<int> dto, long userId, long orgId, long branchId);
    }
}
