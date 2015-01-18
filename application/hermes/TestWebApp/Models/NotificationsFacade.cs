using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWebApp.Models
{
    public static class NotificationsFacade
    {
        private static readonly HermesDBEntities _HermesDBEntities = new HermesDBEntities();

        #region public methods
        public static void CreateSLANotification(int ownerId, string subject, string text)
        {
            Notification newNotification = CreateNotification(ownerId, 
                _HermesDBEntities.Users.First(u => u.webpages_Roles.FirstOrDefault().RoleName == "System").id,
                 subject, text);
            newNotification.NotificationTypeId =
                _HermesDBEntities.NotificationsTypes.SingleOrDefault(t => t.Name == NotificationType.SLA.ToString()).Id;

            _HermesDBEntities.Notifications.Add(newNotification);
            _HermesDBEntities.SaveChanges();
        }

        public static void CreateSystemNotification(int ownerId, string subject, string text)
        {
            Notification newNotification = CreateNotification(ownerId, 
                _HermesDBEntities.Users.First(u => u.webpages_Roles.FirstOrDefault().RoleName == "System").id,
                 subject, text);
            newNotification.NotificationTypeId =
                _HermesDBEntities.NotificationsTypes.SingleOrDefault(t => t.Name == NotificationType.System.ToString()).Id;

            _HermesDBEntities.Notifications.Add(newNotification);
            _HermesDBEntities.SaveChanges();
        }

        public static void CreateEmailingNotification(int ownerId, int creatorId, string subject, string text)
        {
            Notification newNotification = CreateNotification(ownerId, creatorId, subject, text);
            newNotification.NotificationTypeId =
                _HermesDBEntities.NotificationsTypes.SingleOrDefault(t => t.Name == NotificationType.Emailing.ToString()).Id;

            _HermesDBEntities.Notifications.Add(newNotification);
            _HermesDBEntities.SaveChanges();
        }

        public static void CreateTicketFlowNotification(int ownerId, int creatorId, string subject, string text)
        {
            Notification newNotification = CreateNotification(ownerId, creatorId, subject, text);
            newNotification.NotificationTypeId =
                _HermesDBEntities.NotificationsTypes.SingleOrDefault(t => t.Name == NotificationType.TicketFlow.ToString()).Id;

            _HermesDBEntities.Notifications.Add(newNotification);
            _HermesDBEntities.SaveChanges();
        }
        #endregion

        #region private methods
        private static Notification CreateNotification(int ownerId, int creatorId, string subject, string text)
        {
            return new Notification()
            {
                CreatorId = creatorId,
                OwnerId = ownerId,
                Subject = subject,
                Description = text,
                CreatedAt = DateTime.UtcNow,
            };
        }
        #endregion
    }

    public enum NotificationType
    {
        Emailing,
        SLA,
        System,
        TicketFlow,
    }
}