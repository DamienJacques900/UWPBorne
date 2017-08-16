using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Model
{
    public class UserPerson
    {
        [Key]
        [MaxLength(20)]
        [MinLength(6)] // chiffres et lettres only ?
        private string userPersonID;
        public string UserPersonID
        {
            get { return userPersonID; }
            set
            {
                if(value.All(Char.IsLetterOrDigit))
                    userPersonID = value; 
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
        private string  firstName;
        public string FirstName
        {
            get { return firstName; }
            set
            {
                if (value.All(Char.IsLetter))
                    firstName = value;
            }
        }

        [MaxLength(30)]
        [Required]
        private string lastName;
        public string LastName
        {
            get { return lastName; }
            set
            {
                if (value.All(Char.IsLetter))
                    lastName = value;
            }
        }

        [MaxLength(50)]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [MaxLength(12)]
        [Required]
        public string PhoneNumber { get; set; }

        public ICollection<Charity> Charities { get; set; }

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