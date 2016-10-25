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
        private const int caesarMoveVal = 9;

        /// <summary>
        /// Method for hashing password using MD5 algorithm
        /// </summary>
        /// <param name="password">Hashed password</param>
        /// <returns></returns>
        public string HashPasswordMD5(string password)
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

        /// <summary>
        /// Method for hashing password which can be later unhashed. Do not use it on strings that can have
        /// other ASCII values than ones between 32 and 126.
        /// It is suitable for hashing email adresses, since email adress can't have other values than that.
        /// </summary>
        /// <param name="password">Hashed password</param>
        /// <returns></returns>
        public string HashPasswordCaesar(string password)
        {
            string hashedPassword = "";

            try
            {
                string asciiString = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(password));

                if (asciiString != password)
                {
                    throw new Exception("Password have unsupported chars");
                }
                foreach (var item in Encoding.ASCII.GetBytes(password))
                {

                    if (item < 32 || item > 126)
                    {
                        throw new Exception("Password have unsupported chars");
                    }
                    var newAscii = item + caesarMoveVal;
                    if(newAscii < 32)
                    {
                        newAscii = 126 - (32 - (newAscii + 1));
                    }
                    else if(newAscii > 126)
                    {
                        newAscii = 32 + ((newAscii - 1) - 126);
                    }
                    hashedPassword += Convert.ToChar(newAscii);
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
                hashedPassword = "";
            }
            return hashedPassword;
        }

        /// <summary>
        /// Method for unhashing hashed password using caesar algorithm. For more info, see method HashPasswordCaesar
        /// </summary>
        /// <param name="password">Unhashed password</param>
        /// <returns></returns>
        public string UnhashPasswordCaesar(string password)
        {
            string unhashedPassword = "";

            try
            {
                foreach (var item in Encoding.ASCII.GetBytes(password))
                {
                    var newAscii = item - caesarMoveVal;
                    if (newAscii < 32)
                    {
                        newAscii = 126 - (32 - (newAscii + 1));
                    }
                    else if (newAscii > 126)
                    {
                        newAscii = 32 + ((newAscii - 1) - 126);
                    }
                    unhashedPassword += Convert.ToChar(newAscii);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                unhashedPassword = "";
            }

            return unhashedPassword;
        }
    }
}