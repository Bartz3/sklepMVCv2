using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace sklepMVCv2.Models
{
    public class OrderProduct
    {
        public int OrderProductID { get; set; }
        [Required]
        [Display(Name = "Ilość")]
        public int Amount { get; set; }

        [Display(Name = "Numer produktu")]
        public int ProductID { get; set; }
        [Display(Name ="Numer zamówienia")]
        public int OrderID { get; set; }

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}