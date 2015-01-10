using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TestWebApp.Models;
using WebMatrix.WebData;

namespace TestWebApp.Models
{
    public partial class Notification
    {
        public User Owner
        {
            get { return this.User1; }//it can dynamically CHanged if we change DB :(((
            set { this.User1 = value; }
        }

        public User Creator
        {
            get { return this.User; }//it can dynamically CHanged if we change DB :(((
            set { this.User = value; }
        }
        public string createdAgo
        {
            get
            {
                if ((DateTime.UtcNow - CreatedAt).Minutes < 1)
                    return "few seconds ago";
                if ((DateTime.UtcNow - CreatedAt).Hours < 1)
                {
                    if ((DateTime.UtcNow - CreatedAt).Minutes == 1)
                        return String.Format("{0} minute ago", (DateTime.Now - CreatedAt).Minutes);
                    else
                        return String.Format("{0} minutes ago", (DateTime.Now - CreatedAt).Minutes);
                }
                if ((DateTime.UtcNow - CreatedAt).Days < 1)
                {
                    if ((DateTime.UtcNow - CreatedAt).Hours == 1)
                        return String.Format("{0} hour ago", (DateTime.Now - CreatedAt).Hours);
                    else
                        return String.Format("{0} hours ago", (DateTime.Now - CreatedAt).Hours);
                }
                if ((DateTime.UtcNow - CreatedAt).Days == 1)
                    return String.Format("{0} day ago", (DateTime.Now - CreatedAt).Days);
                else
                    return String.Format("{0} days ago", (DateTime.Now - CreatedAt).Days);
            }
        }

        public static IEnumerable<Notification> GetLastFiveNew(int userId ,HelpDeskEntities hDesk = null)
        {
            hDesk = hDesk ?? new HelpDeskEntities();

            return hDesk.Notifications.Where(t => t.Readed == false && t.OwnerId == userId).OrderByDescending(t=>t.CreatedAt).Take(5).ToList();
        }

        public static IEnumerable<Notification> GetNew(int userId, bool newFirst = true, HelpDeskEntities hDesk = null)
        {
            hDesk = hDesk ?? new HelpDeskEntities();
            
            if (newFirst)
                return hDesk.Notifications.Where(t => t.Readed == false && t.OwnerId == userId).OrderByDescending(t => t.CreatedAt).ToList();
            else
                return hDesk.Notifications.Where(t => t.Readed == false && t.OwnerId == userId).OrderBy(t => t.CreatedAt).ToList();
        }

        public static IEnumerable<Notification> GetAllsortedByCreatedAt(int userId, HelpDeskEntities hDesk = null)
        {
            hDesk = hDesk ?? new HelpDeskEntities();

            IEnumerable<Notification> newNotifications = hDesk.Notifications.Where(t => t.OwnerId == userId).ToList().OrderBy(t => t.CreatedAt).Reverse();
            return newNotifications;
        }
        public static void setAllReaded(int userId, HelpDeskEntities hDesk = null)
        {
            hDesk = hDesk ?? new HelpDeskEntities();

            foreach(Notification notif in hDesk.Notifications.Where(n => n.OwnerId == userId))
            {
                notif.Readed = true;
            }
            hDesk.SaveChanges();
        }
    }
}