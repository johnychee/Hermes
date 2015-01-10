using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWebApp.Models
{
    public partial class HistoryAction
    {
        public static HistoryAction Created(HelpDeskEntities hDesk = null)
        {
            if (hDesk == null)
                hDesk = new HelpDeskEntities();

            return hDesk.HistoryActions.FirstOrDefault(a => a.name == "Created");

        }
        public static HistoryAction AssignedToGroup(HelpDeskEntities hDesk = null)
        {
            if (hDesk == null)
                hDesk = new HelpDeskEntities();

            return hDesk.HistoryActions.FirstOrDefault(a => a.name == "AssignedToGroup");

        }
        public static HistoryAction Accepted(HelpDeskEntities hDesk = null)
        {
            if (hDesk == null)
                hDesk = new HelpDeskEntities();

            return hDesk.HistoryActions.FirstOrDefault(a => a.name == "Accepted");

        }
        public static HistoryAction AssignedToUser(HelpDeskEntities hDesk = null)//asigned ticket to the solutionist
        {
            if (hDesk == null)
                hDesk = new HelpDeskEntities();

            return hDesk.HistoryActions.FirstOrDefault(a => a.name == "AssignedToUser");

        }
        public static HistoryAction SendToCustomer(HelpDeskEntities hDesk = null)
        {
            if (hDesk == null)
                hDesk = new HelpDeskEntities();

            return hDesk.HistoryActions.FirstOrDefault(a => a.name == "SendToCustomer");

        }
        public static HistoryAction SendTo3rdParty(HelpDeskEntities hDesk = null)
        {
            if (hDesk == null)
                hDesk = new HelpDeskEntities();

            return hDesk.HistoryActions.FirstOrDefault(a => a.name == "SendTo3rdParty");

        }
        public static HistoryAction Completed(HelpDeskEntities hDesk = null)
        {
            if (hDesk == null)
                hDesk = new HelpDeskEntities();

            return hDesk.HistoryActions.FirstOrDefault(a => a.name == "Completed");

        }

        public static HistoryAction TypeChanged(HelpDeskEntities hDesk)
        {
            return hDesk.HistoryActions.SingleOrDefault(ha => ha.name == "TypeChanged");
        }
    }
}