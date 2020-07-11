using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IProductionAssembleStockDetailBusiness
    {
        IEnumerable<ProductionAssembleStockDetail> GetProductionAssembleStockDetails(long orgId);
        bool SaveProductionAssembleStockDetailStockIn(List<ProductionAssembleStockDetailDTO> stockDetails, long userId, long orgId);
        bool SaveProductionAssembleStockDetailStockOut(List<ProductionAssembleStockDetailDTO> stockDetails, long userId, long orgId);
        bool SaveReceiveQCItems(long qcPassId, string status, long userId, long orgId);
    }
}
