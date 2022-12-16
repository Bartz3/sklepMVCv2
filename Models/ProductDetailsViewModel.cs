using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sklepMVCv2.Models
{
    public class ProductDetailsViewModel
    {

            public Product Product { get; set; }
            public List<ExtraFile> PdfFiles { get; set; }
        
    }
}