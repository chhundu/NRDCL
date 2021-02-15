using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NRDCL.Models.Report
{
    public class Report
    {
        public string CustomerID { get; set; }
        public string ProductName { get; set; }
        public decimal PriceAmount { get; set; }
        public decimal TransportAmount { get; set; }
        public decimal AdvanceBalance { get; set; }
        public int ProductID { get; set; }
    }
}