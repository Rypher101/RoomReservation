using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace RoomReservation.Models
{
    [Table("t_img")]
    public partial class TImg
    {
        [Key]
        [Column("im_id")]
        public int ImId { get; set; }
        [Required]
        [Column("im_path", TypeName = "text")]
        public string ImPath { get; set; }
        [Required]
        [Column("cat_id")]
        [StringLength(10)]
        public string CatId { get; set; }

        [ForeignKey(nameof(CatId))]
        [InverseProperty(nameof(TCategory.TImg))]
        public virtual TCategory Cat { get; set; }
    }
}
