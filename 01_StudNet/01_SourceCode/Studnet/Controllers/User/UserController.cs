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
using System.Text.RegularExpressions;

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
        public ActionResult Register(Models.User user, string password_repeat)
        {
            try
            {
                Regex regex;
                regex = new Regex(@"^(?=.*)[a-zA-ZęąśżźćńłóĘĄŚŻŹĆŃŁÓ]{1,12}$");
                
                // data validation
                if (this.validatePassword(user.user_password) == false)
                {
                    ViewBag.Style = "<style>input[name=\"user_password\"] {background-color: #fc0000;}</style>";
                    throw new Exception("Hasło nie spełnia kryteriów bezpieczeństwa");
                }
                if (regex.IsMatch(user.user_name) == false)
                {
                    ViewBag.Style = "<style>input[name=\"user_name\"] {background-color: #fc0000;}</style>";
                    throw new Exception("Imię może składać się tylko z liter oraz polskich znaków. Długość to 1-12 znaków.");
                }
                if (user.user_address_street.Length < 1 || user.user_address_street.Length > 32)
                {
                    ViewBag.Style = "<style>input[name=\"user_address_street\"] {background-color: #fc0000;}</style>";
                    throw new Exception("Nazwa ulicy przekracza dozwoloną ilość znaków (32)");
                }
                regex = new Regex(@"^(?=.*)[a-zA-ZęąśżźćńłóĘĄŚŻŹĆŃŁÓ]{1,}$");
                if (regex.IsMatch(user.user_address_city))
                {
                    user.user_address_city = user.user_address_city.First().ToString().ToUpper() + user.user_address_city.Substring(1); // first letter to upper
                }
                else
                {
                    ViewBag.Style = "<style>input[name=\"user_address_city\"] {background-color: #fc0000;}</style>";
                    throw new Exception("Miasto to tylko litery oraz polskie znaki.");
                }
                if (user.user_semester < 0 || user.user_semester > 9)
                {
                    ViewBag.Style = "<style>input[name=\"user_semester\"] {background-color: #fc0000;}</style>";
                    throw new Exception("Dozwolone znaki w semestrze to to 0-9");
                }
                if (int.Parse(user.user_address_home_number) < 0 || int.Parse(user.user_address_home_number) > 9)
                {
                    ViewBag.Style = "<style>input[name=\"user_address_home_number\"] {background-color: #fc0000;}</style>";
                    throw new Exception("Numer domu to tylko cyfry 0-9");
                }

                if (user.user_study_year < 0 || user.user_study_year > 9)
                {
                    ViewBag.Style = "<style>input[name=\"user_study_year\"] {background-color: #fc0000;}</style>";
                    throw new Exception("Rok studiów to tylko cyfry 0-9");
                }

                regex = new Regex(@"^(?<=.*)[\d]{1,5}$");
                if (regex.IsMatch(user.user_index.ToString()) == false)
                {
                    ViewBag.Style = "<style>input[name=\"user_index\"] {background-color: #fc0000;}</style>";
                    throw new Exception("Numer indeksu to 1-5 znaków, same cyfry");
                }

                //walidacja email
                regex = new Regex(@"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$");
                if (regex.IsMatch(user.user_mail) == false)
                {
                    ViewBag.Style = "<style>input[name=\"user_mail\"] {background-color: #fc0000;}</style>";
                    throw new Exception("Niewłaściwy format adresu email");
                }

                //email - ucinanie od plusa
                string temp;
                if (user.user_mail.IndexOf("+") != -1)
                {
                    temp = user.user_mail.Substring(0, user.user_mail.IndexOf("+"));
                    temp += user.user_mail.Substring(user.user_mail.IndexOf("@"));
                    user.user_mail = temp;
                }

                regex = new Regex(@"^(?=.*)[a-zA-ZęąśżźćńłóĘĄŚŻŹĆŃŁÓ-]{1,24}$");
                if (regex.IsMatch(user.user_surname) == false)
                {
                    ViewBag.Style = "<style>input[name=\"user_surname\"] {background-color: #fc0000;}</style>";
                    throw new Exception("Nazwisko to tylko litery, polskie znaki i \"-\". Długość to 1-24 znaki.");
                }
                
                if (user.user_password != password_repeat)
                {
                    ViewBag.Style = "<style>input[name=\"user_password_repeat\"] {background-color: #fc0000;}</style>";
                    throw new Exception("Podane hasła nie pasują do siebie");
                }
                else
                {
                    AppData.Instance().StudnetDatabase
                        .UserManagement.AddUser(user);

                    // send verification mail

                    UrlHelper urlHelper = new UrlHelper(this.ControllerContext.RequestContext);
                    string url = AppData.Instance().WebsiteAdress + urlHelper.Action("VerifyMail", "User", new { userMail = Server.HtmlDecode(user.user_mail) });
                    AppData.Instance().StudnetDatabase.UserManagement.SendEmailToUser(user.user_mail, "Weryfikacja adresu email",
                        "Witamy, \n" +
                            "Aby zweryfikować adres e-mail wejdź pod adres \n" +
                            url + "\nPozdrawiamy,\n" +
                            "Zespół StudNet");
                    ViewBag.Mail = user.user_mail;
                    return View("PostRegister");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                ViewBag.Error = ex.Message;
                return View(user);
            }
        }

        [ValidateInput(false)]
        [HttpGet]
        public ActionResult ResetPassword(string dateTime, string user_mail)
        {
            PasswordHasher hasher = new PasswordHasher();
            dateTime = hasher.UnhashPasswordCaesar(dateTime);
            user_mail = hasher.UnhashPasswordCaesar(user_mail);
            try
            {
                var data = dateTime.Split(':');
                int controlSum = Convert.ToInt32(data[0]);
                int year = Convert.ToInt32(data[3]);
                int month = Convert.ToInt32(data[2]);
                int day = Convert.ToInt32(data[1]);
                int hour = Convert.ToInt32(data[4]);
                int minute = Convert.ToInt32(data[5]);
                DateTime requestDate = new DateTime(year, month, day, hour, minute, 0);
                if (DateTime.Now.Subtract(requestDate).Hours > 12)
                {
                    throw new Exception("Given link if outdated");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                switch (ex.Message.ToLower())
                {
                    case "given link if outdated":
                        ViewBag.TotalError = "Podany link wygasł. Spróbuj ponownie";
                        break;
                    default:
                        ViewBag.TotalError = "Podany adres jest niepoprawny, spróbuj ponownie zresetować hasło";
                        break;
                }
            }
            ViewBag.Email = user_mail;
            return View();
        }


        [HttpPost]
        public ActionResult ResetPassword(string user_mail, string newPassword, string repeatNewPassword)
        {
            if (!newPassword.Equals(repeatNewPassword))
            {
                ViewBag.Error = "Nowe hasła nie zgadzają się ze sobą";
                ViewBag.Email = user_mail;
                return View();
            }

            if (!validatePassword(newPassword))
            {
                ViewBag.Error =
                   "Nowe hasło nie spełnia kryteriów bezpieczeństwa (8-32 znaki, małe litery, duże litery i cyfry lub znaki specjalne)";
                ViewBag.Email = user_mail;
                return View();
            }

            AppData.Instance().StudnetDatabase.UserManagement.ChangePassword(user_mail, newPassword);

            return View("PostResetPassword");
        }

        private bool validatePassword(string _passwd)
        {
            /*Regex passwordRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d[$@!%*?&+/<{}\-_=\"\',.`~\|;:ęąśżźćńłóĘĄŚŻŹĆŃŁÓ]{8,32}$");
            Match regexMath = passwordRegex.Match(_passwd);
            if (regexMath.Success)
            {
                return true;
            }
            else
            {
                return false;
            }*/

            return true;
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
                returnMsg = "Konto z takim adresem e-mail nie istnieje w bazie. Możliwe, że konto nie zostało aktywowane w ciągu tygodnia od jego założenia. Spróbuj założyć konto ponownie";
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
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string user_mail)
        {
            if (!AppData.Instance().StudnetDatabase.UserManagement.CheckIfUserExists(user_mail))
            {
                ViewBag.Error = "Podany email nie jest przypisany do żadnego konta";
                return View();
            }
            else
            {
                DateTime now = DateTime.Now;
                int controlSum = now.Day + now.Month + now.Year + now.Hour + now.Minute;
                PasswordHasher hasher = new PasswordHasher();
                string hashedDateTime = hasher.HashPasswordCaesar(controlSum.ToString() + ":" + now.ToString("dd:MM:yyyy:H:mm"));
                if (hashedDateTime != "")
                {
                    UrlHelper urlHelper = new UrlHelper(this.ControllerContext.RequestContext);
                    string url = AppData.Instance().WebsiteAdress + urlHelper.Action("ResetPassword", "User", new { dateTime = hashedDateTime, user_mail = hasher.HashPasswordCaesar(user_mail) });
                    AppData.Instance().StudnetDatabase
                        .UserManagement.SendEmailToUser(user_mail, "Resetowanie hasła", "Witaj, \n" +
                        "Otrzymaliśmy polecenie zresetowania Twojego hasła. Jeśli chcesz zresetować hasło, wejdź w link podany poniżej. \n" +
                        url + "\n Jeśli to polecenie nie pochodzi od Ciebie, zignoruj tą wiadomośc. \n Pozdrawiamy, \n Zespół StudNet");

                    return View("PostForgotPassword");
                }
                else
                {
                    ViewBag.Error = "Wystąpił błąd podczas generowania polecenia. Spróbuj ponownie";
                    return View();
                }
            }
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

            if (!validatePassword(newPassword))
            {
                 ViewBag.Error =
                    "Nowe hasło nie spełnia kryteriów bezpieczeństwa (8-32 znaki, małe litery, duże litery i cyfry lub znaki specjalne)";
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