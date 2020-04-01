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
    }
}
