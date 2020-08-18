using ERPBLL.Production.Interface;
using ERPBO.Production.DTOModel;
using ERPDAL.ProductionDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production
{
    public class IMEITransferToRepairDetailBusiness : IIMEITransferToRepairDetailBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly IMEITransferToRepairDetailRepository _iMEITransferToRepairDetailRepository;
        public IMEITransferToRepairDetailBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._iMEITransferToRepairDetailRepository = new IMEITransferToRepairDetailRepository(this._productionDb);
        }

        public IEnumerable<IMEITransferToRepairDetailDTO> GetIMEIProblemDTOByQuery(long transferId, string qrCode, string imei, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<IMEITransferToRepairDetailDTO>(IMEIProblemDTOByQuery(transferId, qrCode, imei, orgId)).ToList();
        }
        private string IMEIProblemDTOByQuery(long transferId,string qrCode, string imei, long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;
            param += string.Format(@" and ip.OrganizationId={0}", orgId);
            if (transferId > 0)
            {
                param += string.Format(@" and ip.IMEITRInfoId={0}", transferId);
            }
            if (!string.IsNullOrEmpty(qrCode) && qrCode.Trim() != "")
            {
                param += string.Format(@" and ip.QRCode='{0}'", qrCode.Trim());
            }
            if (!string.IsNullOrEmpty(imei) && imei.Trim() != "")
            {
                param += string.Format(@" and ip.IMEI LIKE'%{0}%'", imei.Trim());
            }
            query = string.Format(@"Select * From [Production].dbo.tblIMEITransferToRepairDetail ip
Where 1=1 {0} order by IMEITRDetailId desc", param);
            return query;
        }
    }
}
