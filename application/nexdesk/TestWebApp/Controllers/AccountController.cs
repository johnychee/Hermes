using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using NexDesk.MailProvider;
using TestWebApp.Models;
using WebMatrix.WebData;

namespace TestWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly NexDeskMailProvider _mailProvider = new NexDeskMailProvider();

        public NexDeskMailProvider MailProvider
        {
            get { return _mailProvider; }
        }

        public ActionResult Index()
        {
            return View("Login");
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Login filledInfo,string ReturnUrl,string checkRememberMe)
        {
            if (ModelState.IsValid)
            {
                bool useCookie = checkRememberMe == "on";
                if (WebSecurity.Login(filledInfo.username, filledInfo.Password,useCookie))
                {
                    if (!string.IsNullOrEmpty(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                 
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "The username or password is invalid");
                }
            }
            return View(filledInfo);

        }

        public ActionResult Settings()
        {
            return View();
        }

        [Authorize(Roles="SuperUser")]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(Register model,string Role)
        {
            if(ModelState.IsValid)
            {
                WebSecurity.CreateUserAndAccount(model.username, model.Password,
                new
                {
                    phone = model.phone,
                    email = model.email
                });

                Roles.AddUserToRole(model.username, Role);
                return RedirectToAction("Login");
            }
            ModelState.AddModelError("", "Error ocurred during registrating process");
            return View(model);
        }
        public ActionResult Logout()
        {
            WebSecurity.Logout();
            return RedirectToAction("Login","Account");
        }
        public ActionResult UserDetails(int id)
        {
            HermesDBEntities hDesk = new HermesDBEntities();
            User u = hDesk.Users.Single(x => x.id == id);
            return View(u);
        }
        [Authorize(Roles = "SuperUser")]
        public ActionResult SuperUserPanel()
        {
            HermesDBEntities hDesk = new HermesDBEntities();
            return View(hDesk);
        }

     
        [HttpPost]
        [Authorize(Roles = "SuperUser")]
        public ActionResult FindUser(string UserName)
        {
            HermesDBEntities hDesk = new HermesDBEntities();
            if (hDesk.Users.SingleOrDefault(x => x.username == UserName) != null)
            {
                User u = hDesk.Users.Single(x => x.username == UserName);
                ViewData["UserName"] = u.username;
            }
            else
            {
                ViewBag.Error = "No user has been found,please try again!";
            }
            return View("SuperUserPanel",hDesk);
        }

        [Authorize(Roles = "SuperUser")]
        public ActionResult AddGroup()
        {
           return View();
        }
        [Authorize(Roles = "SuperUser")]
        [HttpPost]
        public ActionResult AddGroup(string AddGroup,string GroupLevel,IEnumerable<UsersViewModel> usersViewModel,string GroupName)
        {
            HermesDBEntities hDesk = new HermesDBEntities();
            List<UsersViewModel> model = new List<UsersViewModel>();
            if (GroupLevel != null)
            TempData["GroupLevel"] = GroupLevel;
            if (AddGroup == "ChooseSupportLvl")
            {
                foreach (User u in hDesk.Users)
                {
                    if (Roles.GetRolesForUser(u.username)[0] == "SupportLevel" + GroupLevel)
                    {
                        UsersViewModel usv = new UsersViewModel();
                        usv.Id = u.id;
                        usv.UserName = u.username;
                        usv.IsChecked = false;
                        model.Add(usv);
                    }
                }
            }
            if (AddGroup == "Create Group")
            {
                if (usersViewModel == null)
                {
                    ViewBag.Error1 = "Please select at least 2 supporters to this group,choose support level and click OK";
                    return View(model);
                }
                if (usersViewModel.Count(x => x.IsChecked) <= 1)
                    ViewBag.Error1 = "Please select at least 2 supporters to this group";
                if (string.IsNullOrEmpty(GroupName))
                    ViewBag.Error2 = "Please fill the name for the group";
                else
                {
                    Group sg = new Group();
                    sg.level = Convert.ToByte(TempData["GroupLevel"]);
                    sg.name = GroupName;
                    hDesk.Groups.Add(sg);
                    hDesk.SaveChanges();
                    
                    foreach (UsersViewModel usv in usersViewModel) 
                    { 
                        foreach(User u in hDesk.Users)
                        {
                            if (u.id == usv.Id && usv.IsChecked)
                            {
                                u.Groups.Add(sg);
                            }
                        }
                    }
                    hDesk.SaveChanges();
                    ViewBag.Success = "The support group was successfully created!";
                }

            }
                
            return View(model);

        }
    }
}