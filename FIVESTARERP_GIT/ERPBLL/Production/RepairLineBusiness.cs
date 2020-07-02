using ERPBLL.Production.Interface;
using ERPBO.Common;
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
    public class RepairLineBusiness : IRepairLineBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly RepairLineRepository _repairLineRepository;
        public RepairLineBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._repairLineRepository = new RepairLineRepository(this._productionDb);
        }
        public RepairLine GetRepairLineById(long id, long orgId)
        {
            return _repairLineRepository.GetOneByOrg(r => r.RepairLineId == id && r.OrganizationId == orgId);
        }

        public IEnumerable<RepairLine> GetRepairLinesByOrgId(long orgId)
        {
            return _repairLineRepository.GetAll(r => r.OrganizationId == orgId);
        }

        public IEnumerable<Dropdown> GetRepairLineWithFloor(long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<Dropdown>(string.Format(@"Select Cast(rl.RepairLineId as Nvarchar(100))+'#'+Cast(pl.LineId as nvarchar(100)) 'value',
rl.RepairLineName +' ['+ pl.LineNumber+']' 'text' From tblRepairLine rl
Inner Join tblProductionLines pl on rl.ProductionLineId = pl.LineId
Where 1= 1 and rl.OrganizationId={0}", orgId)).ToList();
        }

        public bool SaveRepairLine(RepairLineDTO dto, long userId, long orgId)
        {
            if(dto.RepairLineId == 0)
            {
                RepairLine repairLine = new RepairLine
                {
                    RepairLineName = dto.RepairLineName,
                    IsActive = dto.IsActive,
                    Remarks = dto.Remarks,
                    EUserId = userId,
                    EntryDate = DateTime.Now,
                    ProductionLineId = dto.ProductionLineId,
                    OrganizationId = orgId
                };
                _repairLineRepository.Insert(repairLine);
            }
            else
            {
                var repairLineInDb = GetRepairLineById(dto.RepairLineId, orgId);
                if(repairLineInDb != null)
                {
                    repairLineInDb.RepairLineName = dto.RepairLineName;
                    repairLineInDb.IsActive = dto.IsActive;
                    repairLineInDb.Remarks = dto.Remarks;
                    repairLineInDb.UpUserId = userId;
                    repairLineInDb.UpdateDate = DateTime.Now;
                    _repairLineRepository.Update(repairLineInDb);
                }
            }
            return _repairLineRepository.Save();
        }
    }
}
