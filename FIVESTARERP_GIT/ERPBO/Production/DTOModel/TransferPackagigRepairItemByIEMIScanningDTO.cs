using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DTOModel
{
    public class TransferPackagigRepairItemByIMEIScanningDTO
    {
        public string QRCode { get; set; }
        public long PackagingLineId { get; set; }
        public long FloorId { get; set; }
        public long ModelId { get; set; }
        public long ItemId { get; set; }
    }
}
