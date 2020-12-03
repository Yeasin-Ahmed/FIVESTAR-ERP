using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration.Interface
{
    public interface IMissingStockBusiness
    {
        MissingStock GetMissingStockOneById(long id, long orgId);
        MissingStock GetMissingStockOneByModelAndColorAndPartsAndStockType(long model, long color, long parts, string type, long orgId);
        bool SaveMissingStock(MissingStockDTO dto, long userId, long branchId, long orgId);
        IEnumerable<MissingStockDTO> GetMissingStockInfoByQuery(long? modelId, long? colorId, long? partsId, string stockType, long orgId);
    }
}
