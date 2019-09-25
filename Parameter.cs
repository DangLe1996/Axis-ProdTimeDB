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
        public int id { get; set; }

        [Required]
        [StringLength(25)]
        public string Code { get; set; }

        [StringLength(75)]
        public string Description { get; set; }

        [Required]
        [StringLength(250)]
        public string Footnote { get; set; }
    }
}
