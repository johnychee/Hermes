using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using TestWebApp.Models;
using WebMatrix.WebData;

namespace TestWebApp.Models
{
    // User - Customer
    // User1 - Created by
    // User2 - Solutionist
    public partial class Ticket
    {
        [Required]
        [Display(Name = "Subject")]
        [MaxLength(60)]
        public string subject_
        {
            get { return this.subject; }
            set { this.subject = value; }
        }
        [Required]
        [Display(Name = "Issue")]
        [MaxLength(1000)]
        public string text_
        {
            get { return this.text; }
            set { this.text = value; }
        }

        [Required]
        [Display(Name = "Product")]
        public int? productId_
        {
            get { return this.productId; }
            set { this.productId = value; }
        }

        [Required]
        public int authorId_
        {
            get { return this.authorId; }
            set { this.authorId = value; }
        }

        public string author_ { get; set; }
        public User Author
        {
            get { return this.User; }
            set { this.User = value; }
        }
        public User CreatedBy_
        {
            get { return this.User1; }//it can dynamically CHanged if we change DB :(((
            set { this.User1 = value; }
        }
        public User Solutionist_
        {
            get { return this.User2; }//it can dynamically CHanged if we change DB :(((
            set { this.User2 = value; }
        }
        public bool isAccepted()
        {
            return (this.Status.name == "Accepted");
        }

        public static IQueryable<Ticket> getNew(HelpDeskEntities hDesk = null)
        {
            if (hDesk == null)
                hDesk = new HelpDeskEntities();

            return hDesk.Tickets.Where(t => t.Status.name == "New");
        }
        public static IQueryable<Ticket> getAssigned(HelpDeskEntities hDesk = null)
        {
            if (hDesk == null)
                hDesk = new HelpDeskEntities();

            return hDesk.Tickets.Where(t => t.Status.name == "Assigned");
        }
        public static IQueryable<Ticket> getAccepted(HelpDeskEntities hDesk = null)
        {
            if (hDesk == null)
                hDesk = new HelpDeskEntities();

            return hDesk.Tickets.Where(t => t.Status.name == "Accepted");
        }

        public static IQueryable<Ticket> getRefused(HelpDeskEntities hDesk = null)
        {
            if (hDesk == null)
                hDesk = new HelpDeskEntities();

            return hDesk.Tickets.Where(t => t.Status.name == "Refused");
        }
        public static IQueryable<Ticket> getDone(HelpDeskEntities hDesk = null)
        {
            if (hDesk == null)
                hDesk = new HelpDeskEntities();

            return hDesk.Tickets.Where(t => t.Status.name == "Done");
        }
        // return all tickets, that their status is not Done
        public static IQueryable<Ticket> getNotDone(HelpDeskEntities hDesk = null)
        {
            if (hDesk == null)
                hDesk = new HelpDeskEntities();
            return hDesk.Tickets.Where(t => t.Status.name != "Done");
        }

        // returns all tickets created by given user  And also  tickets accepted by him
        public static IQueryable<Ticket> GetOwnTickets(int userId, HelpDeskEntities hDesk = null)
        {
            if (hDesk == null)
                hDesk = new HelpDeskEntities();
            return hDesk.Tickets.Where(t => t.authorId == userId);
        }
        public static IQueryable<Ticket> GetAllMyOwn(int userId, HelpDeskEntities hDesk = null)
        {
            if (hDesk == null)
                hDesk = new HelpDeskEntities();
            return hDesk.Tickets.Where(t => t.createdByUserId == userId || t.authorId == userId);
        }

        public static IQueryable<Ticket> GetByCreatedByUserId(int userId, HelpDeskEntities hDesk = null)
        {
            if (hDesk == null)
                hDesk = new HelpDeskEntities();
            return hDesk.Tickets.Where(t => t.createdByUserId == userId);
        }
        public static IQueryable<Ticket> getMyDone(int userId, HelpDeskEntities hDesk = null)
        {
            if (hDesk == null)
                hDesk = new HelpDeskEntities();

            return hDesk.Tickets.Where(t => t.authorId == userId && t.Status.name == "Done");
        }
        public static IQueryable<Ticket> getMyNotDone(int userId, HelpDeskEntities hDesk = null)
        {
            if (hDesk == null)
                hDesk = new HelpDeskEntities();

            return hDesk.Tickets.Where(t => t.authorId == userId && t.Status.name != "Done");
        }

        public static IQueryable<Ticket> GetNotSolved(User user, HelpDeskEntities hDesk = null)
        {
            if (hDesk == null)
                hDesk = new HelpDeskEntities();

            return getAssigned_toUserGroups(user).Where(t => t.Status.name != "Done");
        }
        public static IQueryable<Ticket> GetNotSolved(int userId, HelpDeskEntities hDesk = null)
        {
            if (hDesk == null)
                hDesk = new HelpDeskEntities();

            User user = hDesk.Users.FirstOrDefault(u => u.id == userId);
            return GetNotSolved(user, hDesk);
        }

        public static IQueryable<Ticket> GetSolvedByMe(int userId, HelpDeskEntities hDesk = null)
        {
            if (hDesk == null)
                hDesk = new HelpDeskEntities();

            return getAccepted_byUser(userId, true);
        }

        // return tickets, that are assigned to users groups
        public static IQueryable<Ticket> getAssigned_toUserGroups(User user, HelpDeskEntities hDesk = null)
        {
            if (hDesk == null)
                hDesk = new HelpDeskEntities();

            return hDesk.Tickets.Where(t => hDesk.Users.FirstOrDefault(u => u.id == user.id).Groups.Contains(t.Group));
            //return hDesk.Tickets.SqlQuery("Select dbo.Tickets.* from dbo.Tickets WHERE dbo.Tickets.groupId in (SELECT groupId FROM dbo.Groups_Users WHERE userId = @p0)", userId);
        }
        public static IQueryable<Ticket> getAssigned_toUserGroups(int userId, HelpDeskEntities hDesk = null)
        {
            if (hDesk == null)
                hDesk = new HelpDeskEntities();

            User user = hDesk.Users.FirstOrDefault(u => u.id == userId);
            return getAssigned_toUserGroups(user, hDesk);
        }

        // return tickets, that are assigned to the specific user
        public static IQueryable<Ticket> getAssigned_toUser(int userId, HelpDeskEntities hDesk = null)
        {
            if (hDesk == null)
                hDesk = new HelpDeskEntities();
            
            return hDesk.Tickets.Where(t => t.solutionistId == userId && t.Status.name == "AssignedToUser");
        }

        // return tickets accepted by given user
        public static IQueryable<Ticket> getAccepted_byUser(int userId, bool ticketIsDone = false, HelpDeskEntities hDesk = null)
        {
            if (hDesk == null)
                hDesk = new HelpDeskEntities();
            if (ticketIsDone)
            {
                return hDesk.Tickets.Where(t => t.solutionistId == userId && t.Status.name == "Done");

            }
            return hDesk.Tickets.Where(t => t.solutionistId == userId && t.Status.name == "Accepted");
        }

        //// return tickets, that are new and asigned to given users product
        //public static IQueryable<Ticket> getNew_assignedToUserProduct(User user, HelpDeskEntities hDesk = null)
        //{
        //    if (hDesk == null)
        //        hDesk = new HelpDeskEntities();


        //    return hDesk.Tickets.SqlQuery("Select dbo.Ticket.* from dbo.TicketNew JOIN dbo.Ticket ON dbo.TicketNew.TicketId = dbo.Ticket.TicketId WHERE dbo.Ticket.ProductId IN (SELECT dbo.Product.ProductId FROM dbo.Product JOIN dbo.SupportGroup_Product ON dbo.SupportGroup_Product.ProductId = dbo.Product.ProductId WHERE dbo.SupportGroup_Product.SupportGroupId IN (SELECT dbo.SupportGroup.SupportGroupId FROM dbo.SupportGroup JOIN dbo.SupportGroup_User ON dbo.SupportGroup_User.SupportGroupId = dbo.SupportGroup.SupportGroupId WHERE dbo.SupportGroup_User.UserId = @p0))", userId);
        //}
        //public static IQueryable<Ticket> getNew_assignedToUserProduct(int userId, HelpDeskEntities hDesk = null)
        //{
        //    if (hDesk == null)
        //        hDesk = new HelpDeskEntities();

        //    User user = hDesk.Users.FirstOrDefault(u => u.id == userId);
        //    return getNew_assignedToUserProduct(userId, hDesk);
        //}

        public void CreateBusinessTicketId(HelpDeskEntities hDesk = null)
        {
            if (hDesk == null)
                hDesk = new HelpDeskEntities();

            string tempId = id.ToString();

            for (int i = 0; i < 6 - id.ToString().Length; i++)
            {
                tempId = tempId.Insert(0, "0");
            }
            if (typeId == null) ticketId = tempId.Insert(0, "NT");
            else if (typeId == hDesk.TicketTypes.Single(tt=>tt.name == "Incident").id) ticketId = tempId.Insert(0, "IN");
            else if (typeId == hDesk.TicketTypes.Single(tt => tt.name == "Request").id) ticketId = tempId.Insert(0, "SR");
            
            hDesk.SaveChanges();
        }

        //COunt time ago
        public  string TimeAgo(DateTime date)
        {
            TimeSpan timeSince = DateTime.UtcNow.Subtract(date);
            if (timeSince.TotalMilliseconds < 1) return "Not yet";
            if (timeSince.TotalMinutes < 1) return "Just now";
            if (timeSince.TotalMinutes < 2) return "1 minute ago";
            if (timeSince.TotalMinutes < 60) return string.Format("{0} minutes ago", timeSince.Minutes);
            if (timeSince.TotalMinutes < 120) return "1 hour ago";
            if (timeSince.TotalHours < 24) return string.Format("{0} hours ago", timeSince.Hours);
            if (timeSince.TotalDays < 2) return "Yesterday";
            if (timeSince.TotalDays < 7) return string.Format("{0} days ago", timeSince.Days);
            if (timeSince.TotalDays < 14) return "Last week";
            if (timeSince.TotalDays < 21) return "2 weeks ago";
            if (timeSince.TotalDays < 28) return "3 weeks ago";
            if (timeSince.TotalDays < 60) return "Last month";
            if (timeSince.TotalDays < 365) return string.Format("{0} months ago", Math.Round(timeSince.TotalDays / 30));
            if (timeSince.TotalDays < 730) return "Last year"; //last but not least...
            return string.Format("{0} years ago", Math.Round(timeSince.TotalDays / 365));
        }


    }
}