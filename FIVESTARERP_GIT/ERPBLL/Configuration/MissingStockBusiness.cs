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
    public class MissingStockBusiness : IMissingStockBusiness
    {
        private readonly IConfigurationUnitOfWork _configDb;
        private readonly MissingStockRepository _missingStockRepository;
        public MissingStockBusiness(IConfigurationUnitOfWork configDb)
        {
            this._configDb = configDb;
            _missingStockRepository = new MissingStockRepository(this._configDb);
        }

        public MissingStock GetMissingStockOneById(long id, long orgId)
        {
            return _missingStockRepository.GetOneByOrg(s => s.OrganizationId == orgId && s.MissingStockId == id);
        }
        public MissingStock GetMissingStockOneByModelAndColorAndPartsAndStockType(long model,long color, long parts, string type, long orgId)
        {
            return _missingStockRepository.GetOneByOrg(s => s.DescriptionId == model && s.ColorId == color && s.PartsId == parts && s.OrganizationId == orgId);
        }
        public bool SaveMissingStock(MissingStockDTO dto, long userId, long branchId, long orgId)
        {
            if (dto.MissingStockId > 0)
            {
                var stockIsExists = GetMissingStockOneByModelAndColorAndPartsAndStockType(dto.DescriptionId, dto.ColorId, dto.PartsId, dto.StockType, orgId);
                if (stockIsExists!=null)
                {
                    return false;
                }
            }
            var stockInfo = GetMissingStockOneByModelAndColorAndPartsAndStockType(dto.DescriptionId, dto.ColorId, dto.PartsId, dto.StockType, orgId);
            if (stockInfo != null)
            {
                stockInfo.Quantity += dto.Quantity;
                stockInfo.UpdateDate = DateTime.Now;
                stockInfo.UpUserId = userId;
                _missingStockRepository.Update(stockInfo);
            }
            else
            {
                if (dto.MissingStockId == 0)
                {
                    MissingStock stock = new MissingStock()
                    {
                        BranchId = branchId,
                        ColorId = dto.ColorId,
                        DescriptionId = dto.DescriptionId,
                        EntryDate = DateTime.Now,
                        EUserId = userId,
                        OrganizationId = orgId,
                        PartsId = dto.PartsId,
                        Quantity = dto.Quantity,
                        StockType = dto.StockType,
                        IMEI = dto.IMEI,
                        Remarks = "",
                    };
                    _missingStockRepository.Insert(stock);
                }
                else
                {
                    var missingStock = GetMissingStockOneById(dto.MissingStockId, orgId);
                    missingStock.PartsId = dto.PartsId;
                    missingStock.Quantity = dto.Quantity;
                    missingStock.Remarks = dto.Remarks;
                    missingStock.StockType = dto.StockType;
                    missingStock.IMEI = dto.IMEI;
                    missingStock.UpdateDate = DateTime.Now;
                    missingStock.UpUserId = userId;
                    missingStock.DescriptionId = dto.DescriptionId;
                    missingStock.ColorId = dto.ColorId;
                    _missingStockRepository.Update(missingStock);
                }
            }
            return _missingStockRepository.Save();
        }

        public IEnumerable<MissingStockDTO> GetMissingStockInfoByQuery(long? modelId, long? colorId, long? partsId, string stockType,long orgId)
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
            if (partsId != null && partsId.Value > 0)
            {
                param += string.Format(@" and stock.PartsId={0}", partsId);
            }
            if (!string.IsNullOrEmpty(stockType) && stockType.Trim() != "")
            {
                param += string.Format(@" and stock.StockType='{0}'", stockType.Trim());
            }

            query = string.Format(@"Select stock.MissingStockId,stock.ColorId,co.ColorName,stock.Quantity,
stock.StockType,stock.Remarks,stock.OrganizationId,stock.DescriptionId,de.DescriptionName 'ModelName',mp.MobilePartName 'PartsName', stock.PartsId, stock.IMEI 
From tblMissingStock stock
Left Join [Inventory].dbo.tblColors co  on stock.ColorId = co.ColorId and stock.OrganizationId =co.OrganizationId
Left Join [Inventory].dbo.tblDescriptions de  on stock.DescriptionId = de.DescriptionId and stock.OrganizationId =de.OrganizationId
Left Join [Configuration].dbo.tblMobileParts mp  on stock.PartsId = mp.MobilePartId and stock.OrganizationId =mp.OrganizationId
Where 1=1 and stock.OrganizationId={0} {1}", orgId, Utility.ParamChecker(param));

            return _configDb.Db.Database.SqlQuery<MissingStockDTO>(query).ToList();
        }
    }
}
