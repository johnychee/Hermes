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

        // USER
        public static IQueryable<User> orderByName(this IQueryable<User> source)
        {
            return source.OrderBy(u => u.isSolutionist ? u.Solutionist.LastName + u.Solutionist.FirstName : u.Retailer.name);
        }
    }
}