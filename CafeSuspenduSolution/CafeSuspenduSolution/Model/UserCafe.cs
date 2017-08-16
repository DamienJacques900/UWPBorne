using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class UserCafe
    {
        [Key]
        [MaxLength(20)]
        [MinLength(6)]
        private string userCafeID;
        public string UserCafeID
        {
            get { return userCafeID; }
            set
            {
                if (value.All(Char.IsLetterOrDigit))
                    userCafeID = value;
            }
        }

        [MaxLength(20)]
        [MinLength(6)]
        [Required]
        private string password;
        public string Password
        {
            get { return password; }

            set
            {
                using (MD5 md5Hash = MD5.Create())
                {
                    string hash = GetMd5Hash(md5Hash, value);
                    password = hash;
                }

            }
        }

        [MaxLength(30)]
        [Required]
        [Index(IsUnique = true)]
        public string CafeName { get; set; }

        [MaxLength(50)]
        [Required]
        public string Street { get; set; }

        [MaxLength(5)]
        [Required]
        public string Number { get; set; }

        [Required]
        public int? NbCoffeeRequiredForPromotion { get; set; }

        [Required]
        public double PromotionValue { get; set; }

        public ICollection<Booking> Bookings { get; set; }
        public ICollection<Charity> Charities { get; set; }
        public ICollection<TimeTable> TimeTables { get; set; }

        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

    }
}