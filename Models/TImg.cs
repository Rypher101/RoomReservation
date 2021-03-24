using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

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
        [Column("cat_id")]
        [StringLength(10)]
        public string CatId { get; set; }

        [ForeignKey(nameof(CatId))]
        [InverseProperty(nameof(TCategory.TImgs))]
        public virtual TCategory Cat { get; set; }
    }
}
