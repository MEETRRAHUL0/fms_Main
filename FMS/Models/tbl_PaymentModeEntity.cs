using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FMS.Models
{
    [MetadataType(typeof(tbl_PaymentModeMetadata))]
    public class tbl_PaymentMode
    {
    }


    public partial class tbl_PaymentModeMetadata
    {       

        //public int ID { get; set; }

        [Display(Name = "Mode")]
        [Required(ErrorMessage = "Mode is missing")]
        public string Mode { get; set; }
 
    }
}