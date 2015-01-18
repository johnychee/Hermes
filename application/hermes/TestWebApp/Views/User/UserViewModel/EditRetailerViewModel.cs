using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TestWebApp.Views.User.UserViewModel
{
    public class EditRetailerViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string username { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "OldPassword")]
        public string OldPassword { get; set; }


        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "NewPassword")]
        public string NewPassword { get; set; }


        [Required]
        [Display(Name = "Name")]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "City")]
        [MaxLength(30)]
        public string City { get; set; }

        [Display(Name = "Dealer")]
        public int? dealerId { get; set; }

        [Display(Name = "Workstation")]
        public int? workStationId { get; set; }


        [Required]
        [Display(Name = "Role")]
        public int roleId { get; set; }

        [Required]
        [Display(Name = "Address")]
        [MaxLength(100)]
        public string address { get; set; }

        [Required]
        [Display(Name = "Zip Code")]
        [MaxLength(10)]
        public string zipcode { get; set; }

        
        [Display(Name = "Contact Person")]
        public int? contactId { get; set; }

        [Required]
        [Display(Name = "Region")]
        public int regionId { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        public string phone { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email")]
        public string email { get; set; }

        public string profilePictureUrl { get; set; }
    }
}