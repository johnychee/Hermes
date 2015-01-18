using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestWebApp.Models
{
    public partial class Company
    {
        public static List<SelectListItem> toSelectList(User user = null)
        {
            HermesDBEntities hdesk = new HermesDBEntities();
            List<SelectListItem> companies = new List<SelectListItem>();
            foreach (Company company in hdesk.Companies)
            {
                if (user != null && user.Solutionist.companyId == company.id)
                {
                    SelectListItem sli = new SelectListItem { Text = company.name, Value = company.id.ToString(),Selected=true };
                    companies.Add(sli);
                }
                else
                {
                    SelectListItem sli = new SelectListItem { Text = company.name, Value = company.id.ToString() };
                    companies.Add(sli);
                }
            }
            return companies;
        }
    }
}