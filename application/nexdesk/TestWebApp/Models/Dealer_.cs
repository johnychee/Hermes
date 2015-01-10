using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestWebApp.Models
{
    public partial class Dealer
    {
        [Required]
        [Display(Name="Customers name")]
        [MaxLength(30)]
        public string name_
        {
            get { return this.name; }
            set { this.name = value; }
        }


        public static List<SelectListItem> toSelectList(User user = null)
        {
            HelpDeskEntities hdesk = new HelpDeskEntities();
            List<SelectListItem> dealers = new List<SelectListItem>();
            
            foreach (Dealer dealer in hdesk.Dealers)
            {
                if (user != null && user.Retailer.Dealer != null && user.Retailer.dealerId == dealer.id)
                {
                    SelectListItem sli = new SelectListItem { Text = dealer.name, Value = dealer.id.ToString(),Selected=true };
                    dealers.Add(sli);
                }
                else
                {
                    SelectListItem sli = new SelectListItem { Text = dealer.name, Value = dealer.id.ToString() };
                    dealers.Add(sli);
                }
            }
            return dealers;
        }
    }
}