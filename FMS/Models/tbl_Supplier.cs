using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FMS
{
    [MetadataType(typeof(tbl_SupplierMetadata))]
    public partial class tbl_Supplier
    {
    }


    public partial class tbl_SupplierMetadata
    {
        

        public string ID ;    

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Supplier Name is missing")]
        public string Name ;

        [Display(Name = "Contact Person")]
        public string ContactPerson ;

        [Display(Name = "Contact Number")]
        public string ContactNumber ;

        [Display(Name = "Type")]
        public string Type ;

        [Display(Name = "Place")]
        public string Place ;

        [Display(Name = "GST Num")]
        public string GST_No ;

        [Display(Name = "TIN Num")]
        public string TIN_No ;

        [Display(Name = "PAN Num")]
        public string PAN_No ;

        [Display(Name = "CIN Nu")]
        public string CIN_No ;

        [Display(Name = "Address Line 1")]
        public string Address_Line1 ;

        [Display(Name = "Address Line 2")]
        public string Address_Line2 ;

        [Display(Name = "City")]
        public string City ;

        [Display(Name = "State")]
        public string State ;

        [Display(Name = "Pin Code")]
        public Nullable<int> Pin_Code ;

        [Display(Name = "Phone Number")]
        public string PhoneNumber ;

        [Display(Name = "Email ID")]
        public string Email ;

        [Display(Name = "Opening Balance")]
        public string OpeningBalance ;

        [Display(Name = "opening Balance Date")]
        public Nullable<System.DateTime> openingBalanceDate ;

        [Display(Name = "Credit Limit")]
        public Nullable<decimal> CreditLimit ;

        [Display(Name = "Credit Period")]
        public Nullable<int> CreditPeriod ;

        [Display(Name = "Credit Intrest Rate")]
        public Nullable<int> CreditIntrestRate ;

        [Display(Name = "Remark")]
        public string Remark ;

        [Display(Name = "Photo")]
        public string Photo ;

        [Display(Name = "Created Date Time")]
        public Nullable<System.DateTime> CreatedDateTime ;

        
    }
}