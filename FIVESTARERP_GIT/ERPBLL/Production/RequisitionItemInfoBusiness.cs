using ERPBLL.Common;
using ERPBLL.Production.Interface;
using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using ERPDAL.ProductionDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production
{
    public class RequisitionItemInfoBusiness : IRequisitionItemInfoBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly RequisitionItemInfoRepository _requisitionItemInfoRepository;
        public RequisitionItemInfoBusiness(IProductionUnitOfWork productionDb, RequisitionItemInfoRepository requisitionItemInfoRepository)
        {
            this._productionDb = productionDb;
            this._requisitionItemInfoRepository = requisitionItemInfoRepository;

        }
        public IEnumerable<RequisitionItemInfo> GetRequisitionItemInfos(long orgId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<RequisitionItemInfoDTO> GetRequisitionItemInfosByQuery(long? reqItemIfoId, long? floorId, long? assembly, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, long? reqInfoId, string status, string reqCode, string fromDate, string toDate, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<RequisitionItemInfoDTO>(QueryFortRequisitionItemInfos(reqItemIfoId, floorId, assembly, modelId, warehouseId, itemTypeId, itemId, reqInfoId, reqCode, fromDate, toDate, orgId)).ToList();
        }

        private string QueryFortRequisitionItemInfos(long? reqItemIfoId, long? floorId, long? assembly, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, long? reqInfoId, string status, string reqCode, string fromDate, string toDate, long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;

            param += string.Format(@" and rii.OrganizationId={0}", orgId);
            if (reqItemIfoId != null && reqItemIfoId > 0)
            {
                param += string.Format(@" and rii.ReqItemInfoId={0}", reqItemIfoId);
            }
            if (reqInfoId != null && reqInfoId > 0)
            {
                param += string.Format(@" and and rii.ReqInfoId={0}", reqInfoId);
            }
            if (floorId != null && floorId > 0)
            {
                param += string.Format(@" and and rii.ReqInfoId={0}", floorId);
            }
            if (assembly != null && assembly > 0)
            {
                param += string.Format(@" and and rii.AssemblyLineId={0}", assembly);
            }
            if(modelId != null && modelId > 0)
            {
                param += string.Format(@" and and rii.DescriptionId={0}", modelId);
            }
            if (warehouseId != null && warehouseId > 0)
            {
                param += string.Format(@" and and rii.WarehouseId={0}", warehouseId);
            }
            if (itemTypeId != null && itemTypeId > 0)
            {
                param += string.Format(@" and and rii.ItemTypeId={0}", itemTypeId);
            }
            if (itemId != null && itemId > 0)
            {
                param += string.Format(@" and and rii.ItemId={0}", itemId);
            }
            if (!string.IsNullOrEmpty(status) && status.Trim() != "")
            {
                param += string.Format(@" and ri.StateStatus ='{0}'", status);
            }
            if (!string.IsNullOrEmpty(reqCode) && reqCode.Trim() !="")
            {
                param += string.Format(@" and and ri.ReqInfoCode LIKE'%{0}%'", reqCode);
            }

            if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "" && !string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(ri.EntryDate as date) between '{0}' and '{1}'", fDate, tDate);
            }
            else if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(ri.EntryDate as date)='{0}'", fDate);
            }
            else if (!string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(ri.EntryDate as date)='{0}'", tDate);
            }

            query = string.Format(@"Select ri.ReqInfoId,rii.ReqItemInfoId,rii.FloorId,pl.LineNumber 'FloorName',rii.AssemblyLineId, al.AssemblyLineName,rii.DescriptionId,de.DescriptionName 'ModelName',rii.WarehouseId,w.WarehouseName,
rii.ItemTypeId,it.ItemName 'ItemTypeName',rii.ItemId,i.ItemName,rii.UnitId,un.UnitSymbol 'UnitName',
rii.Quantity,rii.EntryDate,app.UserName 'EntryUser',rii.UpdateDate,(Select UserName from [ControlPanel].dbo.tblApplicationUsers Where UserId = rii.UpUserId) 'UpdateUser'
From [Production].dbo.tblRequisitionItemInfo rii
Inner Join [Production].dbo.tblRequsitionInfo ri on rii.ReqInfoId = ri.ReqInfoId
Inner Join [Production].dbo.tblProductionLines pl on rii.FloorId = pl.LineId
Inner Join [Production].dbo.tblAssemblyLines al on rii.AssemblyLineId = al.AssemblyLineId
Inner Join [Inventory].dbo.tblDescriptions de on rii.DescriptionId = de.DescriptionId
Inner Join [Inventory].dbo.tblWarehouses w on rii.WarehouseId = w.Id
Inner Join [Inventory].dbo.tblItemTypes it on rii.ItemTypeId = it.ItemId
Inner Join [Inventory].dbo.tblItems i on rii.ItemId = i.ItemId
Inner Join [Inventory].dbo.tblUnits un on rii.UnitId = un.UnitId
Inner Join [ControlPanel].dbo.tblApplicationUsers app on rii.EUserId = app.UserId
Where 1=1",Utility.ParamChecker(param));
            return query;
        }
    }
}
