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
                HelpDeskEntities hDesk = new HelpDeskEntities();
                var current_user = hDesk.Users.SingleOrDefault(u => u.id == WebSecurity.CurrentUserId);
                if (current_user.webpages_Roles.FirstOrDefault(r => r.RoleName == "SuperUser") != null)
                {
                    ViewBag.NewIssues = Ticket.getNew(hDesk).OrderByDescending(t => t.createdAt).ToList().ToPagedList(pageNew ?? 1, 10);//newTickets
                    ViewBag.NewIssuesCount = Ticket.getNew(hDesk).Count();//newTicketsCount

                    int a = Ticket.getNotDone(hDesk).Count();
                    ViewBag.NotSolved = Ticket.getNotDone(hDesk).OrderByDescending(t => t.createdAt).ToList().ToPagedList(pageNotDone ?? 1, 10); //notDone tickets
                    ViewBag.NotSolvedCount = Ticket.getNotDone(hDesk).Count(); //COunt not done tickets

                    ViewBag.all = hDesk.Tickets.OrderByDescending(t => t.createdAt).ToList().ToPagedList(pageAll ?? 1, 10);//all tickets
                    ViewBag.allCount = hDesk.Tickets.Count();//count all tickets

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