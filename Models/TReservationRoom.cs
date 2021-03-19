using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

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
        [InverseProperty(nameof(TReservation.TReservationRoom))]
        public virtual TReservation Res { get; set; }
        [ForeignKey(nameof(RoomId))]
        [InverseProperty(nameof(TRoom.TReservationRoom))]
        public virtual TRoom Room { get; set; }
    }
}
