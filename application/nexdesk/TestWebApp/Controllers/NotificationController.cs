using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestWebApp.Models;
using WebMatrix.WebData;
using PagedList;
using PagedList.Mvc;
namespace TestWebApp.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        public ActionResult Index(int? page)
        {
            ViewBag.Notifications = Notification.GetAllsortedByCreatedAt(WebSecurity.CurrentUserId).ToPagedList(page ?? 1,10);
            Notification.setAllReaded(WebSecurity.CurrentUserId);

            return View();
        }

        public JsonResult getNew(int id)
        {
            List<Dictionary<string, string>> newNotifications = new List<Dictionary<string, string>>();

            foreach(Notification notification in Notification.GetNew(WebSecurity.CurrentUserId, false))
            {
                Dictionary<string, string> JSONnotification = new Dictionary<string, string>();
                JSONnotification["Id"] = notification.Id.ToString();
                JSONnotification["ticketId"] = notification.TicketId.ToString();
                JSONnotification["NotificationType"] = notification.NotificationsType.Name;
                JSONnotification["Subject"] = notification.Subject;
                JSONnotification["CreatedAgo"] = notification.createdAgo;

                newNotifications.Add(JSONnotification);
            }

            return Json(newNotifications, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SetAllReaded(int id)
        {
            Notification.setAllReaded(WebSecurity.CurrentUserId);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}