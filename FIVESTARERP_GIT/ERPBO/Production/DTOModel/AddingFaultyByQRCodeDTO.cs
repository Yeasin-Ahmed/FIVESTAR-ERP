using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DTOModel
{
    public class FaultyInfoByQRCodeDTO
    {
        [Required]
        public string QRCode { get; set; }
        [Range(1,long.MaxValue)]
        public long TransferId { get; set; }
        public string TransferCode { get; set; }
        [Range(1, long.MaxValue)]
        public long ModelId { get; set; }
        public List<FaultyItemsByQRCodeDTO> FaultyItems { get; set; }
        // Optional
        public string IMEI { get; set; }
    }

    public class FaultyItemsByQRCodeDTO
    {
        [Range(1, long.MaxValue)]
        public long ItemId { get; set; }
        [Range(1, long.MaxValue)]
        public long ItemTypeId { get; set; }
        [Range(1, long.MaxValue)]
        public long WarehouseId { get; set; }
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
        public bool IsChinaFaulty { get; set; }
    }
}
