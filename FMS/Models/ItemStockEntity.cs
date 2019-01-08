using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FMS
{
    [MetadataType(typeof(tbl_ItemStockMetadata1))]
    public partial class tbl_ItemStock1
    {
    }

    public partial class tbl_ItemStockMetadata1
    {
        [Display(Name = "No.")]
        public int AutoID ;

        [Display(Name = "Item Id")]
        public string ID ;

        [Display(Name = "Stock Type")]
        public string StockType ;

        [Display(Name = "Purchase ID")]
        public string PurchaseID ;

        [Display(Name = "Sale ID")]
        public string SaleID ;

        [Display(Name = "Invoice Num")]
        public string InvoiceNo ;

        [Display(Name = "Item ID")]
        public string ItemID ;

        [Display(Name = "Qty")]
        [Required(ErrorMessage = "Qty")]
        public Nullable<int> Qty ;

        [Display(Name = "Price Per Unit")]
        public Nullable<decimal> PricePerUnit ;

        [Display(Name = "Discount Per Unit")]
        public Nullable<decimal> ItemwiseDiscount ;

        [Display(Name = "Price Per Unit After Discount")]
        public Nullable<decimal> PricePerUnitAfterDiscount ;

        [Display(Name = "Total Price Before Tax")]
        public Nullable<decimal> TotalPriceBeforeTax ;

        [Display(Name = "SGST")]
        public Nullable<decimal> SGST ;

        [Display(Name = "CGST")]
        public Nullable<decimal> CGST ;

        [Display(Name = "IGST")]
        public Nullable<decimal> IGST ;

        [Display(Name = "GST")]
        public Nullable<decimal> GST ;

        [Display(Name = "Total Price After Tax")]
        public Nullable<decimal> TotalPriceAfterTax ;

        [Display(Name = "MRP")]
        public Nullable<decimal> MRP ;

        [Display(Name = "Manufacture Date")]
        public Nullable<System.DateTime> ManufactureDate ;

        [Display(Name = "Expiry Date")]
        public Nullable<System.DateTime> Expirydate ;

        [Display(Name = "Batch Num")]
        public string BatchNo ;

        [Display(Name = "Created Date time")]
        public string CreatedDatetime ;    
    }
}