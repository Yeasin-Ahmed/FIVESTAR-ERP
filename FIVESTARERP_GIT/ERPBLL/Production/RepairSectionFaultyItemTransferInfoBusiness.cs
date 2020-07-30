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
    public class RepairSectionFaultyItemTransferInfoBusiness : IRepairSectionFaultyItemTransferInfoBusiness
    {
        private readonly IProductionUnitOfWork _production;
        private readonly RepairSectionFaultyItemTransferInfoRepository _repairSectionFaultyItemTransferInfoRepository;
        private readonly IFaultyItemStockDetailBusiness _faultyItemStockDetailBusiness;
        public RepairSectionFaultyItemTransferInfoBusiness(IProductionUnitOfWork production, IFaultyItemStockDetailBusiness faultyItemStockDetailBusiness)
        {
            this._production = production;
            this._repairSectionFaultyItemTransferInfoRepository = new RepairSectionFaultyItemTransferInfoRepository(this._production);
            this._faultyItemStockDetailBusiness = faultyItemStockDetailBusiness;
        }

        public IEnumerable<RepairSectionFaultyItemTransferInfo> GetRepairSectionFaultyItemTransferInfoByFloor(long floorId, long orgId)
        {
            return _repairSectionFaultyItemTransferInfoRepository.GetAll(f => f.ProductionFloorId == floorId && f.OrganizationId == orgId);
        }

        public IEnumerable<RepairSectionFaultyItemTransferInfo> GetRepairSectionFaultyItemTransferInfoByOrg(long orgId)
        {
            return _repairSectionFaultyItemTransferInfoRepository.GetAll(f => f.OrganizationId == orgId);
        }

        public IEnumerable<RepairSectionFaultyItemTransferInfo> GetRepairSectionFaultyItemTransferInfoByRepairLine(long floorId, long RepairLine, long orgId)
        {
            return _repairSectionFaultyItemTransferInfoRepository.GetAll(f => f.ProductionFloorId == floorId && f.RepairLineId == RepairLine && f.OrganizationId == orgId);
        }

        public IEnumerable<RepairSectionFaultyItemTransferInfoDTO> GetRepairSectionFaultyItemTransferInfoList(long? floorId, long? repairLineId,string transferCode, string status, string fromDate, string toDate, long orgId)
        {
            return this._production.Db.Database.SqlQuery<RepairSectionFaultyItemTransferInfoDTO>(QueryForRepairSectionFaultyItemTransferInfoList(floorId, repairLineId, transferCode, status, fromDate, toDate, orgId)).ToList();
        }

        public RepairSectionFaultyItemTransferInfo GetRepairSectionFaultyTransferInfoById(long transferId, long orgId)
        {
            return _repairSectionFaultyItemTransferInfoRepository.GetOneByOrg(s => s.RSFIRInfoId == transferId && s.OrganizationId == orgId);
        }
        
        public bool SaveRepairSectionFaultyItemTransfer(RepairSectionFaultyItemTransferInfoDTO faultyItems, long orgId, long userId)
        {
            bool IsSuccess = false;
            string code = (DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss"));
            RepairSectionFaultyItemTransferInfo info = new RepairSectionFaultyItemTransferInfo
            {
                ProductionFloorId = faultyItems.ProductionFloorId,
                ProductionFloorName = faultyItems.ProductionFloorName,
                RepairLineId = faultyItems.RepairLineId,
                RepairLineName = faultyItems.RepairLineName,
                StateStatus = RequisitionStatus.Approved,
                OrganizationId = orgId,
                EUserId = userId,
                EntryDate = DateTime.Now,
                TransferCode = "RSFIT-"+ code,
                Remarks = faultyItems.Remarks
            };
            List<RepairSectionFaultyItemTransferDetail> details = new List<RepairSectionFaultyItemTransferDetail>();
            List<FaultyItemStockDetailDTO> faultyItemStocks = new List<FaultyItemStockDetailDTO>();
            foreach (var item in faultyItems.RepairSectionFaultyItemRequisitionDetails)
            {
                RepairSectionFaultyItemTransferDetail detail = new RepairSectionFaultyItemTransferDetail
                {
                    ProductionFloorId = item.ProductionFloorId,
                    ProductionFloorName = item.ProductionFloorName,
                    RepairLineId = item.RepairLineId,
                    RepairLineName = item.RepairLineName,
                    QCLineId = item.QCLineId,
                    QCLineName = item.QCLineName,
                    DescriptionId = item.DescriptionId,
                    ModelName = item.ModelName,
                    WarehouseId = item.WarehouseId,
                    WarehouseName = item.WarehouseName,
                    ItemTypeId = item.ItemTypeId,
                    ItemTypeName = item.ItemTypeName,
                    ItemId = item.ItemId,
                    ItemName = item.ItemName,
                    UnitId = item.UnitId,
                    UnitName = item.UnitName,
                    FaultyQty = item.FaultyQty,
                    OrganizationId = orgId,
                    EUserId = userId,
                    EntryDate = DateTime.Now,
                    ReferenceNumber = code
                };
                details.Add(detail);

                FaultyItemStockDetailDTO faulty = new FaultyItemStockDetailDTO {
                    ProductionFloorId = item.ProductionFloorId,
                    RepairLineId = item.RepairLineId,
                    QCId = item.QCLineId,
                    DescriptionId = item.DescriptionId,
                    WarehouseId = item.WarehouseId,
                    ItemTypeId = item.ItemTypeId,
                    ItemId = item.ItemId,
                    UnitId = item.UnitId,
                    ReferenceNumber = code,
                    Quantity = item.FaultyQty,
                    OrganizationId= orgId,
                    EUserId = userId,
                    EntryDate = DateTime.Now,
                    StockStatus = StockStatus.StockOut
                };
                faultyItemStocks.Add(faulty);
            }

            var totalUnit = details.Select(d => d.FaultyQty).Sum();
            info.TotalUnit = totalUnit;
            info.RepairSectionFaultyItemRequisitionDetails = details;

            _repairSectionFaultyItemTransferInfoRepository.Insert(info);

            if (_repairSectionFaultyItemTransferInfoRepository.Save()) {
                // Faulty Item Stock Out .....
                IsSuccess= this._faultyItemStockDetailBusiness.SaveFaultyItemStockOut(faultyItemStocks,userId,orgId);
            }
            return IsSuccess;
        }

        // Private Methods //

        private string QueryForRepairSectionFaultyItemTransferInfoList(long? floorId, long? repairLineId,string transferCode, string status, string fromDate, string toDate,long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;

            param += string.Format(@" and info.OrganizationId={0}", orgId);
            if (repairLineId != null && repairLineId > 0)
            {
                param += string.Format(@" and info.RepairLineId={0}", repairLineId);
            }
            if (floorId != null && floorId > 0)
            {
                param += string.Format(@" and info.ProductionFloorId={0}", floorId);
            }
            if (!string.IsNullOrEmpty(status) && status.Trim() != "")
            {
                param += string.Format(@" and info.StateStatus='{0}'", status);
            }
            if (!string.IsNullOrEmpty(transferCode) && transferCode.Trim() != "")
            {
                param += string.Format(@" and info.TransferCode Like'%{0}%'", transferCode);
            }
            if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "" && !string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(info.EntryDate as date) between '{0}' and '{1}'", fDate, tDate);
            }
            else if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(info.EntryDate as date)='{0}'", fDate);
            }
            else if (!string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(info.EntryDate as date)='{0}'", tDate);
            }

            query = string.Format(@"Select RSFIRInfoId,TransferCode,ProductionFloorName,RepairLineName,StateStatus,TotalUnit,app.UserName 'EntryUser',info.EntryDate
From tblRepairSectionFaultyItemTransferInfo info
Inner Join [ControlPanel].dbo.tblApplicationUsers app on info.EUserId=app.UserId Where 1=1 {0}", Utility.ParamChecker(param));

            return query;
        }
    }
}
