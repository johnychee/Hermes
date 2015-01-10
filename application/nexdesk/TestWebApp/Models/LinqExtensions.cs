using System;
using System.Linq;

namespace TestWebApp.Models
{
    public static class LINQExtension
    {
        // ALL
        public static IQueryable<TSource> page<TSource>(this IQueryable<TSource> source, int page, int pageSize = 10)
        {
            return source.Skip((page - 1) * pageSize).Take(pageSize);
        }

        // TICKETS
        public static IQueryable<Ticket> filter(this IQueryable<Ticket> source, System.Collections.Specialized.NameValueCollection param)
        {
            // ticket Id
            string id = param.Get("filter_id");
            if (id != null)
                source = source.Where(t => t.ticketId.Contains(id));

            // subject
            string subject = param.Get("filter_subject");
            if (subject != null)
                source = source.Where(t => t.subject.Contains(subject));

            // status
            try
            {
                int statusId = Convert.ToInt32(param.Get("filter_status"));
                if (statusId != 0)
                    source = source.Where(t => t.statusId == statusId);
            }
            catch (FormatException) { }

            return source;
        }

        public static IQueryable<Ticket> Order(this IQueryable<Ticket> source, System.Collections.Specialized.NameValueCollection param)
        {
            string orderBy = param.Get("order_by") ?? "createdAt";
            string order = param.Get("order") ?? "asc";

            if (order == "asc")
                switch (orderBy)
                {
                    case "id":
                        source = source.OrderBy(t => t.ticketId);
                        break;
                    case "subject":
                        source = source.OrderBy(t => t.subject);
                        break;
                    case "status":
                        source = source.OrderBy(t => t.Status.name);
                        break;
                    case "RT":
                        source = source.OrderByDescending(t => new { @a = t.Status.name != "New", t.TicketSla.RT_expiresAt });
                        break;
                    case "FT_remote":
                        source = source.OrderByDescending(t => t.TicketSla.FT_expiresAt);
                        break;
                    case "FT_onsite":
                        source = source.OrderByDescending(t => t.TicketSla.FT3lvl_expiresAt);
                        break;
                    case "createdAt":
                        source = source.OrderBy(t => t.createdAt);
                        break;
                    case "createdBy":
                        source = source.OrderBy(t => t.User1.isSolutionist ? t.User1.Solutionist.LastName + t.User1.Solutionist.FirstName : t.User1.Retailer.name);
                        break;
                    case "customer":
                        source = source.OrderBy(t => t.User.isSolutionist ? t.User.Solutionist.LastName + t.User.Solutionist.FirstName : t.User.Retailer.name);
                        break;
                    case "group":
                        source = source.OrderBy(t => t.Group.name);
                        break;
                    case "solutionist":
                        source = source.OrderBy(t => t.User2.isSolutionist ? t.User2.Solutionist.LastName + t.User2.Solutionist.FirstName : t.User2.Retailer.name);
                        break;
                }
            else
                switch (orderBy)
                {
                    case "id":
                        source = source.OrderByDescending(t => t.ticketId);
                        break;
                    case "subject":
                        source = source.OrderByDescending(t => t.subject);
                        break;
                    case "status":
                        source = source.OrderByDescending(t => t.Status.name);
                        break;
                    case "RT":
                        source = source.OrderBy(t => new { @a = t.Status.name != "New", t.TicketSla.RT_expiresAt });
                        break;
                    case "FT_remote":
                        source = source.OrderBy(t => t.TicketSla.FT_expiresAt);
                        break;
                    case "FT_onsite":
                        source = source.OrderBy(t => t.TicketSla.FT3lvl_expiresAt);
                        break;
                    case "createdAt":
                        source = source.OrderByDescending(t => t.createdAt);
                        break;
                    case "createdBy":
                        source = source.OrderByDescending(t => t.CreatedBy_.fullName);
                        break;
                    case "customer":
                        source = source.OrderByDescending(t => t.Author.fullName);
                        break;
                    case "group":
                        source = source.OrderByDescending(t => t.Group.name);
                        break;
                    case "solutionist":
                        source = source.OrderByDescending(t => t.Solutionist_.fullName);
                        break;
                }

            return source;
        }


        // USER
        public static IQueryable<User> orderByName(this IQueryable<User> source)
        {
            return source.OrderBy(u => u.isSolutionist ? u.Solutionist.LastName + u.Solutionist.FirstName : u.Retailer.name);
        }
    }
}