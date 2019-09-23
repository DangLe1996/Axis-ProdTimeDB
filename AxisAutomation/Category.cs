namespace Axis_ProdTimeDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FixtureSetupCodes.Categories")]
    public partial class Category
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Category()
        {
            CategoryAtFixtures = new HashSet<CategoryAtFixture>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(75)]
        public string Name { get; set; }

        public int TypeID { get; set; }

        public bool IsMultiselect { get; set; }

        [Required]
        [StringLength(250)]
        public string Footnote { get; set; }

        public bool IsOptional { get; set; }

        public virtual CategoryType CategoryType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CategoryAtFixture> CategoryAtFixtures { get; set; }
    }
}
