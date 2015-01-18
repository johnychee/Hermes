using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestWebApp.Models;
using WebMatrix.WebData;

namespace TestWebApp.Controllers
{
    [Authorize]
    public class GroupController : Controller
    {
        public ActionResult Index(string msg = null)
        {
            ViewBag.Message = msg;
            HermesDBEntities hDesk = new HermesDBEntities();
            return View(hDesk.Groups.ToList());
        }

        [Authorize(Roles = "SuperUser")]
        public ActionResult Create(string msg = null)
        {
            ViewBag.Message = msg;
            Group group = new Group();
            return View(group);
        }
        [HttpPost]
        [Authorize(Roles = "SuperUser")]
        public ActionResult Create(Group model, string msg = null) //DO group model!!!!!
        {
            ViewBag.Message = msg;
            HermesDBEntities hDesk = new HermesDBEntities();

            try
            {
                if (ModelState.IsValid)
                {
                    hDesk.Groups.Add(model);
                    hDesk.SaveChanges();
                    return RedirectToAction("Details", new { @id = model.id });
                }
                else
                {
                    ViewBag.IsNotValid = true;
                    return View(model);
                }
            }
            catch (FormatException e)
            {
                ViewBag.message = e.Message;
                return View(model);
            }
        }

        public ActionResult Details(int id, string msg = null)
        {
            ViewBag.Message = msg;
            HermesDBEntities hDesk = new HermesDBEntities();
            Group group = hDesk.Groups.SingleOrDefault(g => g.id == id);
            if(group == null)
            {
                return RedirectToAction("NotFound", "Errors");
            }
            return View(group);
        }
        [HttpPost]
        [Authorize(Roles = "SuperUser")]
        public ActionResult Details(int id, FormCollection fc, string msg = null)
        {
            ViewBag.Message = msg;
            HermesDBEntities hDesk = new HermesDBEntities();
            var group = hDesk.Groups.Single(g => g.id == id);

            try
            {
                // name
                group.name = fc["name"];
                // users
                foreach (var user in TestWebApp.Models.User.inRole("SupportLevel", group.level, hDesk).ToList())
                {
                    bool inDatabase = group.Users.SingleOrDefault(u => u.id == user.id) != null;
                    bool input = fc["user_" + user.id] != "false";

                    if (inDatabase != input)
                    {
                        if (input)
                            group.Users.Add(user);
                        else
                            group.Users.Remove(user);
                    }
                }
              

                hDesk.SaveChanges();
                return RedirectToAction("Index", new { @id = id, @msg = "Group was successfully updated!" });
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
            }
            ViewBag.users = TestWebApp.Models.User.inRole("SupportLevel", group.level, hDesk);
            return View(group);
        }

        [Authorize(Roles = "SuperUser")]
        public ActionResult Remove(int id)
        {
            HermesDBEntities hDesk = new HermesDBEntities();
            Group group = hDesk.Groups.Single(g => g.id == id);
            List<User> users = group.Users.ToList();
            foreach (User user in users)
            {
                group.Users.Remove(user);
            }
            hDesk.Groups.Remove(group);
            hDesk.SaveChanges();
            return RedirectToAction("Index", new { msg = "Group was removed!" });
        }

        public JsonResult GetGroups_onSameLvl(int id, string term)
        {
            List<string> groups = Group.getWithName_onUserLvl(term, id, false).Select(g => g.name).ToList();
            return Json(groups, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Exists(int id, string name)
        {
            bool valid = Group.getWithName_onUserLvl(name, id, true).Count() > 0;
            return Json(valid, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getFiltered(int level = 0)
        {
            HermesDBEntities hDesk = new HermesDBEntities();
            var groups = hDesk.Groups
                .Where(g => level == 0 || g.level == level)
                .OrderBy(g => g.name)
                .Select(g => new { g.id, g.name });
            return Json(groups, JsonRequestBehavior.AllowGet);
        }

        public ActionResult List_My_Groups()
        {
            HermesDBEntities hDesk = new HermesDBEntities();
            User current_user = hDesk.Users.SingleOrDefault(u => u.id == WebSecurity.CurrentUserId);
            return View("Index",current_user.Groups.ToList());
        }
  
    }
}