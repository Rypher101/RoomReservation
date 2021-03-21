using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace RoomReservation.Models
{
    [Table("t_room")]
    public partial class TRoom
    {
        public TRoom()
        {
            TRate = new HashSet<TRate>();
            TReservationRoom = new HashSet<TReservationRoom>();
        }

        [Key]
        [Column("room_id", TypeName = "decimal(4, 0)")]
        public decimal RoomId { get; set; }
        [Column("room_floor")]
        public int RoomFloor { get; set; }
        [Column("room_status", TypeName = "decimal(1, 0)")]
        public decimal RoomStatus { get; set; }
        [Column("cat_id")]
        [StringLength(10)]
        public string CatId { get; set; }

        [ForeignKey(nameof(CatId))]
        [InverseProperty(nameof(TCategory.TRoom))]
        public virtual TCategory Cat { get; set; }
        [InverseProperty("Room")]
        public virtual ICollection<TRate> TRate { get; set; }
        [InverseProperty("Room")]
        public virtual ICollection<TReservationRoom> TReservationRoom { get; set; }
    }
}
