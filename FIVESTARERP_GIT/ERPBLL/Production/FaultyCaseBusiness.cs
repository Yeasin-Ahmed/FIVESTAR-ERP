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
    public class FaultyCaseBusiness : IFaultyCaseBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly FaultyCaseRepository _faultyCaseRepository;

        public FaultyCaseBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._faultyCaseRepository = new FaultyCaseRepository(this._productionDb);
        }

        public FaultyCase GetFaultyById(long faultyId, long orgId)
        {
            return _faultyCaseRepository.GetOneByOrg(f=> f.CaseId == faultyId && f.OrganizationId == orgId);
        }

        public IEnumerable<FaultyCase> GetFaultyCases(long orgId)
        {
            return _faultyCaseRepository.GetAll(f => f.OrganizationId == orgId);
        }

        public bool SaveFaultyCase(FaultyCaseDTO faulty, long userId, long orgId)
        {
            if(faulty.CaseId == 0)
            {
                FaultyCase newfaulty = new FaultyCase
                {
                    DescriptionId = faulty.DescriptionId,
                    ProblemDescription = faulty.ProblemDescription,
                    OrganizationId = orgId,
                    EUserId = userId,
                    EntryDate = DateTime.Now,
                    Remarks =faulty.Remarks
                };
                _faultyCaseRepository.Insert(newfaulty);
            }
            else
            {
                var faultyInDb = GetFaultyById(faulty.CaseId, orgId);
                if(faultyInDb != null)
                {
                    faultyInDb.DescriptionId = faulty.DescriptionId;
                    faultyInDb.ProblemDescription = faulty.ProblemDescription;
                    faultyInDb.UpdateDate = DateTime.Now;
                    faultyInDb.UpUserId = userId;
                    _faultyCaseRepository.Update(faultyInDb);
                }
            }
            return _faultyCaseRepository.Save();
        }
    }
}
