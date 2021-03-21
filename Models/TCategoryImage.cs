using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace RoomReservation.Models
{
    [Table("t_category_image")]
    public partial class TCategoryImage
    {
        [Key]
        [Column("cat_id")]
        [StringLength(10)]
        public string CatId { get; set; }
        [Key]
        [Column("img_id")]
        public int ImgId { get; set; }

        [ForeignKey(nameof(CatId))]
        [InverseProperty(nameof(TCategory.TCategoryImage))]
        public virtual TCategory Cat { get; set; }
        [ForeignKey(nameof(ImgId))]
        [InverseProperty(nameof(TImg.TCategoryImage))]
        public virtual TImg Img { get; set; }
    }
}
