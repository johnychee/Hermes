using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestWebApp.Models
{
    public partial class Workstation
    {
        [Required]
        [Display(Name="Name")]
        [MaxLength(25)]
        public string wsName_
        {
            get { return this.wsName; }
            set { this.wsName = value; }
        }

        [Required]
        [Display(Name = "Owner")]
        [MaxLength(25)]
        public string wsOwner_
        {
            get { return this.wsOwner; }
            set { this.wsOwner = value; }
        }

        [Required]
        [Display(Name = "Manufacturer")]
        [MaxLength(25)]
        public string wsManufacturer_
        {
            get { return this.wsManufacturer; }
            set { this.wsManufacturer = value; }
        }

        [Required]
        [Display(Name = "Operating System")]
        [MaxLength(25)]
        public string osType_
        {
            get { return this.osType; }
            set { this.osType = value; }
        }


        public static List<SelectListItem> toSelectList(User user  = null)
        {
            HelpDeskEntities hdesk = new HelpDeskEntities();
            List<SelectListItem> workstations = new List<SelectListItem>();
            foreach (Workstation w in hdesk.Workstations)
            {

                if (user != null && user.Retailer.Workstation != null && user.Retailer.Workstation.id == w.id)
                    {
                        SelectListItem sli = new SelectListItem { Text = w.wsName, Value = w.id.ToString(), Selected = true };
                        workstations.Add(sli);
                    }
                    else
                    {
                        SelectListItem sli = new SelectListItem { Text = w.wsName, Value = w.id.ToString() };
                        workstations.Add(sli);
                    }
                
            }
            return workstations;
        }
    }
}