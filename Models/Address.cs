using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace sklepMVCv2.Models
{
    public class Address
    {

        public int AddressID{ get; set; }
        [Required]
        [MaxLength(100)]
        [Display(Name ="Miasto")]
        public string City { get; set; }
        [Required]
        [MaxLength(100)]
        [Display(Name = "Ulica")]
        public string Street { get; set; }
        [DisplayName("Numer mieszkania")]
        public Nullable<int> HouseNumber { get; set; }
        [Required]
        [MaxLength(10)]
        [Display(Name = "Kod pocztowy")]
        public string ZipCode { get; set; }
        public virtual ICollection<ApplicationUser> User { get; set; }
    }
}