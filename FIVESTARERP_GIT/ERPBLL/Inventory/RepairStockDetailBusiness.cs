using ERPBLL.Common;
using ERPBLL.Inventory.Interface;
using ERPBO.Inventory.DomainModels;
using ERPBO.Inventory.DTOModel;
using ERPDAL.InventoryDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Inventory
{
    public class RepairStockDetailBusiness : IRepairStockDetailBusiness
    {
        /// <summary>
        ///  BC Stands for          - Business Class
        ///  db Stands for          - Database
        ///  repo Stands for       - Repository
        /// </summary>
        /// 
        private readonly IInventoryUnitOfWork _inventoryDb; // db
        private readonly IItemBusiness _itemBusiness;
        private readonly IRepairStockInfoBusiness _repairStockInfoBusiness;
        private readonly RepairStockInfoRepository repairStockInfoRepository;
        private readonly RepairStockDetailRepository repairStockDetailRepository;
        public RepairStockDetailBusiness(IInventoryUnitOfWork inventoryDb, IItemBusiness itemBusiness, IRepairStockInfoBusiness repairStockInfoBusiness)
        {
            this._inventoryDb = inventoryDb;
            this._itemBusiness = itemBusiness;
            this._repairStockInfoBusiness = repairStockInfoBusiness;
            this.repairStockInfoRepository = new RepairStockInfoRepository(this._inventoryDb);
            this.repairStockDetailRepository = new RepairStockDetailRepository(this._inventoryDb);
        }

        public RepairStockDetail GetRepairStockDetailById(long orgId, long stockDetailId)
        {
            return repairStockDetailRepository.GetAll(r => r.OrganizationId == orgId && r.RStockDetailId == stockDetailId).FirstOrDefault();
        }

        public IEnumerable<RepairStockDetail> GetRepairStockDetails(long orgId)
        {
            return repairStockDetailRepository.GetAll(r => r.OrganizationId == orgId).ToList();
        }

        public bool SaveRepairStockIn(List<RepairStockDetailDTO> repairStockDetailDTOs, long orgId, long userId)
        {
            List<RepairStockDetail> repairStockDetails = new List<RepairStockDetail>();
            foreach (var item in repairStockDetailDTOs)
            {
                RepairStockDetail stockDetail = new RepairStockDetail();
                stockDetail.WarehouseId = item.WarehouseId;
                stockDetail.ItemTypeId = item.ItemTypeId;
                stockDetail.ItemId = item.ItemId;
                stockDetail.Quantity = item.Quantity;
                stockDetail.OrganizationId = orgId;
                stockDetail.EUserId = userId;
                stockDetail.Remarks = item.Remarks;
                stockDetail.UnitId = _itemBusiness.GetItemById(item.ItemId.Value, orgId).UnitId;
                stockDetail.EntryDate = DateTime.Now;
                stockDetail.StockStatus = StockStatus.StockIn;
                stockDetail.RefferenceNumber = item.RefferenceNumber;
                stockDetail.DescriptionId = item.DescriptionId;
                stockDetail.LineId = item.LineId;
                stockDetail.ReturnType = item.ReturnType;
                stockDetail.FaultyCase= item.FaultyCase;

                var repairStockInfo = _repairStockInfoBusiness.GetRepairStockInfos(orgId).Where(o => o.ItemTypeId == item.ItemTypeId && o.ItemId == item.ItemId && o.LineId == item.LineId && o.DescriptionId == item.DescriptionId).FirstOrDefault();
                if (repairStockInfo != null)
                {
                    repairStockInfo.StockInQty += item.Quantity;
                    repairStockInfoRepository.Update(repairStockInfo);
                }
                else
                {
                    RepairStockInfo repairStockInfoNew = new RepairStockInfo();
                    repairStockInfoNew.WarehouseId = item.WarehouseId;
                    repairStockInfoNew.ItemTypeId = item.ItemTypeId;
                    repairStockInfoNew.ItemId = item.ItemId;
                    repairStockInfoNew.UnitId = stockDetail.UnitId;
                    repairStockInfoNew.StockInQty = item.Quantity;
                    repairStockInfoNew.StockOutQty = 0;
                    repairStockInfoNew.OrganizationId = orgId;
                    repairStockInfoNew.EUserId = userId;
                    repairStockInfoNew.EntryDate = DateTime.Now;
                    repairStockInfoNew.LineId = item.LineId;
                    repairStockInfoNew.DescriptionId = item.DescriptionId;
                    repairStockInfoRepository.Insert(repairStockInfoNew);
                }
                repairStockDetails.Add(stockDetail);
            }
            repairStockDetailRepository.InsertAll(repairStockDetails);
            return repairStockDetailRepository.Save();
        }

        public IEnumerable<RepairStockDetailListDTO> GetRepairStockDetailList(long? lineId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string stockStatus, string fromDate, string toDate, string refNum,long orgId, string returnType, string faultyCase)
        {
            IEnumerable<RepairStockDetailListDTO> repairStockDetailListDTOs = new List<RepairStockDetailListDTO>();
            repairStockDetailListDTOs = this._inventoryDb.Db.Database.SqlQuery<RepairStockDetailListDTO>(QueryForRepairStockDetailList(lineId, modelId, warehouseId, itemTypeId, itemId, stockStatus, fromDate, toDate, refNum,orgId, returnType, faultyCase)).ToList();
            return repairStockDetailListDTOs;
        }

        private string QueryForRepairStockDetailList(long? lineId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string stockStatus, string fromDate, string toDate, string refNum, long orgId, string returnType, string faultyCase)
        {
            string query = string.Empty;
            string param = string.Empty;
            param += string.Format(@" and rsd.OrganizationId={0}", orgId);
            if (lineId != null && lineId > 0)
            {
                param += string.Format(@" and pl.LineId={0}", lineId);
            }
            if (modelId != null && modelId > 0)
            {
                param += string.Format(@" and de.DescriptionId={0}", modelId);
            }
            if (warehouseId != null && warehouseId > 0)
            {
                param += string.Format(@" and wh.Id={0}", warehouseId);
            }
            if (itemTypeId != null && itemTypeId > 0)
            {
                param += string.Format(@" and it.ItemId={0}", itemTypeId);
            }
            if (itemId != null && itemId > 0)
            {
                param += string.Format(@" and i.ItemId ={0}", itemId);
            }
            if (!string.IsNullOrEmpty(stockStatus) && stockStatus.Trim() != "")
            {
                param += string.Format(@" and rsd.StockStatus='{0}'", stockStatus);
            }
            if (!string.IsNullOrEmpty(refNum) && refNum.Trim() != "")
            {
                param += string.Format(@" and rsd.RefferenceNumber Like'%{0}%'", refNum);
            }

            if (!string.IsNullOrEmpty(returnType) && returnType.Trim() != "")
            {
                param += string.Format(@" and rsd.ReturnType ='{0}'", returnType);
            }
            if (!string.IsNullOrEmpty(faultyCase) && faultyCase.Trim() != "")
            {
                param += string.Format(@" and rsd.FaultyCase ='{0}'", faultyCase);
            }

            if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "" && !string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(rsd.EntryDate as date) between '{0}' and '{1}'", fDate, tDate);
            }
            else if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(rsd.EntryDate as date)='{0}'", fDate);
            }
            else if (!string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(rsd.EntryDate as date)='{0}'", tDate);
            }

            query = string.Format(@"Select rsd.RStockDetailId,pl.LineNumber,de.DescriptionName 'ModelName',wh.WarehouseName,it.ItemName 'ItemTypeName',i.ItemName,u.UnitSymbol 'UnitName',rsd.Quantity,rsd.StockStatus
,Convert(nvarchar(20),rsd.EntryDate,106) 'EntryDate', ISNULL(rsd.RefferenceNumber,'N/A') as 'RefferenceNumber',au.UserName 'EntryUser',rsd.ReturnType,rsd.FaultyCase  From tblRepairStockDetails rsd
Left Join tblWarehouses wh on rsd.WarehouseId = wh.Id
Left Join tblItemTypes it on rsd.ItemTypeId = it.ItemId
Left Join tblItems i on rsd.ItemId  = i.ItemId
Left Join tblUnits u on rsd.UnitId= u.UnitId
Left Join tblDescriptions de on rsd.DescriptionId = de.DescriptionId
Left Join [Production].dbo.[tblProductionLines] pl on rsd.LineId = pl.LineId
Left Join [ControlPanel].dbo.tblApplicationUsers au on rsd.EUserId = au.UserId
Where 1=1 {0}", Utility.ParamChecker(param));
            return query;
        }
    }
}
