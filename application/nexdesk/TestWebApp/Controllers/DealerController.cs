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
    public class DealerController : Controller
    {
        public ActionResult Index(int? page, string msg = null)
        {
            ViewBag.Message = msg;
            HelpDeskEntities hDesk = new HelpDeskEntities();
            ViewBag.Dealers = hDesk.Dealers.OrderBy(d => d.name).ToPagedList(page ?? 1, 10);
            return View();
        }

        [Authorize(Roles="SuperUser")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "SuperUser")]
        [HttpPost]
        public ActionResult Create(Dealer model, FormCollection fc)
        {
            if (ModelState.IsValid)
            {
                HelpDeskEntities hDesk = new HelpDeskEntities();
                foreach (string key in fc)
                {
                    if (key.StartsWith("customer_"))
                    {
                        int customerId = Convert.ToInt32(fc[key]);
                        model.Retailers.Add(hDesk.Retailers.SingleOrDefault(r => r.userId == customerId));
                    }
                }

                hDesk.Dealers.Add(model);
                hDesk.SaveChanges();
                return RedirectToAction("Details/" + model.id, new { msg = "Customer group has been successfully created" });
            }
            else
            {
                ViewBag.IsNotValid = true;
                return View(model);
            }
        }

        public ActionResult Details(int id,string msg = null)
        {
            HelpDeskEntities hDesk = new HelpDeskEntities();
            ViewBag.Message = msg;
            Dealer deal = hDesk.Dealers.SingleOrDefault(d => d.id == id);
            return View(deal);
        }

        [Authorize(Roles="SuperUser")]
        public ActionResult Edit(int id)
        {
            HelpDeskEntities hDesk = new HelpDeskEntities();
            Dealer deal = hDesk.Dealers.SingleOrDefault(d => d.id == id);
            return View(deal);
        }

        [HttpPost]
        public ActionResult Edit(int id, Dealer model, FormCollection fc)
        {
            HelpDeskEntities hDesk = new HelpDeskEntities();
            Dealer dealer = hDesk.Dealers.SingleOrDefault(d => d.id == id);
            if (dealer != null)
            {
                dealer.name = model.name;
                foreach(Retailer retailer in hDesk.Retailers)
                {
                    // retailer je odstraněn - je v db, není v fc
                    if (dealer.Retailers.Contains(retailer) && fc["customer_" + retailer.userId] == null)
                        dealer.Retailers.Remove(retailer);
                    // retailer je přidán - není v db, je v fc
                    else if (!dealer.Retailers.Contains(retailer) && fc["customer_" + retailer.userId] != null)
                        dealer.Retailers.Add(retailer);
                }
                hDesk.SaveChanges();
                return RedirectToAction("Details/" + dealer.id, "Dealer", new { msg = "Customer Group has been successfully updated" });
            }
            else
            {
                return RedirectToAction("NotFound", "Errors");
            }
        }
        

        [Authorize(Roles="SuperUser")]
        public ActionResult Delete(int id)
        {
            HelpDeskEntities hDesk = new HelpDeskEntities();
            Dealer deal = hDesk.Dealers.SingleOrDefault(d => d.id == id);

            if (deal != null)
            {
                hDesk.Dealers.Remove(deal);
                foreach (Retailer retail in hDesk.Retailers)
                {
                    if (retail.dealerId == deal.id)
                        retail.dealerId = null;
                }
                hDesk.SaveChanges();
                return RedirectToAction("Index", "Dealer", new { msg = "Customer Group has been successfully removed" });
            }
            else
            {
                return RedirectToAction("NotFound", "Errors");
            }
        }
    }
}