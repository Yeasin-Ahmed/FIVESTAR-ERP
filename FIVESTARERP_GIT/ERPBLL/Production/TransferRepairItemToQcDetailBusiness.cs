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
    public class TransferRepairItemToQcDetailBusiness : ITransferRepairItemToQcDetailBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly TransferRepairItemToQcDetailRepository _transferRepairItemToQcRepository;

        public TransferRepairItemToQcDetailBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._transferRepairItemToQcRepository = new TransferRepairItemToQcDetailRepository(this._productionDb);
        }

        public IEnumerable<TransferRepairItemToQcDetail> GetTransferRepairItemToQcDetailByInfo(long infoId, long orgId)
        {
            return _transferRepairItemToQcRepository.GetAll(t => t.TRQInfoId == infoId && t.OrganizationId == orgId).ToList();
        }

        public IEnumerable<TransferRepairItemToQcDetailDTO> GetTransferRepairItemToQcDetailByQuery(long transferId, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<TransferRepairItemToQcDetailDTO>(QueryForTransferRepairItemToQcDetailByQuery(transferId, orgId)).ToList();
        }

        private string QueryForTransferRepairItemToQcDetailByQuery(long transferId, long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;
            param += string.Format(@" and td.OrganizationId ={0}", orgId);
            if (transferId > 0)
            {
                param += string.Format(@" and td.TRQInfoId ={0}", transferId);
            }

            query = string.Format(@"Select td.QRCode,td.IncomingTransferId,td.IncomingTransferCode,app.UserName 'EntryUser',td.EntryDate
From [Production].dbo.tblTransferRepairItemToQcDetail td
Inner Join [ControlPanel].dbo.tblApplicationUsers app on td.EUserId = app.UserId
Where 1=1 {0}", Utility.ParamChecker(param));
            return query;
        }

        public IEnumerable<TransferRepairItemToQcDetail> GetTransferRepairItemToQcDetails(long orgId)
        {
            return _transferRepairItemToQcRepository.GetAll(t => t.OrganizationId == orgId).ToList();
        }
    }
}
