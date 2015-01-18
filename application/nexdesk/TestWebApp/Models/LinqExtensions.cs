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

        // USER
        public static IQueryable<User> orderByName(this IQueryable<User> source)
        {
            return source.OrderBy(u => u.isSolutionist ? u.Solutionist.LastName + u.Solutionist.FirstName : u.Retailer.name);
        }
    }
}