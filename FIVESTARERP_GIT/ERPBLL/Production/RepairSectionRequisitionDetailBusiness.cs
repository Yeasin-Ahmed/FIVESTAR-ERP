using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPBLL.Production.Interface;
using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using ERPDAL.ProductionDAL;

namespace ERPBLL.Production
{
    public class RepairSectionRequisitionDetailBusiness : IRepairSectionRequisitionDetailBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly RepairSectionRequisitionDetailRepository _repairSectionRequisitionDetailRepository;
        public RepairSectionRequisitionDetailBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._repairSectionRequisitionDetailRepository = new RepairSectionRequisitionDetailRepository(this._productionDb);
        }

        public RepairSectionRequisitionDetail GetRepairSectionRequisitionDetailById(long reqDetailId, long reqInfo, long orgId)
        {
            return this._repairSectionRequisitionDetailRepository.GetOneByOrg(r => r.RSRInfoId == reqInfo && r.RSRDetailId == reqDetailId && r.OrganizationId == orgId);
        }

        public IEnumerable<RepairSectionRequisitionDetail> GetRepairSectionRequisitionDetailByInfoId(long reqId, long orgId)
        {
            return this._repairSectionRequisitionDetailRepository.GetAll(r => r.RSRInfoId == reqId && r.OrganizationId == orgId).ToList();
        }

        public IEnumerable<RepairSectionRequisitionDetailDTO> GetRepairSectionRequisitionDetailPendingByReqId(long reqId, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<RepairSectionRequisitionDetailDTO>(string.Format(@"Select RSRDetailId,rd.ItemTypeId,rd.ItemId,rd.UnitId,ItemTypeName,ItemName,RequestQty,IssueQty,UnitName,(ISNULL(ws.StockInQty,0)-ISNULL(ws.StockOutQty,0)) 'AvailableQty'
From tblRepairSectionRequisitionDetail rd
Inner Join tblRepairSectionRequisitionInfo ri on rd.RSRInfoId= ri.RSRInfoId
Left Join [Inventory].dbo.tblWarehouseStockInfo ws on rd.WarehouseId = ws.WarehouseId and rd.ItemTypeId=ws.ItemTypeId and rd.ItemId = ws.ItemId and ri.DescriptionId = ws.DescriptionId Where ri.RSRInfoId={0} and ri.OrganizationId={1}",reqId,orgId)).ToList();

        }
    }
}
