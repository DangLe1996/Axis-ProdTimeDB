namespace Axis_ProdTimeDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProdFamTB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProdFamTBs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FamCode = c.String(),
                        OpCode = c.String(),
                        OpDesc = c.String(),
                        WorkCenter = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ProdFamTBOptionTBs",
                c => new
                    {
                        ProdFamTB_ID = c.Int(nullable: false),
                        OptionTB_OptionID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProdFamTB_ID, t.OptionTB_OptionID })
                .ForeignKey("dbo.ProdFamTBs", t => t.ProdFamTB_ID, cascadeDelete: true)
                .ForeignKey("dbo.OptionTBs", t => t.OptionTB_OptionID, cascadeDelete: true)
                .Index(t => t.ProdFamTB_ID)
                .Index(t => t.OptionTB_OptionID);
            
            AddColumn("dbo.ProdTBs", "ProdFamTB_ID", c => c.Int());
            CreateIndex("dbo.ProdTBs", "ProdFamTB_ID");
            AddForeignKey("dbo.ProdTBs", "ProdFamTB_ID", "dbo.ProdFamTBs", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProdTBs", "ProdFamTB_ID", "dbo.ProdFamTBs");
            DropForeignKey("dbo.ProdFamTBOptionTBs", "OptionTB_OptionID", "dbo.OptionTBs");
            DropForeignKey("dbo.ProdFamTBOptionTBs", "ProdFamTB_ID", "dbo.ProdFamTBs");
            DropIndex("dbo.ProdFamTBOptionTBs", new[] { "OptionTB_OptionID" });
            DropIndex("dbo.ProdFamTBOptionTBs", new[] { "ProdFamTB_ID" });
            DropIndex("dbo.ProdTBs", new[] { "ProdFamTB_ID" });
            DropColumn("dbo.ProdTBs", "ProdFamTB_ID");
            DropTable("dbo.ProdFamTBOptionTBs");
            DropTable("dbo.ProdFamTBs");
        }
    }
}
