using ERPBLL.Inventory.Interface;
using ERPBLL.Production.Interface;
using ERPBO.Inventory.DTOModel;
using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using ERPDAL.InventoryDAL;
using ERPDAL.ProductionDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production
{
    public class GeneratedIMEIBusiness : IGeneratedIMEIBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly IInventoryUnitOfWork _inventoryDb;
        private readonly IDescriptionBusiness _descriptionBusiness;
        private readonly GeneratedIMEIRepository _generatedIMEIRepository;
        private readonly DescriptionRepository _descriptionRepository;
        public GeneratedIMEIBusiness(IProductionUnitOfWork productionDb, IInventoryUnitOfWork inventoryDb, IDescriptionBusiness descriptionBusiness)
        {
            this._productionDb = productionDb;
            this._generatedIMEIRepository = new GeneratedIMEIRepository(this._productionDb);

            this._descriptionBusiness = descriptionBusiness;

            this._inventoryDb = inventoryDb;
            this._descriptionRepository = new DescriptionRepository(this._inventoryDb);
        }

        public IEnumerable<GeneratedIMEIDTO> GetGeneratedIMEIDTOs(long userId,long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<GeneratedIMEIDTO>(string.Format(@"Select *,app.UserName 'EntryUser' From tblGeneratedIMEI ge
Left  Join [ControlPanel].dbo.tblApplicationUsers app on ge.EUserId = app.UserId
Where 1= 1 and ge.OrganizationId={0} and ge.EUserId={1} Order By ge.Id desc", orgId,userId)).ToList();
        }

        public bool SaveGeneratedIMEI(List<GeneratedIMEIDTO> model, long userId, long orgId)
        {
            List<GeneratedIMEI> generatedIMEIs = new List<GeneratedIMEI>();
            foreach (var item in model)
            {
                GeneratedIMEI iMEI = new GeneratedIMEI()
                {
                    FloorId = item.FloorId,
                    ProductionFloorName = item.ProductionFloorName,
                    PackagingLineId = item.PackagingLineId,
                    PackagingLineName = item.PackagingLineName,
                    AssemblyLineId = item.AssemblyLineId,
                    AssemblyLineName = item.AssemblyLineName,
                    CodeId = item.CodeId,
                    QRCode = item.QRCode,
                    DescriptionId = item.DescriptionId,
                    DescriptionName = item.DescriptionName,
                    IMEI = item.IMEI,
                    OrganizationId = orgId,
                    EUserId = userId,
                    EntryDate = DateTime.Now,
                    Remarks= ""
                };
                generatedIMEIs.Add(iMEI);
            }
            _generatedIMEIRepository.InsertAll(generatedIMEIs);
            return _generatedIMEIRepository.Save();
        }

        public bool SaveGeneratedIMEIByUser(CommitGeneratedIMEIDTO model, long userId, long orgId)
        {
            List<GeneratedIMEI> generatedIMEIs = new List<GeneratedIMEI>();
            foreach (var item in model.IMEIs)
            {
                GeneratedIMEI iMEI = new GeneratedIMEI()
                {
                    FloorId =null,
                    ProductionFloorName = null,
                    PackagingLineId = null,
                    PackagingLineName = null,
                    AssemblyLineId = null,
                    AssemblyLineName = null,
                    CodeId = 0,
                    QRCode = "",
                    DescriptionId = model.DescriptionId,
                    DescriptionName = model.DescriptionName,
                    IMEI = item,
                    OrganizationId = orgId,
                    EUserId = userId,
                    EntryDate = DateTime.Now,
                    Remarks = ""
                };
                generatedIMEIs.Add(iMEI);
            }
            _generatedIMEIRepository.InsertAll(generatedIMEIs);

            var modelInDb = _descriptionBusiness.GetDescriptionOneByOrdId(model.DescriptionId, orgId);
            modelInDb.EndPoint = model.Serial;
            _descriptionRepository.Update(modelInDb);
            _descriptionRepository.Save();
            return _generatedIMEIRepository.Save();
        }
    }
}
