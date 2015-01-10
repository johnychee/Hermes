using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWebApp.Models
{
    public class UsersViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public bool IsChecked { get; set; }
    }
}