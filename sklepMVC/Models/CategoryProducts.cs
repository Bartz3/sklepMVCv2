using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sklepMVCv2.Models
{
    public class CategoryProducts
    {
        public int CategoryProductsID { get; set; }
        public int ProductID { get; set; }
        public int CategoryID { get; set; }
        public virtual Product Product { get; set; }
        public virtual Category Category { get; set; }
    }
}