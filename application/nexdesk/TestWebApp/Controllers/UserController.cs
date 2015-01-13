using CsvHelper;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using NexDesk.MailProvider;
using TestWebApp.Models;
using TestWebApp.Views.User.UserViewModel;
using WebMatrix.WebData;
using PagedList;
using PagedList.Mvc;
using System.Net.Mail;
namespace TestWebApp.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly NexDeskMailProvider _mailProvider = new NexDeskMailProvider();

        public NexDeskMailProvider MailProvider
        {
            get { return _mailProvider; }
           
        }

        public ActionResult Index(int? page,string msg = null)
        {
            ViewBag.Message = msg;
            HelpDeskEntities hDesk = new HelpDeskEntities();
            ViewBag.Users = hDesk.Users.orderByName().ToPagedList(page ?? 1, 10);
            return View();
        }

        public ActionResult ListRetailer(int? page)
        {
            ViewBag.Users = TestWebApp.Models.User.GetRetailers().OrderBy(u => u.id).ToPagedList(page ?? 1, 10);
            return View("Index");
        }
        public ActionResult ListCustomerGroupRetailer(int id, int? page)
        {
            ViewBag.Users = TestWebApp.Models.User.GetCustomerGroupRetailers(id).OrderBy(u => u.id).ToPagedList(page ?? 1, 10);
            return View("Index");
        }

        public ActionResult Create(string msg = null)
        {
            ViewBag.Message = msg;
            ViewBag.FormStart = "true";//we now that the form is in the inicall state
            return View();
        }
        [HttpPost]
        public ActionResult Create(RegisterViewModel model,string Submit, HttpPostedFileBase attachedFile, FormCollection fc, string msg = null)
        {
            ViewBag.Message = msg;
            HelpDeskEntities hDesk = new HelpDeskEntities();
            BlobStorageService bss = new BlobStorageService();
            bool isOkToUpload = false;

            //#########################################################################################################
            //####################IF   USER    CLICKS    TO     REGISTER      AS     SOLUTIONIST#######################
            if (Submit == "RegisterSolutionist")
            {
                if (attachedFile != null && attachedFile.ContentLength < 1000000)//upload the picture,check the extionsion later!
                {
                    isOkToUpload = true;
                }
                else if (attachedFile != null)
                {
                    if (attachedFile.ContentLength > 1000000)
                        ModelState.AddModelError("", "File size must be less than 1MB");
                }
                if (hDesk.Users.Where(u => u.username == model.rSolutionistViewModel.username).Count() != 0)
                    ModelState.AddModelError("", "Username is already taken");

                if (ModelState.IsValid)
                {
                    webpages_Roles Role = hDesk.webpages_Roles.SingleOrDefault(r => r.RoleId == model.rSolutionistViewModel.roleId);
                    if (isOkToUpload)//if is is ok to uoload,upload picture and create user
                    {
                        CloudBlobContainer blobContainer = bss.GetBlobContainer();
                        CloudBlockBlob blob = blobContainer.GetBlockBlobReference(WebSecurity.CurrentUserId + model.rSolutionistViewModel.username + attachedFile.FileName);
                        blob.UploadFromStream(attachedFile.InputStream);
                        model.rSolutionistViewModel.profilePictureUrl = blob.Uri.ToString();

                        WebSecurity.CreateUserAndAccount(model.rSolutionistViewModel.username, model.rSolutionistViewModel.Password,
                        new
                        {
                            regionId = model.rSolutionistViewModel.regionId,
                            phone = model.rSolutionistViewModel.phone,
                            email = model.rSolutionistViewModel.email,
                            profilePictureUrl = model.rSolutionistViewModel.profilePictureUrl,
                            isSolutionist = true
                        });
                    }
                    else//if no picture
                    {
                        WebSecurity.CreateUserAndAccount(model.rSolutionistViewModel.username, model.rSolutionistViewModel.Password,
                        new
                        {
                            regionId = model.rSolutionistViewModel.regionId,
                            phone = model.rSolutionistViewModel.phone,
                            email = model.rSolutionistViewModel.email,
                            isSolutionist = true
                        });
                    }
                    User registeredUser = hDesk.Users.SingleOrDefault(u => u.username == model.rSolutionistViewModel.username);

                    //Now create the instance of Solutiuonist and gice the id for the user :-)
                    Solutionist solutionist = new Solutionist()
                    {
                        User = registeredUser,
                        companyId = model.rSolutionistViewModel.companyId,
                        teamviewerID = model.rSolutionistViewModel.teamViewerId == null ? "" : model.rSolutionistViewModel.teamViewerId.ToString(),
                        FirstName = model.rSolutionistViewModel.FirstName,
                        LastName = model.rSolutionistViewModel.LastName
                    };

                    hDesk.Solutionists.Add(solutionist); //Solutionist CREATED
                    hDesk.SaveChanges(); //save CHanges so that we have user  from created user before
                    webpages_Roles.AddUserToTheRole(registeredUser.id,Role.level); //add the role for user
                    registeredUser.isPasswordConfirmed = false; //passwordCOnfirmed set to false => in the overview will be the red alert so that user know the he/.she shoiudl change pass
                    // add user to group
                    foreach(Group group in Group.getWithName_onUserLvl("", registeredUser.id, false, hDesk))
                    {
                        if (fc["groups_" + group.id] != null)
                            registeredUser.Groups.Add(group);
                    }
                    hDesk.SaveChanges(); //savechanges again


                    return RedirectToAction("Index", new { msg = "The user[solutionist] has been successfully created" });
                }
                else
                {
                    ViewBag.IsNotValid = true;
                    ViewBag.Solutionist = "SolutionistHasModelError";
                    return View(model);
                }
            }
            //#########################################################################################################
            //####################IF   USER    CLICKS    TO     REGISTER      AS     RETAILER #######################
            if (Submit == "RegisterRetailer")
            {
                if (attachedFile != null && attachedFile.ContentLength < 1000000)//upload the picture,check the extionsion later!
                {
                    isOkToUpload = true;
                }
                else if (attachedFile != null)
                {
                    if (attachedFile.ContentLength > 1000000)
                        ModelState.AddModelError("", "File size must be less than 1MB");
                }
                if (hDesk.Users.Where(u => u.username == model.rRetailerViewModel.username).Count() != 0)
                    ModelState.AddModelError("", "Username is already taken");

                if (ModelState.IsValid)
                {
                    webpages_Roles Role = hDesk.webpages_Roles.SingleOrDefault(r => r.RoleName == "Retailer");
                    if (isOkToUpload)//if it is ok to upload,then upload picure and create user
                    {
                        CloudBlobContainer blobContainer = bss.GetBlobContainer();
                        CloudBlockBlob blob = blobContainer.GetBlockBlobReference(WebSecurity.CurrentUserId +model.rRetailerViewModel.username + attachedFile.FileName);
                        blob.UploadFromStream(attachedFile.InputStream);
                        model.rRetailerViewModel.profilePictureUrl = blob.Uri.ToString();

                        WebSecurity.CreateUserAndAccount(model.rRetailerViewModel.username, model.rRetailerViewModel.Password,
                        new
                        {
                            regionId = model.rRetailerViewModel.regionId,
                            phone = model.rRetailerViewModel.phone,
                            email = model.rRetailerViewModel.email,
                            profilePictureUrl = model.rRetailerViewModel.profilePictureUrl
                        });
                    }
                    else//if no picture
                    {
                        WebSecurity.CreateUserAndAccount(model.rRetailerViewModel.username, model.rRetailerViewModel.Password,
                           new
                           {
                               regionId = model.rRetailerViewModel.regionId,
                               phone = model.rRetailerViewModel.phone,
                               email = model.rRetailerViewModel.email
                           });
                    }
                    User registeredUser = hDesk.Users.SingleOrDefault(u => u.username == model.rRetailerViewModel.username);
                    registeredUser.isPasswordConfirmed = false; //passwordCOnfirmed set to false => in the overview will be the red alert so that user know the he/.she shoiudl change pass
                    Roles.AddUserToRole(model.rRetailerViewModel.username, Role.RoleName); //add the role for the user
                    //Now create the instance of Retailer and give the id for the user :-)
                    Retailer retailer = new Retailer();
                    retailer.User = registeredUser;
                    retailer.adress = model.rRetailerViewModel.address;
                    retailer.city = model.rRetailerViewModel.City;
                    retailer.contactId = model.rRetailerViewModel.contactId;
                    retailer.dealerId = model.rRetailerViewModel.dealerId;
                    retailer.name = model.rRetailerViewModel.Name;
                    retailer.wsId = model.rRetailerViewModel.workStationId;
                    retailer.zipcode = model.rRetailerViewModel.zipcode;

                    hDesk.Retailers.Add(retailer); //Solutionist CREATED
                    hDesk.SaveChanges(); //save CHanges so that we have user  from created user before


                    return RedirectToAction("Index", new { msg = "The user[retailer] has been successfully created" });
                }
                else
                {
                    ViewBag.IsNotValid = true;
                    return View(model);
                }
            }

            return View(model);

        }

        public ActionResult Details(int id, string msg = null)
        {
            ViewBag.Message = msg;
            HelpDeskEntities hDesk = new HelpDeskEntities();
            User user = hDesk.Users.Single(u => u.id == id);

            return View(user);
        }

        public ActionResult Edit(int id, string msg = null)
        {
            ViewBag.Message = msg;
            HelpDeskEntities hDesk = new HelpDeskEntities();
            User current_user = hDesk.Users.SingleOrDefault(u => u.id == WebSecurity.CurrentUserId);
            //if the logged user is not a SuperUSer or hes/her id is not equal to id in the url,then return error
            if (current_user.id != id && current_user.webpages_Roles.First().RoleName != "SuperUser")
            {
                return RedirectToAction("NotFound", "Errors");
            }
            ViewBag.UserId = id;
            return View();
        }

        [HttpPost]
        public ActionResult Edit(int id, EditViewModel model,string Submit,FormCollection fc, HttpPostedFileBase attachedFile, string msg = null)
        {
            HelpDeskEntities hDesk = new HelpDeskEntities();
            User current_user = hDesk.Users.SingleOrDefault(u=>u.id == WebSecurity.CurrentUserId);
            User user = hDesk.Users.Single(u => u.id == id); //get the USer we need to edit
            ViewBag.Message = msg;
            BlobStorageService bss = new BlobStorageService();
            string profileUrlNeedToChange = user.profilePictureUrl; //get the profileUrl of this user.
            bool profilePictureChanged = false;
            bool isOkToUpload = false;
            if (attachedFile != null && attachedFile.ContentLength < 1000000)//upload the picture,check the extionsion later!
            {
                isOkToUpload = true;
            }
            if (attachedFile != null && attachedFile.ContentLength > 1000000)
            {
                ModelState.AddModelError("", "File size must be less than 1MB");
            }
       
        //#######################################################################################################################
        //#######################################################################################################################
        //####################IF    USER   IS     A     SOLUTIONIST ##############################################################
      //##################################################################################################################

            if (Submit == "UpdateSolutionist")
            {
                // username changed to already taken username
                User userWithName = hDesk.Users.SingleOrDefault(u => u.username == model.eSolutionistViewModel.username);
                if (userWithName != null && userWithName.id != id)
                    ModelState.AddModelError("", "Username is already taken");
                if(current_user.webpages_Roles.FirstOrDefault(r=>r.RoleName == "SuperUser") != null)
                { 
                    // suport groups with the same level like user roles
                    foreach (Group group in Group.getWithName_onUserLvl("", user.id, false, hDesk))
                    {
                        // je v databázi přiřazen uživatel ke skupině?
                        bool inDatabase = group.Users.SingleOrDefault(u => u.id == user.id) != null;
                        // je ve formuláři přiřazen uživatel ke skupině?
                        bool input = fc["groups_" + group.id] != null;

                        // co se změnilo
                        if (inDatabase != input)
                        {
                            // přidal
                            if (input)
                                group.Users.Add(user);
                            // odebral
                            else
                                group.Users.Remove(user);
                        }
                    }
                }
               
                    // other attr
                if (ModelState.IsValid)
                {
                    if (model.eSolutionistViewModel.OldPassword != null) //change password for user
                    {
                        bool changed = WebSecurity.ChangePassword(user.username, model.eSolutionistViewModel.OldPassword, model.eSolutionistViewModel.NewPassword);

                        if (!changed)
                        {
                            ModelState.AddModelError("", "Please check again your old password,it is incorrect");
                            ViewBag.IsNotValid = true;
                            ViewBag.userId = user.id;
                            return View();
                        }
                        else
                        {
                            user.isPasswordConfirmed = true;
                        }
                    }

                  
                    CloudBlobContainer blobContainer = bss.GetBlobContainer();//get the container
                    if (isOkToUpload)//if it is ok to uplload we will upload the picture ,se the profilePictureCHanged to TRUE
                    {//and change the picture for our MODEL.
                        CloudBlockBlob blob = blobContainer.GetBlockBlobReference(user.id + current_user.username + current_user.id + attachedFile.FileName); //name + date for uniqe
                        blob.UploadFromStream(attachedFile.InputStream);
                        model.eSolutionistViewModel.profilePictureUrl = blob.Uri.ToString(); //UPLOAD the file and change the profilePictureChanged to true
                        profilePictureChanged = true; //now  we know that the profile picture was changed
                        user.profilePictureUrl = model.eSolutionistViewModel.profilePictureUrl; //set for ther user
                    }
                    //if he actually has a profile picture and the New picture was UPLOADED => we need to delete the old picture
                    if (profileUrlNeedToChange != null && profilePictureChanged)
                    {
                        CloudBlockBlob blob = blobContainer.GetBlockBlobReference(attachedFile.FileName);
                        //Delete old picture from blob container
                        string fileName = Path.GetFileName(profileUrlNeedToChange);
                        blob = blobContainer.GetBlockBlobReference(fileName); // get the blob from given fileName
                        blob.Delete(); //delete from storage
                    }

                    bool usernameChanged = user.username != model.eSolutionistViewModel.username ? true : false;
                    user.username = model.eSolutionistViewModel.username; //SET CHANGES
                    user.email = model.eSolutionistViewModel.email;
                    user.phone = model.eSolutionistViewModel.phone;
                    user.regionId = model.eSolutionistViewModel.regionId;


                    user.Solutionist.companyId = model.eSolutionistViewModel.companyId;
                    user.Solutionist.FirstName = model.eSolutionistViewModel.FirstName;
                    user.Solutionist.LastName = model.eSolutionistViewModel.LastName;
                    user.Solutionist.teamviewerID = model.eSolutionistViewModel.teamViewerId;
                    hDesk.SaveChanges(); //save changes

                    if (usernameChanged)
                        return RedirectToAction("Login", "Account");
                    return RedirectToAction("Details", new { @id = id, @msg = "The profile has been updated" });
                }
                else
                {
                    ViewBag.IsNotValid = true;
                    ViewBag.userId = user.id;
                    return View();
                }
            }
            //#######################################################################################################################
            //#######################################################################################################################
            //####################IF    USER   IS     A     RETAILER ##############################################################
            //##################################################################################################################
            if (Submit == "UpdateRetailer")
            {
                // username changed to already taken username
                User userWithName = hDesk.Users.SingleOrDefault(u => u.username == model.eRetailerViewModel.username);
                if (userWithName != null && userWithName.id != id)
                    ModelState.AddModelError("", "Username is already taken");

                if (ModelState.IsValid)
                {
                    if (model.eRetailerViewModel.OldPassword != null) //change password for user
                    {
                        bool changed = WebSecurity.ChangePassword(user.username, model.eRetailerViewModel.OldPassword, model.eRetailerViewModel.NewPassword);

                        if (!changed)
                        {
                            ModelState.AddModelError("", "Please check again your old password,it is incorrect");
                            ViewBag.IsNotValid = true;
                            ViewBag.userId = user.id;
                            return View();
                        }
                        else
                        {
                            user.isPasswordConfirmed = true;
                        }
                    }
                    CloudBlobContainer blobContainer = bss.GetBlobContainer();//get the container
                    if (isOkToUpload)//if it is ok to uplload we will upload the picture ,se the profilePictureCHanged to TRUE
                    {//and change the picture for our MODEL.
                        CloudBlockBlob blob = blobContainer.GetBlockBlobReference(user.id + current_user.username + current_user.id + attachedFile.FileName); //name + date for uniqe
                        blob.UploadFromStream(attachedFile.InputStream);
                        model.eRetailerViewModel.profilePictureUrl = blob.Uri.ToString(); //UPLOAD the file and change the profilePictureChanged to true
                        profilePictureChanged = true; //now  we know that the profile picture was changed
                        user.profilePictureUrl = model.eRetailerViewModel.profilePictureUrl; //set for ther user
                    }
                    //if he actually has a profile picture and the New picture was UPLOADED => we need to delete the old picture
                    if (profileUrlNeedToChange != null && profilePictureChanged)
                    {
                        CloudBlockBlob blob = blobContainer.GetBlockBlobReference(attachedFile.FileName);
                        //Delete old picture from blob container
                        string fileName = Path.GetFileName(profileUrlNeedToChange);
                        blob = blobContainer.GetBlockBlobReference(fileName); // get the blob from given fileName
                        blob.Delete(); //delete from storage
                    }
                    
                    bool usernameChanged = user.username != model.eRetailerViewModel.username ? true : false;
                    user.username = model.eRetailerViewModel.username; //SET CHANGES
                    user.email = model.eRetailerViewModel.email;
                    user.phone = model.eRetailerViewModel.phone;
                    user.regionId = model.eRetailerViewModel.regionId;
                    
                    user.Retailer.adress = model.eRetailerViewModel.address;
                    user.Retailer.city = model.eRetailerViewModel.City;
                    user.Retailer.contactId = model.eRetailerViewModel.contactId;
                    user.Retailer.dealerId = model.eRetailerViewModel.dealerId;
                    user.Retailer.name = model.eRetailerViewModel.Name;
                    user.Retailer.wsId = model.eRetailerViewModel.workStationId;
                    user.Retailer.zipcode = model.eRetailerViewModel.zipcode;

                    hDesk.SaveChanges(); //save changes
                    if(usernameChanged)
                        return RedirectToAction("Login", "Account");
                    return RedirectToAction("Details", new { @id = id, @msg = "The profile has been updated" });
                }
                else
                {
                    ViewBag.IsNotValid = true;
                    ViewBag.userId = user.id;
                    return View();
                }
            }
            return RedirectToAction("Details", new { @id = id});
        }

        [Authorize(Roles = "SuperUser")]
        public ActionResult Destroy(int id)
        {
            HelpDeskEntities hDesk = new HelpDeskEntities();

            // inverse IsConfirmed
            webpages_Membership membership = hDesk.webpages_Membership.Single(m => m.UserId == id);
            membership.IsConfirmed = !membership.IsConfirmed;
            hDesk.SaveChanges();

            return RedirectToAction("Index");
        }
        public JsonResult Get(string term)
        {
            HelpDeskEntities hDesk = new HelpDeskEntities();
            List<string> users = 
                hDesk.Users
                    .Where(u => (u.isSolutionist && (u.Solutionist.FirstName.Contains(term) || u.Solutionist.LastName.Contains(term))) || u.Retailer.name.Contains(term))
                    .Select(u => u.isSolutionist ? u.Solutionist.FirstName + " " + u.Solutionist.LastName : u.Retailer.name).ToList();
            return Json(users, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Exists(string username)
        {
            HelpDeskEntities hDesk = new HelpDeskEntities();
            bool valid = hDesk.Users.SingleOrDefault(u => u.username == username) != null;
            return Json(valid, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getFiltered(int? Region = null, bool? UserType = null) // retailer - true, solutionist - false
        {
            HelpDeskEntities hDesk = new HelpDeskEntities();
            var users =
                TestWebApp.Models.User.getNonsystemUser(hDesk)
                    .Where(u => (Region == null || u.regionId == Region) && (UserType == null || u.isSolutionist == UserType))
                    .orderByName()
                    .Select(u => new { @id = u.id, @name = u.isSolutionist ? u.Solutionist.FirstName + " " + u.Solutionist.LastName : u.Retailer.name });
            return Json(users, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getFilteredRetailers(int? region = null)
        {
            HelpDeskEntities hDesk = new HelpDeskEntities();
            var retailers = hDesk.Retailers
                .Where(r => (region == null || r.User.regionId == region))
                .OrderBy(r => r.name)
                .Select(r => new { @id = r.userId, @name = r.name });
            return Json(retailers, JsonRequestBehavior.AllowGet);
        }

        //##########################IMPORT USERS ####################################################
        [Authorize(Roles = "SuperUser")]
        public ActionResult Import()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "SuperUser")]
        public ActionResult Import(HttpPostedFileBase file)
        {
            if (file == null) //if nothing was choosed, return view and alert
            {
                ViewBag.Error = "Please choose the csv file before importing!";
                return View();
            }
            List<UserCSV> users = new List<UserCSV>();
            try
            {
                if (file.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    //path = Server.MapPath("~/Content/Upload/" + fileName); //we wont upload it to the server!
                    //file.SaveAs(path);
                    var csv = new CsvReader(new StreamReader(file.InputStream));
                    string extension = Path.GetExtension(file.FileName);
                    if (extension != ".csv")
                    {
                        ViewBag.Error = "Please choose csv file";
                        return View();
                    }
                    users = csv.GetRecords<UserCSV>().ToList(); //read the csv and send it to our List of userCSV.
                    //now we need to add to db 
                    HelpDeskEntities hDesk = new HelpDeskEntities();
                    int line = 0;
                    foreach (UserCSV user in users)
                    { //##################### VALIDATING USERS IN CSV ###################
                        line += 1;
                        User newUser = hDesk.Users.SingleOrDefault(usn => usn.username == user.UserName);
                        if (newUser != null) //if the user is already in the db,we cant import
                        {
                            ModelState.AddModelError("", "Error in line [" + line + "] User " + user.UserName + " was not added because this username is already in the database");
                        }
                        if (user.UserName.Length <= 0 || user.UserName.Length > 50)
                        {
                            ModelState.AddModelError("", "Error in line [" + line + "] User " + user.UserName + " was not added because the length of username must be < 0 and > 50");
                        }
                        if (user.FirstName.Length <= 0 || user.FirstName.Length > 50)
                        {
                            ModelState.AddModelError("", "Error in line [" + line + "] User " + user.UserName + " was not added because the length of first name must be < 0 and > 50");
                        }
                        if (user.LastName.Length <= 0 || user.FirstName.Length > 50)
                        {
                            ModelState.AddModelError("", "Error in line [" + line + "] User " + user.UserName + " was not added because the length of last name must be < 0 and > 50");
                        }
                        //########## VALIDATE GROUPS IN CSV ############
                        if (!String.IsNullOrEmpty(user.Groups)) //for examp. [1,2,3]
                        {
                            string str = user.Groups.Replace('[', ' ').Replace(']', ' '); //remove [ and ] char
                            string[] groups = str.Split(';');
                            for (int i = 0; i < groups.Count(); i++)
                            {
                                int groupId = Convert.ToInt32(groups[i]);
                                Group sg = hDesk.Groups.SingleOrDefault(s => s.id == groupId);
                                if (sg != null && sg.level != (int)Char.GetNumericValue(user.Role[user.Role.Length - 1])) //if the support group like that exists
                                {//if the level are different => throw error
                                    ModelState.AddModelError("", "Error in line [" + line + "] User " + user.UserName + " was not added to the group " + sg.name + " because this Group does not exist or the support level of the group and user is different");
                                }
                            }
                        }
                    }

                    if (ModelState.IsValid)
                    {
                        foreach (UserCSV user_ in users)
                        {
                            WebSecurity.CreateUserAndAccount(user_.UserName, user_.Password,
                              new
                              {
                                  FirstName = user_.FirstName,
                                  LastName = user_.LastName,
                                  Email = user_.Email,
                                  Mobile = user_.Mobile,
                                  SupportLevel = user_.Role.StartsWith("Support") ? (int)Char.GetNumericValue(user_.Role[user_.Role.Length - 1]) : 0
                              });
                            Roles.AddUserToRole(user_.UserName, user_.Role);
                            User u = hDesk.Users.SingleOrDefault(us => us.username == user_.UserName);
                            if (!String.IsNullOrEmpty(user_.Groups)) //for examp. [1,2,3]
                            {
                                string str = user_.Groups.Replace('[', ' ').Replace(']', ' '); //remove [ and ] char
                                string[] groups = str.Split(';');
                                for (int i = 0; i < groups.Count(); i++)
                                {
                                    int groupId = Convert.ToInt32(groups[i]);
                                    Group sg = hDesk.Groups.SingleOrDefault(s => s.id == groupId);
                                    if (sg != null && sg.level == u.webpages_Roles.First().level) //if the support group like that exists
                                    {//and if the supportgrup level is = to user spp level
                                        sg.Users.Add(u);
                                        hDesk.SaveChanges();
                                        ViewBag.Success = "The file has been successfully imported!";
                                    }
                                    else
                                    {
                                        ViewBag.Error = "The user " + u.username + " was not added to the group " + sg.name + " because this Group does not exist or the support level of the group and user is different";
                                    }
                                }
                            }
                        }
                    }


                }
                else
                {
                    ViewBag.Error = "The file given has 0 byte.please choose another file";
                }

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return View();
        }

        //###################################### EXPORT USERS##############################################
        [Authorize(Roles = "SuperUser")]
        public ActionResult Export()
        {
            HelpDeskEntities hDesk = new HelpDeskEntities();
            return View(hDesk.Users);
        }
        [Authorize(Roles = "SuperUser")]
        public void ExportToCSV()
        {
            HelpDeskEntities hDesk = new HelpDeskEntities();
            StringWriter sWriter = new StringWriter();
            sWriter.WriteLine("UserId,UserName,FirstName,LastName,Mobile,Email,SupportLevel");
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=Exported_Users.csv");
            Response.ContentType = "text/csv";
            Response.ContentEncoding = Encoding.Default;
            foreach (User user in hDesk.Users)
            {
                sWriter.WriteLine("{0},{1},{2},{3}", user.id, user.username, user.phone, user.email);
            }
            Response.Write(sWriter.ToString());
            Response.End();
        }


        [AllowAnonymous]
        //FORGOT PASWORD FUNCTION
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(string UserName)
        {
            //check user existance
            var user = Membership.GetUser(UserName);
            if (user == null)
            {
                TempData["Message"] = "User Not exist.";
            }
            else
            {
                //generate password token
                var token = WebSecurity.GeneratePasswordResetToken(UserName);
                //create url with above token
                var resetLink = "<a href='" + Url.Action("ResetPassword", "User", new { un = UserName, rt = token }, "http") + "'>Follow this link</a>";
                //get user emailid
                HelpDeskEntities hDesk = new HelpDeskEntities();
                var email = (from i in hDesk.Users
                             where i.username == UserName
                             select i.email).FirstOrDefault();
                //send mail
                try
                {
                    SendForgotPasswordMailWithResetLink(email, resetLink);
                    TempData["Message"] = "Mail Sent.";
                }
                catch (Exception ex)
                {
                    TempData["Message"] = "Error occured while sending email." + ex.Message;
                }
                //only for testing
                TempData["Message"] = "The link has been sent to your email,after clicking to the link given your new password will be generated.";
            }

            return View();
        }

        [Authorize(Roles="SuperUser")]
        public ActionResult GeneratePassToken(string UserName)
        {
            HelpDeskEntities hDesk = new HelpDeskEntities();
            //check user existance
            var user = Membership.GetUser(UserName);
            User userEditing = hDesk.Users.SingleOrDefault(u => u.username == UserName);
            if(userEditing == null)
                return RedirectToAction("NotFound","Errors");
            User current_user = hDesk.Users.SingleOrDefault(us => us.id == WebSecurity.CurrentUserId);
                //generate password token
                var token = WebSecurity.GeneratePasswordResetToken(UserName);
                //create url with above token
                var resetLink = "<a href='" + Url.Action("ResetPassword", "User", new { un = UserName, rt = token }, "http") + "'>Follow this link</a>";
                //get user emailid
               
                //send mail
                try
                {
                    SendForgotPasswordMailWithResetLink(userEditing.email, resetLink);
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Edit/" + userEditing.id, new { @msg = "Error occured while sending email." + ex.Message });
                }
                //only for testing


                return RedirectToAction("Edit/" + userEditing.id, new { @msg = "Password Token has been successfully generated,please check your email to know what to do next."});
        }

        private void SendForgotPasswordMailWithResetLink(string emailid, string resetLink)
        {
            MailTemplatesProcessor processor = new MailTemplatesProcessor();
            Message message = processor.CreateMailMessageForgotPassword(resetLink);
            message.Recipients.Add(emailid);
            MailProvider.SendMail(message);
        }

        private void SendMailWithNewPassword(string emailid, string newPassword)
        {
            MailTemplatesProcessor processor = new MailTemplatesProcessor();
            Message message = processor.CreateMailMessageNewPassword(newPassword);
            message.Recipients.Add(emailid);
            MailProvider.SendMail(message);
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string un, string rt)
        {
            HelpDeskEntities hDesk = new HelpDeskEntities();            //TODO: Check the un and rt matching and then perform following
            //get userid of received username
            var userid = (from i in hDesk.Users
                          where i.username == un
                          select i.id).FirstOrDefault();
            User userEditing = hDesk.Users.SingleOrDefault(u => u.id == userid);
            //check userid and token matches
            bool any = (from j in hDesk.webpages_Membership
                        where (j.UserId == userid)
                        && (j.PasswordVerificationToken == rt)
                        //&& (j.PasswordVerificationTokenExpirationDate < DateTime.Now)
                        select j).Any();

            if (any == true)
            {
                //generate random password
                string newpassword = GenerateRandomPassword(6);
                //reset password
                bool response = WebSecurity.ResetPassword(rt, newpassword);
                if (response == true)
                {
                    //get user emailid to send password
                    var email = (from i in hDesk.Users
                                   where i.username == un
                                   select i.email).FirstOrDefault();
                    //send email
                    try
                    {
                        SendMailWithNewPassword(email, newpassword);
                        userEditing.isPasswordConfirmed = false;
                        TempData["Message"] = "Please check your email,the newly generated has been sent to you.";
                    }
                    catch (Exception ex)
                    {
                        TempData["Message"] = "Error occured while sending email." + ex.Message;
                    }

                    //display message
                    TempData["Message"] = "Success!Please check email we sent to you and check the newly generated password";
                }
                else
                {
                    TempData["Message"] = "Hey, avoid random request on this page.";
                }
            }
            else
            {
                TempData["Message"] = "Username and token not maching.";
            }

            return View();
        }

        private string GenerateRandomPassword(int length)
        {
            string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-*&#+";
            char[] chars = new char[length];
            Random rd = new Random();
            for (int i = 0; i < length; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }
            return new string(chars);
        }


    }
}