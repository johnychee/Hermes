//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TestWebApp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProductCategory
    {
        public ProductCategory()
        {
            this.ProductCategories1 = new HashSet<ProductCategory>();
            this.Products = new HashSet<Product>();
        }
    
        public int id { get; set; }
        public Nullable<int> topId { get; set; }
        public string name { get; set; }
    
        public virtual ICollection<ProductCategory> ProductCategories1 { get; set; }
        public virtual ProductCategory ProductCategory1 { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}