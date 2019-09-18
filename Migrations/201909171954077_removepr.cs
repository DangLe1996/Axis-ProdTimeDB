namespace Axis_ProdTimeDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removepr : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProdTBs", "ProdFamTB_ID", "dbo.ProdFamTBs");
            DropIndex("dbo.ProdTBs", new[] { "ProdFamTB_ID" });
            DropColumn("dbo.ProdTBs", "ProdFamTB_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProdTBs", "ProdFamTB_ID", c => c.Int());
            CreateIndex("dbo.ProdTBs", "ProdFamTB_ID");
            AddForeignKey("dbo.ProdTBs", "ProdFamTB_ID", "dbo.ProdFamTBs", "ID");
        }
    }
}
