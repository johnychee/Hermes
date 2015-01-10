using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestWebApp.Models
{
    public partial class Group
    {
        [Required(ErrorMessage = "Group Name is required")]
        public string name_
        {
            get { return this.name; }
            set { this.name = value; }
        }

        [Required(ErrorMessage = "Support Group Level is required")]
        [Range(1, 4, ErrorMessage = "The field Support Group Level must be between 1 and 4")]
        public byte level_ 
        {
            get { return this.level; }
            set { this.level = value; }
        }

        public static List<SelectListItem> all_toSelectList(int? level = null, int? userId = null, HelpDeskEntities hDesk = null)
        {
            hDesk = hDesk ?? new HelpDeskEntities();

            List<SelectListItem> groups = new List<SelectListItem>();
            foreach(Group group in hDesk.Groups.Where(g => (level == null || g.level == level) &&
                                                           (userId == null || g.Users.Any(gu => gu.id == userId))
                                                     ))
            {
                groups.Add(new SelectListItem { Text = group.name + " (Level: " + group.level + ") ", Value = group.id.ToString() });
            }

            return groups;
        }

        public static IEnumerable<Group> getWithName_onUserLvl(string name, int userId, bool isFullName, HelpDeskEntities hDesk = null)
        {
            hDesk = hDesk ?? new HelpDeskEntities();

            // všechny groupy daného jména
            if (userId == 0)
            {
                if (isFullName)
                    return hDesk.Groups.Where(g => g.name == name);
                else
                    return hDesk.Groups.Where(g => g.name.Contains(name));
            }
            // groupy daného jména a na stejném lvl jako uživatel
            else
            {
                if (isFullName)
                    return hDesk.Groups.SqlQuery("SELECT dbo.Groups.* FROM dbo.Groups WHERE dbo.Groups.name = @p0 AND dbo.Groups.level IN (SELECT dbo.webpages_Roles.level FROM dbo.webpages_Roles JOIN dbo.webpages_UsersInRoles ON dbo.webpages_Roles.roleId = dbo.webpages_UsersInRoles.roleId WHERE dbo.webpages_UsersInRoles.userId = @p1);", name, userId);
                else
                    return hDesk.Groups.SqlQuery("SELECT dbo.Groups.* FROM dbo.Groups WHERE dbo.Groups.name LIKE @p0 AND dbo.Groups.level IN (SELECT dbo.webpages_Roles.level FROM dbo.webpages_Roles JOIN dbo.webpages_UsersInRoles ON dbo.webpages_Roles.roleId = dbo.webpages_UsersInRoles.roleId WHERE dbo.webpages_UsersInRoles.userId = @p1);", String.Format("%{0}%", name), userId);
            }
        }
    }
}