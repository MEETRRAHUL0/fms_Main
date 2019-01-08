using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FMS.Models
{

    [MetadataType(typeof(tbl_ItemTypeMetadata))]
    public class tbl_ItemType
    {
    }


    public partial class tbl_ItemTypeMetadata
    {
         

        public int ItemTypeId { get; set; }

        [Display(Name = "Item Type")]
        [Required(ErrorMessage = "Item Type is missing")]
        public string ItemType { get; set; }

         
    }
}