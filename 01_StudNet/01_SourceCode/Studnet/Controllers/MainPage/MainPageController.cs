using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Studnet.Controllers.Main
{
    public class MainPageController : Controller
    {
        // GET: Main
        public ActionResult Index()
        {
            AppData.Instance().WebsiteAdress = Request.Url.Host + ":" + Request.Url.Port;
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}