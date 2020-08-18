using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.ViewModels
{
    public class TransferPackagigRepairItemByIMEIScanningViewModel
    {
        [Required, StringLength(150)]
        public string QRCode { get; set; }
        [Range(1, long.MaxValue)]
        public long PackagingLineId { get; set; }
        [Range(1, long.MaxValue)]
        public long FloorId { get; set; }
        [Range(1, long.MaxValue)]
        public long ModelId { get; set; }
        [Range(1, long.MaxValue)]
        public long ItemId { get; set; }
    }

}
