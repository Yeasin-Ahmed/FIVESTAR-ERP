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
    public class TechnicalServicesStockBusiness : ITechnicalServicesStockBusiness
    {
        private readonly IFrontDeskUnitOfWork _frontDeskUnitOfWork;//db
        private readonly TechnicalServicesStockRepository technicalServicesStockRepository;
        private readonly IRequsitionInfoForJobOrderBusiness _requsitionInfoForJobOrderBusiness;
        private readonly IRequsitionDetailForJobOrderBusiness _requsitionDetailForJobOrderBusiness;
        private readonly ITsStockReturnInfoBusiness _tsStockReturnInfoBusiness;
        private readonly IJobOrderTSBusiness _jobOrderTSBusiness;
        private readonly IJobOrderBusiness _jobOrderBusiness;

        public TechnicalServicesStockBusiness(IFrontDeskUnitOfWork frontDeskUnitOfWork, IRequsitionInfoForJobOrderBusiness requsitionInfoForJobOrderBusiness, IRequsitionDetailForJobOrderBusiness requsitionDetailForJobOrderBusiness, ITsStockReturnInfoBusiness tsStockReturnInfoBusiness, IJobOrderTSBusiness jobOrderTSBusiness, IJobOrderBusiness jobOrderBusiness)
        {
            this._frontDeskUnitOfWork = frontDeskUnitOfWork;
            this.technicalServicesStockRepository = new TechnicalServicesStockRepository(this._frontDeskUnitOfWork);
            this._requsitionInfoForJobOrderBusiness = requsitionInfoForJobOrderBusiness;
            this._requsitionDetailForJobOrderBusiness = requsitionDetailForJobOrderBusiness;
            this._tsStockReturnInfoBusiness = tsStockReturnInfoBusiness;
            this._jobOrderTSBusiness = jobOrderTSBusiness;
            this._jobOrderBusiness = jobOrderBusiness;
        }

        public IEnumerable<TechnicalServicesStock> GetAllTechnicalServicesStock(long id, long orgId, long branchId)
        {
            return technicalServicesStockRepository.GetAll(stock => stock.JobOrderId == id && stock.OrganizationId == orgId && stock.BranchId == branchId).ToList();
        }

        public IEnumerable<TechnicalServicesStock> GetRequsitionStockByStockId(long id, long orgId, long branchId)
        {
            return technicalServicesStockRepository.GetAll(stock => stock.TsStockId == id && stock.OrganizationId == orgId && stock.BranchId == branchId).ToList();
        }

        public IEnumerable<TSStockByRequsitionDTO> GetStockByJobOrder(long? jobOrderId, long tsId, long orgId, long branchId,string roleName)
        {
            return _frontDeskUnitOfWork.Db.Database.SqlQuery<TSStockByRequsitionDTO>(QueryForStock(jobOrderId,tsId,orgId,branchId,roleName)).ToList();
        }

        public IEnumerable<TechnicalServicesStockDTO> GetUsedParts(long? partsId,long? tsId,long orgId, long branchId, string fromDate, string toDate)
        {
            return _frontDeskUnitOfWork.Db.Database.SqlQuery<TechnicalServicesStockDTO>(QueryForUsedParts(partsId,tsId,orgId, branchId, fromDate, toDate)).ToList();
        }
        private string QueryForUsedParts(long? partsId,long? tsId, long orgId, long branchId, string fromDate, string toDate)
        {
            string query = string.Empty;
            string param = string.Empty;
            if (tsId != null && tsId > 0)
            {
                param += string.Format(@"and ts.UpUserId={0}", tsId);
            }
            if (partsId !=null && partsId > 0)
            {
                param += string.Format(@"and ts.PartsId={0}", partsId);
            }
            if (orgId > 0)
            {
                param += string.Format(@"and ts.OrganizationId={0}", orgId);
            }
            if (branchId > 0)
            {
                param += string.Format(@"and ts.BranchId={0}", branchId);
            }
            if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "" && !string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(ts.EntryDate as date) between '{0}' and '{1}'", fDate, tDate);
            }
            else if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(ts.EntryDate as date)='{0}'", fDate);
            }
            else if (!string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(ts.EntryDate as date)='{0}'", tDate);
            }
            query = string.Format(@"select ts.PartsId,rq.RequsitionCode,ps.MobilePartName 'PartsName',ps.MobilePartCode,ts.UsedQty,ts.EntryDate,app.UserName 'UserName',ts.UpUserId
from tblTechnicalServicesStock ts
left join [Configuration].dbo.tblMobileParts ps
on ts.PartsId=ps.MobilePartId
left join tblRequsitionInfoForJobOrders rq
on ts.RequsitionInfoForJobOrderId=rq.RequsitionInfoForJobOrderId
left join [ControlPanel].dbo.tblApplicationUsers app
on ts.UpUserId=app.UserId
where ts.UsedQty>0 and 1=1{0}

", Utility.ParamChecker(param));
            return query;
        }

        public bool SaveJobSignOutWithStock(TSStockInfoDTO dto, long userId, long orgId, long branchId)
        {
            bool IsSuccess = true;
            var jobOrderInDb = _jobOrderBusiness.GetJobOrderById(dto.JobOrderId, orgId);
            if(jobOrderInDb != null)
            {
                //jobOrderInDb.StateStatus = dto.TsRepairStatus == "REPAIR AND RETURN" ? "Repair-Done" : JobOrderStatus.CustomerApproved;

                if(dto.StockDetails.Count > 0) // Stock-Start
                {
                    List<TechnicalServicesStock> servicesStocks = new List<TechnicalServicesStock>();
                    List<TsStockReturnDetailDTO> returnStocks = new List<TsStockReturnDetailDTO>();

                    foreach (var item in dto.StockDetails)
                    {
                        var servicesInfo = GetAllTechnicalServicesStock(dto.JobOrderId, orgId, branchId).Where(o => o.PartsId == item.PartsId && o.JobOrderId == dto.JobOrderId && o.RequsitionInfoForJobOrderId == item.RequsitionInfoForJobOrderId).FirstOrDefault();
                        servicesInfo.UsedQty = item.UsedQty;
                        servicesInfo.ReturnQty = item.Quantity - item.UsedQty;
                        servicesInfo.StateStatus = "Stock-Closed";
                        servicesInfo.UpUserId = userId;
                        servicesInfo.UpdateDate = DateTime.Now;

                        technicalServicesStockRepository.Update(servicesInfo);
                        if (servicesInfo.ReturnQty > 0)
                        {
                            TsStockReturnDetailDTO returnStock = new TsStockReturnDetailDTO()
                            {
                                ReqInfoId = item.RequsitionInfoForJobOrderId,
                                RequsitionCode = item.RequsitionCode,
                                PartsId = item.PartsId,
                                PartsName = item.PartsName,
                                Quantity = servicesInfo.ReturnQty,
                                JobOrderId = dto.JobOrderId,
                                BranchId = branchId,
                                OrganizationId = orgId,
                                EUserId = userId,
                            };
                            returnStocks.Add(returnStock); // 
                        }

                    }
                    if(returnStocks.Count > 0)
                    {
                        var distinctReturnInfo = returnStocks.Select(s => new { RequsitionCode = s.RequsitionCode, JobOrderId = s.JobOrderId, ReqInfoId = s.ReqInfoId }).Distinct().ToList();

                        List<TsStockReturnInfoDTO> returnInfoList = new List<TsStockReturnInfoDTO>();
                        foreach (var info in distinctReturnInfo)
                        {
                            TsStockReturnInfoDTO tsReturnInfo = new TsStockReturnInfoDTO()
                            {
                                RequsitionCode = info.RequsitionCode,
                                JobOrderId = info.JobOrderId,
                                ReqInfoId = info.ReqInfoId,

                            };
                            var detailsList = returnStocks.Where(s => s.RequsitionCode == info.RequsitionCode && s.JobOrderId == info.JobOrderId && s.ReqInfoId == info.ReqInfoId).ToList();
                            tsReturnInfo.TsStockReturnDetails = detailsList;
                            returnInfoList.Add(tsReturnInfo);
                        }

                        technicalServicesStockRepository.InsertAll(servicesStocks);
                        IsSuccess = technicalServicesStockRepository.Save();
                        if (IsSuccess)
                        {
                            // Done - JobOrder - Repair-Done // Not Done -- JobOrder - Customer-Approved
                            IsSuccess = _tsStockReturnInfoBusiness.SaveTsReturnStock(returnInfoList, userId, orgId, branchId);
                        }
                        else
                        {
                            IsSuccess = false;
                        }
                    }
                    else
                    {
                        IsSuccess= technicalServicesStockRepository.Save(); // update used qty
                    }
                    

                }// Stock-End

                if (IsSuccess== true && _jobOrderTSBusiness.UpdateJobOrderTsStatus(dto.JobOrderId, userId, orgId, branchId) == true)
                {
                    if (_jobOrderBusiness.UpdateJobSingOutStatus(dto.JobOrderId, userId, orgId, branchId)) {
                        IsSuccess=_requsitionInfoForJobOrderBusiness.UpdatePendingCurrentRequisitionStatus(jobOrderInDb.JodOrderId, jobOrderInDb.TsRepairStatus, userId, orgId, branchId);
                    }
                }
            }
            return IsSuccess;
        }

        public bool SaveTechnicalServicesStockIn(List<TechnicalServicesStockDTO> servicesStockDTOs, long userId, long orgId, long branchId)
        {
            List<TechnicalServicesStock> servicesStocks = new List<TechnicalServicesStock>();
            foreach (var item in servicesStockDTOs)
            {
                var servicesInfo = GetAllTechnicalServicesStock(item.JobOrderId.Value, orgId, branchId).Where(o => o.SWarehouseId == item.SWarehouseId && o.PartsId == item.PartsId && o.JobOrderId == item.JobOrderId && o.RequsitionInfoForJobOrderId == item.RequsitionInfoForJobOrderId).FirstOrDefault();
                if (servicesInfo != null)
                {
                    servicesInfo.Quantity += item.Quantity;
                    technicalServicesStockRepository.Update(servicesInfo);
                }
                else
                {
                    TechnicalServicesStock stockServices = new TechnicalServicesStock();
                    stockServices.JobOrderId = item.JobOrderId;
                    stockServices.SWarehouseId = item.SWarehouseId;
                    stockServices.RequsitionInfoForJobOrderId = item.RequsitionInfoForJobOrderId;
                    stockServices.PartsId = item.PartsId;
                    stockServices.StateStatus = "Stock-Open";
                    stockServices.CostPrice = item.CostPrice;
                    stockServices.SellPrice = item.SellPrice;
                    stockServices.Quantity = item.Quantity;
                    stockServices.UsedQty = 0;
                    stockServices.ReturnQty = 0;
                    stockServices.OrganizationId = orgId;
                    stockServices.BranchId = branchId;
                    stockServices.EUserId = userId;
                    stockServices.Remarks = item.Remarks;
                    stockServices.EntryDate = DateTime.Now;
                    technicalServicesStockRepository.Insert(stockServices);
                }
            }
            technicalServicesStockRepository.InsertAll(servicesStocks);
            return technicalServicesStockRepository.Save();
        }

        public bool SaveTechnicalServicesStockOut(List<TechnicalServicesStockDTO> servicesStockDTOs, long jobOrderId, long userId, long orgId, long branchId)
        {
            bool IsSuccess = false;
            List<TechnicalServicesStock> servicesStocks = new List<TechnicalServicesStock>();
            List<TsStockReturnDetailDTO> returnStocks = new List<TsStockReturnDetailDTO>();
            foreach (var item in servicesStockDTOs)
            {
                var servicesInfo = GetAllTechnicalServicesStock(item.JobOrderId.Value, orgId, branchId).Where(o => o.PartsId == item.PartsId && o.JobOrderId == item.JobOrderId && o.RequsitionInfoForJobOrderId == item.RequsitionInfoForJobOrderId).FirstOrDefault();
                servicesInfo.UsedQty = item.UsedQty;
                servicesInfo.ReturnQty = item.Quantity - item.UsedQty;
                servicesInfo.StateStatus = "Stock-Closed";

                technicalServicesStockRepository.Update(servicesInfo);
                if (servicesInfo.ReturnQty > 0)
                {
                    TsStockReturnDetailDTO returnStock = new TsStockReturnDetailDTO()
                    {
                        ReqInfoId = item.RequsitionInfoForJobOrderId,
                        RequsitionCode = item.RequsitionCode,
                        PartsId = item.PartsId.Value,
                        PartsName = item.PartsName,
                        Quantity = servicesInfo.ReturnQty,
                        JobOrderId = item.JobOrderId.Value,
                        BranchId = branchId,
                        OrganizationId = orgId,
                        EUserId = userId,
                    };
                    returnStocks.Add(returnStock); // 
                }

            }

            var distinctReturnInfo = returnStocks.Select(s => new { RequsitionCode = s.RequsitionCode, JobOrderId = s.JobOrderId, ReqInfoId = s.ReqInfoId }).Distinct().ToList();

            List<TsStockReturnInfoDTO> returnInfoList = new List<TsStockReturnInfoDTO>();
            foreach (var info in distinctReturnInfo)
            {
                TsStockReturnInfoDTO tsReturnInfo = new TsStockReturnInfoDTO()
                {
                    RequsitionCode = info.RequsitionCode,
                    JobOrderId = info.JobOrderId,
                    ReqInfoId = info.ReqInfoId,

                };
                var detailsList = returnStocks.Where(s => s.RequsitionCode == info.RequsitionCode && s.JobOrderId == info.JobOrderId && s.ReqInfoId == info.ReqInfoId).ToList();
                tsReturnInfo.TsStockReturnDetails = detailsList;
                returnInfoList.Add(tsReturnInfo);
            }

            technicalServicesStockRepository.InsertAll(servicesStocks);
            if (technicalServicesStockRepository.Save() == true)
            {
                // Done - JobOrder - Repair-Done // Not Done -- JobOrder - Customer-Approved
                IsSuccess = _tsStockReturnInfoBusiness.SaveTsReturnStock(returnInfoList, userId, orgId, branchId);
                if (IsSuccess == true)
                {
                    if (_jobOrderTSBusiness.UpdateJobOrderTsStatus(jobOrderId, userId, orgId, branchId) == true)
                    {
                        return _jobOrderBusiness.UpdateJobSingOutStatus(jobOrderId, userId, orgId, branchId);
                    }

                }
            }
            return IsSuccess;
        }
        public bool SaveTechnicalStockInRequistion(long id, string status, long orgId, long userId, long branchId)
        {
            var reqInfo = _requsitionInfoForJobOrderBusiness.GetAllRequsitionInfoForJobOrderId(id, orgId);
            var reqDetail = _requsitionDetailForJobOrderBusiness.GetAllRequsitionDetailForJobOrderId(id, orgId, branchId).ToList();
            if (reqDetail != null && reqInfo.StateStatus == RequisitionStatus.Approved && reqDetail.Count > 0)
            {
                List<TechnicalServicesStockDTO> technicalServicesStockDTOs = new List<TechnicalServicesStockDTO>();
                foreach (var item in reqDetail)
                {
                    TechnicalServicesStockDTO technicalServicesStockDTO = new TechnicalServicesStockDTO()
                    {
                        JobOrderId = reqInfo.JobOrderId,
                        SWarehouseId = reqInfo.SWarehouseId,
                        PartsId = item.PartsId,
                        CostPrice = item.CostPrice,
                        SellPrice = item.SellPrice,
                        Quantity = item.Quantity,
                        UsedQty = 0,
                        ReturnQty = 0,
                        Remarks = item.Remarks,
                        OrganizationId = orgId,
                        EUserId = userId,
                        BranchId = branchId,
                    };
                    technicalServicesStockDTOs.Add(technicalServicesStockDTO);
                }
                if (SaveTechnicalServicesStockIn(technicalServicesStockDTOs, userId, orgId, branchId) == true)
                {
                    return _requsitionInfoForJobOrderBusiness.SaveRequisitionStatus(id, status, userId, orgId, branchId);
                }
            }
            return false;
        }

        private string QueryForStock(long? jobOrderId, long tsId, long orgId, long branchId, string roleName)
        {
            string query = string.Empty;
            string param = string.Empty;
            if (jobOrderId != null && jobOrderId > 0) // Single Job Order Searching
            {
                param += string.Format(@" and jo.JodOrderId ={0}", jobOrderId);
            }
            if (orgId > 0)
            {
                param += string.Format(@" and jo.OrganizationId={0}", orgId);
            }
            if (branchId > 0)
            {
                param += string.Format(@" and jo.BranchId={0}", branchId);
            }
            if (roleName == "Technical Services")
            {
                param += string.Format(@" and rq.EUserId ={0}", tsId);
            }


            query = string.Format(@"Select jo.JodOrderId,rq.RequsitionInfoForJobOrderId,jo.JobOrderCode,rq.RequsitionCode,parts.MobilePartName,rq.EUserId,rq.EntryDate,jo.DescriptionId,
        stock.PartsId,stock.Quantity,stock.StateStatus 
            from [FrontDesk].dbo.tblTechnicalServicesStock stock 
        inner join [FrontDesk].dbo.tblJobOrders jo on stock.JobOrderId=jo.JodOrderId
        inner join [FrontDesk].dbo.tblRequsitionInfoForJobOrders rq on stock.RequsitionInfoForJobOrderId=rq.RequsitionInfoForJobOrderId
        inner join [Configuration].dbo.tblMobileParts parts on stock.PartsId=parts.MobilePartId
        where  rq.StateStatus='Approved'{0} and stock.StateStatus='Stock-Open'{0}", Utility.ParamChecker(param));
            return query;
        }
    }
    
}
