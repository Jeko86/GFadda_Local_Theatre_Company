using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GFadda_Local_Theatre_Company.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }

        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage ="insert text")]
        public string CommentBody { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyy hh.mm tt}")]
        [Display(Name = "Date Posted")]
        public DateTime DatePosted { get; set; }

        public bool IsApproved { get; set; }


        //******************Navigational Property*********************\\


        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("Post")]
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}