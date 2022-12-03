using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace sklepMVCv2.Models
{
    public class ExtraFile
    {
        public int ExtraFileID { get; set; }
        [Required]
        [MaxLength(100)]
        [Display(Name = "Nazwa pliku")]
        public string Name { get; set; }
     
        [Display(Name = "Plik")]
        public byte[] File { get; set; }
       
        [MaxLength(500)]
        [Display(Name = "Opis pliku")]
        public string FileDescription { get; set; }
        public int ProductID { get; set; }

        public virtual Product Product { get; set; }
    }
}