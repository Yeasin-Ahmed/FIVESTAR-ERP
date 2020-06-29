using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IFaultyItemStockDetailBusiness
    {
        IEnumerable<FaultyItemStockDetail> GetFaultyItemStocks(long orgId);
        bool SaveFaultyItemStockIn(List<FaultyItemStockDetailDTO> stockDetails, long userId,long orgId);
        bool SaveFaultyItemStockOut(List<FaultyItemStockDetailDTO> stockDetails, long userId, long orgId);
    }
}
