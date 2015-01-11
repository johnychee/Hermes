using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Xml;
using NexDesk.MailProvider;

namespace TestWebApp.Models
{
    /// <summary>
    /// Class that makes appropriate objects of type Message for all kind off events
    /// </summary>
    public class MailTemplatesProcessor
    {
        private Dictionary<string,string> dictionaryOfTicketInformations = new Dictionary<string, string>();

        //Constants
        private const string TicketID = "{TicketID}";
        private const string Type = "{Type}";
        private const string Solutionist = "{Solutionist}";
        private const string State = "{State}";
        private const string Classification = "{Classification}";
        private const string Region = "{Region}";
        private const string LocalTime = "{LocalTime}";
        private const string Customer = "{Customer}";
        private const string Product = "{Product}";
        private const string Title = "{Title}";
        private const string Description = "{Description}";
        private const string DetailsUrl = "{DetailsUrl}";
        private const string HDTime = "{HDTime}";
        private const string Group = "{Group}";
        private const string RTRemainingHours = "{RTRemainingHours}";
        private const string FTRemainingHours = "{FTRemainingHours}";
        private const string OFTRemainingHours = "{OFTRemainingHours}";
        private const string ResponseTime = "{ResponseTime}";
        private const string UrlToAcceptDirectly = "{UrlToAcceptDirectly}";
        private const string AcceptDirectlyAsRequestUrl = "{AcceptDirectlyAsRequestUrl}";
        private const string AcceptDirectlyAsIncidentUrl = "{AcceptDirectlyAsIncidentUrl}";
        private const string PasswordResetLink = "{PasswordResetLink}";
        private const string NewPassword = "{NewPassword}";

        private const string Default = "Default";

        //TemplatesPaths
        private const string TemplateTicketAcceptedPath = "~/Content/MailTemplates/TicketAccepted.xml";
        private const string TemplateTicketAssignedToGroupPath = "~/Content/MailTemplates/AssignedToGroup.xml";
        private const string TemplateTicketAssignedToUserPath = "~/Content/MailTemplates/AssignedToUser.xml";
        private const string TemplateTicketSolvedPath = "~/Content/MailTemplates/TicketSolved.xml";
        private const string TemplateTicketSolvedForCustomerPath = "~/Content/MailTemplates/TicketSolvedForCustomer.xml";
        private const string TemplateTicketCreatedForCustomerPath = "~/Content/MailTemplates/TicketCreatedForCustomer.xml";
        private const string TemplateComplaintFromCustomerPath = "~/Content/MailTemplates/ComplaintFromCustomer.xml";
        private const string TemplateForgotPasswordPath = "~/Content/MailTemplates/ForgotPassword.xml";
        private const string TemplateNewPasswordPath = "~/Content/MailTemplates/NewPassword.xml";

        private string subject;
        public string Subject
        {
            get { return subject; }
        }

        private string message;
        public string Message
        {
            get { return message; }
        }

        /// <summary>
        /// Creates the mail message for forgot password.
        /// </summary>
        /// <param name="resetLink">The reset link.</param>
        /// <returns></returns>
        public Message CreateMailMessageForgotPassword(string resetLink)
        {
            LoadMailTemplate(TemplateForgotPasswordPath);
            ReplaceKeyWithValueFromMessage(PasswordResetLink, resetLink);

            return CreateMessageObject();
        }

        /// <summary>
        /// Creates the mail message for new password.
        /// </summary>
        /// <param name="newPassword">The new password.</param>
        /// <returns></returns>
        public Message CreateMailMessageNewPassword(string newPassword)
        {
            LoadMailTemplate(TemplateNewPasswordPath);
            ReplaceKeyWithValueFromMessage(NewPassword, newPassword);

            return CreateMessageObject();
        }

        /// <summary>
        /// Loads the mail template.
        /// </summary>
        /// <param name="templatePath">The template path.</param>
        private void LoadMailTemplate(string templatePath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(HttpContext.Current.Server.MapPath(templatePath));
            XmlElement root = doc.DocumentElement;
            subject = root.FirstChild.InnerText;
            message = root.LastChild.InnerText;
        }

        /// <summary>
        /// Replaces the key with value from message.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        private void ReplaceKeyWithValueFromMessage(string key,string value)
        {
            subject = subject.Replace(key, value);
            message = message.Replace(key, value);
        }

        private Message CreateMessageObject()
        {
            return new Message()
            {
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };
        }
    }
}