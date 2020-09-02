using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.ViewModels
{
    public class DashBoardAssemblyProgressViewModel
    {
        public long ProductionFloorId { get; set; }
        public string ProductionFloorName { get; set; }
        public long AssemblyLineId { get; set; }
        public string AssemblyLineName { get; set; }
        public int TargetQuantity { get; set; }
        public int CompleteQuantity { get; set; }
        public int RepairIn { get; set; }
        public int RepairOut { get; set; }
        public List<DashBoardAssemblyFaultyViewModel> AssemblyFaultys { get; set; }
        public List<DashBoardAssemblyProblemViewModel> AssemblyProblems { get; set; }
    }
    public class DashBoardAssemblyFaultyViewModel
    {
        public long AssemblyLineId { get; set; }
        public long RepairLineId { get; set; }
        public long QCLineId { get; set; }
        public long ItemTypeId { get; set; }
        public string ItemTypeName { get; set; }
        public long ItemId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
    }
    public class DashBoardAssemblyProblemViewModel
    {
        public long ProductionFloorId { get; set; }
        public string ProductionFloorName { get; set; }
        public long AssemblyLineId { get; set; }
        public string AssemblyLineName { get; set; }
        public long ProblemId { get; set; }
        public string ProblemDescription { get; set; }
        public int Count { get; set; }
    }
}
