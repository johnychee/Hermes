using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TestWebApp.Models
{
    public partial class TravelContract
    {
        [Required]
        [Display(Name = "Name of Client")]
        public string NameOfClient_
        {
            get { return NameOfClient; }
            set { NameOfClient = value; }
        }

        [Required]
        [Display(Name = "Adress")]
        public string Adress_
        {
            get { return Adress; }
            set { Adress = value; }
        }

        [Required]
        [Display(Name = "Quantity")]
        [Range(0, 1000)]
        public int Quantity_
        {
            get { return Quantity; }
            set { Quantity = value; }
        }

        [Required]
        [Display(Name = "Event")]
        public string eventName { get; set; }
    }
}