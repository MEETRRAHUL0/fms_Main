using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FMS
{
    [MetadataType(typeof(tbl_ItemsMetadata))]
    public partial class tbl_Items
    {
    }

    public partial class tbl_ItemsMetadata
    {

        [Display(Name = "Item Id")]
        public string ID;


        [Display(Name = "Item Name")]
        [Required(ErrorMessage = "Item ID Required")]
        public string Name;

        [Display(Name = "HSN SAC Number")]
        [Required(ErrorMessage = "HSN SAC Number Required")]
        public string HSN_SAC_NO;

        [Display(Name = "Type")]
        [Required(ErrorMessage = "Type Required")]
        public Nullable<int> Type ;

        [Display(Name = "Measuring Unit")]
        [Required(ErrorMessage = "Measuring Unit Required")]
        public Nullable<int> MeasuringUnit;

        [Display(Name = "Manufacture")]
        public string Manufacture ;

        [Display(Name = "Bar Code")]
        public string BarCode ;
         
        [Display(Name = "Item Unique Description")]
        public string ItemUniqueDescription ;

        [Display(Name = "Supplier")]
        [Required(ErrorMessage = "Supplier Required")]
        public string Supplier ;

        [Display(Name = "Photo")]
        public string Photo ;

        [Display(Name = "Use Batch No")]
        public Nullable<bool> UseBatchNo ;

        [Display(Name = "Use Mfg Date")]
        public Nullable<bool> UseMfgDate ;

        [Display(Name = "Use Expiry Date")]
        public Nullable<bool> UseExpiryDate ;

        [Display(Name = "Created On")]
        public Nullable<System.DateTime> CreatedDatetime ;

        [Required(ErrorMessage = "GST Required")]
        public string GST;

      

        //[Display(Name = "OpeningStock")]
        //[Required(ErrorMessage = "Opening Stock Required")]
        //public string OpeningStock { get; set; }

    }
}