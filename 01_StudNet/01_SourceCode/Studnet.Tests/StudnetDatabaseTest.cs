using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Studnet.Models;

namespace Studnet.Tests.Controllers
{
    [TestClass]
    public class StudnetDatabaseTest
    {
        [TestMethod]
        public void AuthorizeUserShouldReturnTrueWhenLoggingAsAdmin()
        {
            User result = AppData.Instance().StudnetDatabase
                .UserManagement.AuthorizeUser("studnet@msnowak.webd.pl", "admin");
            Assert.AreNotSame(null, result);
        }

        [TestMethod]
        public void AuthorizeUserShouldFailOnInvalidData()
        {
            User result;
            try
            {
                result = AppData.Instance().StudnetDatabase
                    .UserManagement.AuthorizeUser("Sum", "Wrong");
            }
            catch(Exception ex)
            {
                result = null;
            }
            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void CheckIfUserExistsShouldReturnTrueForAdmin()
        {
            User user = new User();
            user.user_name = "Admin";
            user.user_mail = "studnet@msnowak.webd.pl";
            bool result = AppData.Instance().StudnetDatabase
                .UserManagement.CheckIfUserExists(user);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckIfUserExistsShouldReturnFalseForInvalidData()
        {
            User user = new User();
            user.user_name = "SumWrong";
            user.user_mail = "SumWrongMail";
            bool result = AppData.Instance().StudnetDatabase
                .UserManagement.CheckIfUserExists(user);
            Assert.IsFalse(result);
        }
    }
}
