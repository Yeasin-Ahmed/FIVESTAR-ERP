using ERPBLL.Common;
using ERPBLL.Inventory.Interface;
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
    public class ItemReturnDetailBusiness : IItemReturnDetailBusiness
    {
        /// <summary>
        ///  BC Stands for          - Business Class
        ///  db Stands for          - Database
        ///  repo Stands for       - Repository
        /// </summary>
        private readonly IProductionUnitOfWork _productionDb; // database
        private readonly IProductionStockDetailBusiness _productionStockDetailBusiness; // BC
        private readonly IItemBusiness _itemBusiness; // BC
        private readonly IWarehouseBusiness _warehouseBusiness; //BC
        private readonly IProductionLineBusiness _productionLineBusiness; //BC
        private readonly ItemReturnInfoRepository _itemReturnInfoRepository; //repo
        private readonly ItemReturnDetailRepository _itemReturnDetailRepository; //repo

        public ItemReturnDetailBusiness(IProductionUnitOfWork productionDb, IProductionStockDetailBusiness productionStockDetailBusiness, IItemBusiness itemBusiness, IWarehouseBusiness warehouseBusiness, IProductionLineBusiness productionLineBusiness)
        {
            this._productionDb = productionDb;
            this._productionStockDetailBusiness = productionStockDetailBusiness;
            this._itemBusiness = itemBusiness;
            this._warehouseBusiness = warehouseBusiness;
            this._productionLineBusiness = productionLineBusiness;
            this._itemReturnInfoRepository = new ItemReturnInfoRepository(this._productionDb);
            this._itemReturnDetailRepository = new ItemReturnDetailRepository(this._productionDb);
        }

        public IEnumerable<ItemReturnDetail> GetItemReturnDetails(long OrgId)
        {
            return _itemReturnDetailRepository.GetAll(i => i.OrganizationId == OrgId).ToList();
        }

        public IEnumerable<ItemReturnDetail> GetItemReturnDetailsByReturnInfoId(long OrgId, long returnInfoId)
        {
            return _itemReturnDetailRepository.GetAll(i => i.OrganizationId == OrgId && i.IRInfoId == returnInfoId).ToList();
        }

        public IEnumerable<ItemReturnDetailListDTO> GetItemReturnDetailList(string refNum, string returnType, string faultyCase, long? lineId, long? warehouseId, string status, long? itemTypeId, long? itemId, string fromDate, string toDate, long? modelId,long orgId)
        {
            IEnumerable<ItemReturnDetailListDTO> list = new List<ItemReturnDetailListDTO>();
            list = _productionDb.Db.Database.SqlQuery<ItemReturnDetailListDTO>(QueryForItemReturnDetailList(refNum, returnType, faultyCase, lineId, warehouseId, status, itemTypeId, itemId, fromDate, toDate, modelId,orgId)).ToList();
            return list;
        }

        private string QueryForItemReturnDetailList(string refNum, string returnType, string faultyCase, long? lineId, long? warehouseId, string status, long? itemTypeId, long? itemId, string fromDate, string toDate, long? modelId, long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;
            param += string.Format(@" and ird.OrganizationId={0}", orgId);
            if (!string.IsNullOrEmpty(refNum) && refNum.Trim() != "")
            {
                param += string.Format(@" and iri.IRCode Like '%{0}%'", refNum.Trim());
            }
            if (!string.IsNullOrEmpty(returnType) && returnType.Trim() != "")
            {
                param += string.Format(@" and iri.ReturnType ='{0}'", returnType.Trim());
            }
            if (!string.IsNullOrEmpty(faultyCase) && faultyCase.Trim() != "")
            {
                param += string.Format(@" and iri.FaultyCase ='{0}'", faultyCase.Trim());
            }
            if (!string.IsNullOrEmpty(status) && status.Trim() != "")
            {
                param += string.Format(@" and iri.StateStatus ='{0}'", status.Trim());
            }
            if (lineId != null && lineId > 0)
            {
                param += string.Format(@" and iri.LineId ={0}", lineId);
            }
            if (warehouseId != null && warehouseId > 0)
            {
                param += string.Format(@" and iri.WarehouseId ={0}", warehouseId);
            }
            if (modelId != null && modelId > 0)
            {
                param += string.Format(@" and iri.DescriptionId ={0}", modelId);
            }
            if (itemTypeId != null && itemTypeId > 0)
            {
                param += string.Format(@" and ird.ItemTypeId={0}", itemTypeId);
            }
            if (itemId != null && itemId > 0)
            {
                param += string.Format(@" and ird.ItemId ={0}", itemId);
            }
            if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "" && !string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(iri.EntryDate as date) between '{0}' and '{1}'", fDate,tDate);
            }
            else if(!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(iri.EntryDate as date)='{0}'", fDate);
            }
            else if (!string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(iri.EntryDate as date)='{0}'", tDate);
            }

            query = string.Format(@"Select ird.IRDetailId,iri.IRCode,iri.ReturnType,ISNULL(iri.FaultyCase,'') 'FaultyCase',de.DescriptionName 'ModelName',pl.LineNumber,w.WarehouseName,itype.ItemName 'ItemTypeName',item.ItemName,ird.Quantity,un.UnitSymbol,iri.StateStatus
,ISNULL(ird.Remarks,'') 'Remarks',Convert(nvarchar(30),iri.EntryDate,100) 'EntryDate',au.UserName 'EntryUser' From tblItemReturnDetail ird
Inner Join tblItemReturnInfo iri on ird.IRInfoId = iri.IRInfoId
Inner Join tblProductionLines Pl on iri.LineId= pl.LineId
Inner Join [Inventory].dbo.tblDescriptions de on iri.DescriptionId= de.DescriptionId
Inner Join [Inventory].dbo.[tblWarehouses] w on iri.WarehouseId = w.Id
Inner Join [Inventory].dbo.[tblItemTypes] itype on ird.ItemTypeId = itype.ItemId
Inner Join [Inventory].dbo.[tblItems] item on ird.ItemId = item.ItemId
Inner Join [Inventory].dbo.[tblUnits] un on item.UnitId = un.UnitId
Left Join [ControlPanel].dbo.tblApplicationUsers au on ird.EUserId = au.UserId
Where 1=1 {0}", Utility.ParamChecker(param));
            return query;
        }
    }
}
