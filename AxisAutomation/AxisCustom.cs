namespace Axis_ProdTimeDB
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class AxisCustom : DbContext
    {
        public AxisCustom()
            : base("name=AxisCustom")
        {
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<CategoryAtFixture> CategoryAtFixtures { get; set; }
        public virtual DbSet<CategoryType> CategoryTypes { get; set; }
        public virtual DbSet<Fixture> Fixtures { get; set; }
        public virtual DbSet<ParameterAtCategoryAtFixture> ParameterAtCategoryAtFixtures { get; set; }
        public virtual DbSet<ParameterCustomValueType> ParameterCustomValueTypes { get; set; }
        public virtual DbSet<ParameterDescriptionOverrides___OLD> ParameterDescriptionOverrides___OLD { get; set; }
        public virtual DbSet<ParameterReturnValueOverride> ParameterReturnValueOverrides { get; set; }
        public virtual DbSet<Parameter> Parameters { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryAtFixture>()
                .HasMany(e => e.ParameterAtCategoryAtFixtures)
                .WithRequired(e => e.CategoryAtFixture)
                .HasForeignKey(e => e.CategoryAtFixtureId);

            modelBuilder.Entity<CategoryType>()
                .HasMany(e => e.Categories)
                .WithRequired(e => e.CategoryType)
                .HasForeignKey(e => e.TypeID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ParameterAtCategoryAtFixture>()
                .HasMany(e => e.CategoryAtFixtures)
                .WithOptional(e => e.ParameterAtCategoryAtFixture)
                .HasForeignKey(e => e.DefaultFallbackSelection);

            modelBuilder.Entity<ParameterAtCategoryAtFixture>()
                .HasMany(e => e.ParameterReturnValueOverrides)
                .WithRequired(e => e.ParameterAtCategoryAtFixture)
                .HasForeignKey(e => e.PAC_Id)
                .WillCascadeOnDelete(false);
        }
    }
}
