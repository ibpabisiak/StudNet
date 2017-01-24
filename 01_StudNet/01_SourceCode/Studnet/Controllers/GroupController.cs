using Studnet.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Studnet.Controllers
{
    public class GroupController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            if ((bool)Session["IsLogged"])
            {
                if (Session["Rank"].ToString().Contains("admin"))
                {
                    return View(AppData.Instance().StudnetDatabase.group.ToList());
                }
                else
                {
                    return RedirectToAction("Index", "Group");
                }
            }
            return RedirectToAction("Login", "User");
        }

        [HttpGet]
        public ActionResult RemoveUser(string userId, string groupId)
        {
            if ((bool)Session["IsLogged"])
            {
                if (Session["Rank"].ToString().Contains("admin"))
                {
                    try
                    {
                        AppData.Instance().StudnetDatabase.GroupManagement.RemoveUserFromGroup(
                            AppData.Instance().StudnetDatabase.users.Where(m => m.Id.ToString() == userId).Single(),
                            AppData.Instance().StudnetDatabase.group.Where(m => m.Id.ToString() == groupId).Single());
                        return View("Index",AppData.Instance().StudnetDatabase.group.ToList());
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                        ViewBag.Error = "Wystąpił błąd podczas usuwania użytkownika z grupy: " + ex.Message;
                    }
                    return View("Index", AppData.Instance().StudnetDatabase.group.ToList());
                }
                else
                {
                    return RedirectToAction("Index", "Group");
                }
            }
            return RedirectToAction("Login", "User");
        }

        [HttpPost]
        public ActionResult Add(group newGroup)
        {
            try
            {
                AppData.Instance().StudnetDatabase.GroupManagement.AddGroup(newGroup);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                ViewBag.Error = "Wystąpił błąd podczas dodawania nowej grupy: " + ex.Message;
                return View();
            }
        }

        [HttpGet]
        public ActionResult Add()
        {
            if ((bool)Session["IsLogged"])
            {
                if (Session["Rank"].ToString().Contains("admin"))
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Group");
                }
            }
            return RedirectToAction("Login", "User");
        }

        [HttpGet]
        public ActionResult AddUserToGroup(string groupId)
        {
            if ((bool)Session["IsLogged"])
            {
                if (Session["Rank"].ToString().Contains("admin"))
                {
                    try
                    {
                        var currentGroup = AppData.Instance().StudnetDatabase.group.Where(m => m.Id.ToString() == groupId).Single();
                        var availableUsersToAdd = AppData.Instance().StudnetDatabase.users.ToList().Except(currentGroup.users).ToList();
                        ViewBag.Group = groupId;
                        ViewBag.GroupName = currentGroup.group_name;
                        return View(availableUsersToAdd);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Group");
                }
            }
            return RedirectToAction("Login", "User");
        }

        [HttpGet]
        public ActionResult RemoveGroup(string groupId)
        {
            if ((bool)Session["IsLogged"])
            {
                if (Session["Rank"].ToString().Contains("admin"))
                {
                    try
                    {
                        var selectedGroup = AppData.Instance().StudnetDatabase.group.Where(m => m.Id.ToString() == groupId).Single();
                        AppData.Instance().StudnetDatabase.GroupManagement.RemoveGroup(selectedGroup);
                        return RedirectToAction("Index");
                    }
                    catch(Exception ex)
                    {
                        ViewBag.Error = "Wystąpił błąd podczas usuwania grupy: " + ex.Message;
                        return View("Index", AppData.Instance().StudnetDatabase.group);
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Group");
                }
            }
            return RedirectToAction("Login", "User");
        }

        [HttpGet]
        public ActionResult SeeGroup(string groupId)
        {
            if ((bool)Session["IsLogged"])
            {
                if (Session["Rank"].ToString().Contains("admin"))
                {
                    try
                    {
                        var selectedGroup = AppData.Instance().StudnetDatabase.group.Where(m => m.Id.ToString() == groupId).Single();
                        return View(selectedGroup);
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Error = "Wystąpił błąd podczas próby wyświetlenia grupy: " + ex.Message;
                        return View("Index", AppData.Instance().StudnetDatabase.group);
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Group");
                }
            }
            return RedirectToAction("Login", "User");
        }

        [HttpPost]
        public ActionResult AddUserToGroup(string group_id, List<string> user)
        {
            if ((bool)Session["IsLogged"])
            {
                if (Session["Rank"].ToString().Contains("admin"))
                {
                    try
                    {
                        var currentGroup = AppData.Instance().StudnetDatabase.group.Where(m => m.Id.ToString() == group_id).Single();
                        foreach (var singleUser in user)
                        {
                            var currentUser = AppData.Instance().StudnetDatabase.users.Where(m => m.user_mail == singleUser).Single();
                            AppData.Instance().StudnetDatabase.GroupManagement.AddUserToGroup(currentUser, currentGroup);
                        }
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                        ViewBag.Error = "Wystapił błąd podczas dodawania jednego lub wielu użytkowników do grupy: " + ex.Message;
                        var currentGroup = AppData.Instance().StudnetDatabase.group.Where(m => m.Id.ToString() == group_id).Single();
                        var availableUsersToAdd = AppData.Instance().StudnetDatabase.users.ToList().Except(currentGroup.users).ToList();
                        ViewBag.Group = group_id;
                        ViewBag.GroupName = currentGroup.group_name;
                        return View();
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Group");
                }
            }
            return RedirectToAction("Login", "User");
        }
    }
}