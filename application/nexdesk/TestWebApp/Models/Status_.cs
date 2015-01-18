using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestWebApp.Models;

namespace TestWebApp.Models
{
    public partial class Status
    {
        public static Status New(HermesDBEntities hDesk = null)
        {
            hDesk = hDesk ?? new HermesDBEntities();

            return hDesk.Status.Single(s => s.name == "New");
        }
        public static Status AssignedToUser(HermesDBEntities hDesk = null)
        {
            hDesk = hDesk ?? new HermesDBEntities();

            return hDesk.Status.Single(s => s.name == "AssignedToUser");
        }
        public static Status AssignedToGroup(HermesDBEntities hDesk = null)
        {
            hDesk = hDesk ?? new HermesDBEntities();

            return hDesk.Status.Single(s => s.name == "AssignedToGroup");
        }
        public static Status Accepted(HermesDBEntities hDesk = null)
        {
            hDesk = hDesk ?? new HermesDBEntities();

            return hDesk.Status.Single(s => s.name == "Accepted");
        }
        public static Status Done(HermesDBEntities hDesk = null)
        {
            hDesk = hDesk ?? new HermesDBEntities();

            return hDesk.Status.Single(s => s.name == "Done");
        }
        public static Status WaitingForCustomer(HermesDBEntities hDesk = null)
        {
            hDesk = hDesk ?? new HermesDBEntities();

            return hDesk.Status.Single(s => s.name == "WaitingForCustomer");
        }
        public static Status WaitingForExternal(HermesDBEntities hDesk = null)
        {
            hDesk = hDesk ?? new HermesDBEntities();

            return hDesk.Status.Single(s => s.name == "WaitingForExternal");
        }
    }
}