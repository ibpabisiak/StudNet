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

        private AppData()
        {
            studnetDatabase = new StudnetDatabase();
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