using Studnet.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Studnet.Controllers
{
    public class EventController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            if ((bool)Session["IsLogged"])
            {
                return View(AppData.Instance().StudnetDatabase._event.ToList());
            }
            return RedirectToAction("Login", "User");
        }

        [HttpGet]
        public ActionResult SeeEvent(int eventId)
        {
            if ((bool)Session["IsLogged"])
            {
                return View(AppData.Instance().StudnetDatabase._event.Where(m => m.Id == eventId).Single());
            }
            return RedirectToAction("Login", "User");
        }

        [HttpGet]
        public ActionResult AddEvent(string dateClicked = null)
        {
            if ((bool)Session["IsLogged"])
            {
                if (dateClicked != null)
                {
                    ViewBag.DateSelected = DateTime.Parse(dateClicked);
                }
                else
                {
                    ViewBag.DateSelected = DateTime.Now;
                }
                if (Session["Rank"].ToString().Contains("admin"))
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Event");
                }
            }
            return RedirectToAction("Login", "User");
        }

        [HttpPost]
        public ActionResult RemoveEvent(int eventId)
        {
            try
            {
                AppData.Instance().StudnetDatabase.RemoveRecordFromTable(StudnetDatabase.TableType.Event, AppData.Instance().StudnetDatabase._event.Where(m => m.Id == eventId).Single());
                return RedirectToAction("Index", "Event");
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
                ViewBag.Error = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie. Wiadomość błędu: " + ex.Message;
                return View("SeeEvent");
            }
        }

        [HttpPost]
        public ActionResult AddEvent(_event newEvent)
        {
            try
            {
                if (newEvent.event_description == null || newEvent.event_description.Length <= 2)
                {
                    ViewBag.Error = "Opis wydarzenia musi zawierać co najmniej 3 znaki";
                    ViewBag.DateSelected = newEvent.event_start;
                    return View();
                }
                else if (newEvent.event_title == null || newEvent.event_title.Length <= 2)
                {
                    ViewBag.Error = "Tytuł wydarzenia musi zawierać co najmniej 3 znaki";
                    ViewBag.DateSelected = newEvent.event_start;
                    return View();
                }
                else if (newEvent.event_start == null)
                {
                    ViewBag.Error = "Data rozpoczęcia nie może być pusta";
                    ViewBag.DateSelected = DateTime.Now;
                    return View();
                }
                else if (newEvent.event_end == null)
                {
                    ViewBag.Error = "Data zakończenia nie może być pusta";
                    ViewBag.DateSelected = newEvent.event_start;
                    return View();
                }
                else if (newEvent.event_end.CompareTo(newEvent.event_start) <= 0)
                {
                    ViewBag.Error = "Wydarzenie nie może mieć zerowego ani ujemnego czasu trwania";
                    ViewBag.DateSelected = newEvent.event_start;
                    return View();
                }
                else
                {
                    AppData.Instance().StudnetDatabase.AddRecordToTable(StudnetDatabase.TableType.Event, newEvent);
                    return RedirectToAction("Index", "Event");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                ViewBag.Error = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie. Treść błędu: " + ex.Message;
                return View();
            }
        }

        [HttpPost]
        public ActionResult EditEvent(_event editedEvent)
        {
            try
            {
                if(editedEvent.event_description == null || editedEvent.event_description.Length <=2)
                {
                    ViewBag.Error = "Opis wydarzenia musi zawierać co najmniej 3 znaki";
                    return View("SeeEvent", AppData.Instance().StudnetDatabase._event.Where(m => m.Id == editedEvent.Id).Single());
                }
                else if (editedEvent.event_title == null || editedEvent.event_title.Length <= 2)
                {
                    ViewBag.Error = "Tytuł wydarzenia musi zawierać co najmniej 3 znaki";
                    return View("SeeEvent", AppData.Instance().StudnetDatabase._event.Where(m => m.Id == editedEvent.Id).Single());
                }
                else if (editedEvent.event_start == null)
                {
                    ViewBag.Error = "Data rozpoczęcia nie może być pusta";
                    return View("SeeEvent", AppData.Instance().StudnetDatabase._event.Where(m => m.Id == editedEvent.Id).Single());
                }
                else if(editedEvent.event_end == null)
                {
                    ViewBag.Error = "Data zakończenia nie może być pusta";
                    return View("SeeEvent", AppData.Instance().StudnetDatabase._event.Where(m => m.Id == editedEvent.Id).Single());
                }
                else if(editedEvent.event_end.CompareTo(editedEvent.event_start) <=0)
                {
                    ViewBag.Error = "Wydarzenie nie może mieć zerowego ani ujemnego czasu trwania";
                    return View("SeeEvent", AppData.Instance().StudnetDatabase._event.Where(m => m.Id == editedEvent.Id).Single());
                }
                else
                {
                    AppData.Instance().StudnetDatabase.UpdateTableEntry(StudnetDatabase.TableType.Event, editedEvent);
                    return RedirectToAction("Index", "Event");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                ViewBag.Error = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie. Treść błędu: " + ex.Message;
                return View("SeeEvent", AppData.Instance().StudnetDatabase._event.Where(m => m.Id == editedEvent.Id).Single());
            }
        }

    }
}