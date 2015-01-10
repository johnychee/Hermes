using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestWebApp.Models;

namespace TestWebApp.Models
{
    public partial class Status
    {
        public static Status New(HelpDeskEntities hDesk = null)
        {
            hDesk = hDesk ?? new HelpDeskEntities();

            return hDesk.Status.Single(s => s.name == "New");
        }
        public static Status AssignedToUser(HelpDeskEntities hDesk = null)
        {
            hDesk = hDesk ?? new HelpDeskEntities();

            return hDesk.Status.Single(s => s.name == "AssignedToUser");
        }
        public static Status AssignedToGroup(HelpDeskEntities hDesk = null)
        {
            hDesk = hDesk ?? new HelpDeskEntities();

            return hDesk.Status.Single(s => s.name == "AssignedToGroup");
        }
        public static Status Accepted(HelpDeskEntities hDesk = null)
        {
            hDesk = hDesk ?? new HelpDeskEntities();

            return hDesk.Status.Single(s => s.name == "Accepted");
        }
        public static Status Done(HelpDeskEntities hDesk = null)
        {
            hDesk = hDesk ?? new HelpDeskEntities();

            return hDesk.Status.Single(s => s.name == "Done");
        }
        public static Status WaitingForCustomer(HelpDeskEntities hDesk = null)
        {
            hDesk = hDesk ?? new HelpDeskEntities();

            return hDesk.Status.Single(s => s.name == "WaitingForCustomer");
        }
        public static Status WaitingForExternal(HelpDeskEntities hDesk = null)
        {
            hDesk = hDesk ?? new HelpDeskEntities();

            return hDesk.Status.Single(s => s.name == "WaitingForExternal");
        }
    }
}