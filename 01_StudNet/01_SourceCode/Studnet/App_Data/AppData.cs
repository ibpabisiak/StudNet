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
        public StudnetDatabase studnetDatabase { get; set; }
        public List<User> logedUsers { get; private set; }

        private AppData()
        {
            logedUsers = new List<User>();
            studnetDatabase = new StudnetDatabase();
        }

        public void LogonUser(string user_mail, string user_password)
        {
            User user = null;
            try
            {
                user = studnetDatabase.AuthorizeUser(user_mail, user_password);
                logedUsers.Add(user);
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