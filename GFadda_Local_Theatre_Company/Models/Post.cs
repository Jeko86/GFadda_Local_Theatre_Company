using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GFadda_Local_Theatre_Company.Models
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name ="Data Posted")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString ="{0:d}")]

        public DateTime DatePosted { get; set; }

        public bool IsApproved { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Upload Theatre poster")]
        public string File { get; set; }


        //******************Navigational Property*********************\\

        [ForeignKey("Staff")]
        public string UserId { get; set; }
        public Staff Staff { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }


        public List<Comment> Comments { get; set; }
    }
}