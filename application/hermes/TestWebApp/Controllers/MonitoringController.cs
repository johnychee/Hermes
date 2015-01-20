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
            HermesDBEntities hDesk = new HermesDBEntities();
            ViewBag.Reports = hDesk.Reports;
            return View();
        }

        [Authorize]
        public ActionResult Create()
        {
            HermesDBEntities hDesk = new HermesDBEntities();
            ViewBag.Groups = hDesk.Groups;
            return View();
        }
        
        [HttpPost]
        [Authorize]
        public ActionResult Create(Report model, string groupName)
        {
            HermesDBEntities hDesk = new HermesDBEntities();
            User current_user = hDesk.Users.SingleOrDefault(u => u.id == WebSecurity.CurrentUserId);
            Group group = hDesk.Groups.FirstOrDefault(g => g.name == groupName);

            IEnumerable<TravelContract> contractsForPeriod = group.TravelContracts.Where(tc => tc.CreatedAt >= model.PeriodFrom && tc.CreatedAt <= model.PeriodTo);

            Report report = new Report();
            report.GeneratedAt = DateTime.Now;
            report.Group = group;
            report.PeriodFrom = model.PeriodFrom;
            report.PeriodTo = model.PeriodTo;
            report.AmountOfMoneyForPeriod = 522;

            int amount = 0;
            foreach (TravelContract travelContract in contractsForPeriod)
            {
                amount += travelContract.TravelEvent.Price*travelContract.Quantity;
            }
            report.AmountOfMoneyForPeriod = amount;
            hDesk.Reports.Add(report);
            hDesk.SaveChanges();
            return RedirectToAction("Index", new { msg = "Report úspěšně vygenerován" });
        }

        public ActionResult Details(int id, string msg = null)
        {
            ViewBag.Message = msg;
            HermesDBEntities hDesk = new HermesDBEntities();
            Report report = hDesk.Reports.SingleOrDefault(r => r.Id == id);
            ViewBag.ReportContractsCount =
                report.Group.TravelContracts.Count(tc => tc.CreatedAt >= report.PeriodFrom && tc.CreatedAt <= report.PeriodTo).ToString();
            return View(report);
        }
    }
}