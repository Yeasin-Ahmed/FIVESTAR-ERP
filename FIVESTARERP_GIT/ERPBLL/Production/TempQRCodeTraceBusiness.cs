using ERPBLL.Common;
using ERPBLL.Production.Interface;
using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using ERPDAL.ProductionDAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production
{
    public class TempQRCodeTraceBusiness : ITempQRCodeTraceBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly TempQRCodeTraceRepository _tempQRCodeTraceRepository;
        private readonly LotInLogRepository _lotInLogRepository;
        public TempQRCodeTraceBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._tempQRCodeTraceRepository = new TempQRCodeTraceRepository(this._productionDb);
            this._lotInLogRepository = new LotInLogRepository(this._productionDb);
        }

        public async Task<TempQRCodeTrace> GetIMEIinQRCode(string imei, string status, long floorId, long packagingId, long orgId)
        {
            return await _productionDb.Db.Database.SqlQuery<TempQRCodeTrace>(string.Format(@"Select * From tblTempQRCodeTrace Where  IMEI LIKE'%{0}%' and StateStatus IN({1}) and ProductionFloorId ={2} and PackagingLineId ={3} and OrganizationId = {4}", Utility.ParamChecker(imei), Utility.ParamChecker(status), floorId, packagingId, orgId)).FirstOrDefaultAsync();
        }

        public async Task<TempQRCodeTrace> GetIMEIWithOutThisQRCode(string imei, long codeId, long orgId)
        {
            return await _tempQRCodeTraceRepository.GetOneByOrgAsync(s => s.IMEI.Contains(imei) && s.CodeId != codeId && s.OrganizationId == orgId);
        }

        public async Task<TempQRCodeTrace> GetIMEIWithThisQRCode(string imei, long codeId, long orgId)
        {
            return await _tempQRCodeTraceRepository.GetOneByOrgAsync(s => s.IMEI.Contains(imei) && s.CodeId == codeId && s.OrganizationId == orgId);
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

        public TempQRCodeTrace GetTempQRCodeTraceByIMEI(string imei, long orgId)
        {
            return _tempQRCodeTraceRepository.GetOneByOrg(s => s.IMEI.Contains(imei) && s.OrganizationId == orgId);
        }

        public IEnumerable<TempQRCodeTrace> GetTempQRCodeTraceByOrg(long orgId)
        {
            return _tempQRCodeTraceRepository.GetAll(q => q.OrganizationId == orgId);
        }

        public async Task<IEnumerable<TempQRCodeTrace>> GetTempQRCodeTracesByQRCodesAsync(List<string> qrCodes, long orgId)
        {
            return await _tempQRCodeTraceRepository.GetAllAsync(s => qrCodes.Contains(s.CodeNo) && s.OrganizationId == orgId);
        }

        public bool IsExistIMEIWithStatus(string imei, string status, long orgId)
        {
            return this._tempQRCodeTraceRepository.GetOneByOrg(s => s.OrganizationId == orgId && s.IMEI.Contains(imei) && s.StateStatus == status) != null;
        }

        public bool IsExistQRCodeWithStatus(string qrCode, string status, long orgId)
        {
            return this._tempQRCodeTraceRepository.GetOneByOrg(s => s.OrganizationId == orgId && s.CodeNo == qrCode && s.StateStatus == status) != null;
        }

        public bool IsExistQRCodeWithStatus(string qrCode, DateTime date, string status, long orgId)
        {
            return this._tempQRCodeTraceRepository.GetOneByOrg(s => s.OrganizationId == orgId && s.CodeNo == qrCode && s.StateStatus == status && DbFunctions.TruncateTime(s.EntryDate) == DbFunctions.TruncateTime(date)) != null;
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

        public bool SaveQRCodeStatusByLotIn(string qrCode, long orgId, long userId)
        {
            var qrCodeInfo = GetTempQRCodeTraceByCode(qrCode, orgId);
            qrCodeInfo.StateStatus = "Lot-In";
            qrCodeInfo.UpdateDate = DateTime.Now;
            qrCodeInfo.UpUserId = userId;
            _tempQRCodeTraceRepository.Update(qrCodeInfo);

            LotInLog lotInLog = new LotInLog();
            lotInLog.AssemblyId = qrCodeInfo.AssemblyId;
            lotInLog.CodeId = qrCodeInfo.CodeId;
            lotInLog.CodeNo = qrCodeInfo.CodeNo;
            lotInLog.DescriptionId = qrCodeInfo.DescriptionId;
            lotInLog.EntryDate = DateTime.Now;
            lotInLog.EUserId = userId;
            lotInLog.ItemId = qrCodeInfo.ItemId;
            lotInLog.ItemTypeId = qrCodeInfo.ItemTypeId;
            lotInLog.OrganizationId = qrCodeInfo.OrganizationId;
            lotInLog.ProductionFloorId = qrCodeInfo.ProductionFloorId;
            lotInLog.ReferenceId = qrCodeInfo.ReferenceId;
            lotInLog.ReferenceNumber = qrCodeInfo.ReferenceNumber;
            lotInLog.Remarks = qrCodeInfo.Remarks;
            lotInLog.StateStatus = qrCodeInfo.StateStatus;
            lotInLog.WarehouseId = qrCodeInfo.WarehouseId;
            _lotInLogRepository.Insert(lotInLog);

            return _lotInLogRepository.Save();
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
            qrCodeInDb.UpdateDate = DateTime.Now;
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
