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
    
    public partial class Solutionist
    {
        public int userId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<int> companyId { get; set; }
        public string teamviewerID { get; set; }
    
        public virtual Company Company { get; set; }
        public virtual User User { get; set; }
    }
}
