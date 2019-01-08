using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FMS.Models
{

    [MetadataType(typeof(tbl_ItemUnitsMetadata))]
    public partial class tbl_ItemUnits
    {
    }

    public partial class tbl_ItemUnitsMetadata
    {
        

        public int MeasurintUnitID { get; set; }

        [Display(Name = "Measuring Units")]
        [Required(ErrorMessage = "MeasuringUnits is missing")]
        public string MeasuringUnits { get; set; }

        
    }
}