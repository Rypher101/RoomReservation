using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace RoomReservation.Models
{
    [Table("t_reservation_room")]
    public partial class TReservationRoom
    {
        [Key]
        [Column("res_id")]
        public int ResId { get; set; }
        [Key]
        [Column("room_id", TypeName = "decimal(4, 0)")]
        public decimal RoomId { get; set; }

        [ForeignKey(nameof(ResId))]
        [InverseProperty(nameof(TReservation.TReservationRooms))]
        public virtual TReservation Res { get; set; }
        [ForeignKey(nameof(RoomId))]
        [InverseProperty(nameof(TRoom.TReservationRooms))]
        public virtual TRoom Room { get; set; }
    }
}
