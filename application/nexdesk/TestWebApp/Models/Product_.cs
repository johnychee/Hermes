using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TestWebApp.Models
{
    public partial class Product
    {
        [Required]
        [MaxLength(50)]
        [Display(Name = "Name")]
        public string name_
        {
            get { return this.name; }
            set { this.name = value; }
        }

        [Required]
        public string CategoryId_
        {
            get { return this.categoryId.ToString(); }
            set { this.categoryId = Convert.ToInt32(value); }
        }

        [MaxLength(1000)]
        [Display(Name = "Description")]
        public string description_
        {
            get { return this.description; }
            set { this.description = value; }
        }

        [MaxLength(50)]
        [Display(Name = "Garant")]
        public string garant_
        {
            get { return this.garant; }
            set { this.garant = value; }
        }

        [MaxLength(50)]
        [Display(Name = "Supplier")]
        public string supplier_
        {
            get { return this.supplier; }
            set { this.supplier = value; }
        }
    }
}