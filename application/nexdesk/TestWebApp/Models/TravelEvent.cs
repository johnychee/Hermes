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
    
    public partial class TravelEvent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Capacity { get; set; }
        public int TravelDestinationId { get; set; }
        public int Price { get; set; }
    
        public virtual TravelDestination TravelDestination { get; set; }
    }
}
