using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;


namespace GFadda_Local_Theatre_Company.Models.ViewModels
{
    public class PostCommentsViewModel
    {       
        public Post Post { get; set; }
        public List<Comment> Comments { get; set; }
    }
}