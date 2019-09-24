namespace Axis_ProdTimeDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class singledbupdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OptionTBFixtureTBs", "OptionTB_OptionID", "dbo.OptionTBs");
            DropForeignKey("dbo.OptionTBFixtureTBs", "FixtureTB_ID", "dbo.FixtureTBs");
            DropForeignKey("dbo.ProdFamTBOptionTBs", "ProdFamTB_ID", "dbo.ProdFamTBs");
            DropForeignKey("dbo.ProdFamTBOptionTBs", "OptionTB_OptionID", "dbo.OptionTBs");
            DropIndex("dbo.OptionTBFixtureTBs", new[] { "OptionTB_OptionID" });
            DropIndex("dbo.OptionTBFixtureTBs", new[] { "FixtureTB_ID" });
            DropIndex("dbo.ProdFamTBOptionTBs", new[] { "ProdFamTB_ID" });
            DropIndex("dbo.ProdFamTBOptionTBs", new[] { "OptionTB_OptionID" });
            DropTable("dbo.FixtureTBs");
            DropTable("dbo.ProdFamTBs");
            DropTable("dbo.OptionTBFixtureTBs");
            DropTable("dbo.ProdFamTBOptionTBs");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ProdFamTBOptionTBs",
                c => new
                    {
                        ProdFamTB_ID = c.Int(nullable: false),
                        OptionTB_OptionID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProdFamTB_ID, t.OptionTB_OptionID });
            
            CreateTable(
                "dbo.OptionTBFixtureTBs",
                c => new
                    {
                        OptionTB_OptionID = c.Int(nullable: false),
                        FixtureTB_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.OptionTB_OptionID, t.FixtureTB_ID });
            
            CreateTable(
                "dbo.ProdFamTBs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FamCode = c.String(),
                        OpCode = c.String(),
                        WorkCenter = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.FixtureTBs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FxCode = c.String(),
                        OpCode = c.String(),
                        WorkCenter = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateIndex("dbo.ProdFamTBOptionTBs", "OptionTB_OptionID");
            CreateIndex("dbo.ProdFamTBOptionTBs", "ProdFamTB_ID");
            CreateIndex("dbo.OptionTBFixtureTBs", "FixtureTB_ID");
            CreateIndex("dbo.OptionTBFixtureTBs", "OptionTB_OptionID");
            AddForeignKey("dbo.ProdFamTBOptionTBs", "OptionTB_OptionID", "dbo.OptionTBs", "OptionID", cascadeDelete: true);
            AddForeignKey("dbo.ProdFamTBOptionTBs", "ProdFamTB_ID", "dbo.ProdFamTBs", "ID", cascadeDelete: true);
            AddForeignKey("dbo.OptionTBFixtureTBs", "FixtureTB_ID", "dbo.FixtureTBs", "ID", cascadeDelete: true);
            AddForeignKey("dbo.OptionTBFixtureTBs", "OptionTB_OptionID", "dbo.OptionTBs", "OptionID", cascadeDelete: true);
        }
    }
}
