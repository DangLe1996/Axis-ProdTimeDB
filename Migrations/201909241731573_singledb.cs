namespace Axis_ProdTimeDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class singledb : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProdTBs", "Type", c => c.String());
            AddColumn("dbo.ProdTBs", "Code", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProdTBs", "Code");
            DropColumn("dbo.ProdTBs", "Type");
        }
    }
}
