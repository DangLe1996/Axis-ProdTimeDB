namespace Axis_ProdTimeDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removebadrederege1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("FixtureSetupCodes.CategoryAtFixture", "CategoryId", "FixtureSetupCodes.Categories");
            DropForeignKey("FixtureSetupCodes.Categories", "CategoryType_id", "FixtureSetupCodes.CategoryTypes");
            DropForeignKey("FixtureSetupCodes.CategoryAtFixture", "FixtureId", "FixtureSetupCodes.Fixtures");
            DropForeignKey("FixtureSetupCodes.CategoryAtFixture", "ParameterAtCategoryAtFixture_id", "FixtureSetupCodes.ParameterAtCategoryAtFixture");
            DropForeignKey("FixtureSetupCodes.ParameterAtCategoryAtFixture", "CategoryAtFixture_id", "FixtureSetupCodes.CategoryAtFixture");
            DropForeignKey("FixtureSetupCodes.ParameterAtCategoryAtFixture", "CategoryAtFixtureId", "FixtureSetupCodes.CategoryAtFixture");
            DropForeignKey("FixtureSetupCodes.CategoryAtFixture", "ParameterAtCategoryAtFixture_id1", "FixtureSetupCodes.ParameterAtCategoryAtFixture");
            DropForeignKey("FixtureSetupCodes.ParameterAtCategoryAtFixture", "ParameterId", "FixtureSetupCodes.Parameters");
            DropForeignKey("FixtureSetupCodes.ParameterReturnValueOverrides", "ParameterAtCategoryAtFixture_id", "FixtureSetupCodes.ParameterAtCategoryAtFixture");
            DropForeignKey("dbo.ParametersTBs", "DWParam_id", "FixtureSetupCodes.Parameters");
            DropIndex("dbo.ParametersTBs", new[] { "DWParam_id" });
            DropIndex("FixtureSetupCodes.ParameterAtCategoryAtFixture", new[] { "CategoryAtFixtureId" });
            DropIndex("FixtureSetupCodes.ParameterAtCategoryAtFixture", new[] { "ParameterId" });
            DropIndex("FixtureSetupCodes.ParameterAtCategoryAtFixture", new[] { "CategoryAtFixture_id" });
            DropIndex("FixtureSetupCodes.CategoryAtFixture", new[] { "CategoryId" });
            DropIndex("FixtureSetupCodes.CategoryAtFixture", new[] { "FixtureId" });
            DropIndex("FixtureSetupCodes.CategoryAtFixture", new[] { "ParameterAtCategoryAtFixture_id" });
            DropIndex("FixtureSetupCodes.CategoryAtFixture", new[] { "ParameterAtCategoryAtFixture_id1" });
            DropIndex("FixtureSetupCodes.Categories", new[] { "CategoryType_id" });
            DropIndex("FixtureSetupCodes.ParameterReturnValueOverrides", new[] { "ParameterAtCategoryAtFixture_id" });
            DropColumn("dbo.ParametersTBs", "DWParam_id");
            DropTable("FixtureSetupCodes.Parameters");
            DropTable("FixtureSetupCodes.ParameterAtCategoryAtFixture");
            DropTable("FixtureSetupCodes.CategoryAtFixture");
            DropTable("FixtureSetupCodes.Categories");
            DropTable("FixtureSetupCodes.CategoryTypes");
            DropTable("FixtureSetupCodes.Fixtures");
            DropTable("FixtureSetupCodes.ParameterReturnValueOverrides");
        }
        
        public override void Down()
        {
            CreateTable(
                "FixtureSetupCodes.ParameterReturnValueOverrides",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        PAC_Id = c.Int(nullable: false),
                        ValueExtractRegex = c.String(nullable: false, maxLength: 50),
                        ValueReAssemblyString = c.String(nullable: false, maxLength: 50),
                        ParameterAtCategoryAtFixture_id = c.Int(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "FixtureSetupCodes.Fixtures",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 50),
                        Description = c.String(maxLength: 100),
                        FamilyName = c.String(maxLength: 100),
                        IsDWEnabled = c.Boolean(nullable: false),
                        ApplicationTypeId = c.Int(nullable: false),
                        DirectionalityId = c.Int(nullable: false),
                        HasSingleEmmiter = c.Boolean(nullable: false),
                        DWGouverningProjectId = c.Int(nullable: false),
                        IsVerified = c.Boolean(nullable: false),
                        VerifiedBy = c.String(maxLength: 50),
                        VerifiedDate = c.DateTime(storeType: "date"),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "FixtureSetupCodes.CategoryTypes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 25),
                        Description = c.String(nullable: false, maxLength: 25),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "FixtureSetupCodes.Categories",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 75),
                        TypeID = c.Int(nullable: false),
                        IsMultiselect = c.Boolean(nullable: false),
                        Footnote = c.String(nullable: false, maxLength: 250),
                        IsOptional = c.Boolean(nullable: false),
                        CategoryType_id = c.Int(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "FixtureSetupCodes.CategoryAtFixture",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        CategoryId = c.Int(nullable: false),
                        FixtureId = c.Int(nullable: false),
                        DisplayOrder = c.Int(nullable: false),
                        IsOptionalOverride = c.Boolean(nullable: false),
                        FootnoteOverride = c.String(nullable: false, maxLength: 250),
                        IsMultiselectOverride = c.Boolean(nullable: false),
                        IsObsolete = c.Boolean(nullable: false),
                        DefaultFallbackSelection = c.Int(),
                        ParameterAtCategoryAtFixture_id = c.Int(),
                        ParameterAtCategoryAtFixture_id1 = c.Int(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "FixtureSetupCodes.ParameterAtCategoryAtFixture",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        CategoryAtFixtureId = c.Int(nullable: false),
                        ParameterId = c.Int(nullable: false),
                        DisplayOrder = c.Int(nullable: false),
                        FootnoteOverride = c.String(nullable: false, maxLength: 250),
                        IsObsolete = c.Boolean(nullable: false),
                        DescriptionOverride = c.String(nullable: false, maxLength: 75),
                        CategoryAtFixture_id = c.Int(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "FixtureSetupCodes.Parameters",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 25),
                        Description = c.String(maxLength: 75),
                        Footnote = c.String(nullable: false, maxLength: 250),
                    })
                .PrimaryKey(t => t.id);
            
            AddColumn("dbo.ParametersTBs", "DWParam_id", c => c.Int());
            CreateIndex("FixtureSetupCodes.ParameterReturnValueOverrides", "ParameterAtCategoryAtFixture_id");
            CreateIndex("FixtureSetupCodes.Categories", "CategoryType_id");
            CreateIndex("FixtureSetupCodes.CategoryAtFixture", "ParameterAtCategoryAtFixture_id1");
            CreateIndex("FixtureSetupCodes.CategoryAtFixture", "ParameterAtCategoryAtFixture_id");
            CreateIndex("FixtureSetupCodes.CategoryAtFixture", "FixtureId");
            CreateIndex("FixtureSetupCodes.CategoryAtFixture", "CategoryId");
            CreateIndex("FixtureSetupCodes.ParameterAtCategoryAtFixture", "CategoryAtFixture_id");
            CreateIndex("FixtureSetupCodes.ParameterAtCategoryAtFixture", "ParameterId");
            CreateIndex("FixtureSetupCodes.ParameterAtCategoryAtFixture", "CategoryAtFixtureId");
            CreateIndex("dbo.ParametersTBs", "DWParam_id");
            AddForeignKey("dbo.ParametersTBs", "DWParam_id", "FixtureSetupCodes.Parameters", "id");
            AddForeignKey("FixtureSetupCodes.ParameterReturnValueOverrides", "ParameterAtCategoryAtFixture_id", "FixtureSetupCodes.ParameterAtCategoryAtFixture", "id");
            AddForeignKey("FixtureSetupCodes.ParameterAtCategoryAtFixture", "ParameterId", "FixtureSetupCodes.Parameters", "id", cascadeDelete: true);
            AddForeignKey("FixtureSetupCodes.CategoryAtFixture", "ParameterAtCategoryAtFixture_id1", "FixtureSetupCodes.ParameterAtCategoryAtFixture", "id");
            AddForeignKey("FixtureSetupCodes.ParameterAtCategoryAtFixture", "CategoryAtFixtureId", "FixtureSetupCodes.CategoryAtFixture", "id", cascadeDelete: true);
            AddForeignKey("FixtureSetupCodes.ParameterAtCategoryAtFixture", "CategoryAtFixture_id", "FixtureSetupCodes.CategoryAtFixture", "id");
            AddForeignKey("FixtureSetupCodes.CategoryAtFixture", "ParameterAtCategoryAtFixture_id", "FixtureSetupCodes.ParameterAtCategoryAtFixture", "id");
            AddForeignKey("FixtureSetupCodes.CategoryAtFixture", "FixtureId", "FixtureSetupCodes.Fixtures", "id", cascadeDelete: true);
            AddForeignKey("FixtureSetupCodes.Categories", "CategoryType_id", "FixtureSetupCodes.CategoryTypes", "id");
            AddForeignKey("FixtureSetupCodes.CategoryAtFixture", "CategoryId", "FixtureSetupCodes.Categories", "id", cascadeDelete: true);
        }
    }
}
