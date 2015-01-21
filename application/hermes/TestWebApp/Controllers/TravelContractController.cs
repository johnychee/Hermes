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
    public class TravelContractController : Controller
    {
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [Authorize]
        public ActionResult Create(TravelContract model)
        {
            HermesDBEntities hDesk = new HermesDBEntities();
            User current_user = hDesk.Users.SingleOrDefault(u => u.id == WebSecurity.CurrentUserId);
            if (ModelState.IsValid)
            {
                TravelContract te = new TravelContract();
                te.NameOfClient = model.NameOfClient;
                te.Adress = model.Adress;
                te.Quantity = model.Quantity;
                te.eventName = model.eventName;
                te.TravelEvent = hDesk.TravelEvents.FirstOrDefault(td => td.Name == model.eventName);
                te.Group = current_user.Groups.First();
                te.CreatedAt = DateTime.Now;
                hDesk.TravelContracts.Add(te);
                //hDesk.SaveChanges();
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