namespace Axis_ProdTimeDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProdFam : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProdTBs", "ProductFamily", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProdTBs", "ProductFamily");
        }
    }
}
