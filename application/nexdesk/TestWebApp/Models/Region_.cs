using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestWebApp.Models
{
    public partial class Region
    {
        [Required]
        [Display(Name="Name")]
        [MaxLength(50)]
        public string name_ 
        {
            get { return this.name; }
            set { this.name = value; }
        }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Response Time must be greather than 0")]
        [Display(Name="Response Time")]
        public int RT_minutes_
        {
            get { return this.RT_minutes; }
            set { this.RT_minutes = value; }
        }


        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Fixed Time must be greather than 0")]
        [Display(Name = "Fixed Time")]
        public int FT_minutes_ 
        {
            get { return this.FT_minutes; }
            set { this.FT_minutes = value; }
        }


        [Required]
        [Display(Name = "Utc Time Shift")]
        public double UTCtimeShift_
        {
            get { return this.UTCtimeShift; }
            set { this.UTCtimeShift = value; }
        }


        public static List<SelectListItem> toSelectList(HelpDeskEntities hDesk = null, User user = null)
        {
            hDesk = hDesk ?? new HelpDeskEntities();
            List<SelectListItem> regions = new List<SelectListItem>();
            foreach (Region region in hDesk.Regions)
            {
                regions.Add(new SelectListItem {
                    Text = region.name, 
                    Value = region.id.ToString(),
                    Selected = (user != null && user.regionId == region.id) // selected is true if user region is equal to 'region'
                });
            }
            return regions;
        }

        public static List<SelectListItem> getTimeZones(Region region = null)
        {
            List<SelectListItem> tZones = new List<SelectListItem>();
            var timeZones = TimeZoneInfo.GetSystemTimeZones(); //get time zones
            foreach(var tz in timeZones) //value is total hour difference from UTC and text is the location exapmple
            {
                SelectListItem sli = new SelectListItem() {
                    Text = tz.DisplayName,
                    Value = tz.Id,
                    Selected = (region != null && region.timeZoneId == tz.Id) // selected is true if regions timezone is 'tz'
                };
                tZones.Add(sli); //add it to our list of  selecte list item
            }
            return tZones;//and finally return it :-)
        }

        public static IEnumerable<Region> all(HelpDeskEntities hDesk = null)
        {
            hDesk = hDesk ?? new HelpDeskEntities();
            return hDesk.Regions;
        }
    }
}