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
    public class StockItemReturnInfoBusiness : IStockItemReturnInfoBusiness
    {
        // Database //
        private readonly IProductionUnitOfWork _productionDb;
        // Business //
        private readonly IAssemblyLineStockDetailBusiness _assemblyLineStockDetailBusiness;
        private readonly IRepairLineStockDetailBusiness _repairLineStockDetailBusiness;
        private readonly IPackagingLineStockDetailBusiness _packagingLineStockDetailBusiness;
        private readonly IPackagingRepairRawStockDetailBusiness _repairRawStockDetailBusiness;
        // Repository //
        private readonly StockItemReturnInfoRepository _stockItemReturnInfoRepository;

        public StockItemReturnInfoBusiness(IProductionUnitOfWork productionDb, IAssemblyLineStockDetailBusiness assemblyLineStockDetailBusiness, IRepairLineStockDetailBusiness repairLineStockDetailBusiness, IPackagingLineStockDetailBusiness packagingLineStockDetailBusiness, IPackagingRepairRawStockDetailBusiness repairRawStockDetailBusiness)
        {
            // Database
            this._productionDb = productionDb;
            // Business 
            this._assemblyLineStockDetailBusiness = assemblyLineStockDetailBusiness;
            this._repairLineStockDetailBusiness = repairLineStockDetailBusiness;
            this._packagingLineStockDetailBusiness = packagingLineStockDetailBusiness;
            this._repairRawStockDetailBusiness = repairRawStockDetailBusiness;
            // Repository 
            this._stockItemReturnInfoRepository = new StockItemReturnInfoRepository(this._productionDb);
        }

        public StockItemReturnInfo GetStockItemReturnInfoById(long id, long orgId)
        {
            return this._stockItemReturnInfoRepository.GetOneByOrg(s => s.SIRInfoId == id && s.OrganizationId == orgId);
        }

        public IEnumerable<StockItemReturnInfo> GetStockItemReturnInfoByOrg(long orgId)
        {
            return this._stockItemReturnInfoRepository.GetAll(s => s.OrganizationId == orgId);
        }

        public IEnumerable<StockItemReturnInfoDTO> GetStockItemReturnInfosByQuery(long? modelId,long? floorId, long? assemblyId, long? repairId, long? packagingId, long? warehouse, long? transferId, string transferCode, string returnFlag, string status, string fromDate, string toDate, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<StockItemReturnInfoDTO>(QueryForStockItemReturnInfos(modelId,floorId, assemblyId, repairId, packagingId, warehouse, transferId, transferCode, returnFlag, status, fromDate, toDate, orgId)).ToList();
        }

        private string QueryForStockItemReturnInfos(long? modelId, long? floorId, long? assemblyId, long? repairId, long? packagingId, long? warehouse, long? transferId, string transferCode, string returnFlag, string status, string fromDate, string toDate, long orgId)
        {
            string param = string.Empty;
            string query = string.Empty;

            if (floorId != null && floorId > 0)
            {
                param += string.Format(@" and sr.ProductionFloorId={0}", floorId);
            }
            if (modelId != null && modelId > 0)
            {
                param += string.Format(@" and sr.DescriptionId={0}", modelId);
            }
            if (assemblyId !=null && assemblyId > 0)
            {
                param += string.Format(@" and sr.AssemblyLineId={0}", assemblyId);
            }
            if(repairId != null && repairId > 0)
            {
                param += string.Format(@" and sr.RepairLineId={0}", repairId);
            }
            if(packagingId !=null && packagingId > 0)
            {
                param += string.Format(@" and sr.PackagingLineId={0}", packagingId);
            }
            if(warehouse !=null && warehouse > 0)
            {
                param += string.Format(@" and sr.WarehouseId={0}", warehouse);
            }
            if(transferId !=null && transferId > 0)
            {
                param += string.Format(@" and sr.SIRInfoId={0}", transferId);
            }
            if(!string.IsNullOrEmpty(transferCode) && transferCode.Trim() != "")
            {
                param += string.Format(@" and sr.ReturnCode LIKE '%{0}%'", transferCode.Trim());
            }
            if (!string.IsNullOrEmpty(returnFlag) && returnFlag.Trim() != "")
            {
                param += string.Format(@" and sr.Flag='{0}'", returnFlag.Trim());
            }
            if (!string.IsNullOrEmpty(status) && status.Trim() != "")
            {
                param += string.Format(@" and sr.StateStatus='{0}'", status.Trim());
            }
            if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "" && !string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(sr.EntryDate as date) between '{0}' and '{1}'", fDate, tDate);
            }
            else if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(sr.EntryDate as date)='{0}'", fDate);
            }
            else if (!string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(sr.EntryDate as date)='{0}'", tDate);
            }

            query = string.Format(@"Select sr.SIRInfoId,sr.ReturnCode,sr.DescriptionId,de.DescriptionName 'ModelName',sr.ProductionFloorId,pl.LineNumber 'ProductionFloorName',
	(Case When (sr.AssemblyLineId IS NOT NULL OR sr.AssemblyLineId > 0)  then al.AssemblyLineName
	 When (sr.RepairLineId IS NOT NULL OR rl.RepairLineId > 0)  then rl.RepairLineName
	 When (sr.PackagingLineId IS NOT NULL OR pac.PackagingLineId > 0)  then pac.PackagingLineName
	 else NULL end) 'Section',sr.Flag,sr.Remarks,sr.EntryDate,app.UserName 'EntryUser',sr.StateStatus
From tblStockItemReturnInfo sr
Inner Join [Inventory].dbo.tblDescriptions de on sr.DescriptionId = de.DescriptionId
Inner Join [Inventory].dbo.tblWarehouses w on sr.WarehouseId = w.Id
Inner Join [Production].dbo.tblProductionLines pl on sr.ProductionFloorId =pl.LineId
Left Join [Production].dbo.tblAssemblyLines al on sr.AssemblyLineId = al.AssemblyLineId
Left Join [Production].dbo.tblRepairLine rl on sr.RepairLineId = rl.RepairLineId
Left Join [Production].dbo.tblPackagingLine pac on sr.PackagingLineId=pac.PackagingLineId
Inner Join [ControlPanel].dbo.tblApplicationUsers app on sr.EUserId = app.UserId
Where 1=1 and sr.OrganizationId={0} {1}",orgId, Utility.ParamChecker(param));

            return query;
        }

        public bool SaveStockItemReturn(List<StockItemReturnDetailDTO> items, long userId, long orgId)
        {
            var itemWarehouses = items.Select(s => s.WarehouseId).Distinct().ToList();
            string flag = string.Empty;
            List<StockItemReturnInfo> stockItemReturnInfos = new List<StockItemReturnInfo>();
            List<AssemblyLineStockDetailDTO> assemblyLineStocks = new List<AssemblyLineStockDetailDTO>();
            List<RepairLineStockDetailDTO> repairLineStocks = new List<RepairLineStockDetailDTO>();
            List<PackagingLineStockDetailDTO> packagingLineStocks = new List<PackagingLineStockDetailDTO>();
            List<PackagingRepairRawStockDetailDTO> packagingRepairRawStocks = new List<PackagingRepairRawStockDetailDTO>();

            foreach (var stockWarehouse in itemWarehouses)
            {
                string returnCode = ("SIR-" + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss"));

                var data = items.Where(s => s.WarehouseId == stockWarehouse);
                StockItemReturnInfo stockItemReturnInfo = new StockItemReturnInfo()
                {
                    DescriptionId = data.FirstOrDefault().DescriptionId,
                    ProductionFloorId = data.FirstOrDefault().ProductionFloorId,
                    AssemblyLineId = data.FirstOrDefault().AssemblyLineId,
                    RepairLineId = data.FirstOrDefault().RepairLineId,
                    PackagingLineId = data.FirstOrDefault().PackagingLineId,
                    Flag = data.FirstOrDefault().Flag,
                    WarehouseId = stockWarehouse,
                    OrganizationId = orgId,
                    EUserId = userId,
                    EntryDate = DateTime.Now,
                    StateStatus = "Send",
                    ReturnCode = returnCode
                };
                List<StockItemReturnDetail> stockItemReturnDetails = new List<StockItemReturnDetail>();
                foreach (var item in data)
                {
                    flag = item.Flag;
                    stockItemReturnInfo.Remarks = (item.AssemblyLineId != null && item.AssemblyLineId > 0) ? "Stock Item Return By Assembly Line" : ((item.RepairLineId != null && item.RepairLineId > 0) ? "Stock Item Return By Repair Line" : "Stock Item Return By Packaging Line");
                    StockItemReturnDetail stockItemReturnDetail = new StockItemReturnDetail()
                    {
                        DescriptionId = item.DescriptionId,
                        ProductionFloorId = item.ProductionFloorId,
                        AssemblyLineId = item.AssemblyLineId,
                        RepairLineId = item.RepairLineId,
                        PackagingLineId = item.PackagingLineId,
                        Flag = item.Flag,
                        WarehouseId = item.WarehouseId,
                        ItemTypeId = item.ItemTypeId,
                        ItemId = item.ItemId,
                        UnitId = item.UnitId,
                        Quantity = item.Quantity,
                        OrganizationId = orgId,
                        EUserId = userId,
                        EntryDate = DateTime.Now,
                        Remarks = stockItemReturnInfo.Remarks,
                        ReturnCode = returnCode
                    };
                    stockItemReturnDetails.Add(stockItemReturnDetail);
                    if (item.Flag == StockRetunFlag.AssemblyLine)
                    {
                        AssemblyLineStockDetailDTO assemblyLineStockDetail = new AssemblyLineStockDetailDTO()
                        {
                            AssemblyLineId = item.AssemblyLineId,
                            ProductionLineId = item.ProductionFloorId,
                            DescriptionId = item.DescriptionId,
                            WarehouseId = item.WarehouseId,
                            ItemTypeId = item.ItemTypeId,
                            ItemId = item.ItemId,
                            UnitId = item.UnitId,
                            Quantity = item.Quantity,
                            OrganizationId = orgId,
                            EUserId = userId,
                            EntryDate = DateTime.Now,
                            Remarks = stockItemReturnInfo.Remarks,
                            RefferenceNumber = returnCode,
                            StockStatus = StockStatus.StockReturn
                        };
                        assemblyLineStocks.Add(assemblyLineStockDetail);
                    }
                    else if (item.Flag == StockRetunFlag.AssemblyRepair)
                    {
                        RepairLineStockDetailDTO repairLineStockDetail = new RepairLineStockDetailDTO()
                        {
                            RepairLineId = item.RepairLineId,
                            ProductionLineId = item.ProductionFloorId,
                            DescriptionId = item.DescriptionId,
                            WarehouseId = item.WarehouseId,
                            ItemTypeId = item.ItemTypeId,
                            ItemId = item.ItemId,
                            UnitId = item.UnitId,
                            Quantity = item.Quantity,
                            OrganizationId = orgId,
                            EUserId = userId,
                            EntryDate = DateTime.Now,
                            Remarks = stockItemReturnInfo.Remarks,
                            RefferenceNumber = returnCode,
                            StockStatus = StockStatus.StockReturn
                        };
                        repairLineStocks.Add(repairLineStockDetail);
                    }
                    else if (item.Flag == StockRetunFlag.PackagingLine)
                    {
                        PackagingLineStockDetailDTO packagingLineStockDetail = new PackagingLineStockDetailDTO()
                        {
                            PackagingLineId = item.PackagingLineId,
                            ProductionLineId = item.ProductionFloorId,
                            DescriptionId = item.DescriptionId,
                            WarehouseId = item.WarehouseId,
                            ItemTypeId = item.ItemTypeId,
                            ItemId = item.ItemId,
                            UnitId = item.UnitId,
                            Quantity = item.Quantity,
                            OrganizationId = orgId,
                            EUserId = userId,
                            EntryDate = DateTime.Now,
                            Remarks = stockItemReturnInfo.Remarks,
                            RefferenceNumber = returnCode,
                            StockStatus = StockStatus.StockReturn
                        };
                        packagingLineStocks.Add(packagingLineStockDetail);
                    }
                    else if (item.Flag == StockRetunFlag.PackagingRepair)
                    {
                        PackagingRepairRawStockDetailDTO packagingRepairStockDetail = new PackagingRepairRawStockDetailDTO()
                        {
                            PackagingLineId = item.PackagingLineId,
                            FloorId = item.ProductionFloorId,
                            DescriptionId = item.DescriptionId,
                            WarehouseId = item.WarehouseId,
                            ItemTypeId = item.ItemTypeId,
                            ItemId = item.ItemId,
                            UnitId = item.UnitId,
                            Quantity = item.Quantity,
                            OrganizationId = orgId,
                            EUserId = userId,
                            EntryDate = DateTime.Now,
                            Remarks = stockItemReturnInfo.Remarks,
                            RefferenceNumber = returnCode,
                            StockStatus = StockStatus.StockReturn
                        };
                        packagingRepairRawStocks.Add(packagingRepairStockDetail);
                    }
                }
                stockItemReturnInfo.StockItemReturnDetails = stockItemReturnDetails;
                stockItemReturnInfos.Add(stockItemReturnInfo);
            }
            if (stockItemReturnInfos.Count > 0)
            {
                _stockItemReturnInfoRepository.InsertAll(stockItemReturnInfos);
                if (_stockItemReturnInfoRepository.Save()) {
                    if (flag == StockRetunFlag.AssemblyLine)
                    {
                      return  _assemblyLineStockDetailBusiness.SaveAssemblyLineStockReturn(assemblyLineStocks, userId, orgId, string.Empty);
                    }
                    else if (flag == StockRetunFlag.AssemblyRepair)
                    {
                        return _repairLineStockDetailBusiness.SaveRepairLineStockReturn(repairLineStocks, userId, orgId, string.Empty);
                    }
                    else if (flag == StockRetunFlag.PackagingLine)
                    {
                        return _packagingLineStockDetailBusiness.SavePackagingLineStockReturn(packagingLineStocks, userId, orgId, string.Empty);
                    }
                    else if (flag == StockRetunFlag.PackagingRepair)
                    {
                        return _repairRawStockDetailBusiness.SavePackagingRepairRawStockReturn(packagingRepairRawStocks, userId, orgId);
                    }
                }
            }
            return false;
        }

        public bool UpdateStockItemReturnStatus(long id, string status, long userId, long orgId)
        {
            var stockItem = GetStockItemReturnInfoById(id, orgId);
            if (stockItem != null)
            {
                stockItem.StateStatus = status;
                stockItem.UpUserId = userId;
                stockItem.UpdateDate = DateTime.Now;
                this._stockItemReturnInfoRepository.Update(stockItem);
            }
            return this._stockItemReturnInfoRepository.Save();
        }
    }
}
