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
    public class TempQRCodeTraceBusiness : ITempQRCodeTraceBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly TempQRCodeTraceRepository _tempQRCodeTraceRepository;
        public TempQRCodeTraceBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._tempQRCodeTraceRepository = new TempQRCodeTraceRepository(this._productionDb);
        }

        public async Task<TempQRCodeTrace> GetIMEIinQRCode(string imei, string status, long floorId, long packagingId, long orgId)
        {
            return await _productionDb.Db.Database.SqlQuery<TempQRCodeTrace>(string.Format(@"Select * From tblTempQRCodeTrace Where  IMEI LIKE'%{0}%' and StateStatus IN({1}) and ProductionFloorId ={2} and PackagingLineId ={3} and OrganizationId = {4}", imei, status, floorId, packagingId, orgId)).FirstOrDefaultAsync();
        }

        public TempQRCodeTrace GetTempQRCodeTraceByCode(string code, long orgId)
        {
            return _tempQRCodeTraceRepository.GetOneByOrg(q => q.CodeNo == code && q.OrganizationId == orgId);
        }

        public async Task<TempQRCodeTrace> GetTempQRCodeTraceByCodeAsync(string code, long orgId)
        {
            return await _tempQRCodeTraceRepository.GetOneByOrgAsync(s => s.CodeNo == code && s.OrganizationId == orgId);
        }

        public async Task<TempQRCodeTrace> GetTempQRCodeTraceByCodeWithFloorAsync(string code,string status,long? floorId, long orgId)
        {
            return await _productionDb.Db.Database.SqlQuery<TempQRCodeTrace>(string.Format(@"Select * From tblTempQRCodeTrace Where  CodeNo ='{0}' and StateStatus IN({1}) and ProductionFloorId ={2} and OrganizationId = {3}", code, status, floorId, orgId)).FirstOrDefaultAsync();
                
                //_tempQRCodeTraceRepository.GetOneByOrgAsync(s => s.CodeNo == code && s.ProductionFloorId == floorId && s.OrganizationId == orgId);
        }

        public IEnumerable<TempQRCodeTrace> GetTempQRCodeTraceByOrg(long orgId)
        {
            return _tempQRCodeTraceRepository.GetAll(q => q.OrganizationId == orgId);
        }

        public async Task<IEnumerable<TempQRCodeTrace>> GetTempQRCodeTracesByQRCodesAsync(List<string> qrCodes, long orgId)
        {
            return await _tempQRCodeTraceRepository.GetAllAsync(s => qrCodes.Contains(s.CodeNo) && s.OrganizationId == orgId);
        }

        public bool IsExistQRCodeWithStatus(string qrCode, string status, long orgId)
        {
            return this._tempQRCodeTraceRepository.GetOneByOrg(s => s.OrganizationId == orgId && s.CodeNo == qrCode && s.StateStatus == status) != null;
        }

        public async Task<bool> SaveBatteryCodeAsync(BatteryWriteDTO dto, long userId, long orgId)
        {
            var imeiInDb = await GetIMEIinQRCode(dto.imei,string.Format(@"'PackagingLine'"),dto.floorId,dto.packagingLineId, orgId);

            if(imeiInDb != null && string.IsNullOrEmpty(imeiInDb.BatteryCode))
            {
                imeiInDb.BatteryCode = dto.batteryCode;
                imeiInDb.UpdateDate = DateTime.Now;
                imeiInDb.UpUserId = userId;
                _tempQRCodeTraceRepository.Update(imeiInDb);
            }
            return await _tempQRCodeTraceRepository.SaveAsync();
        }

        public async Task<bool> SaveQRCodeIEMIAsync(IMEIWriteDTO dto, long userId, long orgId)
        {
            var qrCodeInDb = await GetTempQRCodeTraceByCodeAsync(dto.qrCode, orgId);
            if(qrCodeInDb != null)
            {
                var previousBarCode = qrCodeInDb.IMEI;
                qrCodeInDb.IMEI = dto.barCode;
                var preIEMIinDb = string.IsNullOrEmpty(qrCodeInDb.PreviousIMEI) ? "" : qrCodeInDb.PreviousIMEI +",";
                qrCodeInDb.PreviousIMEI = preIEMIinDb+previousBarCode;
                qrCodeInDb.StateStatus = "PackagingLine";
                qrCodeInDb.UpdateDate = DateTime.Now;
                qrCodeInDb.PackagingLineId = dto.packagingLineId;
                qrCodeInDb.PackagingLineName = dto.packagingLineName;
                qrCodeInDb.UpUserId = userId;
                _tempQRCodeTraceRepository.Update(qrCodeInDb);
            }
            return await _tempQRCodeTraceRepository.SaveAsync();
        }

        public bool SaveTempQRCodeTrace(List<TempQRCodeTraceDTO> dtos, long userId, long orgId)
        {
            List<TempQRCodeTrace> list = new List<TempQRCodeTrace>();
            foreach (var item in dtos)
            {
                TempQRCodeTrace qRCode = new TempQRCodeTrace()
                {
                    ProductionFloorId = item.ProductionFloorId,
                    DescriptionId = item.DescriptionId,
                    ColorId = item.ColorId,
                    ColorName = item.ColorName,
                    CodeNo = item.CodeNo,
                    CodeImage = item.CodeImage,
                    WarehouseId = item.WarehouseId,
                    ItemTypeId = item.ItemTypeId,
                    ItemId = item.ItemId,
                    OrganizationId = orgId,
                    EUserId = userId,
                    EntryDate = DateTime.Now,
                    ReferenceId = item.ReferenceId,
                    ReferenceNumber = item.ReferenceNumber,
                    Remarks = item.Remarks,
                    ProductionFloorName = item.ProductionFloorName,
                    ModelName = item.ModelName,
                    WarehouseName = item.WarehouseName,
                    ItemTypeName = item.ItemTypeName,
                    ItemName = item.ItemName,
                    AssemblyId = item.AssemblyId,
                    AssemblyLineName = item.AssemblyLineName
                };
                list.Add(qRCode);
            }
            if (list.Count > 0)
            {
                _tempQRCodeTraceRepository.InsertAll(list);
            }
            return _tempQRCodeTraceRepository.Save();
        }

        public async Task<bool> UpdateQRCodeBatchAsync(List<TempQRCodeTrace> qrCodes, long orgId)
        {
             _tempQRCodeTraceRepository.UpdateAll(qrCodes);
            return await _tempQRCodeTraceRepository.SaveAsync();
        }

        public bool UpdateQRCodeStatus(string qrCode, string status, long orgId)
        {
            var qrCodeInDb = GetTempQRCodeTraceByCode(qrCode, orgId);
            qrCodeInDb.StateStatus = status;
            _tempQRCodeTraceRepository.Update(qrCodeInDb);
            return _tempQRCodeTraceRepository.Save();
        }

        public async Task<bool> UpdateQRCodeStatusAsync(string qrCode, string status, long orgId)
        {
            var qrCodeInDb = GetTempQRCodeTraceByCode(qrCode, orgId);
            qrCodeInDb.StateStatus = status;
            _tempQRCodeTraceRepository.Update(qrCodeInDb);
            return await _tempQRCodeTraceRepository.SaveAsync();
        }

        public async Task<bool> UpdateQRCodeStatusWithQCAsync(string qrCode, string status,long qcId, string qcName, long orgId)
        {
            var qrCodeInDb = GetTempQRCodeTraceByCode(qrCode, orgId);
            qrCodeInDb.StateStatus = status;
            qrCodeInDb.QCLineId = qcId;
            qrCodeInDb.QCLineName = qcName;
            _tempQRCodeTraceRepository.Update(qrCodeInDb);
            return await _tempQRCodeTraceRepository.SaveAsync();
        }
    }
}
