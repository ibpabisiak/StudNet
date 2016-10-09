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
            return View();
        }
    }
}