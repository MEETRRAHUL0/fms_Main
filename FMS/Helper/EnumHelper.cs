using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMS.Helper
{
    public class EnumHelper
    {


        public enum TransactionEntryType { Purchase, Sales, Recipt, Payment }
        public enum SequenceTable
        {
            tbl_Purchase,
            tbl_ItemStock,
            tbl_Supplier,
            tbl_Items,
            tbl_Sale,
            tbl_SalesInvoice,
            tbl_Customer,
            tbl_Payment,
            tbl_TransactionDebit,
            tbl_TransactionCredit,
        }
        public enum TransactionType { Debit, Credit } //--  For Real: Debit what comes in, credit what goes out.
    }


    public static class PaymentMode
    {
        public const int HOLD = 1;
        public const int CREDIT_PURCHASE = 2;
        public const int CASH_PAY = 3;
        public const int MULTI_MODE_PAY = 4;

    }

    public static class TransactionStatus
    {
        public const string Paid = "Paid";
        public const string Unpaid = "Unpaid";
        public const string Pending = "Pending";
        public const string PaidAgainstEntries = "Paid Against Entries :";
        public const string ReciveAgainstEntries = "Recive Against Entries :";

    }


}