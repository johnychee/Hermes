using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestWebApp.Models;

namespace TestWebApp.Controllers
{
    public class SearchController : Controller
    {
        //
        // POST: /Search/
        [HttpPost]
        [Authorize]
        public ActionResult GlobalSearch(FormCollection fc)
        {
            HelpDeskEntities hDesk = new HelpDeskEntities();
            string result = fc["tbSearch"];
            List<Ticket> foundedTickets = new List<Ticket>();

            if (!string.IsNullOrEmpty(result)) foundedTickets = hDesk.Tickets.Where(t => t.ticketId.Contains(result) ||
                                                                   t.subject.Contains(result) ||
                                                                   t.User.username.Contains(result)).ToList();

            ViewBag.FoundedTickets = foundedTickets;
            return View();
        }
    }
}