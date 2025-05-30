﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GFadda_Local_Theatre_Company.Models.ViewModels
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Please, Insert your first name")]//data annotation which  specify a madatory value
        [Display(Name = "* First Name")]// data annotation which specify name of the value that will be displayed on the app
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please, Insert your Last name")]
        [Display(Name = "* Last Name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Please, Insert your Address")]
        [Display(Name = "* Street")]
        public string Street { get; set; }
        [Required(ErrorMessage = "Please, Insert the city of residence")]
        [Display(Name = "* City")]
        public string City { get; set; }
        [Required(ErrorMessage = "Please, Insert your post code")]
        [Display(Name = "* Post Code")]
        public string PostCode { get; set; }

        //[Required(ErrorMessage = "Please, Provide a mobile number")]
        [Display(Name = "Mobile number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{8})$", ErrorMessage = "Please, enter with a valid phone number")]//role to write the mobile number correctly
        public virtual string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please, Insert your email address")]
        [EmailAddress]
        [Display(Name = "* Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please, Insert a new password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "* Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please, confirm your password")]
        [DataType(DataType.Password)]
        [Display(Name = "* Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
