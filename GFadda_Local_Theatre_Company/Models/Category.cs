using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GFadda_Local_Theatre_Company.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Display(Name ="Category")]
        public string Name { get; set; }



        //******************Navigational Property*********************\\



        public List<Post> Posts { get; set; }


    }
}