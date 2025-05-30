﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GFadda_Local_Theatre_Company.Models.ViewModels
{
    public class ChangeRoleViewModel
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ActualRole { get; set; }

        public ICollection<SelectListItem> Roles { get; set; }
        [Required, Display(Name = "Role")]
        public string Role { get; set; }
    }
}