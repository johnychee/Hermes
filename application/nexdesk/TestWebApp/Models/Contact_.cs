using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestWebApp.Models
{
    public partial class Contact
    {
        [Required]
        [MaxLength(50)]
        [Display(Name="First Name")]
        public string fName_
        {
            get 
            {
                return this.firstName;
            }
            set 
            {
                this.firstName = value;
            }
        }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Last Name")]
        public string lName_
        {
            get
            {
                return this.lastName;
            }
            set
            {
                this.lastName = value;
            }
        }

        [Required]
        [MaxLength(500)]
        [Display(Name = "Address")]
        public string address_
        {
            get
            {
                return this.address;
            }
            set
            {
                this.address = value;                
            }
        }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Email")]
        public string email_
        {
            get
            {
                return this.email;
            }
            set
            {
                this.email = value;
            }
        }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Phone")]
        public string phone_
        {
            get
            {
                return this.phone;

            }
            set
            {
                this.phone = value;
            }
        }
        public static List<SelectListItem> toSelectList(User user = null)
        {
            HermesDBEntities hdesk = new HermesDBEntities();
            List<SelectListItem> contacts = new List<SelectListItem>();
            foreach (Contact contact in hdesk.Contacts)
            {
                if (user != null && user.Retailer.Contact != null && user.Retailer.contactId == contact.id)
                {
                    SelectListItem sli = new SelectListItem { Text = contact.firstName + " " + contact.lastName, Value = contact.id.ToString() ,Selected=true};
                    contacts.Add(sli);
                }
                else 
                {
                    SelectListItem sli = new SelectListItem { Text = contact.firstName + " " + contact.lastName, Value = contact.id.ToString() };
                    contacts.Add(sli);
                }
            }
            return contacts;
        }
    }
}