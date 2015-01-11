﻿using System;
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
            HelpDeskEntities hDesk = new HelpDeskEntities();
            ViewBag.TravelEvents = hDesk.TravelEvents.OrderBy(w => w.Name).ToPagedList(page ?? 1, 10);
            return View();
        }

        public ActionResult Details(int id, string msg = null)
        {
            ViewBag.Message = msg;
            HelpDeskEntities hDesk = new HelpDeskEntities();
            TravelEvent ws = hDesk.TravelEvents.SingleOrDefault(w => w.Id == id);
            return View(ws);
        }

        [Authorize]
        public ActionResult Create()
        {
            HelpDeskEntities hDesk = new HelpDeskEntities();
            ViewBag.Destinations = hDesk.TravelDestinations;
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(TravelEvent model, string destination)
        {
            HelpDeskEntities hDesk = new HelpDeskEntities();
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
            else
            {
                ViewBag.IsNotValid = true;
                return View(model);
            }
        }

        [Authorize]
        public ActionResult Edit(int id,TravelEvent model)
        {
            HelpDeskEntities hDesk = new HelpDeskEntities();
            if (ModelState.IsValid)
            {
                TravelEvent ws = new TravelEvent();
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
            HelpDeskEntities hDesk = new HelpDeskEntities();
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