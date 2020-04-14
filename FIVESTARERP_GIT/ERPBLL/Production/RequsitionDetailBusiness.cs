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
        public RequsitionDetailBusiness(IProductionUnitOfWork productionDb, IRequsitionInfoBusiness requsitionInfoBusiness)
        {
            this._productionDb = productionDb;
            this._requsitionDetailRepository = new RequsitionDetailRepository(this._productionDb);
            this._requsitionInfoBusiness = requsitionInfoBusiness;
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
    }
}
