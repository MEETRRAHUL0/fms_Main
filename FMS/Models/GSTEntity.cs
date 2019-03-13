using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FMS.Models
{
    [MetadataType(typeof(tbl_GSTMetadata))]
    public partial class tbl_GST
    {
    }


    public partial class tbl_GSTMetadata
    {
        

        public int AutoID { get; set; }
        public string ID { get; set; }

        [Display(Name = "GST")]
        [Required(ErrorMessage = "GST is missing")]
        public Nullable<decimal> GST { get; set; }

        [Display(Name = "SGST")]
        [Required(ErrorMessage = "SGST is missing")]
        public Nullable<decimal> SGST { get; set; }

        [Display(Name = "CGST")]
        [Required(ErrorMessage = "CGST is missing")]
        public Nullable<decimal> CGST { get; set; }
        public Nullable<decimal> IGST { get; set; }
        public string Comment { get; set; }

        [Display(Name = "Display Name")]
        [Required(ErrorMessage = "Display Name is missing")]
        public string Name { get; set; }

        public int Ordinal { get; set; }
         
    }
}