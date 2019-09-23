namespace Axis_ProdTimeDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FixtureSetupCodes.ParameterReturnValueOverrides")]
    public partial class ParameterReturnValueOverride
    {
        public int id { get; set; }

        public int PAC_Id { get; set; }

        [Required]
        [StringLength(50)]
        public string ValueExtractRegex { get; set; }

        [Required]
        [StringLength(50)]
        public string ValueReAssemblyString { get; set; }

        public virtual ParameterAtCategoryAtFixture ParameterAtCategoryAtFixture { get; set; }
    }
}
