using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TestWebApp.Models
{
    public partial class TravelEvent
    {
        [Required]
        [Display(Name = "Jméno zájezdu")]
        public string Name_
        {
            get { return Name; }
            set { Name = value; }
        }


        [Required]
        [Display(Name = "Popis")]
        public string Description_
        {
            get { return Description; }
            set { Description = value; }
        }

        [Required]
        [Display(Name = "Cena za jednotlivce")]
        [Range(0, 5000000)]
        public int Price_
        {
            get { return Price; }
            set { Price = value; }
        }

        [Required]
        [Display(Name = "Kapacity")]
        [Range(0, 1000)]
        public int Capacity_
        {
            get { return Capacity; }
            set { Capacity = value; }
        }
    }
}