namespace Axis_ProdTimeDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeopdesc : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.FixtureTBs", "OpDesc");
            DropColumn("dbo.ProdFamTBs", "OpDesc");
            DropColumn("dbo.ProdTBs", "OpDesc");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProdTBs", "OpDesc", c => c.String());
            AddColumn("dbo.ProdFamTBs", "OpDesc", c => c.String());
            AddColumn("dbo.FixtureTBs", "OpDesc", c => c.String());
        }
    }
}
