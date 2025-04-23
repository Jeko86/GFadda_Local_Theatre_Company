using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GFadda_Local_Theatre_Company.Models.ViewModels
{
    public class CreatePostViewModel
    {
        [Key]
        public int PostId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Data Posted")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:d}")]

        public DateTime DatePosted { get; set; }
        public bool IsApproved { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Upload movie poster")]
        public string File { get; set; }

        public ICollection<SelectListItem> Categories { get; set; }
        [Required, Display(Name = "Category")]
        public Category Category { get; set; }


        [ForeignKey("Staff")]
        public string UserId { get; set; }
        public Staff Staff { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        


        public List<Comment> Comments { get; set; }


    }
}