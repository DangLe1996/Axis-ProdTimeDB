namespace Axis_ProdTimeDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OptionTBs",
                c => new
                    {
                        OptionID = c.Int(nullable: false, identity: true),
                        OptionName = c.String(),
                        sectionLength = c.Int(),
                        ProdTime = c.Double(nullable: false),
                        ProdTB_ID = c.Int(),
                    })
                .PrimaryKey(t => t.OptionID)
                .ForeignKey("dbo.ProdTBs", t => t.ProdTB_ID)
                .Index(t => t.ProdTB_ID);
            
            CreateTable(
                "dbo.ParametersTBs",
                c => new
                    {
                        ParamsID = c.Int(nullable: false, identity: true),
                        ParamName = c.String(),
                        ParamValue = c.String(),
                    })
                .PrimaryKey(t => t.ParamsID);
            
            CreateTable(
                "dbo.ProdTBs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProdCode = c.String(),
                        OpCode = c.String(),
                        OpDesc = c.String(),
                        WorkCenter = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ParametersTBOptionTBs",
                c => new
                    {
                        ParametersTB_ParamsID = c.Int(nullable: false),
                        OptionTB_OptionID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ParametersTB_ParamsID, t.OptionTB_OptionID })
                .ForeignKey("dbo.ParametersTBs", t => t.ParametersTB_ParamsID, cascadeDelete: true)
                .ForeignKey("dbo.OptionTBs", t => t.OptionTB_OptionID, cascadeDelete: true)
                .Index(t => t.ParametersTB_ParamsID)
                .Index(t => t.OptionTB_OptionID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OptionTBs", "ProdTB_ID", "dbo.ProdTBs");
            DropForeignKey("dbo.ParametersTBOptionTBs", "OptionTB_OptionID", "dbo.OptionTBs");
            DropForeignKey("dbo.ParametersTBOptionTBs", "ParametersTB_ParamsID", "dbo.ParametersTBs");
            DropIndex("dbo.ParametersTBOptionTBs", new[] { "OptionTB_OptionID" });
            DropIndex("dbo.ParametersTBOptionTBs", new[] { "ParametersTB_ParamsID" });
            DropIndex("dbo.OptionTBs", new[] { "ProdTB_ID" });
            DropTable("dbo.ParametersTBOptionTBs");
            DropTable("dbo.ProdTBs");
            DropTable("dbo.ParametersTBs");
            DropTable("dbo.OptionTBs");
        }
    }
}
