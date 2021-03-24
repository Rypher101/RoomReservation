using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace RoomReservation.Models
{
    [Table("t_reservation")]
    public partial class TReservation
    {
        public TReservation()
        {
            TReservationRooms = new HashSet<TReservationRoom>();
            TSurveys = new HashSet<TSurvey>();
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
        [InverseProperty(nameof(TUser.TReservations))]
        public virtual TUser User { get; set; }
        [InverseProperty(nameof(TReservationRoom.Res))]
        public virtual ICollection<TReservationRoom> TReservationRooms { get; set; }
        [InverseProperty(nameof(TSurvey.Res))]
        public virtual ICollection<TSurvey> TSurveys { get; set; }
    }
}
