﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Studnet.Models;

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
            string url = "http://" + Request.Url.Host + urlHelper.Action("VerifyMail", "User", new {mail = Server.HtmlDecode(user.user_mail) });
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
            return View("PostRegister");
        }

        /// <summary>
        /// Na podstawie podanego adresu e-mail sprawdza zawartosc bazy pod katem jego istnienia.
        /// W przypadku znalezienia adresu, ustawia wlasciwosc user_mail_check na true
        /// </summary>
        /// <param name="userMail">Adres e-mail do weryfikacji.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult VerifyMail(string userMail)
        {
            var userFind =
                AppData.Instance().studnetDatabase.users.FirstOrDefault(w => w.user_mail == userMail);
            string returnMsg = "";
            if (userFind != null)
            {
                userFind.user_mail_check = true;
                AppData.Instance().studnetDatabase.SaveChanges();
                returnMsg = "e-mail " + userFind.user_mail + " został zweryfikowany";
            }
            else
            {
                returnMsg = "Konto z takim adresem e-mail nie istnieje w bazie";
            }
            
            ViewBag.Message = returnMsg;
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string user_mail, string user_password)
        {

            return RedirectToAction("Index","MainPage");
        }
    }
}