using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IProductionFaultyStockDetailBusiness
    {
        IEnumerable<ProductionFaultyStockDetail> GetProductionFaultyInfoStocks(long orgId);
        bool SaveProductionFaultyStockIn(List<ProductionFaultyStockDetailDTO> stockDetailsDTO, long userId,long orgId);
        bool SaveProductionFaultyStockOut(List<ProductionFaultyStockDetailDTO> stockDetailsDTO, long userId, long orgId);
        bool StockInByRepairSection(long transferId, string status,long orgId, long userId);
    }
}
