using ERPBLL.Common;
using ERPBLL.Inventory.Interface;
using ERPBLL.Production.Interface;
using ERPBO.Inventory.DTOModel;
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
    public class RepairSectionRequisitionInfoBusiness : IRepairSectionRequisitionInfoBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly RepairSectionRequisitionInfoRepository _repairSectionRequisitionInfoRepository;
        private readonly IRepairSectionRequisitionDetailBusiness _repairSectionRequisitionDetailBusiness;
        private readonly IWarehouseStockDetailBusiness _warehouseStockDetailBusiness;
        private readonly RepairSectionRequisitionDetailRepository _repairSectionRequisitionDetailRepository;

        public RepairSectionRequisitionInfoBusiness(IProductionUnitOfWork productionDb, IWarehouseStockDetailBusiness warehouseStockDetailBusiness, IRepairSectionRequisitionDetailBusiness repairSectionRequisitionDetailBusiness, RepairSectionRequisitionDetailRepository repairSectionRequisitionDetailRepository)
        {
            this._productionDb = productionDb;
            this._repairSectionRequisitionInfoRepository = new RepairSectionRequisitionInfoRepository(this._productionDb);
            this._warehouseStockDetailBusiness = warehouseStockDetailBusiness;
            this._repairSectionRequisitionDetailBusiness = repairSectionRequisitionDetailBusiness;
            this._repairSectionRequisitionDetailRepository = new RepairSectionRequisitionDetailRepository(this._productionDb);
        }

        public IEnumerable<RepairSectionRequisitionInfoDTO> GetRepairSectionRequisitionInfoList(long? repairLineId, long? modelId, long? warehouseId, string status, string requisitionCode, string fromDate, string toDate, string queryFor, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<RepairSectionRequisitionInfoDTO>(QueryForRepairSectionRequisitionInfo(repairLineId, modelId, warehouseId, status, requisitionCode, fromDate, toDate, queryFor, orgId)).ToList();
        }

        private string QueryForRepairSectionRequisitionInfo(long? repairLineId, long? modelId, long? warehouseId, string status, string requisitionCode, string fromDate, string toDate,string queryFor, long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;

            param += string.Format(@" and req.OrganizationId={0}", orgId);

            if (!string.IsNullOrEmpty(queryFor))
            {
                if (queryFor == "Production" || queryFor =="Repair") {
                    param += string.Format(@" and req.StateStatus IN('Pending','Checked','Rechecked','Rejected','Approved','HandOver','Accepted')");
                }
                else
                {
                    param += string.Format(@" and req.StateStatus IN('Checked','Approved','HandOver','Accepted')");
                }
            }

            
            if (repairLineId != null && repairLineId > 0)
            {
                param += string.Format(@" and req.RepairLineId={0}", repairLineId);
            }
            if (modelId != null && modelId > 0)
            {
                param += string.Format(@" and req.DescriptionId={0}", modelId);
            }
            if (warehouseId != null && warehouseId > 0)
            {
                param += string.Format(@" and req.WarehouseId={0}", warehouseId);
            }
            if (!string.IsNullOrEmpty(status) && status.Trim() != "")
            {
                param += string.Format(@" and req.StateStatus='{0}'", status);
            }
            if (!string.IsNullOrEmpty(requisitionCode) && requisitionCode.Trim() != "")
            {
                param += string.Format(@" and req.RequisitionCode Like'%{0}%'", requisitionCode);
            }
            if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "" && !string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(req.EntryDate as date) between '{0}' and '{1}'", fDate, tDate);
            }
            else if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(req.EntryDate as date)='{0}'", fDate);
            }
            else if (!string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(req.EntryDate as date)='{0}'", tDate);
            }

            query = string.Format(@"Select req.RSRInfoId,req.RequisitionCode,(rl.RepairLineName +' ['+pl.LineNumber+']') As 'RepairLineName',req.ModelName,w.WarehouseName,req.TotalUnitQty,req.StateStatus,appUser.UserName 'EntryUser', req.EntryDate,
(Select UserName From  [ControlPanel].dbo.tblApplicationUsers Where UserId = req.ApprovedBy) 'ApproveUser',
req.ApprovedDate, 
(Select UserName From  [ControlPanel].dbo.tblApplicationUsers Where UserId = req.ReceivedBy) 'RecheckUser',
req.RecheckedDate,
(Select UserName From  [ControlPanel].dbo.tblApplicationUsers Where UserId = req.RejectedBy) 'RejectUser',
req.RejectedDate,
(Select UserName From  [ControlPanel].dbo.tblApplicationUsers Where UserId = req.CanceledBy) 'CancelUser',
req.CanceledDate,
(Select UserName From  [ControlPanel].dbo.tblApplicationUsers Where UserId = req.ReceivedBy) 'ReceiveUser',
req.ReceivedDate,
(Select UserName From  [ControlPanel].dbo.tblApplicationUsers Where UserId = req.UpUserId) 'UpdateUser',
req.UpdateDate,
(Select UserName From  [ControlPanel].dbo.tblApplicationUsers Where UserId = req.CheckedBy) 'CheckUser',
req.CheckedDate,
(Select UserName From  [ControlPanel].dbo.tblApplicationUsers Where UserId = req.HandOverId) 'HandOverUser',
req.HandOverDate
From tblRepairSectionRequisitionInfo req
Inner Join tblRepairLine rl on req.RepairLineId = rl.RepairLineId
Inner Join tblProductionLines pl on req.ProductionFloorId = pl.LineId
Inner Join [Inventory].dbo.tblWarehouses w on req.WarehouseId = w.Id
Inner Join [ControlPanel].dbo.tblApplicationUsers appUser on appUser.OrganizationId={1} and req.EUserId = appUser.UserId
Where 1=1 {0}", Utility.ParamChecker(param),orgId);
            return query;
        }

        public IEnumerable<RepairSectionRequisitionInfo> GetRepairSectionRequisitionInfos(long orgId)
        {
            return _repairSectionRequisitionInfoRepository.GetAll(r => r.OrganizationId == orgId);
        }
        public bool SaveRepairSectionRequisition(RepairSectionRequisitionInfoDTO model, long userId, long orgId)
        {
            string code = (DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss"));

            RepairSectionRequisitionInfo info = new RepairSectionRequisitionInfo
            {
                ProductionFloorId = model.ProductionFloorId,
                ProductionFloorName = model.ProductionFloorName,
                RepairLineId = model.RepairLineId,
                RepairLineName = model.RepairLineName,
                WarehouseId = model.WarehouseId,
                WarehouseName = model.WarehouseName,
                DescriptionId = model.DescriptionId,
                ModelName = model.ModelName,
                StateStatus = RequisitionStatus.Pending,
                EUserId = userId,
                EntryDate = DateTime.Now,
                OrganizationId = orgId,
                RequisitionCode = code
            };
            List<RepairSectionRequisitionDetail> details = new List<RepairSectionRequisitionDetail>();

            foreach (var item in model.RepairSectionRequisitionDetails)
            {
                RepairSectionRequisitionDetail detail = new RepairSectionRequisitionDetail
                {
                    RepairLineId = model.RepairLineId,
                    RepairLineName = model.RepairLineName,
                    ItemTypeId = item.ItemTypeId,
                    ItemTypeName = item.ItemTypeName,
                    ItemId = item.ItemId,
                    ItemName = item.ItemName,
                    UnitId = item.UnitId,
                    UnitName = item.UnitName,
                    EUserId = userId,
                    EntryDate = DateTime.Now,
                    RequestQty = item.RequestQty,
                    Remarks = item.Remarks,
                    WarehouseId = model.WarehouseId,
                    WarehouseName = model.WarehouseName,
                    RequisitionCode = code,
                    OrganizationId = orgId
                };
                details.Add(detail);
            }
            info.TotalUnitQty = details.Select(s => s.RequestQty).Sum();
            info.RepairSectionRequisitionDetails = details;

            _repairSectionRequisitionInfoRepository.Insert(info);
            return _repairSectionRequisitionInfoRepository.Save();
        }
        public RepairSectionRequisitionInfo GetRepairSectionRequisitionById(long reqId, long orgId)
        {
            return _repairSectionRequisitionInfoRepository.GetOneByOrg(f => f.RSRInfoId == reqId && f.OrganizationId == orgId);
        }
        public RepairSectionRequisitionInfoDTO GetRepairSectionRequisitionDataById(long reqId, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<RepairSectionRequisitionInfoDTO>(string.Format(@"Select ri.RSRInfoId,ri.RequisitionCode,ri.RepairLineName+' ['+ri.ProductionFloorName+']' 'RepairLineName'
,ri.StateStatus,ri.WarehouseName,ri.ModelName,app.UserName 'EntryUser',ri.EntryDate
From tblRepairSectionRequisitionInfo ri
Inner Join [ControlPanel].dbo.[tblApplicationUsers] app on ri.EUserId = app.UserId
Where 1=1 and ri.OrganizationId={0} and ri.RSRInfoId={1}", orgId, reqId)).Single();

        }

        public bool SaveRepairSectionRequisitionIssueByWarehouse(RepairRequisitionInfoStateDTO model, long orgId, long userId)
        {
            if (model.Status == RequisitionStatus.Approved)
            {
                if (SaveRepairSectionRequisitionStatus(model.RequistionId, model.Status, orgId, userId))
                {
                    var reqInfo = GetRepairSectionRequisitionById(model.RequistionId, orgId);

                    List<WarehouseStockDetailDTO> warehouseStocks = new List<WarehouseStockDetailDTO>();
                    foreach (var item in model.Details)
                    {
                        WarehouseStockDetailDTO warehouse = new WarehouseStockDetailDTO
                        {
                            WarehouseId = reqInfo.WarehouseId,
                            DescriptionId = reqInfo.DescriptionId,
                            ItemTypeId = item.ItemTypeId,
                            ItemId = item.ItemId,
                            Quantity = item.IssueQty,
                            OrganizationId = orgId,
                            UnitId = item.UnitId,
                            StockStatus = StockStatus.StockOut,
                            EUserId = userId,
                            EntryDate = DateTime.Now,
                            RefferenceNumber = reqInfo.RequisitionCode,
                            Remarks = "Repair Section Requisition has been issued"
                        };
                        warehouseStocks.Add(warehouse);

                        // Requisition Detail Issue Qty Update
                        var reqDetail = _repairSectionRequisitionDetailBusiness.GetRepairSectionRequisitionDetailById(item.RSRDetailId, reqInfo.RSRInfoId, orgId);
                        if (reqDetail != null)
                        {
                            reqDetail.IssueQty = item.IssueQty;
                            reqDetail.UpdateDate = DateTime.Now;
                            reqDetail.UpUserId = userId;
                            _repairSectionRequisitionDetailRepository.Update(reqDetail);
                        }
                    }
                    if (_repairSectionRequisitionDetailRepository.Save())
                    {
                        return _warehouseStockDetailBusiness.SaveWarehouseStockOut(warehouseStocks, userId, orgId, string.Empty);
                    }
                }
            }
            return false;
        }

        public bool SaveRepairSectionRequisitionStatus(long requisitionId, string status, long orgId, long userId)
        {
            var reqInfo = GetRepairSectionRequisitionById(requisitionId, orgId);
            if (reqInfo.StateStatus == RequisitionStatus.Pending && status == RequisitionStatus.Checked )
            {
                reqInfo.CheckedDate = DateTime.Now;
                reqInfo.CheckedBy = userId;
                reqInfo.StateStatus = status;
                _repairSectionRequisitionInfoRepository.Update(reqInfo);
            }
            if (reqInfo.StateStatus == RequisitionStatus.Checked && (status == RequisitionStatus.Approved || status == RequisitionStatus.Rejected))
            {
                if (status == RequisitionStatus.Approved)
                {
                    reqInfo.ApprovedDate = DateTime.Now;
                    reqInfo.ApprovedBy = userId;
                    reqInfo.StateStatus = status;
                    _repairSectionRequisitionInfoRepository.Update(reqInfo);
                }
                else if (status == RequisitionStatus.Rejected)
                {
                    reqInfo.RejectedDate = DateTime.Now;
                    reqInfo.RejectedBy = userId;
                    reqInfo.StateStatus = status;
                    _repairSectionRequisitionInfoRepository.Update(reqInfo);
                }
            }
            else if (reqInfo.StateStatus == RequisitionStatus.Approved && status == RequisitionStatus.HandOver)
            {
                reqInfo.HandOverDate = DateTime.Now;
                reqInfo.HandOverId = userId;
                reqInfo.StateStatus = status;
                _repairSectionRequisitionInfoRepository.Update(reqInfo);
            }
            else if (reqInfo.StateStatus == RequisitionStatus.HandOver && status == RequisitionStatus.Accepted)
            {
                reqInfo.ReceivedDate = DateTime.Now;
                reqInfo.ReceivedBy = userId;
                reqInfo.StateStatus = status;
                _repairSectionRequisitionInfoRepository.Update(reqInfo);
            }
            else if (reqInfo.StateStatus == RequisitionStatus.Rechecked && status == RequisitionStatus.Pending)
            {
                reqInfo.UpdateDate = DateTime.Now;
                reqInfo.UpUserId = userId;
                reqInfo.StateStatus = status;
                _repairSectionRequisitionInfoRepository.Update(reqInfo);
            }
            return _repairSectionRequisitionInfoRepository.Save();
        }
    }
}
