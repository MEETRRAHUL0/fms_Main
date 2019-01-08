using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FMS
{
    [MetadataType(typeof(tbl_PurchaseMetadata))]
    public partial class tbl_Purchase
    {
        [NotMapped]
        public int Amount { get; set; }
    }

    public partial class tbl_PurchaseMetadata
    {


        //public int AutoID ;   [Display(Name = "Quantity")]
        //public string ID ;   [Display(Name = "Quantity")]

        [Display(Name = "Party Invoice Date")]
        [Required(ErrorMessage = "Party Invoice Date is missing")]
        public Nullable<System.DateTime> PartyInvoiceDate;

        [Display(Name = "Purchase Date")]
        [Required(ErrorMessage = "Purchase Date is missing")]
        public Nullable<System.DateTime> PurchaseDate;

        [Display(Name = "Party Invoice No")]
        [Required(ErrorMessage = "Party Invoice No is missing")]
        public string PartyInvoiceNo;

        [Display(Name = "Party Name")]
        public string PartyName;

        [Display(Name = "Payment Due After")]
        public string PaymentDueAfter;

        [Display(Name = "Reverse Charge")]
        public Nullable<bool> ReverseCharge;


        [Display(Name = "Remark")]
        public string Remark;

        [Display(Name = "Discount Amount")]
        public Nullable<decimal> DiscountAmount;

        [Display(Name = "Other Charge")]
        public Nullable<decimal> OtherCharge;

        [Display(Name = "Total Amount Before Tax")]
        public Nullable<decimal> TotalAmountBeforeTax;

        [Display(Name = "Tax Amount")]
        public Nullable<decimal> TaxAmount;

        [Display(Name = "Total Amount After Tax")]
        public Nullable<decimal> TotalAmountAfterTax;

        [Display(Name = "Round Off Amount")]
        public Nullable<decimal> RoundOff;

        [Display(Name = "Grand Total")]
        public Nullable<decimal> GrandTotal;

        [Display(Name = "Purchase Book")]
        public Nullable<int> PurchaseBook;

        [Display(Name = "Payment Mode")]
        [Required(ErrorMessage = "Please select Payment option")]
        public Nullable<int> PaymentMode;

        [Display(Name = "Scan Copy")]
        public string ScanCopy;

        [Display(Name = "Created On")]
        public string CreatedDatetime;
    }
}