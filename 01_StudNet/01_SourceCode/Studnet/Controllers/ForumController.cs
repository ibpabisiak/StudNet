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
        public ActionResult Index()
        {
            if((bool)Session["IsLogged"])
            {
                ViewBag.Categories = AppData.Instance().StudnetDatabase.forum_topic_category.ToList();
                return View(AppData.Instance().StudnetDatabase.forum.ToList());
            }
            return RedirectToAction("Login", "User");
        }

        [HttpGet]
        public ActionResult AddReply(int thread_id)
        {
            try
            {
                if ((bool)Session["IsLogged"])
                {
                    return View("AddReply", AppData.Instance().StudnetDatabase.forum_topic.Where(m => m.Id == thread_id).Single());
                }
                return RedirectToAction("Login", "User");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return RedirectToAction("Index", "Forum");
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
                return RedirectToAction("Index", "Forum");
            }
        }

        [HttpGet]
        public ActionResult SeeThread(int thread_id)
        {
            try
            {
                if ((bool)Session["IsLogged"])
                {
                    return View("Thread", AppData.Instance().StudnetDatabase.forum_topic.Where(m => m.Id == thread_id).Single());
                }
                return RedirectToAction("Login", "User");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return RedirectToAction("Index", "Forum");
            }
        }

        [HttpGet]
        public ActionResult AddThread(int forum_id)
        {
            try
            {
                if ((bool)Session["IsLogged"])
                {
                    ViewBag.Categories = AppData.Instance().StudnetDatabase.forum_topic_category.ToList();
                    return View("AddThread", AppData.Instance().StudnetDatabase.forum.Where(m => m.Id == forum_id).Single());
                }
                return RedirectToAction("Login", "User");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return RedirectToAction("Index", "Forum");
            }
        }

        [HttpGet]
        public ActionResult AddForum()
        {
            try
            {
                if ((bool)Session["IsLogged"])
                {
                    if (Session["Rank"].ToString().Contains("admin"))
                    {
                        return View("AddForum");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Forum");
                    }
                }
                return RedirectToAction("Login", "User");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return RedirectToAction("Index", "MainPage");
            }
        }

        [HttpPost]
        public ActionResult AddForum(forum newForum)
        {
            if(newForum.forum_name == null || newForum.forum_name.Length<= 2)
            {
                ViewBag.Error = "Nazwa forum musi zawierać minimum 3 znaki";
                return View();
            }
            else
            {
                try
                {
                    AppData.Instance().StudnetDatabase.AddRecordToTable(StudnetDatabase.TableType.Forum, newForum);
                    return View("Index");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    ViewBag.Error = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie";
                    return View();
                }
            }
            
        }

        [HttpGet]
        public ActionResult ManageCategories()
        {
            if ((bool)Session["IsLogged"])
            {
                if (Session["Rank"].ToString().Contains("admin"))
                {
                    return View("ManageCategories", AppData.Instance().StudnetDatabase.forum_topic_category.ToList());
                }
                else
                {
                    return RedirectToAction("Index", "Forum");
                }
            }
            return RedirectToAction("Login", "User");
        }

        [HttpPost]
        public ActionResult AddCategory(forum_topic_category newCategory)
        {
            if(newCategory.forum_topic_category_name == null || newCategory.forum_topic_category_name.Length <= 2)
            {
                ViewBag.Error = "Nazwa kategorii musi zawierać co najmniej 3 znaki";
                return View("ManageCategories", AppData.Instance().StudnetDatabase.forum_topic_category.ToList());
            }
            else
            {
                AppData.Instance().StudnetDatabase.AddRecordToTable(StudnetDatabase.TableType.ForumTopicCategory, newCategory);
                return View("ManageCategories", AppData.Instance().StudnetDatabase.forum_topic_category.ToList());
            }
        }

        [HttpPost]
        public ActionResult RemoveCategory(int categoryToDelete)
        {
            try
            {
                AppData.Instance().StudnetDatabase.RemoveRecordFromTable(StudnetDatabase.TableType.ForumTopicCategory, AppData.Instance().StudnetDatabase.forum_topic_category.Where(m=>m.Id == categoryToDelete).Single());
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
                ViewBag.Error = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie. Opis błędu: " + ex.Message;
            }
            return View("ManageCategories", AppData.Instance().StudnetDatabase.forum_topic_category.ToList());
        }

        [HttpPost]
        public ActionResult AddThread(int forum_id, forum_topic forumTopic, forum_topic_reply threadPost)
        {
            forum threadForum = AppData.Instance().StudnetDatabase.forum.Where(m => m.Id == forum_id).Single();
            if (AppData.Instance().StudnetDatabase.ForumManagement.CheckIfThreadExists(threadForum, forumTopic.forum_topic_title))
            {
                ViewBag.Error = "Temat o podanej nazwie już istnieje!";
                ViewBag.Categories = AppData.Instance().StudnetDatabase.forum_topic_category.ToList();
                return View(threadForum);
            }
            else if(forumTopic.forum_topic_title == null || forumTopic.forum_topic_title.Length <=1)
            {
                ViewBag.Error = "Nazwa tematu musi zawierać co najmniej dwa znaki";
                ViewBag.Categories = AppData.Instance().StudnetDatabase.forum_topic_category.ToList();
                return View(threadForum);
            }
            else if(threadPost.forum_topic_reply_content == null || threadPost.forum_topic_reply_content.Length <= 1)
            {
                ViewBag.Error = "Treść wpisu musi zawierać co najmniej dwa znaki";
                ViewBag.Categories = AppData.Instance().StudnetDatabase.forum_topic_category.ToList();
                return View(threadForum);
            }
            else if(forumTopic.forum_topic_category_id <= 0)
            {
                ViewBag.Error = "Kategoria nie może być pusta";
                ViewBag.Categories = AppData.Instance().StudnetDatabase.forum_topic_category.ToList();
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
                    ViewBag.Categories = AppData.Instance().StudnetDatabase.forum_topic_category.ToList();
                    return View(threadForum);
                }
                return View("Thread", forumTopic);
            }
        }
    }
}