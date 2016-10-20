using Studnet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
/// <summary>
/// Singleton class for holding most important app data
/// </summary>

namespace Studnet
{
    public class AppData
    {
        private static AppData instance;

        //App data
        public StudnetDatabase StudnetDatabase { get; private set; }
        public List<User> LogedUsers { get; private set; }

        private AppData()
        {
            LogedUsers = new List<User>();
            StudnetDatabase = new StudnetDatabase();
        }

        /// <summary>
        /// Method which logs in user
        /// </summary>
        /// <param name="user_mail">User mail</param>
        /// <param name="user_password">User password</param>
        public void LogonUser(string user_mail, string user_password)
        {
            User user = null;
            try
            {
                user = StudnetDatabase.UserManagement
                    .AuthorizeUser(user_mail, user_password);
                LogedUsers.Add(user);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        
        public static AppData Instance()
        {
            if(null == instance)
            {
                instance = new AppData();
            }
            return instance;
        }


    }
}