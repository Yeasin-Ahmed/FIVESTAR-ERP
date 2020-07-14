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

        public async Task<QRCodeTrace> GetQRCodeTraceByCodeAsync(string code, long orgId)
        {
            return await _qRCodeTraceRepository.GetOneByOrgAsync(s => s.CodeNo == code && s.OrganizationId == orgId);
        }

        public IEnumerable<QRCodeTrace> GetQRCodeTraceByOrg(long orgId)
        {
            return _qRCodeTraceRepository.GetAll(q => q.OrganizationId == orgId);
        }
        public bool SaveQRCodeTrace(List<QRCodeTraceDTO> dtos, long userId, long orgId)
        {
            List<QRCodeTrace> list = new List<QRCodeTrace>();
            foreach (var item in dtos)
            {
                QRCodeTrace qRCode = new QRCodeTrace()
                {
                    ProductionFloorId= item.ProductionFloorId,
                    DescriptionId = item.DescriptionId,
                    ColorId = item.ColorId,
                    ColorName = item.ColorName,
                    CodeNo = item.CodeNo,
                    CodeImage = item.CodeImage,
                    WarehouseId = item.WarehouseId,
                    ItemTypeId = item.ItemTypeId,
                    ItemId = item.ItemId,
                    OrganizationId = orgId,
                    EUserId =  userId,
                    EntryDate = DateTime.Now,
                    ReferenceId = item.ReferenceId,
                    ReferenceNumber = item.ReferenceNumber,
                    Remarks = item.Remarks,
                    ProductionFloorName = item.ProductionFloorName,
                    ModelName = item.ModelName,
                    WarehouseName = item.WarehouseName,
                    ItemTypeName = item.ItemTypeName,
                    ItemName = item.ItemName,
                    AssemblyId =item.AssemblyId,
                    AssemblyLineName = item.AssemblyLineName
                };
                list.Add(qRCode);
            }
            if(list.Count > 0)
            {
                _qRCodeTraceRepository.InsertAll(list);
            }
            return _qRCodeTraceRepository.Save();
        }
    }
}
