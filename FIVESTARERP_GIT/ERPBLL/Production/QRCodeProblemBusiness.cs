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
    public class QRCodeProblemBusiness : IQRCodeProblemBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly QRCodeProblemRepository _qRCodeProblemRepository;
        public QRCodeProblemBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._qRCodeProblemRepository = new QRCodeProblemRepository(this._productionDb);
        }
        public IEnumerable<QRCodeProblemDTO> GetQRCodeProblemDTOByQuery(long transferId, string qrCode, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<QRCodeProblemDTO>(QRCodeProblemDTOByQuery(transferId, qrCode, orgId)).ToList();
        }

        private string QRCodeProblemDTOByQuery(long transferId, string qrCode, long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;
            param += string.Format(@" and qp.OrganizationId={0}",orgId);
            if(transferId > 0)
            {
                param += string.Format(@" and qp.TransferId={0}", transferId);
            }
            if(!string.IsNullOrEmpty(qrCode) && qrCode.Trim() != "")
            {
                param += string.Format(@" and qp.QRCode='{0}'", qrCode.Trim());
            }
            query = string.Format(@"Select * From [Production].dbo.tblQRCodeProblem qp
Where 1=1 {0} order by QRProbId desc", param);
            return query;
        }
    }
}
