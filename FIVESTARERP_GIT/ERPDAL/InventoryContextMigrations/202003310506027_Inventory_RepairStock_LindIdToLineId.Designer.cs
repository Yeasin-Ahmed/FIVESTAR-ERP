// <auto-generated />
namespace ERPDAL.InventoryContextMigrations
{
    using System.CodeDom.Compiler;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Resources;
    
    [GeneratedCode("EntityFramework.Migrations", "6.2.0-61023")]
    public sealed partial class Inventory_RepairStock_LindIdToLineId : IMigrationMetadata
    {
        private readonly ResourceManager Resources = new ResourceManager(typeof(Inventory_RepairStock_LindIdToLineId));
        
        string IMigrationMetadata.Id
        {
            get { return "202003310506027_Inventory_RepairStock_LindIdToLineId"; }
        }
        
        string IMigrationMetadata.Source
        {
            get { return null; }
        }
        
        string IMigrationMetadata.Target
        {
            get { return Resources.GetString("Target"); }
        }
    }
}
