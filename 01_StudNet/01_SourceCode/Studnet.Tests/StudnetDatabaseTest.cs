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
            users result = AppData.Instance().StudnetDatabase
                .UserManagement.AuthorizeUser("studnet@msnowak.webd.pl", "admin");
            Assert.AreNotSame(null, result);
        }

        [TestMethod]
        public void AuthorizeUserShouldFailOnInvalidData()
        {
            users result;
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
            users user = new users();
            user.user_name = "Admin";
            user.user_mail = "studnet@msnowak.webd.pl";
            bool result = AppData.Instance().StudnetDatabase
                .UserManagement.CheckIfUserExists(user);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckIfUserExistsShouldReturnFalseForInvalidData()
        {
            users user = new users();
            user.user_name = "SumWrong";
            user.user_mail = "SumWrongMail";
            bool result = AppData.Instance().StudnetDatabase
                .UserManagement.CheckIfUserExists(user);
            Assert.IsFalse(result);
        }
    }
}
