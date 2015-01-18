using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWebApp.Models
{
    public partial class HistoryAction
    {
        public static HistoryAction Created(HermesDBEntities hDesk = null)
        {
            if (hDesk == null)
                hDesk = new HermesDBEntities();

            return hDesk.HistoryActions.FirstOrDefault(a => a.name == "Created");

        }
        public static HistoryAction AssignedToGroup(HermesDBEntities hDesk = null)
        {
            if (hDesk == null)
                hDesk = new HermesDBEntities();

            return hDesk.HistoryActions.FirstOrDefault(a => a.name == "AssignedToGroup");

        }
        public static HistoryAction Accepted(HermesDBEntities hDesk = null)
        {
            if (hDesk == null)
                hDesk = new HermesDBEntities();

            return hDesk.HistoryActions.FirstOrDefault(a => a.name == "Accepted");

        }
        public static HistoryAction AssignedToUser(HermesDBEntities hDesk = null)//asigned ticket to the solutionist
        {
            if (hDesk == null)
                hDesk = new HermesDBEntities();

            return hDesk.HistoryActions.FirstOrDefault(a => a.name == "AssignedToUser");

        }
        public static HistoryAction SendToCustomer(HermesDBEntities hDesk = null)
        {
            if (hDesk == null)
                hDesk = new HermesDBEntities();

            return hDesk.HistoryActions.FirstOrDefault(a => a.name == "SendToCustomer");

        }
        public static HistoryAction SendTo3rdParty(HermesDBEntities hDesk = null)
        {
            if (hDesk == null)
                hDesk = new HermesDBEntities();

            return hDesk.HistoryActions.FirstOrDefault(a => a.name == "SendTo3rdParty");

        }
        public static HistoryAction Completed(HermesDBEntities hDesk = null)
        {
            if (hDesk == null)
                hDesk = new HermesDBEntities();

            return hDesk.HistoryActions.FirstOrDefault(a => a.name == "Completed");

        }

        public static HistoryAction TypeChanged(HermesDBEntities hDesk)
        {
            return hDesk.HistoryActions.SingleOrDefault(ha => ha.name == "TypeChanged");
        }
    }
}