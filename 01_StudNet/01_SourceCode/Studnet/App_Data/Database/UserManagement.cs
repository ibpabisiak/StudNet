using Studnet.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

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
                    user.user_password = passwordHasher.HashPassword(user.user_password);
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
            password = passwordHasher.HashPassword(password);

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
    }
}