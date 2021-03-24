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
   public class STransferInfoMToMBusiness: ISTransferInfoMToMBusiness
    {
        private readonly IInventoryUnitOfWork _inventoryDb; // database
        private readonly STransferInfoMToMRepository sTransferInfoMToMRepository; // repo
        private readonly IItemBusiness _itemBusiness;
        private readonly IWarehouseStockDetailBusiness _warehouseStockDetailBusiness;
        public STransferInfoMToMBusiness(IInventoryUnitOfWork inventoryDb, IItemBusiness itemBusiness, IWarehouseStockDetailBusiness warehouseStockDetailBusiness)
        {
            this._inventoryDb = inventoryDb;
            sTransferInfoMToMRepository = new STransferInfoMToMRepository(this._inventoryDb);
            this._itemBusiness = itemBusiness;
            this._warehouseStockDetailBusiness = warehouseStockDetailBusiness;
        }

        public IEnumerable<StockTransferInfoMToMDTO> GetAllTransfer(long orgId)
        {
            return _inventoryDb.Db.Database.SqlQuery<StockTransferInfoMToMDTO>(QueryForGetAllTransferData(orgId)).ToList();
        }
        private string QueryForGetAllTransferData(long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;

            param += string.Format(@" and info.OrganizationId={0}", orgId);
            
            query = string.Format(@"Select info.STransferInfoId,TransferCode,ware.WarehouseName'Warehouse',des.DescriptionName'FromModel',
des2.DescriptionName'ToModel',info.EntryDate From tblStockTransferInfoMToM info
Left Join tblDescriptions des on info.FromDescriptionId=des.DescriptionId
Left Join tblDescriptions des2 on info.ToDescriptionId=des2.DescriptionId
Left Join tblWarehouses ware on info.WarehouseId=ware.Id
Where 1=1{0}
Order By info.EntryDate desc", Utility.ParamChecker(param));
            return query;
        }

        public bool SaveStockTransferMToM(StockTransferInfoMToMDTO dto, long userId, long orgId)
        {
            bool IsSuccess = false;

            List<StockTransferDetailsMToM> details = new List<StockTransferDetailsMToM>();
            List<WarehouseStockDetailDTO> warehouseDetailsForStockOut = new List<WarehouseStockDetailDTO>();
            List<WarehouseStockDetailDTO> warehouseDetailsForStockIn = new List<WarehouseStockDetailDTO>();

            StockTransferInfoMToM info = new StockTransferInfoMToM()
            {
                TransferCode = ("STM-" + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss")),
                FromDescriptionId=dto.FromDescriptionId,
                ToDescriptionId=dto.ToDescriptionId,
                WarehouseId=dto.WarehouseId,
                EntryDate=DateTime.Now,
                EUserId=userId,
                OrganizationId=orgId
            };
            foreach (var item in dto.stockTransferDetailsMToM)
            {
                StockTransferDetailsMToM detail = new StockTransferDetailsMToM()
                {
                    
                    FromDescriptionId = dto.FromDescriptionId,
                    ToDescriptionId = dto.ToDescriptionId,
                    WarehouseId=item.WarehouseId,
                    ItemId=item.ItemId,
                    ItemTypeId=item.ItemTypeId,
                    EntryDate = DateTime.Now,
                    EUserId = userId,
                    OrganizationId = orgId,
                    Quantity = item.Quantity,
                    Remarks = item.Remarks,
                };
                details.Add(detail);
                WarehouseStockDetailDTO stockOutItem = new WarehouseStockDetailDTO
                {
                    WarehouseId = item.WarehouseId,
                    ItemTypeId = item.ItemTypeId,
                    ItemId = item.ItemId,
                    Quantity = item.Quantity,
                    OrganizationId = orgId,
                    EUserId = userId,
                    Remarks = item.Remarks,
                    UnitId = _itemBusiness.GetItemById(item.ItemId.Value, orgId).UnitId,
                    StockStatus = StockStatus.StockOut,
                    RefferenceNumber = info.TransferCode,
                    DescriptionId = dto.FromDescriptionId,
                    OrderQty = 0,
                    EntryDate = DateTime.Now,
                };
                warehouseDetailsForStockOut.Add(stockOutItem);

                WarehouseStockDetailDTO stockInItem = new WarehouseStockDetailDTO
                {
                    WarehouseId = item.WarehouseId,
                    ItemTypeId = item.ItemTypeId,
                    ItemId = item.ItemId,
                    Quantity = item.Quantity,
                    OrganizationId = orgId,
                    EUserId = userId,
                    Remarks = item.Remarks,
                    UnitId = _itemBusiness.GetItemById(item.ItemId.Value, orgId).UnitId,
                    StockStatus = StockStatus.StockIn,
                    RefferenceNumber = info.TransferCode,
                    DescriptionId = dto.ToDescriptionId,
                    OrderQty = 0,
                    EntryDate=DateTime.Now,
                };
                warehouseDetailsForStockIn.Add(stockInItem);
            }

            info.stockTransferDetailsMToM = details;
            sTransferInfoMToMRepository.Insert(info);
            if (sTransferInfoMToMRepository.Save())
            {

                if (_warehouseStockDetailBusiness.SaveWarehouseStockOut(warehouseDetailsForStockOut, userId, orgId,string.Empty))
                {
                    IsSuccess = _warehouseStockDetailBusiness.SaveWarehouseStockIn(warehouseDetailsForStockIn, userId, orgId);
                }
            }
            return IsSuccess;
        }

        public StockTransferInfoMToM GetDataOneByOrgId(long id, long orgId)
        {
            return sTransferInfoMToMRepository.GetOneByOrg(i => i.STransferInfoId == id && i.OrganizationId == orgId);
        }
    }
}
