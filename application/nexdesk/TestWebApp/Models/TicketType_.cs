using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestWebApp.Models
{
    public partial class TicketType
    {
        public static TicketType Incident(HelpDeskEntities hDesk = null)
        {
            hDesk = hDesk ?? new HelpDeskEntities();

            return hDesk.TicketTypes.Single(tt => tt.name == "Incident");
        }

        public static TicketType Request(HelpDeskEntities hDesk = null)
        {
            hDesk = hDesk ?? new HelpDeskEntities();

            return hDesk.TicketTypes.Single(tt => tt.name == "Request");
        }

        public static List<SelectListItem>all_toSelectList(Ticket currentTicket = null,HelpDeskEntities hDesk = null)
        {
            hDesk = hDesk ?? new HelpDeskEntities();
            List<SelectListItem> types = new List<SelectListItem>();
            foreach (TicketType type in hDesk.TicketTypes)
            {
                if (currentTicket != null && currentTicket.TicketType != null && type.name == currentTicket.TicketType.name)
                {
                    SelectListItem sli = new SelectListItem() { Text = type.name, Value = type.id.ToString(), Selected = true };
                    types.Add(sli);
                }
                else
                {
                    SelectListItem sli = new SelectListItem() { Text = type.name, Value = type.id.ToString() };
                    types.Add(sli);
                }
            }
            return types;
        }
    }
}