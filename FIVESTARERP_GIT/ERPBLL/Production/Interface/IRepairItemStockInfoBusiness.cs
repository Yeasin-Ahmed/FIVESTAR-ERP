﻿using ERPBO.Production.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IRepairItemStockInfoBusiness
    {
        IEnumerable<RepairItemStockInfo> GetRepairItemStocks(long orgId);
        IEnumerable<RepairItemStockInfo> GetRepairItemStockInfById(long repairLineId, long modelId, long itemId, long orgId);
        IEnumerable<RepairItemStockInfo> GetRepairItemStockInfoByQC(long qcId, long modelId, long itemId, long orgId);
        RepairItemStockInfo GetRepairItem(long qcId, long repairLineId, long modelId, long itemId, long orgId);
    }
}