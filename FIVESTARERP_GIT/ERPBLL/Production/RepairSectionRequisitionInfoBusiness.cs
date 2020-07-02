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
    public class RepairSectionRequisitionInfoBusiness : IRepairSectionRequisitionInfoBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly RepairSectionRequisitionInfoRepository _repairSectionRequisitionInfoRepository;
       
        public RepairSectionRequisitionInfoBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._repairSectionRequisitionInfoRepository = new RepairSectionRequisitionInfoRepository(this._productionDb);
        }

        public IEnumerable<RepairSectionRequisitionInfoDTO> GetRepairSectionRequisitionInfoList(long? repairLineId, long? modelId, long? warehouseId, string status, string requisitionCode, string fromDate, string toDate, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<RepairSectionRequisitionInfoDTO>(QueryForRepairSectionRequisitionInfo(repairLineId, modelId, warehouseId, status, requisitionCode, fromDate, toDate, orgId)).ToList();
        }

        private string QueryForRepairSectionRequisitionInfo(long? repairLineId, long? modelId, long? warehouseId, string status, string requisitionCode, string fromDate, string toDate, long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;
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
            if (!string.IsNullOrEmpty(status) && status.Trim() !="")
            {
                param += string.Format(@" and req.StateStatus='{0}'", status);
            }
            if (!string.IsNullOrEmpty(requisitionCode) && requisitionCode.Trim() != "")
            {
                param += string.Format(@" and wsd.RequisitionCode Like'%{0}%'", requisitionCode);
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
req.UpdateDate
From tblRepairSectionRequisitionInfo req
Inner Join tblRepairLine rl on req.RepairLineId = rl.RepairLineId
Inner Join tblProductionLines pl on req.ProductionFloorId = pl.LineId
Inner Join [Inventory].dbo.tblWarehouses w on req.WarehouseId = w.Id
Inner Join [ControlPanel].dbo.tblApplicationUsers appUser on req.EUserId = appUser.UserId
Where 1=1 {0}",Utility.ParamChecker(param));
            return query;
        }

        public IEnumerable<RepairSectionRequisitionInfo> GetRepairSectionRequisitionInfos(long orgId)
        {
            return _repairSectionRequisitionInfoRepository.GetAll(r=> r.OrganizationId == orgId);
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
                RequisitionCode= code
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
                    EUserId= userId,
                    EntryDate = DateTime.Now,
                    RequestQty= item.RequestQty,
                    Remarks= item.Remarks,
                    WarehouseId=model.WarehouseId,
                    WarehouseName = model.WarehouseName,
                    RequisitionCode = code
                };
                details.Add(detail);
            }
            info.TotalUnitQty = details.Select(s => s.RequestQty).Sum();
            info.RepairSectionRequisitionDetails = details;

            _repairSectionRequisitionInfoRepository.Insert(info);
            return _repairSectionRequisitionInfoRepository.Save();
        }
    }
}
