namespace Axis_ProdTimeDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FixtureSetupCodes.ParameterAtCategoryAtFixture")]
    public partial class ParameterAtCategoryAtFixture
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ParameterAtCategoryAtFixture()
        {
            CategoryAtFixtures = new HashSet<CategoryAtFixture>();
            ParameterReturnValueOverrides = new HashSet<ParameterReturnValueOverride>();
        }

        public int id { get; set; }

        public int CategoryAtFixtureId { get; set; }

        public int ParameterId { get; set; }

        public int DisplayOrder { get; set; }

        [Required]
        [StringLength(250)]
        public string FootnoteOverride { get; set; }

        public bool IsObsolete { get; set; }

        [Required]
        [StringLength(75)]
        public string DescriptionOverride { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CategoryAtFixture> CategoryAtFixtures { get; set; }

        public virtual CategoryAtFixture CategoryAtFixture { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParameterReturnValueOverride> ParameterReturnValueOverrides { get; set; }

        public virtual Parameter Parameter { get; set; }
    }
}
