using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestWebApp.Models;
using PagedList;
using PagedList.Mvc;
using WebMatrix.WebData;

namespace TestWebApp.Controllers
{
    [Authorize]
    public class MonitoringController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            HelpDeskEntities hDesk = new HelpDeskEntities();
            ViewBag.MonitoringReports = hDesk.TravelEvents;
            return View();
        }

        [Authorize]
        public ActionResult Create()
        {
            HelpDeskEntities hDesk = new HelpDeskEntities();
            ViewBag.Events = hDesk.TravelEvents;
            return View();
        }
        
        [HttpPost]
        [Authorize]
        public ActionResult Create(TravelContract model, string eventName)
        {
            HelpDeskEntities hDesk = new HelpDeskEntities();
            User current_user = hDesk.Users.SingleOrDefault(u => u.id == WebSecurity.CurrentUserId);
            if (ModelState.IsValid)
            {
                TravelContract te = new TravelContract();
                te.NameOfClient = model.NameOfClient;
                te.Adress = model.Adress;
                te.Quantity = model.Quantity;
                te.TravelEvent = hDesk.TravelEvents.FirstOrDefault(td => td.Name == eventName);
                te.Group = current_user.Groups.First();
                hDesk.TravelContracts.Add(te);
                hDesk.SaveChanges();
                return RedirectToAction("Index", "TravelEvent", new { msg = "Rezervace vytvořena" }); 
            }
            else
            {
                ViewBag.IsNotValid = true;
                return View(model);
            }
        }
    }
}