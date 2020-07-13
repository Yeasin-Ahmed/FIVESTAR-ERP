using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IProductionToPackagingStockTransferDetailBusiness
    {
        IEnumerable<ProductionToPackagingStockTransferDetail> GetProductionToPackagingStockTransferDetails(long orgId);
        IEnumerable<ProductionToPackagingStockTransferDetail> GetProductionToPackagingStockTransferDetailsByInfoId(long transferInfoId,long orgId);
        IEnumerable<ProductionToPackagingStockTransferDetailDTO> GetProductionToPackagingStockTransferDetailsByQuery(long transferInfoId,long orgId);
    }
}
