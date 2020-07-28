using ERPBLL.Common;
using ERPBLL.Production.Interface;
using ERPBO.Inventory.DTOModel;
using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using ERPDAL.ProductionDAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ERPBLL.Production
{
    public class RequsitionDetailBusiness : IRequsitionDetailBusiness
    {
        private readonly IProductionUnitOfWork _productionDb; // database
        private readonly RequsitionDetailRepository _requsitionDetailRepository; // table
        private readonly IRequsitionInfoBusiness _requsitionInfoBusiness;
        private readonly IRequisitionItemDetailBusiness _requisitionItemDetailBusiness;
        public RequsitionDetailBusiness(IProductionUnitOfWork productionDb, IRequsitionInfoBusiness requsitionInfoBusiness, IRequisitionItemDetailBusiness requisitionItemDetailBusiness)
        {
            this._productionDb = productionDb;
            this._requsitionDetailRepository = new RequsitionDetailRepository(this._productionDb);
            this._requsitionInfoBusiness = requsitionInfoBusiness;
            this._requisitionItemDetailBusiness = requisitionItemDetailBusiness;
        }

        public IEnumerable<RequsitionDetail> GetAllReqDetailByOrgId(long orgId)
        {
            return _requsitionDetailRepository.GetAll(unit => unit.OrganizationId == orgId).ToList();
        }
        public bool SaveReqDetails(RequsitionDetailDTO detailDTO, long userId, long orgId)
        {

            RequsitionDetail requsitionDetail = new RequsitionDetail();
            if (detailDTO.ReqDetailId == 0)
            {
                requsitionDetail.ItemTypeId = detailDTO.ItemTypeId;
                requsitionDetail.ItemId = detailDTO.ItemId;
                requsitionDetail.Quantity = detailDTO.Quantity;
                requsitionDetail.UnitId = detailDTO.UnitId;
                requsitionDetail.Remarks = requsitionDetail.Remarks;
                requsitionDetail.EUserId = userId;
                requsitionDetail.EntryDate = DateTime.Now;
                requsitionDetail.OrganizationId = orgId;
                _requsitionDetailRepository.Insert(requsitionDetail);
            }
            else
            {
                requsitionDetail.ItemTypeId = detailDTO.ItemTypeId;
                requsitionDetail.ItemId = detailDTO.ItemId;
                requsitionDetail.Quantity = detailDTO.Quantity;
                requsitionDetail.UnitId = detailDTO.UnitId;
                requsitionDetail.Remarks = requsitionDetail.Remarks;
                requsitionDetail.UpUserId = userId;
                requsitionDetail.UpdateDate = DateTime.Now;
                requsitionDetail.OrganizationId = orgId;
                _requsitionDetailRepository.Update(requsitionDetail);
            }
            return _requsitionDetailRepository.Save();
        }

        public IEnumerable<RequsitionDetail> GetRequsitionDetailByReqId(long id, long orgId)
        {
            return _requsitionDetailRepository.GetAll(rd => rd.ReqInfoId == id && rd.OrganizationId == orgId).ToList();
        }

        public bool SaveRequisitionDetail(ReqInfoDTO reqInfoDTO,long userId, long orgId)
        {
            bool IsSuccess = false;
            var reqInfo = _requsitionInfoBusiness.GetRequisitionById(reqInfoDTO.ReqInfoId, orgId);
            if (reqInfo != null && reqInfo.ReqInfoId > 0 && reqInfo.StateStatus == RequisitionStatus.Pending)
            {
                foreach (var item in reqInfoDTO.ReqDetails)
                {
                    var reDetailInDb = GetRequsitionDetailById(item.ReqDetailId, orgId);
                    if(reDetailInDb != null)
                    {
                        reDetailInDb.Quantity = item.Quantity;
                        reDetailInDb.UpUserId = userId;
                        reDetailInDb.UpdateDate = DateTime.Now;
                        _requsitionDetailRepository.Update(reDetailInDb);
                    }
                }
            }
            IsSuccess = _requsitionDetailRepository.Save();
            return IsSuccess;
        }

        public RequsitionDetail GetRequsitionDetailById(long id, long orgId)
        {
            return _requsitionDetailRepository.GetAll(rd => rd.ReqDetailId == id && rd.OrganizationId == orgId).FirstOrDefault();
        }

        public IEnumerable<RequsitionDetailDTO> GetRequisitionDetailsByQuery(long? reqInfoId, long? reqDetailId, long? itemTypeId, long? itemId, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<RequsitionDetailDTO>(QueryForRequisitionDetails(reqInfoId, reqDetailId, itemTypeId,itemId,orgId)).ToList();
        }

        private string QueryForRequisitionDetails(long? reqInfoId, long? reqDetailId, long? itemTypeId, long? itemId, long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;
            param += string.Format(@" and rd.OrganizationId={0}",orgId);
            if(reqInfoId != null && reqInfoId > 0)
            {
                param += string.Format(@" and ri.ReqInfoId={0}", reqInfoId);
            }
            if (reqDetailId != null && reqDetailId > 0)
            {
                param += string.Format(@" and rd.ReqDetailId={0}", reqDetailId);
            }
            if (itemTypeId != null && itemTypeId > 0)
            {
                param += string.Format(@" and rd.ItemTypeId={0}", itemTypeId);
            }
            if (itemId != null && itemId > 0)
            {
                param += string.Format(@" and rd.ItemId={0}", itemId);
            }

            query = string.Format(@"Select ri.ReqInfoId,rd.ReqDetailId,rd.ItemTypeId,it.ItemName 'ItemTypeName',rd.ItemId,i.ItemName,rd.UnitId,
u.UnitSymbol 'UnitName',rd.Quantity,rd.EntryDate,app.UserName 'EntryUser',rd.UpdateDate,(Select UserName From [ControlPanel].dbo.tblApplicationUsers Where UserId = rd.UpUserId) 'UpdateUser' 
From [Production].dbo.tblRequsitionDetails rd
Inner Join [Production].dbo.tblRequsitionInfo ri on rd.ReqInfoId = ri.ReqInfoId
Inner Join [Inventory].dbo.tblItemTypes it on rd.ItemTypeId = it.ItemId
Inner Join [Inventory].dbo.tblItems i on rd.ItemId = i.ItemId
Inner Join [Inventory].dbo.tblUnits u on i.UnitId = u.UnitId
Inner Join [ControlPanel].dbo.tblApplicationUsers app on rd.EUserId = app.UserId
Where 1=1 {0}",Utility.ParamChecker(param));

            return query;
        }

        
    }
}
