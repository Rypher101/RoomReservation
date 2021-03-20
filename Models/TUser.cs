using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace RoomReservation.Models
{
    [Table("t_user")]
    public partial class TUser
    {
        public TUser()
        {
            TRate = new HashSet<TRate>();
            TReservation = new HashSet<TReservation>();
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

        [InverseProperty("User")]
        public virtual ICollection<TRate> TRate { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<TReservation> TReservation { get; set; }

        public void ShaEnc()
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(UserPass));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                UserPass= builder.ToString();
            }
        }
    }
}
