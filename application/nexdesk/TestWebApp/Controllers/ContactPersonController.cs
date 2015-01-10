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
    public class ContactPersonController : Controller
    {
        public ActionResult Index(int?page ,string msg=null)
        {
            HelpDeskEntities hDesk = new HelpDeskEntities();
            ViewBag.Message = msg;
            ViewBag.Contacts = hDesk.Contacts.OrderBy(c => c.lastName + c.firstName).ToPagedList(page ?? 1, 10);
            return View();
        }

        [Authorize(Roles = "SuperUser")]
        public ActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "SuperUser")]
        public ActionResult Create(Contact model)
        {
            HelpDeskEntities hDesk = new HelpDeskEntities();
            if (ModelState.IsValid)
            {
                Contact contact = new Contact()
                {
                    firstName = model.fName_,
                    lastName = model.lName_,
                    address = model.address_,
                    email = model.email_,
                    phone = model.phone_
                };
                hDesk.Contacts.Add(contact);
                hDesk.SaveChanges();
                return RedirectToAction("Details/" + contact.id, new { msg = "Contact person has been successfully created" });
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
            Contact contact = hDesk.Contacts.SingleOrDefault(c => c.id == id);
            if(contact == null)
                return RedirectToAction("NotFound" ,"Errors");
            return View(contact);
        }

        [Authorize(Roles = "SuperUser")]
        public ActionResult Edit(int id)
        {
            HelpDeskEntities hDesk = new HelpDeskEntities();
            Contact contact = hDesk.Contacts.SingleOrDefault(c => c.id == id);
            if (contact == null)
                return RedirectToAction("NotFound", "Errors");

            else
            {
                return View(contact);
            }
        }

        [HttpPost]
        public ActionResult Edit(Contact model)
        {
            if (ModelState.IsValid)
            {
                HelpDeskEntities hDesk = new HelpDeskEntities();
                Contact contact = hDesk.Contacts.SingleOrDefault(c => c.id == model.id);
                contact.firstName = model.fName_;
                contact.lastName = model.lName_;
                contact.address = model.address_;
                contact.email = model.email_;
                contact.phone = model.phone_;
                hDesk.SaveChanges();
                return RedirectToAction("Details/" + contact.id, new { msg = "Contact person has been successfully updated" });
            }
            else
            {
                ViewBag.IsNotValid = true;
                return View(model);  
            }
        }

        [Authorize(Roles = "SuperUser")]
        public ActionResult Delete(int id)
        {
            HelpDeskEntities hDesk = new HelpDeskEntities();
            Contact contact = hDesk.Contacts.SingleOrDefault(c => c.id == id);
             if (contact == null)
                return RedirectToAction("NotFound", "Errors");
            foreach (Retailer retail in hDesk.Retailers)
            {
                if (retail.contactId == id)
                {
                    retail.Contact = null;
                }
            }
            hDesk.Contacts.Remove(contact);
            hDesk.SaveChanges();
            return RedirectToAction("Index", new { msg = "Contact person has been successfully removed" });
        }
        
    }
}