using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Studnet.Tests
{
    [TestClass]
    public class PasswordHasherTest
    {
        [TestMethod]
        public void PasswordHasherMD5ShouldReturnEmptyValueWhenPasswordIsNull()
        {
            string password = null;
            string result = new PasswordHasher().HashPasswordMD5(password);
            Assert.AreEqual("", result);
        }

        [TestMethod]
        public void PasswordHasherCaesarShouldReturnEmptyValueWhenPasswordIsNull()
        {
            string password = null;
            string result = new PasswordHasher().HashPasswordCaesar(password);
            Assert.AreEqual("", result);
        }

        [TestMethod]
        public void PasswordHasherMD5ShouldReturnHasherPassword()
        {
            string password = "kiełbasa";
            string result = new PasswordHasher().HashPasswordMD5(password);
            Assert.AreNotEqual("", result);
        }

        [TestMethod]
        public void PasswordHasherCaesarShouldResurnEmptyValueWhenPasswordIsIncorrect()
        {
            string password = "kiełbasa";
            PasswordHasher hasher = new PasswordHasher();
            string result = hasher.HashPasswordCaesar(password);
            Assert.AreEqual("", result);
        }

        [TestMethod]
        public void PasswordHasherCaesarShouldHashAndUnhashTheSamePassword()
        {
            string password = "kielbasa";
            string expected = password;
            PasswordHasher hasher = new PasswordHasher();
            string result = hasher.UnhashPasswordCaesar(hasher.HashPasswordCaesar(password));
            Assert.AreEqual(expected, result);
        }
    }
}
