using Studnet.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Studnet.Controllers
{
    public class ForumController : Controller
    {
        private ActionResult ValidateLoggedAction(ActionResult loggedAction)
        {
            if (!(bool)Session["IsLogged"])
            {
                return RedirectToAction("Index", "MainPage");
            }
            else
            {
                return loggedAction;
            }
        }

        public ActionResult Index()
        {
            return ValidateLoggedAction(View());
        }

        [HttpGet]
        public ActionResult AddReply(int thread_id)
        {
            try
            {
                return ValidateLoggedAction(View("AddReply", AppData.Instance().StudnetDatabase.forum_topic.Where(m => m.Id == thread_id).Single()));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return RedirectToAction("Main", "Index");
            }
        }

        [HttpPost]
        public ActionResult AddReply(int forum_id, int thread_id, forum_topic_reply threadReply)
        {
            try
            {
                forum_topic thread = AppData.Instance().StudnetDatabase.forum.Where(m => m.Id == forum_id).Single().forum_topic.Where(m => m.Id == thread_id).Single();
                if (threadReply.forum_topic_reply_content.Length <= 1)
                {
                    ViewBag.Error = "Treść wpisu musi zawierać co najmniej dwa znaki";
                }
                else
                {
                    AppData.Instance().StudnetDatabase.ForumManagement.AddReply(thread, threadReply, Session["User"].ToString());
                }
                return RedirectToAction("SeeThread", new { thread_id = thread_id });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return RedirectToAction("Main", "Index");
            }
        }

        [HttpGet]
        public ActionResult SeeThread(int thread_id)
        {
            try
            {
                return ValidateLoggedAction(View("Thread", AppData.Instance().StudnetDatabase.forum_topic.Where(m => m.Id == thread_id).Single()));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return RedirectToAction("Main", "Index");
            }
        }

        [HttpGet]
        public ActionResult AddThread(int forum_id)
        {
            try
            {
                return ValidateLoggedAction(View("AddThread", AppData.Instance().StudnetDatabase.forum.Where(m => m.Id == forum_id).Single()));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return RedirectToAction("Main", "Index");
            }
        }

        [HttpPost]
        public ActionResult AddThread(int forum_id, forum_topic forumTopic, forum_topic_reply threadPost)
        {
            forum threadForum = AppData.Instance().StudnetDatabase.forum.Where(m => m.Id == forum_id).Single();
            if (AppData.Instance().StudnetDatabase.ForumManagement.CheckIfThreadExists(threadForum, forumTopic.forum_topic_title))
            {
                ViewBag.Error = "Temat o podanej nazwie już istnieje!";
                return View(threadForum);
            }
            else if(forumTopic.forum_topic_title == null || forumTopic.forum_topic_title.Length <=1)
            {
                ViewBag.Error = "Nazwa tematu musi zawierać co najmniej dwa znaki";
                return View(threadForum);
            }
            else if(threadPost.forum_topic_reply_content == null || threadPost.forum_topic_reply_content.Length <= 1)
            {
                ViewBag.Error = "Treść wpisu musi zawierać co najmniej dwa znaki";
                return View(threadForum);
            }
            else
            {
                try
                {
                    AppData.Instance().StudnetDatabase.ForumManagement.AddThread(threadForum, forumTopic, threadPost, Session["User"].ToString());
                }
                catch(Exception ex)
                {
                    Debug.WriteLine(ex);
                    ViewBag.Error = "Wystąpił nieoczekiwany błąd. Sprawdź wszystkie wprowadzone dane i spróbuj ponownie";
                    return View(threadForum);
                }
                return View("Thread", forumTopic);
            }
        }
    }
}