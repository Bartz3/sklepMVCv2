using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace sklepMVCv2.Models
{
    public class Product
    {
        [Display(Name ="ID")]
        public int ProductID { get; set; }
        [Required]
        [MaxLength(100)]
        [Display(Name = "Nazwa produktu")]
        public string Name { get; set; }
        [Required]
        [MaxLength(1000)]
        [Display(Name = "Opis produktu")]
        public string Description { get; set; }
        [Required]
        [Range(0,int.MaxValue,ErrorMessage ="Podana cena jest nieprawidłowa.")]
        [Display(Name = "Cena")]
        public decimal Price { get; set; }
        [Display(Name ="Obrazek mały")]
        public byte[] SmallImage { get; set; }
        [Display(Name ="Obrazek duży")]
        public byte[] BigImage { get; set; }
     
        [Display(Name = "Ilość")]
        public int Quantity { get; set; }
        [Display(Name = "Stawka VAT")]
        public int VatID { get; set; }


        public virtual ICollection<ExtraFile> ExtraFile { get; set; }

        public virtual ICollection<OrderProduct> OrderProduct { get; set; }
        public virtual Vat Vat { get; set; }
        [Display(Name="Kategoria")]
        public virtual ICollection<Category> Category { get; set; }
    }
}