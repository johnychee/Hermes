using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using NexDesk.MailProvider;
using TestWebApp.Models;
using WebMatrix.WebData;
using PagedList;
using PagedList.Mvc;
namespace TestWebApp.Controllers
{
    [Authorize]
    public class TicketController : Controller
    {
        private readonly NexDeskMailProvider _mailProvider = new NexDeskMailProvider();

        public NexDeskMailProvider MailProvider
        {
            get { return _mailProvider; }
        }
      
        public ActionResult Index(int page = 1, string msg = null)
        {
            
            ViewBag.Message = msg;
            HelpDeskEntities hDesk = new HelpDeskEntities();
            User current_user = hDesk.Users.SingleOrDefault(u => u.id == WebSecurity.CurrentUserId);
            if (current_user.Retailer != null)
                return RedirectToAction("NotFound", "Errors");
            ViewBag.SubTitle = "All Tickets";
            ViewBag.Parents = new LinkedList<KeyValuePair<string, string>>();
            ViewBag.Parents.AddLast(new KeyValuePair<string, string>("Tickets", null));
            ViewBag.Tickets = hDesk.Tickets
                .filter(Request.Params)
                .Order(Request.Params)
                .ToList()
                .ToPagedList(page, 10);
            return View();
        }
        public ActionResult List_Own_Created(int page = 1, string msg = null)
        {
            ViewBag.Message = msg;

            ViewBag.SubTitle = "My Requests";
            ViewBag.Parents = new LinkedList<KeyValuePair<string, string>>();
            ViewBag.Parents.AddLast(new KeyValuePair<string, string>("Own", null));
            ViewBag.Tickets = Ticket.GetOwnTickets(WebSecurity.CurrentUserId)
                .filter(Request.Params)
                .Order(Request.Params)
                .ToPagedList(page, 10);
            return View("Index");
        }
        public ActionResult List_All_My_Own(int page = 1, string msg = null)
        {
            ViewBag.Message = msg;

            ViewBag.SubTitle = "All My Own";
            ViewBag.Parents = new LinkedList<KeyValuePair<string, string>>();
            ViewBag.Parents.AddLast(new KeyValuePair<string, string>("Own", null));
            ViewBag.Tickets = Ticket.GetAllMyOwn(WebSecurity.CurrentUserId)
                .filter(Request.Params)
                .Order(Request.Params)
                .ToPagedList(page, 10);
            return View("Index");
        }
        public ActionResult List_Not_Solved(int page = 1, string msg = null)
        {
            ViewBag.Message = msg;
            ViewBag.SubTitle = "Not Solved Yet";
            ViewBag.Parents = new LinkedList<KeyValuePair<string, string>>();
            ViewBag.Parents.AddLast(new KeyValuePair<string, string>("Not Solved", null));
            ViewBag.Tickets = Ticket.GetNotSolved(WebSecurity.CurrentUserId)
                .filter(Request.Params)
                .Order(Request.Params)
                .ToPagedList(page, 10);
            return View("Index");
        }
        public ActionResult List_Solved_By_Me(int page = 1, string msg = null)
        {
            ViewBag.Message = msg;

            ViewBag.SubTitle = "Solved by Me";
            ViewBag.Parents = new LinkedList<KeyValuePair<string, string>>();
            ViewBag.Parents.AddLast(new KeyValuePair<string, string>("Solved", null));
            ViewBag.Tickets = Ticket.GetSolvedByMe(WebSecurity.CurrentUserId)
                .filter(Request.Params)
                .Order(Request.Params)
                .ToPagedList(page, 10);
            return View("Index");
        }
        public ActionResult List_Created_by(int page = 1, string msg = null)
        {
            ViewBag.Message = msg;

            ViewBag.SubTitle = "Tickets Created for Others";
            ViewBag.Parents = new LinkedList<KeyValuePair<string, string>>();
            ViewBag.Parents.AddLast(new KeyValuePair<string, string>("Created by me", null));
            ViewBag.Tickets = Ticket.GetByCreatedByUserId(WebSecurity.CurrentUserId)
                .filter(Request.Params)
                .Order(Request.Params)
                .ToPagedList(page, 10);
            return View("Index");
        }
        public ActionResult List_New(int page = 1, string msg = null)
        {
            ViewBag.Message = msg;

            ViewBag.SubTitle = "New Tickets";
            ViewBag.Parents = new LinkedList<KeyValuePair<string, string>>();
            ViewBag.Parents.AddLast(new KeyValuePair<string, string>("New", null));
            ViewBag.Tickets = Ticket.getNew()
                .filter(Request.Params)
                .Order(Request.Params)
                .ToPagedList(page, 10);
            return View("Index");
        }

        public ActionResult List_Refused(int page = 1, string msg = null)
        {
            ViewBag.Message = msg;

            ViewBag.SubTitle = "Refused";
            ViewBag.Parents = new LinkedList<KeyValuePair<string, string>>();
            ViewBag.Parents.AddLast(new KeyValuePair<string, string>("Refused", null));
            ViewBag.Tickets = Ticket.getRefused()
                .filter(Request.Params)
                .Order(Request.Params)
                .ToPagedList(page, 10);
            return View("Index");
        }
        public ActionResult List_Accepted(int page = 1, string msg = null)
        {
            ViewBag.Message = msg;

            ViewBag.SubTitle = "Accepted";
            ViewBag.Parents = new LinkedList<KeyValuePair<string, string>>();
            ViewBag.Parents.AddLast(new KeyValuePair<string, string>("Accepted", null));
            ViewBag.Tickets = Ticket.getAccepted_byUser(WebSecurity.CurrentUserId)
                .filter(Request.Params)
                .Order(Request.Params)
                .ToPagedList(page, 10);
            return View("Index");
        }
        public ActionResult List_Assigned_To_Group(int page = 1, string msg = null)
        {
            ViewBag.Message = msg;

            ViewBag.SubTitle = "Assigned Tickets";
            ViewBag.Parents = new LinkedList<KeyValuePair<string, string>>();
            ViewBag.Parents.AddLast(new KeyValuePair<string, string>("Assigned", null));
            ViewBag.Tickets = Ticket.getAssigned_toUserGroups(WebSecurity.CurrentUserId)
                .Where(t => t.Status.name != "Done")
                .filter(Request.Params)
                .Order(Request.Params)
                .ToPagedList(page, 10);
            return View("Index");
        }
        public ActionResult List_Assigned_To_Group_Done(int page = 1, string msg = null)
        {
            ViewBag.Message = msg;

            ViewBag.SubTitle = "Done Tickets";
            ViewBag.Parents = new LinkedList<KeyValuePair<string, string>>();
            ViewBag.Parents.AddLast(new KeyValuePair<string, string>("Done", null));
            ViewBag.Tickets = Ticket.getAssigned_toUserGroups(WebSecurity.CurrentUserId)
                .Where(t => t.Status.name == "Done")
                .filter(Request.Params)
                .Order(Request.Params)
                .ToPagedList(page, 10);
            return View("Index");
        }
        public ActionResult List_Assigned_To_Group_Accepted(int page = 1, string msg = null)
        {
            ViewBag.Message = msg;

            ViewBag.SubTitle = "Accepted Tickets";
            ViewBag.Parents = new LinkedList<KeyValuePair<string, string>>();
            ViewBag.Parents.AddLast(new KeyValuePair<string, string>("Accepted", null));
            ViewBag.Tickets = Ticket.getAssigned_toUserGroups(WebSecurity.CurrentUserId)
                .Where(t => t.Status.name != "Accepted")
                .filter(Request.Params)
                .Order(Request.Params)
                .ToPagedList(page, 10);
            return View("Index");
        }
        public ActionResult List_Groups_Tickets(int group_id, int page = 1) //return all groups tickets
        {
            ViewBag.SubTitle = "All groups tickets";
            ViewBag.Parents = new LinkedList<KeyValuePair<string, string>>();
            ViewBag.Parents.AddLast(new KeyValuePair<string, string>("Groups Tickets", null));

            HelpDeskEntities hDesk = new HelpDeskEntities();
            ViewBag.Tickets = hDesk.Tickets
                .Where(t => t.groupId == group_id)
                .filter(Request.Params)
                .Order(Request.Params)
                .ToPagedList(page, 10);

            return View("Index");
        }
        public ActionResult List_Assigned_To_User(int page = 1, string msg = null)
        {
            ViewBag.Message = msg;

            ViewBag.SubTitle = "Assigned to me";
            ViewBag.Parents = new LinkedList<KeyValuePair<string, string>>();
            ViewBag.Parents.AddLast(new KeyValuePair<string, string>("Assigned", null));
            ViewBag.Tickets = Ticket.getAssigned_toUser(WebSecurity.CurrentUserId)
                .filter(Request.Params)
                .Order(Request.Params)
                .ToPagedList(page, 10);
            return View("Index");
        }

        public ActionResult List_notDone(int page = 1, string msg = null)
        {
            HelpDeskEntities hDesk = new HelpDeskEntities();
            User current_user = hDesk.Users.SingleOrDefault(u => u.id == WebSecurity.CurrentUserId);
            if (current_user.Retailer != null)
                return RedirectToAction("NotFound", "Errors");
            ViewBag.Message = msg;

            ViewBag.SubTitle = "In Progress";
            ViewBag.Parents = new LinkedList<KeyValuePair<string, string>>();
            ViewBag.Parents.AddLast(new KeyValuePair<string, string>("Not Done", null));
            ViewBag.Tickets = Ticket.getNotDone()
                .filter(Request.Params)
                .Order(Request.Params)
                .ToPagedList(page, 10);
            return View("Index");
        }

        public ActionResult List_My_Not_Done(int page = 1, string msg = null)//list all the tickets from the author,and the status is not done
        {
            ViewBag.Message = msg;

            ViewBag.SubTitle = "My Not Done";
            ViewBag.Parents = new LinkedList<KeyValuePair<string, string>>();
            ViewBag.Parents.AddLast(new KeyValuePair<string, string>("Not Done", null));

            ViewBag.Tickets = Ticket.getMyNotDone(WebSecurity.CurrentUserId)
                .filter(Request.Params)
                .Order(Request.Params)
                .ToPagedList(page, 10);

            return View("Index");
        }
        public ActionResult List_My_Done(int page = 1, string msg = null)
        {
            ViewBag.Message = msg;

            ViewBag.SubTitle = "My Solved Tickets";
            ViewBag.Parents = new LinkedList<KeyValuePair<string, string>>();
            ViewBag.Parents.AddLast(new KeyValuePair<string, string>("Done", null));
            ViewBag.Tickets = Ticket.getMyDone(WebSecurity.CurrentUserId)
                .filter(Request.Params)
                .Order(Request.Params)
                .ToPagedList(page, 10);
            return View("Index");
        }

        public ActionResult List_Done(int page = 1, string msg = null)
        {
            HelpDeskEntities hDesk = new HelpDeskEntities();
            User current_user = hDesk.Users.SingleOrDefault(u => u.id == WebSecurity.CurrentUserId);
            if (current_user.Retailer != null)
                return RedirectToAction("NotFound", "Errors");
            ViewBag.Message = msg;

            ViewBag.SubTitle = "Solved Tickets";
            ViewBag.Parents = new LinkedList<KeyValuePair<string, string>>();
            ViewBag.Parents.AddLast(new KeyValuePair<string, string>("Done", null));
            ViewBag.Tickets = Ticket.getDone()
                .filter(Request.Params)
                .Order(Request.Params)
                .ToPagedList(page, 10);
            return View("Index");
        }

        public ActionResult Create(string msg = null) //creating ticket controller
        {
            ViewBag.Message = msg;

            return View();
        }
        [HttpPost]
        public ActionResult Create(Ticket model, string msg = null)
        {
            ViewBag.Message = msg;
            HelpDeskEntities hDesk = new HelpDeskEntities();
            try
            {
                
                model.author_ = hDesk.Users.SingleOrDefault(u => u.id == model.authorId).fullName;
                if (ModelState.IsValid)
                {
                    // Ticket
                    model.Status = Status.New(hDesk);
                    model.createdAt = DateTime.UtcNow;
                    model.createdByUserId = WebSecurity.CurrentUserId;

                    hDesk.Tickets.Add(model);
                    hDesk.SaveChanges();

                    model.CreateBusinessTicketId(hDesk);

                    // TicketHistory
                    TicketHistory tHistory = new TicketHistory()
                    {
                        date = DateTime.UtcNow,
                        HistoryAction = HistoryAction.Created(hDesk),
                        Ticket = model,
                        User = hDesk.Users.SingleOrDefault(u => u.username == WebSecurity.CurrentUserName)
                    };
                    tHistory.text = "Ticket has been created by: " + WebSecurity.CurrentUserName + " for " + model.author_;
                    hDesk.TicketHistories.Add(tHistory);

                    // ticket SLA
                    TicketSla tSla = new TicketSla()
                    {
                        Ticket = model,
                        startAt = DateTime.UtcNow
                    };
                    tSla.RT_expiresAt = tSla.countEndAt(tSla.startAt, TimeSpan.FromMinutes(30));
                    tSla.FT_expiresAt = tSla.countEndAt(tSla.startAt, TimeSpan.FromHours(8));
                    tSla.FT3lvl_expiresAt = tSla.countEndAt(tSla.startAt, null);
                    hDesk.TicketSlas.Add(tSla);

                    // save all
                    hDesk.SaveChanges();

                    //Creates a notification
                    foreach (Group group in hDesk.Groups.Where(g => g.level == 1))
                    {
                        foreach (User user in group.Users)
                        {
                            NotificationsFacade.CreateTicketFlowNotification(user.id,
                                WebSecurity.CurrentUserId,
                                "New ticket for your group",
                                String.Format("New ticket for your group created by {0} at {1}", WebSecurity.CurrentUserName,
                                    DateTime.UtcNow.ToLocalTime()), model.id);
                        }
                    }

                    if (string.IsNullOrEmpty(model.author_))
                    {
                        return RedirectToAction("List_Own_Created", new { msg = "Ticket has been successfully created!" });
                    }
                    else
                    {
                        return RedirectToAction("List_Created_By", new { msg = "Ticket has been successfully created!" });
                    }
                }
                else
                {
                    ViewBag.IsNotValid = true;
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(model);
            }
        }

        public ActionResult Details(int id, int page = 1, int AttachmentPage = 1, string msg = null)
        {
            ViewBag.Message = msg;
            HelpDeskEntities hDesk = new HelpDeskEntities();

            Ticket ticket = hDesk.Tickets.Single(tick => tick.id == id);
            ViewBag.TicketPosts = ticket.TicketPosts.OrderByDescending(tp => tp.createdAt).ToPagedList(page, 4);
            ViewBag.TicketAttachments = ticket.Attachments.OrderByDescending(a => a.id).ToPagedList(AttachmentPage, 6);

            return View(ticket);
        }
        [HttpPost]
        public ActionResult Details(int id, string TextComment, string Submit, FormCollection fc, IEnumerable<HttpPostedFileBase> attachedFiles, int page = 1, int AttachmentPage = 1, string msg = null)
        {
            ViewBag.Message = msg;
            HelpDeskEntities hDesk = new HelpDeskEntities();
            Ticket ticket = hDesk.Tickets.Single(x => x.id == id);
            User current_user = hDesk.Users.Single(u => u.id == WebSecurity.CurrentUserId);
            ViewBag.TicketPosts = ticket.TicketPosts.OrderByDescending(tp => tp.createdAt).ToPagedList(page, 4);
            ViewBag.TicketAttachments = ticket.Attachments.OrderByDescending(a => a.id).ToPagedList(AttachmentPage, 6);
            if (Submit == "AcceptTicket") //when the supporter clicks to ACCEPT,this will happen
            {
                if (ticket.Status.name != "Accepted")
                {
                    if (ticket.TicketType == null && fc["Types"] == "")
                    {
                        ViewBag.Message = null;
                        ViewBag.Error = "Please choose the type of ticket before accepting,thanks";
                        return View(ticket);
                    }
                    else if (ticket.TicketType == null && fc["Types"] != "")
                    {
                        int type_id = Convert.ToInt32(fc["Types"]);
                        ticket.TicketType = hDesk.TicketTypes.SingleOrDefault(t => t.id == type_id);
                    }
                    if (fc["UsersGroupsLevel1"] != null && ticket.Status.name == "New")
                    {
                        int group_id = Convert.ToInt32(fc["UsersGroupsLevel1"]);
                        Group group = hDesk.Groups.SingleOrDefault(g => g.id == group_id);
                        ticket.Group = group; //set the griup for the user
                    }
                    if (fc["UsersGroupsLevel1"] == null && ticket.Status.name == "New")
                    {
                        ViewBag.Message = null;
                        ViewBag.Error = "Please choose the group before accepting this ticket,if you are not currently in any group,contact admin";
                        return View(ticket);
                    }

                    // ticket sla
                    var tSla = ticket.TicketSla;
                    if (ticket.Status == Status.New(hDesk))
                        tSla.RT_realCompletedAt = DateTime.UtcNow;

                    // update ticket
                    ticket.Status = Status.Accepted(hDesk);
                    ticket.solutionistId = current_user.id; //set the colutionist for the user

                    // ticket history
                    TicketHistory tHistory = new TicketHistory()
                    {
                        date = DateTime.UtcNow,
                        ticketId = id,
                        userId = current_user.id,
                        actionId = HistoryAction.Accepted(hDesk).id,
                        text = "Ticket has been accepted by " + current_user.fullName + (ticket.Group == null ? "" : " for group " + ticket.Group.name)
                    };
                    hDesk.TicketHistories.Add(tHistory);
                    hDesk.SaveChanges();

                    // generate TicketId
                    if (ticket.ticketId == null || ticket.ticketId.Contains("NT")) ticket.CreateBusinessTicketId(hDesk);

                    //Creates notification for author
                    NotificationsFacade.CreateTicketFlowNotification(ticket.authorId,
                            current_user.id,
                            "Ticket was accepted",
                            String.Format("Ticket was accepted by {0} at {1}", current_user.fullName, DateTime.UtcNow.ToLocalTime()), ticket.id);

                    //Create MailMessage for Ticket Accepted event
                    MailTemplatesProcessor mailProcessor = new MailTemplatesProcessor(ticket);
                    Message message = mailProcessor.CreateMailMessageTicketAccepted();
                    message.Recipients.Add(current_user.email);
                    MailProvider.SendMail(message);

                    ViewBag.Message = null;
                    ViewBag.Success = "You have successfully accepted this ticket";
                    return View(ticket);
                }
                else //if the ticket is already accepted, infor the supporter.
                {
                    ViewBag.Message = null;
                    ViewBag.Error = String.Format("You don't have permission to accept this ticket,it is already accepted by user[{0}]", ticket.Solutionist_.fullName);
                    return View(ticket);
                }

            }

            if (Submit == "Assign") //if user clicks to Assign button =>
            {
                if (fc["Groups"] == "")
                {
                    ViewBag.Message = null;
                    ViewBag.Error = "Please choose the group you want to assign this ticket to";
                    ViewBag.Posts = ticket.TicketPosts.ToList();
                    return View(ticket);
                }
                else
                {
                    int group_id = Convert.ToInt32(fc["Groups"]);
                    Group sg = hDesk.Groups.Single(g => g.id == group_id);
                    ticket.Group = sg;
                    ticket.Solutionist_ = null;
                    ticket.Status = hDesk.Status.SingleOrDefault(s => s.name == "AssignedToGroup"); //change the status of the ticket to Assigned
                    TicketHistory tHistory = new TicketHistory()
                    {
                        date = DateTime.UtcNow,
                        ticketId = id,
                        userId = current_user.id,
                        actionId = HistoryAction.AssignedToGroup(hDesk).id,
                        text = "The ticket has been succesfully assigned to group " + sg.name
                    };
                    hDesk.TicketHistories.Add(tHistory);
                    hDesk.SaveChanges();

                    //Creates notification for group
                    foreach (User user in sg.Users)
                    {
                        NotificationsFacade.CreateTicketFlowNotification(user.id,
                            current_user.id,
                            "Ticket assigned to your group",
                            String.Format("Ticket was assigned to your group by {0} at {1}", current_user.fullName, DateTime.UtcNow.ToLocalTime()), ticket.id);
                    }

                    ViewBag.Message = null;
                    ViewBag.Success = "The ticket has been succesfully assigned to group " + sg.name;
                    return View(ticket);
                }

            }
            if (Submit == "SetComplete")
            {
                ticket.Status = hDesk.Status.SingleOrDefault(s => s.name == "Done");
                TicketHistory tHistory = new TicketHistory()
                {
                    ticketId = ticket.id,
                    date = DateTime.UtcNow,
                    userId = current_user.id,
                    actionId = HistoryAction.Completed().id,
                    text = String.Format("Ticket has been successfully solved by {0} from group {1}", current_user.fullName, ticket.Group.name)
                };
                hDesk.TicketHistories.Add(tHistory);
                hDesk.SaveChanges();

                //Create notification for Author
                NotificationsFacade.CreateTicketFlowNotification(ticket.authorId, current_user.id,
                    "Ticket was solved",
                    String.Format("Ticket was solved by {0} at {1}", current_user.fullName, DateTime.UtcNow.ToLocalTime()), ticket.id);

                ViewBag.Message = null;
                ViewBag.Success = "You have successfully marked the ticket as solved";
            }
            if (Submit == "UploadFiles")
            {
                try
                {
                    double sizeOfFiles = 0; //we declare the size is 0 now
                    if (attachedFiles.First() != null)
                    {
                        foreach (var file in attachedFiles) //here we check the total size uf uploaded files
                        {
                            sizeOfFiles += file.ContentLength;
                        }
                    }
                    if (sizeOfFiles > 6000000) //if size  > 6MB, alert
                    {
                        ModelState.AddModelError("", "Uploading failed,maximum size allowed for attachments is 6MB");
                    }
                    if (attachedFiles.First() == null)
                    {
                        ModelState.AddModelError("", "Uploading failed,please choose the file before uploading");
                    }
                    if (ModelState.IsValid)
                    {
                        BlobStorageService bss = new BlobStorageService();
                        CloudBlobContainer blobContainer = bss.GetBlobContainer();
                        foreach (var file in attachedFiles)
                        {
                            if (file.FileName != "" && file.ContentLength > 0)
                            {
                                CloudBlockBlob blob = blobContainer.GetBlockBlobReference(file.FileName);
                                blob.UploadFromStream(file.InputStream);
                                Attachment attach = new Attachment(); //post the file to the server 
                                attach.Ticket = ticket;
                                attach.url = blob.Uri.ToString();
                                attach.User = current_user;
                                attach.name = String.Format("{0} [by: {1}] | {2}", file.FileName, WebSecurity.CurrentUserName, DateTime.Now.ToLocalTime().ToShortTimeString());
                                hDesk.Attachments.Add(attach);
                            }
                        }
                        hDesk.SaveChanges();

                        //Create notification
                        List<int> idsOfNotificationReceivers = new List<int>();
                        idsOfNotificationReceivers.Add(ticket.authorId);
                        if (ticket.solutionistId != null) idsOfNotificationReceivers.Add((int)ticket.solutionistId);
                        idsOfNotificationReceivers.Remove(current_user.id);

                        foreach (int receiverId in idsOfNotificationReceivers)
                        {
                            NotificationsFacade.CreateEmailingNotification(receiverId,
                                    current_user.id,
                                    "New attachement on ticket",
                                    String.Format("New attachement on ticket from {0} at {1}", WebSecurity.CurrentUserName, DateTime.UtcNow.ToLocalTime()), ticket.id);
                        }

                        return RedirectToAction("Details/" + ticket.id, "Ticket", new { msg = "You have successfully uploaded file(s)" });
                    }
                    else
                    {
                        ViewBag.IsNotValid = true;
                        return View(ticket);
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                }
            }
            return View(ticket);
        }

        [HttpPost]
        [Authorize]
        public ActionResult CreatePost(int id, string Text, string isVisible, string msg = null)
        {
            if (isVisible == null || isVisible == "true")
                isVisible = "true";
            else
                isVisible = "false";
            ViewBag.Message = msg;
            HelpDeskEntities hDesk = new HelpDeskEntities();
            User current_user = hDesk.Users.SingleOrDefault(u => u.id == WebSecurity.CurrentUserId);
            if (!string.IsNullOrEmpty(Text))
            {
                int currentUserId = WebSecurity.CurrentUserId;
                TicketPost tPost = new TicketPost();
                tPost.userId = currentUserId;
                tPost.ticketId = id;
                tPost.isVisible = Convert.ToBoolean(isVisible);
                tPost.text = Text;
                tPost.createdAt = DateTime.UtcNow;
                hDesk.TicketPosts.Add(tPost);
                hDesk.SaveChanges();

                hDesk.SaveChanges();
                ViewBag.Status = "New comment posted successfuly";

                Ticket ticket = hDesk.Tickets.Single(t => t.id == id);
                //Create notification
                List<int> idsOfNotificationReceivers = new List<int>();
                User receiver = hDesk.Users.SingleOrDefault(u => u.id == ticket.authorId);

                if (receiver.isSolutionist || Convert.ToBoolean(isVisible))
                    idsOfNotificationReceivers.Add(ticket.authorId);

                if (ticket.solutionistId != null) idsOfNotificationReceivers.Add((int)ticket.solutionistId);
                idsOfNotificationReceivers.Remove(currentUserId);

                foreach (int receiverId in idsOfNotificationReceivers)
                {
                    NotificationsFacade.CreateEmailingNotification(receiverId,
                            currentUserId,
                            "New comment on ticket",
                            String.Format("New comment on ticket from {0} at {1}", WebSecurity.CurrentUserName, DateTime.UtcNow.ToLocalTime()), ticket.id);
                }
            }

            else
            {
                return RedirectToAction("Details", new { @id = id, msg = "You need to write a comment before posting it" });
            }


            return RedirectToAction("Details", new { @id = id, msg = "The post was successfully created" });
        }

        public ActionResult SetComplete(int id, string msg = null)
        {
            ViewBag.Message = msg;

            // load ticket
            HelpDeskEntities hDesk = new HelpDeskEntities();
            Ticket ticket = hDesk.Tickets.Single(t => t.id == id);
            User current_user = hDesk.Users.Single(u => u.id == WebSecurity.CurrentUserId);

            //Create notification for Author
            NotificationsFacade.CreateTicketFlowNotification(ticket.authorId, current_user.id,
                "Ticket was solved",
                String.Format("Ticket was solved by {0} at {1}", current_user.fullName, DateTime.UtcNow.ToLocalTime()), ticket.id);

            // set status
            ticket.Status = hDesk.Status.Single(s => s.name == "Done");
            // save
            hDesk.SaveChanges();

            // return to detail
            return RedirectToAction("Details", new { @id = id, msg = "Ticket is completed!" });
        }

        public JsonResult GetSupportersInGroup(string term)
        {
            HelpDeskEntities hDesk = new HelpDeskEntities();
            List<string> supporters = new List<string>();
            User current_user = hDesk.Users.SingleOrDefault(u => u.username == WebSecurity.CurrentUserName);
            foreach (Group sg in current_user.Groups)
            {
                foreach (User u in sg.Users) //iterate the users in users group.If everything is ok then add to the list.
                { 
                    if (!supporters.Contains(u.username) && u.username != current_user.username && u.username.StartsWith(term))
                    {
                        supporters.Add(u.username);
                    }
                }
            }
            return Json(supporters, JsonRequestBehavior.AllowGet);
        }
        //This method care about sending tickets beetween people in the group in which the current user is.
        public ActionResult Transmit(int id)
        {
            HelpDeskEntities hDesk = new HelpDeskEntities();
            Ticket ticket = hDesk.Tickets.SingleOrDefault(t => t.id == id);
            User current_user = hDesk.Users.SingleOrDefault(u => u.id == WebSecurity.CurrentUserId);
            if (ticket == null || current_user.webpages_Roles.SingleOrDefault(r => r.level >= 1) == null) //if theres no ticket like that is our database,throw error 404
            {//of the users role level is < 1
                return RedirectToAction("NotFound", "Errors");
            }
            return View(ticket);
        }


        [HttpPost]
        public ActionResult Transmit(int id, string UserName, string Reason)
        {
            HelpDeskEntities hDesk = new HelpDeskEntities();
            User current_user = hDesk.Users.SingleOrDefault(u => u.id == WebSecurity.CurrentUserId);//get the current user
            Ticket ticket = hDesk.Tickets.SingleOrDefault(t => t.id == id); //get the current ticket

            if (Reason.Length <= 10 || Reason.Length > 700)
            {
                ModelState.AddModelError("", "The length of the reason must be at least 10 and less than 700");
            }
            if (string.IsNullOrEmpty(UserName))
            {
                ModelState.AddModelError("", "User name is required");
            }
            if (UserName == current_user.username)
            {
                ModelState.AddModelError("", "You are not allowed to transmit the ticket to yourself");
            }
            bool userExistsInGroup = false;
            foreach (Group sg in current_user.Groups)
            {//if this group has the current ticket assigned and the user given in the textbox is also in the group
                if (sg.Tickets.SingleOrDefault(t => t.id == ticket.id) != null && sg.Users.SingleOrDefault(u => u.username == UserName) != null)
                {
                    userExistsInGroup = true;
                    break;
                }
            }
            if (!userExistsInGroup)
            {
                ModelState.AddModelError("", "The user given is not in the same group like you or doesnt exist at all");
            }
            if (Ticket.getAccepted_byUser(current_user.id).SingleOrDefault(t => t.id == id) == null)
            {
                ModelState.AddModelError("", "You dont have permission to transmit this ticket");

            }
            if (ModelState.IsValid)
            { //if everything is OK

                User newSolutionist = hDesk.Users.SingleOrDefault(u => u.username == UserName);
                ticket.solutionistId = newSolutionist.id;
                ticket.statusId = 2;
                TicketHistory tHistory = new TicketHistory()
                {
                    date = DateTime.UtcNow,
                    ticketId = ticket.id,
                    userId = newSolutionist.id,
                    actionId = HistoryAction.AssignedToUser(hDesk).id,
                    text = "Ticket has been transmited from " + current_user.fullName + " to " + newSolutionist.fullName,
                    reason = Reason
                };
                NotificationsFacade.CreateTicketFlowNotification(newSolutionist.id,
                              current_user.id,
                              "Ticket has been assigned to you",
                              String.Format("Ticket {0 } was assigned to you by {1} at {2}", ticket.ticketId, current_user.fullName, DateTime.UtcNow.ToLocalTime()), ticket.id);
                hDesk.TicketHistories.Add(tHistory);
                hDesk.SaveChanges();

                //Creates a notification
                NotificationsFacade.CreateTicketFlowNotification(ticket.authorId,
                    current_user.id,
                    "Your ticket has been assigned",
                    String.Format("Ticket {0} was transmited to {1} by {2} at {3}", ticket.ticketId, newSolutionist.fullName, current_user.fullName, DateTime.UtcNow.ToLocalTime()), ticket.id);

                return RedirectToAction("Details/" + ticket.id, "Ticket", new { msg = "You have successfully transmited this ticket to " + newSolutionist.fullName });

            }
            else
            {
                ModelState.AddModelError("", "Error ocurred during sending ticket process");
                ViewBag.IsNotValid = true;
                return View(ticket);
            }
        }
        public ActionResult ChangeType(int id)
        {
            HelpDeskEntities hDesk = new HelpDeskEntities();
            Ticket ticketNeedChange = hDesk.Tickets.SingleOrDefault(t => t.id == id);
            User current_user = hDesk.Users.SingleOrDefault(u => u.id == WebSecurity.CurrentUserId);
            if (ticketNeedChange.solutionistId != current_user.id)
            {
                return RedirectToAction("Details/" + ticketNeedChange.id, "Ticket", new { msg = "You are not allowed to change type of this ticket" });
            }
            return View(ticketNeedChange);
        }

        [HttpPost]
        public ActionResult ChangeType(FormCollection fc, int id)
        {
            HelpDeskEntities hDesk = new HelpDeskEntities();
            Ticket ticketNeedChange = hDesk.Tickets.SingleOrDefault(t => t.id == id);
            User current_user = hDesk.Users.SingleOrDefault(u => u.id == WebSecurity.CurrentUserId);
            int ticketTypeId = 0;

            if (fc["Reason"].Length <= 10 || fc["Reason"].Length > 700)
                ModelState.AddModelError("", "The length of the reason must be at least 10 and less than 700");

            if (fc["Type"] == "")
                ModelState.AddModelError("", "Please choose the type you want to change to");
            if (Convert.ToInt32(fc["Type"]) == ticketNeedChange.TicketType.id)
                ModelState.AddModelError("", "Please choose a different type for this ticket,you have selected the current type");
            else
                ticketTypeId = Convert.ToInt32(fc["Type"]);
            if (ModelState.IsValid)
            {
                string oldType = ticketNeedChange.TicketType.name;
                string newType = hDesk.TicketTypes.SingleOrDefault(t => t.id == ticketTypeId).name;
                ticketNeedChange.typeId = ticketTypeId; //change the type 
                TicketHistory tHistory = new TicketHistory();
                tHistory.HistoryAction = HistoryAction.TypeChanged(hDesk);
                tHistory.date = DateTime.UtcNow;
                tHistory.text = String.Format("The type of ticket has been changed by {0} from {1} to {2}", current_user.fullName, oldType, newType);
                tHistory.Ticket = ticketNeedChange;
                tHistory.User = current_user;
                tHistory.reason = fc["Reason"];

                NotificationsFacade.CreateTicketFlowNotification(ticketNeedChange.Author.id,
                 current_user.id,
                 "Ticket type has been changed",
                 String.Format("The type of ticket {0} has been changed by {1} at {2}", ticketNeedChange.ticketId, current_user.fullName, DateTime.UtcNow.ToLocalTime()), ticketNeedChange.id);
                hDesk.TicketHistories.Add(tHistory); //add the history 
                hDesk.SaveChanges(); //and finally save changes
                return RedirectToAction("Details/" + ticketNeedChange.id, "Ticket", new { msg = "The type of this ticket has been successfully changed" });

            }
            else
            {
                ViewBag.IsNotValid = true;
                return View(ticketNeedChange);
            }
        }

        [Authorize]
        public ActionResult AssignToUser(int id)
        { 
            HelpDeskEntities hDesk = new HelpDeskEntities();
            Ticket ticket = hDesk.Tickets.SingleOrDefault(t => t.id == id); //ticket to assign
            User current_user = hDesk.Users.SingleOrDefault(u=>u.id == WebSecurity.CurrentUserId);
            if(ticket == null)
                return RedirectToAction("NotFound","Errors");
            if (ticket.Status.name == "Done" || ticket.Status.name == "New" || current_user.webpages_Roles.SingleOrDefault(r => r.level == 1) == null)
                return RedirectToAction("Details/" + id, new { msg = "You dont have permission to assign this ticket" });
            else
                return View(ticket);
        }
        [Authorize]
        [HttpPost]
        public ActionResult AssignToUser(int id,string reason, string groups,string supporters,string Submit)
        {

            HelpDeskEntities hDesk = new HelpDeskEntities();
            Ticket ticket = hDesk.Tickets.SingleOrDefault(t => t.id == id); //ticket to assign
            User current_user = hDesk.Users.SingleOrDefault(u => u.id == WebSecurity.CurrentUserId);

            try
            {
                if (Submit == "Next")
                {
                    //Supporter not selected
                    if (groups == "")
                    {
                        ModelState.AddModelError("", "Please choose the group you want to assign this ticket to");
                        ViewBag.IsNotValid = true;
                        return View(ticket);
                    }
                    if (groups != "")
                    {
                        int group_id = Convert.ToInt32(groups);
                        return RedirectToAction("AssignToUser/" + id, new { group_id = group_id, step = "2" });
                    }
                }
                if (Submit == "Back")
                {
                    if (groups != "")
                    {
                        int group_id = Convert.ToInt32(Request.QueryString["group_id"]);
                        return RedirectToAction("AssignToUser/" + id, new { group_id = group_id, step = "1" });
                    }
                }
                if (Submit == "Assign")
                {//if user clicks to Assign
                    //if the reason is empty or less than10 words

                    int group_id = Convert.ToInt32(Request.QueryString["group_id"]);
                    int user_id = Convert.ToInt32(supporters); //now we find the id of selected group and supporter and assign it,change the status,and add to the history in db.
                    User user = hDesk.Users.SingleOrDefault(u => u.id == user_id);
                    Group group = hDesk.Groups.SingleOrDefault(g => g.id == group_id);

                    if (group == null || user == null)
                    {
                        ModelState.AddModelError("", "Selected group or user does not exist in the database");

                    }
                    if (string.IsNullOrEmpty(reason))
                    {
                        ModelState.AddModelError("", "The length of the reason must be between 10 and 700");
                    }

                    //Supporter not selected
                    if (supporters == "")
                    {
                        ModelState.AddModelError("", "Please choose the supporter you want to assign this ticket to");
                    }
                    if (!user.Groups.Contains(group))
                    {
                        ModelState.AddModelError("", "Selected supporter is not in the group " + group.name);
                    }

                    if (ModelState.IsValid)
                    {
                        ticket.Status = hDesk.Status.SingleOrDefault(s => s.name == "AssignedToUser"); //change the status of the ticket to Assigned
                        ticket.Solutionist_ = user;
                        ticket.Group = group;

                        TicketHistory tHistory = new TicketHistory();
                        tHistory.date = DateTime.UtcNow;
                        tHistory.Ticket = ticket;
                        tHistory.User = user;
                        tHistory.HistoryAction = HistoryAction.AssignedToUser(hDesk);
                        tHistory.text = "Ticket has been assigned to " + user.fullName + " by " + current_user.fullName;
                        tHistory.reason = reason;
                        hDesk.TicketHistories.Add(tHistory);
                        hDesk.SaveChanges();

                        NotificationsFacade.CreateTicketFlowNotification(user.id,
                                  current_user.id,
                                  "Ticket has been assigned to you",
                                  String.Format("Ticket {0 } was assigned to you by {1} at {2}", ticket.ticketId, current_user.fullName, DateTime.UtcNow.ToLocalTime()), ticket.id);

                        //Creates a notification
                        NotificationsFacade.CreateTicketFlowNotification(ticket.authorId,
                            current_user.id,
                            "Your ticket has been assigned",
                            String.Format("Ticket {0} was transmited to {1} by {2} at {3}", ticket.ticketId, user.fullName, current_user.fullName, DateTime.UtcNow.ToLocalTime()), ticket.id);

                        return RedirectToAction("Details/" + ticket.id, new { @msg = "Ticket has been successfully assigned" });
                    }



                    else
                    {
                        ViewBag.IsNotValid = true;
                        return View(ticket);
                    }






                }
                return View(ticket);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Details/" + ticket.id, new { @msg = ex.Message });
            }
        }


        public ActionResult Refuse(int id)
        { 
            HelpDeskEntities hDesk = new HelpDeskEntities();
            User current_user = hDesk.Users.SingleOrDefault(u => u.id == WebSecurity.CurrentUserId);
            Ticket ticket = hDesk.Tickets.SingleOrDefault(t=>t.id == id);
            if (ticket.Status.name != "New")
                return RedirectToAction("Details/" + id, new { @msg = "Sorry this ticket can not be refused" });
            if (!Roles.IsUserInRole("Support") && current_user.webpages_Roles.SingleOrDefault(r=>r.level ==1) == null)
                return RedirectToAction("Details/" + id, new { @msg = "Sorry you dont have permission to refuse this ticket" });
            return View(ticket);
        }

        [HttpPost]
        public ActionResult Refuse(int id, string Reason)
        {
            HelpDeskEntities hDesk = new HelpDeskEntities();
            Ticket ticket = hDesk.Tickets.SingleOrDefault(t => t.id == id);
            User current_user = hDesk.Users.SingleOrDefault(u => u.id == WebSecurity.CurrentUserId);
            if (Reason.Length < 10 || Reason.Length > 700)
            {
                ModelState.AddModelError("", "The length of the reason must be between 10 and 700");
            }
            else
            {
                Status status = hDesk.Status.SingleOrDefault(s => s.name == "Refused");
                ticket.Status = status;

                TicketHistory tHistory = new TicketHistory()
                {
                    date = DateTime.UtcNow,
                    Ticket = ticket,
                    User = current_user,
                    text = "Ticket has been refused by " +current_user.fullName,
                    reason = Reason,
                    actionId = 1008
                };
                hDesk.TicketHistories.Add(tHistory);
                hDesk.SaveChanges();

                NotificationsFacade.CreateTicketFlowNotification(ticket.authorId,
                               current_user.id,
                               "Ticket has been refused",
                               String.Format("Ticket has been refused by {0} at {1}", current_user.fullName,
                                   DateTime.UtcNow.ToLocalTime()),ticket.id);
            }
            if (ModelState.IsValid)
            {
                return RedirectToAction("Details/" + id, new { @msg = "Ticket has been successfully refused" });
            }
            else
            {
                ViewBag.IsNotValid = true;
                return View(ticket);
            }
        }

        public ActionResult Complain(int id)
        {
            HelpDeskEntities hDesk = new HelpDeskEntities();
            User current_user = hDesk.Users.SingleOrDefault(u => u.id == WebSecurity.CurrentUserId);
            Ticket ticket = hDesk.Tickets.SingleOrDefault(t => t.id == id);
            if (ticket.Status.name != "Refused" && ticket.Status.name != "Done")
                return RedirectToAction("Details/" + id, new { @msg = "Sorry you can not complain about this ticket" });
            if (current_user.Retailer == null)
                return RedirectToAction("Details/" + id, new { @msg = "Sorry you dont have permission to complain about this ticket" });
            return View(ticket);
        }

        [HttpPost]
        public ActionResult Complain(int id, string Reason)
        {
            HelpDeskEntities hDesk = new HelpDeskEntities();
            Ticket ticket = hDesk.Tickets.SingleOrDefault(t => t.id == id);
            User current_user = hDesk.Users.SingleOrDefault(u => u.id == WebSecurity.CurrentUserId);
            if (Reason.Length < 10 || Reason.Length > 700)
            {
                ModelState.AddModelError("", "The length of the reason must be between 10 and 700");
            }
            else
            {
                if (ticket.Status.name == "Refused")
                {
                    Status status = hDesk.Status.SingleOrDefault(s => s.name == "New");
                    ticket.Status = status;
                }
                else
                {
                    Status status = hDesk.Status.SingleOrDefault(s => s.name == "Accepted");
                    ticket.Status = status;
                }

                TicketHistory tHistory = new TicketHistory()
                {
                    date = DateTime.UtcNow,
                    Ticket = ticket,
                    User = current_user,
                    text = current_user.fullName + " was complained about ticket",
                    reason = Reason,
                    actionId = 1008
                };
                hDesk.TicketHistories.Add(tHistory);
                hDesk.SaveChanges();

                //Creates a notification
                if (ticket.Status.name == "New") //if status has been changed to NEW
                {
                    foreach (Group group in hDesk.Groups.Where(g => g.level == 1))
                    {
                        foreach (User user in group.Users)
                        {
                            NotificationsFacade.CreateTicketFlowNotification(user.id,
                                current_user.id,
                                "The complaint about ticket has been made",
                                String.Format("The complaint about ticket has been made by {0} at {1}", current_user.fullName,
                                    DateTime.UtcNow.ToLocalTime()), ticket.id);
                        }
                    }
                }
                if(ticket.Status.name == "Accepted")
                { 
                    //if ticket is ACCEPTED then show notification to solutionist
                    NotificationsFacade.CreateTicketFlowNotification(ticket.Solutionist_.id,
                                current_user.id,
                                "The complaint about ticket has been made",
                                String.Format("The complaint about ticket has been made by {0} at {1}", current_user.fullName,
                                    DateTime.UtcNow.ToLocalTime()), ticket.id);
                }
            }
            if (ModelState.IsValid)
            {
                return RedirectToAction("Details/" + id, new { @msg = "The complaint has been successfully made" });
            }
            else
            {
                ViewBag.IsNotValid = true;
                return View(ticket);
            }
        }
    }
    
}