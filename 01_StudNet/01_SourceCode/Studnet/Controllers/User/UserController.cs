using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Studnet.Controllers.User
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Models.User user)
        {
            AppData.Instance().studnetDatabase.users.Add(user);
            AppData.Instance().studnetDatabase.SaveChanges();

            // send verification mail

            UrlHelper urlHelper = new UrlHelper(this.ControllerContext.RequestContext);
            string url = urlHelper.Action("VerifyMail", "User", new {mail = user.user_mail});
            MailMessage mail = new MailMessage("studnet@msnowak.webd.pl", user.user_mail);
            mail.Subject = "Weryfikacja adresu email";
            mail.Body = "Witamy,<br>" +
                        "Aby zweryfikować adres e-mail wejdź pod adres <a href=\"http://address.com"+url+"\">KLIK</a>" + 
                        "<br>Pozdrawiamy,<br>"+
                        "Zespół StudNet";
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Port = 25;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Host = "msnowak.webd.pl";
            smtpClient.Credentials = new NetworkCredential("studnet@msnowak.webd.pl", "studnet");
            smtpClient.Send(mail);
            return View();
        }

        [HttpGet]
        public ActionResult VerifyMail(string userMail)
        {
            //tutaj będzie weryfikacja adresu email
            string returnMsg = "e-mail " + userMail + " został zweryfikowany";
            ViewBag.Message = returnMsg;
            return View();
        }
    }
}