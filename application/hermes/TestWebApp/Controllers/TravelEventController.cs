using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestWebApp.Models;
using PagedList;
using PagedList.Mvc;
namespace TestWebApp.Controllers
{
    [Authorize]
    public class TravelEventController : Controller
    {
        public ActionResult Index(int? page, string msg = null)
        {
            ViewBag.Message = msg;
            HermesDBEntities hDesk = new HermesDBEntities();
            ViewBag.TravelEvents = hDesk.TravelEvents.OrderBy(w => w.Name).ToPagedList(page ?? 1, 10);
            return View();
        }

        public ActionResult Details(int id, string msg = null)
        {
            ViewBag.Message = msg;
            HermesDBEntities hDesk = new HermesDBEntities();
            TravelEvent ws = hDesk.TravelEvents.SingleOrDefault(w => w.Id == id);
            return View(ws);
        }

        [Authorize]
        public ActionResult Create()
        {
            HermesDBEntities hDesk = new HermesDBEntities();
            ViewBag.Destinations = hDesk.TravelDestinations;
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(TravelEvent model, string destination)
        {
            HermesDBEntities hDesk = new HermesDBEntities();
            if (ModelState.IsValid)
            {
                TravelEvent ws = new TravelEvent();
                ws.Name = model.Name;
                ws.Description = model.Description;
                ws.Price = model.Price;
                ws.TravelDestination = hDesk.TravelDestinations.FirstOrDefault(td => td.Name == destination);
                ws.Capacity = model.Capacity;
                hDesk.TravelEvents.Add(ws);
                hDesk.SaveChanges();
                return RedirectToAction("Index", new { msg = "Zájezd byl vytvořen" });
            }
            ViewBag.Destinations = hDesk.TravelDestinations;
            ViewBag.IsNotValid = true;
            return View(model);
        }

        [Authorize]
        public ActionResult Edit(int id,TravelEvent model)
        {
            HermesDBEntities hDesk = new HermesDBEntities();
            if (ModelState.IsValid)
            {
                TravelEvent ws = hDesk.TravelEvents.FirstOrDefault(te=>te.Id == id);
                ws.Name = model.Name;
                ws.Description = model.Description;
                ws.Price = model.Price;
                ws.Capacity = model.Capacity;
                hDesk.TravelEvents.Add(ws);
                hDesk.SaveChanges();
                return RedirectToAction("Index", new { msg = "Zájezd byl upraven" });
            }
            else
            {
                return RedirectToAction("Index", new { msg = "Špatná vstupní pole" });
            }
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            HermesDBEntities hDesk = new HermesDBEntities();
            TravelEvent re = hDesk.TravelEvents.SingleOrDefault(w => w.Id == id);
            if (re != null)
            {
                hDesk.TravelEvents.Remove(re);
                hDesk.SaveChanges();
                return RedirectToAction("Index", new { msg = "Zájezd úspěšně vymazán" });
            }
            else
            {
                return RedirectToAction("NotFound", "Errors");
            }
        }
    }
}