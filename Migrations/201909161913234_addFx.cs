namespace Axis_ProdTimeDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addFx : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FixtureTBs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FxCode = c.String(),
                        OpCode = c.String(),
                        OpDesc = c.String(),
                        WorkCenter = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.OptionTBs", "FixtureTB_ID", c => c.Int());
            CreateIndex("dbo.OptionTBs", "FixtureTB_ID");
            AddForeignKey("dbo.OptionTBs", "FixtureTB_ID", "dbo.FixtureTBs", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OptionTBs", "FixtureTB_ID", "dbo.FixtureTBs");
            DropIndex("dbo.OptionTBs", new[] { "FixtureTB_ID" });
            DropColumn("dbo.OptionTBs", "FixtureTB_ID");
            DropTable("dbo.FixtureTBs");
        }
    }
}
