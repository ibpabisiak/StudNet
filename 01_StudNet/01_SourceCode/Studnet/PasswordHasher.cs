﻿using System;
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

        public string HashPasswordCaesar(string password)
        {
            string hashedPassword = "";

            try
            {
                foreach (var item in Encoding.ASCII.GetBytes(password))
                {
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