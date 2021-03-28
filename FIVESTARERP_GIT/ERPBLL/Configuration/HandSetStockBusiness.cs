using ERPBLL.Common;
using ERPBLL.Configuration.Interface;
using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using ERPDAL.ConfigurationDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration
{
    public class HandSetStockBusiness : IHandSetStockBusiness
    {
        private readonly IConfigurationUnitOfWork _configurationDb;
        private readonly HandSetStockRepository _handSetStockRepository;
        public HandSetStockBusiness(IConfigurationUnitOfWork configurationDb)
        {
            this._configurationDb = configurationDb;
            _handSetStockRepository = new HandSetStockRepository(this._configurationDb);
        }
        public IEnumerable<HandSetStock> GetAllHansetStockByOrgId(long orgId)
        {
            return _handSetStockRepository.GetAll(s => s.OrganizationId == orgId).ToList();
        }
        public IEnumerable<HandSetStock> GetAllHansetStockByOrgIdAndBranchId(long orgId, long branchId)
        {
            return _handSetStockRepository.GetAll(s => s.OrganizationId == orgId && s.BranchId == branchId).ToList();
        }
        public HandSetStock GetHandsetOneInfoById(long id, long orgId)
        {
            return _handSetStockRepository.GetOneByOrg(s => s.HandSetStockId == id && s.OrganizationId == orgId);
        }
        public bool SaveHandSetStock(HandSetStockDTO dto, long userId, long branchId, long orgId)
        {
            if (dto.HandSetStockId == 0)
            {
                HandSetStock handSet = new HandSetStock()
                {
                    DescriptionId = dto.DescriptionId,
                    BranchId = branchId,
                    ColorId = dto.ColorId,
                    IMEI = dto.IMEI,
                    IMEI1=dto.IMEI1,
                    StateStatus = StockStatus.StockIn,
                    StockType = dto.StockType,
                    Remarks = "StockIn",
                    OrganizationId = orgId,
                    EUserId = userId,
                    EntryDate = DateTime.Now,
                };
                _handSetStockRepository.Insert(handSet);
            }
            else
            {
                var handsetInfo = GetHandsetOneInfoById(dto.HandSetStockId, orgId);
                handsetInfo.StockType = dto.StockType;
                handsetInfo.ColorId = dto.ColorId;
                handsetInfo.DescriptionId = dto.DescriptionId;
                handsetInfo.UpUserId = userId;
                handsetInfo.UpdateDate = DateTime.Now;
                _handSetStockRepository.Update(handsetInfo);
            }
            return _handSetStockRepository.Save();
        }
        public IEnumerable<HandSetStockDTO> GetHandsetStockInformationsByQuery(long? modelId, long? colorId, string stockType, long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;

            if (modelId != null && modelId.Value > 0)
            {
                param += string.Format(@" and stock.DescriptionId={0}", modelId);
            }
            if (colorId != null && colorId.Value > 0)
            {
                param += string.Format(@" and stock.ColorId={0}", colorId);
            }
            if (!string.IsNullOrEmpty(stockType) && stockType.Trim() != "")
            {
                param += string.Format(@" and stock.StockType='{0}'", stockType.Trim());
            }

            query = string.Format(@"Select stock.HandSetStockId,stock.ColorId,co.ColorName,stock.IMEI,stock.IMEI1,
stock.StockType,stock.Remarks,stock.OrganizationId,stock.DescriptionId,de.ModelName 
From tblHandSetStock stock
Left Join [Configuration].dbo.tblColorSS co  on stock.ColorId = co.ColorId and stock.OrganizationId =co.OrganizationId
Left Join [Configuration].dbo.tblModelSS de  on stock.DescriptionId = de.ModelId and stock.OrganizationId =de.OrganizationId
Where 1=1 and stock.StateStatus='Stock-In' and stock.OrganizationId={0} {1}", orgId, Utility.ParamChecker(param));

            return _configurationDb.Db.Database.SqlQuery<HandSetStockDTO>(query).ToList();
        }
        public bool IsDuplicateHandsetStockIMEI(string imei, long id, long orgId)
        {
            return _handSetStockRepository.GetOneByOrg(s => s.IMEI == imei && s.OrganizationId == orgId && s.HandSetStockId != id) != null ? true : false;
        }
        public bool IsExistsHandsetStockIMEI(string imei,long orgId, string status)
        {
            return _handSetStockRepository.GetOneByOrg(s => s.IMEI1 == imei && s.OrganizationId == orgId && s.StateStatus == status) != null ? true : false;
        }
        public IEnumerable<HandSetStockDTO> GetAllHansetModelAndColor(long orgId)
        {
            return this._configurationDb.Db.Database.SqlQuery<HandSetStockDTO>(
                string.Format(@"SELECT (Cast(hst.DescriptionId as nvarchar(100)) + '#' + Cast(hst.ColorId as nvarchar(100))) 'ModelId',(des.ModelName +'-'+ clr.ColorName)'ModelName'
FROM [Configuration].dbo.tblHandsetStock hst
Inner Join [Configuration].dbo.tblModelSS des on hst.DescriptionId = des.ModelId and hst.OrganizationId = des.OrganizationId
Inner Join [Configuration].dbo.tblColorSS clr on hst.ColorId = clr.ColorId and hst.OrganizationId = clr.OrganizationId
where hst.OrganizationId={0}", orgId)).ToList();
        }

        public HandSetStock GetIMEI2ByIMEI1(string imei, long branchId, long orgId)
        {
            return _handSetStockRepository.GetOneByOrg(s => s.IMEI1 == imei && s.BranchId == branchId && s.OrganizationId == orgId);
        }
    }
}
