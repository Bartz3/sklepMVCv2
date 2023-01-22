using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace sklepMVCv2.Models
{
   
    public class Order
    {
        public int OrderID { get; set; }
     
        [Display(Name = "Data zamówienia")]
        public System.DateTime Date { get; set; }
        [Required]
        [MaxLength(30)]
        [Display(Name = "Status zamówienia")]
        public string Status { get; set; }
        public string UserID { get; set; } // <--- TU BYŁ INT!!!
        [Required]
        [Display(Name = "Cena całkowita")]
        public decimal TotalPrice { get; set; }
        [Required]
        [MaxLength(50)]
        [Display(Name = "Metoda płatności")]
        public string PaymentMethod { get; set; }
        [Required]
        [MaxLength(50)]
        [Display(Name = "Sposób wysyłki")]
        public string ShippingMethod { get; set; }

        public virtual ICollection<OrderProduct> OrderProduct { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
    public enum OrderStatus
    {
        Nowe,
        W_trakcie_realizacji,
        Zrealizowane,
        Anulowane
    }
}