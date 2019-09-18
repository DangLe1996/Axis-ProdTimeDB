namespace Axis_ProdTimeDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addRls : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OptionTBs", "FixtureTB_ID", "dbo.FixtureTBs");
            DropForeignKey("dbo.OptionTBs", "ProdTB_ID", "dbo.ProdTBs");
            DropIndex("dbo.OptionTBs", new[] { "FixtureTB_ID" });
            DropIndex("dbo.OptionTBs", new[] { "ProdTB_ID" });
            CreateTable(
                "dbo.OptionTBFixtureTBs",
                c => new
                    {
                        OptionTB_OptionID = c.Int(nullable: false),
                        FixtureTB_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.OptionTB_OptionID, t.FixtureTB_ID })
                .ForeignKey("dbo.OptionTBs", t => t.OptionTB_OptionID, cascadeDelete: true)
                .ForeignKey("dbo.FixtureTBs", t => t.FixtureTB_ID, cascadeDelete: true)
                .Index(t => t.OptionTB_OptionID)
                .Index(t => t.FixtureTB_ID);
            
            CreateTable(
                "dbo.ProdTBOptionTBs",
                c => new
                    {
                        ProdTB_ID = c.Int(nullable: false),
                        OptionTB_OptionID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProdTB_ID, t.OptionTB_OptionID })
                .ForeignKey("dbo.ProdTBs", t => t.ProdTB_ID, cascadeDelete: true)
                .ForeignKey("dbo.OptionTBs", t => t.OptionTB_OptionID, cascadeDelete: true)
                .Index(t => t.ProdTB_ID)
                .Index(t => t.OptionTB_OptionID);
            
            DropColumn("dbo.OptionTBs", "FixtureTB_ID");
            DropColumn("dbo.OptionTBs", "ProdTB_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OptionTBs", "ProdTB_ID", c => c.Int());
            AddColumn("dbo.OptionTBs", "FixtureTB_ID", c => c.Int());
            DropForeignKey("dbo.ProdTBOptionTBs", "OptionTB_OptionID", "dbo.OptionTBs");
            DropForeignKey("dbo.ProdTBOptionTBs", "ProdTB_ID", "dbo.ProdTBs");
            DropForeignKey("dbo.OptionTBFixtureTBs", "FixtureTB_ID", "dbo.FixtureTBs");
            DropForeignKey("dbo.OptionTBFixtureTBs", "OptionTB_OptionID", "dbo.OptionTBs");
            DropIndex("dbo.ProdTBOptionTBs", new[] { "OptionTB_OptionID" });
            DropIndex("dbo.ProdTBOptionTBs", new[] { "ProdTB_ID" });
            DropIndex("dbo.OptionTBFixtureTBs", new[] { "FixtureTB_ID" });
            DropIndex("dbo.OptionTBFixtureTBs", new[] { "OptionTB_OptionID" });
            DropTable("dbo.ProdTBOptionTBs");
            DropTable("dbo.OptionTBFixtureTBs");
            CreateIndex("dbo.OptionTBs", "ProdTB_ID");
            CreateIndex("dbo.OptionTBs", "FixtureTB_ID");
            AddForeignKey("dbo.OptionTBs", "ProdTB_ID", "dbo.ProdTBs", "ID");
            AddForeignKey("dbo.OptionTBs", "FixtureTB_ID", "dbo.FixtureTBs", "ID");
        }
    }
}
