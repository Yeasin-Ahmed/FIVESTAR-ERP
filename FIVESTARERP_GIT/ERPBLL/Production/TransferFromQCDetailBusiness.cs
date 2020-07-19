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
    public class TransferFromQCDetailBusiness : ITransferFromQCDetailBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly TransferFromQCDetailRepository _transferFromQCDetailRepository;

        public TransferFromQCDetailBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._transferFromQCDetailRepository = new TransferFromQCDetailRepository(this._productionDb);
        }

        public IEnumerable<TransferFromQCDetailDTO> GetTransferFromQCDetailDTO(long infoId, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<TransferFromQCDetailDTO>(QueryTransferFromQCDetailDTO(infoId, orgId)).ToList();            
        }

        private string QueryTransferFromQCDetailDTO(long infoId, long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;

            param += string.Format(@" and qd.OrganizationId ={0}",orgId);
            if(infoId> 0)
            {
                param += string.Format(@" and qd.TSQInfoId ={0}", infoId);
            }
            query = string.Format(@"Select qd.TFQDetailId,qd.TSQInfoId,qd.WarehouseId,w.WarehouseName,it.ItemId 'ItemTypeId',it.ItemName 'ItemTypeName',
qd.ItemId,i.ItemName,(qd.Quantity * qi.ForQty) 'Quantity',qd.UnitId,u.UnitSymbol as 'UnitName'
From [Production].dbo.tblTransferFromQCDetail qd
Inner Join [Production].dbo.tblTransferFromQCInfo qi on qd.TSQInfoId= qi.TFQInfoId
Inner Join [Inventory].dbo.tblWarehouses w on qd.WarehouseId = w.Id
Inner Join [Inventory].dbo.tblItemTypes it on qd.ItemTypeId = it.ItemId
Inner Join [Inventory].dbo.tblItems i on qd.ItemId = i.ItemId
Left Join [Inventory].dbo.tblUnits u on qd.UnitId = u.UnitId
where 1= 1 {0}", Utility.ParamChecker(param));

            return query;
        }

        public IEnumerable<TransferFromQCDetail> GetTransferFromQCDetailByInfo(long infoId, long orgId)
        {
            return _transferFromQCDetailRepository.GetAll(t => t.TSQInfoId == infoId && t.OrganizationId == orgId).ToList();
        }

        public async Task<IEnumerable<TransferFromQCDetail>> GetTransferFromQCDetailByInfoAsync(long infoId, long orgId)
        {
            return await _transferFromQCDetailRepository.GetAllAsync(t => t.TSQInfoId == infoId && t.OrganizationId == orgId);
        }

        public IEnumerable<TransferFromQCDetail> GetTransferFromQCDetails(long orgId)
        {
            return _transferFromQCDetailRepository.GetAll(t => t.OrganizationId == orgId).ToList();
        }
    }
}
