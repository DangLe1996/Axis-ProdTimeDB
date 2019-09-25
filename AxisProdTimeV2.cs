namespace Axis_ProdTimeDB
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class AxisProdTimeV2 : DbContext
    {
        public AxisProdTimeV2()
            : base("name=AxisProdTimeV2")
        {
        }

        public virtual DbSet<OptionTB> OptionTBs { get; set; }
        public virtual DbSet<ParametersTB> ParametersTBs { get; set; }
        public virtual DbSet<ProdTB> ProdTBs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OptionTB>()
                .HasMany(e => e.ParametersTBs)
                .WithMany(e => e.OptionTBs)
                .Map(m => m.ToTable("ParametersTBOptionTBs"));

            modelBuilder.Entity<OptionTB>()
                .HasMany(e => e.ProdTBs)
                .WithMany(e => e.OptionTBs)
                .Map(m => m.ToTable("ProdTBOptionTBs"));
        }
    }
}
