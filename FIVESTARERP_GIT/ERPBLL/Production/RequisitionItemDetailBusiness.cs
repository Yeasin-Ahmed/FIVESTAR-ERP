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
    public class RequisitionItemDetailBusiness : IRequisitionItemDetailBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly RequisitionItemDetailRepository _requisitionItemDetailRepository;

        public RequisitionItemDetailBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._requisitionItemDetailRepository = new RequisitionItemDetailRepository(this._productionDb);
        }

        public IEnumerable<RequisitionItemDetail> GetRequisitionItemDetails(long orgId)
        {
            return this._requisitionItemDetailRepository.GetAll(s => s.OrganizationId == orgId).ToList();
        }

        public IEnumerable<RequisitionItemDetailDTO> GetRequisitionItemDetailsByQuery(long? reqInfoId, long? reqItemInfoId, long? reqItemDetailId, long? itemTypeId, long? itemId, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<RequisitionItemDetailDTO>(QueryForRequisitionItemDetails(reqInfoId, reqItemInfoId, reqItemDetailId, itemTypeId, itemId, orgId)).ToList();
        }

        public IEnumerable<RequisitionItemDetail> GetRequisitionItemDetailsByReqItemInfos(List<long> reqInfoItems, long orgId)
        {
            return _requisitionItemDetailRepository.GetAll(s => reqInfoItems.Contains(s.ReqItemInfoId) && s.OrganizationId == orgId);
        }

        private string QueryForRequisitionItemDetails(long? reqInfoId, long? reqItemInfoId, long? reqItemDetailId, long? itemTypeId, long? itemId, long orgId)
        {
            string param = string.Empty;
            string query = string.Empty;

            param += string.Format(@" and rid.OrganizationId={0}",orgId);
            if(reqItemInfoId != null && reqItemInfoId > 0)
            {
                param += string.Format(@" and rii.ReqItemInfoId={0}", reqItemInfoId);
            }
            if(reqInfoId !=null && reqInfoId > 0)
            {
                param += string.Format(@" and ri.ReqInfoId={0}", reqInfoId);
            }
            if (reqItemDetailId != null && reqItemDetailId > 0)
            {
                param += string.Format(@" and rid.ReqItemDetailId={0}", reqItemDetailId);
            }
            if(itemTypeId !=null && itemTypeId > 0)
            {
                param += string.Format(@" and rid.ItemTypeId={0}", reqItemDetailId);
            }
            if(itemId != null && itemId > 0)
            {
                param += string.Format(@" and rid.ItemId={0}", itemId);
            }

            query = string.Format(@"Select rii.ReqItemInfoId,rid.ReqItemDetailId,rid.WarehouseId,w.WarehouseName,rid.ItemTypeId,it.ItemName 'ItemTypeName',rid.ItemId,i.ItemName,rid.ConsumptionQty
,rid.TotalQuantity,rid.EntryDate,app.UserName 'EntryUser',(Select UserName From [ControlPanel].dbo.tblApplicationUsers Where UserId = rid.UpUserId) 'UpdateUser'
,rid.UpdateDate
From [Production].dbo.tblRequisitionItemDetail rid
Inner Join [Production].dbo.tblRequisitionItemInfo rii on rid.ReqItemInfoId = rii.ReqItemInfoId
Inner Join [Production].dbo.tblRequsitionInfo ri on rii.ReqInfoId = ri.ReqInfoId
Inner Join [Inventory].dbo.tblWarehouses w on rid.WarehouseId = w.Id
Inner Join [Inventory].dbo.tblItemTypes it on rid.ItemTypeId = it.ItemId
Inner Join [Inventory].dbo.tblItems i on rid.ItemId = i.ItemId
Inner Join [ControlPanel].dbo.tblApplicationUsers app on rid.EUserId = app.UserId
Where 1=1 {0}",Utility.ParamChecker(param));

            return query;
        }


    }
}
