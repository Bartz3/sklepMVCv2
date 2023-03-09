using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace sklepMVCv2.Models
{
    public class VisitCount
    {
        public int VisitCountID { get; set; }
        [Required]
        [Display(Name = "Liczba odwiedzin")]
        public int VisitsNumber { get; set; }
    }
}