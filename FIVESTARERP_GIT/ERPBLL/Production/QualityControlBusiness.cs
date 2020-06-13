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
    public class QualityControlBusiness : IQualityControlBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly QualityControlRepository _qualityControlRepository;
        public QualityControlBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._qualityControlRepository = new QualityControlRepository(this._productionDb);
        }
        public QualityControl GetQualityControlById(long id, long orgId)
        {
            return _qualityControlRepository.GetOneByOrg(q => q.QCId == id && q.OrganizationId == orgId);
        }
        public IEnumerable<QualityControl> GetQualityControls(long orgId)
        {
            return _qualityControlRepository.GetAll(q => q.OrganizationId == orgId).ToList();
        }
        public bool SaveQualityControl(QualityControlDTO dto, long userId, long orgId)
        {
            if(dto.QCId == 0)
            {
                QualityControl qualityControl = new QualityControl
                {
                    QCName = dto.QCName,
                    IsActive = dto.IsActive,
                    Remarks = dto.Remarks,
                    EntryDate = DateTime.Now,
                    EUserId = userId,
                    OrganizationId = orgId,
                    ProductionLineId = dto.ProductionLineId
                };
                _qualityControlRepository.Insert(qualityControl);
            }
            else
            {
                var QcInDb = this.GetQualityControlById(dto.QCId, orgId);
                if (QcInDb != null)
                {
                    QcInDb.QCName = dto.QCName;
                    QcInDb.IsActive = dto.IsActive;
                    QcInDb.Remarks = dto.Remarks;
                    QcInDb.UpUserId = userId;
                    QcInDb.UpdateDate = DateTime.Now;
                    _qualityControlRepository.Update(QcInDb);
                }
            }
            return _qualityControlRepository.Save();
        }
    }
}
