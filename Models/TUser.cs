using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace RoomReservation.Models
{
    [Table("t_user")]
    [Index(nameof(UserEmail), Name = "IX_t_user", IsUnique = true)]
    public partial class TUser
    {
        public TUser()
        {
            TRates = new HashSet<TRate>();
            TReservations = new HashSet<TReservation>();
        }

        [Key]
        [Column("user_id")]
        public int UserId { get; set; }
        [Required]
        [Column("user_name")]
        [StringLength(50)]
        public string UserName { get; set; }
        [Required]
        [Column("user_pass")]
        [StringLength(64)]
        public string UserPass { get; set; }
        [Required]
        [Column("user_email")]
        [StringLength(30)]
        public string UserEmail { get; set; }
        [Required]
        [Column("user_address")]
        [StringLength(100)]
        public string UserAddress { get; set; }
        [Column("user_tp", TypeName = "decimal(12, 0)")]
        public decimal UserTp { get; set; }
        [Column("user_status", TypeName = "decimal(1, 0)")]
        public decimal UserStatus { get; set; }
        [Required]
        [Column("user_type")]
        public bool? UserType { get; set; }

        [InverseProperty(nameof(TRate.User))]
        public virtual ICollection<TRate> TRates { get; set; }
        [InverseProperty(nameof(TReservation.User))]
        public virtual ICollection<TReservation> TReservations { get; set; }

        public void ShaEnc()
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(UserPass));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                UserPass = builder.ToString();
            }
        }
    }
}
