using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GFadda_Local_Theatre_Company.Models.ViewModels
{
    public class CreateCommentViewModel
    {
        public Post Post { get; set; }

        [DataType(DataType.MultilineText)]
        public string CommentBody { get; set; }

        public Category Category { get; set; }
    }

}