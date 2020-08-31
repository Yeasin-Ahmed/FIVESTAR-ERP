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
    public class TransferPackagingRepairItemToQcDetailBusiness : ITransferPackagingRepairItemToQcDetailBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        //Repository
        private readonly TransferPackagingRepairItemToQcDetailRepository _transferPackagingRepairItemToQcDetailRepository;

        public TransferPackagingRepairItemToQcDetailBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._transferPackagingRepairItemToQcDetailRepository = new TransferPackagingRepairItemToQcDetailRepository(this._productionDb);
        }

        public IEnumerable<TransferPackagingRepairItemToQcDetail> GetPackagingRepairItemToQcDetailsByTransferId(long transferId, long orgId)
        {
            return _transferPackagingRepairItemToQcDetailRepository.GetAll(s => s.TPRQInfoId == transferId && s.OrganizationId == orgId);
        }

        public async Task<IEnumerable<TransferPackagingRepairItemToQcDetail>> GetPackagingRepairItemToQcDetailsByTransferIdAsync(long transferId, long orgId)
        {
            return await _transferPackagingRepairItemToQcDetailRepository.GetAllAsync(s => s.TPRQInfoId == transferId && s.OrganizationId == orgId);
        }

        public IEnumerable<TransferPackagingRepairItemToQcDetailDTO> GetTransferPackagingRepairItemToQcDetailByQuery(string qrCode, string imei, long? transferId, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<TransferPackagingRepairItemToQcDetailDTO>(QueryForTransferPackagingRepairItemToQcDetail(qrCode, imei, transferId, orgId)).ToList();
        }

        private string QueryForTransferPackagingRepairItemToQcDetail(string qrCode, string imei, long? transferId, long orgId)
        {
            string param = string.Empty;
            string query = string.Empty;
            if(!string.IsNullOrEmpty(qrCode) && qrCode.Trim() == "")
            {
                param += string.Format(@" and td.QRCode ='{0}'",qrCode);
            }
            if (!string.IsNullOrEmpty(imei) && imei.Trim() == "")
            {
                param += string.Format(@" and td.IMEI LIKE '%{0}%'", imei);
            }
            if (transferId != null && transferId > 0)
            {
                param += string.Format(@" and td.TPRQInfoId ={0}", transferId);
            }

            query = string.Format(@"Select td.*,app.UserName 'EntryUser',ti.StateStatus From [Production].dbo.tblTransferPackagingRepairItemToQcDetail td
Inner Join [Production].dbo.tblTransferPackagingRepairItemToQcInfo ti on td.TPRQInfoId = ti.TPRQInfoId
Inner Join [ControlPanel].dbo.tblApplicationUsers app on td.EUserId = app.UserId
Where 1=1 and td.OrganizationId={0} {1}", orgId,Utility.ParamChecker(param));
            return query;
        }
    }
}
