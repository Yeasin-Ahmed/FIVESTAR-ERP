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
    public class PackagingLineBusiness : IPackagingLineBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly PackagingLineRepository _packagingLineRepository;
        public PackagingLineBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._packagingLineRepository = new PackagingLineRepository(this._productionDb);
        }
        public PackagingLine GetPackagingLineById(long id, long orgId)
        {
            return _packagingLineRepository.GetOneByOrg(r => r.PackagingLineId == id && r.OrganizationId == orgId);
        }
        public IEnumerable<PackagingLine> GetPackagingLinesByOrgId(long orgId)
        {
            return _packagingLineRepository.GetAll(r => r.OrganizationId == orgId);
        }
        public bool SavePackagingLine(PackagingLineDTO dto, long userId, long orgId)
        {
            if(dto.PackagingLineId == 0)
            {
                PackagingLine pacakgingLine = new PackagingLine
                {
                    PackagingLineName = dto.PackagingLineName,
                    IsActive = dto.IsActive,
                    Remarks = dto.Remarks,
                    EUserId = userId,
                    EntryDate = DateTime.Now,
                    ProductionLineId = dto.ProductionLineId,
                    OrganizationId = orgId
                };
                _packagingLineRepository.Insert(pacakgingLine);
            }
            else
            {
                var pacakgingLineInDb = GetPackagingLineById(dto.PackagingLineId, orgId);
                if(pacakgingLineInDb != null)
                {
                    pacakgingLineInDb.PackagingLineName = dto.PackagingLineName;
                    pacakgingLineInDb.IsActive = dto.IsActive;
                    pacakgingLineInDb.Remarks = dto.Remarks;
                    pacakgingLineInDb.UpUserId = userId;
                    pacakgingLineInDb.UpdateDate = DateTime.Now;
                    _packagingLineRepository.Update(pacakgingLineInDb);
                }
            }
            return _packagingLineRepository.Save();
        }
    }
}
