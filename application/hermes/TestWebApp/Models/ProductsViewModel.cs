using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWebApp.Models
{
    public class ProductsViewModel
    {
        public int Id {get;set;}
        public string Name { get; set; }
        public string Supplier { get; set; }
        public bool IsSelected { get; set; }
    }
}