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
                    StockType = dto.StockType,
                    IMEI = dto.IMEI,
                    Remarks = "",
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
                handsetInfo.IMEI = dto.IMEI;
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

            query = string.Format(@"Select stock.HandSetStockId,stock.ColorId,co.ColorName,stock.IMEI,
stock.StockType,stock.Remarks,stock.OrganizationId,stock.DescriptionId,de.DescriptionName 'ModelName' 
From tblHandSetStock stock
Left Join [Inventory].dbo.tblColors co  on stock.ColorId = co.ColorId and stock.OrganizationId =co.OrganizationId
Left Join [Inventory].dbo.tblDescriptions de  on stock.DescriptionId = de.DescriptionId and stock.OrganizationId =de.OrganizationId
Where 1=1 and stock.OrganizationId={0} {1}", orgId, Utility.ParamChecker(param));

            return _configurationDb.Db.Database.SqlQuery<HandSetStockDTO>(query).ToList();
        }
        public bool IsDuplicateHandsetStockIMEI(string imei, long id, long orgId)
        {
            return _handSetStockRepository.GetOneByOrg(s => s.IMEI == imei && s.OrganizationId == orgId && s.HandSetStockId != id) != null ? true : false;
        }
    }
}
