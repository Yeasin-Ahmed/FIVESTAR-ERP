using ERPBLL.Common;
using ERPBLL.SalesAndDistribution.Interface;
using ERPBO.SalesAndDistribution.DomainModels;
using ERPBO.SalesAndDistribution.DTOModels;
using ERPDAL.SalesAndDistributionDAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ERPBLL.SalesAndDistribution
{
    public class DealerRequisitionInfoBusiness : IDealerRequisitionInfoBusiness
    {
        private readonly ISalesAndDistributionUnitOfWork _salesAndDistributionDb;
        private readonly IDealerBusiness _dealerBusiness;
        private readonly DealerRequisitionInfoRepository _dealerRequisitionInfoRepository;
        public DealerRequisitionInfoBusiness(ISalesAndDistributionUnitOfWork salesAndDistribution, IDealerBusiness dealerBusiness)
        {
            this._salesAndDistributionDb = salesAndDistribution;
            this._dealerBusiness = dealerBusiness;
            this._dealerRequisitionInfoRepository = new DealerRequisitionInfoRepository(this._salesAndDistributionDb);
        }

        public IEnumerable<DealerRequisitionInfoDTO> GetDealerRequisitionInfos(long? dealerId, long? srId,long? districtId, long? zoneId, string refNum, string status, string fromDate, string toDate, string flag, long orgId, string role, long? userId)
        {
            string query = string.Empty;
            string param = string.Empty;
            
            if (srId != null && srId > 0)
            {

            }
            if (districtId != null && districtId > 0)
            {
                param += string.Format(@" and d.DistrictId ={0}", districtId);
            }
            if (zoneId != null && zoneId > 0)
            {
                param += string.Format(@" and d.ZoneId ={0}", zoneId);
            }
            if (!string.IsNullOrEmpty(refNum) && refNum.Trim() != ""){
                param += string.Format(@" and ri.RequisitionCode LIKE '%{0}%'", refNum.Trim());
            }
            if (!string.IsNullOrEmpty(status) && status.Trim() != "")
            {
                param += string.Format(@" and ri.StateStatus='{0}'", status.Trim());
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
            if (flag == "dealer")
            {
                if (dealerId != null && dealerId > 0)
                {
                    param += string.Format(@" and d.UserId={0}", dealerId);
                }
                query = string.Format(@"Select ri.DREQInfoId,ri.RequisitionCode,ri.DealerId,ri.StateStatus,ri.Remarks,ri.EntryDate,
ri.EUserId,ri.UpdateDate,ri.UpUserId,ri.ApprovedDate,ri.ApprovedBy,ri.DealerId,d.DealerName,app.UserName 'EntryUser',
(Select Count(*) from tblDealerRequisitionDetail Where DREQInfoId =ri.DREQInfoId) 'ItemCount'
From tblDealerRequisitionInfo ri
Inner Join [ControlPanel].dbo.tblApplicationUsers app on ri.EUserId = app.UserId
Inner Join tblDealer d on ri.DealerId = d.DealerId
Where d.OrganizationId={0} {1} ", orgId, Utility.ParamChecker(param));
            }
            else
            {

            }
            return this._salesAndDistributionDb.Db.Database.SqlQuery<DealerRequisitionInfoDTO>(query).ToList();
        }

        public bool SaveDealerRequisition(DealerRequisitionInfoDTO model, long userId, long orgId)
        {
            var dealer = model.Flag == "Dealer" ? _dealerBusiness.GetDealerByUserId(userId, orgId) : _dealerBusiness.GetDealerById(model.DealerId, orgId);
            if (dealer != null)
            {
                DealerRequisitionInfo dealerRequisitionInfo = new DealerRequisitionInfo()
                {
                    DealerId = dealer.DealerId,
                    StateStatus = RequisitionStatus.Pending,
                    EUserId = userId,
                    BranchId = 0,
                    OrganizationId = orgId,
                    EntryDate = DateTime.Now,
                    Remarks = "Requisition By Dealer"
                };
                dealerRequisitionInfo.DealerRequisitionDetails = new List<DealerRequisitionDetail>();
                foreach (var item in model.DealerRequisitionDetails)
                {
                    DealerRequisitionDetail dealerRequisitionDetail = new DealerRequisitionDetail()
                    {
                        CategoryId = item.CategoryId,
                        BrandId = item.BrandId,
                        ModelId = item.ModelId,
                        ColorId = item.ColorId,
                        Quantity = item.Quantity,
                        EUserId = userId,
                        EntryDate = DateTime.Now
                    };
                    dealerRequisitionInfo.DealerRequisitionDetails.Add(dealerRequisitionDetail);
                }
                dealerRequisitionInfo.RequisitionCode = "DREQ-" + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss");
                _dealerRequisitionInfoRepository.Insert(dealerRequisitionInfo);
            }
            return _dealerRequisitionInfoRepository.Save();
        }

    }
}
