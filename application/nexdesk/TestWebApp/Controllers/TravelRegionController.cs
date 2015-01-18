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
    public class TravelRegionController : Controller
    {
        public ActionResult Index(int? page,string msg = null)
        {
            ViewBag.Message = msg;
            HermesDBEntities hDesk = new HermesDBEntities();
            ViewBag.TravelRegions = hDesk.TravelRegions.OrderBy(r => r.Name).ToPagedList(page ?? 1, 10);
            return View();
        }
    }
}