using ERPBO.FrontDesk.DomainModels;
using ERPBO.FrontDesk.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.FrontDesk.Interface
{
   public interface ITechnicalServicesStockBusiness
    {
        IEnumerable<TechnicalServicesStock> GetAllTechnicalServicesStock(long id, long orgId, long branchId);
        bool SaveTechnicalServicesStockIn(List<TechnicalServicesStockDTO> servicesStockDTOs, long userId, long orgId,long branchId,long modelId);
        bool SaveTechnicalStockInRequistion(long id, string status, long orgId, long userId,long branchId);
        IEnumerable<TechnicalServicesStock> GetRequsitionStockByStockId(long id, long orgId, long branchId);
        IEnumerable<TSStockByRequsitionDTO> GetStockByJobOrder(long? jobOrderId, long tsId, long orgId, long branchId, string roleName);
        bool SaveTechnicalServicesStockOut(List<TechnicalServicesStockDTO> servicesStockDTOs,long jobOrderId, long userId, long orgId, long branchId);

        bool SaveJobSignOutWithStock(TSStockInfoDTO dto, long userId, long orgId, long branchId);
        IEnumerable<TechnicalServicesStockDTO> GetUsedParts(long? partsId, long? tsId, long orgId, long branchId, string fromDate, string toDate, string jobCode);
    }
}
