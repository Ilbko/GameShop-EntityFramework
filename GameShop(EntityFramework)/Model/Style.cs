namespace GameShop_EntityFramework_.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Style")]
    public partial class Style
    {
        [Key]
        public int Style_Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Style_Name { get; set; }
    }
}
