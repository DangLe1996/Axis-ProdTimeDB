namespace Axis_ProdTimeDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class singledbupdate1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ProdTBs", "ProdCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProdTBs", "ProdCode", c => c.String());
        }
    }
}
