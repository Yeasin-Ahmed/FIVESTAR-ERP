using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPBLL.Production.Interface;
using ERPBO.Common;
using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using ERPDAL.ProductionDAL;

namespace ERPBLL.Production
{
    public class AssemblyLineBusiness : IAssemblyLineBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly AssemblyLineRepository _assemblyLineRepository;
        public AssemblyLineBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._assemblyLineRepository = new AssemblyLineRepository(this._productionDb);
        }
        public AssemblyLine GetAssemblyLineById(long id, long orgId)
        {
            return _assemblyLineRepository.GetOneByOrg(a => a.AssemblyLineId == id && a.OrganizationId == orgId);
        }
        public IEnumerable<AssemblyLine> GetAssemblyLines(long orgId)
        {
            return _assemblyLineRepository.GetAll(a =>a.OrganizationId == orgId).ToList();
        }

        public IEnumerable<Dropdown> GetAssemblyLinesWithProduction(long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<Dropdown>(string.Format(@"Select (al.AssemblyLineName+' ['+pl.LineNumber+']') 'text'
, Cast(al.AssemblyLineId as Nvarchar(50))+'#'+Cast(pl.LineId as Nvarchar(50)) 'value'
From [Production].dbo.tblAssemblyLines al
Inner Join  [Production].dbo.tblProductionLines pl on pl.LineId = al.AssemblyLineId
Where 1=1 and al.OrganizationId={0}", orgId)).ToList();
        }

        public bool SaveAssembly(AssemblyLineDTO line, long userId, long orgId)
        {
            if(line != null)
            {
                if(line.AssemblyLineId == 0)
                {
                    // Insert 
                    AssemblyLine assembly = new AssemblyLine
                    {
                        AssemblyLineName = line.AssemblyLineName,
                        IsActive = line.IsActive,
                        Remarks = line.Remarks,
                        OrganizationId  = orgId,
                        EUserId = userId,
                        EntryDate = DateTime.Now,
                        ProductionLineId = line.ProductionLineId
                    };
                    _assemblyLineRepository.Insert(assembly);
                }
                else
                {
                    AssemblyLine assembly = GetAssemblyLineById(line.AssemblyLineId, orgId);
                    if(assembly != null)
                    {
                        assembly.AssemblyLineName = line.AssemblyLineName;
                        assembly.IsActive = line.IsActive;
                        assembly.Remarks = line.Remarks;
                        assembly.UpUserId = userId;
                        assembly.UpdateDate = DateTime.Now;
                        _assemblyLineRepository.Update(assembly);
                    }
                }
            }
            return _assemblyLineRepository.Save();
        }
    }
}
