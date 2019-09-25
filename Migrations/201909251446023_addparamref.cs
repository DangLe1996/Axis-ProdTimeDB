namespace Axis_ProdTimeDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addparamref : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "FixtureSetupCodes.Parameters",
            //    c => new
            //        {
            //            id = c.Int(nullable: false, identity: true),
            //            Code = c.String(nullable: false, maxLength: 25),
            //            Description = c.String(maxLength: 75),
            //            Footnote = c.String(nullable: false, maxLength: 250),
            //        })
            //    .PrimaryKey(t => t.id);
            
            AddColumn("dbo.ParametersTBs", "ParameterRef_id", c => c.Int());
            CreateIndex("dbo.ParametersTBs", "ParameterRef_id");
            AddForeignKey("dbo.ParametersTBs", "ParameterRef_id", "FixtureSetupCodes.Parameters", "id");
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.ParametersTBs", "ParameterRef_id", "FixtureSetupCodes.Parameters");
            //DropIndex("dbo.ParametersTBs", new[] { "ParameterRef_id" });
            //DropColumn("dbo.ParametersTBs", "ParameterRef_id");
            //DropTable("FixtureSetupCodes.Parameters");
        }
    }
}
