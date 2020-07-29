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
    public class QCPassTransferDetailBusiness : IQCPassTransferDetailBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly QCPassTransferDetailRepository _qCPassTransferDetailRepository;

        public QCPassTransferDetailBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._qCPassTransferDetailRepository = new QCPassTransferDetailRepository(this._productionDb);
        }

        public IEnumerable<QCPassTransferDetail> GetQCPassTransferDetails(long orgId)
        {
            return _qCPassTransferDetailRepository.GetAll(s => s.OrganizationId == orgId);
        }

        public IEnumerable<QCPassTransferDetailDTO> GetQCPassTransferDetailsByQuery(long? floorId, long? assemblyId, long? qcLineId, string qrCode, string transferCode,string status, string date, long? qcPassId, long? userId, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<QCPassTransferDetailDTO>(QueryForQCPassTransferDetails(floorId, assemblyId, qcLineId, qrCode, transferCode, status, date, qcPassId, userId, orgId)).ToList();
        }

        private string QueryForQCPassTransferDetails(long? floorId, long? assemblyId, long? qcLineId, string qrCode, string transferCode, string status, string date,long? qcPassId, long? userId, long orgId)
        {
            string param = string.Empty;
            string query = string.Empty;

            param += string.Format(@" and qcd.OrganizationId={0}", orgId);
            if (floorId != null && floorId > 0)
            {
                param += string.Format(@" and qci.FloorId={0}", floorId);
            }
            if (assemblyId != null && assemblyId > 0)
            {
                param += string.Format(@" and qci.AssemblyLineId={0}", assemblyId);
            }
            if (qcLineId != null && qcLineId > 0)
            {
                param += string.Format(@" and qci.QCLineId={0}", qcLineId);
            }
            if (qcPassId != null && qcPassId > 0)
            {
                param += string.Format(@" and qci.QPassId={0}", qcPassId);
            }
            if (!string.IsNullOrEmpty(qrCode) && qrCode != "")
            {
                param += string.Format(@" and qcd.QRCode Like '%{0}%'", qrCode);
            }
            if (!string.IsNullOrEmpty(transferCode) && transferCode != "")
            {
                param += string.Format(@" and qci.TransferCode Like '%{0}%'", transferCode);
            }
            if (!string.IsNullOrEmpty(date) && date != "")
            {
                string fDate = Convert.ToDateTime(date).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(qcd.EntryDate as date)='{0}'", fDate);
            }
            if (userId != null && userId > 0)
            {
                param += string.Format(@" and qcd.EUserId ={0}", userId);
            }
            if (!string.IsNullOrEmpty(status) && status.Trim() != "")
            {
                param += string.Format(@" and qci.StateStatus ='{0}'", status);
            }

            query = string.Format(@"Select qcd.QPassDetailId,qci.QCPassCode,qcd.QRCode,qci.ProductionFloorId,pl.LineNumber 'ProductionFloorName',qci.AssemblyLineId,al.AssemblyLineName,qci.QCLineId,qc.QCName 'QCLineName',qci.StateStatus,qci.EntryDate,qci.UpdateDate
From [Production].dbo.tblQCPassTransferDetail qcd
Inner Join [Production].dbo.tblQCPassTransferInformation qci on qcd.QPassId = qci.QPassId
Inner Join [Production].dbo.tblProductionLines pl on qci.ProductionFloorId = pl.LineId
Inner Join [Production].dbo.tblAssemblyLines al on qci.AssemblyLineId = al.AssemblyLineId
Inner Join [Production].dbo.tblQualityControl qc on qci.QCLineId = qc.QCId
Where 1=1 {0}", Utility.ParamChecker(param));

            return query;
        }
    }
}
