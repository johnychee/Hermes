using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestWebApp.Models
{
    public partial class webpages_Roles
    {
        public string fullname
        {
            get
            {
                if (this.RoleName != "Support")
                    return this.RoleName;
                else
                    return this.RoleName + " level " + this.level.ToString();
            }
        }
        public static List<SelectListItem> toSelectList(bool isSolutionist = true,HelpDeskEntities hDesk = null)
        {
            if (hDesk == null)
                hDesk = new HelpDeskEntities();
            var rolesSortedByLevel = hDesk.webpages_Roles.OrderBy(r => r.level);
            List<SelectListItem> roles = new List<SelectListItem>();
            foreach (webpages_Roles role in rolesSortedByLevel)
            {
                if (isSolutionist)
                {
                    if (role.RoleName != "Retailer" && role.RoleName != "System")
                    {
                        roles.Add(new SelectListItem { Text = role.fullname, Value = role.RoleId.ToString() });
                    }
                }
                else
                {
                    if (role.RoleName == "Retailer")
                    {
                        roles.Add(new SelectListItem { Text = role.fullname, Value = role.RoleId.ToString() });
                    }
                }
            }
            return roles;
        }

        public static void AddUserToTheRole(int id,int level) //add the user to the role
        {
            HelpDeskEntities hDesk = new HelpDeskEntities();
            User user = hDesk.Users.SingleOrDefault(u => u.id == id);
            webpages_Roles role = hDesk.webpages_Roles.SingleOrDefault(r => r.level == level);
            user.webpages_Roles.Add(role);
            hDesk.SaveChanges();
        }
    }
}