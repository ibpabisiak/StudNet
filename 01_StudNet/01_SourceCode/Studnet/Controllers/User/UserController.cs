using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Studnet.Models;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;
using System.Security.Cryptography;

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

        /// <summary>
        /// Method which registeres user based on data given by user
        /// </summary>
        /// <param name="user">User object created from data posted from form</param>
        /// <returns>Action based on result of adding</returns>
        [HttpPost]
        public ActionResult Register(Models.User user)
        {
            try
            {
                AppData.Instance().StudnetDatabase
                    .UserManagement.AddUser(user);

                // send verification mail

                UrlHelper urlHelper = new UrlHelper(this.ControllerContext.RequestContext);
                string url = Request.Url.Host + ":" + Request.Url.Port + urlHelper.Action("VerifyMail", "User", new { userMail = Server.HtmlDecode(user.user_mail) });
                MailMessage mail = new MailMessage("studnet@msnowak.webd.pl", user.user_mail);
                mail.Subject = "Weryfikacja adresu email";
                mail.Body = "Witamy, \n" +
                            "Aby zweryfikować adres e-mail wejdź pod adres \n" +
                            url + "\nPozdrawiamy,\n" +
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
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                ViewBag.Error = ex.Message;
                return View(user);
            }
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
                AppData.Instance().StudnetDatabase.Users.FirstOrDefault(w => w.user_mail == userMail);
            string returnMsg = "";
            if (userFind != null)
            {
                userFind.user_mail_check = true;
                AppData.Instance().StudnetDatabase.SaveChanges();
                returnMsg = "Adres e-mail " + userFind.user_mail + " został zweryfikowany";
            }
            else
            {
                returnMsg = "Konto z takim adresem e-mail nie istnieje w bazie";
            }
            
            ViewBag.Message = returnMsg;
            return View();
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "MainPage");
        }

        [HttpGet]
        public ActionResult Login()
        {
            if ((String)Session["user"] != null)
            {
                Session.Abandon();
                return RedirectToAction("Index", "MainPage");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Login(string user_mail, string user_password)
        {
            try
            {
                AppData.Instance().LogonUser(user_mail, user_password);
                var loggedUser = AppData.Instance().StudnetDatabase.Users.FirstOrDefault(m => m.user_mail == user_mail);
                Session["IsLogged"] = true;
                Session.Add("User", loggedUser.user_mail);
                Session.Add("Username", loggedUser.user_name + " " + loggedUser.user_surname);
                return RedirectToAction("Index", "MainPage");
            }
            catch (Exception ex)
            {
                switch(ex.Message.ToLower())
                {
                    case "invalid email":
                        ViewBag.Error = "Podany adres email nie znajduje się w bazie";
                        break;
                    case "invalid password":
                        ViewBag.Error = "Podane hasło jest nieprawidłowe";
                        break;
                    case "user not activated":
                        ViewBag.Error = "Konto nie zostało aktywowane. Sprawdź swoją skrzynkę pocztową i wejdź w link aktywacyjny";
                        break;
                    default:
                        ViewBag.Error = "Wystąpił błąd. Spróbuj ponownie";
                        break;
                }
                return View();
            }
        }

        public ActionResult ChangePassword()
        {
            if (Session["isLogged"] == null)
                RedirectToAction("Index", "MainPage");
            ViewBag.Success = false;
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(string oldPassword, string newPassword, string newPasswordRepeat)
        {
            if (Session["isLogged"] == null)
                RedirectToAction("Index", "MainPage");

            if (!newPassword.Equals(newPasswordRepeat))
            {
                ViewBag.Error = "Nowe hasła nie zgadzają się ze sobą";
                ViewBag.Success = false;
                return View();
            }

            if (!Models.User.validatePassword(newPassword))
            {
                 ViewBag.Error =
                    "Nowe hasło nie spełnia kryteriów bezpieczeństwa (8-32 znaki, małe litery, duże litery i cyfry lub znaki specjalne";
                ViewBag.Success = false;
                return View();
            }
               
            try
            {
                AppData.Instance()
                    .StudnetDatabase.UserManagement.ChangePassword(Session["User"].ToString(), newPassword, oldPassword);
                ViewBag.Success = true;
            }
            catch (Exception e)
            {
                switch (e.Message.ToLower())
                {
                    case "invalid email":
                        ViewBag.Error = "Błędny adres E-Mail użytkownika";
                        break;
                    case "invalid password":
                        ViewBag.Error = "Podano błędne obecne hasło użytkownika";
                        break;
                }
                ViewBag.Success = false;
            }

            return View();
        }
    }
}