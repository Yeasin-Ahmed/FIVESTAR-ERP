using ERPBLL.Common;
using ERPBLL.FrontDesk.Interface;
using ERPBO.FrontDesk.DomainModels;
using ERPBO.FrontDesk.DTOModels;
using ERPDAL.FrontDeskDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.FrontDesk
{
   public class RequsitionInfoForJobOrderBusiness: IRequsitionInfoForJobOrderBusiness
    {
        private readonly IFrontDeskUnitOfWork _frontDeskUnitOfWork;//db
        private readonly RequsitionInfoForJobOrderRepository requsitionInfoForJobOrderRepository;

        public RequsitionInfoForJobOrderBusiness(IFrontDeskUnitOfWork frontDeskUnitOfWork)
        {
            this._frontDeskUnitOfWork = frontDeskUnitOfWork;
            this.requsitionInfoForJobOrderRepository = new RequsitionInfoForJobOrderRepository(this._frontDeskUnitOfWork);
        }

        public IEnumerable<RequsitionInfoForJobOrder> GetAllRequsitionInfoForJob(long orgId, long branchId)
        {
            return requsitionInfoForJobOrderRepository.GetAll(info => info.OrganizationId == orgId && info.BranchId == branchId).ToList();
        }

        public IEnumerable<RequsitionInfoForJobOrder> GetAllRequsitionInfoForJobOrder(long joborderId,long orgId, long branchId)
        {
            return requsitionInfoForJobOrderRepository.GetAll(info =>info.JobOrderId== joborderId && info.OrganizationId == orgId && info.BranchId == branchId).ToList();
        }

        public RequsitionInfoForJobOrder GetAllRequsitionInfoForJobOrderId(long reqInfoId, long orgId)
        {
            return requsitionInfoForJobOrderRepository.GetOneByOrg(info => info.RequsitionInfoForJobOrderId == reqInfoId && info.OrganizationId == orgId);
        }

        public bool SaveRequisitionInfoForJobOrder(RequsitionInfoForJobOrderDTO requsitionInfoDTO, List<RequsitionDetailForJobOrderDTO> details, long userId, long orgId, long branchId)
        {
            bool IsSuccess = false;
            RequsitionInfoForJobOrder requsitionInfo = new RequsitionInfoForJobOrder();
            if (requsitionInfoDTO.RequsitionInfoForJobOrderId == 0)
            {
                requsitionInfo.RequsitionCode = ("REQ-" + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss"));
                requsitionInfo.SWarehouseId = requsitionInfoDTO.SWarehouseId;
                requsitionInfo.JobOrderId = requsitionInfoDTO.JobOrderId;

                requsitionInfo.StateStatus = RequisitionStatus.Pending;
                requsitionInfo.Remarks = requsitionInfoDTO.Remarks;
                requsitionInfo.EntryDate = DateTime.Now;
                requsitionInfo.EUserId = userId;
                requsitionInfo.OrganizationId = orgId;
                requsitionInfo.BranchId = branchId;
                List<RequsitionDetailForJobOrder> requsitionDetails = new List<RequsitionDetailForJobOrder>();

                foreach (var item in details)
                {
                    RequsitionDetailForJobOrder requsitionDetail = new RequsitionDetailForJobOrder();
                    requsitionDetail.RequsitionDetailForJobOrderId = item.RequsitionDetailForJobOrderId;
                    requsitionDetail.PartsId = item.PartsId;
                    requsitionDetail.Quantity = item.Quantity;
                    requsitionDetail.JobOrderId = requsitionInfo.JobOrderId;
                    requsitionDetail.Remarks = item.Remarks;
                    requsitionDetail.SWarehouseId = requsitionInfo.SWarehouseId;
                    requsitionDetail.EUserId = userId;
                    requsitionDetail.EntryDate = DateTime.Now;
                    requsitionDetail.OrganizationId = orgId;
                    requsitionDetail.BranchId = branchId;
                    requsitionDetails.Add(requsitionDetail);
                }
                requsitionInfo.RequsitionDetailForJobOrders = requsitionDetails;
                requsitionInfoForJobOrderRepository.Insert(requsitionInfo);
                IsSuccess = requsitionInfoForJobOrderRepository.Save();
            }
            else
            {
            }
            return IsSuccess;
        }

        public bool SaveRequisitionStatus(long reqId, string status, long userId, long orgId, long branchId)
        {
            var reqInfo = requsitionInfoForJobOrderRepository.GetOneByOrg(req => req.RequsitionInfoForJobOrderId == reqId && req.OrganizationId == orgId && req.BranchId==branchId);
            if (reqInfo != null)
            {
                reqInfo.StateStatus = status;
                reqInfo.UpUserId = userId;
                requsitionInfoForJobOrderRepository.Update(reqInfo);
            }
            return requsitionInfoForJobOrderRepository.Save();
        }
    }
}
