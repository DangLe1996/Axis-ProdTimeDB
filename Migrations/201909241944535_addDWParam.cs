namespace Axis_ProdTimeDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDWParam : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ParametersTBs", "DWParam_id", c => c.Int());
            CreateIndex("dbo.ParametersTBs", "DWParam_id");
            AddForeignKey("dbo.ParametersTBs", "DWParam_id", "FixtureSetupCodes.Parameters", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ParametersTBs", "DWParam_id", "FixtureSetupCodes.Parameters");
            DropIndex("dbo.ParametersTBs", new[] { "DWParam_id" });
            DropColumn("dbo.ParametersTBs", "DWParam_id");
        }
    }
}
