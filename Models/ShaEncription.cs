using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RoomReservation.Models
{
    public class ShaEncription
    {
        //public void ShaEnc(string value)
        //{
        //    using (SHA256 sha256Hash = SHA256.Create())
        //    {
        //        byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(UserPass));
        //        StringBuilder builder = new StringBuilder();
        //        for (int i = 0; i < bytes.Length; i++)
        //        {
        //            builder.Append(bytes[i].ToString("x2"));
        //        }
        //        UserPass = builder.ToString();
        //    }
        //}
        //if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["MainDB"].ConnectionString);
        //    }

        //Scaffold-DbContext "Data Source=RUSHI\SQLEXPRESS;Initial Catalog=RoomReservation;Integrated Security=True" Microsoft.EntityFrameWorkCore.SqlServer -outputdir Models -contextdir Data -DataAnnotations -Force
    }
}
