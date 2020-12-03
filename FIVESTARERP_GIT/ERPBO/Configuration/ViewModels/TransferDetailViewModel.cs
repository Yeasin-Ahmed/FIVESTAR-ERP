﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Configuration.ViewModels
{
   public class TransferDetailViewModel
    {
        public long TransferDetailId { get; set; }
        public long? PartsId { get; set; }
        public int Quantity { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        public long? BranchId { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }


        public long TransferInfoId { get; set; }
        [StringLength(100)]
        public string PartsName { get; set; }
        [StringLength(100)]
        public string BranchName { get; set; }

        //
        public long? BranchTo { get; set; }
        public double CostPrice { get; set; }
        public double SellPrice { get; set; }
        //Nishad
        public long? DescriptionId { get; set; }
    }
}
