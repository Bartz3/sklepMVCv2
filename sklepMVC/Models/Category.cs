using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace sklepMVCv2.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        [Required]
        [MaxLength(100)]
        [Display(Name = "Nazwa kategorii")]
        public string CategoryName { get; set; }

        public virtual ICollection<CategoryProducts> CategoryProducts { get; set; }
    }
}