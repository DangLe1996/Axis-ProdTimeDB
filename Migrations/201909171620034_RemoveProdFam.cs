namespace Axis_ProdTimeDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveProdFam : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ProdTBs", "ProductFamily");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProdTBs", "ProductFamily", c => c.String());
        }
    }
}
