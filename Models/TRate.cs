using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

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
        [InverseProperty(nameof(TRoom.TRates))]
        public virtual TRoom Room { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(TUser.TRates))]
        public virtual TUser User { get; set; }
    }
}
