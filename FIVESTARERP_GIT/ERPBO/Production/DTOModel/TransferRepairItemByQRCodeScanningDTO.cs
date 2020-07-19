using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DTOModel
{
    public class TransferRepairItemByQRCodeScanningDTO
    {
        [Required,StringLength(150)]
        public string QRCode { get; set; }
        public long RepairLineId { get; set; }
        public long QCLineId { get; set; }
        public long ModelId { get; set; }
        public long ItemId { get; set; }
    }
}
