using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWebApp.Models
{
    public partial class Solutionist
    {
        public string FullName
        {
            get { return this.FirstName + " " + this.LastName; }
        }
    }
}