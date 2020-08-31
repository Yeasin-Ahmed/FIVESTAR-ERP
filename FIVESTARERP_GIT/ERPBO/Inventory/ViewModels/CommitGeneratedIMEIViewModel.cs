using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Inventory.ViewModels
{
    public class CommitGeneratedIMEIViewModel
    {
        public string DescriptionName { get; set; }
        public long DescriptionId { get; set; }
        public long TAC { get; set; }
        public long Serial { get; set; }
        public List<string> IMEIs { get; set; }
    }
}
