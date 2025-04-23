using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GFadda_Local_Theatre_Company.Models.ViewModels
{
    public class CreateStaffViewModel
    {
        [Required]//data annotation which  specify a madatory value
        [Display(Name = "First Name")]// data annotation which specify name of the value that will be displayed on the app
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        [Display(Name = "Post Code")]
        public string PostCode { get; set; }

        [Display(Name = "Registration date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime RegistrationDate { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Email Confirm")]
        public bool EmailConfirm { get; set; }

        [Display(Name = "Phone Confirm")]
        public bool PhoneConfirm { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        public ICollection<SelectListItem> Roles { get; set; }
        [Required, Display(Name = "Role")]
        public string Role { get; set; }
    }

}
