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
    
    public partial class Workstation
    {
        public Workstation()
        {
            this.Retailers = new HashSet<Retailer>();
        }
    
        public int id { get; set; }
        public string wsName { get; set; }
        public string wsOwner { get; set; }
        public string wsManufacturer { get; set; }
        public string osType { get; set; }
    
        public virtual ICollection<Retailer> Retailers { get; set; }
    }
}
