using Studnet.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public List<users> LogedUsers { get; private set; }
        public string WebsiteAdress { get; set; }

        public users users
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        public StudnetDatabase StudnetDatabase1
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        private AppData()
        {
            LogedUsers = new List<users>();
            StudnetDatabase = new StudnetDatabase();
        }

        /// <summary>
        /// Method which logs in user
        /// </summary>
        /// <param name="user_mail">User mail</param>
        /// <param name="user_password">User password</param>
        public void LogonUser(string user_mail, string user_password)
        {
            users user = null;
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

        public void LogoutUser(string user_mail)
        {
            try
            {
                LogedUsers.Remove(LogedUsers.Where(m => m.user_mail == user_mail).Single());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
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