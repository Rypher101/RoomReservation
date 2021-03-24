using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace RoomReservation.Models
{
    [Table("t_room")]
    public partial class TRoom
    {
        public TRoom()
        {
            TRates = new HashSet<TRate>();
            TReservationRooms = new HashSet<TReservationRoom>();
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
        [InverseProperty(nameof(TCategory.TRooms))]
        public virtual TCategory Cat { get; set; }
        [InverseProperty(nameof(TRate.Room))]
        public virtual ICollection<TRate> TRates { get; set; }
        [InverseProperty(nameof(TReservationRoom.Room))]
        public virtual ICollection<TReservationRoom> TReservationRooms { get; set; }
    }
}
