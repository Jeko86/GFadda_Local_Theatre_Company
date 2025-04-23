using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GFadda_Local_Theatre_Company.Models
{
    //child class inherited to User
    public class Staff : User
    {
        //navigational property
        public List<Post> Posts { get; set; }
    }
}