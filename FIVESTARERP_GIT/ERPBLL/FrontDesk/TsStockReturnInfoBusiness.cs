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
   public class TsStockReturnInfoBusiness: ITsStockReturnInfoBusiness
    {
        private readonly IFrontDeskUnitOfWork _frontDeskUnitOfWork;
        private readonly TsStockReturnInfoRepository _tsStockReturnInfoRepository;
        private readonly IJobOrderBusiness _jobOrderBusiness;

        public TsStockReturnInfoBusiness(IFrontDeskUnitOfWork frontDeskUnitOfWork,IJobOrderBusiness jobOrderBusiness)
        {
            this._frontDeskUnitOfWork = frontDeskUnitOfWork;
            this._tsStockReturnInfoRepository = new TsStockReturnInfoRepository(this._frontDeskUnitOfWork);
            this._jobOrderBusiness = jobOrderBusiness;
        }

        public IEnumerable<DashbordTsPartsReturnDTO> DashboardReturnParts(long orgId, long branchId)
        {
            return this._frontDeskUnitOfWork.Db.Database.SqlQuery<DashbordTsPartsReturnDTO>(
                string.Format(@"select tr.ReturnInfoId,tr.JobOrderId,jo.JobOrderCode,RequsitionCode,tr.StateStatus,tr.EntryDate from tblTsStockReturnInfo tr
                Inner join tblJobOrders jo on tr.JobOrderId=jo.JodOrderId
                Where  tr.StateStatus='Stock-Return' and  jo.OrganizationId={0} and jo.BranchId={1}", orgId, branchId)).ToList();
        }

        public TsStockReturnInfo GetAllReturnId(long returnInfoId, long orgId, long branchId)
        {
            return _tsStockReturnInfoRepository.GetOneByOrg(info => info.ReturnInfoId == returnInfoId && info.OrganizationId == orgId && info.BranchId == branchId);
        }

        public bool SaveTsReturnStock(List<TsStockReturnInfoDTO> returnInfoList, long userId, long orgId, long branchId)
        {
            //bool IsSuccess = false;
            var jobId = _jobOrderBusiness.GetJobOrderById(returnInfoList.LastOrDefault().JobOrderId, orgId);
            List<TsStockReturnInfo> returnInfoLists = new List<TsStockReturnInfo>();
            foreach(var item in returnInfoList)
            {
                TsStockReturnInfo info = new TsStockReturnInfo();
                info.JobOrderId = item.JobOrderId;
                info.ReqInfoId = item.ReqInfoId;
                info.RequsitionCode = item.RequsitionCode;
                info.StateStatus = "Stock-Return";
                info.EUserId = userId;
                info.EntryDate = DateTime.Now;
                info.OrganizationId = orgId;
                info.BranchId = jobId.BranchId;
                List<TsStockReturnDetail> detail = new List<TsStockReturnDetail>();
                foreach(var details in item.TsStockReturnDetails)
                {
                    TsStockReturnDetail d = new TsStockReturnDetail();
                    d.ReqInfoId = details.ReqInfoId;
                    d.JobOrderId = details.JobOrderId;
                    d.RequsitionCode = details.RequsitionCode;
                    d.PartsId = details.PartsId;
                    d.Quantity = details.Quantity;
                    d.BranchId = jobId.BranchId;
                    d.OrganizationId = orgId;
                    d.EUserId = userId;
                    d.EntryDate = DateTime.Now;
                    detail.Add(d);
                }
                info.TsStockReturnDetails = detail;
                _tsStockReturnInfoRepository.Insert(info);
            }
            _tsStockReturnInfoRepository.InsertAll(returnInfoLists);
            
            return _tsStockReturnInfoRepository.Save();
        }

        public bool UpdateReturnInfoStatus(long returnInfoId, string status, long userId, long orgId, long branchId)
        {
                var returnInfo = GetAllReturnId(returnInfoId, orgId,branchId);
                if (returnInfo != null && returnInfo.StateStatus == "Stock-Return")
                {
                returnInfo.StateStatus = "Stock-Closed";
                returnInfo.UpUserId = userId;
                returnInfo.UpdateDate = DateTime.Now;
                _tsStockReturnInfoRepository.Update(returnInfo);
                }
                return _tsStockReturnInfoRepository.Save();
        }
    }
}
