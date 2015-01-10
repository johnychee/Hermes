using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TestWebApp.Views.User.UserViewModel
{
    public class RegisterSolutionistViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string username { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Display(Name = "Company")]
        public int companyId { get; set; }

        [Display(Name = "TeamViewer Id")]
        public string teamViewerId { get; set; }

        [Required]
        [Display(Name = "Region")]
        public int regionId { get; set; }

        [Required]
        [Display(Name = "Role")]
        public int roleId { get; set; }
        
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