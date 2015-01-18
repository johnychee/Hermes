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
    public class RegionController : Controller
    {
        public ActionResult Index(int? page,string msg = null)
        {
            ViewBag.Message = msg;
            HermesDBEntities hDesk = new HermesDBEntities();
            ViewBag.Regions = hDesk.Regions.OrderBy(r => r.name).ToPagedList(page ?? 1, 10);
            return View();
        }

        [Authorize(Roles="SuperUser")]
        public ActionResult Create()
        {
            return View();
        }
        
        [Authorize(Roles = "SuperUser")]
        [HttpPost]
        public ActionResult Create(Region model)
        {
            if (ModelState.IsValid)
            {
                HermesDBEntities hDesk = new HermesDBEntities();

                TimeZoneInfo timeZone = TimeZoneInfo.GetSystemTimeZones().SingleOrDefault(t => t.Id == model.timeZoneId);
                Region newRegion = model;
                newRegion.UTCtimeShift = timeZone.BaseUtcOffset.TotalHours;
                hDesk.Regions.Add(newRegion);
                hDesk.SaveChanges();

                return RedirectToActionPermanent("Details", new { @id = newRegion.id, msg = "The region has been successfully created" });
            }
            else
            {
                ViewBag.IsNotValid = true;
                return View(model);
            }
        }

        public ActionResult Details(int id, string msg = null)
        {
            ViewBag.Message = msg;
            HermesDBEntities hDesk = new HermesDBEntities();
            Region region = hDesk.Regions.SingleOrDefault(r => r.id == id);
            return View(region);
        }

        [Authorize(Roles="SuperUser")]
        public ActionResult Edit(int id)
        {
            HermesDBEntities hDesk = new HermesDBEntities();
            Region region = hDesk.Regions.SingleOrDefault(r => r.id == id);
            return View(region);
        }

        [HttpPost]
        [Authorize(Roles = "SuperUser")]
        public ActionResult Edit(int id, Region model)
        {
            HermesDBEntities hDesk = new HermesDBEntities();
            Region region = hDesk.Regions.SingleOrDefault(r => r.id == id);
            if (ModelState.IsValid)
            {
                TimeZoneInfo timeZone = TimeZoneInfo.GetSystemTimeZones().SingleOrDefault(t => t.Id == model.timeZoneId);
                region.FT_minutes = model.FT_minutes_;
                region.name = model.name_;
                region.RT_minutes = model.RT_minutes_;
                region.timeZoneId = model.timeZoneId;
                region.UTCtimeShift = timeZone.BaseUtcOffset.TotalHours;
                hDesk.SaveChanges();
                return RedirectToAction("Details/" + region.id, new { msg = "Region has been successfully updated" });
            }
            else
            {
                ViewBag.IsNotValid = true;
                return View(model);
            }
        }

        [Authorize(Roles = "SuperUser")]
        public ActionResult Remove(int id)
        {
            HermesDBEntities hDesk = new HermesDBEntities();
            Region region = hDesk.Regions.SingleOrDefault(r => r.id == id);
            hDesk.Regions.Remove(region);
            hDesk.SaveChanges();

            return RedirectToAction("Index", new { msg = String.Format("Region {0} was removed.", region.name) });
        }
    }
}