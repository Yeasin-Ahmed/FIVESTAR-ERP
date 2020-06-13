using ERPBO.Production.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IAssemblyLineStockInfoBusiness
    {
        IEnumerable<AssemblyLineStockInfo> GetAssemblyLineStockInfos(long orgId);
        IEnumerable<AssemblyLineStockInfo> GetAssemblyLineStockInfoByAssemblyAndItemId(long assemblyId,long itemId,long orgId);
        AssemblyLineStockInfo GetAssemblyLineStockInfoByAssemblyAndItemAndModelId(long assemblyId, long itemId,long modelId ,long orgId);
    }
}
