﻿using ERPBLL.FrontDesk.Interface;
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

        public TsStockReturnInfoBusiness(IFrontDeskUnitOfWork frontDeskUnitOfWork)
        {
            this._frontDeskUnitOfWork = frontDeskUnitOfWork;
            this._tsStockReturnInfoRepository = new TsStockReturnInfoRepository(this._frontDeskUnitOfWork);
        }

        public IEnumerable<DashbordTsPartsReturnDTO> DashboardReturnParts(long orgId, long branchId)
        {
            return this._frontDeskUnitOfWork.Db.Database.SqlQuery<DashbordTsPartsReturnDTO>(
                string.Format(@"select tr.ReturnInfoId,tr.JobOrderId,jo.JobOrderCode,RequsitionCode,tr.StateStatus,tr.EntryDate from tblTsStockReturnInfo tr
                Inner join tblJobOrders jo on tr.JobOrderId=jo.JodOrderId
                Where  tr.StateStatus='Stock-Return' and  jo.OrganizationId={0} and jo.BranchId={1}", orgId, branchId)).ToList();
        }

        public bool SaveTsReturnStock(List<TsStockReturnInfoDTO> returnInfoList, long userId, long orgId, long branchId)
        {
            //bool IsSuccess = false;
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
                info.BranchId = branchId;
                List<TsStockReturnDetail> detail = new List<TsStockReturnDetail>();
                foreach(var details in item.TsStockReturnDetails)
                {
                    TsStockReturnDetail d = new TsStockReturnDetail();
                    d.ReqInfoId = details.ReqInfoId;
                    d.JobOrderId = details.JobOrderId;
                    d.RequsitionCode = details.RequsitionCode;
                    d.PartsId = details.PartsId;
                    d.Quantity = details.Quantity;
                    d.BranchId = branchId;
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
    }
}