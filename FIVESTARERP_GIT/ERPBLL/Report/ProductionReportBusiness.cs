using ERPBLL.Report.Interface;
using ERPBO.Production.ReportModels;
using ERPDAL.ProductionDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Report
{
    public class ProductionReportBusiness: IProductionReportBusiness
    {
        private readonly IProductionUnitOfWork _productionDb; // database
        public ProductionReportBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
        }

        public IEnumerable<ProductionRequisitionReport> GetProductionRequisitionReport(long reqInfoId)
        {
            IEnumerable<ProductionRequisitionReport> list = new List<ProductionRequisitionReport>();
            list = this._productionDb.Db.Database.SqlQuery<ProductionRequisitionReport>(string.Format(@"Select ri.ReqInfoCode,ri.RequisitionType,de.DescriptionName 'ModelName',ri.EntryDate,us.FullName 'RequisitionBy',
pl.LineNumber,w.WarehouseName,it.ItemName 'ItemTypeName'
,i.ItemName,rd.Quantity,un.UnitSymbol 'UnitName',rd.Remarks,ri.StateStatus 
From tblRequsitionDetails rd
Inner Join tblRequsitionInfo ri on rd.ReqInfoId = ri.ReqInfoId
Inner Join tblProductionLines pl on ri.LineId = pl.LineId
Inner Join [Inventory].dbo.tblWarehouses w on ri.WarehouseId = w.Id
Inner Join [Inventory].dbo.tblItemTypes it on rd.ItemTypeId = it.ItemId
Inner Join [Inventory].dbo.tblItems i on rd.ItemId = i.ItemId and rd.ItemTypeId = i.ItemTypeId
Inner Join [Inventory].dbo.tblDescriptions de on ri.DescriptionId = de.DescriptionId
Inner Join [Inventory].dbo.tblUnits un on rd.UnitId = Un.UnitId
Inner Join [ControlPanel].dbo.tblApplicationUsers us on ri.EUserId = us.UserId
Where ri.ReqInfoId={0}", reqInfoId)).ToList();
            return list;
        }
    }
}
