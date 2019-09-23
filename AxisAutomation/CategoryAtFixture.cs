namespace Axis_ProdTimeDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FixtureSetupCodes.CategoryAtFixture")]
    public partial class CategoryAtFixture
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CategoryAtFixture()
        {
            ParameterAtCategoryAtFixtures = new HashSet<ParameterAtCategoryAtFixture>();
        }

        public int id { get; set; }

        public int CategoryId { get; set; }

        public int FixtureId { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsOptionalOverride { get; set; }

        [Required]
        [StringLength(250)]
        public string FootnoteOverride { get; set; }

        public bool IsMultiselectOverride { get; set; }

        public bool IsObsolete { get; set; }

        public int? DefaultFallbackSelection { get; set; }

        public virtual Category Category { get; set; }

        public virtual ParameterAtCategoryAtFixture ParameterAtCategoryAtFixture { get; set; }

        public virtual Fixture Fixture { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParameterAtCategoryAtFixture> ParameterAtCategoryAtFixtures { get; set; }
    }
}
