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
        [Display(Name = "Jméno klienta")]
        public string NameOfClient_
        {
            get { return NameOfClient; }
            set { NameOfClient = value; }
        }

        [Required]
        [Display(Name = "Adresa")]
        public string Adress_
        {
            get { return Adress; }
            set { Adress = value; }
        }

        [Required]
        [Display(Name = "Počet lidí")]
        [Range(0, 1000)]
        public int Quantity_
        {
            get { return Quantity; }
            set { Quantity = value; }
        }

        [Required]
        [Display(Name = "Zájezd")]
        public string eventName { get; set; }
    }
}