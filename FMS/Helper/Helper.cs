using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FMS.Models;

namespace FMS.Helper
{


    public  static class GenericHelper
    {
        private static FMSExpEntities db = new FMSExpEntities();
        public static string GetMaxValue(string TableName)
        {
           var result =   db.tbl_Sequence.Where(q => q.TableName == TableName).Select(q => new { maxValue = q.PreFix + (q.RecordCount + 1) });
            var c = result.Count();
            return result.FirstOrDefault().maxValue;
        }

        public static tbl_Sequence GetNextUpdatedData(string TableName)
        {
            var result = db.tbl_Sequence.Where(q => q.TableName == TableName).FirstOrDefault();
            result.RecordCount += 1;
            return result;
        }

        //public static tbl_ItemStock MapItemStock(ItemStockEntity uIinput)
        //{
        //    tbl_ItemStock res = new tbl_ItemStock
        //            {
        //                AutoID = uIinput.AutoID,
        //                BatchNo = uIinput.BatchNo,
        //                Expirydate = uIinput.Expirydate,
        //                ID = uIinput.ID,
        //                InvoiceNo = uIinput.InvoiceNo,
        //                ItemID = uIinput.ItemID,
        //                ItemwiseDiscount = Convert.ToInt32( uIinput.ItemwiseDiscount),
        //                ManufactureDate =  uIinput.ManufactureDate,
        //                MRP = Convert.ToInt32(uIinput.MRP),
        //                //PricePerUnitAfterTax = Convert.ToInt32(uIinput.PricePerUnitAfterTax),
        //                //PricePerUnitBeforeTax = Convert.ToInt32(uIinput.PricePerUnitBeforeTax),
        //                PurchaseID = uIinput.PurchaseID,
        //                Qty = Convert.ToInt32(uIinput.Qty),
        //                //Tax = Convert.ToInt32(uIinput.Tax),
        //                tbl_Items =  uIinput.tbl_Items,
        //                TotalPriceAfterTax = Convert.ToInt32(uIinput.TotalPriceAfterTax),
        //                TotalPriceBeforeTax = Convert.ToInt32(uIinput.TotalPriceBeforeTax),
        //                //TotalTax = Convert.ToInt32(uIinput.TotalTax)
        //    };

        //    return res;
        //}
    }
}