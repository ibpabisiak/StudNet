using Studnet.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Studnet.Controllers
{
    public class MessageController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            if ((bool)Session["IsLogged"])
            {
                string userMail = Session["User"].ToString();
                users currentUser = AppData.Instance().StudnetDatabase.users.Where(m => m.user_mail == userMail).Single();
                List<message> userMessages = currentUser.message.Concat(currentUser.message1).ToList();
                userMessages = userMessages.OrderByDescending(m => m.message_date_created).ToList();
                List<users> conversatedWith = new List<users>();
                foreach (var item in userMessages)
                {
                    conversatedWith.Add(item.users);
                    conversatedWith.Add(item.users1);
                }
                conversatedWith.RemoveAll(m => m.user_mail == userMail);
                conversatedWith = conversatedWith.Distinct().ToList();
                List<message> latestMessages = new List<message>();
                foreach (var user in conversatedWith)
                {
                    latestMessages.Add(userMessages.Where(m => m.users == user || m.users1 == user).First());
                }
                
                ViewBag.CurrentUser = currentUser.Id;
                return View(latestMessages);
            }
            return RedirectToAction("Login", "User");
        }

        [HttpGet]
        public ActionResult Create(int userToId = -1, string preMessage = null)
        {
            if ((bool)Session["IsLogged"])
            {
                string userMail = Session["User"].ToString();
                ViewBag.Users = AppData.Instance().StudnetDatabase.users.Where(m => m.user_mail != userMail).ToList();
                if(userToId != -1)
                {
                    ViewBag.UserTo = userToId;
                }
                if(preMessage != null)
                {
                    ViewBag.PreMessage = preMessage.Trim();
                }
                return View();
            }
            return RedirectToAction("Login", "User");
        }

        [HttpGet]
        public ActionResult Read(int userId)
        {
            List<message> messagesInConv = new List<message>();
            try
            {
                string userMail = Session["User"].ToString();
                users currentUser = AppData.Instance().StudnetDatabase.users.Where(m => m.user_mail == userMail).Single();
                users userToRead = AppData.Instance().StudnetDatabase.users.Where(m => m.Id == userId).Single();
                messagesInConv = currentUser.message.Concat(currentUser.message1).Where(m => m.users == userToRead || m.users1 == userToRead).ToList().OrderBy(m => m.message_date_created).ToList();
                ViewBag.ConversationWith = userToRead.GetFullName();
                ViewBag.ConversedWithId = userToRead.Id;
                foreach (var item in messagesInConv)
                {
                    if (item.message_user_to_id == currentUser.Id && !item.message_is_read)
                    {
                        item.message_is_read = true;
                        AppData.Instance().StudnetDatabase.SaveChanges();
                    }
                }
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return RedirectToAction("Index");
            }
            return View(messagesInConv);
        }

        [HttpPost]
        public ActionResult Read(message newMessage)
        {
            newMessage.message_date_created = DateTime.Now;
            string userMail = Session["User"].ToString();
            newMessage.message_user_from_id = AppData.Instance().StudnetDatabase.users.Where(m => m.user_mail == userMail).Single().Id;
            newMessage.message_is_read = false;
            users currentUser = AppData.Instance().StudnetDatabase.users.Where(m => m.user_mail == userMail).Single();
            users userToRead = AppData.Instance().StudnetDatabase.users.Where(m => m.Id == newMessage.message_user_to_id).Single();
            List<message> messagesInConv = currentUser.message.Concat(currentUser.message1).Where(m => m.users == userToRead || m.users1 == userToRead).ToList().OrderBy(m => m.message_date_created).ToList();
            if (!AppData.Instance().StudnetDatabase.users.Any(m => m.Id == newMessage.message_user_to_id))
            {
                ViewBag.Error = "Niepoprawny odbiorca";
                ViewBag.Users = AppData.Instance().StudnetDatabase.users.Where(m => m.user_mail != userMail).ToList();
                ViewBag.ConversationWith = userToRead.GetFullName();
                ViewBag.ConversedWithId = userToRead.Id;
                return View(messagesInConv);
            }
            else if (newMessage.message_user_to_id == newMessage.message_user_from_id)
            {
                ViewBag.Error = "Nie możesz wysłać wiadomości do samego siebie";
                ViewBag.Users = AppData.Instance().StudnetDatabase.users.Where(m => m.user_mail != userMail).ToList();
                ViewBag.ConversationWith = userToRead.GetFullName();
                ViewBag.ConversedWithId = userToRead.Id;
                return View(messagesInConv);
            }
            else if (newMessage.message_text == null || newMessage.message_text.Length < 1)
            {
                ViewBag.Error = "Wiadomość nie może być pusta";
                ViewBag.Users = AppData.Instance().StudnetDatabase.users.Where(m => m.user_mail != userMail).ToList();
                ViewBag.ConversationWith = userToRead.GetFullName();
                ViewBag.ConversedWithId = userToRead.Id;
                return View(messagesInConv);
            }
            else
            {
                AppData.Instance().StudnetDatabase.MessageManagement.SendMessage(newMessage);
            }

            return RedirectToAction("Read",new { userId = userToRead.Id });
        }

        [HttpPost]
        public ActionResult Create(message newMessage)
        {
            newMessage.message_date_created = DateTime.Now;
            string userMail = Session["User"].ToString();
            newMessage.message_user_from_id = AppData.Instance().StudnetDatabase.users.Where(m => m.user_mail == userMail).Single().Id;
            newMessage.message_is_read = false;
            if(!AppData.Instance().StudnetDatabase.users.Any(m=>m.Id == newMessage.message_user_to_id))
            {
                ViewBag.Error = "Niepoprawny odbiorca";
                ViewBag.Users = AppData.Instance().StudnetDatabase.users.Where(m => m.user_mail != userMail).ToList();
                return View();
            }
            else if (newMessage.message_user_to_id == newMessage.message_user_from_id)
            {
                ViewBag.Error = "Nie możesz wysłać wiadomości do samego siebie";
                ViewBag.Users = AppData.Instance().StudnetDatabase.users.Where(m => m.user_mail != userMail).ToList();
                return View();
            }
            else if(newMessage.message_text == null || newMessage.message_text.Length < 1)
            {
                ViewBag.Error = "Wiadomość nie może być pusta";
                ViewBag.Users = AppData.Instance().StudnetDatabase.users.Where(m => m.user_mail != userMail).ToList();
                return View();
            }
            else
            {
                AppData.Instance().StudnetDatabase.MessageManagement.SendMessage(newMessage);
            }

            return RedirectToAction("Index");
        }
    }
}