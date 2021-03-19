using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace RoomReservation.Models
{
    [Table("t_reservation")]
    public partial class TReservation
    {
        public TReservation()
        {
            TReservationRoom = new HashSet<TReservationRoom>();
        }

        [Key]
        [Column("res_id")]
        public int ResId { get; set; }
        [Column("res_date", TypeName = "date")]
        public DateTime ResDate { get; set; }
        [Column("res_from", TypeName = "date")]
        public DateTime ResFrom { get; set; }
        [Column("res_to", TypeName = "date")]
        public DateTime ResTo { get; set; }
        [Column("res_status", TypeName = "decimal(1, 0)")]
        public decimal ResStatus { get; set; }
        [Column("user_id")]
        public int? UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(TUser.TReservation))]
        public virtual TUser User { get; set; }
        [InverseProperty("Res")]
        public virtual ICollection<TReservationRoom> TReservationRoom { get; set; }
    }
}
