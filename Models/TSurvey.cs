using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace RoomReservation.Models
{
    [Table("t_survey")]
    public partial class TSurvey
    {
        [Key]
        [Column("sur_id")]
        public int SurId { get; set; }
        [Column("sur_room")]
        public int SurRoom { get; set; }
        [Column("sur_room_service")]
        public int SurRoomService { get; set; }
        [Column("sur_service")]
        public int SurService { get; set; }
        [Column("sur_price")]
        public int SurPrice { get; set; }
        [Column("sur_food")]
        public int SurFood { get; set; }
        [Column("res_id")]
        public int ResId { get; set; }

        [ForeignKey(nameof(ResId))]
        [InverseProperty(nameof(TReservation.TSurveys))]
        public virtual TReservation Res { get; set; }
    }
}
