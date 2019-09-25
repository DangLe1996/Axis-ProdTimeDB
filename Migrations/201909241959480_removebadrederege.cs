namespace Axis_ProdTimeDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removebadrederege : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ParametersTBs", "parameter_id", "FixtureSetupCodes.Parameters");
            DropIndex("dbo.ParametersTBs", new[] { "parameter_id" });
            DropColumn("dbo.ParametersTBs", "parameter_id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ParametersTBs", "parameter_id", c => c.Int());
            CreateIndex("dbo.ParametersTBs", "parameter_id");
            AddForeignKey("dbo.ParametersTBs", "parameter_id", "FixtureSetupCodes.Parameters", "id");
        }
    }
}
