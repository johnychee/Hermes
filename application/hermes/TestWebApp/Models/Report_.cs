using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TestWebApp.Models
{
    public partial class Report
    {
        [Required]
        [Display(Name = "Vlastnost Od")]
        public DateTime PeriodFrom_
        {
            get { return PeriodFrom; }
            set { PeriodFrom = value; }
        }


        [Required]
        [Display(Name = "Vlastnost Do")]
        public DateTime PeriodTo_
        {
            get { return PeriodTo; }
            set { PeriodTo = value; }
        }

        [Required]
        [Display(Name = "Jméno skupiny")]
        public string groupName { get; set; }
    }
}