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
                if (current_user.webpages_Roles.FirstOrDefault(r => r.RoleName == "Retailer") != null)
                {
                    ViewBag.notSolvedIssues = Ticket.getMyNotDone(WebSecurity.CurrentUserId).OrderByDescending(t => t.createdAt).ToList().ToPagedList(pageNotDone ?? 1, 10);
                    ViewBag.notSolvedIssuesCount = Ticket.getMyNotDone(WebSecurity.CurrentUserId).Count();

                    ViewBag.solvedIssues = Ticket.getMyDone(WebSecurity.CurrentUserId).OrderByDescending(t => t.createdAt).ToList().ToPagedList(pageDone ?? 1, 10);
                    ViewBag.solvedIssuesCount = Ticket.getMyDone(WebSecurity.CurrentUserId).Count();
                    return View("IndexUser");
                }
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
                    ViewBag.NewIssues = hDesk.Tickets.Where(t => t.Status.name == "New").OrderByDescending(t => t.createdAt).ToList().ToPagedList(pageNew ?? 1, 10);//NEw tickets
                    ViewBag.NewIssuesCount = hDesk.Tickets.Where(t => t.Status.name == "New").Count();//NEw tickets count

                    ViewBag.AcceptedIssues = Ticket.getAccepted_byUser(current_user.id).OrderByDescending(t => t.createdAt).ToList().ToPagedList(pageAccepted ?? 1, 10);//accepted tickets
                    ViewBag.AcceptedIssuesCount = Ticket.getAccepted_byUser(current_user.id).Count();//accepted tickets count

                    if (Ticket.getAssigned_toUser(current_user.id).Count() == 0)
                    {
                      ViewBag.countAssignedToUserIssues = 0;
                    }

                    else
                    {
                      ViewBag.countAssignedToUserIssues = current_user.Tickets.Where(t => t.Status.name == "AssignedToUser" && t.solutionistId == current_user.id).Count();
                    }
                    ViewBag.AssignedToGroup = Ticket.getAssigned_toUserGroups(current_user).Where(t => t.Status.name != "Done").OrderByDescending(t => t.createdAt).ToList().ToPagedList(pageAssigned ?? 1, 10); //asigned to group if the user
                    ViewBag.AssignedToGroupCount = Ticket.getAssigned_toUserGroups(current_user).Where(t => t.Status.name != "Done").Count(); //countasigned to group if the user

                    return View("IndexSupport");
                }
                if (

                    (current_user.webpages_Roles.FirstOrDefault(r => r.RoleName == "Support") != null) 
                    && (current_user.webpages_Roles.FirstOrDefault(r => r.level == 2) != null
                    || current_user.webpages_Roles.FirstOrDefault(r => r.level == 3) != null
                    || current_user.webpages_Roles.FirstOrDefault(r => r.level == 4) != null)

                    )
                {
                    ViewBag.AcceptedIssues = Ticket.getAccepted_byUser(current_user.id).OrderByDescending(t => t.createdAt).ToList().ToPagedList(pageAccepted ?? 1, 10);
                    ViewBag.AcceptedIssuesCount = Ticket.getAccepted_byUser(current_user.id).Count();

                    if (current_user.Tickets.Where(t => t.Status.name == "AssignedToUser").Count() == 0)
                    {
                        ViewBag.countAssignedToUserIssues = 0;
                    }
                    else
                    {
                        ViewBag.countAssignedToUserIssues = current_user.Tickets.Where(t => t.Status.name == "AssignedToUser" && t.solutionistId == current_user.id).Count();
                    } 
                    ViewBag.AssignedToGroup = 0;
                    //count assigned to the group of users
                    ViewBag.AssignedToGroup = Ticket.getAssigned_toUserGroups(current_user).OrderByDescending(t => t.createdAt).ToList().ToPagedList(pageAssigned ?? 1, 10); //asigned to group if the user
                    ViewBag.AssignedToGroupCount = Ticket.getAssigned_toUserGroups(current_user).Count(); //asigned to group if the user

                    return View("IndexSupport");
                }
                // SupportLevel4
                return View("IndexSupport", hDesk);
            }
            // not logged users
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        public ActionResult NexDeskDashboard()
        {
            return View("NexDeskDashboard");
        }
    }
}