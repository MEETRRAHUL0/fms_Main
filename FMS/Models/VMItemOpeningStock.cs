using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMS.Models
{
    public class VMItemOpeningStock
    {
        public tbl_Items tbl_Items { get; set; }
        public tbl_ItemStock tbl_ItemStock { get; set; }
    }
}