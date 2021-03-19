using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace RoomReservation.Models
{
    [Table("t_category")]
    public partial class TCategory
    {
        public TCategory()
        {
            TCategoryImage = new HashSet<TCategoryImage>();
            TRoom = new HashSet<TRoom>();
        }

        [Key]
        [Column("cat_id")]
        [StringLength(4)]
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

        [InverseProperty("Cat")]
        public virtual ICollection<TCategoryImage> TCategoryImage { get; set; }
        [InverseProperty("Cat")]
        public virtual ICollection<TRoom> TRoom { get; set; }
    }
}
