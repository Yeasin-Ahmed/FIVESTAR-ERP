using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DTOModel
{
    public class QRCodeTransferToRepairInfoDTO
    {
        public long QRTRInfoId { get; set; }
        public long FloorId { get; set; }
        public long QCLineId { get; set; }
        public long RepairLineId { get; set; }
        [StringLength(100)]
        public string QRCode { get; set; }
        public long AssemblyLineId { get; set; }
        public long DescriptionId { get; set; }
        public long? WarehouseId { get; set; }
        public long? ItemTypeId { get; set; }
        public long? ItemId { get; set; }
        [StringLength(100)]
        public string StateStatus { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public ICollection<QRCodeProblemDTO> QRCodeProblems { get; set; }
    }
}
