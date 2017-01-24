using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Studnet.Controllers
{
    public class AdminPanelController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(List<string> admins)
        {
            if(admins == null)
            {
                admins = new List<string>();
            }
            var allUsers = AppData.Instance().StudnetDatabase.users;
            bool ifFound = false;
            foreach (var user in allUsers)
            {
                ifFound = false;
                if (user.rank.rank_name.ToLower() != "superadmin")
                {
                    foreach (var newAdmin in admins)
                    {
                        if (newAdmin == user.Id.ToString() && !user.rank.rank_name.ToLower().Contains("admin"))
                        {
                            ifFound = true;
                            AppData.Instance().StudnetDatabase.UserManagement.SetUserRank(user, UserManagement.Rank.Admin);
                        }
                        else if(newAdmin == user.Id.ToString() && user.rank.rank_name.ToLower().Contains("admin"))
                        {
                            ifFound = true;
                        }
                    }
                    if (!ifFound)
                    {
                        if (user.rank.rank_name.ToLower() != "user")
                        {
                            AppData.Instance().StudnetDatabase.UserManagement.SetUserRank(user, UserManagement.Rank.User);
                        }
                    }
                }
            }
            return View();
        }
    }
}