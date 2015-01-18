using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestWebApp.Models;
using System.Data.Entity.Infrastructure;
using System.ComponentModel.DataAnnotations;
using WebMatrix.WebData;

namespace TestWebApp.Models
{
    public partial class User
    {
        [Required]
        [Display(Name = "User name")]
        public string username_ 
        {
            get { return this.username; }
            set { this.username = value; }
        }


        public string fullName
        {
            get { return this.isSolutionist ? this.Solutionist.FirstName + " " + this.Solutionist.LastName : this.Retailer.name; }
        }

        public static IQueryable<User> getNonsystemUser(HermesDBEntities hDesk = null)
        {
            hDesk = hDesk ?? new HermesDBEntities();

            return hDesk.Users
                .Where(u => !u.webpages_Roles.Any(r => r.RoleName == "System"))
                .orderByName();
        }

        public static User findByFullname(string fullname, HermesDBEntities hDesk = null)
        {
            hDesk = hDesk ?? new HermesDBEntities();

            return hDesk.Users.SingleOrDefault(u => (u.isSolutionist && u.Solutionist.FirstName + " " + u.Solutionist.LastName == fullname) || u.Retailer.name == fullname);
        }

        public static User current_user(HermesDBEntities hDesk = null)
        {
            hDesk = hDesk ?? new HermesDBEntities();

            return hDesk.Users.Single(u => u.id == WebSecurity.CurrentUserId);
        }
        public static IQueryable<User> inRole(string roleName, int level, HermesDBEntities hDesk = null)
        {
            hDesk = hDesk ?? new HermesDBEntities();

            //return hDesk.Users.SqlQuery("SELECT [dbo].[User].* FROM [dbo].[User] JOIN [dbo].[webpages_UsersInRoles] ON [dbo].[User].[UserId] = [dbo].[webpages_UsersInRoles].[UserId] JOIN [dbo].[webpages_Roles] ON [dbo].[webpages_UsersInRoles].[RoleId] = [dbo].[webpages_Roles].[RoleId] WHERE [dbo].[webpages_Roles].[RoleName] = @p0;", roleName);
            return hDesk.Users.Where
                (
                    u => u.webpages_Roles.Contains
                    (
                        hDesk.webpages_Roles.FirstOrDefault(r => r.RoleName == roleName && (level == null || level == r.level))
                    )
                );
            
        }

        public static IQueryable<User> GetRetailers(HermesDBEntities hDesk = null)
        {
            hDesk = hDesk ?? new HermesDBEntities();
            return hDesk.Users.Where(u => u.Retailer != null);
        }

        public static IQueryable<User> GetCustomerGroupRetailers(int id, HermesDBEntities hDesk = null)
        {
            hDesk = hDesk ?? new HermesDBEntities();
            return hDesk.Users.Where(u => u.Retailer != null && u.Retailer.dealerId == id);
        }
    }
}
