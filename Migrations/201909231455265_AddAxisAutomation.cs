namespace Axis_ProdTimeDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAxisAutomation : DbMigration
    {
        public override void Up()
        {
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
                .PrimaryKey(t => t.id)
                .ForeignKey("FixtureSetupCodes.CategoryAtFixture", t => t.CategoryAtFixture_id)
                .ForeignKey("FixtureSetupCodes.CategoryAtFixture", t => t.CategoryAtFixtureId, cascadeDelete: true)
                .ForeignKey("FixtureSetupCodes.Parameters", t => t.ParameterId, cascadeDelete: true)
                .Index(t => t.CategoryAtFixtureId)
                .Index(t => t.ParameterId)
                .Index(t => t.CategoryAtFixture_id);
            
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
                .PrimaryKey(t => t.id)
                .ForeignKey("FixtureSetupCodes.Categories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("FixtureSetupCodes.Fixtures", t => t.FixtureId, cascadeDelete: true)
                .ForeignKey("FixtureSetupCodes.ParameterAtCategoryAtFixture", t => t.ParameterAtCategoryAtFixture_id)
                .ForeignKey("FixtureSetupCodes.ParameterAtCategoryAtFixture", t => t.ParameterAtCategoryAtFixture_id1)
                .Index(t => t.CategoryId)
                .Index(t => t.FixtureId)
                .Index(t => t.ParameterAtCategoryAtFixture_id)
                .Index(t => t.ParameterAtCategoryAtFixture_id1);
            
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
                .PrimaryKey(t => t.id)
                .ForeignKey("FixtureSetupCodes.CategoryTypes", t => t.CategoryType_id)
                .Index(t => t.CategoryType_id);
            
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
                "FixtureSetupCodes.ParameterReturnValueOverrides",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        PAC_Id = c.Int(nullable: false),
                        ValueExtractRegex = c.String(nullable: false, maxLength: 50),
                        ValueReAssemblyString = c.String(nullable: false, maxLength: 50),
                        ParameterAtCategoryAtFixture_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("FixtureSetupCodes.ParameterAtCategoryAtFixture", t => t.ParameterAtCategoryAtFixture_id)
                .Index(t => t.ParameterAtCategoryAtFixture_id);
            
            AddColumn("dbo.ParametersTBs", "parameter_id", c => c.Int());
            CreateIndex("dbo.ParametersTBs", "parameter_id");
            AddForeignKey("dbo.ParametersTBs", "parameter_id", "FixtureSetupCodes.Parameters", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ParametersTBs", "parameter_id", "FixtureSetupCodes.Parameters");
            DropForeignKey("FixtureSetupCodes.ParameterReturnValueOverrides", "ParameterAtCategoryAtFixture_id", "FixtureSetupCodes.ParameterAtCategoryAtFixture");
            DropForeignKey("FixtureSetupCodes.ParameterAtCategoryAtFixture", "ParameterId", "FixtureSetupCodes.Parameters");
            DropForeignKey("FixtureSetupCodes.CategoryAtFixture", "ParameterAtCategoryAtFixture_id1", "FixtureSetupCodes.ParameterAtCategoryAtFixture");
            DropForeignKey("FixtureSetupCodes.ParameterAtCategoryAtFixture", "CategoryAtFixtureId", "FixtureSetupCodes.CategoryAtFixture");
            DropForeignKey("FixtureSetupCodes.ParameterAtCategoryAtFixture", "CategoryAtFixture_id", "FixtureSetupCodes.CategoryAtFixture");
            DropForeignKey("FixtureSetupCodes.CategoryAtFixture", "ParameterAtCategoryAtFixture_id", "FixtureSetupCodes.ParameterAtCategoryAtFixture");
            DropForeignKey("FixtureSetupCodes.CategoryAtFixture", "FixtureId", "FixtureSetupCodes.Fixtures");
            DropForeignKey("FixtureSetupCodes.Categories", "CategoryType_id", "FixtureSetupCodes.CategoryTypes");
            DropForeignKey("FixtureSetupCodes.CategoryAtFixture", "CategoryId", "FixtureSetupCodes.Categories");
            DropIndex("FixtureSetupCodes.ParameterReturnValueOverrides", new[] { "ParameterAtCategoryAtFixture_id" });
            DropIndex("FixtureSetupCodes.Categories", new[] { "CategoryType_id" });
            DropIndex("FixtureSetupCodes.CategoryAtFixture", new[] { "ParameterAtCategoryAtFixture_id1" });
            DropIndex("FixtureSetupCodes.CategoryAtFixture", new[] { "ParameterAtCategoryAtFixture_id" });
            DropIndex("FixtureSetupCodes.CategoryAtFixture", new[] { "FixtureId" });
            DropIndex("FixtureSetupCodes.CategoryAtFixture", new[] { "CategoryId" });
            DropIndex("FixtureSetupCodes.ParameterAtCategoryAtFixture", new[] { "CategoryAtFixture_id" });
            DropIndex("FixtureSetupCodes.ParameterAtCategoryAtFixture", new[] { "ParameterId" });
            DropIndex("FixtureSetupCodes.ParameterAtCategoryAtFixture", new[] { "CategoryAtFixtureId" });
            DropIndex("dbo.ParametersTBs", new[] { "parameter_id" });
            DropColumn("dbo.ParametersTBs", "parameter_id");
            DropTable("FixtureSetupCodes.ParameterReturnValueOverrides");
            DropTable("FixtureSetupCodes.Fixtures");
            DropTable("FixtureSetupCodes.CategoryTypes");
            DropTable("FixtureSetupCodes.Categories");
            DropTable("FixtureSetupCodes.CategoryAtFixture");
            DropTable("FixtureSetupCodes.ParameterAtCategoryAtFixture");
            DropTable("FixtureSetupCodes.Parameters");
        }
    }
}
