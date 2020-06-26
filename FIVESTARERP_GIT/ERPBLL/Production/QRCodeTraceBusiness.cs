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
    public class QRCodeTraceBusiness : IQRCodeTraceBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly QRCodeTraceRepository _qRCodeTraceRepository;
        public QRCodeTraceBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._qRCodeTraceRepository = new QRCodeTraceRepository(this._productionDb);
        }

        public QRCodeTrace GetQRCodeTraceByCode(string code, long orgId)
        {
            return _qRCodeTraceRepository.GetOneByOrg(q => q.CodeNo == code && q.OrganizationId == orgId);
        }

        public IEnumerable<QRCodeTrace> GetQRCodeTraceByOrg(long orgId)
        {
            return _qRCodeTraceRepository.GetAll(q => q.OrganizationId == orgId);
        }

        public bool SaveQRCodeTrace(List<QRCodeTraceDTO> dtos, long userId, long orgId)
        {
            throw new NotImplementedException();
        }
    }
}
