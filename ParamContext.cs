namespace Axis_ProdTimeDB
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ParamContext : DbContext
    {
        public ParamContext()
            : base("name=ParamContext")
        {
        }

        public virtual DbSet<CategoryType> CategoryTypes { get; set; }
        public virtual DbSet<Fixture> Fixtures { get; set; }
        public virtual DbSet<Parameter> Parameters { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
