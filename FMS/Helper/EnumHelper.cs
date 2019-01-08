using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMS.Helper
{
    public class EnumHelper
    {

        public enum TransactionEntryType { Purchase , Sales , Recipt , Payment }
        //enum Status { Paid = "Paid", Unpaid, Pending, paument = "Paid Against entries" , Recive  Against entries }
        //            TransactionType = null,  ///Debit/ credit --  For Real: Debit what comes in, credit what goes out.
    }
}