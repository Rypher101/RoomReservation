using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace RoomReservation.Models
{
    [Table("t_category")]
    public partial class TCategory
    {
        public TCategory()
        {
            TImgs = new HashSet<TImg>();
            TRooms = new HashSet<TRoom>();
        }

        [Key]
        [Column("cat_id")]
        [StringLength(10)]
        public string CatId { get; set; }
        [Required]
        [Column("cat_type")]
        [StringLength(20)]
        public string CatType { get; set; }
        [Column("cat_bed")]
        public int CatBed { get; set; }
        [Required]
        [Column("cat_description", TypeName = "text")]
        public string CatDescription { get; set; }
        [Column("cat_price", TypeName = "decimal(12, 2)")]
        public decimal CatPrice { get; set; }

        [InverseProperty(nameof(TImg.Cat))]
        public virtual ICollection<TImg> TImgs { get; set; }
        [InverseProperty(nameof(TRoom.Cat))]
        public virtual ICollection<TRoom> TRooms { get; set; }
    }
}
