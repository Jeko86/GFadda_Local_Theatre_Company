using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GFadda_Local_Theatre_Company.Models.ViewModels
{
    public class EditStaffViewModel
    {
        //data annotation which  specify a madatory value
        [Display(Name = "First Name")]// data annotation which specify name of the value that will be displayed on the app
        public string FirstName { get; set; }


        [Display(Name = "Last Name")]
        public string LastName { get; set; }


        public string Street { get; set; }


        public string City { get; set; }


        [Display(Name = "Post Code")]
        public string PostCode { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }


    }
}