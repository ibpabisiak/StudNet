using Studnet.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Studnet
{
    public class UserManagement
    {
        private DbSet<User> users;
        private PasswordHasher passwordHasher = new PasswordHasher();

        public UserManagement(DbSet<User> usersTable)
        {
            users = usersTable;
        }

        /// <summary>
        /// Method which adds user to database
        /// </summary>
        /// <param name="user">Validated user to add</param>
        public void AddUser(User user)
        {
            try
            {
                if (users.FirstOrDefault(m => m.user_mail == user.user_mail) != null)
                {
                    throw new Exception("User already exists in database");
                }
                else
                {
                    user.user_password = passwordHasher.HashPasswordMD5(user.user_password);
                    AppData.Instance().StudnetDatabase.AddRecordToTable(StudnetDatabase.TableType.Users, user);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method which authorizes given mail and password and if there is user coresponding
        /// to given data, method returns it
        /// </summary>
        /// <param name="mail">User mail</param>
        /// <param name="password">User password</param>
        /// <returns>User corespodning to given data</returns>
        public User AuthorizeUser(string mail, string password)
        {
            var user = users.FirstOrDefault(m => m.user_mail == mail);
            password = passwordHasher.HashPasswordMD5(password);

            if (user == null)
            {
                throw new Exception("Invalid email");
            }
            else if (user.user_password != password)
            {
                throw new Exception("Invalid password");
            }
            else if (!user.user_mail_check)
            {
                throw new Exception("User not activated");
            }

            return user;
        }

        /// <summary>
        /// Method which checks if user exists in database
        /// </summary>
        /// <param name="user">User to check if exists in database</param>
        /// <returns>True if user exists in database</returns>
        public bool CheckIfUserExists(User user)
        {
            bool ifUserExists = false;

            if (users.FirstOrDefault(m => m.user_mail == user.user_mail) != null)
            {
                ifUserExists = true;
            }
            return ifUserExists;
        }

        /// <summary>
        /// Method which sends an email to given user
        /// </summary>
        /// <param name="user_mail">User's email adress</param>
        /// <param name="title">Title of the message</param>
        /// <param name="body">Body of the message</param>
        public void SendEmailToUser(string user_mail, string title, string body)
        {
            try
            {
                MailMessage mail = new MailMessage("studnet@msnowak.webd.pl", user_mail);
                mail.Subject = title;
                mail.Body = body;
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Port = 25;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Host = "msnowak.webd.pl";
                smtpClient.Credentials = new NetworkCredential("studnet@msnowak.webd.pl", "studnet");
                smtpClient.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method which checks if user exists in database
        /// </summary>
        /// <param name="user_mail">User's mail</param>
        /// <returns>True if user exists in database</returns>
        public bool CheckIfUserExists(string user_mail)
        {
            bool ifUserExists = false;

            if (users.FirstOrDefault(m => m.user_mail == user_mail) != null)
            {
                ifUserExists = true;
            }
            return ifUserExists;
        }

        public void ChangePassword(string _email, string _newPasswd)
        {
            var user = users.FirstOrDefault(m => m.user_mail == _email);
            _newPasswd = passwordHasher.HashPasswordMD5(_newPasswd);
            if (user == null)
                throw new Exception("invalid email");
            user.user_password = _newPasswd;
            AppData.Instance().StudnetDatabase.SaveChanges();
        }

        public void ChangePassword(string _email, string _newPasswd, string _oldPassword)
        {
            var user = users.FirstOrDefault(m => m.user_mail == _email);
            _newPasswd = passwordHasher.HashPasswordMD5(_newPasswd);
            _oldPassword = passwordHasher.HashPasswordMD5(_oldPassword);
            if(user == null)
                throw new Exception("invalid email");
            else if(!user.user_password.Equals(_oldPassword))
                throw new Exception("invalid password");
            user.user_password = _newPasswd;
            AppData.Instance().StudnetDatabase.SaveChanges();
        }
    }
}