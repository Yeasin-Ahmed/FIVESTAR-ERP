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
    public class FinishGoodsSendToWarehouseDetailBusiness : IFinishGoodsSendToWarehouseDetailBusiness
    {
        private readonly IProductionUnitOfWork _productionDb; // database
        public readonly FinishGoodsSendToWarehouseDetailRepository _finishGoodsSendToWarehouseDetailRepository;

        public FinishGoodsSendToWarehouseDetailBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._finishGoodsSendToWarehouseDetailRepository = new FinishGoodsSendToWarehouseDetailRepository(this._productionDb);
        }
        public IEnumerable<FinishGoodsSendToWarehouseDetail> GetFinishGoodsDetailByInfoId(long infoId, long orgId)
        {
            return _finishGoodsSendToWarehouseDetailRepository.GetAll(f => f.OrganizationId == orgId && f.SendId == infoId).ToList();
        }

        public IEnumerable<FinishGoodsSendDetailListDTO> GetGoodsSendDetailList(long? lineId, long? warehouseId, long? modelId, long? itemTypeId, long? itemId,string status, string refNum, long orgId, string fromDate, string toDate)
        {
            IEnumerable<FinishGoodsSendDetailListDTO> finishGoodsSendDetailListDTO = new List<FinishGoodsSendDetailListDTO>();
            finishGoodsSendDetailListDTO = this._productionDb.Db.Database.SqlQuery<FinishGoodsSendDetailListDTO>(QueryForFinishGoodsSendDetailList(lineId, modelId, warehouseId, itemTypeId, itemId, status, fromDate, toDate, refNum, orgId)).ToList();
            return finishGoodsSendDetailListDTO;
        }

        private string QueryForFinishGoodsSendDetailList(long? lineId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string status, string fromDate, string toDate, string refNum,long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;

            param += string.Format(@" and d.OrganizationId={0}",orgId);
            if (lineId != null && lineId > 0)
            {
                param += string.Format(@" and l.LineId={0}", lineId);
            }
            if (modelId != null && modelId > 0)
            {
                param += string.Format(@" and de.DescriptionId={0}", modelId);
            }
            if (warehouseId != null && warehouseId > 0)
            {
                param += string.Format(@" and w.Id={0}", warehouseId);
            }
            if (itemTypeId != null && itemTypeId > 0)
            {
                param += string.Format(@" and it.ItemId={0}", itemTypeId);
            }
            if (itemId != null && itemId > 0)
            {
                param += string.Format(@" and ii.ItemId ={0}", itemId);
            }
            if (!string.IsNullOrEmpty(status) && status.Trim() != "")
            {
                param += string.Format(@" and i.StateStatus='{0}'", status);
            }
            if (!string.IsNullOrEmpty(refNum) && refNum.Trim() != "")
            {
                param += string.Format(@" and i.RefferenceNumber Like'%{0}%'", refNum);
            }
            if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "" && !string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(d.EntryDate as date) between '{0}' and '{1}'", fDate, tDate);
            }
            else if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(d.EntryDate as date)='{0}'", fDate);
            }
            else if (!string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(d.EntryDate as date)='{0}'", tDate);
            }

            query = string.Format(@"Select d.SendDetailId,l.LineNumber,w.WarehouseName,de.DescriptionName 'ModelName', 
it.ItemName 'ItemTypeName', ii.ItemName,d.Quantity,u.UnitSymbol,i.RefferenceNumber,d.Remarks,
d.Flag,CONVERT(nvarchar(50),i.EntryDate,100) 'EntryDate',i.StateStatus
From tblFinishGoodsSendToWarehouseDetail d
Inner Join tblFinishGoodsSendToWarehouseInfo i on i.SendId = d.SendId
Inner Join tblProductionLines l on i.LineId = l.LineId
Inner Join [Inventory].dbo.[tblWarehouses] w on i.WarehouseId = w.Id
Inner Join tblDescriptions de on i.DescriptionId = de.DescriptionId
Inner Join [Inventory].dbo.[tblItemTypes] it on d.ItemTypeId = it.ItemId
Inner Join [Inventory].dbo.[tblItems] ii on d.ItemId = ii.ItemId
Inner Join [Inventory].dbo.[tblUnits] u on d.UnitId = u.UnitId
Where 1=1 {0}", Utility.ParamChecker(param));

            return query;
        }
    }
}
