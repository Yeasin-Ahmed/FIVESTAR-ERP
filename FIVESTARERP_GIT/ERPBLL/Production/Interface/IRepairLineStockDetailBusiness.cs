using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using ERPBO.Production.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IRepairLineStockDetailBusiness
    {
        IEnumerable<RepairLineStockDetail> GetRepairLineStockDetails(long orgId);
        bool SaveRepairLineStockIn(List<RepairLineStockDetailDTO> repairLineStockDetailDTO, long userId, long orgId);
        bool SaveRepairLineStockOut(List<RepairLineStockDetailDTO> repairLineStockDetailDTO, long userId, long orgId, string flag);
        bool StockOutByFaultyItem(List<FaultyItemStockDetailDTO> details, long userId, long orgId);
        bool StockInByRepairSectionRequisition(long reqId, string status, long userId, long orgId);
        // bool StockOutByAddingFaultyWithQRCode(FaultyInfoByQRCodeDTO model, long userId, long orgId);
        Task<bool> SaveRepairLineStockOutAsync(List<RepairLineStockDetailDTO> repairLineStockDetailDTO, long userId, long orgId, string flag);
    }
}
