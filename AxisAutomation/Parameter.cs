namespace Axis_ProdTimeDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FixtureSetupCodes.Parameters")]
    public partial class Parameter
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Parameter()
        {
            ParameterAtCategoryAtFixtures = new HashSet<ParameterAtCategoryAtFixture>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(25)]
        public string Code { get; set; }

        [StringLength(75)]
        public string Description { get; set; }

        [Required]
        [StringLength(250)]
        public string Footnote { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParameterAtCategoryAtFixture> ParameterAtCategoryAtFixtures { get; set; }
    }
}
