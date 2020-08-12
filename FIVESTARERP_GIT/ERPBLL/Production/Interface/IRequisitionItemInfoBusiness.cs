using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IRequisitionItemInfoBusiness
    {
        IEnumerable<RequisitionItemInfo> GetRequisitionItemInfos(long orgId);
        IEnumerable<RequisitionItemInfo> GetRequisitionItemInfosByReqInfoId(long reqInfoId,long orgId);
        IEnumerable<RequisitionItemInfoDTO> GetRequisitionItemInfosByQuery(long? reqItemIfoId, long? floorId, long? assembly, long? packagingLine, long? repairLine, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, long? reqInfoId,string status, string reqCode, string fromDate, string toDate, long orgId);
        RequsitionInfoDTO GetRequsitionInfoModalProcessData(long? floorId, long? assemblyId, long? packagingLine, long? repairLine, long? warehouseId, long? modelId, string reqCode, string reqType, string reqFor, string fromDate, string toDate, string status, string reqFlag, long? reqInfoId, long orgId);
        bool SaveRequisitionItemStocksInAssemblyOrRepairOrPackaging(long reqInfoId, string status, long userId, long orgId);
    }
}
