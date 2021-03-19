using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace RoomReservation.Models
{
    [Table("t_rate")]
    public partial class TRate
    {
        [Key]
        [Column("user_id")]
        public int UserId { get; set; }
        [Key]
        [Column("room_id", TypeName = "decimal(4, 0)")]
        public decimal RoomId { get; set; }
        [Column("rate", TypeName = "decimal(1, 0)")]
        public decimal Rate { get; set; }
        [Column("review", TypeName = "text")]
        public string Review { get; set; }

        [ForeignKey(nameof(RoomId))]
        [InverseProperty(nameof(TRoom.TRate))]
        public virtual TRoom Room { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(TUser.TRate))]
        public virtual TUser User { get; set; }
    }
}
