using ERPBLL.Common;
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
    public class RepairSectionFaultyItemTransferDetailBusiness : IRepairSectionFaultyItemTransferDetailBusiness
    {
        private readonly IProductionUnitOfWork _production;
        private readonly RepairSectionFaultyItemTransferDetailRepository _repairSectionFaultyItemTransferDetailRepository;

        public RepairSectionFaultyItemTransferDetailBusiness(IProductionUnitOfWork production)
        {
            this._production = production;
            this._repairSectionFaultyItemTransferDetailRepository = new RepairSectionFaultyItemTransferDetailRepository(this._production);
        }

        public IEnumerable<RepairSectionFaultyItemTransferDetail> GetRepairSectionFaultyItemTransferDetailByInfo(long transferInfoId, long orgId)
        {
            return _repairSectionFaultyItemTransferDetailRepository.GetAll(s => s.RSFIRInfoId == transferInfoId && s.OrganizationId == orgId);
        }

        public IEnumerable<RepairSectionFaultyItemTransferDetail> GetRepairSectionFaultyItemTransferDetails(long orgId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<RepairSectionFaultyItemTransferDetail> GetRepairSectionFaultyItemTransferDetailsByInfo(long transferId, long orgId)
        {
            return this._repairSectionFaultyItemTransferDetailRepository.GetAll(s => s.RSFIRInfoId == transferId && s.OrganizationId == orgId);
        }

        public IEnumerable<RepairSectionFaultyItemTransferDetailDTO> GetRepairSectionFaultyItemTransferDetailsByQuery(long? floorId, long? repairId, string status, long? transferInfoId, long orgId)
        {
            return this._production.Db.Database.SqlQuery<RepairSectionFaultyItemTransferDetailDTO>(QueryForRepairSectionFaultyItemTransferDetails(floorId,repairId,status,transferInfoId,orgId)).ToList();
        }

        private string QueryForRepairSectionFaultyItemTransferDetails(long? floorId, long? repairId, string status, long? transferInfoId, long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;

            param += string.Format(@" and rsi.OrganizationId= {0}", orgId);
            if(floorId != null && floorId > 0)
            {
                param += string.Format(@" and rsi.ProductionFloorId= {0}", floorId);
            }
            if (repairId != null && repairId > 0)
            {
                param += string.Format(@" and rsi.RepairLineId= {0}", repairId);
            }
            if (transferInfoId != null && transferInfoId > 0)
            {
                param += string.Format(@" and rsi.RSFIRInfoId= {0}", transferInfoId);
            }
            if(!string.IsNullOrEmpty(status) && status.Trim() != "")
            {
                param += string.Format(@" and rsi.StateStatus= '{0}'", status);
            }

            query = string.Format(@"Select rsi.RSFIRInfoId,rsi.TransferCode,rsd.ProductionFloorName,rsd.RepairLineName,rsi.StateStatus,rsd.ModelName,rsd.WarehouseName,
rsd.ItemTypeName,rsd.ItemName ,rsd.FaultyQty,rsi.StateStatus,rsi.EntryDate,app.UserName 'EntryUser'
From [Production].dbo.tblRepairSectionFaultyItemTransferDetail rsd
Inner Join [Production].dbo.tblRepairSectionFaultyItemTransferInfo rsi on rsd.RSFIRInfoId = rsi.RSFIRInfoId
Inner Join [ControlPanel].dbo.tblApplicationUsers app on rsi.EUserId = app.UserId
Where 1= 1  {0}", Utility.ParamChecker(param));

            return query;
        }

        
    }
}
