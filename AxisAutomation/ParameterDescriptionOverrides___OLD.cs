namespace Axis_ProdTimeDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FixtureSetupCodes.ParameterDescriptionOverrides___OLD")]
    public partial class ParameterDescriptionOverrides___OLD
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PACAFID { get; set; }

        [Required]
        [StringLength(250)]
        public string DescriptionOverride { get; set; }
    }
}
