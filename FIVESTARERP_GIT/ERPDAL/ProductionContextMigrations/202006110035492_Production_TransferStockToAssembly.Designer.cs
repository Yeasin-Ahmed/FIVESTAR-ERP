// <auto-generated />
namespace ERPDAL.ProductionContextMigrations
{
    using System.CodeDom.Compiler;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Resources;
    
    [GeneratedCode("EntityFramework.Migrations", "6.2.0-61023")]
    public sealed partial class Production_TransferStockToAssembly : IMigrationMetadata
    {
        private readonly ResourceManager Resources = new ResourceManager(typeof(Production_TransferStockToAssembly));
        
        string IMigrationMetadata.Id
        {
            get { return "202006110035492_Production_TransferStockToAssembly"; }
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