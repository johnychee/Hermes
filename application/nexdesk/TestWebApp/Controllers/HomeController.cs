using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TestWebApp.Models;
using WebMatrix.WebData;
using PagedList;
using PagedList.Mvc;
namespace TestWebApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index(int? pageNew,int?pageAssigned,int? pageAccepted,int? pageNotDone,int? pageDone,int? pageAll)
        {
            // show each user his own index
            if (User.Identity.IsAuthenticated)
            {
                HermesDBEntities hDesk = new HermesDBEntities();
                var current_user = hDesk.Users.SingleOrDefault(u => u.id == WebSecurity.CurrentUserId);
                if (current_user.webpages_Roles.FirstOrDefault(r => r.RoleName == "SuperUser") != null)
                {
                    ViewBag.NewTravelEvents = hDesk.TravelEvents;
                    ViewBag.NewIssuesCount = hDesk.TravelEvents.Count();

                    ViewBag.DiscountedTravelEvents = hDesk.TravelEvents; ;
                    ViewBag.DiscountedTravelEventsCount = 1;

                    return View("IndexSuperUser");
                }
                if (current_user.webpages_Roles.FirstOrDefault(r => r.RoleName == "Support") != null 
                    && current_user.webpages_Roles.FirstOrDefault(r => r.level == 1) != null)
                {
                    ViewBag.NewTravelEvents = hDesk.TravelEvents;
                    ViewBag.NewIssuesCount = hDesk.TravelEvents.Count();

                    ViewBag.DiscountedTravelEvents = hDesk.TravelEvents;;
                    ViewBag.DiscountedTravelEventsCount = 1;

                    return View("IndexSupport");
                }
                return View("IndexSupport");
            }
            // not logged users
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
    }
}