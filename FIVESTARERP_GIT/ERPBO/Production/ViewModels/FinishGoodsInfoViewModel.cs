﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.ViewModels
{
    public class FinishGoodsInfoViewModel
    {
        public long FinishGoodsInfoId { get; set; }
        [Range(1,long.MaxValue)]
        public long ProductionLineId { get; set; }
        [Range(1, long.MaxValue)]
        public long PackagingLineId { get; set; }
        [Range(1, long.MaxValue)]
        public long DescriptionId { get; set; }
        [Range(1, long.MaxValue)]
        public long WarehouseId { get; set; }
        public long ItemTypeId { get; set; }
        [Range(1, long.MaxValue)]
        public long ItemId { get; set; }        
        public long UnitId { get; set; }
        public int Quanity { get; set; }
        public string ProductionType { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        //Custom Property
        public string LineNumber { get; set; }
        public string PackagingLineName { get; set; }
        public string ModelName { get; set; }
        public string WarehouseName { get; set; }
        public string ItemTypeName { get; set; }
        public string ItemName { get; set; }
        public string UnitName { get; set; }
    }
}
