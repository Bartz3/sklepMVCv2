using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace sklepMVCv2.Models
{
    public class Complaint
    {
        public int ComplaintID { get; set; }
        [Required]
        [MaxLength(1000)]
        [Display(Name = "Treść zgłoszenia")]
        public string Text { get; set; }
        public int UserID { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}