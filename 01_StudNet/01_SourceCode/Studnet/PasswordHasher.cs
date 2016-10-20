using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Studnet
{
    public class PasswordHasher
    {
        private MD5CryptoServiceProvider hasher = new MD5CryptoServiceProvider();
        private ASCIIEncoding asciiEncoding = new ASCIIEncoding();

        public string HashPassword(string password)
        {
            string hashedPassword = "";

            try
            {
                var passByteArray = Encoding.ASCII.GetBytes(password);
                var hashedPassByteArray = hasher.ComputeHash(passByteArray);
                hashedPassword = asciiEncoding.GetString(hashedPassByteArray);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                hashedPassword = "";
            }

            return hashedPassword;
        }
    }
}