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
    public class WorkstationController : Controller
    {
        public ActionResult Index(int? page, string msg = null)
        {
            ViewBag.Message = msg;
            HelpDeskEntities hDesk = new HelpDeskEntities();
            ViewBag.Workstations = hDesk.Workstations.OrderBy(w => w.wsName).ToPagedList(page ?? 1, 10);
            return View();
        }

        [Authorize(Roles="SuperUSer")]
        public ActionResult Create()
        {
           return View();
        }

        [HttpPost]
        [Authorize(Roles = "SuperUSer")]
        public ActionResult Create(Workstation model)
        {
            HelpDeskEntities hDesk = new HelpDeskEntities();
            if (ModelState.IsValid)
            {
                Workstation ws = new Workstation();
                ws.wsName = model.wsName_;
                ws.wsOwner = model.wsOwner_;
                ws.wsManufacturer = model.wsManufacturer_;
                ws.osType = model.osType_;
                hDesk.Workstations.Add(ws);
                hDesk.SaveChanges();
                return RedirectToAction("Details/" + ws.id, new { msg = "Workstation has been successfully created" });
            }
            else
            {
                ViewBag.IsNotValid = true;
                return View(model);
            }
        }

        public ActionResult Details(int id,string msg = null)
        {
            ViewBag.Message = msg;
            HelpDeskEntities hDesk = new HelpDeskEntities();
            Workstation ws = hDesk.Workstations.SingleOrDefault(w => w.id == id);
            return View(ws);
        }

        [Authorize(Roles="SuperUser")]
        public ActionResult Edit(int id)
        {
            HelpDeskEntities hDesk = new HelpDeskEntities();
            Workstation ws = hDesk.Workstations.SingleOrDefault(w => w.id == id);

            return View(ws);
        }

        [HttpPost]
        [Authorize(Roles = "SuperUser")]
        public ActionResult Edit(int id, Workstation model)
        {
            HelpDeskEntities hDesk = new HelpDeskEntities();
            Workstation ws = hDesk.Workstations.SingleOrDefault(w => w.id == id);
            ws.wsName = model.wsName_;
            ws.wsOwner = model.wsOwner_;
            ws.wsManufacturer = model.wsManufacturer_;
            ws.osType = model.osType_;
            hDesk.SaveChanges();
            return RedirectToAction("Details/" + ws.id, new { msg = "Workstation has been successfully edited" });
        }

        [Authorize(Roles = "SuperUser")]
        public ActionResult Delete(int id)
        {
            HelpDeskEntities hDesk = new HelpDeskEntities();
            Workstation ws = hDesk.Workstations.SingleOrDefault(w => w.id == id);
            if (ws != null)
            {
                hDesk.Workstations.Remove(ws);
                foreach(Retailer retail in hDesk.Retailers)
                {
                    if (retail.wsId == ws.id)
                        retail.wsId = null;
                }
                hDesk.SaveChanges();
                return RedirectToAction("Index", new { msg = "Workstation has been successfully removed" });
            }
            else
            {
                return RedirectToAction("NotFound", "Errors");
            }
        }
    }
}