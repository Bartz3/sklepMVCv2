using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace sklepMVCv2.Models
{
    public class Vat
    {

        public int VatID { get; set; }
        [Required]
        [Display(Name = "Stawka VAT")]
        public decimal VatRate { get; set; }
        public virtual ICollection<Product> Product { get; set; }
    }
}