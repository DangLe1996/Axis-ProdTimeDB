namespace Axis_ProdTimeDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FixtureSetupCodes.Fixtures")]
    public partial class Fixture
    {
        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        [StringLength(100)]
        public string FamilyName { get; set; }

        public bool IsDWEnabled { get; set; }

        public int ApplicationTypeId { get; set; }

        public int DirectionalityId { get; set; }

        public bool HasSingleEmmiter { get; set; }

        public int DWGouverningProjectId { get; set; }

        public bool IsVerified { get; set; }

        [StringLength(50)]
        public string VerifiedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime? VerifiedDate { get; set; }

        public int? FamilyID { get; set; }
    }
}
